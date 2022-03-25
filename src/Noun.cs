namespace VocabularyTrainer2
{
    internal enum Gender
    {
        Masculine,
        Feminine,
        Neuter
    }

    internal static class Extensions
    {
        public static string ToArticle(this Gender gender)
        {
            return gender switch
            {
                Gender.Masculine => "der",
                Gender.Feminine => "die",
                Gender.Neuter => "das",
                _ => throw new Exception("Invalid type of gender."),
            };
        }
    }

    internal class Noun
    {
        public int Id { get; }
        public string EnglishDescription { get; }
        public Gender Gender { get; }
        public string? GermanSingularForm { get; }
        public string? GermanPluralForm { get; }

        public Noun(int id, string englishDescription, string? germanSingularForm, string? germanPluralForm)
        {
            Id = id;
            EnglishDescription = englishDescription;

            if (germanSingularForm == null && germanPluralForm == null)
                throw new Exception("German singular and plural forms cannot be null at the same time.");

            if (germanSingularForm != null)
                (Gender, GermanSingularForm) = ParseGermanSingularForm(germanSingularForm);

            if (germanPluralForm != null)
                GermanPluralForm = ParseGermanPluralForm(germanPluralForm);
        }

        public static (Gender, string) ParseGermanSingularForm(string data)
        {
            string[] parts = data.Trim().Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid german singular form format. Should be: [article] [noun]");

            string article = parts[0];
            string noun = parts[1];

            if (article != "der" && article != "die" && article != "das")
                throw new Exception("Invalid german singular form article.");

            Gender gender = Gender.Masculine;

            if (article == "die")
                gender = Gender.Feminine;
            else if (article == "das")
                gender = Gender.Neuter;

            if (noun.Length < 2)
                throw new Exception("German singular form needs to have at least 2 characters.");

            if (noun.ToUpper()[0] != noun[0])
                throw new Exception("German singular form needs to be capitalized.");

            return (gender, noun);
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
