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
        public string? ComparativeDegree { get; }
        public string? SuperlativeDegree { get; }
        public string? Bonus { get; }

        public Adjective(int id, string description, string positiveDegree, string? comparativeDegree, string? superlativeDegree, string? bonus)
        {
            Id = id;
            Description = description;

            if (string.IsNullOrEmpty(comparativeDegree) != string.IsNullOrEmpty(superlativeDegree))
                throw new Exception("Adjective comparative degree and superlative degree have to be null or non-null at the same time");

            PositiveDegree = positiveDegree;
            ComparativeDegree = comparativeDegree;
            SuperlativeDegree = superlativeDegree;
            Bonus = bonus;
        }

        public static List<Adjective> ReadAllFromCsvFile()
        {
            using var streamReader = new StreamReader(Config.Instance.AdjectivesCsvFilePath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            var adjectives = new List<Adjective>();

            for (var id = 0; csvReader.Read(); id += 10)
            {
                if (!csvReader.GetField<bool>(0))
                    continue;

                var description = csvReader.GetField<string>(1);
                var positiveDegree = csvReader.GetField<string>(2);
                var comparativeDegree = csvReader.GetField<string>(3);
                var superlativeDegree = csvReader.GetField<string>(4);
                var bonus = csvReader.GetField<string>(5);

                ValidateDescription(description);
                ValidatePositiveDegree(positiveDegree);
                ValidateComparativeDegree(comparativeDegree);
                ValidateSuperlativeDegree(superlativeDegree);

                if (comparativeDegree.Length == 0)
                    comparativeDegree = null;

                if (superlativeDegree.Length == 0)
                    superlativeDegree = null;

                if (bonus.Length == 0)
                    bonus = null;

                adjectives.Add(new Adjective(id, description, positiveDegree, comparativeDegree, superlativeDegree, bonus));
            }

            return adjectives;
        }

        private static void ValidateDescription(string description)
        {
            if (description.Length == 0)
                throw new ArgumentException("Adjective description cannot be empty");

            if (!description.IsLower())
                throw new ArgumentException("Adjective description needs to be lowercase");
        }

        private static void ValidatePositiveDegree(string positiveDegree)
        {
            if (positiveDegree.Length == 0)
                throw new ArgumentException("Adjective positive degree cannot be empty");

            if (!positiveDegree.IsLower())
                throw new ArgumentException("Adjective positive degree needs to be lowercase");
        }

        private static void ValidateComparativeDegree(string comparativeDegree)
        {
            if (comparativeDegree.Length == 0)
                return;

            if (!comparativeDegree.IsLower())
                throw new ArgumentException("Adjective comparative degree needs to be lowercase");
        }

        private static void ValidateSuperlativeDegree(string superlativeDegree)
        {
            if (superlativeDegree.Length == 0)
                return;

            if (!superlativeDegree.IsLower())
                throw new ArgumentException("Adjective superlative degree needs to be lowercase");

            var parts = superlativeDegree.Split(' ');

            if (parts.Length != 2)
                throw new ArgumentException($"Superlative degree should be made of 2 parts instead of {parts.Length}");

            if (parts[0] != "am")
                throw new ArgumentException("Superlative degree first part should be equal to 'am'");
        }
    }
}
