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
        public string? Bonus { get; }

        public Noun(int id, string description, string? singularForm, string? pluralForm, string? bonus)
        {
            Id = id;
            Description = description;

            if (singularForm == null && pluralForm == null)
                throw new Exception("Noun singular form and plural form cannot be null at the same time");

            SingularForm = singularForm;
            PluralForm = pluralForm;
            Bonus = bonus;
        }

        public static List<Noun> ReadAllFromCsvFile()
        {
            using var streamReader = new StreamReader(Config.Instance.NounsCsvFilePath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            var nouns = new List<Noun>();

            for (var id = 1; csvReader.Read(); id += 10)
            {
                if (!csvReader.GetField<bool>(0))
                    continue;

                var description = csvReader.GetField<string>(1);
                var singularForm = csvReader.GetField<string>(2);
                var pluralForm = csvReader.GetField<string>(3);
                var bonus = csvReader.GetField<string>(4);

                ValidateDescription(description);
                ValidateSingularForm(singularForm);
                ValidatePluralForm(pluralForm);

                if (singularForm.Length == 0)
                    singularForm = null;

                if (pluralForm.Length == 0)
                    pluralForm = null;

                if (bonus.Length == 0)
                    bonus = null;

                nouns.Add(new Noun(id, description, singularForm, pluralForm, bonus));
            }

            return nouns;
        }

        private static void ValidateDescription(string description)
        {
            if (description.Length == 0)
                throw new ArgumentException("Noun description cannot be empty");
        }

        private static void ValidateSingularForm(string singularForm)
        {
            if (singularForm.Length == 0)
                return;

            var parts = singularForm.Split(' ');

            if (parts.Length != 2)
                throw new ArgumentException($"Singular form noun should be made of 2 parts instead of {parts.Length}");

            var article = parts[0];
            var noun = parts[1];

            switch (article)
            {
                case "der":
                case "die":
                case "das":
                    break;
                default:
                    throw new ArgumentException($"Singular form article should be 'der', 'die' or 'das', not '{article}'");
            }

            if (!noun.IsCapitalized())
                throw new ArgumentException("Singular form noun should be capitalized");
        }

        private static void ValidatePluralForm(string pluralForm)
        {
            if (pluralForm.Length == 0)
                return;

            var parts = pluralForm.Split(' ');

            if (parts.Length != 2)
                throw new ArgumentException($"Plural form noun should be made of 2 parts instead of {parts.Length}");

            var article = parts[0];
            var noun = parts[1];

            if (article != "die")
                throw new ArgumentException($"Plural form article should be equal to 'die' instead of '{article}'");

            if (!noun.IsCapitalized())
                throw new ArgumentException("Plural forum noun should be capitalized");
        }
    }
}
