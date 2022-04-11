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
        public string ExampleSentence { get; }

        public Other(int id, string question, string answer, string exampleSentence)
        {
            Id = id;
            Question = question;
            Answer = answer;
            ExampleSentence = exampleSentence;
        }

        public static List<Other> ReadOthersFromCsvFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null, empty or whitespace.", nameof(fileName));

            var extension = Path.GetExtension(fileName);

            if (extension == null || extension != ".csv")
                throw new ArgumentException("fileName needs to have a .csv extension.", nameof(fileName));

            using var streamReader = new StreamReader(fileName);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            var others = new List<Other>();
            var id = Config.MinimalOtherId;

            while (csvReader.Read())
            {
                var verified = csvReader.GetField<bool>(0);

                if (!verified)
                {
                    id++;
                    continue;
                }

                var question = csvReader.GetField<string>(1);

                if (string.IsNullOrWhiteSpace(question))
                    throw new IOException("question cannot be null, empty or whitespace.");

                var answer = csvReader.GetField<string>(2);

                if (string.IsNullOrWhiteSpace(answer))
                    throw new IOException("answer cannot be null, empty or whitespace.");

                var exampleSentence = csvReader.GetField<string>(3);

                if (string.IsNullOrWhiteSpace(exampleSentence))
                    throw new IOException("exampleSentence cannot be null, empty or whitespace.");

                others.Add(new Other(id, question, answer, exampleSentence));
                id++;
            }

            return others;
        }
    }
}
