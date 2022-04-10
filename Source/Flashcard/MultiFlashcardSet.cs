using System.Text.Json;
using System.Text.Json.Serialization;
using VocabularyTrainer2.Source.Common;

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

            if (File.Exists(fileName))
            {
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
        }

        public void SaveToFileAsJson()
        {
            Utility.SaveToFileAsJson(_fileName, _flashcards);
        }

        public void AddFlashcardsFromVerbs(List<Word.Verb> verbs)
        {
            throw new NotImplementedException();
        }
    }
}
