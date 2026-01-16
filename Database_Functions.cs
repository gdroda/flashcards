using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;


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
                    Console.WriteLine("\nPress ENTER to return");
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

        static List<Stacks>? EstablishAndReceive(string str)
        {
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=flashcardDB;Trusted_Connection=true;");
            SqlCommand command = new(str, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<Stacks> stacksList = new();
                Console.WriteLine("Name");
                Console.WriteLine("------");
                while (reader.Read())
                {
                    string? name = reader.IsDBNull(1) ? null : reader.GetString(1);
                    Console.WriteLine($"{name}");
                    stacksList.Add(new Stacks(reader.GetInt32(0), name));
                }
                return stacksList;
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

        public static void AddToTable(string table, Stacks? stackInp)
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
                    if (stackInp != null)
                    {
                        Console.Write("\nFront value: ");
                        string? flashcardFront = Console.ReadLine();
                        Console.Write("\nBack value: ");
                        string? flashcardBack = Console.ReadLine();
                        string strF = $@"INSERT INTO {table} (Front, Back, StackId) VALUES ('{flashcardFront}','{flashcardBack}',{stackInp.Id})";
                        EstablishConnection(strF, true);
                        User_Input.StacksMenu();
                    }
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

        public static List<Stacks>? ShowStacks()
        {
            string str = @"SELECT * FROM Stacks";
            List<Stacks>? stacksList = EstablishAndReceive(str);
            return stacksList;
        }

        public static void ShowFlashcard(Stacks stack)
        {
            string str = $@"SELECT * FROM Flashcards WHERE StackId = {stack.Id}";
            EstablishConnection(str, true);
        }
    }

    class Stacks
    {
        private int _iD;
        private string? _name;

        public Stacks(int id, string? name)
        {
            _iD = id;
            _name = name;
        }

        public int Id { get => _iD; }
        public string? Name { get => _name; }
    }

    class Flashcard
    {
        private int _iD;
        private string? _front;
        private string? _back;
        private int _stackId;

        public Flashcard(int id, string? front, string? back, int sid)
        {
            _iD = id;
            _front = front;
            _back = back;
            _stackId = sid;
        }

        public int Id { get => _iD; }
        public string? Front { get => _front; }
        public string? Back { get => _back; }
        public int StackId { get => _stackId; }
    }
}
