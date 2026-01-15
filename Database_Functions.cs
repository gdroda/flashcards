using Microsoft.Data.SqlClient;


namespace flashcards
{
    internal class Database_Functions
    {
        static void EstablishConnection(string str, bool msg)
        {
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=flashcardDB;Trusted_Connection=true;");
            SqlCommand command = new(str, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                if (msg)
                {
                    Console.WriteLine("Success!");
                    Console.WriteLine("Press ENTER to return");
                    Console.ReadLine();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database...{ex}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        static List<string> EstablishAndReceive(string str)
        {
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=flashcardDB;Trusted_Connection=true;");
            SqlCommand command = new(str, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<string> stackList = new();
                Console.WriteLine("Name");
                Console.WriteLine("------");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[1]}");
                    stackList.Add(reader[1].ToString() ?? "empty");
                }
                return stackList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database...{ex}");
                return null;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void CreateDefaultTables()
        {
            string stra = "IF OBJECT_ID('Stacks') is null CREATE TABLE Stacks (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(64))";
            EstablishConnection(stra, false);
            string strb = "IF OBJECT_ID('Flashcards') is null CREATE TABLE Flashcards (Id INT PRIMARY KEY IDENTITY, Front VARCHAR(64), Back VARCHAR(64), StackId INT, FOREIGN KEY (StackId) REFERENCES Stacks(Id))";
            EstablishConnection(strb, false);
        }

        public static void AddToTable(string table)
        {
            switch (table)
            {
                case "Stacks":
                    Console.Write("\nName: ");
                    string? stackName = Console.ReadLine();
                    string strS = $@"INSERT INTO {table} (Name) VALUES ('{stackName}')";
                    EstablishConnection(strS, true);
                    User_Input.StacksMenu();
                        break;
                case "Flashcards":
                    Console.Write("\nName: ");
                    string? flashcardName = Console.ReadLine();
                    string strF= $@"INSERT INTO {table} (Name) VALUES ('{flashcardName}')"; //NEED DIFFERENT INPUT VALUES
                    EstablishConnection(strF, true);
                    User_Input.StacksMenu();
                    break;
                default:
                    break;
            }
        }

        public static void RemoveFromTable(string table)
        {
            switch (table)
            {
                case "Stacks":
                    Console.Write("\nName: ");
                    string? stackName = Console.ReadLine();
                    string str = $@"DELETE FROM {table} Where (Name = '{stackName}')";
                    EstablishConnection(str, true);
                    User_Input.StacksMenu();
                    break;
                case "Flashcards":
                    break;
                default:
                    break;
            }
        }

        public static List<string> ShowStacks()
        {
            string str = @"SELECT * FROM Stacks";
            List<string> stacksList = EstablishAndReceive(str);
            return stacksList;
        }
    }
}
