using System.Text;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Common.Statistics;
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
                    RunWithoutAnyLimit();
                    break;
                case 1:
                    switch (args[0])
                    {
                        case "-c":
                        case "--clear-cache":
                            ClearCache();
                            break;

                        case "-s":
                        case "--statistics":
                            DisplayStatistics();
                            break;
                        default:
                            Utility.WriteRedLine($"Invalid option: '{args[0]}'");
                            break;
                    }
                    break;
                case 2:
                    switch (args[0])
                    {
                        case "-f":
                        case "--flashcard-limit":
                            if (int.TryParse(args[1], out int flashcardLimit) && flashcardLimit > 0)
                                RunWithFlashcardLimit(flashcardLimit);
                            else
                                Utility.WriteRedLine($"Option '{args[0]}' first arg should be a positive int");
                            break;
                        case "-t":
                        case "--time-limit":
                            if (int.TryParse(args[1], out int timeLimit) && timeLimit > 0)
                                RunWithTimeLimit(timeLimit);
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

        private void RunWithoutAnyLimit()
        {
            Console.Clear();

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

                Console.WriteLine();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private void RunWithFlashcardLimit(int flashcardLimit)
        {
            Console.Clear();

            var counter = 0;

            while (true)
            {
                if (flashcardLimit == counter)
                {
                    Utility.WriteGreenLine($"Congratulations! You have reached your flashcards limit of {flashcardLimit}.");
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

        private void RunWithTimeLimit(int timeLimit)
        {
            Console.Clear();

            var endTime = DateTime.Now.AddMinutes(timeLimit);

            while (true)
            {
                if (DateTime.Now >= endTime)
                {
                    var minutes = timeLimit > 1 ? "minutes" : "minute";

                    Utility.WriteGreenLine($"Congratulations! You have reached your time limit of {timeLimit} {minutes}.");
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

        private void DisplayStatistics()
        {
            var statistics = new Statistics(_adjectives, _nouns, _others, _verbs, _flashcardSet);

            Utility.WriteBlueLine("Vocabulary:");
            Utility.WriteYellow($"  {"Adjectives:",-11} ");
            Console.WriteLine($"{statistics.AdjectivesCount,4}");
            Utility.WriteYellow($"  {"Nouns:",-11} ");
            Console.WriteLine($"{statistics.NounsCount,4}");
            Utility.WriteYellow($"  {"Others:",-11} ");
            Console.WriteLine($"{statistics.OthersCount,4}");
            Utility.WriteYellow($"  {"Verbs:",-11} ");
            Console.WriteLine($"{statistics.VerbsCount,4}");
            Utility.WriteYellow($"  {"Total:",-11} ");
            Console.WriteLine($"{statistics.VocabularyCount,4}");

            Utility.WriteBlueLine("Flashcards:");
            Utility.WriteYellow($"  {"Available (now):",-26}");
            Console.WriteLine($"{statistics.FlashcardsAvailableNow,5}");
            Utility.WriteYellow($"  {"Available (next 24 hours):",-26}");
            Console.WriteLine($"{statistics.FlashcardsAvailableNext24Hours,5}");
            Utility.WriteYellow($"  {"Available (next 7 days):",-26}");
            Console.WriteLine($"{statistics.FlashcardsAvailableNext7Days,5}");
            Utility.WriteYellow($"  {"All:",-26}");
            Console.WriteLine($"{statistics.FlashcardsCount,5}");

            Utility.WriteBlueLine("Results:");
            ShowFlashcardsStatistics("  Today:", statistics.FlashcardsToday);
            ShowFlashcardsStatistics("  Last 7 days:", statistics.FlashcardsLast7Days);
            ShowFlashcardsStatistics("  Last 30 days:", statistics.FlashcardsLast30Days);
            ShowFlashcardsStatistics("  Lifetime:", statistics.FlashcardsLifetime);

            Utility.WriteBlueLine("Cooldowns:");
            foreach (var cooldown in statistics.FlashcardsCooldowns.Keys.OrderBy(item => item))
            {
                var count = statistics.FlashcardsCooldowns[cooldown];
                var percentage = statistics.FlashcardsCooldownsPercentages[cooldown];
                var days = cooldown.Days > 1 ? "days" : "day";

                Utility.WriteYellow($"  {cooldown.Days,3} {days,4}:");
                Console.WriteLine($"{count,4} ({percentage:00.00}%)");
            }
        }

        private static void ShowFlashcardsStatistics(string description, FlashcardsStatistics statistics)
        {
            Console.WriteLine(description);
            Utility.WriteYellow($"    {"Practiced:",-15} ");
            Console.WriteLine($"{statistics.Practiced,6}");
            Utility.WriteYellow($"    {"Successfully:",-15} ");
            Utility.WriteGreen($"{statistics.PracticedSuccessfully,6} ");
            Console.WriteLine($"({statistics.PracticedSuccessfullyPercentage:00.00}%)");
            Utility.WriteYellow($"    {"Unsuccessfully:",-15} ");
            Utility.WriteRed($"{statistics.PracticedUnsuccessfully,6} ");
            Console.Write($"({statistics.PracticedUnsuccessfullyPercentage:00.00}%)");
            Console.WriteLine();
        }

        private static void ClearCache()
        {
            File.Delete(Config.Instance.VerbEndingsCacheFilePath);
        }
    }
}
