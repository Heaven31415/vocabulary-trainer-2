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

        private static VerbEndings DownloadEndings(string infinitive, int controlCode, HtmlDocument document, string xpath)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(xpath);

            if (node == null)
                throw new Exception($"Unable to download {xpath} verb endings for '{infinitive}'.");

            if (controlCode != 1 && controlCode != 2)
                throw new ArgumentException("controlCode must be equal to 1 or 2.", nameof(controlCode));

            var offset = 6 * (controlCode - 1); // 0 or 6

            if (node.ChildNodes.Count < 6 + offset)
                throw new Exception($"Invalid amount of {xpath} verb endings for '{infinitive}'.");

            var verbEndings = new VerbEndings();

            for (int i = 0; i < 6; i++)
                verbEndings.Add((Verb.PersonalPronoun)i, node.ChildNodes[i + offset].InnerText);

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

        private static VerbEndings? DownloadImperativeEndings(string infinitive, int controlCode, HtmlDocument document)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode(GetXPath("Imperativ Präsens"));

            if (node == null)
                return null;

            if (controlCode != 1 && controlCode != 2)
                throw new ArgumentException("controlCode must be equal to 1 or 2.", nameof(controlCode));

            var offset = 4 * (controlCode - 1); // 0 or 4

            if (node.ChildNodes.Count < 4 + offset)
                throw new Exception($"Invalid amount of imperative verb endings for '{infinitive}'.");

            var verbEndings = new VerbEndings();

            var secondSingularEnding = node.ChildNodes[0 + offset].InnerText;
            secondSingularEnding = secondSingularEnding.Capitalize().Replace(" (du)", "") + "!";
            verbEndings.Add(Verb.PersonalPronoun.SecondSingular, secondSingularEnding);

            var secondPluralEnding = node.ChildNodes[2 + offset].InnerText;
            secondPluralEnding = secondPluralEnding.Capitalize().Replace(" ihr", "") + "!";
            verbEndings.Add(Verb.PersonalPronoun.SecondPlural, secondPluralEnding);

            var thirdPluralEnding = node.ChildNodes[3 + offset].InnerText;
            thirdPluralEnding = thirdPluralEnding.Capitalize() + "!";
            verbEndings.Add(Verb.PersonalPronoun.ThirdPlural, thirdPluralEnding);

            return verbEndings;
        }

        public static List<VerbEndings> Download(string infinitive, List<int> controlCodes)
        {
            var web = new HtmlWeb();
            var document = web.Load($"{Config.Instance.VerbEndingsUrl}-{infinitive.Trim()}.html");

            var allVerbEndings = new List<VerbEndings>
            {
                DownloadEndings(infinitive, controlCodes[0], document, GetXPath("Indikativ Präsens")),
                DownloadEndings(infinitive, controlCodes[1], document, GetXPath("Indikativ Präteritum")),
                DownloadEndings(infinitive, controlCodes[2], document, GetXPath("Indikativ Perfekt"))
            };

            FixWebsiteBug(allVerbEndings[0]);
            FixWebsiteBug(allVerbEndings[1]);

            var imperativeEndings = DownloadImperativeEndings(infinitive, controlCodes[3], document);

            if (imperativeEndings != null)
                allVerbEndings.Add(imperativeEndings);

            return allVerbEndings;
        }
    }
}
