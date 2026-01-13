using System;
using Microsoft.Data.SqlClient;


namespace flashcards
{
    internal class Database_Functions
    {
        static void EstablishConnection(string str)
        {
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=flashcardDB;Trusted_Connection=true;");
            SqlCommand command = new(str, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Table Created.");
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

        public static void CreateDefaultTables()
        {
            string stra = "IF OBJECT_ID('Stacks') is null CREATE TABLE Stacks (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(64), StackId INT, FOREIGN KEY (StackId) REFERENCES Stacks(Id))";
            EstablishConnection(stra);
            string strb = "IF OBJECT_ID('Flashcards') is null CREATE TABLE Flashcards (Id INT PRIMARY KEY IDENTITY, Front VARCHAR(64), Back VARCHAR(64), FlashcardId INT, FOREIGN KEY (FlashcardId) REFERENCES Flashcards(Id))";
            EstablishConnection(strb);
        }
    }
}
