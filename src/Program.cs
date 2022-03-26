namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

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

        static void TestFlashcard(FlashcardBase flashcard)
        {
            Utility.WriteLine($"Translate to german: '{flashcard.AskQuestion()}'");
            Utility.Write("Answer: ");
            var answer = Utility.ReadLine();

            var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

            if (isCorrect)
                Utility.WriteLine("You are correct!", ConsoleColor.Green);
            else
                Utility.WriteLine($"This is not correct. The correct answer is: '{correctAnswer}'", ConsoleColor.Red);
        }
    }
}
