namespace VocabularyTrainer2
{
    internal enum Gender
    {
        Masculine,
        Feminine,
        Neuter
    }

    internal class Noun
    {
        public Gender Gender { get; set; }

        public string? SingularForm { get; set; }

        public string? PluralForm { get; set; }

        public Noun(string? singularForm, string? pluralForm)
        {
            if (singularForm == null && pluralForm == null)
                throw new Exception("Singular form and plural form cannot be null at the same time.");

            if (singularForm != null)
                (Gender, SingularForm) = ParseNounSingularForm(singularForm);

            if (pluralForm != null)
                PluralForm = ParseNounPluralForm(pluralForm);
        }

        public static (Gender, string) ParseNounSingularForm(string data)
        {
            string[] parts = data.Trim().Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid singular form format. Should be: [article] [noun]");

            string article = parts[0];
            string noun = parts[1];

            if (article != "der" && article != "die" && article != "das")
                throw new Exception("Invalid singular form article.");

            Gender gender = Gender.Masculine;

            if (article == "die")
                gender = Gender.Feminine;
            else if (article == "das")
                gender = Gender.Neuter;

            if (noun.Length < 2)
                throw new Exception("Singular form needs to have at least 2 characters.");

            if (noun.ToUpper()[0] != noun[0])
                throw new Exception("Singular form needs to be capitalized.");

            return (gender, noun);
        }

        public static string ParseNounPluralForm(string data)
        {
            string[] parts = data.Trim().Split(' ');

            if (parts.Length != 2)
                throw new Exception("Invalid plural form format. Should be: [article] [noun]");

            string article = parts[0];
            string noun = parts[1];

            if (article != "die")
                throw new Exception("Invalid plural form article.");

            if (noun.Length < 2)
                throw new Exception("Plural form needs to have at least 2 characters.");

            if (noun.ToUpper()[0] != noun[0])
                throw new Exception("Plural form needs to be capitalized.");

            return data;
        }
    }
}
