using System;
using System.Collections.Generic;
using System.IO;

namespace ExpenditureDB
{

    public class Entry
    {
        private String info;
        private double amount;
        private DateTime date;

        public Entry(double amount, String info)
        {
            this.amount = amount;
            this.info = info;
            this.date = DateTime.Now;
        }

        public Entry(String entryString)
        {
            String[] comp = entryString.Split("|");
            this.date = DateTime.Parse(comp[0]);
            this.amount = Double.Parse(comp[1]);
            this.info = comp[2];
        }

        public override string ToString() => GetDate().ToString() + "|" + GetAmount().ToString() + "|" + GetInfo() ;

        public String GetInfo() => info;
        public double GetAmount() => amount;
        public DateTime GetDate() => date;
    }


    public class DataBase
    {
        private double balance;

        private List<Entry> entries;

        private readonly int pad = 20;

        public DataBase()
        {
            entries = new List<Entry>();
        }

        public void NewEntry(double amount, String info)
        {
            entries.Add(new Entry(amount, info));
            balance += amount;
        }

        public void NewEntry(String entryString)
        {
            Entry fromStr = new Entry(entryString);
            entries.Add(fromStr);
            balance += fromStr.GetAmount();
        }

        public List<Entry> GetEntriesList() => entries;

        public String GetEntries(DateTime date)
        {
            String output = ShowTableHead();

            foreach (var entry in entries)
            {
                if (entry.GetDate() == date.Date)
                {
                    output += entry.GetDate() + "|" + entry.GetInfo() + "|" + entry.GetAmount() +"\n";
                }
            }

            output += ShowTableEnd();
            return output;
        }

        public String GetAllEntries()
        {
            String output = ShowTableHead();

            foreach (var entry in entries)
            {
                    output += StrC(entries.IndexOf(entry).ToString(), 3) + "|" + StrC(entry.GetDate().ToString(), pad) + "|" + StrC(entry.GetInfo(), pad) + "|" + StrC(entry.GetAmount().ToString(), pad) + "|\n";
            }
            output += ShowTableEnd();
            return output;
        }

        public String GetEntries(int d, int m, int a)
        {
            String output = ShowTableHead();

            foreach (var entry in entries)
            {
                if (entry .GetDate().Day == d && entry.GetDate().Month == m && entry.GetDate().Year == a)
                {
                    output += StrC(entries.IndexOf(entry).ToString(), 3) + entries.IndexOf(entry) + "|" + StrC(entry.GetDate().ToString(), pad) + "|" + StrC(entry.GetInfo(), pad) + "|" + StrC(entry.GetAmount().ToString(), pad) + "|\n";
                }
            }
            output += ShowTableEnd();
            return output;
        }
        public String GetEntries(int m, int a)
        {
            String output = ShowTableHead();

            foreach (var entry in entries)
            {
                if (entry.GetDate().Month == m && entry.GetDate().Year == a)
                {
                    output += StrC(entries.IndexOf(entry).ToString(), 3) + entries.IndexOf(entry) + "|" + StrC(entry.GetDate().ToString(), pad) + "|" + StrC(entry.GetInfo(), pad) + "|" + StrC(entry.GetAmount().ToString(), pad) + "|\n";
                }
            }
            output += ShowTableEnd();
            return output;
        }

        public String GetEntries(int a)
        {
            String output = ShowTableHead();

            foreach (var entry in entries)
            {
                if (entry.GetDate().Year == a)
                {
                    output += StrC(entries.IndexOf(entry).ToString(), 3) + "|" + StrC(entry.GetDate().ToString(), pad) + "|" + StrC(entry.GetInfo(), pad) + "|" + StrC(entry.GetAmount().ToString(), pad) + "|\n";
                }
            }
            output += ShowTableEnd();
            return output;
        }

        public String ShowTableHead()
        {
            String output = StrC("", 3) + "|" + StrC("Date", pad) + "|" + StrC("Info", pad) + "|" + StrC("Transaction", pad) + "|\n";
            output += StrC("", 3) + new string('-', pad * 3 + 4);
            output += "\n";
            return output;
        }

        public String ShowTableEnd()
        {
            String output = StrC("", 3) + new string('-', pad * 3 + 4);
            output += "\nBalance: " + balance;
            return output;
        }

        public DateTime GetDate() => DateTime.Now;

        private double GetBalance() => balance;

        public void ClearDB()
        {
            entries.Clear();
            balance = 0;
        }

        public string StrC(string s, int desiredLength)
        {
            if (s.Length >= desiredLength) return s;
            int firstpad = (s.Length + desiredLength) / 2;
            return s.PadLeft(firstpad).PadRight(desiredLength);
        }

        public void DeleteAt(int i)
        {
            balance -= entries[i].GetAmount();
            entries.RemoveAt(i);
        }
    }
    
    
    class Program
    {
        public static DataBase db = new DataBase();
        private static string fileName = "entries.txt";

        private static void NewEntry()
        {
            String input = "";
            String am = "";
            String info = "";
            while (true)
            {
                Console.Write("Input '+' or '-':");
                input = Console.ReadLine();

                if (input == "") break;

                if (input == "+")
                {
                    while (true)
                    {
                        Console.Write("Input [amount] [info] >>> ");
                        input = Console.ReadLine();
                        if (input == "") break;
                        try
                        {
                            if (!input.Contains(" ")) am = input;
                            else
                            {
                                am = input.Substring(0, input.IndexOf(" "));
                                info = input.Substring(input.IndexOf(" ") + 1);
                            }

                            db.NewEntry(Math.Abs(Double.Parse(am)), info);
                        }
                        catch { Console.WriteLine("Incorrect amount" + am); }
                    }
                    continue;
                }

                if (input == "-")
                {
                    while (true)
                    {
                        Console.Write("Input [amount] [info] >>> ");
                        input = Console.ReadLine();
                        if (input == "") break;
                        try
                        {
                            if (!input.Contains(" ")) am = input;
                            else
                            {
                                am = input.Substring(0, input.IndexOf(" "));
                                info = input.Substring(input.IndexOf(" ") + 1);
                            }
                            
                            db.NewEntry(-Math.Abs(Double.Parse(am)), info);
                        }
                        catch { Console.WriteLine("Incorrect amount" + am); }
                    }
                    continue;
                }
                Console.WriteLine(input);
                continue;
            }
        }

        private static void Exit()
        {
            Save();
            Environment.Exit(0);
        }

        private static void ShowEntries()
        {
            String input;
            String output = "";
            bool con = true;
            while (con)
            {
                Console.Write("Input date [dd/mm/aa] or [all]: ");
                input = Console.ReadLine();

                if (input == "all")
                {
                    output = db.GetAllEntries();
                    con = false;
                    continue;
                }

                String[] s = input.Split('.', '-', '|', '/');

                con = false;

                switch (s.Length)
                {
                    case 1:
                        try { int a = int.Parse(s[0]); output = db.GetEntries(a); }
                        catch { Console.WriteLine("Incorrect date " + input); con = true; }
                        break;
                    case 2:
                        try { int m = int.Parse(s[0]); int a = int.Parse(s[1]); output = db.GetEntries(m, a); }
                        catch { Console.WriteLine("Incorrect date " + input); con = true; }
                        break;
                    case 3:
                        try { int d = int.Parse(s[0]); int m = int.Parse(s[1]); int a = int.Parse(s[2]); output = db.GetEntries(d, m, a); }
                        catch { Console.WriteLine("Incorrect date " + input); con = true; }
                        break;
                }
            }

            Console.WriteLine(output);
        }

        public static void Help()
        {
            Console.WriteLine("Commands: 'new' | 'show' | 'delete' | 'clear' | 'exit'");
        }

        public static void ClearAll()
        {
            while (true)
            {
                Console.WriteLine("Are you sure yo want to delete all entries? [y/n]");
                String yn = Console.ReadLine();
                if (yn == "y")
                {
                    db.ClearDB();
                    Console.WriteLine("All entries have been deleted");
                    return;
                }
                if (yn == "n")
                {
                    Console.WriteLine("Nothing deleted!");
                    return;
                }
                else Console.WriteLine("Invalid command!");
            }
            
        }


        public static void Delete()
        {
            Console.Write("Which entries would you like to delete? > ");
            string entryIndexes = Console.ReadLine();
            if (entryIndexes == "") return;
            DeleteEntries(entryIndexes);
        }

        public static void DeleteEntries(String entryIndexes)
        {
            String[] ind = entryIndexes.Split(' ', ',', '|', '/');
            int[] indexes = ParseStringArrayToInt(ind);
            if (indexes == null)
            {
                Console.WriteLine("The given indexes were not integers!");
                return;
            }
            int[] sortedInd = DistictSorted(indexes);
            foreach (int i in sortedInd)
            {
                db.DeleteAt(i);
            }
            Console.WriteLine("Entries were deleted succesfully!");
        }

        public static int[] ParseStringArrayToInt(String[] indexes)
        {
            int[] ind = new int[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                try
                {
                    ind[i] = int.Parse(indexes[i]);
                }
                catch
                {
                    return null;
                }
            }
            return ind;
        }

        public static int[] DistictSorted(int[] ar)
        {
            Array.Sort(ar);
            Array.Reverse(ar);
            List<int> dist = new List<int>();
            for (int i = 0; i < ar.Length; i++)
            {
                if (i != 0 && ar[i] != ar[i - 1])
                {
                    dist.Add(ar[i]);
                }
            }
            return dist.ToArray();
        }

        public static void Run()
        {
            String input;

            Read();
            while (true)
            {
                Console.Write("Action: ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "new":
                        NewEntry();
                        break;

                    case "exit":
                        Exit();
                        break;

                    case "show":
                        ShowEntries();
                        break;

                    case "delete":
                        Delete();
                        break;

                    case "clear":
                        ClearAll();
                        break;

                    case "help":
                        Help();
                        break;

                    default:
                        Console.WriteLine("Incorrect action " + input);
                        break;

                }
            }
        }

        public static void Save()
        {
            List<Entry> entries = db.GetEntriesList();
            ClearFile();
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Entry entry in entries)
                { 
                    writer.WriteLine(entry.ToString());
                }
            }
            Console.WriteLine("Saved to file " + fileName);
        }

        public static void Read()
        {
            IEnumerable<string> filetext;
            try { filetext = File.ReadLines(fileName); }
            catch { 
                Console.WriteLine("File not found!");
                Console.WriteLine("Created new database");
                return;
            }
            foreach (string line in filetext)
            { 
                db.NewEntry(line);
            }
        }

        public static void ClearFile()
        {
            File.WriteAllText(fileName, String.Empty);
        }



        static void Main(string[] args)
        {
            Run();
        }

    }
}


// Implement to be able to do show all or new + 200 "newEntry"