namespace VocabularyTrainer2
{
    internal class Adjective
    {
        public int Id { get; }
        public string EnglishDescription { get; }
        public string PositiveDegree { get; }
        public string? ComparativeDegree { get; }
        public string? SuperlativeDegree { get; }

        public Adjective(int id, string englishDescription, string positiveDegree)
        {
            Id = id;
            EnglishDescription = englishDescription;
            PositiveDegree = positiveDegree;
            ComparativeDegree = null;
            SuperlativeDegree = null;
        }

        public Adjective(int id, string englishDescription, string positiveDegree, string comparativeDegree, string superlativeDegree)
        {
            Id = id;
            EnglishDescription = englishDescription;
            PositiveDegree = positiveDegree;
            ComparativeDegree = comparativeDegree;
            SuperlativeDegree = superlativeDegree;
        }
    }
}
