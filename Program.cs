using flashcards;
using Microsoft.Data.SqlClient;

namespace Flashcards
{
    internal class Flashcards
    {
        static void Main(string[] args)
        {
            CreateDatabase();
            User_Input.MainMenu();
        }

        static void CreateDatabase()
        {
            string str;
            string check_str;
            SqlConnection connection = new("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=master;Trusted_Connection=true;");

            check_str = "IF NOT EXISTS(SELECT name FROM master.sys.databases WHERE name = N'flashcardDB')";
            str = "CREATE DATABASE flashcardDB ON PRIMARY " +
                "(NAME = flashcard_DB, " +
                "FILENAME = 'C:\\SQLData\\Flashcard\\flashcardDB.mdf', " +
                "SIZE = 5MB, MAXSIZE = 20MB, FILEGROWTH = 5MB)" +
                "LOG ON (NAME = flashcardDB_Log, " +
                "FILENAME = 'C:\\SQLDATA\\Flashcard\\flashcardLog.ldf', " +
                "SIZE = 2MB, MAXSIZE = 5MB, FILEGROWTH = 2MB)";
            

            SqlCommand command = new(check_str + str, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Database created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database...{ex}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            Database_Functions.CreateDefaultTables();
        }
    }
}