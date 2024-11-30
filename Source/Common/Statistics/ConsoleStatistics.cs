namespace VocabularyTrainer2.Source.Common.Statistics
{
    public class ConsoleStatistics
    {
        private readonly Statistics _statistics;
        private readonly Dictionary<Text, string> _data;

        public ConsoleStatistics(Statistics statistics, Dictionary<Text, string> data)
        {
            _statistics = statistics;
            _data = data;
        }

        public void DisplayAll()
        {
            var indentation = 2;

            DisplayVocabularyStatistics(indentation);
            DisplayFlashcardsStatistics(indentation);
            DisplayFlashcardsResultsStatistics(indentation);
            DisplayFlashcardsCooldowns(indentation);
        }

        private void DisplayVocabularyStatistics(int indentation)
        {
            var textPadding = Utility.CommonPadding(new[] {
                _data[Text.Vocabulary],
                _data[Text.Adjectives],
                _data[Text.Nouns],
                _data[Text.Others],
                _data[Text.Verbs],
                _data[Text.Total]
            });

            var adjectivesCount = _statistics.AdjectivesCount.ToString();
            var nounsCount = _statistics.NounsCount.ToString();
            var othersCount = _statistics.OthersCount.ToString();
            var verbsCount = _statistics.VerbsCount.ToString();
            var totalCount = _statistics.VocabularyCount.ToString();

            var valuePadding = Utility.CommonPadding(new[] {
                adjectivesCount,
                nounsCount,
                othersCount,
                verbsCount,
                totalCount
            });

            Utility.WriteLine(ConsoleColor.Blue, _data[Text.Vocabulary]);

            foreach (var (text, value) in new Dictionary<Text, string>
            {
                {Text.Adjectives, adjectivesCount},
                {Text.Nouns, nounsCount},
                {Text.Others, othersCount},
                {Text.Verbs, verbsCount},
                {Text.Total, totalCount},
            })
            {
                Utility.Space(indentation);
                Utility.Write(ConsoleColor.White, _data[text].PadRight(textPadding));
                Utility.Space(1);
                Utility.WriteLine(ConsoleColor.White, value.PadRight(valuePadding));
            }
        }

        private void DisplayFlashcardsStatistics(int indentation)
        {
            var textPadding = Utility.CommonPadding(new[] {
                _data[Text.FlashcardsAvailableNow],
                _data[Text.FlashcardsAvailableNext24Hours],
                _data[Text.FlashcardsAvailableNext7Days]
            });

            var flashcardsCount = _statistics.FlashcardsCount.ToString();
            var flashcardsAvailableNow = _statistics.FlashcardsAvailableNow.ToString();
            var flashcardsAvailableNowPercentage = $"{_statistics.FlashcardsAvailableNowPercentage:00.00}%";
            var flashcardsAvailableNext24Hours = _statistics.FlashcardsAvailableNext24Hours.ToString();
            var flashcardsAvailableNext24HoursPercentage = $"{_statistics.FlashcardsAvailableNext24HoursPercentage:00.00}%";
            var flashcardsAvailableNext7Days = _statistics.FlashcardsAvailableNext7Days.ToString();
            var flashcardsAvailableNext7DaysPercentage = $"{_statistics.FlashcardsAvailableNext7DaysPercentage:00.00}%";

            var valuePadding = Utility.CommonPadding(new[] {
                flashcardsAvailableNow,
                flashcardsAvailableNext24Hours,
                flashcardsAvailableNext7Days
            });

            Utility.WriteLine(ConsoleColor.Blue, _data[Text.Flashcards]);

            Utility.Space(indentation);
            Utility.WriteLine(ConsoleColor.Cyan, _data[Text.FlashcardsAvailable]);

            foreach (var (text, (value, percentage)) in new Dictionary<Text, (string, string)>
            {
                {Text.FlashcardsAvailableNow, (flashcardsAvailableNow, flashcardsAvailableNowPercentage)},
                {Text.FlashcardsAvailableNext24Hours, (flashcardsAvailableNext24Hours, flashcardsAvailableNext24HoursPercentage)},
                {Text.FlashcardsAvailableNext7Days, (flashcardsAvailableNext7Days, flashcardsAvailableNext7DaysPercentage)}
            })
            {
                Utility.Space(indentation * 2);
                Utility.Write(ConsoleColor.White, _data[text].PadRight(textPadding));
                Utility.Space(1);
                Utility.Write(ConsoleColor.White, value.PadRight(valuePadding));
                Utility.Write(ConsoleColor.White, " / ");
                Utility.Write(ConsoleColor.White, flashcardsCount);
                Utility.Space(1);
                Utility.WriteLine(ConsoleColor.White, $"({percentage})");
            }
        }

        private void DisplayFlashcardsResultsStatistics(int indentation)
        {
            var today = _statistics.FlashcardsToday;
            var last7Days = _statistics.FlashcardsLast7Days;
            var last30Days = _statistics.FlashcardsLast30Days;
            var lifetime = _statistics.FlashcardsLifetime;

            var valuePadding = Utility.CommonPadding(new[] {
                " ".Repeat(Utility.CommonPadding(today)),
                " ".Repeat(Utility.CommonPadding(last7Days)),
                " ".Repeat(Utility.CommonPadding(last30Days)),
                " ".Repeat(Utility.CommonPadding(lifetime))
            });

            Utility.Space(indentation);
            Utility.WriteLine(ConsoleColor.Cyan, _data[Text.FlashcardsResults]);

            Utility.Space(indentation * 2);
            DisplayFlashcardsResults(Text.FlashcardsResultsToday, today, valuePadding, indentation * 3);

            Utility.Space(indentation * 2);
            DisplayFlashcardsResults(Text.FlashcardsResultsLast7Days, last7Days, valuePadding, indentation * 3);

            Utility.Space(indentation * 2);
            DisplayFlashcardsResults(Text.FlashcardsResultsLast30Days, last30Days, valuePadding, indentation * 3);

            Utility.Space(indentation * 2);
            DisplayFlashcardsResults(Text.FlashcardsResultsLifetime, lifetime, valuePadding, indentation * 3);
        }

        private void DisplayFlashcardsResults(Text text, FlashcardsStatistics statistics, int valuePadding, int indentation)
        {
            var textPadding = Utility.CommonPadding(new[] {
                _data[Text.FlashcardsResultsPracticed],
                _data[Text.FlashcardsResultsPracticedSuccessfully],
                _data[Text.FlashcardsResultsPracticedUnsuccessfully]
            });

            var practiced = statistics.Practiced.ToString();
            var practicedSuccessfully = statistics.PracticedSuccessfully.ToString();
            var practicedSuccessfullyPercentage = $"{statistics.PracticedSuccessfullyPercentage:00.00}";
            var practicedUnsucessfully = statistics.PracticedUnsuccessfully.ToString();
            var practicedUnsucessfullyPercentage = $"{statistics.PracticedUnsuccessfullyPercentage:00.00}";

            Utility.WriteLine(ConsoleColor.Yellow, _data[text]);

            Utility.Space(indentation);
            Utility.Write(ConsoleColor.White, _data[Text.FlashcardsResultsPracticed].PadRight(textPadding));
            Utility.Space(1);
            Utility.WriteLine(ConsoleColor.White, practiced.PadLeft(valuePadding));

            Utility.Space(indentation);
            Utility.Write(ConsoleColor.White, _data[Text.FlashcardsResultsPracticedSuccessfully].PadRight(textPadding));
            Utility.Space(1);
            Utility.Write(ConsoleColor.Green, practicedSuccessfully.PadLeft(valuePadding));
            Utility.Space(1);
            Utility.WriteLine(ConsoleColor.White, $"({practicedSuccessfullyPercentage}%)");

            Utility.Space(indentation);
            Utility.Write(ConsoleColor.White, _data[Text.FlashcardsResultsPracticedUnsuccessfully].PadRight(textPadding));
            Utility.Space(1);
            Utility.Write(ConsoleColor.Red, practicedUnsucessfully.PadLeft(valuePadding));
            Utility.Space(1);
            Utility.WriteLine(ConsoleColor.White, $"({practicedUnsucessfullyPercentage}%)");
        }

        private void DisplayFlashcardsCooldowns(int indentation)
        {
            var cooldowns = _statistics.FlashcardsCooldowns;
            var cooldownsPercentages = _statistics.FlashcardsCooldownsPercentages;

            var daysPadding = Utility.CommonPadding(cooldowns.Keys.ToList().ConvertAll(c => c.Days));
            var countPadding = Utility.CommonPadding(cooldowns.Values);

            Utility.Space(indentation);
            Utility.WriteLine(ConsoleColor.Cyan, _data[Text.FlashcardsCooldowns]);

            Utility.Space(indentation * 2);
            Utility.WriteLine(ConsoleColor.Yellow, _data[Text.FlashcardsCooldownsDays]);

            foreach (var cooldown in cooldowns.Keys.OrderBy(item => item))
            {
                Utility.Space(indentation * 3);
                Utility.Write(ConsoleColor.White, cooldown.Days.ToString().PadLeft(daysPadding));
                Utility.Write(ConsoleColor.White, ": ");
                Utility.Write(ConsoleColor.White, cooldowns[cooldown].ToString().PadLeft(countPadding));
                Utility.Space(1);
                Utility.WriteLine(ConsoleColor.White, $"({cooldownsPercentages[cooldown]:00.00}%)");
            }
        }
    }
}
