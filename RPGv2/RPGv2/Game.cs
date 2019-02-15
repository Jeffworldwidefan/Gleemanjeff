using System;
using System.Collections.Generic;

namespace RPGv2
{
    internal class Game
    {
        public static void StartGame()
        {
            Console.Write("Enter years of history: ");
            History h = StartHistory(int.Parse(Console.ReadLine()));
            foreach (Faction f in h.Factions)
                Console.WriteLine(f.ToString());
            bool done = false;
            while(!done)
            {
                Console.Write(">");
                string[] inp = Console.ReadLine().Split(' ');
                switch(inp[0])
                {
                    case "get":
                        foreach (Faction fac in h.Factions)
                            if (fac.Name == inp[1])
                            {
                                Console.WriteLine(fac.ToString());
                            }
                        break;
                    default:
                        done = true;
                        break;
                }
            }
            Console.ReadKey();
        }
        public static History StartHistory(int years)
        {

            History h = new History();
            List<Race> races = new List<Race>();
            List<Faction> factions = new List<Faction>();
            for (int i = 0; i < Race.RacesAmount(); i++)
            {
                races.Add(new Race(i, new Random().Next(20000)));
            }
            Random rand = new Random();
            for (int i = 0; i < races.Count; i++)
            {
                Race race = races[i];
                int[] vals = race.GetVals();
                factions.Add(new Faction(races[i], "Main City: " + race.Name));
                int mainCityInd = factions.Count - 1;
                for (int j = 0; j <= vals[3]; j++)
                {
                    int num = rand.Next(100);
                    if (num < 60)
                    {
                        factions[mainCityInd].Pop++;
                        num = 101;
                    }
                    if (num < 80)
                    {
                        bool done = false;
                        Random rand2 = new Random();
                        while (!done)
                        {
                            int num2 = rand2.Next(factions.Count);
                            Faction f = factions[num2];
                            if (f.Race == race)
                            {
                                done = true;
                                factions[num2].Pop++;
                            }
                        }
                        num = 101;
                    }
                    if (num < 100)
                    {
                        bool exists = false;
                        Faction f = new Faction(race);
                        for (int k = 0; k < factions.Count; k++)
                        {
                            if (factions[k].Name == f.Name)
                            {
                                exists = true;
                                factions[k].Pop++;
                            }
                        }
                        if (!exists)
                        {
                            factions.Add(new Faction(race));
                            factions[factions.Count - 1].Pop++;
                        }
                    }
                }
            }
            int totalPeople = 0;
            foreach (Faction f in factions)
                totalPeople += f.Pop;
            double averagePopSeverity = 0;
            for (int i = 0; i <= years; i++)
            {
                foreach(Faction f in factions)
                {
                    f.PopSeverity = (double)f.Pop / totalPeople;
                    averagePopSeverity += f.PopSeverity;
                }
                averagePopSeverity /= factions.Count;
                #region events
                for (int j = 0; j < factions.Count; j++)
                {
                    int chainAmount = 0;
                    do
                    {
                        Faction f = factions[j];
                        Event e = new Event();
                        switch (e.Name)
                        {
                            #region none
                            case "None":
                                break;
                            #endregion
                            #region chain event
                            case "Chain Event":
                                chainAmount += 3;
                                break;
                            #endregion
                            #region famine
                            case "Famine":
                                int deathChance = new Random().Next(70);
                                Random rando = new Random();
                                f.Pop -= Convert.ToInt32(f.Pop * (deathChance / 100.0));
                                if (deathChance < 10)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("slight famine", i));
                                    break;
                                }
                                if (deathChance < 30)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("mild famine", i));
                                    break;
                                }
                                if (deathChance < 70)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("severe famine", i));
                                    break;
                                }
                                if (deathChance < 100)
                                {
                                    f.HistoricalEvents.Add(new HistoricalEvent("extreme famine", i));
                                    break;
                                }
                                break;
                            #endregion
                            #region popup
                            case "Population Up":
                                int percentUp = new Random().Next(1, 20);
                                f.Pop += Convert.ToInt32(f.Pop * (percentUp / 100.0));
                                break;
                            #endregion
                            #region popdown
                            case "Population Down":
                                int percentDown = new Random().Next(1, 10);
                                f.Pop += Convert.ToInt32(f.Pop * (percentDown / 100.0));
                                break;
                            #endregion
                            case "War Declaration":
                                bool canFind = false;
                                for(int k = 0; k<factions.Count; k++)
                                {
                                    if (factions[k].Race != f.Race && factions[k] != f && f.Pop > factions[k].Pop / 2 && f.Pop < factions[k].Pop * 2)
                                    {
                                        canFind = true;
                                    }
                                }
                                if (!canFind)
                                    break;
                                bool doneFinding = false;
                                Faction opp = new Faction(new Race(0, 0));
                                while (!doneFinding)
                                {
                                    int num = rand.Next(factions.Count);
                                    opp = factions[num];
                                    if (opp.Race != f.Race && opp != f && f.Pop > opp.Pop / 2 && f.Pop < opp.Pop * 2)
                                        doneFinding = true;
                                }
                                f.Wars.Add(new War(i, f, opp));
                                break;
                            case "Discovery":
                                break;
                            #region default
                            default:
                                Console.Clear();
                                Console.WriteLine("An unknown event has occured, name: {0}", e.Name);
                                Console.ReadKey();
                                break;
                                #endregion
                        }
                        chainAmount--;
                    } while (chainAmount > 0);
                }
                #endregion
                #region warhandling
                for (int j = 0; j < factions.Count; j++)
                {
                    Faction f = factions[j];
                    bool inWar = false;
                    List<War> wars = new List<War>();
                    foreach (War w in f.Wars)
                        if (w.OnGoing)
                        {
                            inWar = true;
                            wars.Add(w);
                        }
                    if (inWar)
                    {
                        for (int k = 0; k < wars.Count; k++)
                        {
                            Faction warWith = wars[k].Side2;
                            WarEvent we = new WarEvent();
                            switch(we.Name)
                            {
                                case "None":
                                    wars[k].Length++;
                                    break;
                                case "Attack":
                                    for(int l = 0; l<f.Pop+ warWith.Pop; l++)
                                    {
                                        int num1 = HelperClasses.RandomNumber(0, f.Race.GetVals()[2] + (f.Race.GetVals()[1] / 2) + f.Race.GetVals()[0]);
                                        int num2 = HelperClasses.RandomNumber(0, warWith.Race.GetVals()[1] + (warWith.Race.GetVals()[2] / 2) + warWith.Race.GetVals()[0]);
                                        if (num1 >= num2)
                                            warWith.Pop--;
                                        else
                                            f.Pop--;
                                    }
                                    wars[k].Length++;
                                    break;
                                case "Defend":
                                    for (int l = 0; l < f.Pop + warWith.Pop; l++)
                                    {
                                        int num1 = HelperClasses.RandomNumber(0, warWith.Race.GetVals()[1] + (warWith.Race.GetVals()[2]/2) + warWith.Race.GetVals()[0]);
                                        int num2 = HelperClasses.RandomNumber(0, f.Race.GetVals()[2] + (f.Race.GetVals()[1] / 2) + f.Race.GetVals()[0]);
                                        if (num1 >= num2)
                                            f.Pop--;
                                        else
                                            warWith.Pop--;
                                    }
                                    wars[k].Length++;
                                    break;
                                case "End War":
                                    wars[k].OnGoing = false;
                                    f.HistoricalEvents.Add(new HistoricalEvent(String.Format("At war with {0} for {1} years",wars[k].Side2,wars[k].Length), i));
                                    break;
                                default:
                                    Console.Clear();
                                    Console.WriteLine("An unknown event has occured, name: {0}", we.Name);
                                    Console.ReadKey();
                                    break;
                            }
                        }
                    }
                }
                for (int i1 = 0; i1 < factions.Count; i1++)
                {
                    Faction f = factions[i1];
                    if (f.Pop <= 0)
                    {
                        factions.Remove(f);
                        Console.WriteLine("{0} has fallen!", f.Name);
                    }
                }
                #endregion
            }
            h.Races = races;
            h.Factions = factions;
            return h;
        }
    }
}
