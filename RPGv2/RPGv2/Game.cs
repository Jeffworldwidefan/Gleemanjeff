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
            for (int i = 0; i < Race.RacesAmount(); i++)
            {
                races.Add(new Race(i));
            }
        }
    }
}
