using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class FlashcardRepository
    {
        public static List<Flashcard> CreateFlashcards(List<Noun> nouns)
        {
            var flashcards = new List<Flashcard>();

            foreach (var noun in nouns)
            {
                if (noun.GermanSingularForm != null)
                {
                    string question = $"{noun.EnglishDescription} (singular)";
                    string answer = $"{noun.Gender.ToArticle()} {noun.GermanSingularForm}";
                    flashcards.Add(new Flashcard(noun.Id, Type.NounSingularForm, question, answer));
                }

                if (noun.GermanPluralForm != null)
                {
                    string question = $"{noun.EnglishDescription} (plural)";
                    string answer = noun.GermanPluralForm;
                    flashcards.Add(new Flashcard(noun.Id, Type.NounPluralForm, question, answer));
                }
            }

            return flashcards;
        }

        public static List<Flashcard> CreateFlashcards(List<Adjective> adjectives)
        {
            var flashcards = new List<Flashcard>();

            foreach (var adjective in adjectives)
            {
                {
                    string question = $"{adjective.EnglishDescription} (positive)";
                    string answer = adjective.PositiveDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectivePositiveDegree, question, answer));
                }

                if (!string.IsNullOrEmpty(adjective.ComparativeDegree))
                {
                    string question = $"{adjective.EnglishDescription} (comparative)";
                    string answer = adjective.ComparativeDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectiveComparativeDegree, question, answer));
                }

                if (!string.IsNullOrEmpty(adjective.SuperlativeDegree))
                {
                    string question = $"{adjective.EnglishDescription} (superlative)";
                    string answer = adjective.SuperlativeDegree;
                    flashcards.Add(new Flashcard(adjective.Id, Type.AdjectiveSuperlativeDegree, question, answer));
                }
            }

            return flashcards;
        }

        public static List<Flashcard> CreateFlashcards(List<Verb> verbs)
        {
            var flashcards = new List<Flashcard>();

            foreach (var verb in verbs)
            {
                {
                    string question = $"{verb.EnglishDescription} (Präsens, ich)";
                    string answer = verb.Present[PersonalPronoun.FirstSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbPresentFirstSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präsens, du)";
                    string answer = verb.Present[PersonalPronoun.SecondSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbPresentSecondSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präsens, er)";
                    string answer = verb.Present[PersonalPronoun.ThirdSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbPresentThirdSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präsens, ihr)";
                    string answer = verb.Present[PersonalPronoun.SecondPlural];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbPresentSecondPlural, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präteritum, ich)";
                    string answer = verb.SimplePast[PersonalPronoun.FirstSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbSimplePastFirstSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präteritum, du)";
                    string answer = verb.SimplePast[PersonalPronoun.SecondSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbSimplePastSecondSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präteritum, er)";
                    string answer = verb.SimplePast[PersonalPronoun.ThirdSingular];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbSimplePastThirdSingular, question, answer));
                }

                {
                    string question = $"{verb.EnglishDescription} (Präteritum, ihr)";
                    string answer = verb.SimplePast[PersonalPronoun.SecondPlural];
                    flashcards.Add(new Flashcard(verb.Id, Type.VerbSimplePastSecondPlural, question, answer));
                }
            }

            return flashcards;
        }

        public static List<RandomFlashcard> CreateRandomFlashcards(List<Verb> verbs)
        {
            var flashcards = new List<RandomFlashcard>();

            foreach (var verb in verbs)
            {
                {
                    var questions = new List<string>();
                    var answers = new List<string>();

                    questions.Add($"{verb.EnglishDescription} (Präsens, wir)");
                    answers.Add(verb.Present[PersonalPronoun.FirstPlural]);

                    questions.Add($"{verb.EnglishDescription} (Präsens, Sie)");
                    answers.Add(verb.Present[PersonalPronoun.ThirdPlural]);

                    flashcards.Add(new RandomFlashcard(verb.Id, Type.VerbPresentFirstOrThirdPlural, questions, answers));
                }

                {
                    var questions = new List<string>();
                    var answers = new List<string>();

                    questions.Add($"{verb.EnglishDescription} (Perfekt, ich)");
                    answers.Add(verb.Perfekt[PersonalPronoun.FirstSingular]);

                    questions.Add($"{verb.EnglishDescription} (Perfekt, du)");
                    answers.Add(verb.Perfekt[PersonalPronoun.SecondSingular]);

                    questions.Add($"{verb.EnglishDescription} (Perfekt, er)");
                    answers.Add(verb.Perfekt[PersonalPronoun.ThirdSingular]);

                    questions.Add($"{verb.EnglishDescription} (Perfekt, wir)");
                    answers.Add(verb.Perfekt[PersonalPronoun.FirstPlural]);

                    questions.Add($"{verb.EnglishDescription} (Perfekt, ihr)");
                    answers.Add(verb.Perfekt[PersonalPronoun.SecondPlural]);

                    questions.Add($"{verb.EnglishDescription} (Perfekt, Sie)");
                    answers.Add(verb.Perfekt[PersonalPronoun.ThirdPlural]);

                    flashcards.Add(new RandomFlashcard(verb.Id, Type.VerbPerfekt, questions, answers));
                }
            }

            return flashcards;
        }

        public static void UpdateFlashcards(List<Noun> nouns, List<Flashcard> flashcards)
        {
            foreach (var noun in nouns)
            {
                if (noun.GermanSingularForm != null)
                {
                    var singularFormFlashcard = flashcards.Find(f => f.ParentId == noun.Id && f.Type == Type.NounSingularForm);

                    string question = $"{noun.EnglishDescription} (singular)";
                    string answer = $"{noun.Gender.ToArticle()} {noun.GermanSingularForm}";
                    var flashcardCandidate = new Flashcard(noun.Id, Type.NounSingularForm, question, answer);

                    if (singularFormFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (singularFormFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            singularFormFlashcard.Question = flashcardCandidate.Question;
                            singularFormFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                if (noun.GermanPluralForm != null)
                {
                    var pluralFormFlashcard = flashcards.Find(f => f.ParentId == noun.Id && f.Type == Type.NounPluralForm);

                    string question = $"{noun.EnglishDescription} (plural)";
                    string answer = noun.GermanPluralForm;
                    var flashcardCandidate = new Flashcard(noun.Id, Type.NounPluralForm, question, answer);

                    if (pluralFormFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (pluralFormFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            pluralFormFlashcard.Question = flashcardCandidate.Question;
                            pluralFormFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }
            }
        }

        public static void UpdateFlashcards(List<Adjective> adjectives, List<Flashcard> flashcards)
        {
            throw new NotImplementedException();
        }

        public static void UpdateFlashcards(List<Verb> verbs, List<Flashcard> flashcards)
        {
            throw new NotImplementedException();
        }

        public static void UpdateRandomFlashcards(List<Verb> verbs, List<RandomFlashcard> flashcards)
        {
            throw new NotImplementedException();
        }

        public static List<Flashcard> LoadFlashcards(string path)
        {
            var json = File.ReadAllText(path);
            var flashcardHelpers = JsonSerializer.Deserialize<List<FlashcardHelper>>(json);
            var flashcards = new List<Flashcard>();

            if (flashcardHelpers == null)
                throw new Exception("Flashcard Helpers are null!");

            foreach (var flashcardHelper in flashcardHelpers)
            {
                var parentId = flashcardHelper.ParentId;
                var type = flashcardHelper.Type;
                var question = flashcardHelper.Question;
                var answer = flashcardHelper.Answer;

                var flashcard = new Flashcard(parentId, type, question, answer)
                {
                    LastTrainingTime = flashcardHelper.LastTrainingTime,
                    Cooldown = flashcardHelper.Cooldown
                };

                flashcards.Add(flashcard);
            }

            return flashcards;
        }

        public static void SaveFlashcards(string path, List<Flashcard> flashcards)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(flashcards, options);
            File.WriteAllText(path, json);
        }

        public static List<RandomFlashcard> LoadRandomFlashcards(string path)
        {
            var json = File.ReadAllText(path);
            var flashcardHelpers = JsonSerializer.Deserialize<List<RandomFlashcardHelper>>(json);
            var flashcards = new List<RandomFlashcard>();

            if (flashcardHelpers == null)
                throw new Exception("Flashcard Helpers are null!");

            foreach (var flashcardHelper in flashcardHelpers)
            {
                var parentId = flashcardHelper.ParentId;
                var type = flashcardHelper.Type;
                var questions = flashcardHelper.Questions;
                var answers = flashcardHelper.Answers;

                var flashcard = new RandomFlashcard(parentId, type, questions, answers)
                {
                    LastTrainingTime = flashcardHelper.LastTrainingTime,
                    Cooldown = flashcardHelper.Cooldown
                };

                flashcards.Add(flashcard);
            }

            return flashcards;
        }

        public static void SaveRandomFlashcards(string path, List<RandomFlashcard> flashcards)
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
