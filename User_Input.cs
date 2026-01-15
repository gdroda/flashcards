
namespace flashcards
{
    internal class User_Input
    {
        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("~ Flashcards ~");
            Console.WriteLine("\n1. Stacks Menu");
            Console.WriteLine("2. Exit\n");

            string? comm = Console.ReadLine();

            switch (comm)
            {
                case "1":
                    StacksMenu();
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    MainMenu();
                    break;
            }
        }

        public static void StacksMenu()
        {
            Console.Clear();
            var stackList = Database_Functions.ShowStacks();
            Console.WriteLine("\n1. Enter stack");
            Console.WriteLine("2. Create new stack");
            Console.WriteLine("3. Delete stack");
            Console.WriteLine("4. Return\n");

            string? comm = Console.ReadLine();

            switch (comm)
            {
                case "1":
                    Console.Write("\nEnter name of stack: ");
                    string? stack = Console.ReadLine();
                    if (stack != null)
                    {
                        foreach(string s in stackList)
                        {
                            if (s == stack)
                            {
                                FlashcardMenu(stack);
                                return;
                            }
                        }
                        Console.WriteLine("\nStack not found");
                        Console.WriteLine("Press ENTER to return");
                        Console.ReadLine();
                        StacksMenu();
                    }
                    break;
                case "2":
                    Database_Functions.AddToTable("Stacks");
                    break;
                case "3":
                    Database_Functions.RemoveFromTable("Stacks");
                    break;
                case "4":
                    MainMenu();
                    break;
                default:
                    StacksMenu();
                    break;
            }
        }

        public static void FlashcardMenu(string stackToView)
        {
            string stack = stackToView;
            if (stack == "") StacksMenu();

            Console.Clear();
            Console.WriteLine("\n1. Show stacks");
            Console.WriteLine("2. Create new flashcard");
            Console.WriteLine("3. Delete flashcard");
            Console.WriteLine("4. Return\n");

            string? comm = Console.ReadLine();

            switch (comm)
            {
                case "1":
                    FlashcardMenu(stack); //temp
                    break;
                case "2":
                    Database_Functions.AddToTable("Flashcards");
                    break;
                case "3":
                    Database_Functions.RemoveFromTable("Flashcards");
                    break;
                case "4":
                    MainMenu();
                    break;
                default:
                    FlashcardMenu(stack);
                    break;
            }
        }
    }
}
