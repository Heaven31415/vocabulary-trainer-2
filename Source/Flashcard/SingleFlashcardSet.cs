using System.Text.Json;
using VocabularyTrainer2.Source.Common;

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
                var rawFlashcards = JsonSerializer.Deserialize<List<SingleFlashcard.Raw>>(json);

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

        public void AddFlashcardsFromAdjectives(List<Word.Adjective> adjectives)
        {
            throw new NotImplementedException();
        }

        public void AddFlashcardsFromNouns(List<Word.Noun> nouns)
        {
            throw new NotImplementedException();
        }

        public void AddFlashcardsFromOthers(List<Word.Other> others)
        {
            throw new NotImplementedException();
        }

        public void AddFlashcardsFromVerbs(List<Word.Verb> verbs)
        {
            throw new NotImplementedException();
        }
    }
}
