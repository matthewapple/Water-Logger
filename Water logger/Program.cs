using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;

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
                Console.WriteLine("MAIN MENU\n");
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
                        Environment.Exit(0);
                        break;
                    
                    case "1":
                        Insert();
                        break;

                    case "2":
                        Update();
                        break;

                    case "3":
                        GetAllRecords();
                        break;

                    case "4":
                        Delete();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a number 0 - 4");
                        break;
                }
                
            }


        }
        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = @"SELECT * FROM drinking_water";

                List<drinkingwater> tableData = new List<drinkingwater>();

                var reader = tableCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new drinkingwater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)

                        }); ;

                    }

                }
                else
                    Console.WriteLine("No data found");

                connection.Close();

                Console.WriteLine("\n--------------------");

                foreach (var i in tableData)
                {
                    Console.WriteLine($"{i.Id} -- {i.Date.ToString("MM-dd-yy")} -- Quantity: {i.Quantity}");
                }

                Console.WriteLine("\n--------------------");

            }
        }
        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("Please enter quantity of water consumed (no decimals allowed). Press 0 to return to Main Menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}',{quantity})";
                
                tableCommand.ExecuteNonQuery();

                connection.Close();
            }

        }
        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var inputId = GetNumberInput("Enter ID of record you would like to delete. Type 0 to return to main menu.");

            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $"DELETE FROM drinking_water WHERE Id = {inputId}";

                int rowCount = tableCommand.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine("Record does not exist. Press any key to try again.");
                    Console.ReadKey();
                    Delete();
                }
                    
                else
                {
                    Console.WriteLine("Record was deleted. Press any key to continue");
                    Console.ReadKey();
                }

                connection.Close();

            }
            GetUserInput();
        }
        private static void Update()
        {
            Console.Clear();

            GetAllRecords();

            var inputId = GetNumberInput("Enter ID of record you would like to update. Type 0 to return to main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();

                string date = GetDateInput();

                int quantity = GetNumberInput("Enter updated quantity (no decimals allowed). Type 0 to return to main menu.");

                tableCommand.CommandText = $"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity} WHERE Id = {inputId}";

                int rowCount = tableCommand.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine("Record not exist. Please try again.");
                }
                else
                {
                    Console.WriteLine($"Record {inputId} was updated with date as {date} and quantity as {quantity}");
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                GetUserInput();
            }


            return Convert.ToInt32(input);

        }
        internal static string GetDateInput()
        {
            Console.WriteLine("Please enter a date in MM-DD-YY format. Press 0 to return to Main Menu.");

            string input = Console.ReadLine();

            if (input == "0")
                GetUserInput();

            return input;
        }
    }
    public class drinkingwater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
