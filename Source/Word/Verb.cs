global using VerbEndings = System.Collections.Generic.Dictionary<VocabularyTrainer2.Source.Word.Verb.PersonalPronoun, string>;

using CsvHelper;
using System.Globalization;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class Verb
    {
        public enum PersonalPronoun
        {
            FirstSingular = 0,
            SecondSingular = 1,
            ThirdSingular = 2,
            FirstPlural = 3,
            SecondPlural = 4,
            ThirdPlural = 5,
        }

        public int Id { get; }
        public string Description { get; }
        public VerbEndings Present { get; }
        public VerbEndings SimplePast { get; }
        public VerbEndings Perfekt { get; }
        public VerbEndings Imperative { get; }
        public string? Bonus { get; }

        public Verb(int id, string description, List<VerbEndings> endings, string? bonus = null)
        {
            Id = id;
            Description = description;

            if (endings.Count != 4)
                throw new ArgumentException($"Verb endings must have 4 elements, not {endings.Count}.");

            Present = endings[0];
            SimplePast = endings[1];
            Perfekt = endings[2];
            Imperative = endings[3];
            Bonus = bonus;
        }

        public static List<Verb> ReadAllFromCsvFile(VerbEndingsCache cache)
        {
            using var streamReader = new StreamReader(Config.Instance.VerbsCsvFilePath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();

            var verbs = new List<Verb>();

            for (var id = 3; csvReader.Read(); id += 10)
            {
                if (!csvReader.GetField<bool>(0))
                    continue;

                var description = csvReader.GetField<string>(1);
                var infinitive = csvReader.GetField<string>(2);
                var controlCode = csvReader.GetField<string>(3);

                ValidateDescription(description);
                ValidateInfinitive(infinitive);
                ValidateControlCode(controlCode);

                var allVerbEndings = cache.Get(infinitive, controlCode.Length == 0 ? 1111 : int.Parse(controlCode));

                if (allVerbEndings != null)
                    verbs.Add(new Verb(id, description, allVerbEndings));
            }

            return verbs;
        }

        private static void ValidateDescription(string description)
        {
            if (description.Length == 0)
                throw new ArgumentException("Verb description cannot be empty.");
        }

        private static void ValidateInfinitive(string infinitive)
        {
            if (infinitive.Length == 0)
                throw new ArgumentException("Verb infinitive cannot be empty.");

            if (!infinitive.IsLower())
                throw new ArgumentException("Verb infinitive needs to be lowercase.");
        }

        private static void ValidateControlCode(string controlCode)
        {
            if (controlCode.Length != 0 && controlCode.Length != 4)
                throw new ArgumentException("Verb control code must be empty or have 4 characters.");

            foreach (var c in controlCode)
                if (c != '1' && c != '2')
                    throw new ArgumentException("Verb control code can only have '1' or '2' as characters.");
        }
    }
}
