using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class VerbCache
    {
        private readonly string path;
        private readonly Dictionary<string, Verb> verbs;

        public VerbCache(string path)
        {
            var json = File.ReadAllText(path);
            var verbs = JsonSerializer.Deserialize<Dictionary<string, Verb>>(json);

            if (verbs == null)
                throw new Exception($"Unable to deserialize verbs from this path: '{path}'.");

            this.path = path;
            this.verbs = verbs;
        }

        public Verb Get(int id, string englishDescription, string germanVerbInfinitive)
        {
            if (verbs.ContainsKey(germanVerbInfinitive))
                return verbs[germanVerbInfinitive];

            var allVerbEndings = VerbDownloader.Download(germanVerbInfinitive);

            var verb = new Verb(id, englishDescription)
            {
                Present = allVerbEndings[0],
                SimplePast = allVerbEndings[1],
                Perfekt = allVerbEndings[2]
            };

            verbs.Add(germanVerbInfinitive, verb);
            Save();

            return verb;
        }

        private void Save()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(verbs, options);
            File.WriteAllText(path, json);
        }
    }
}
