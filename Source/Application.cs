using System.Text;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Flashcard;
using VocabularyTrainer2.Source.Word;

namespace VocabularyTrainer2.Source
{
    public class Application
    {
        private readonly VerbEndingsCache _verbEndingsCache;
        private readonly List<Adjective> _adjectives;
        private readonly List<Noun> _nouns;
        private readonly List<Other> _others;
        private readonly List<Verb> _verbs;
        private readonly FlashcardSet _flashcardSet;

        public Application()
        {
            Console.Title = Config.Instance.ProgramName;
            Console.OutputEncoding = Encoding.UTF8;

            if (Config.Instance.OnlineMode)
            {
                var sheetsDownloader = new SheetsDownloader();
                sheetsDownloader.DownloadAll();
            }

            _verbEndingsCache = new VerbEndingsCache();
            _adjectives = Adjective.ReadAllFromCsvFile();
            _nouns = Noun.ReadAllFromCsvFile();
            _others = Other.ReadAllFromCsvFile();
            _verbs = Verb.ReadAllFromCsvFile(_verbEndingsCache);

            _flashcardSet = new FlashcardSet(_adjectives, _nouns, _others, _verbs);
        }

        public void ProcessArgs(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    Run();
                    break;
                case 1:
                    switch (args[0])
                    {
                        case "-c":
                        case "--clear-cache":
                            File.Delete(Config.Instance.VerbEndingsCacheFilePath);
                            break;

                        case "-s":
                        case "--stats":
                            Console.WriteLine("Vocabulary:");
                            Console.WriteLine($"  Adjectives: {_adjectives.Count}");
                            Console.WriteLine($"  Nouns: {_nouns.Count}");
                            Console.WriteLine($"  Others: {_others.Count}");
                            Console.WriteLine($"  Verbs: {_verbs.Count}");

                            var flashcards = _flashcardSet.Flashcards;
                            var available = flashcards.FindAll(f => f.IsAvailable()).Count;
                            var availableAtDaysEnd = flashcards.FindAll(f => f.IsAvailableAtDaysEnd()).Count;

                            Console.WriteLine("Flashcards: ");
                            Console.WriteLine($"  All: {flashcards.Count}");
                            Console.WriteLine($"  Available: {available}");
                            Console.WriteLine($"  Available at days end: {availableAtDaysEnd}");

                            var results = 0;
                            var successfulResults = 0;

                            foreach (var f in flashcards)
                            {
                                foreach (var r in f.Results)
                                {
                                    results++;

                                    if (r.IsSuccessful)
                                        successfulResults++;
                                }
                            }

                            var pct = 0.0;

                            if (results != 0)
                                pct = 100.0 * successfulResults / results;

                            Console.WriteLine("Results: ");
                            Console.WriteLine($"  All: {results}");
                            Console.WriteLine($"  Successful: {successfulResults} ({pct:0.00}%)");

                            break;
                        default:
                            Utility.WriteRedLine($"Invalid option: '{args[0]}'");
                            break;
                    }
                    break;
                default:
                    Utility.WriteRedLine($"Invalid number of arguments: {args.Length}");
                    break;
            }
        }

        private void Run()
        {
            while (true)
            {
                var flashcard = _flashcardSet.GetRandomFlashcard();

                if (flashcard == null)
                {
                    Utility.WriteGreenLine("Congratulations! There is nothing else to practice.");
                    Console.WriteLine();
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }

                Console.Write($"Answer question '{flashcard.AskQuestion()}': ");
                var answer = Utility.ReadLine();

                var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

                Console.WriteLine();

                if (isCorrect)
                    Utility.WriteGreenLine("Correct!");
                else
                    Utility.WriteRedLine($"Incorrect! The correct answer is: '{correctAnswer}'.");

                var bonus = FindFlashcardBonus(flashcard);

                if (bonus != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Additional information: {bonus}");
                }

                Console.WriteLine();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();

                _flashcardSet.SaveToFileAsJson();
            }
        }

        private string? FindFlashcardBonus(Flashcard.Flashcard flashcard)
        {
            return (flashcard.ParentId % 10) switch
            {
                0 => _adjectives.Find(a => a.Id == flashcard.ParentId)?.Bonus,
                1 => _nouns.Find(n => n.Id == flashcard.ParentId)?.Bonus,
                2 => _others.Find(o => o.Id == flashcard.ParentId)?.Bonus,
                3 => _verbs.Find(v => v.Id == flashcard.ParentId)?.Bonus,
                _ => throw new Exception("Unknown type of object. Unable to find flashcard bonus."),
            };
        }
    }
}
