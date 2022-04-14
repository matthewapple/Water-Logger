using System;
using Microsoft.Data.Sqlite;

namespace Water_logger
{
    internal class Program
    {
        static string connectionString = @"Data Source = WaterLogger.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water 
                    (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER)";
                
                tableCommand.ExecuteNonQuery();
                
                connection.Close();
            }

            GetUserInput();
        }
         static void GetUserInput()
        {
            Console.Clear();
           
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("Please select an option");
                Console.WriteLine("\n----------------------");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to Insert an Entry");
                Console.WriteLine("Type 2 to Update an Entry");
                Console.WriteLine("Type 3 to View all Entries");
                Console.WriteLine("Type 4 to Delete an Entry");

                string input = Console.ReadLine();

                switch(input)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        break;
                    
                    case "1":
                        Insert();
                        break;

                    case "2":
                        GetAllRecords();
                        break;
                }
                Environment.Exit(0);
            }


        }
        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("Please enter quantity of water consumed. Press 0 to return to Main Menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}',{quantity})";
                
                tableCommand.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine($"Data successfully added with {date} as date and {quantity} as quantity. Press any key to continue.");

            Console.ReadKey();

            GetUserInput();

        }
        private static string GetDateInput()
        {
            Console.WriteLine("Please enter a date in MM-DD-YY format. Press 0 to return to Main Menu.");
           
            string input = Console.ReadLine();

            if (input == "0")
                GetUserInput();

            return input;
        }
        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            if (input == "0")
                GetUserInput();

            return Convert.ToInt32(input);

        }
        private static void GetAllRecords()
        {
            Console.Clear();
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = @"SELECT * FROM drinking_water";
            }
        }

    }

    public class drinkingwater
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int quantity { get; set; }
    }
}
