using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace VocabularyTrainer2.Source.Common
{
    public class SheetsDownloader
    {
        private readonly UserCredential _userCredential;
        private readonly DriveService _driveService;
        private readonly MediaDownloader _mediaDownloader;

        public SheetsDownloader()
        {
            using var stream = new FileStream(Config.Instance.CredentialsFilePath, FileMode.Open, FileAccess.Read);

            _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new string[] { DriveService.Scope.DriveReadonly },
                "User",
                CancellationToken.None,
                new FileDataStore(Config.Instance.UserCredentialDirectoryPath, true)).Result;

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _userCredential
            });

            _mediaDownloader = new MediaDownloader(_driveService);
        }

        private void Download(string key, string sheetName, string fileName)
        {
            var url = $"https://docs.google.com/spreadsheets/d/{key}/gviz/tq?tqx=out:csv&sheet={sheetName}";

            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _mediaDownloader.Download(url, stream);
        }

        public void DownloadAll()
        {
            foreach (var sheetName in Config.Instance.SpreadsheetNames)
                Download(Config.Instance.SpreadsheetKey, sheetName, $"Data/{sheetName}.csv");
        }
    }
}
