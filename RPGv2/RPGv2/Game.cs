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
                        Console.WriteLine(race.Name);
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
            for(int i = 0; i<=years; i++)
            {
                Event e = new Event();
                Console.ReadKey();
            }
            h.Races = races;
            h.Factions = factions;
            return h;
        }
    }
}
