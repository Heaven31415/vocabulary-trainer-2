using HtmlAgilityPack;

namespace VocabularyTrainer2.Source.Word
{
    public class VerbEndingsDownloader
    {
        private static string GetURL(string infinitive)
        {
            return $"https://conjugator.reverso.net/conjugation-german-verb-{infinitive.Trim()}.html";
        }

        private static string GetXPath(string mobileTitle)
        {
            return $"//div[@mobile-title='{mobileTitle}']//ul";
        }

        private static VerbEndings DownloadEndings(HtmlDocument document, string xpath)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);
            var verbEndings = new VerbEndings();

            var i = 0;
            foreach (var n in node.ChildNodes)
            {
                verbEndings.Add((Verb.PersonalPronoun)i, n.InnerText);

                if (i == 5) 
                    break;

                i++;
            }

            var thirdSingular = Verb.PersonalPronoun.ThirdSingular;
            verbEndings[thirdSingular] = verbEndings[thirdSingular].Replace("er/sie/es", "er");

            return verbEndings;
        }

        public static List<VerbEndings> Download(string infinitive)
        {
            var web = new HtmlWeb();
            var document = web.Load(GetURL(infinitive));

            var presentTenseEndings = DownloadEndings(document, GetXPath("Indikativ Präsens"));
            var simplePastEndings = DownloadEndings(document, GetXPath("Indikativ Präteritum"));
            var perfektEndings = DownloadEndings(document, GetXPath("Indikativ Perfekt"));

            return new List<VerbEndings> { presentTenseEndings, simplePastEndings, perfektEndings };
        }
    }
}
