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
                    string? stackName = Console.ReadLine();
                    var nameCheck = (StackAndFlashcardList.AllStacks ?? Enumerable.Empty<Stacks>()).Where(a => a.Name == stackName).ToList();
                    if (nameCheck.Count > 0) FlashcardMenu(nameCheck[0]);
                    else
                    {
                        Console.WriteLine("Stack name not found. Press ENTER to return");
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
                        StudySession(stackToView); //temp NEXT TO WORK ON, IMPLEMENT STUDY SESSION
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

        public static void StudySession(Stacks stackToStudy)
        {
            //CREATE A TEMP LIST OF FLASHCARDS BELONGING TO CURRENT STACK
            var currentList = (StackAndFlashcardList.AllFlashcards ?? Enumerable.Empty<Flashcard>()).Where(i => i.StackId == stackToStudy.Id).ToList();
            int score = 0;
            int maxScore = currentList.Count;

            //SHOW RANDOM FLASHCARD AND WAIT FOR INPUT
            while (currentList.Count > 0)
            {
                var rand = new Random();
                var index = rand.Next(0,currentList.Count-1);
                Console.Clear();
                Console.WriteLine($"Score: {score} / {maxScore}\n");
                Console.WriteLine(currentList[index].Front);
                Console.WriteLine("\nType the answer or 0 to exit.");
                //IF INPUT CORRECT INCREASE SCORE IF NOT, SHOW CORRECT AND MOVE TO NEXT FLASHCARD WHILE REMOVING SHOWN FROM LIST
                Console.Write("Answer: ");
                string? answer = Console.ReadLine();

                if (answer == "0") FlashcardMenu(stackToStudy);

                if (answer != null)
                if (string.Equals(answer, currentList[index].Back, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else Console.WriteLine($"Wrong! Correct answer was {currentList[index].Back}");

                currentList.Remove(currentList[index]);
                Console.WriteLine("\nPress ENTER to continue");
                Console.ReadLine();
            }

            FlashcardMenu(stackToStudy);
            //SAVE SESSION DATA
        }

        public static void DisplayFlashcard()
        {

        }
    }
}
