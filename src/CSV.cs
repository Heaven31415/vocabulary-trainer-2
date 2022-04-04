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
                string description = csv.GetField(1);
                string? germanSingularForm = csv.GetField(2);
                string? germanPluralForm = csv.GetField(3);
                // string? exampleSentence = csv.GetField(4);

                if (germanSingularForm == "-")
                    germanSingularForm = null;

                if (germanPluralForm == "-")
                    germanPluralForm = null;

                nouns.Add(new Noun(id, description, germanSingularForm, germanPluralForm));
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
                string description = csv.GetField(1);
                string positiveDegree = csv.GetField(2);
                string comparativeDegree = csv.GetField(3);
                string superlativeDegree = csv.GetField(4);
                // string? exampleSentence = csv.GetField(5);

                if (comparativeDegree == "-" && superlativeDegree == "-")
                    adjectives.Add(new Adjective(id, description, positiveDegree));
                else
                    adjectives.Add(new Adjective(id, description, positiveDegree, comparativeDegree, superlativeDegree));
            }

            return adjectives;
        }

        public static List<Verb> ReadVerbsFromFile(string path)
        {
            var verbs = new List<Verb>();
            var verbCache = new VerbCache(Config.VerbCachePath);

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                int id = csv.GetField<int>(0);
                string description = csv.GetField(1);
                string germanInfinitive = csv.GetField(2);
                // string? exampleSentence = csv.GetField(3);

                verbs.Add(verbCache.Get(id, description, germanInfinitive));
            }

            return verbs;
        }
    }
}
