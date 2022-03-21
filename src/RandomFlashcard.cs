namespace VocabularyTrainer2
{
    internal class RandomFlashcard : IFlashcardable
    {
        private readonly static Random random = new();

        private readonly List<string> questions;
        private readonly List<string> answers;
        private DateTime lastTrainingTime;
        private TimeSpan cooldown;
        private int index;

        public RandomFlashcard(List<string> questions, List<string> answers)
        {
            if (questions.Count == 0)
                throw new Exception("At least 1 question is required.");

            if (answers.Count == 0)
                throw new Exception("At least 1 answer is required.");

            if (questions.Count != answers.Count)
                throw new Exception("Number of questions and answers cannot be different.");

            this.questions = questions;
            this.answers = answers;
            lastTrainingTime = DateTime.Now.AddDays(-1);
            cooldown = TimeSpan.FromDays(1);
            index = random.Next(0, questions.Count);
        }

        public string AskQuestion() => questions[index];

        public (bool, string) GiveAnswer(string answer)
        {
            lastTrainingTime = DateTime.Now;

            var isCorrect = answers[index] == answer;

            if (isCorrect)
                cooldown *= 2;
            else
            {
                if (cooldown.Days > 1)
                    cooldown /= 2;
            }

            var correctAnswer = answers[index];
            index = random.Next(0, questions.Count);

            return (isCorrect, correctAnswer);
        }

        public bool IsAvailable() => DateTime.Now > lastTrainingTime.Add(cooldown);
    }
}
