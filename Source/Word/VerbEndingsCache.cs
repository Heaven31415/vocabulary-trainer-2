using System.Text.Json;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class VerbEndingsCache
    {
        private readonly string _fileName;
        private readonly Dictionary<string, List<VerbEndings>> _verbEndings;

        public VerbEndingsCache(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null, empty or whitespace.", nameof(fileName));

            var extension = Path.GetExtension(fileName);

            if (extension == null || extension != ".json")
                throw new ArgumentException("fileName needs to have a .json extension.", nameof(fileName));

            if (!File.Exists(fileName))
                _verbEndings = new Dictionary<string, List<VerbEndings>>();
            else
            {
                var json = File.ReadAllText(fileName);
                var verbEndings = JsonSerializer.Deserialize<Dictionary<string, List<VerbEndings>>>(json);

                if (verbEndings == null)
                    throw new IOException($"{fileName} contains invalid content. Unable to deserialize it.");

                _verbEndings = verbEndings;
            }

            _fileName = fileName;

            SaveToFileAsJson();
        }

        public List<VerbEndings> Get(string infinitive, List<int> controlCodes)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("infinitive cannot be null, empty or whitespace.", nameof(infinitive));

            if (_verbEndings.ContainsKey(infinitive))
                return _verbEndings[infinitive];

            var allVerbEndings = VerbEndingsDownloader.Download(infinitive, controlCodes);
            _verbEndings.Add(infinitive, allVerbEndings);

            SaveToFileAsJson();

            return allVerbEndings;
        }

        private void SaveToFileAsJson() => Utility.SaveToFileAsJson(_fileName, _verbEndings);
    }
}
