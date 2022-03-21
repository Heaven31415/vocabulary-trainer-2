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
            var flashcards = new List<Flashcard>();

            foreach (var noun in nouns)
            {
                if (noun.GermanSingularForm != null)
                {
                    string question = $"{noun.EnglishDescription} (singular)";
                    flashcards.Add(new Flashcard(question, $"{noun.Gender.ToArticle()} {noun.GermanSingularForm}"));
                }

                if (noun.GermanPluralForm != null)
                {
                    string question = $"{noun.EnglishDescription} (plural)";
                    flashcards.Add(new Flashcard(question, $"die {noun.GermanPluralForm}"));
                }
            }

            var flashcard = flashcards[0];

            Utility.WriteLine($"Translate to german: '{flashcard.AskQuestion()}'");
            Utility.Write("Answer: ");
            var answer = Utility.ReadLine();

            var (isCorrect, correctAnswer) = flashcard.GiveAnswer(answer);

            if (isCorrect)
                Utility.WriteLine("You are correct!", ConsoleColor.Green);
            else
                Utility.WriteLine($"This is not correct. The correct answer is: '{correctAnswer}'", ConsoleColor.Red);
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
    }
}
