namespace VocabularyTrainer2
{
    internal class Flashcard : IFlashcardable
    {
        private readonly string question;
        private readonly string answer;
        private DateTime lastTrainingTime;
        private TimeSpan cooldown;

        public Flashcard(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
            lastTrainingTime = DateTime.Now.AddDays(-1);
            cooldown = TimeSpan.FromDays(1);
        }

        public string AskQuestion() => question;

        public (bool, string) GiveAnswer(string answer)
        {
            lastTrainingTime = DateTime.Now;

            if (this.answer == answer)
                cooldown *= 2;
            else
            {
                if (cooldown.Days > 1)
                    cooldown /= 2;
            }

            return (this.answer == answer, this.answer);
        }

        public bool IsAvailable() => DateTime.Now > lastTrainingTime.Add(cooldown);
    }
}
