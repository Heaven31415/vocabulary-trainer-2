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

        public SheetsDownloader(string credentialsFilePath, string userCredentialDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(credentialsFilePath))
                throw new ArgumentException("credentialsFilePath cannot be null, empty or whitespace.", nameof(credentialsFilePath));

            var extension = Path.GetExtension(credentialsFilePath);

            if (extension == null || extension != ".json")
                throw new ArgumentException("credentialsFilePath needs to have a .json extension.", nameof(credentialsFilePath));

            if (string.IsNullOrWhiteSpace(userCredentialDirectoryPath))
                throw new ArgumentException("userCredentialDirectoryPath cannot be null, empty or whitespace.", nameof(userCredentialDirectoryPath));

            using var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read);

            _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new string[] { DriveService.Scope.DriveReadonly },
                "User",
                CancellationToken.None,
                new FileDataStore(userCredentialDirectoryPath, true)).Result;

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _userCredential
            });

            _mediaDownloader = new MediaDownloader(_driveService);
        }

        public void Download(string key, string sheetName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("key cannot be null, empty or whitespace.", nameof(key));

            if (string.IsNullOrWhiteSpace(sheetName))
                throw new ArgumentException("sheetName cannot be null, empty or whitespace.", nameof(sheetName));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null, empty or whitespace.", nameof(fileName));

            var extension = Path.GetExtension(fileName);

            if (extension == null || extension != ".csv")
                throw new ArgumentException("fileName needs to have a .csv extension.", nameof(fileName));

            var url = $"https://docs.google.com/spreadsheets/d/{key}/gviz/tq?tqx=out:csv&sheet={sheetName}";

            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _mediaDownloader.Download(url, stream);
        }
    }
}
