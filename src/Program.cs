using CsvHelper;
using System.Globalization;

namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

            var nouns = ReadNounsFromCSVFile("data/Nouns.csv");
            var flashcards = CreateFlashcards(nouns);
            TestFlashcard(flashcards[0]);
        }

        static List<Noun> ReadNounsFromCSVFile(string path)
        {
            var nouns = new List<Noun>();

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                string englishDescription = csv.GetField(0);
                string? germanSingularForm = csv.GetField(1);
                string? germanPluralForm = csv.GetField(2);

                if (string.IsNullOrEmpty(germanSingularForm))
                    germanSingularForm = null;

                if (string.IsNullOrEmpty(germanPluralForm))
                    germanPluralForm = null;

                var noun = new Noun(englishDescription, germanSingularForm, germanPluralForm);
                nouns.Add(noun);
            }

            return nouns;
        }

        static List<FlashcardBase> CreateFlashcards(List<Noun> nouns)
        {
            var flashcards = new List<FlashcardBase>();

            foreach (var noun in nouns)
            {
                if (noun.GermanSingularForm != null)
                {
                    string question = $"{noun.EnglishDescription} (singular)";
                    string answer = $"{noun.Gender.ToArticle()} {noun.GermanSingularForm}";
                    flashcards.Add(new Flashcard(0, Type.NounSingularForm, question, answer));
                }

                if (noun.GermanPluralForm != null)
                {
                    string question = $"{noun.EnglishDescription} (plural)";
                    string answer = $"die {noun.GermanPluralForm}";
                    flashcards.Add(new Flashcard(0, Type.NounPluralForm, question, answer));
                }
            }

            return flashcards;
        }

        static void TestFlashcard(FlashcardBase flashcard)
        {
            Utility.WriteLine($"Translate to german: '{flashcard.Ask()}'");
            Utility.Write("Answer: ");
            var answer = Utility.ReadLine();

            var (isCorrect, correctAnswer) = flashcard.Answer(answer);

            if (isCorrect)
                Utility.WriteLine("You are correct!", ConsoleColor.Green);
            else
                Utility.WriteLine($"This is not correct. The correct answer is: '{correctAnswer}'", ConsoleColor.Red);
        }
    }
}
