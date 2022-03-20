using CsvHelper;
using System.Globalization;

namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Vocabulary Trainer 2";

            var nouns = ReadNounsFromCSVFile("data/Nouns.csv");

            foreach (var noun in nouns)
            {
                Utility.WriteLine($"{noun.EnglishDescription}");
            }
        }

        static List<Noun> ReadNounsFromCSVFile(string path)
        {
            var nouns = new List<Noun>();

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                string englishDescription = csv.GetField(0);
                string? germanSingularForm = csv.GetField(1);
                string? germanPluralForm = csv.GetField(2);

                if (string.IsNullOrEmpty(germanSingularForm))
                    germanSingularForm = null;

                if (string.IsNullOrEmpty(germanPluralForm))
                    germanPluralForm = null;

                var noun = new Noun(englishDescription, germanSingularForm, germanPluralForm);
                nouns.Add(noun);
            }

            return nouns;
        }
    }
}
