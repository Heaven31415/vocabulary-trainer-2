using System.Text.Json;
using System.Text.Json.Serialization;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Word;
using static VocabularyTrainer2.Source.Flashcard.Flashcard;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class FlashcardSet
    {
        private readonly Random _random = new();
        private readonly string _fileName = Config.Instance.FlashcardsFilePath;
        private List<Flashcard> _flashcards = new();

        public List<Flashcard> Flashcards { get { return _flashcards; } }

        public FlashcardSet(List<Adjective> adjectives, List<Noun> nouns, List<Other> others, List<Verb> verbs)
        {
            LoadFlashcardsFromFile();

            AddFlashcardsFromAdjectives(adjectives);
            AddFlashcardsFromNouns(nouns);
            AddFlashcardsFromOthers(others);
            AddFlashcardsFromVerbs(verbs);

            SaveToFileAsJson();
        }

        private void LoadFlashcardsFromFile()
        {
            if (!File.Exists(_fileName))
                return;

            var json = File.ReadAllText(_fileName);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(json, options);

            if (flashcards == null)
                throw new IOException($"File '{_fileName}' contains invalid content. Unable to deserialize it");

            _flashcards = flashcards;
        }

        public void SaveToFileAsJson()
        {
            Utility.SaveToFileAsJson(_fileName, _flashcards);
        }

        public Flashcard? GetRandomFlashcard()
        {
            var availableFlashcards = _flashcards.FindAll(f => f.IsAvailable());

            if (availableFlashcards.Count == 0)
                return null;

            return availableFlashcards[_random.Next(availableFlashcards.Count)];
        }

        private void AddFlashcardsFromAdjectives(List<Adjective> adjectives)
        {
            foreach (var adjective in adjectives)
            {
                AddPositiveDegreeFlashcard(adjective, "positive");
                AddComparativeDegreeFlashcard(adjective, "comparative");
                AddSuperlativeDegreeFlashcard(adjective, "superlative");
            }
        }

        private void AddPositiveDegreeFlashcard(Adjective adjective, string suffix)
        {
            var flashcard = _flashcards.Find(f => f.ParentId == adjective.Id && f.Type == FlashcardType.AdjectivePositiveDegree);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{adjective.Description} ({suffix})");
            answers.Add(adjective.PositiveDegree);

            var candidate = new Flashcard(adjective.Id, FlashcardType.AdjectivePositiveDegree, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddComparativeDegreeFlashcard(Adjective adjective, string suffix)
        {
            if (adjective.ComparativeDegree == null)
                return;

            var flashcard = _flashcards.Find(f => f.ParentId == adjective.Id && f.Type == FlashcardType.AdjectiveComparativeDegree);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{adjective.Description} ({suffix})");
            answers.Add(adjective.ComparativeDegree);

            var candidate = new Flashcard(adjective.Id, FlashcardType.AdjectiveComparativeDegree, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddSuperlativeDegreeFlashcard(Adjective adjective, string suffix)
        {
            if (adjective.SuperlativeDegree == null)
                return;

            var flashcard = _flashcards.Find(f => f.ParentId == adjective.Id && f.Type == FlashcardType.AdjectiveSuperlativeDegree);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{adjective.Description} ({suffix})");
            answers.Add(adjective.SuperlativeDegree);

            var candidate = new Flashcard(adjective.Id, FlashcardType.AdjectiveSuperlativeDegree, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddFlashcardsFromNouns(List<Noun> nouns)
        {
            foreach (var noun in nouns)
            {
                AddSingularFormFlashcard(noun, "singular");
                AddPluralFormFlashcard(noun, "plural");
            }
        }

        private void AddSingularFormFlashcard(Noun noun, string suffix)
        {
            if (noun.SingularForm == null)
                return;

            var flashcard = _flashcards.Find(f => f.ParentId == noun.Id && f.Type == FlashcardType.NounSingularForm);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{noun.Description} ({suffix})");
            answers.Add(noun.SingularForm);

            var candidate = new Flashcard(noun.Id, FlashcardType.NounSingularForm, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddPluralFormFlashcard(Noun noun, string suffix)
        {
            if (noun.PluralForm == null)
                return;

            var flashcard = _flashcards.Find(f => f.ParentId == noun.Id && f.Type == FlashcardType.NounPluralForm);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{noun.Description} ({suffix})");
            answers.Add(noun.PluralForm);

            var candidate = new Flashcard(noun.Id, FlashcardType.NounPluralForm, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddFlashcardsFromOthers(List<Other> others)
        {
            foreach (var other in others)
            {
                var flashcard = _flashcards.Find(f => f.ParentId == other.Id && f.Type == FlashcardType.Other);
                var questions = new List<string>();
                var answers = new List<string>();

                questions.Add(other.Question);
                answers.Add(other.Answer);

                var candidate = new Flashcard(other.Id, FlashcardType.Other, questions, answers);

                if (flashcard == null)
                    _flashcards.Add(candidate);
                else if (flashcard.Hash() != candidate.Hash())
                {
                    flashcard.Questions = candidate.Questions;
                    flashcard.Answers = candidate.Answers;
                }
            }
        }

        private void AddFlashcardsFromVerbs(List<Verb> verbs)
        {
            foreach (var verb in verbs)
            {
                AddSinglePresentFlashcard(verb, "Präsens");
                AddMultiPresentFlashcard(verb, "Präsens");
                AddSingleSimplePastFlashcard(verb, "Präteritum");
                AddMultiSimplePastFlashcard(verb, "Präteritum");
                AddPerfektFlashcard(verb, "Perfekt");
                AddImperativeFlashcard(verb, "Imperativ");
            }
        }

        private void AddSinglePresentFlashcard(Verb verb, string prefix)
        {
            var types = new FlashcardType[]
            {
                FlashcardType.VerbPresentFirstSingular,
                FlashcardType.VerbPresentSecondSingular,
                FlashcardType.VerbPresentThirdSingular,
                FlashcardType.VerbPresentSecondPlural,
            };

            var suffixes = new List<string> { "ich", "du", "er", "ihr" };

            var personalPronouns = new Verb.PersonalPronoun[]
            {
                Verb.PersonalPronoun.FirstSingular,
                Verb.PersonalPronoun.SecondSingular,
                Verb.PersonalPronoun.ThirdSingular,
                Verb.PersonalPronoun.SecondPlural,
            };

            for (int i = 0; i < 4; i++)
            {
                var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == types[i]);
                var questions = new List<string>();
                var answers = new List<string>();

                questions.Add($"{verb.Description} ({prefix}, {suffixes[i]})");
                answers.Add(verb.Present[personalPronouns[i]]);

                var candidate = new Flashcard(verb.Id, types[i], questions, answers);

                if (flashcard == null)
                    _flashcards.Add(candidate);
                else if (flashcard.Hash() != candidate.Hash())
                {
                    flashcard.Questions = candidate.Questions;
                    flashcard.Answers = candidate.Answers;
                }
            }
        }

        private void AddMultiPresentFlashcard(Verb verb, string prefix)
        {
            var suffixes = new List<string> { "wir", "Sie" };

            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbPresentFirstOrThirdPlural);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{verb.Description} ({prefix}, {suffixes[0]})");
            answers.Add(verb.Present[Verb.PersonalPronoun.FirstPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[1]})");
            answers.Add(verb.Present[Verb.PersonalPronoun.ThirdPlural]);

            var candidate = new Flashcard(verb.Id, FlashcardType.VerbPresentFirstOrThirdPlural, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddSingleSimplePastFlashcard(Verb verb, string prefix)
        {
            var types = new FlashcardType[]
            {
                FlashcardType.VerbSimplePastFirstSingular,
                FlashcardType.VerbSimplePastSecondSingular,
                FlashcardType.VerbSimplePastThirdSingular,
                FlashcardType.VerbSimplePastSecondPlural,
            };

            var suffixes = new List<string> { "ich", "du", "er", "ihr" };

            var personalPronouns = new Verb.PersonalPronoun[]
            {
                Verb.PersonalPronoun.FirstSingular,
                Verb.PersonalPronoun.SecondSingular,
                Verb.PersonalPronoun.ThirdSingular,
                Verb.PersonalPronoun.SecondPlural,
            };

            for (int i = 0; i < 4; i++)
            {
                var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == types[i]);
                var questions = new List<string>();
                var answers = new List<string>();

                questions.Add($"{verb.Description} ({prefix}, {suffixes[i]})");
                answers.Add(verb.SimplePast[personalPronouns[i]]);

                var candidate = new Flashcard(verb.Id, types[i], questions, answers);

                if (flashcard == null)
                    _flashcards.Add(candidate);
                else if (flashcard.Hash() != candidate.Hash())
                {
                    flashcard.Questions = candidate.Questions;
                    flashcard.Answers = candidate.Answers;
                }
            }
        }

        private void AddMultiSimplePastFlashcard(Verb verb, string prefix)
        {
            var suffixes = new List<string> { "wir", "Sie" };

            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbSimplePastFirstOrThirdPlural);
            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{verb.Description} ({prefix}, {suffixes[0]})");
            answers.Add(verb.SimplePast[Verb.PersonalPronoun.FirstPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[1]})");
            answers.Add(verb.SimplePast[Verb.PersonalPronoun.ThirdPlural]);

            var candidate = new Flashcard(verb.Id, FlashcardType.VerbSimplePastFirstOrThirdPlural, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddPerfektFlashcard(Verb verb, string prefix)
        {
            var suffixes = new List<string> { "ich", "du", "er", "wir", "ihr", "Sie" };

            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbPerfekt);
            var questions = new List<string>();
            var answers = new List<string>();

            for (var i = 0; i < 6; i++)
            {
                questions.Add($"{verb.Description} ({prefix}, {suffixes[i]})");
                answers.Add(verb.Perfekt[(Verb.PersonalPronoun)i]);
            }

            var candidate = new Flashcard(verb.Id, FlashcardType.VerbPerfekt, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.Hash() != candidate.Hash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddImperativeFlashcard(Verb verb, string prefix)
        {
            var types = new FlashcardType[]
            {
                FlashcardType.VerbImperativeSecondSingular,
                FlashcardType.VerbImperativeSecondPlural,
                FlashcardType.VerbImperativeThirdPlural,
            };

            var suffixes = new List<string> { "du", "ihr", "Sie" };

            var personalPronouns = new Verb.PersonalPronoun[]
            {
                Verb.PersonalPronoun.SecondSingular,
                Verb.PersonalPronoun.SecondPlural,
                Verb.PersonalPronoun.ThirdPlural
            };

            for (int i = 0; i < 3; i++)
            {
                var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == types[i]);
                var questions = new List<string>();
                var answers = new List<string>();

                questions.Add($"{verb.Description} ({prefix}, {suffixes[i]})");
                answers.Add(verb.Imperative[personalPronouns[i]]);

                var candidate = new Flashcard(verb.Id, types[i], questions, answers);

                if (flashcard == null)
                    _flashcards.Add(candidate);
                else if (flashcard.Hash() != candidate.Hash())
                {
                    flashcard.Questions = candidate.Questions;
                    flashcard.Answers = candidate.Answers;
                }
            }
        }
    }
}
