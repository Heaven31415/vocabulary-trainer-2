using CsvHelper;
using System.Globalization;

namespace VocabularyTrainer2
{
    internal class CSV
    {
        public static List<Noun> ReadNounsFromFile(string path)
        {
            var nouns = new List<Noun>();

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                int id = csv.GetField<int>(0);
                string englishDescription = csv.GetField(1);
                string? germanSingularForm = csv.GetField(2);
                string? germanPluralForm = csv.GetField(3);

                if (string.IsNullOrEmpty(germanSingularForm))
                    germanSingularForm = null;

                if (string.IsNullOrEmpty(germanPluralForm))
                    germanPluralForm = null;

                var noun = new Noun(id, englishDescription, germanSingularForm, germanPluralForm);
                nouns.Add(noun);
            }

            return nouns;
        }

        public static List<Adjective> ReadAdjectivesFromFile(string path)
        {
            var adjectives = new List<Adjective>();

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                int id = csv.GetField<int>(0);
                string englishDescription = csv.GetField(1);
                string positiveDegree = csv.GetField(2);
                string? comparativeDegree = csv.GetField(3);
                string? superlativeDegree = csv.GetField(4);

                if (string.IsNullOrEmpty(comparativeDegree) && string.IsNullOrEmpty(superlativeDegree))
                    adjectives.Add(new Adjective(id, englishDescription, positiveDegree));
                else
                    adjectives.Add(new Adjective(id, englishDescription, positiveDegree, comparativeDegree, superlativeDegree));
            }

            return adjectives;
        }
    }
}
