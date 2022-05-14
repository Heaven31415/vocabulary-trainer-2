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
            if (!File.Exists(fileName))
                _verbEndings = new Dictionary<string, List<VerbEndings>>();
            else
            {
                var json = File.ReadAllText(fileName);
                var verbEndings = JsonSerializer.Deserialize<Dictionary<string, List<VerbEndings>>>(json);

                if (verbEndings == null)
                    throw new IOException($"File '{fileName}' contains invalid content. Unable to deserialize it.");

                _verbEndings = verbEndings;
            }

            _fileName = fileName;

            Utility.SaveToFileAsJson(_fileName, _verbEndings);
        }

        public List<VerbEndings> Get(string infinitive, int controlCode)
        {
            if (_verbEndings.ContainsKey(infinitive))
                return _verbEndings[infinitive];

            var allVerbEndings = VerbEndingsDownloader.Download(infinitive, controlCode);
            _verbEndings.Add(infinitive, allVerbEndings);

            Utility.SaveToFileAsJson(_fileName, _verbEndings);

            return allVerbEndings;
        }
    }
}
