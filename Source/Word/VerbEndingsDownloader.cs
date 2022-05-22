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

        private static HtmlDocument DownloadDocument(string infinitive)
        {
            return new HtmlWeb().Load($"{Config.Instance.VerbEndingsUrl}-{infinitive.Trim()}.html");
        }

        private static VerbEndings DownloadEndings(string infinitive, int controlCodeDigit, HtmlDocument document, string mobileTitle)
        {
            var xpath = GetXPath(mobileTitle);
            var node = document.DocumentNode.SelectSingleNode(xpath);

            if (node == null)
                throw new Exception($"Unable to find '{mobileTitle}' verb endings for '{infinitive}'.");

            var offset = 6 * (controlCodeDigit - 1); // 0 or 6
            var endingsCount = node.ChildNodes.Count;

            if (controlCodeDigit == 1 && endingsCount != 6 && endingsCount != 12)
                throw new Exception($"Invalid amount of '{mobileTitle}' verb endings for '{infinitive}'. Expected 6 or 12, got {endingsCount}.");

            if (controlCodeDigit == 2 && endingsCount != 12)
                throw new Exception($"Invalid amount of '{mobileTitle}' verb endings for '{infinitive}'. Expected 12, got {endingsCount}.");

            var verbEndings = new VerbEndings();

            for (int i = 0; i < 6; i++)
                verbEndings.Add((Verb.PersonalPronoun)i, node.ChildNodes[i + offset].InnerText);

            var thirdSingular = Verb.PersonalPronoun.ThirdSingular;
            verbEndings[thirdSingular] = verbEndings[thirdSingular].Replace("er/sie/es", "er");

            return verbEndings;
        }

        private static VerbEndings DownloadImperativeEndings(string infinitive, int controlCodeDigit, HtmlDocument document, string mobileTitle)
        {
            var xpath = GetXPath(mobileTitle);
            var node = document.DocumentNode.SelectSingleNode(xpath);

            if (node == null)
                return FixImperativeEndingsBug(infinitive, controlCodeDigit, mobileTitle);

            var offset = 4 * (controlCodeDigit - 1); // 0 or 4
            var endingsCount = node.ChildNodes.Count;

            if (controlCodeDigit == 1 && endingsCount != 4 && endingsCount != 8)
                throw new Exception($"Invalid amount of '{mobileTitle}' verb endings for '{infinitive}'. Expected 4 or 8, got {endingsCount}.");

            if (controlCodeDigit == 2 && endingsCount != 8)
                throw new Exception($"Invalid amount of '{mobileTitle}' verb endings for '{infinitive}'. Expected 8, got {endingsCount}.");

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

        // A bug that happens only for verbs like 'sich anziehen'
        private static VerbEndings FixImperativeEndingsBug(string infinitive, int controlCodeDigit, string mobileTitle)
        {
            var incompleteInfinitive = infinitive.Split(' ')[1];
            var document = DownloadDocument(incompleteInfinitive);

            var verbEndings = DownloadImperativeEndings(incompleteInfinitive, controlCodeDigit, document, mobileTitle);

            foreach (var key in verbEndings.Keys)
            {
                var missingPiece = key switch
                {
                    Verb.PersonalPronoun.SecondSingular => "dich",
                    Verb.PersonalPronoun.SecondPlural => "euch",
                    Verb.PersonalPronoun.ThirdPlural => "sich",
                    _ => throw new Exception("Unable to fix imperative endings bug."),
                };

                var parts = verbEndings[key].Split(' ');

                if (key == Verb.PersonalPronoun.ThirdPlural)
                    verbEndings[key] = $"{parts[0]} {parts[1]} {missingPiece} {parts[2]}";
                else
                    verbEndings[key] = $"{parts[0]} {missingPiece} {parts[1]}";
            }

            return verbEndings;
        }

        // A bug that happens only for verbs like 'sich anziehen'
        private static void FixSeparablePrefixBug(VerbEndings verbEndings)
        {
            if (verbEndings[Verb.PersonalPronoun.FirstSingular].Split(' ').Length != 4)
                return;

            foreach (var key in verbEndings.Keys)
            {
                var parts = verbEndings[key].Split(' ');
                verbEndings[key] = $"{parts[0]} {parts[1]} {parts[3]} {parts[2]}";
            }
        }

        public static List<VerbEndings>? Download(string infinitive, int controlCode)
        {
            if (!Config.Instance.OnlineMode)
                return null;

            var document = DownloadDocument(infinitive);

            var allVerbEndings = new List<VerbEndings>
            {
                DownloadEndings(infinitive, controlCode.Digit(4), document, "Indikativ Präsens"),
                DownloadEndings(infinitive, controlCode.Digit(3), document, "Indikativ Präteritum"),
                DownloadEndings(infinitive, controlCode.Digit(2), document, "Indikativ Perfekt"),
                DownloadImperativeEndings(infinitive, controlCode.Digit(1), document, "Imperativ Präsens")
            };

            FixSeparablePrefixBug(allVerbEndings[0]);
            FixSeparablePrefixBug(allVerbEndings[1]);

            return allVerbEndings;
        }
    }
}
