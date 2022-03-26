using HtmlAgilityPack;

namespace VocabularyTrainer2
{
    internal class VerbExtractor
    {
        private static string GetURL(string germanVerbInfinitive)
        {
            return $"https://conjugator.reverso.net/conjugation-german-verb-{germanVerbInfinitive.Trim()}.html";
        }

        private static string GetXPath(string mobileTitle)
        {
            return $"//div[@mobile-title='{mobileTitle}']//ul";
        }

        private static List<string> ExtractVerbForms(HtmlDocument document, string xpath)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);
            var verbForms = new List<string>();

            foreach (var n in node.ChildNodes)
                verbForms.Add(n.InnerText);

            return verbForms;
        }

        public static Verb Extract(int id, string englishDescription, string germanInfinitive)
        {
            var web = new HtmlWeb();
            var document = web.Load(GetURL(germanInfinitive));
            var verb = new Verb(id, englishDescription);

            var presentTenseVerbForms = ExtractVerbForms(document, GetXPath("Indikativ Präsens"));
            for (int i = 0; i < 6; i++)
                verb.Present[(PersonalPronoun)i] = presentTenseVerbForms[i];

            var simplePastVerbForms = ExtractVerbForms(document, GetXPath("Indikativ Präteritum"));
            for (int i = 0; i < 6; i++)
                verb.SimplePast[(PersonalPronoun)i] = simplePastVerbForms[i];

            var perfektVerbForms = ExtractVerbForms(document, GetXPath("Indikativ Perfekt"));
            for (int i = 0; i < 6; i++)
                verb.Perfekt[(PersonalPronoun)i] = perfektVerbForms[i];

            verb.Present[PersonalPronoun.ThirdSingular] = verb.Present[PersonalPronoun.ThirdSingular].Replace("er/sie/es", "er");
            verb.SimplePast[PersonalPronoun.ThirdSingular] = verb.SimplePast[PersonalPronoun.ThirdSingular].Replace("er/sie/es", "er");
            verb.Perfekt[PersonalPronoun.ThirdSingular] = verb.Perfekt[PersonalPronoun.ThirdSingular].Replace("er/sie/es", "er");

            return verb;
        }
    }
}
