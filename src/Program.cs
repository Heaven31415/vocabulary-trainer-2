namespace VocabularyTrainer2
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = Config.ProgramName;

            try
            {
                var flashcardSet = new FlashcardSet();
                var statistics = new Statistics(Config.StatisticsPath);

                while (true)
                {
                    Utility.WriteLine(flashcardSet.GetVocabularyInformation());
                    Utility.WriteLine(flashcardSet.GetFlashcardInformation()); ;
                    Utility.WriteLine();

                    var flashcard = flashcardSet.GetRandomFlashcard();

                    if (flashcard == null)
                    {
                        Utility.WriteLine("Congratulations! You have practiced everything for the moment!", ConsoleColor.Green);
                        break;
                    }

                    Utility.Write($"Translate to German '{flashcard.AskQuestion()}': ");
                    var answer = Utility.ReadLine();

                    var (isCorrect, correctAnswer) = flashcard.AnswerQuestion(answer);

                    Utility.WriteLine();

                    if (isCorrect)
                        Utility.WriteLine("Correct!", ConsoleColor.Green);
                    else
                        Utility.WriteLine($"Incorrect! The correct answer is: '{correctAnswer}'.", ConsoleColor.Red);

                    statistics.OnQuestionAnswered(isCorrect);

                    Utility.WriteLine();
                    Utility.Write("Press enter to continue... ");
                    Utility.ReadLine();
                    Console.Clear();

                    flashcardSet.Save();
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLine(ex.Message, ConsoleColor.Red);
            }
        }
    }
}
