namespace VocabularyTrainer2
{
    internal class Noun
    {
        public int Id { get; }
        public string Description { get; }
        public string? Article { get; }
        public string? GermanSingularForm { get; }
        public string? GermanPluralForm { get; }

        public Noun(int id, string description, string? germanSingularForm, string? germanPluralForm)
        {
            Id = id;
            Description = description;

            if (germanSingularForm == null && germanPluralForm == null)
                throw new Exception("German singular and plural forms cannot be null at the same time.");

            if (germanSingularForm != null)
                (Article, GermanSingularForm) = ParseGermanSingularForm(germanSingularForm);

            if (germanPluralForm != null)
                GermanPluralForm = ParseGermanPluralForm(germanPluralForm);
        }

        public static (string, string) ParseGermanSingularForm(string data)
        {
            string[] parts = data.Trim().Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid german singular form format. Should be: [article] [noun]");

            string article = parts[0];
            string noun = parts[1];

            if (article != "der" && article != "die" && article != "das")
                throw new Exception("Invalid german singular form article.");

            if (noun.Length < 2)
                throw new Exception("German singular form needs to have at least 2 characters.");

            if (noun.ToUpper()[0] != noun[0])
                throw new Exception("German singular form needs to be capitalized.");

            return (article, noun);
        }

        public static string ParseGermanPluralForm(string data)
        {
            string[] parts = data.Trim().Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid german plural form format. Should be: [article] [noun]");

            string article = parts[0];
            string noun = parts[1];

            if (article != "die")
                throw new Exception("Invalid german plural form article.");

            if (noun.Length < 2)
                throw new Exception("German plural form needs to have at least 2 characters.");

            if (noun.ToUpper()[0] != noun[0])
                throw new Exception("German plural form needs to be capitalized.");

            return data;
        }
    }
}
