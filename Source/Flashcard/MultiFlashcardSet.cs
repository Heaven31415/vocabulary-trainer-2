using System.Text.Json;
using System.Text.Json.Serialization;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Word;
using static VocabularyTrainer2.Source.Flashcard.Flashcard;
using static VocabularyTrainer2.Source.Word.Verb;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class MultiFlashcardSet
    {
        private readonly string _fileName;
        private readonly List<MultiFlashcard> _flashcards;

        public List<MultiFlashcard> Flashcards { get { return _flashcards; } }

        public MultiFlashcardSet(string fileName)
        {
            _flashcards = new List<MultiFlashcard>();

            LoadFlashcardsFromFile(fileName);

            _fileName = fileName;

            SaveToFileAsJson();
        }

        private void LoadFlashcardsFromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null, empty or whitespace.", nameof(fileName));

            var extension = Path.GetExtension(fileName);

            if (extension == null || extension != ".json")
                throw new ArgumentException("fileName needs to have a .json extension.", nameof(fileName));

            if (!File.Exists(fileName))
                return;

            var json = File.ReadAllText(fileName);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var rawFlashcards = JsonSerializer.Deserialize<List<MultiFlashcard.Raw>>(json, options);

            if (rawFlashcards == null)
                throw new IOException($"{fileName} contains invalid content. Unable to deserialize it.");

            foreach (var rawFlashcard in rawFlashcards)
            {
                var parentId = rawFlashcard.ParentId;
                var type = rawFlashcard.Type;
                var questions = rawFlashcard.Questions;
                var answers = rawFlashcard.Answers;

                _flashcards.Add(new MultiFlashcard(parentId, type, questions, answers)
                {
                    LastTrainingTime = rawFlashcard.LastTrainingTime,
                    Cooldown = rawFlashcard.Cooldown,
                    Results = rawFlashcard.Results
                });
            }
        }

        public void SaveToFileAsJson()
        {
            Utility.SaveToFileAsJson(_fileName, _flashcards);
        }

        public void AddFlashcardsFromVerbs(List<Verb> verbs)
        {
            var suffixes = new string[] { "wir", "Sie" };
            var perfektSuffixes = new string[] { "ich", "du", "er", "wir", "ihr", "Sie" };

            foreach (var verb in verbs)
            {
                AddPresentFlashcard(verb, "Präsens", suffixes);
                AddSimplePastFlashcard(verb, "Präteritum", suffixes);
                AddPerfektFlashcard(verb, "Perfekt", perfektSuffixes);
            }
        }

        private void AddPresentFlashcard(Verb verb, string prefix, string[] suffixes)
        {
            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbPresentFirstOrThirdPlural);

            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{verb.Description} ({prefix}, {suffixes[0]})");
            answers.Add(verb.Present[PersonalPronoun.FirstPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[1]})");
            answers.Add(verb.Present[PersonalPronoun.ThirdPlural]);

            var candidate = new MultiFlashcard(verb.Id, FlashcardType.VerbPresentFirstOrThirdPlural, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddSimplePastFlashcard(Verb verb, string prefix, string[] suffixes)
        {
            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbSimplePastFirstOrThirdPlural);

            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{verb.Description} ({prefix}, {suffixes[0]})");
            answers.Add(verb.SimplePast[PersonalPronoun.FirstPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[1]})");
            answers.Add(verb.SimplePast[PersonalPronoun.ThirdPlural]);

            var candidate = new MultiFlashcard(verb.Id, FlashcardType.VerbSimplePastFirstOrThirdPlural, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }

        private void AddPerfektFlashcard(Verb verb, string prefix, string[] suffixes)
        {
            var flashcard = _flashcards.Find(f => f.ParentId == verb.Id && f.Type == FlashcardType.VerbPerfekt);

            var questions = new List<string>();
            var answers = new List<string>();

            questions.Add($"{verb.Description} ({prefix}, {suffixes[0]})");
            answers.Add(verb.Perfekt[PersonalPronoun.FirstSingular]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[1]})");
            answers.Add(verb.Perfekt[PersonalPronoun.SecondSingular]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[2]})");
            answers.Add(verb.Perfekt[PersonalPronoun.ThirdSingular]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[3]})");
            answers.Add(verb.Perfekt[PersonalPronoun.FirstPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[4]})");
            answers.Add(verb.Perfekt[PersonalPronoun.SecondPlural]);

            questions.Add($"{verb.Description} ({prefix}, {suffixes[5]})");
            answers.Add(verb.Perfekt[PersonalPronoun.ThirdPlural]);

            var candidate = new MultiFlashcard(verb.Id, FlashcardType.VerbPerfekt, questions, answers);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Questions = candidate.Questions;
                flashcard.Answers = candidate.Answers;
            }
        }
    }
}
