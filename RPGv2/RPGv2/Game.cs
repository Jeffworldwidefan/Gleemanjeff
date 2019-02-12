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
                    if (num < 70)
                    {
                        factions[mainCityInd].Pop++;
                        num = 101;
                    }
                    if (num < 95)
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
            for (int i = 0; i<=years; i++)
            {
                for(int j = 0; j < factions.Count; j++)
                {
                    Faction f = factions[j];
                    Event e = new Event();
                    int chainAmount = 0;
                    do
                    {
                        switch (e.Name)
                        {
                            case "None":
                                break;
                            case "Chain Event":
                                chainAmount += 3;
                                break;
                            case "Famine":
                                int deathChance = new Random().Next(100);
                                Random rando = new Random();
                                for(int k = 0; k<=f.Pop; k++)
                                {
                                    int numChance = rando.Next(100);
                                    if(numChance < deathChance)
                                    {
                                        f.Pop--;
                                    }
                                }
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
                            default:
                                Console.Clear();
                                Console.WriteLine("An unknown event has occured, name: {0}", e.Name);
                                Console.ReadKey();
                                break;
                        }
                        chainAmount--;
                    } while (chainAmount > 0);
                }
            }
            h.Races = races;
            h.Factions = factions;
            return h;
        }
    }
}
