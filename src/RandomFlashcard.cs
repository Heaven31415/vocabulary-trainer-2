namespace VocabularyTrainer2
{
    internal class RandomFlashcard : FlashcardBase
    {
        private readonly static Random random = new();

        private readonly List<string> questions;
        private readonly List<string> answers;
        private int index;

        public RandomFlashcard(int parentId, Type type, List<string> questions, List<string> answers) : base(parentId, type)
        {
            if (questions.Count == 0)
                throw new Exception("At least 1 question is required.");

            if (answers.Count == 0)
                throw new Exception("At least 1 answer is required.");

            if (questions.Count != answers.Count)
                throw new Exception("Number of questions and answers cannot be different.");

            this.questions = questions;
            this.answers = answers;
            index = random.Next(0, questions.Count);
        }

        public override (bool, string) AnswerQuestion(string answer)
        {
            LastTrainingTime = DateTime.Now;

            var isCorrect = answers[index] == answer;

            if (isCorrect)
                Cooldown *= 2;
            else
            {
                if (Cooldown.Days > 1)
                    Cooldown /= 2;
            }

            var correctAnswer = answers[index];
            index = random.Next(0, questions.Count);

            return (isCorrect, correctAnswer);
        }

        public override string AskQuestion() => questions[index];

        public override string ComputeHash() => Utility.ComputeHash($"{string.Join("", questions)}{string.Join("", answers)}");
    }
}
