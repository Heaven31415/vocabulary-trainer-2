using HtmlAgilityPack;

namespace VocabularyTrainer2
{
    internal class VerbDownloader
    {
        private static string GetURL(string germanVerbInfinitive)
        {
            return $"https://conjugator.reverso.net/conjugation-german-verb-{germanVerbInfinitive.Trim()}.html";
        }

        private static string GetXPath(string mobileTitle)
        {
            return $"//div[@mobile-title='{mobileTitle}']//ul";
        }

        private static Dictionary<PersonalPronoun, string> DownloadVerbEndings(HtmlDocument document, string xpath)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);
            var verbEndings = new Dictionary<PersonalPronoun, string>();

            int i = 0;
            foreach (var n in node.ChildNodes)
            {
                verbEndings.Add((PersonalPronoun)i, n.InnerText);
                i++;
            }

            verbEndings[PersonalPronoun.ThirdSingular] = verbEndings[PersonalPronoun.ThirdSingular].Replace("er/sie/es", "er");

            return verbEndings;
        }

        public static List<Dictionary<PersonalPronoun, string>> Download(string germanVerbInfinitive)
        {
            var web = new HtmlWeb();
            var document = web.Load(GetURL(germanVerbInfinitive));

            var presentTenseVerbEndings = DownloadVerbEndings(document, GetXPath("Indikativ Präsens"));
            var simplePastVerbEndings = DownloadVerbEndings(document, GetXPath("Indikativ Präteritum"));
            var perfektVerbEndings = DownloadVerbEndings(document, GetXPath("Indikativ Perfekt"));

            return new List<Dictionary<PersonalPronoun, string>> { presentTenseVerbEndings, simplePastVerbEndings, perfektVerbEndings };
        }
    }
}
