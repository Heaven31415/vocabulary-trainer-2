namespace VocabularyTrainer2
{
    internal class MultiFlashcard : FlashcardBase
    {
        private readonly static Random random = new();

        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }
        private int index;

        public MultiFlashcard(int parentId, Type type, List<string> questions, List<string> answers) : base(parentId, type)
        {
            if (questions.Count == 0)
                throw new Exception("At least 1 question is required.");

            if (answers.Count == 0)
                throw new Exception("At least 1 answer is required.");

            if (questions.Count != answers.Count)
                throw new Exception("Number of questions and answers cannot be different.");

            Questions = questions;
            Answers = answers;
            index = random.Next(0, questions.Count);
        }

        public override (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            var isCorrect = Answers[index] == answer;

            if (isCorrect)
                Cooldown *= 2;
            else
            {
                if (Cooldown.Days > 1)
                    Cooldown /= 2;
            }

            var correctAnswer = Answers[index];
            index = random.Next(0, Questions.Count);

            return (isCorrect, correctAnswer);
        }

        public override string AskQuestion() => Questions[index];

        public override string ComputeHash() => Utility.ComputeHash($"{string.Join("", Questions)}{string.Join("", Answers)}");
    }
}
