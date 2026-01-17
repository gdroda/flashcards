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
                    var nameCheck = StackAndFlashcardList.AllStacks ?? Enumerable.Empty<Stacks>().Where(a => a.Name == stack).ToList();
                    if (stack != null && nameCheck[0] != null)
                    {
                        FlashcardMenu(nameCheck[0]);
                        Console.WriteLine("Stack not found");
                        Console.WriteLine("\nPress ENTER to return");
                        Console.ReadLine();
                        StacksMenu();
                    }
                    break;
                case "2":
                    Database_Functions.AddToTable("Stacks", null);
                    break;
                case "3":
                    Database_Functions.RemoveFromTable("Stacks", null);
                    break;
                case "4":
                    MainMenu();
                    break;
                default:
                    StacksMenu();
                    break;
            }
        }

        public static void FlashcardMenu(Stacks stackToView)
        {
            if (stackToView != null)
            {
                Console.Clear();
                Database_Functions.ShowFlashcard(stackToView);
                Console.WriteLine("\n1. Start studying session");
                Console.WriteLine("2. Create new flashcard");
                Console.WriteLine("3. Delete flashcard");
                Console.WriteLine("4. Return\n");

                string? comm = Console.ReadLine();

                switch (comm)
                {
                    case "1":
                        FlashcardMenu(stackToView); //temp
                        break;
                    case "2":
                        Database_Functions.AddToTable("Flashcards", stackToView);
                        break;
                    case "3":
                        Database_Functions.RemoveFromTable("Flashcards", stackToView);
                        break;
                    case "4":
                        StacksMenu();
                        break;
                    default:
                        FlashcardMenu(stackToView);
                        break;
                }
            }
            else StacksMenu();

        }
    }
}
