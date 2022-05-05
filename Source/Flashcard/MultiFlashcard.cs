using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class MultiFlashcard : Flashcard
    {
        public class Raw
        {
            public int ParentId { get; set; }
            public FlashcardType Type { get; set; }
            public DateTime LastTrainingTime { get; set; }
            public TimeSpan Cooldown { get; set; }
            public List<Result> Results { get; set; } = new List<Result>();
            public List<string> Questions { get; set; } = new List<string>();
            public List<string> Answers { get; set; } = new List<string>();
        }

        private readonly static Random _random = new();

        private int _index;
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }

        public MultiFlashcard(int parentId, FlashcardType type, List<string> questions, List<string> answers) : base(parentId, type)
        {
            if (questions.Count == 0)
                throw new ArgumentException("questions needs to have at least 1 element.", nameof(questions));

            if (answers.Count == 0)
                throw new ArgumentException("answers needs to have at least 1 element.", nameof(answers));

            if (questions.Count != answers.Count)
                throw new Exception("questions and answers need to have the same amount of elements.");

            _index = _random.Next(0, questions.Count);
            Questions = questions;
            Answers = answers;
        }

        public override (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            var isCorrect = Answers[_index] == answer;

            Results.Add(new Result(isCorrect, LastTrainingTime));

            if (isCorrect && Cooldown.Days < Config.Instance.MaximalFlashcardCooldownInDays)
                Cooldown *= 2;
            else
            {
                if (Cooldown.Days > Config.Instance.MinimalFlashcardCooldownInDays)
                    Cooldown /= 2;
            }

            var correctAnswer = Answers[_index];
            _index = _random.Next(0, Questions.Count);

            return (isCorrect, correctAnswer);
        }

        public override string AskQuestion() => Questions[_index];

        public override string ComputeHash() => Utility.ComputeHash($"{string.Join("", Questions)}{string.Join("", Answers)}");
    }
}
