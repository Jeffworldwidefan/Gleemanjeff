using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPGv2
{
    class Program
    {
        static void Main(string[] args)
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
                Console.ReadKey();
            }
        }
        public static bool Start(bool done)
        {
            Console.Clear();
            Console.WriteLine("1. Start");
            Console.WriteLine("2. Options");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Exit");
            int inp;
            if (!int.TryParse(Console.ReadLine(), out inp))
            {
                Console.Clear();
                Console.WriteLine("Unknown input, please try again.");
                Console.ReadKey();
                Start(done);
            }
            switch (inp)
            {
                case 1:
                    return false;
                case 2:

                    return false;
                case 3:
                    Console.Clear();
                    Console.WriteLine("What would you like to add/modify to?");
                    Console.WriteLine("1. Sword.json");
                    Console.WriteLine("2. Player.json");
                    if (!int.TryParse(Console.ReadLine(), out inp))
                    {
                        Console.Clear();
                        Console.WriteLine("Unknown input, please try again.");
                        Console.ReadKey();
                        Start(done);
                    }
                    switch (inp)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Would you like to add or modify?");
                            Console.WriteLine("1. Add");
                            Console.WriteLine("2. Modify");
                            if (!int.TryParse(Console.ReadLine(), out inp))
                            {
                                Console.Clear();
                                Console.WriteLine("Unknown input, please try again.");
                                Console.ReadKey();
                                Start(done);
                            }
                            JObject o = JObject.Parse(File.ReadAllText(@"Dependencies\sword.json"));
                            JArray array = JArray.Parse(o["Swords"].ToString());
                            List<string> names = new List<string>();
                            foreach(JObject obj in array)
                            {
                                names.Add((string)obj["Name"]);
                            }
                            switch (inp)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine("Enter sword name: ");
                                    string input = Console.ReadLine();
                                    if (names.Contains(input))
                                    {
                                        Console.WriteLine("That sword already exists!");
                                        goto case 1;
                                    }
                                    List<int> datas = new List<int>();
                                    Console.Write("Enter attack: ");
                                    if (!int.TryParse(Console.ReadLine(), out inp))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unknown input, please try again.");
                                        Console.ReadKey();
                                        goto case 1;
                                    }
                                    datas.Add(inp);
                                    Console.Write("Enter defense: ");
                                    if (!int.TryParse(Console.ReadLine(), out inp))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unknown input, please try again.");
                                        Console.ReadKey();
                                        goto case 1;
                                    }
                                    datas.Add(inp);
                                    Console.Write("Enter buy price: ");
                                    if (!int.TryParse(Console.ReadLine(), out inp))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unknown input, please try again.");
                                        Console.ReadKey();
                                        goto case 1;
                                    }
                                    datas.Add(inp);
                                    Console.Write("Enter sell price: ");
                                    if (!int.TryParse(Console.ReadLine(), out inp))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Unknown input, please try again.");
                                        Console.ReadKey();
                                        goto case 1;
                                    }
                                    datas.Add(inp);
                                    JObject newSword = new JObject();
                                    newSword.Add("Name", input);
                                    newSword.Add("Attack", datas[0]);
                                    newSword.Add("Defense", datas[1]);
                                    newSword.Add("Buy Price", datas[2]);
                                    newSword.Add("Sell Price", datas[3]);
                                    array.Add(newSword);
                                    o["Swords"] = array;
                                    File.WriteAllText(@"Dependencies\sword.json", o.ToString());
                                    return false;
                                case 2:

                                default:
                                    Console.WriteLine("Unknown input please try again.");
                                    Console.ReadKey();
                                    Start(done);
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("Unknown input please try again.");
                            Console.ReadKey();
                            Start(done);
                            break;
                    }
                    return false;
                case 4:
                    return true;
                default:
                    Console.WriteLine("Unknown input please try again.");
                    Console.ReadKey();
                    Start(done);
                    break;
            }
            return false;

        }
        public void Serialize(string jsonFile, JObject j)
        {
            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(j));
        }
    }
    class StateManager
    {
        private string state = "start";
        public StateManager()
        {

        }
        public string GetState() => state;
        public void SetState(string s) { state = s; }
    }
    class Player
    {
        private string name;
        private string cla;
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
            JObject o = JObject.Parse(File.ReadAllText(@"Dependencies\player.json"));
            JArray saves = JArray.Parse(o["Saves"].ToString());
            JObject save = JObject.Parse(saves[slot - 1].ToString());
            if (string.IsNullOrEmpty(save["Name"].ToString()))
            {
                CreateCharacter(slot, o);
            }
        }
        public void CreateCharacter(int slot, JObject o)
        {
            JArray saves = JArray.Parse(o["Saves"].ToString());
            JObject save = JObject.Parse(saves[slot - 1].ToString());
            Console.Write("Enter Name: ");
            name = Console.ReadLine();
            save["Name"] = name;
            Console.WriteLine("Enter Class: ");
            Console.WriteLine("1. Mage");
            Console.WriteLine("2. Warrior");
            Console.WriteLine("3. Rogue");
            Console.Write("Input: ");
            int inp;
            if (!int.TryParse(Console.ReadLine(), out inp))
            {
                Console.Clear();
                Console.WriteLine("Unknown input, please try again.");
                Console.ReadKey();
                CreateCharacter(slot, o);
            }
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
                    Console.WriteLine("Could not find that input.");
                    Console.ReadKey();
                    Console.Clear();
                    CreateCharacter(slot, o);
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
            o["Saves"][slot - 1] = JObject.Parse(save.ToString());
            File.WriteAllText(@"Dependencies\player.json", o.ToString());
        }
    }
    class Sword
    {
        private string name;
        private int att;
        private int def;
        private int buyPrice;
        private int sellPrice;
        public Sword(int index)
        {
            JObject o = JObject.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            JArray array = JArray.Parse(o["Swords"].ToString());
            JObject obj = JObject.Parse(array[index].ToString());
            name = (string)obj["Name"];
            att = (int)obj["Attack"];
            def = (int)obj["Defense"];
            buyPrice = (int)obj["Buy Price"];
            sellPrice = (int)obj["Sell Price"];
        }
        public Sword(string n)
        {
            JObject o = JObject.Parse(File.ReadAllText(@"Dependencies\sword.json"));
            JArray array = JArray.Parse(o["Swords"].ToString());
            foreach (JObject obj in array)
            {
                if (obj["Name"].ToString() == n)
                {
                    name = (string)obj["Name"];
                    att = (int)obj["Attack"];
                    def = (int)obj["Defense"];
                    buyPrice = (int)obj["Buy Price"];
                    sellPrice = (int)obj["Sell Price"];
                }
            }
            if (String.IsNullOrEmpty(name))
                throw new InvalidOperationException("Unable to find sword: " + n);
        }

        public string GetName() => name;
        public int[] GetVals() => new int[] { att, def, buyPrice, sellPrice };
    }
}
