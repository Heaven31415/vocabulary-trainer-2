using CsvHelper;
using System.Globalization;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class Other
    {
        public int Id { get; }
        public string Question { get; }
        public string Answer { get; }
        public string? Bonus { get; }

        public Other(int id, string question, string answer, string? bonus)
        {
            Id = id;
            Question = question;
            Answer = answer;
            Bonus = bonus;
        }

        public static List<Other> ReadAllFromCsvFile()
        {
            using var streamReader = new StreamReader(Config.Instance.OthersCsvFilePath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            var others = new List<Other>();

            for (var id = 2; csvReader.Read(); id += 10)
            {
                if (!csvReader.GetField<bool>(0))
                    continue;

                var question = csvReader.GetField<string>(1);
                var answer = csvReader.GetField<string>(2);
                var bonus = csvReader.GetField<string>(3);

                ValidateQuestion(question);
                ValidateAnswer(answer);

                if (bonus.Length == 0)
                    bonus = null;

                others.Add(new Other(id, question, answer, bonus));
            }

            return others;
        }

        private static void ValidateQuestion(string question)
        {
            if (question.Length == 0)
                throw new ArgumentException("Other question cannot be empty.");
        }

        private static void ValidateAnswer(string answer)
        {
            if (answer.Length == 0)
                throw new ArgumentException("Other answer cannot be empty.");
        }
    }
}
