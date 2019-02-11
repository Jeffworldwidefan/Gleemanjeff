using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace RPGv2
{
    internal class HelperClasses
    {
        private static void Main(string[] args)
        {
            StateManager sm = new StateManager();
            bool done = false;
            while (!done)
            {
                switch (sm.GetState())
                {
                    case "start":
                        if (Start(done))
                            done = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("A state error has occured. State: {0}", sm.GetState());
                        break;
                }
            }
        }
        public static bool Start(bool done)
        {
            Console.Clear();
            int inp = MultipleChoice(false, "Start", "Modify JSON files", "Exit");
            switch (inp)
            {
                case 0:
                    Game.StartGame();
                    return false;
                case 1:
                    string[] jsonFiles = Directory.GetFiles("Dependencies");
                    for (int i = 0; i < jsonFiles.Length; i++)
                        jsonFiles[i] = jsonFiles[i].Remove(0, 13);
                    inp = MultipleChoice(false, jsonFiles);
                    JArray arr = JArray.Parse(File.ReadAllText(string.Format(@"Dependencies\{0}", jsonFiles[inp])));
                    int inpIndex = inp;
                    string path = string.Format(@"Dependencies\{0}", jsonFiles[inp]);
                    inp = MultipleChoice(false, "Add", "Modify");
                    List<string> options = new List<string>();
                    JObject o;
                    string newVal;
                    switch (inp)
                    {
                        case 0:
                            options.Clear();
                            o = JObject.Parse(arr[0].ToString());
                            foreach (JProperty jp in o.Properties())
                                options.Add(jp.Name);
                            for (int i = 0; i < options.Count; i++)
                            {
                                Console.Write("Value of {0}: ", options[i]);
                                newVal = Console.ReadLine();
                                if (o[options[i]].Type == JTokenType.String)
                                    o[options[i]] = newVal;
                                if (o[options[i]].Type == JTokenType.Integer)
                                    o[options[i]] = int.Parse(newVal);
                                if (o[options[i]].Type == JTokenType.Float)
                                    o[options[i]] = float.Parse(newVal);
                            }
                            arr.Add(o);
                            File.WriteAllText(path, arr.ToString());
                            break;
                        case 1:
                            foreach (JObject obj in arr)
                                options.Add((string)obj["Name"]);
                            inp = MultipleChoice(false, options.ToArray());
                            options.Clear();
                            o = JObject.Parse(arr[inp].ToString());
                            foreach (JProperty jp in o.Properties())
                                options.Add(jp.Name);
                            List<string> optionsTemp = new List<string>(options);
                            for (int i = 0; i < optionsTemp.Count; i++)
                                optionsTemp[i] = string.Format("{0} ({1})", optionsTemp[i], o[optionsTemp[i]]);
                            inp = MultipleChoice(false, optionsTemp.ToArray());
                            Console.Write("Enter new value: ");
                            newVal = Console.ReadLine();
                            if (o[options[inp]].Type == JTokenType.String)
                                o[options[inp]] = newVal;
                            if (o[options[inp]].Type == JTokenType.Integer)
                                o[options[inp]] = int.Parse(newVal);
                            if (o[options[inp]].Type == JTokenType.Float)
                                o[options[inp]] = float.Parse(newVal);
                            arr[inpIndex] = o;
                            File.WriteAllText(path, arr.ToString());
                            break;
                        default:
                            break;
                    }
                    return false;
                case 2:
                    return true;
                default:
                    break;
            }
            return false;

        }
        
        public static int MultipleChoice(bool canCancel, params string[] options)
        {
            const int startX = 0;
            const int startY = 0;
            const int optionsPerLine = 1;
            const int spacingPerLine = 14;

            int currentSelection = 0;

            ConsoleKey key;

            Console.CursorVisible = false;
            do
            {
                Console.Clear();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(options[i]);

                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            if (currentSelection % optionsPerLine > 0)
                                currentSelection--;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (currentSelection % optionsPerLine < optionsPerLine - 1)
                                currentSelection++;
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (currentSelection >= optionsPerLine)
                                currentSelection -= optionsPerLine;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (currentSelection + optionsPerLine < options.Length)
                                currentSelection += optionsPerLine;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            if (canCancel)
                                return -1;
                            break;
                        }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;

            Console.Clear();
            return currentSelection;
        }
    }

    internal class StateManager
    {
        private string state = "start";
        public StateManager()
        {

        }
        public string GetState()
        {
            return state;
        }

        public void SetState(string s) { state = s; }
    }

    class Race
    {
        string name;
        int intelligence;
        int baseAtt;
        int baseDef;
        int pop;
        public Race(int index, int p)
        {
            pop = p;
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\race.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            intelligence = (int)obj["Intelligence"];
            baseAtt = (int)obj["BaseAttack"];
            baseDef = (int)obj["BaseDefense"];
        }
        public Race(string n, int p)
        {
            pop = p;
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\race.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    intelligence = (int)obj["Intelligence"];
                    baseAtt = (int)obj["BaseAttack"];
                    baseDef = (int)obj["BaseDefense"];
                }
            }
        }
        public int[] GetVals() => new int[] { name, intelligence, baseAtt, baseDef, pop };
        public static int RacesAmount() => JArray.Parse(File.ReadAllText(@"Dependencies\race.json")).Count;
    }

    class Faction
    {
        string name;
        Race race;
        int pop = 0;
        List<string> otherFactions = new List<string>();
        List<string> advances = new List<string>();
        List<int> favor = new List<int>();

        public int Pop { get => pop; set => pop = value; }

        public Faction(Race r, string n)
        {
            name = n;
            race = r;
        }

        public Faction(Race r)
        {
            string[] factionNames = File.ReadAllLines(@"Dependencies\FactionNames.txt");
            name = factionNames[new Random().Next(factionNames.Length)];
            race = r;
        }

        public void AddPop(int add) { Pop+=add }
    }

    internal class Player
    {
        private string name;
        private string cla;
        private string race;
        private string faction;
        private int att;
        private int matk;
        private int def;
        private int mdef;
        private int intel;
        private double money;
        private double luck;
        private double eva;
        public Player(int slot)
        {
            JArray saves = JArray.Parse(File.ReadAllText(@"Dependencies\player.json"));
            JObject save = JObject.Parse(saves[slot - 1].ToString());
            if (string.IsNullOrEmpty(save["Name"].ToString()))
            {
                CreateCharacter(slot, saves);
            }
        }
        public void CreateCharacter(int slot, JArray arr)
        {
            JObject save = JObject.Parse(arr[slot - 1].ToString());
            Console.Write("Enter Name: ");
            name = Console.ReadLine();
            save["Name"] = name;
            int inp = HelperClasses.MultipleChoice(false, "Mage", "Warrior", "Rogue");
            switch (inp)
            {
                case 1:
                    cla = "Mage";
                    att = 2;
                    matk = 7;
                    def = 1;
                    mdef = 5;
                    intel = 9;
                    money = 0;
                    luck = 4;
                    eva = 3;
                    break;
                case 2:
                    cla = "Warrior";
                    att = 9;
                    matk = 2;
                    def = 5;
                    mdef = 3;
                    intel = 4;
                    money = 0;
                    luck = 2;
                    eva = 2;
                    break;
                case 3:
                    cla = "Rogue";
                    att = 6;
                    matk = 4;
                    def = 2;
                    mdef = 3;
                    intel = 6;
                    money = 0;
                    luck = 8;
                    eva = 8;
                    break;
                default:
                    break;
            }
            save["Class"] = cla;
            save["Attack"] = att;
            save["Defense"] = def;
            save["Magic Attack"] = matk;
            save["Magic Defense"] = mdef;
            save["Intelligence"] = intel;
            save["Money"] = money;
            save["Luck"] = luck;
            save["Evasion"] = eva;
            Console.WriteLine((string)save["Name"]);
            arr[slot - 1] = JArray.Parse(save.ToString());
            File.WriteAllText(@"Dependencies\player.json", arr.ToString());
        }
    }

    internal class Sword
    {
        private readonly string name;
        private readonly int att;
        private readonly int def;
        private readonly int buyPrice;
        private readonly int sellPrice;
        private readonly int rarity;
        public Sword(int index)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            att = (int)obj["Attack"];
            def = (int)obj["Defense"];
            buyPrice = (int)obj["Buy Price"];
            sellPrice = (int)obj["Sell Price"];
            rarity = (int)obj["Rarity Level"];
        }
        public Sword(string n)
        {
            JArray array = JArray.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    att = (int)obj["Attack"];
                    def = (int)obj["Defense"];
                    buyPrice = (int)obj["Buy Price"];
                    sellPrice = (int)obj["Sell Price"];
                    rarity = (int)obj["Rarity Level"];
                }
            }
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Unable to find sword: " + n);
        }

        public override string ToString()
        {
            string output = "";
            string.Format(output, "Name: {0}\nAttack: {1}\nDefense: {2}\nBuy Price:{3}\n Sell Price: {4}\nRarity Level: {5}", name, att, def, buyPrice, sellPrice, rarity);
            return output;
        }

        public string GetName()
        {
            return name;
        }

        public int[] GetVals()
        {
            return new int[] { att, def, buyPrice, sellPrice };
        }
    }
}
