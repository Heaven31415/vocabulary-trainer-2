namespace VocabularyTrainer2.Source.Common.Statistics
{
    public class FlashcardsStatistics
    {
        public int Practiced { get; }
        public int PracticedSuccessfully { get; }
        public int PracticedUnsuccessfully { get; }
        public float PracticedSuccessfullyPercentage { get; }
        public float PracticedUnsuccessfullyPercentage { get; }

        public FlashcardsStatistics(List<Flashcard.Flashcard> flashcards, Func<Flashcard.Flashcard.Result, bool> predicate)
        {
            foreach (var flashcard in flashcards)
            {
                foreach (var result in flashcard.Results)
                {
                    if (!predicate(result))
                        continue;

                    if (result.IsSuccessful)
                        PracticedSuccessfully++;
                    else
                        PracticedUnsuccessfully++;

                    Practiced++;
                }
            }

            PracticedSuccessfullyPercentage = 0f;
            PracticedUnsuccessfullyPercentage = 0f;

            if (Practiced != 0)
            {
                PracticedSuccessfullyPercentage = 100f * PracticedSuccessfully / Practiced;
                PracticedUnsuccessfullyPercentage = 100f * PracticedUnsuccessfully / Practiced;
            }
        }
    }
}
