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
            public bool IsSuccessful { get; set; }
            public DateTime Time { get; set; }

            public Result(bool isSuccessful, DateTime time)
            {
                IsSuccessful = isSuccessful;
                Time = time;
            }
        }

        public int ParentId { get; set; }
        public FlashcardType Type { get; set; }
        public DateTime LastTrainingTime { get; set; } = DateTime.Now;
        public TimeSpan Cooldown { get; set; } = TimeSpan.FromDays(1);
        public List<Result> Results { get; set; } = new List<Result>();
        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }

        private readonly static Random _random = new();
        private int _index;

        public bool IsAvailable() => DateTime.Now > LastTrainingTime.Add(Cooldown);
        public bool IsAvailableAtDaysEnd() => DateTime.Today.AddDays(1) > LastTrainingTime.Add(Cooldown);

        public Flashcard(int parentId, FlashcardType type, List<string> questions, List<string> answers)
        {
            if (questions.Count == 0)
                throw new ArgumentException("questions needs to have at least 1 element.", nameof(questions));

            if (answers.Count == 0)
                throw new ArgumentException("answers needs to have at least 1 element.", nameof(answers));

            if (questions.Count != answers.Count)
                throw new Exception("questions and answers need to have the same amount of elements.");

            ParentId = parentId;
            Type = type;
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
            else
            {
                if (Cooldown.Days > Config.Instance.MinimalFlashcardCooldownInDays)
                    Cooldown /= 2;
            }

            var correctAnswer = Answers[_index];
            _index = _random.Next(0, Questions.Count);

            return (isCorrect, correctAnswer);
        }

        public string AskQuestion() => Questions[_index];

        public string ComputeHash() => Utility.ComputeHash($"{string.Join("", Questions)}{string.Join("", Answers)}");
    }
}
