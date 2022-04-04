namespace VocabularyTrainer2
{
    internal class Adjective
    {
        public int Id { get; }
        public string Description { get; }
        public string PositiveDegree { get; }
        public string? ComparativeDegree { get; }
        public string? SuperlativeDegree { get; }

        public Adjective(int id, string description, string positiveDegree)
        {
            Id = id;
            Description = description;
            PositiveDegree = positiveDegree;
            ComparativeDegree = null;
            SuperlativeDegree = null;
        }

        public Adjective(int id, string description, string positiveDegree, string comparativeDegree, string superlativeDegree)
        {
            Id = id;
            Description = description;
            PositiveDegree = positiveDegree;
            ComparativeDegree = comparativeDegree;
            SuperlativeDegree = superlativeDegree;
        }
    }
}
