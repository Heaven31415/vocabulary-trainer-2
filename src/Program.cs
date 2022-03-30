namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = Config.ProgramName;

            UpdateFlashcardsFromCSV();
            Train();
        }

        static void UpdateFlashcardsFromCSV()
        {
            var verbs = CSV.ReadVerbsFromFile(Config.VerbsPath);
            var nouns = CSV.ReadNounsFromFile(Config.NounsPath);
            var adjectives = CSV.ReadAdjectivesFromFile(Config.AdjectivesPath);

            var singleFlashcards = SingleFlashcardSet.Load(Config.SingleFlashcardsPath);
            var multiFlashcards = MultiFlashcardSet.Load(Config.MultiFlashcardsPath);

            SingleFlashcardSet.Update(verbs, singleFlashcards);
            SingleFlashcardSet.Update(nouns, singleFlashcards);
            SingleFlashcardSet.Update(adjectives, singleFlashcards);
            MultiFlashcardSet.Update(verbs, multiFlashcards);

            SingleFlashcardSet.Save(Config.SingleFlashcardsPath, singleFlashcards);
            MultiFlashcardSet.Save(Config.MultiFlashcardsPath, multiFlashcards);
        }

        static void Train()
        {
            var singleFlashcards = SingleFlashcardSet.Load(Config.SingleFlashcardsPath);
            var multiFlashcards = MultiFlashcardSet.Load(Config.MultiFlashcardsPath);
            var flashcards = new List<Flashcard>();

            foreach (var flashcard in singleFlashcards)
                flashcards.Add(flashcard);

            foreach (var flashcard in multiFlashcards)
                flashcards.Add(flashcard);

            var random = new Random();
            var statistics = new Statistics(Config.StatisticsPath);
            var availableAtTheEndOfDay = flashcards.FindAll(f => f.IsAvailableAtTheEndOfDay).Count;

            while (true)
            {
                var availableFlashcards = flashcards.FindAll(f => f.IsAvailable);

                if (availableFlashcards.Count == 0)
                    break;

                Utility.WriteLine($"F<{flashcards.Count}> A<{availableFlashcards.Count}> A24<{availableAtTheEndOfDay}>");
                Utility.WriteLine();

                var flashcard = availableFlashcards[random.Next(availableFlashcards.Count)];

                Utility.Write($"Translate to German '{flashcard.AskQuestion()}': ");
                var answer = Utility.ReadLine();

                var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

                Utility.WriteLine();

                if (isCorrect)
                    Utility.WriteLine("Correct!", ConsoleColor.Green);
                else
                    Utility.WriteLine($"Incorrect. The correct answer is: '{correctAnswer}'", ConsoleColor.Red);

                statistics.OnQuestionAnswered(isCorrect);

                Utility.WriteLine();
                Utility.WriteLine("Press enter to continue...");
                Utility.ReadLine();
                Console.Clear();

                SingleFlashcardSet.Save(Config.SingleFlashcardsPath, singleFlashcards);
                MultiFlashcardSet.Save(Config.MultiFlashcardsPath, multiFlashcards);
            }
        }
    }
}
