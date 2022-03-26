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
            foreach (var adjective in adjectives)
            {
                {
                    var positiveDegreeFlashcard = flashcards.Find(f => f.ParentId == adjective.Id && f.Type == Type.AdjectivePositiveDegree);

                    string question = $"{adjective.EnglishDescription} (positive)";
                    string answer = adjective.PositiveDegree;
                    var flashcardCandidate = new Flashcard(adjective.Id, Type.AdjectivePositiveDegree, question, answer);

                    if (positiveDegreeFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (positiveDegreeFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            positiveDegreeFlashcard.Question = flashcardCandidate.Question;
                            positiveDegreeFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(adjective.ComparativeDegree))
                {
                    var comparativeDegreeFlashcard = flashcards.Find(f => f.ParentId == adjective.Id && f.Type == Type.AdjectiveComparativeDegree);

                    string question = $"{adjective.EnglishDescription} (comparative)";
                    string answer = adjective.ComparativeDegree;
                    var flashcardCandidate = new Flashcard(adjective.Id, Type.AdjectiveComparativeDegree, question, answer);

                    if (comparativeDegreeFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (comparativeDegreeFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            comparativeDegreeFlashcard.Question = flashcardCandidate.Question;
                            comparativeDegreeFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(adjective.SuperlativeDegree))
                {
                    var superlativeDegreeFlashcard = flashcards.Find(f => f.ParentId == adjective.Id && f.Type == Type.AdjectiveSuperlativeDegree);

                    string question = $"{adjective.EnglishDescription} (superlative)";
                    string answer = adjective.SuperlativeDegree;
                    var flashcardCandidate = new Flashcard(adjective.Id, Type.AdjectiveSuperlativeDegree, question, answer);

                    if (superlativeDegreeFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (superlativeDegreeFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            superlativeDegreeFlashcard.Question = flashcardCandidate.Question;
                            superlativeDegreeFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }
            }
        }

        public static void UpdateFlashcards(List<Verb> verbs, List<Flashcard> flashcards)
        {
            foreach (var verb in verbs)
            {
                {
                    var presentFirstSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPresentFirstSingular);

                    string question = $"{verb.EnglishDescription} (Präsens, ich)";
                    string answer = verb.Present[PersonalPronoun.FirstSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbPresentFirstSingular, question, answer);

                    if (presentFirstSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (presentFirstSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            presentFirstSingularFlashcard.Question = flashcardCandidate.Question;
                            presentFirstSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var presentSecondSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPresentSecondSingular);

                    string question = $"{verb.EnglishDescription} (Präsens, du)";
                    string answer = verb.Present[PersonalPronoun.SecondSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbPresentSecondSingular, question, answer);

                    if (presentSecondSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (presentSecondSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            presentSecondSingularFlashcard.Question = flashcardCandidate.Question;
                            presentSecondSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var presentThirdSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPresentThirdSingular);

                    string question = $"{verb.EnglishDescription} (Präsens, er)";
                    string answer = verb.Present[PersonalPronoun.ThirdSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbPresentThirdSingular, question, answer);

                    if (presentThirdSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (presentThirdSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            presentThirdSingularFlashcard.Question = flashcardCandidate.Question;
                            presentThirdSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var presentSecondPluralFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbPresentSecondPlural);

                    string question = $"{verb.EnglishDescription} (Präsens, ihr)";
                    string answer = verb.Present[PersonalPronoun.SecondPlural];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbPresentSecondPlural, question, answer);

                    if (presentSecondPluralFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (presentSecondPluralFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            presentSecondPluralFlashcard.Question = flashcardCandidate.Question;
                            presentSecondPluralFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var simplePastFirstSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbSimplePastFirstSingular);

                    string question = $"{verb.EnglishDescription} (Präteritum, ich)";
                    string answer = verb.SimplePast[PersonalPronoun.FirstSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbSimplePastFirstSingular, question, answer);

                    if (simplePastFirstSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (simplePastFirstSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            simplePastFirstSingularFlashcard.Question = flashcardCandidate.Question;
                            simplePastFirstSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var simplePastSecondSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbSimplePastSecondSingular);

                    string question = $"{verb.EnglishDescription} (Präteritum, du)";
                    string answer = verb.SimplePast[PersonalPronoun.SecondSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbSimplePastSecondSingular, question, answer);

                    if (simplePastSecondSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (simplePastSecondSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            simplePastSecondSingularFlashcard.Question = flashcardCandidate.Question;
                            simplePastSecondSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var simplePastThirdSingularFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbSimplePastThirdSingular);

                    string question = $"{verb.EnglishDescription} (Präteritum, er)";
                    string answer = verb.SimplePast[PersonalPronoun.ThirdSingular];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbSimplePastThirdSingular, question, answer);

                    if (simplePastThirdSingularFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (simplePastThirdSingularFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            simplePastThirdSingularFlashcard.Question = flashcardCandidate.Question;
                            simplePastThirdSingularFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                {
                    var simplePastSecondPluralFlashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == Type.VerbSimplePastSecondPlural);

                    string question = $"{verb.EnglishDescription} (Präteritum, ihr)";
                    string answer = verb.SimplePast[PersonalPronoun.SecondPlural];
                    var flashcardCandidate = new Flashcard(verb.Id, Type.VerbSimplePastSecondPlural, question, answer);

                    if (simplePastSecondPluralFlashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (simplePastSecondPluralFlashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            simplePastSecondPluralFlashcard.Question = flashcardCandidate.Question;
                            simplePastSecondPluralFlashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }
            }
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
