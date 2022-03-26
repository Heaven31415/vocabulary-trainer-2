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

        static void Train()
        {
            var normalFlashcards = FlashcardRepository.LoadFlashcards("data/Flashcards.json");
            var randomFlashcards = FlashcardRepository.LoadRandomFlashcards("data/RandomFlashcards.json");
            var flashcards = new List<FlashcardBase>();

            foreach (var flashcard in normalFlashcards)
                flashcards.Add(flashcard);

            foreach (var flashcard in randomFlashcards)
                flashcards.Add(flashcard);

            var random = new Random();

            while (true)
            {
                var availableFlashcards = flashcards.FindAll(f => f.IsAvailable);

                if (availableFlashcards.Count == 0)
                    break;

                Utility.WriteLine($"Available flashcards: {availableFlashcards.Count}");
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

                Utility.WriteLine();
                Utility.WriteLine("Press enter to continue...");
                Utility.ReadLine();
                Console.Clear();

                FlashcardRepository.SaveFlashcards("data/Flashcards.json", normalFlashcards);
                FlashcardRepository.SaveRandomFlashcards("data/RandomFlashcards.json", randomFlashcards);
            }
        }

        static void UpdateFlashcardsFromCSV()
        {
            var verbs = CSV.ReadVerbsFromFile("data/Verbs.csv");
            var nouns = CSV.ReadNounsFromFile("data/Nouns.csv");
            var adjectives = CSV.ReadAdjectivesFromFile("data/Adjectives.csv");

            var flashcards = FlashcardRepository.LoadFlashcards("data/Flashcards.json");

            FlashcardRepository.UpdateFlashcards(verbs, flashcards);
            FlashcardRepository.UpdateFlashcards(nouns, flashcards);
            FlashcardRepository.UpdateFlashcards(adjectives, flashcards);

            FlashcardRepository.SaveFlashcards("data/Flashcards.json", flashcards);

            var randomFlashcards = new List<RandomFlashcard>();

            FlashcardRepository.UpdateRandomFlashcards(verbs, randomFlashcards);
            FlashcardRepository.SaveRandomFlashcards("data/RandomFlashcards.json", randomFlashcards);
        }

        static void BuildFlashcardsFromCSV()
        {
            var verbs = CSV.ReadVerbsFromFile("data/Verbs.csv");
            var nouns = CSV.ReadNounsFromFile("data/Nouns.csv");
            var adjectives = CSV.ReadAdjectivesFromFile("data/Adjectives.csv");

            var verbFlashcards = FlashcardRepository.CreateFlashcards(verbs);
            var nounFlashcards = FlashcardRepository.CreateFlashcards(nouns);
            var adjectiveFlashcards = FlashcardRepository.CreateFlashcards(adjectives);

            var flashcards = new List<Flashcard>();

            foreach (var flashcard in verbFlashcards)
                flashcards.Add(flashcard);

            foreach (var flashcard in nounFlashcards)
                flashcards.Add(flashcard);

            foreach (var flashcard in adjectiveFlashcards)
                flashcards.Add(flashcard);

            FlashcardRepository.SaveFlashcards("data/Flashcards.json", flashcards);

            var verbRandomFlashcards = FlashcardRepository.CreateRandomFlashcards(verbs);
            FlashcardRepository.SaveRandomFlashcards("data/RandomFlashcards.json", verbRandomFlashcards);
        }
    }
}
