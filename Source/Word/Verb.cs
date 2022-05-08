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
        public VerbEndings? Imperative { get; } = null;

        public Verb(int id, string description, List<VerbEndings> allVerbEndings)
        {
            Id = id;
            Description = description;

            switch (allVerbEndings.Count)
            {
                case 3:
                    Present = allVerbEndings[0];
                    SimplePast = allVerbEndings[1];
                    Perfekt = allVerbEndings[2];
                    break;
                case 4:
                    Present = allVerbEndings[0];
                    SimplePast = allVerbEndings[1];
                    Perfekt = allVerbEndings[2];
                    Imperative = allVerbEndings[3];
                    break;
                default:
                    throw new ArgumentException($"allVerbEndings must contain 3 or 4 elements, not {allVerbEndings.Count}.", nameof(allVerbEndings));
            }
        }

        public static List<Verb> ReadVerbsFromCsvFile(string fileName, VerbEndingsCache cache)
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

            var verbs = new List<Verb>();
            var id = Config.Instance.MinimalVerbId;

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

                var infinitive = csvReader.GetField<string>(2);

                if (string.IsNullOrWhiteSpace(infinitive))
                    throw new IOException("infinitive cannot be null, empty or whitespace.");

                var controlCodes = ParseRawControlCodes(csvReader.GetField<string>(3));

                List<VerbEndings> allVerbEndings = cache.Get(infinitive, controlCodes);
                verbs.Add(new Verb(id, description, allVerbEndings));
                id++;
            }

            return verbs;
        }

        private static List<int> ParseRawControlCodes(string rawControlCodes)
        {
            if (string.IsNullOrWhiteSpace(rawControlCodes))
              throw new ArgumentException("rawControlCodes cannot be null, empty or whitespace.", nameof(rawControlCodes));

            if (rawControlCodes == "-")
                return new List<int> { 1, 1, 1, 1 };

            var splitControlCodes = rawControlCodes.Trim().Split(' ');

            if (splitControlCodes.Length != 8)
                throw new ArgumentException($"rawControlCodes must contain 8 elements, not {splitControlCodes.Length}.", nameof(rawControlCodes));

            if (splitControlCodes[0] != "Pre")
                throw new ArgumentException($"rawControlCodes contains invalid control code name. Expected 'Pre' got '{splitControlCodes[0]}'", nameof(rawControlCodes));

            var presentControlCode = int.Parse(splitControlCodes[1]);

            if (splitControlCodes[2] != "Sim")
                throw new ArgumentException($"rawControlCodes contains invalid control code name. Expected 'Sim' got '{splitControlCodes[2]}'", nameof(rawControlCodes));

            var simplePastControlCode = int.Parse(splitControlCodes[3]);

            if (splitControlCodes[4] != "Per")
                throw new ArgumentException($"rawControlCodes contains invalid control code name. Expected 'Per' got '{splitControlCodes[4]}'", nameof(rawControlCodes));

            var perfektControlCode = int.Parse(splitControlCodes[5]);

            if (splitControlCodes[6] != "Imp")
                throw new ArgumentException($"rawControlCodes contains invalid control code name. Expected 'Imp' got '{splitControlCodes[6]}'", nameof(rawControlCodes));

            var imperativeControlCode = int.Parse(splitControlCodes[7]);

            return new List<int> { presentControlCode, simplePastControlCode, perfektControlCode, imperativeControlCode };
        }
    }
}
