using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class Flashcard
    {
        public enum FlashcardType
        {
            AdjectivePositiveDegree = 0,
            AdjectiveComparativeDegree = 1,
            AdjectiveSuperlativeDegree = 2,
            NounSingularForm = 3,
            NounPluralForm = 4,
            Other = 5,
            VerbPresentFirstSingular = 6,
            VerbPresentSecondSingular = 7,
            VerbPresentThirdSingular = 8,
            VerbPresentFirstOrThirdPlural = 9,
            VerbPresentSecondPlural = 10,
            VerbSimplePastFirstSingular = 11,
            VerbSimplePastSecondSingular = 12,
            VerbSimplePastThirdSingular = 13,
            VerbSimplePastFirstOrThirdPlural = 14,
            VerbSimplePastSecondPlural = 15,
            VerbPerfekt = 16,
            VerbImperativeSecondSingular = 17,
            VerbImperativeSecondPlural = 18,
            VerbImperativeThirdPlural = 19,
        }

        public class Result
        {
            public bool IsSuccessful { get; }
            public DateTime Time { get; }

            public Result(bool isSuccessful, DateTime time)
            {
                IsSuccessful = isSuccessful;
                Time = time;
            }
        }

        public int ParentId { get; }
        public FlashcardType Type { get; }
        public List<Result> Results { get; set; } = new List<Result>();
        public DateTime LastTrainingTime { get; set; }
        public TimeSpan Cooldown { get; set; }
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }

        private readonly static Random _random = new();
        private int _index;

        public bool IsAvailable(DateTime? dateTime = null) => (dateTime != null ? dateTime : DateTime.Now) > LastTrainingTime.Add(Cooldown);

        public Flashcard(int parentId, FlashcardType type, List<string> questions, List<string> answers)
        {
            if (questions.Count == 0)
                throw new ArgumentException("Questions should have at least 1 element");

            if (answers.Count == 0)
                throw new ArgumentException("Answers should have at least 1 element");

            if (questions.Count != answers.Count)
                throw new Exception("Questions and answers should have the same amount of elements");

            ParentId = parentId;
            Type = type;
            LastTrainingTime = DateTime.Now - TimeSpan.FromDays(1) + TimeSpan.FromHours(Config.Instance.InitialFlashcardCooldownInHours);
            Cooldown = TimeSpan.FromDays(1);
            Questions = questions;
            Answers = answers;

            _index = _random.Next(0, questions.Count);
        }

        public (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            var isCorrect = Answers[_index] == answer;

            Results.Add(new Result(isCorrect, LastTrainingTime));

            if (isCorrect && Cooldown.Days < Config.Instance.MaximalFlashcardCooldownInDays)
                Cooldown *= 2;
            else if (!isCorrect && Cooldown.Days > Config.Instance.MinimalFlashcardCooldownInDays)
                Cooldown /= 2;

            var correctAnswer = Answers[_index];
            _index = _random.Next(0, Questions.Count);

            return (isCorrect, correctAnswer);
        }

        public string AskQuestion() => Questions[_index];

        public string Hash() => (string.Join("", Questions) + string.Join("", Answers)).Hash();
    }
}
