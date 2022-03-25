namespace VocabularyTrainer2
{
    internal class Flashcard : FlashcardBase
    {
        private readonly string question;
        private readonly string answer;

        public Flashcard(int parentId, Type type, string question, string answer) : base(parentId, type)
        {
            this.question = question;
            this.answer = answer;
        }

        public override (bool, string) Answer(string answer)
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

        public override string Ask() => question;

        public override string ComputeHash() => Utility.ComputeHash($"{question}{answer}");
    }
}
