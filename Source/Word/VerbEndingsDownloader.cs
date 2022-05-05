using HtmlAgilityPack;
using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source.Word
{
    public class VerbEndingsDownloader
    {
        private static string GetXPath(string mobileTitle)
        {
            return $"//div[@mobile-title='{mobileTitle}']//ul";
        }

        private static VerbEndings DownloadEndings(string infinitive, HtmlDocument document, string xpath)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);

            if (node == null)
                throw new Exception($"Unable to download verb endings for '{infinitive}'.");

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

        private static void FixWebsiteBug(VerbEndings verbEndings)
        {
            if (verbEndings[Verb.PersonalPronoun.FirstSingular].Split(" ").Length != 4)
                return;

            foreach (var key in verbEndings.Keys)
            {
                var parts = verbEndings[key].Split(" ");

                if (parts.Length != 4)
                    throw new Exception("Unable to fix verb endings.");

                verbEndings[key] = $"{parts[0]} {parts[1]} {parts[3]} {parts[2]}";
            }
        }

        private static VerbEndings? DownloadImperativeEndings(string infinitive, HtmlDocument document)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(GetXPath("Imperativ Präsens"));

            if (node == null)
                return null;

            if (node.ChildNodes.Count < 4)
                throw new Exception($"Invalid amount of imperative verb endings for '{infinitive}'.");

            var verbEndings = new VerbEndings();

            var secondSingularEnding = Utility.Capitalize($"{node.ChildNodes[0].InnerText.Replace(" (du)", "")}!");
            verbEndings.Add(Verb.PersonalPronoun.SecondSingular, secondSingularEnding);

            var secondPluralEnding = Utility.Capitalize($"{node.ChildNodes[2].InnerText.Replace(" ihr", "")}!");
            verbEndings.Add(Verb.PersonalPronoun.SecondPlural, secondPluralEnding);

            var thirdPluralEnding = Utility.Capitalize($"{node.ChildNodes[3].InnerText}!");
            verbEndings.Add(Verb.PersonalPronoun.ThirdPlural, thirdPluralEnding);

            return verbEndings;
        }

        public static List<VerbEndings> Download(string infinitive)
        {
            var web = new HtmlWeb();
            var document = web.Load($"{Config.Instance.VerbEndingsUrl}-{infinitive.Trim()}.html");

            var allVerbEndings = new List<VerbEndings>
            {
                DownloadEndings(infinitive, document, GetXPath("Indikativ Präsens")),
                DownloadEndings(infinitive, document, GetXPath("Indikativ Präteritum")),
                DownloadEndings(infinitive, document, GetXPath("Indikativ Perfekt"))
            };

            FixWebsiteBug(allVerbEndings[0]);
            FixWebsiteBug(allVerbEndings[1]);

            var imperativeEndings = DownloadImperativeEndings(infinitive, document);

            if (imperativeEndings != null)
                allVerbEndings.Add(imperativeEndings);

            return allVerbEndings;
        }
    }
}
