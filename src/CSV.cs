using CsvHelper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Globalization;

namespace VocabularyTrainer2
{
    internal class CSV
    {
        public static void DownloadSheetsFromGoogleDriveFolder()
        {
            UserCredential credential;

            using (var stream = new FileStream(Config.CredentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new string[] { DriveService.Scope.DriveReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(Config.UserCredentialPath, true)).Result;

                Utility.WriteLine($"Successfully saved user credential");
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Config.ProgramName,
            });

            var key = Config.SpreadsheetKey;
            var names = Config.SpreadsheetNames;
            var downloader = new MediaDownloader(service);

            foreach (var name in names)
            {
                var url = $"https://docs.google.com/spreadsheets/d/{key}/gviz/tq?tqx=out:csv&sheet={name}";
                using var stream = new FileStream($"data/{name}.csv", FileMode.Create, FileAccess.Write);
                downloader.Download(url, stream);
                Utility.WriteLine($"Downloaded '{name}.csv'");
            }
        }

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
