namespace VocabularyTrainer2
{
    internal class Adjective
    {
        public string EnglishDescription { get; }
        public string PositiveDegree { get; }
        public string? ComparativeDegree { get; }
        public string? SuperlativeDegree { get; }

        public Adjective(string englishDescription, string positiveDegree)
        {
            EnglishDescription = englishDescription;
            PositiveDegree = positiveDegree;
            ComparativeDegree = null;
            SuperlativeDegree = null;
        }

        public Adjective(string englishDescription, string positiveDegree, string comparativeDegree, string superlativeDegree)
        {
            EnglishDescription = englishDescription;
            PositiveDegree = positiveDegree;
            ComparativeDegree = comparativeDegree;
            SuperlativeDegree = superlativeDegree;
        }
    }
}
