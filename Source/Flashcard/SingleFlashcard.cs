using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class SingleFlashcard : Flashcard
    {
        public class Raw
        {
            public int ParentId { get; set; }
            public FlashcardType Type { get; set; }
            public DateTime LastTrainingTime { get; set; }
            public TimeSpan Cooldown { get; set; }
            public List<Result> Results { get; set; } = new List<Result>();
            public string Question { get; set; } = "";
            public string Answer { get; set; } = "";
        }

        public string Question { get; set; }
        public string Answer { get; set; }

        public SingleFlashcard(int parentId, FlashcardType type, string question, string answer) : base(parentId, type)
        {
            Question = question;
            Answer = answer;
        }

        public override (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            if (Answer == answer && Cooldown.Days < Config.MaximalFlashcardCooldownInDays)
                Cooldown *= 2;
            else
            {
                if (Cooldown.Days > Config.MinimalFlashcardCooldownInDays)
                    Cooldown /= 2;
            }

            return (Answer == answer, Answer);
        }

        public override string AskQuestion() => Question;

        public override string ComputeHash() => Utility.ComputeHash($"{Question}{Answer}");
    }
}
