namespace VocabularyTrainer2
{
    internal interface IFlashcardable
    {
        string AskQuestion();
        (bool, string) GiveAnswer(string answer);
        bool IsAvailable();
    }
}