using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source
{
    public class Application
    {
        private readonly SheetsDownloader _sheetsDownloader;
        private readonly Flashcard.FlashcardSet _flashcardSet;

        public Application()
        {
            Console.Title = Config.ProgramName;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            _sheetsDownloader = new SheetsDownloader(Config.CredentialsFilePath, Config.UserCredentialDirectoryPath);
            DownloadSheets();

            _flashcardSet = new Flashcard.FlashcardSet();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine(_flashcardSet.VocabularyInformation);
                Console.WriteLine(_flashcardSet.FlashcardInformation);
                Console.WriteLine();

                var flashcard = _flashcardSet.GetRandomFlashcard();

                if (flashcard == null)
                {
                    Utility.WriteGreenLine("Congratulations! You have practiced everything for the moment!");
                    break;
                }

                Console.Write($"Translate to German '{flashcard.AskQuestion()}': ");
                var answer = Utility.ReadLine();

                var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

                Console.WriteLine();

                if (isCorrect)
                    Utility.WriteGreenLine("Correct!");
                else
                    Utility.WriteRedLine($"Incorrect! The correct answer is: '{correctAnswer}'.");


                Console.WriteLine();
                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();

                _flashcardSet.SaveToFileAsJson();
            }
        }

        private void DownloadSheets()
        {
            foreach (var sheetName in Config.SpreadsheetNames)
                _sheetsDownloader.Download(Config.SpreadsheetKey, sheetName, $"Data/{sheetName}.csv");
        }
    }
}
