using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class MultiFlashcardSet
    {
        public static void Update(List<Verb> verbs, List<MultiFlashcard> flashcards)
        {
            foreach (var verb in verbs)
            {
                {
                    var presentFirstOrThirdPluralFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPresentFirstOrThirdPlural);

                    var questions = new List<string>();
                    var answers = new List<string>();

                    questions.Add($"{verb.Description} (Präsens, wir)");
                    answers.Add(verb.Present[PersonalPronoun.FirstPlural]);

                    questions.Add($"{verb.Description} (Präsens, Sie)");
                    answers.Add(verb.Present[PersonalPronoun.ThirdPlural]);

                    var flashcardCandidate = new MultiFlashcard(verb.Id, Type.VerbPresentFirstOrThirdPlural, questions, answers);

                    if (presentFirstOrThirdPluralFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (presentFirstOrThirdPluralFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            presentFirstOrThirdPluralFlashcard.Questions = flashcardCandidate.Questions;
                            presentFirstOrThirdPluralFlashcard.Answers = flashcardCandidate.Answers;
                        }
                    }
                }

                {
                    var simplePastFirstOrThirdPluralFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbSimplePastFirstOrThirdPlural);

                    var questions = new List<string>();
                    var answers = new List<string>();

                    questions.Add($"{verb.Description} (Präteritum, wir)");
                    answers.Add(verb.SimplePast[PersonalPronoun.FirstPlural]);

                    questions.Add($"{verb.Description} (Präteritum, Sie)");
                    answers.Add(verb.SimplePast[PersonalPronoun.ThirdPlural]);

                    var flashcardCandidate = new MultiFlashcard(verb.Id, Type.VerbSimplePastFirstOrThirdPlural, questions, answers);

                    if (simplePastFirstOrThirdPluralFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (simplePastFirstOrThirdPluralFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            simplePastFirstOrThirdPluralFlashcard.Questions = flashcardCandidate.Questions;
                            simplePastFirstOrThirdPluralFlashcard.Answers = flashcardCandidate.Answers;
                        }
                    }
                }

                {
                    var perfektFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPerfekt);

                    var questions = new List<string>();
                    var answers = new List<string>();

                    questions.Add($"{verb.Description} (Perfekt, ich)");
                    answers.Add(verb.Perfekt[PersonalPronoun.FirstSingular]);

                    questions.Add($"{verb.Description} (Perfekt, du)");
                    answers.Add(verb.Perfekt[PersonalPronoun.SecondSingular]);

                    questions.Add($"{verb.Description} (Perfekt, er)");
                    answers.Add(verb.Perfekt[PersonalPronoun.ThirdSingular]);

                    questions.Add($"{verb.Description} (Perfekt, wir)");
                    answers.Add(verb.Perfekt[PersonalPronoun.FirstPlural]);

                    questions.Add($"{verb.Description} (Perfekt, ihr)");
                    answers.Add(verb.Perfekt[PersonalPronoun.SecondPlural]);

                    questions.Add($"{verb.Description} (Perfekt, Sie)");
                    answers.Add(verb.Perfekt[PersonalPronoun.ThirdPlural]);

                    var flashcardCandidate = new MultiFlashcard(verb.Id, Type.VerbPerfekt, questions, answers);

                    if (perfektFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (perfektFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            perfektFlashcard.Questions = flashcardCandidate.Questions;
                            perfektFlashcard.Answers = flashcardCandidate.Answers;
                        }
                    }
                }
            }
        }

        public static List<MultiFlashcard> Load(string path)
        {
            var json = File.ReadAllText(path);
            var flashcardHelpers = JsonSerializer.Deserialize<List<MultiFlashcardHelper>>(json);
            var flashcards = new List<MultiFlashcard>();

            if (flashcardHelpers == null)
                throw new Exception("Flashcard Helpers are null!");

            foreach (var flashcardHelper in flashcardHelpers)
            {
                var parentId = flashcardHelper.ParentId;
                var type = flashcardHelper.Type;
                var questions = flashcardHelper.Questions;
                var answers = flashcardHelper.Answers;

                var flashcard = new MultiFlashcard(parentId, type, questions, answers)
                {
                    LastTrainingTime = flashcardHelper.LastTrainingTime,
                    Cooldown = flashcardHelper.Cooldown
                };

                flashcards.Add(flashcard);
            }

            return flashcards;
        }

        public static void Save(string path, List<MultiFlashcard> flashcards)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(flashcards, options);
            File.WriteAllText(path, json);
        }
    }
}
