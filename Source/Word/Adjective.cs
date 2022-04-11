using CsvHelper;
using System.Globalization;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class Adjective
    {
        public int Id { get; }
        public string Description { get; }
        public string PositiveDegree { get; }
        public string? ComparativeDegree { get; } = null;
        public string? SuperlativeDegree { get; } = null;
        public string ExampleSentence { get; }

        public Adjective(int id, string description, string[] degrees, string exampleSentence)
        {
            Id = id;
            Description = description;

            switch (degrees.Length)
            {
                case 1:
                    PositiveDegree = degrees[0];
                    break;
                case 3:
                    PositiveDegree = degrees[0];
                    ComparativeDegree = degrees[1];
                    SuperlativeDegree = degrees[2];
                    break;
                default:
                    throw new ArgumentException($"degrees must contain 1 or 3 elements, not {degrees.Length}.", nameof(degrees));
            }

            ExampleSentence = exampleSentence;
        }

        public static List<Adjective> ReadAdjectivesFromCsvFile(string fileName)
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

            var adjectives = new List<Adjective>();
            var id = Config.MinimalAdjectiveId;

            while (csvReader.Read())
            {
                var verified = csvReader.GetField<bool>(0);

                if (!verified)
                {
                    id++;
                    continue;
                }

                var description = csvReader.GetField<string>(1);

                if (string.IsNullOrWhiteSpace(description))
                    throw new IOException("description cannot be null, empty or whitespace.");

                var positiveDegree = csvReader.GetField<string>(2);

                if (string.IsNullOrWhiteSpace(positiveDegree))
                    throw new IOException("positiveDegree cannot be null, empty or whitespace.");

                var comparativeDegree = csvReader.GetField<string>(3);

                if (string.IsNullOrWhiteSpace(comparativeDegree))
                    throw new IOException("comparativeDegree cannot be null, empty or whitespace.");

                var superlativeDegree = csvReader.GetField<string>(4);

                if (string.IsNullOrWhiteSpace(superlativeDegree))
                    throw new IOException("superlativeDegree cannot be null, empty or whitespace.");

                var exampleSentence = csvReader.GetField<string>(5);

                if (string.IsNullOrWhiteSpace(exampleSentence))
                    throw new IOException("exampleSentence cannot be null, empty or whitespace.");

                string[] degrees;

                if (comparativeDegree == "-" && superlativeDegree == "-")
                    degrees = new string[] { positiveDegree };
                else
                    degrees = new string[] { positiveDegree, comparativeDegree, superlativeDegree };

                adjectives.Add(new Adjective(id, description, degrees, exampleSentence));
                id++;
            }

            return adjectives;
        }
    }
}
