using Microsoft.Data.SqlClient;


namespace flashcards
{
    internal class Database_Functions
    {
        static void ConnectAndCommand(string str, bool msg)
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

        static List<Stacks>? ConnectAndReceiveStacks(string str)
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

        static List<Flashcard>? ConnectAndReceiveFlashcards(string str, Stacks stackInp)
        {
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=flashcardDB;Trusted_Connection=true;");
            SqlCommand command = new(str, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<Flashcard> flashcardList = new();
                Console.WriteLine("Front\tBack");
                Console.WriteLine("---------------------");
                while (reader.Read())
                {
                    string? front = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string? back = reader.IsDBNull(2) ? null : reader.GetString(2);
                    flashcardList.Add(new Flashcard(reader.GetInt32(0), front, back, stackInp.Id));
                    Console.WriteLine($"{front}\t{back}");
                }
                return flashcardList;
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
            ConnectAndCommand(stra, false);
            string strb = "IF OBJECT_ID('Flashcards') is null CREATE TABLE Flashcards (Id INT PRIMARY KEY IDENTITY, Front VARCHAR(64), Back VARCHAR(64), StackId INT, FOREIGN KEY (StackId) REFERENCES Stacks(Id))";
            ConnectAndCommand(strb, false);
        }

        public static void AddToTable(string table, Stacks? stackInp)
        {
            switch (table)
            {
                case "Stacks":
                    Console.Write("\nName: ");
                    string? stackName = Console.ReadLine();
                    string strS = $@"INSERT INTO {table} (Name) VALUES ('{stackName}')";
                    ConnectAndCommand(strS, true);
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
                        ConnectAndCommand(strF, true);
                        User_Input.FlashcardMenu(stackInp);
                    }
                    break;
                default:
                    break;
            }
        }


        public static void RemoveFromTable(string table, Stacks? stackInp)
        {
            switch (table)
            {
                case "Stacks":
                    Console.Write("\nName: ");
                    string? stackName = Console.ReadLine();
                    var nameCheck = (StackAndFlashcardList.AllStacks ?? Enumerable.Empty<Stacks>()).Where(s => s.Name == stackName).ToList();
                Repeat:
                    Console.WriteLine("This will also delete the flashcards contained. Are you sure? (y/n)");
                    string? opt = Console.ReadLine();
                    switch (opt)
                    {
                        case "y":
                            string stra = $@"DELETE FROM Flashcards Where (StackId = '{nameCheck[0].Id}')";
                            ConnectAndCommand(stra, false);
                            string strb = $@"DELETE FROM {table} Where (Name = '{stackName}')";
                            ConnectAndCommand(strb, true);
                            User_Input.StacksMenu();
                            break;
                        case "n":
                            User_Input.StacksMenu();
                            break;
                        default:
                            Console.WriteLine("Wrong Input only y or n");
                            goto Repeat;
                    }
                    break;

                case "Flashcards":
                    Console.Write("\nName: ");
                    string? fcFront = Console.ReadLine();
                    string strf = $@"DELETE FROM {table} Where (Front = '{fcFront}')";
                    ConnectAndCommand(strf, true);
                    if (stackInp != null) User_Input.FlashcardMenu(stackInp);
                    break;
                default:
                    break;
            }
        }

        public static List<Stacks>? ShowStacks()
        {
            string str = @"SELECT * FROM Stacks";
            List<Stacks>? stacksList = ConnectAndReceiveStacks(str);
            StackAndFlashcardList.AllStacks = stacksList;
            return stacksList;
        }

        public static void ShowFlashcard(Stacks stack)
        {
            string str = $@"SELECT * FROM Flashcards WHERE StackId = {stack.Id}";
            StackAndFlashcardList.AllFlashcards = ConnectAndReceiveFlashcards(str, stack);
        }
    }

    class StackAndFlashcardList
    {
        static List<Flashcard>? allFlashcards;
        static List<Stacks>? allStacks;

        public static List<Flashcard>? AllFlashcards { get => allFlashcards; set => allFlashcards = value; }
        public static List<Stacks>? AllStacks { get => allStacks; set => allStacks = value; }
    }

    class Stacks
    {
        private readonly int _iD;
        private readonly string? _name;

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
        private readonly int _iD;
        private readonly string? _front;
        private readonly string? _back;
        private readonly int _stackId;

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
