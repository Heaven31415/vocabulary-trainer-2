namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

            /*
            var flashcards = LoadFlashcards("data/Flashcards.json");
            TestFlashcard(flashcards[0]);
            */

            var verbs = CSV.ReadVerbsFromFile("data/Verbs.csv");
            var nouns = CSV.ReadNounsFromFile("data/Nouns.csv");
            var adjectives = CSV.ReadAdjectivesFromFile("data/Adjectives.csv");

            var nounFlashcards = FlashcardRepository.CreateFlashcards(nouns);
            var adjectiveFlashcards = FlashcardRepository.CreateFlashcards(adjectives);

            FlashcardRepository.SaveFlashcards("data/Flashcards.json", nounFlashcards.Concat(adjectiveFlashcards).ToList());
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
