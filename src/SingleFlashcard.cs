namespace VocabularyTrainer2
{
    internal class SingleFlashcard : Flashcard
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public SingleFlashcard(int parentId, Type type, string question, string answer) : base(parentId, type)
        {
            Question = question;
            Answer = answer;
        }

        public override (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            if (Answer == answer)
                Cooldown *= 2;
            else
            {
                if (Cooldown.Days > 1)
                    Cooldown /= 2;
            }

            return (Answer == answer, Answer);
        }

        public override string AskQuestion() => Question;

        public override string ComputeHash() => Utility.ComputeHash($"{Question}{Answer}");
    }
}
