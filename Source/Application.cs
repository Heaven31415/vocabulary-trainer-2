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
                    Utility.WriteGreenLine("Congratulations! There is nothing else to practice");
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
                    Utility.WriteRedLine($"Incorrect! The correct answer is: '{correctAnswer}'");

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
                    Utility.WriteGreenLine($"Congratulations! You have reached your flashcards limit of {flashcardLimit}");
                    Console.WriteLine();
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }

                var flashcard = _flashcardSet.GetRandomFlashcard();

                if (flashcard == null)
                {
                    Utility.WriteGreenLine("Congratulations! There is nothing else to practice");
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
                    Utility.WriteRedLine($"Incorrect! The correct answer is: '{correctAnswer}'");

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

                    Utility.WriteGreenLine($"Congratulations! You have reached your time limit of {timeLimit} {minutes}");
                    Console.WriteLine();
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }

                var flashcard = _flashcardSet.GetRandomFlashcard();

                if (flashcard == null)
                {
                    Utility.WriteGreenLine("Congratulations! There is nothing else to practice");
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
                    Utility.WriteRedLine($"Incorrect! The correct answer is: '{correctAnswer}'");

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
                _ => throw new Exception("Unknown type of object. Unable to find flashcard bonus"),
            };
        }

        private void DisplayStatistics()
        {
            var statistics = new Statistics(_adjectives, _nouns, _others, _verbs, _flashcardSet);

            Dictionary<Text, string> data;

            switch (Config.Instance.Language)
            {
                case "en_US":
                    data = new Dictionary<Text, string>()
                    {
                        {Text.Vocabulary, "Vocabulary"},
                        {Text.Adjectives, "Adjectives:"},
                        {Text.Nouns, "Nouns:"},
                        {Text.Others, "Others:"},
                        {Text.Verbs, "Verbs:"},
                        {Text.Total, "Total:"},
                        {Text.Flashcards, "Flashcards"},
                        {Text.FlashcardsAvailable, "Available"},
                        {Text.FlashcardsAvailableNow, "Now:"},
                        {Text.FlashcardsAvailableNext24Hours, "24 hours:"},
                        {Text.FlashcardsAvailableNext7Days, "7 days:"},
                        {Text.FlashcardsResults, "Results"},
                        {Text.FlashcardsResultsToday, "Today"},
                        {Text.FlashcardsResultsLast7Days, "7 days"},
                        {Text.FlashcardsResultsLast30Days, "30 days"},
                        {Text.FlashcardsResultsLifetime, "Lifetime"},
                        {Text.FlashcardsResultsPracticed, "Practiced:"},
                        {Text.FlashcardsResultsPracticedSuccessfully, "Successfully:"},
                        {Text.FlashcardsResultsPracticedUnsuccessfully, "Unsuccessfully:"},
                        {Text.FlashcardsCooldowns, "Cooldowns"},
                        {Text.FlashcardsCooldownsDays, "Days"}
                    };
                    break;

                case "de_DE":
                    data = new Dictionary<Text, string>()
                    {
                        {Text.Vocabulary, "Wortschatz"},
                        {Text.Adjectives,"Adjektive:"},
                        {Text.Nouns, "Nomen:"},
                        {Text.Others, "Andere:"},
                        {Text.Verbs, "Verben:"},
                        {Text.Total, "Gesamt:"},
                        {Text.Flashcards, "Karteikarten"},
                        {Text.FlashcardsAvailable, "Verfügbar"},
                        {Text.FlashcardsAvailableNow, "Jetzt:"},
                        {Text.FlashcardsAvailableNext24Hours, "24 Stunden:"},
                        {Text.FlashcardsAvailableNext7Days, "7 Tage:"},
                        {Text.FlashcardsResults, "Ergebnisse"},
                        {Text.FlashcardsResultsToday, "Heute"},
                        {Text.FlashcardsResultsLast7Days, "7 Tage"},
                        {Text.FlashcardsResultsLast30Days, "30 Tage"},
                        {Text.FlashcardsResultsLifetime, "Lebenszeit"},
                        {Text.FlashcardsResultsPracticed, "Geübt:"},
                        {Text.FlashcardsResultsPracticedSuccessfully, "Erfolgreich:"},
                        {Text.FlashcardsResultsPracticedUnsuccessfully, "Erfolglos:"},
                        {Text.FlashcardsCooldowns, "Abklingzeiten"},
                        {Text.FlashcardsCooldownsDays, "Tage"}
                    };
                    break;

                case "pl_PL":
                    data = new Dictionary<Text, string>()
                    {
                        {Text.Vocabulary, "Słownictwo"},
                        {Text.Adjectives, "Przymiotniki:"},
                        {Text.Nouns, "Rzeczowniki:"},
                        {Text.Others, "Inne:"},
                        {Text.Verbs, "Czasowniki:"},
                        {Text.Total, "Razem:"},
                        {Text.Flashcards, "Fiszki"},
                        {Text.FlashcardsAvailable, "Dostępne"},
                        {Text.FlashcardsAvailableNow, "Teraz:"},
                        {Text.FlashcardsAvailableNext24Hours, "24 godziny:"},
                        {Text.FlashcardsAvailableNext7Days, "7 dni:"},
                        {Text.FlashcardsResults, "Wyniki"},
                        {Text.FlashcardsResultsToday, "Dzisiaj"},
                        {Text.FlashcardsResultsLast7Days, "7 dni"},
                        {Text.FlashcardsResultsLast30Days, "30 dni"},
                        {Text.FlashcardsResultsLifetime, "Cały czas"},
                        {Text.FlashcardsResultsPracticed, "Przećwiczone:"},
                        {Text.FlashcardsResultsPracticedSuccessfully, "Dobrze:"},
                        {Text.FlashcardsResultsPracticedUnsuccessfully, "Źle:"},
                        {Text.FlashcardsCooldowns, "Czasy odnowienia"},
                        {Text.FlashcardsCooldownsDays, "Dni"}
                    };
                    break;

                default:
                    throw new ArgumentException($"Invalid language value: {Config.Instance.Language}");
            }


            var consoleStatistics = new ConsoleStatistics(statistics, data);
            consoleStatistics.DisplayAll();
        }

        private static void ClearCache()
        {
            File.Delete(Config.Instance.VerbEndingsCacheFilePath);
        }
    }
}
