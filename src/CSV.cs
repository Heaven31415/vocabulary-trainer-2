using CsvHelper;
using System.Globalization;

namespace VocabularyTrainer2
{
    internal class CSV
    {
        public static List<Noun> ReadNounsFromCSVFile(string path)
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
    }
}
