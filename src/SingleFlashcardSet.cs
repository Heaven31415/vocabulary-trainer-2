using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class SingleFlashcardSet
    {
        public static void Update(List<Noun> nouns, List<SingleFlashcard> flashcards)
        {
            foreach (var noun in nouns)
            {
                if (noun.GermanSingularForm != null)
                {
                    var singularFormFlashcard = flashcards.Find(f => f.ParentId == noun.Id && f.Type == Type.NounSingularForm);

                    string question = $"{noun.EnglishDescription} (singular)";
                    string answer = $"{noun.Gender.ToArticle()} {noun.GermanSingularForm}";
                    var flashcardCandidate = new SingleFlashcard(noun.Id, Type.NounSingularForm, question, answer);

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
                    var flashcardCandidate = new SingleFlashcard(noun.Id, Type.NounPluralForm, question, answer);

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

        public static void Update(List<Adjective> adjectives, List<SingleFlashcard> flashcards)
        {
            foreach (var adjective in adjectives)
            {
                {
                    var positiveDegreeFlashcard = flashcards.Find(f => f.ParentId == adjective.Id && f.Type == Type.AdjectivePositiveDegree);

                    string question = $"{adjective.EnglishDescription} (positive)";
                    string answer = adjective.PositiveDegree;
                    var flashcardCandidate = new SingleFlashcard(adjective.Id, Type.AdjectivePositiveDegree, question, answer);

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
                    var flashcardCandidate = new SingleFlashcard(adjective.Id, Type.AdjectiveComparativeDegree, question, answer);

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
                    var flashcardCandidate = new SingleFlashcard(adjective.Id, Type.AdjectiveSuperlativeDegree, question, answer);

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

        public static void Update(List<Verb> verbs, List<SingleFlashcard> flashcards)
        {
            var presentTypes = new List<Type> 
            {
                Type.VerbPresentFirstSingular,
                Type.VerbPresentSecondSingular,
                Type.VerbPresentThirdSingular,
                Type.VerbPresentSecondPlural
            };

            var simplePastTypes = new List<Type>
            {
                Type.VerbSimplePastFirstSingular,
                Type.VerbSimplePastSecondSingular,
                Type.VerbSimplePastThirdSingular,
                Type.VerbSimplePastSecondPlural
            };

            var personalPronouns = new List<PersonalPronoun>
            {
                PersonalPronoun.FirstSingular,
                PersonalPronoun.SecondSingular,
                PersonalPronoun.ThirdSingular,
                PersonalPronoun.SecondPlural
            };

            var questionSuffixes = new List<string> { "ich", "du", "er", "ihr" };

            foreach (var verb in verbs)
            {
                // Present
                for (int i = 0; i < 4; i++)
                {
                    var type = presentTypes[i];
                    var personalPronoun = personalPronouns[i];
                    var suffix = questionSuffixes[i];

                    var flashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == type);

                    string question = $"{verb.EnglishDescription} (Präsens, {suffix})";
                    string answer = verb.Present[personalPronoun];
                    var flashcardCandidate = new SingleFlashcard(verb.Id, type, question, answer);

                    if (flashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (flashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            flashcard.Question = flashcardCandidate.Question;
                            flashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }

                // Simple Past
                for (int i = 0; i < 4; i++)
                {
                    var type = simplePastTypes[i];
                    var personalPronoun = personalPronouns[i];
                    var suffix = questionSuffixes[i];

                    var flashcard = flashcards.Find(f => f.ParentId == verb.Id && f.Type == type);

                    string question = $"{verb.EnglishDescription} (Präteritum, {suffix})";
                    string answer = verb.SimplePast[personalPronoun];
                    var flashcardCandidate = new SingleFlashcard(verb.Id, type, question, answer);

                    if (flashcard == null)
                        flashcards.Add(flashcardCandidate);
                    else
                    {
                        if (flashcard.ComputeHash() != flashcardCandidate.ComputeHash())
                        {
                            flashcard.Question = flashcardCandidate.Question;
                            flashcard.Answer = flashcardCandidate.Answer;
                        }
                    }
                }
            }
        }

        public static List<SingleFlashcard> Load(string path)
        {
            var json = File.ReadAllText(path);
            var flashcardHelpers = JsonSerializer.Deserialize<List<SingleFlashcardHelper>>(json);
            var flashcards = new List<SingleFlashcard>();

            if (flashcardHelpers == null)
                throw new Exception("Flashcard Helpers are null!");

            foreach (var flashcardHelper in flashcardHelpers)
            {
                var parentId = flashcardHelper.ParentId;
                var type = flashcardHelper.Type;
                var question = flashcardHelper.Question;
                var answer = flashcardHelper.Answer;

                var flashcard = new SingleFlashcard(parentId, type, question, answer)
                {
                    LastTrainingTime = flashcardHelper.LastTrainingTime,
                    Cooldown = flashcardHelper.Cooldown
                };

                flashcards.Add(flashcard);
            }

            return flashcards;
        }

        public static void Save(string path, List<SingleFlashcard> flashcards)
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
