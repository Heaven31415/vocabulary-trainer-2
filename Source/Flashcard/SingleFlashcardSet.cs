using System.Text.Json;
using System.Text.Json.Serialization;
using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Word;
using static VocabularyTrainer2.Source.Flashcard.Flashcard;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class SingleFlashcardSet
    {
        private readonly string _fileName;
        private readonly List<SingleFlashcard> _flashcards;

        public List<SingleFlashcard> Flashcards { get { return _flashcards; } }

        public SingleFlashcardSet(string fileName)
        {
            _flashcards = new List<SingleFlashcard>();

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

            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };
                var rawFlashcards = JsonSerializer.Deserialize<List<SingleFlashcard.Raw>>(json, options);

                if (rawFlashcards == null)
                    throw new IOException($"{fileName} contains invalid content. Unable to deserialize it.");

                foreach (var rawFlashcard in rawFlashcards)
                {
                    var parentId = rawFlashcard.ParentId;
                    var type = rawFlashcard.Type;
                    var question = rawFlashcard.Question;
                    var answer = rawFlashcard.Answer;

                    _flashcards.Add(new SingleFlashcard(parentId, type, question, answer)
                    {
                        LastTrainingTime = rawFlashcard.LastTrainingTime,
                        Cooldown = rawFlashcard.Cooldown,
                        Results = rawFlashcard.Results
                    });
                }
            }
        }

        public void SaveToFileAsJson()
        {
            Utility.SaveToFileAsJson(_fileName, _flashcards);
        }

        public void AddFlashcardsFromAdjectives(List<Adjective> adjectives)
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
            var id = adjective.Id;
            var type = FlashcardType.AdjectivePositiveDegree;
            var description = adjective.Description;
            var positiveDegree = adjective.PositiveDegree;

            var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
            var candidate = new SingleFlashcard(id, type, $"{description} ({suffix})", positiveDegree);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Question = candidate.Question;
                flashcard.Answer = candidate.Answer;
            }
        }

        private void AddComparativeDegreeFlashcard(Adjective adjective, string suffix)
        {
            if (adjective.ComparativeDegree == null)
                return;

            var id = adjective.Id;
            var type = FlashcardType.AdjectiveComparativeDegree;
            var description = adjective.Description;
            var comparativeDegree = adjective.ComparativeDegree;

            var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
            var candidate = new SingleFlashcard(id, type, $"{description} ({suffix})", comparativeDegree);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Question = candidate.Question;
                flashcard.Answer = candidate.Answer;
            }
        }

        private void AddSuperlativeDegreeFlashcard(Adjective adjective, string suffix)
        {
            if (adjective.SuperlativeDegree == null)
                return;

            var id = adjective.Id;
            var type = FlashcardType.AdjectiveSuperlativeDegree;
            var description = adjective.Description;
            var superlativeDegree = adjective.SuperlativeDegree;

            var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
            var candidate = new SingleFlashcard(id, type, $"{description} ({suffix})", superlativeDegree);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Question = candidate.Question;
                flashcard.Answer = candidate.Answer;
            }
        }

        public void AddFlashcardsFromNouns(List<Noun> nouns)
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

            var id = noun.Id;
            var type = FlashcardType.NounSingularForm;
            var description = noun.Description;
            var singularForm = noun.SingularForm;

            var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
            var candidate = new SingleFlashcard(id, type, $"{description} ({suffix})", singularForm);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Question = candidate.Question;
                flashcard.Answer = candidate.Answer;
            }
        }

        private void AddPluralFormFlashcard(Noun noun, string suffix)
        {
            if (noun.PluralForm == null)
                return;

            var id = noun.Id;
            var type = FlashcardType.NounPluralForm;
            var description = noun.Description;
            var pluralForm = noun.PluralForm;

            var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
            var candidate = new SingleFlashcard(id, type, $"{description} ({suffix})", pluralForm);

            if (flashcard == null)
                _flashcards.Add(candidate);
            else if (flashcard.ComputeHash() != candidate.ComputeHash())
            {
                flashcard.Question = candidate.Question;
                flashcard.Answer = candidate.Answer;
            }
        }

        public void AddFlashcardsFromOthers(List<Other> others)
        {
            foreach (var other in others)
            {
                var id = other.Id;
                var type = FlashcardType.Other;
                var question = other.Question;
                var answer = other.Answer;

                var flashcard = _flashcards.Find(f => f.ParentId == id && f.Type == type);
                var candidate = new SingleFlashcard(id, type, question, answer);

                if (flashcard == null)
                    _flashcards.Add(candidate);
                else if (flashcard.ComputeHash() != candidate.ComputeHash())
                {
                    flashcard.Question = candidate.Question;
                    flashcard.Answer = candidate.Answer;
                }
            }
        }

        public void AddFlashcardsFromVerbs(List<Verb> verbs)
        {
            throw new NotImplementedException();
        }
    }
}
