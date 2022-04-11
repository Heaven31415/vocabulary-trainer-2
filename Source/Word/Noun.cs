using CsvHelper;
using System.Globalization;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class Noun
    {
        public int Id { get; }
        public string Description { get; }
        public string? SingularForm { get; }
        public string? PluralForm { get; }
        public string ExampleSentence { get; }

        public Noun(int id, string description, string? singularForm, string? pluralForm, string exampleSentence)
        {
            Id = id;
            Description = description;

            if (singularForm == null && pluralForm == null)
                throw new Exception("Singular and plural form cannot be null at the same time.");

            SingularForm = singularForm;
            PluralForm = pluralForm;
            ExampleSentence = exampleSentence;
        }

        public static List<Noun> ReadNounsFromCsvFile(string fileName)
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

            var nouns = new List<Noun>();
            var id = Config.MinimalNounId;

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

                var singularForm = csvReader.GetField<string>(2);

                if (string.IsNullOrWhiteSpace(singularForm))
                    throw new IOException("singularForm cannot be null, empty or whitespace.");

                var pluralForm = csvReader.GetField<string>(3);

                if (string.IsNullOrWhiteSpace(pluralForm))
                    throw new IOException("pluralForm cannot be null, empty or whitespace.");

                var exampleSentence = csvReader.GetField<string>(4);

                if (string.IsNullOrWhiteSpace(exampleSentence))
                    throw new IOException("exampleSentence cannot be null, empty or whitespace.");

                if (singularForm == "-")
                    singularForm = null;

                if (pluralForm == "-")
                    pluralForm = null;

                nouns.Add(new Noun(id, description, singularForm, pluralForm, exampleSentence));
                id++;
            }

            return nouns;
        }
    }
}
