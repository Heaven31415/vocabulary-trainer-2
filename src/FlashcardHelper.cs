namespace VocabularyTrainer2
{
    internal class FlashcardHelper
    {
        public int ParentId { get; set; }
        public Type Type { get; set; }
        public DateTime LastTrainingTime { get; set; }
        public TimeSpan Cooldown { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
    }
}
