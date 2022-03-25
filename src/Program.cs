using CsvHelper;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

            var nouns = ReadNounsFromCSVFile("data/Nouns.csv");
            var flashcards = CreateFlashcards(nouns);
            var objects = new List<object>();

            foreach (var flashcard in flashcards)
                objects.Add(flashcard);

            var options = new JsonSerializerOptions { 
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize<object>(objects, options);
            File.WriteAllText("data/Flashcards.json", json);

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
                int id = csv.GetField<int>(0);
                string englishDescription = csv.GetField(1);
                string? germanSingularForm = csv.GetField(2);
                string? germanPluralForm = csv.GetField(3);

                if (string.IsNullOrEmpty(germanSingularForm))
                    germanSingularForm = null;

                if (string.IsNullOrEmpty(germanPluralForm))
                    germanPluralForm = null;

                var noun = new Noun(id, englishDescription, germanSingularForm, germanPluralForm);
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
                    flashcards.Add(new Flashcard(noun.Id, Type.NounSingularForm, question, answer));
                }

                if (noun.GermanPluralForm != null)
                {
                    string question = $"{noun.EnglishDescription} (plural)";
                    string answer = noun.GermanPluralForm;
                    flashcards.Add(new Flashcard(noun.Id, Type.NounPluralForm, question, answer));
                }
            }

            return flashcards;
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
