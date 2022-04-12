using System.Text;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Flashcard;

namespace VocabularyTrainer2.Source
{
    public class Application
    {
        private readonly SheetsDownloader _sheetsDownloader;
        private readonly FlashcardSet _flashcardSet;

        public Application()
        {
            Console.Title = Config.ProgramName;
            Console.OutputEncoding = Encoding.UTF8;

            _sheetsDownloader = new SheetsDownloader(Config.CredentialsFilePath, Config.UserCredentialDirectoryPath);
            DownloadSheets();

            _flashcardSet = new FlashcardSet();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine(_flashcardSet.VocabularyInformation);
                Console.WriteLine(_flashcardSet.FlashcardInformation);
                Console.WriteLine();

                var (flashcard, exampleSentence) = _flashcardSet.GetRandomFlashcard();

                if (flashcard == null)
                {
                    Utility.WriteGreenLine("Congratulations! You have practiced everything for the moment!");
                    Console.WriteLine();
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
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
                Console.WriteLine($"Example Sentence: '{exampleSentence}'");
                Console.WriteLine();
                Console.Write("Press enter to continue...");
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
