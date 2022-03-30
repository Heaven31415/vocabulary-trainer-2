namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

            UpdateFlashcardsFromCSV();
            Train();
        }

        static void UpdateFlashcardsFromCSV()
        {
            var verbs = CSV.ReadVerbsFromFile("data/Verbs.csv");
            var nouns = CSV.ReadNounsFromFile("data/Nouns.csv");
            var adjectives = CSV.ReadAdjectivesFromFile("data/Adjectives.csv");

            var singleFlashcards = FlashcardRepository.LoadSingleFlashcards("data/SingleFlashcards.json");
            var multiFlashcards = FlashcardRepository.LoadMultiFlashcards("data/MultiFlashcards.json");

            FlashcardRepository.UpdateSingleFlashcards(verbs, singleFlashcards);
            FlashcardRepository.UpdateSingleFlashcards(nouns, singleFlashcards);
            FlashcardRepository.UpdateSingleFlashcards(adjectives, singleFlashcards);
            FlashcardRepository.UpdateMultiFlashcards(verbs, multiFlashcards);

            FlashcardRepository.SaveSingleFlashcards("data/SingleFlashcards.json", singleFlashcards);
            FlashcardRepository.SaveMultiFlashcards("data/MultiFlashcards.json", multiFlashcards);
        }

        static void Train()
        {
            var singleFlashcards = FlashcardRepository.LoadSingleFlashcards("data/SingleFlashcards.json");
            var multiFlashcards = FlashcardRepository.LoadMultiFlashcards("data/MultiFlashcards.json");
            var flashcards = new List<Flashcard>();

            foreach (var flashcard in singleFlashcards)
                flashcards.Add(flashcard);

            foreach (var flashcard in multiFlashcards)
                flashcards.Add(flashcard);

            var random = new Random();
            var statistics = new Statistics("data/Statistics.json");
            var availableAtTheEndOfDay = flashcards.FindAll(f => f.IsAvailableAtTheEndOfDay).Count;

            while (true)
            {
                var availableFlashcards = flashcards.FindAll(f => f.IsAvailable);

                if (availableFlashcards.Count == 0)
                    break;

                Utility.WriteLine($"Flashcards: {flashcards.Count}");
                Utility.WriteLine($"Available: {availableFlashcards.Count}");
                Utility.WriteLine($"Available at the end of day: {availableAtTheEndOfDay}");
                Utility.WriteLine();

                var flashcard = availableFlashcards[random.Next(availableFlashcards.Count)];

                Utility.WriteLine($"Translate to German: '{flashcard.AskQuestion()}'");
                Utility.Write("Answer: ");
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

                FlashcardRepository.SaveSingleFlashcards("data/SingleFlashcards.json", singleFlashcards);
                FlashcardRepository.SaveMultiFlashcards("data/MultiFlashcards.json", multiFlashcards);
            }
        }
    }
}
