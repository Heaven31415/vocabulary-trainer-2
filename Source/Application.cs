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
                            ClearCache();
                            break;

                        case "-s":
                        case "--stats":
                            Stats();
                            break;
                        default:
                            Utility.WriteRedLine($"Invalid option: '{args[0]}'");
                            break;
                    }
                    break;
                case 2:
                    switch (args[0])
                    {
                        case "-l":
                        case "--limit":
                            if (int.TryParse(args[1], out int limit) && limit > 0)
                                Run(limit);
                            else
                                Utility.WriteRedLine($"Option '{args[0]}' first arg should be a positive int");
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

        private void Run(int? limit = null)
        {
            var counter = 0;

            while (true)
            {
                if (limit != null && limit > 0 && limit == counter)
                {
                    Utility.WriteGreenLine($"Congratulations! You have reached your flashcards limit of {limit}.");
                    Console.WriteLine();
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }

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
                var answer = Console.ReadLine();

                if (answer == null)
                    break;

                var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

                _flashcardSet.SaveToFileAsJson();

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

                counter++;

                Console.WriteLine();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();
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

        private void ClearCache()
        {
            File.Delete(Config.Instance.VerbEndingsCacheFilePath);
        }

        private void Stats()
        {
            Utility.WriteGreenLine("Vocabulary:");
            Console.WriteLine($"  Adjectives: {_adjectives.Count}");
            Console.WriteLine($"  Nouns: {_nouns.Count}");
            Console.WriteLine($"  Others: {_others.Count}");
            Console.WriteLine($"  Verbs: {_verbs.Count}");

            var flashcards = _flashcardSet.Flashcards;
            var available = flashcards.FindAll(f => f.IsAvailable()).Count;
            var availableAtDaysEnd = flashcards.FindAll(f => f.IsAvailableAtDaysEnd()).Count;

            Utility.WriteGreenLine("Flashcards:");
            Console.WriteLine($"  All: {flashcards.Count}");
            Console.WriteLine($"  Available: {available}");
            Console.WriteLine($"  Available at days end: {availableAtDaysEnd}");

            var answeredLifetime = 0;
            var successfulLifetime = 0;

            foreach (var f in flashcards)
            {
                foreach (var r in f.Results)
                {
                    answeredLifetime++;

                    if (r.IsSuccessful)
                        successfulLifetime++;
                }
            }

            var pctLifetime = 0.0;

            if (answeredLifetime != 0)
                pctLifetime = 100.0 * successfulLifetime / answeredLifetime;

            Utility.WriteGreenLine("Results:");
            Console.WriteLine("  Lifetime:");
            Console.WriteLine($"    Answered: {answeredLifetime}");
            Console.WriteLine($"    Successful: {successfulLifetime} ({pctLifetime:0.00}%)");

            var answeredToday = 0;
            var successfulToday = 0;

            foreach (var f in flashcards)
            {
                foreach (var r in f.Results)
                {
                    if (DateTime.Today <= r.Time && r.Time < DateTime.Today.AddDays(1))
                    {
                        answeredToday++;

                        if (r.IsSuccessful)
                            successfulToday++;
                    }
                }
            }

            var pctToday = 0.0;

            if (answeredToday != 0)
                pctToday = 100.0 * successfulToday / answeredToday;

            Console.WriteLine("  Today:");
            Console.WriteLine($"    Answered: {answeredToday}");
            Console.WriteLine($"    Successful: {successfulToday} ({pctToday:0.00}%)");
            Console.WriteLine("  Yesterday:");

            var answeredYesterday = 0;
            var successfulYesterday = 0;

            foreach (var f in flashcards)
            {
                foreach (var r in f.Results)
                {
                    if (DateTime.Today.AddDays(-1) <= r.Time && r.Time < DateTime.Today)
                    {
                        answeredYesterday++;

                        if (r.IsSuccessful)
                            successfulYesterday++;
                    }
                }
            }

            var pctYesterday = 0.0;

            if (answeredYesterday != 0)
                pctYesterday = 100.0 * successfulYesterday / answeredYesterday;

            Console.WriteLine($"    Answered: {answeredYesterday}");
            Console.WriteLine($"    Successful: {successfulYesterday} ({pctYesterday:0.00}%)");
        }
    }
}
