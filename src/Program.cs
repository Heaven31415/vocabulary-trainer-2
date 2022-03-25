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

            /*
            var flashcards = LoadFlashcards("data/Flashcards.json");
            TestFlashcard(flashcards[0]);
            */

            var nouns = CSV.ReadNounsFromFile("data/Nouns.csv");
            var adjectives = CSV.ReadAdjectivesFromFile("data/Adjectives.csv");

            var nounFlashcards = CreateFlashcards(nouns);
            var adjectiveFlashcards = CreateFlashcards(adjectives);

            SaveFlashcards("data/Flashcards.json", nounFlashcards.Concat(adjectiveFlashcards).ToList());
        }
        static List<Flashcard> CreateFlashcards(List<Noun> nouns)
        {
            var flashcards = new List<Flashcard>();

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

        static List<Flashcard> CreateFlashcards(List<Adjective> adjectives)
        {
            var flashcards = new List<Flashcard>();

            foreach (var adjective in adjectives)
            {
                {
                    string question = $"{adjective.EnglishDescription} (positive)";
                    string answer = adjective.PositiveDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectivePositiveDegree, question, answer));
                }
                
                if (!string.IsNullOrEmpty(adjective.ComparativeDegree))
                {
                    string question = $"{adjective.EnglishDescription} (comparative)";
                    string answer = adjective.ComparativeDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectiveComparativeDegree, question, answer));
                }

                if (!string.IsNullOrEmpty(adjective.SuperlativeDegree))
                {
                    string question = $"{adjective.EnglishDescription} (superlative)";
                    string answer = adjective.SuperlativeDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectiveSuperlativeDegree, question, answer));
                }
            }

            return flashcards;
        }

        static List<Flashcard> LoadFlashcards(string path)
        {
            var json = File.ReadAllText(path);
            var flashcardHelpers = JsonSerializer.Deserialize<List<FlashcardHelper>>(json);
            var flashcards = new List<Flashcard>();

            if (flashcardHelpers == null)
                throw new Exception("Flashcard Helpers are null!");

            foreach (var flashcardHelper in flashcardHelpers)
            {
                var parentId = flashcardHelper.ParentId;
                var type = flashcardHelper.Type;
                var question = flashcardHelper.Question;
                var answer = flashcardHelper.Answer;

                var flashcard = new Flashcard(parentId, type, question, answer)
                {
                    LastTrainingTime = flashcardHelper.LastTrainingTime,
                    Cooldown = flashcardHelper.Cooldown
                };

                flashcards.Add(flashcard);
            }

            return flashcards;
        }

        static void SaveFlashcards(string path, List<Flashcard> flashcards)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(flashcards, options);
            File.WriteAllText(path, json);
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
