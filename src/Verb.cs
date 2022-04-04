namespace VocabularyTrainer2
{
    enum PersonalPronoun
    {
        FirstSingular,
        SecondSingular,
        ThirdSingular,
        FirstPlural,
        SecondPlural,
        ThirdPlural
    }

    internal class Verb
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Dictionary<PersonalPronoun, string> Present { get; set; }
        public Dictionary<PersonalPronoun, string> SimplePast { get; set; }
        public Dictionary<PersonalPronoun, string> Perfekt { get; set; }

        public Verb(int id, string description)
        {
            Id = id;
            Description = description;
            Present = new Dictionary<PersonalPronoun, string>();
            SimplePast = new Dictionary<PersonalPronoun, string>();
            Perfekt = new Dictionary<PersonalPronoun, string>();
        }
    }
}
