using System;
using System.Collections.Generic;

namespace RPGv2
{
    internal class Game
    {
        public static void StartGame()
        {
            Console.Write("Enter years of history: ");
            StartHistory(int.Parse(Console.ReadLine()));
        }
        public static void StartHistory(int years)
        {
            List<Race> races = new List<Race>();
            List<Faction> factions = new List<Faction>();
            for (int i = 0; i < Race.RacesAmount(); i++)
            {
                races.Add(new Race(i, new Random().Next(2000)));
            }
            Random rand = new Random();
            for (int i = 0; i < races.Count; i++)
            {
                Race race = races[i];
                int[] vals = race.GetVals();
                factions.Add(new Faction(races[i], "Main City: " + vals[0]));
                int mainCityInd = factions.Count - 1;
                for(int j = 0; j<=vals[4]; j++)
                {
                    int num = rand.Next(100);
                    if (num < 70)
                    {
                        factions[mainCityInd].Pop++;
                        num = 101;
                    }
                    if(num < 95)
                    {
                        bool done = false;
                        Random rand2 = new Random();
                        while(!done)
                        {
                            
                        }
                    }

                    factions.Add(new Faction(race));

                    factions[factions.Count - 1].Pop++;
                }
            }
        }
    }
}
