using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VocabularyTrainer2
{
    internal class Statistics
    {
        private class Data
        {
            public int Answers { get; set; }
            public int GoodAnswers { get; set; }
            public int BadAnswers { get; set; }
            public float SuccessRatio { get; set; }
        }

        private readonly string path;
        private readonly Dictionary<DateTime, Data> data;

        public Statistics(string path)
        {
            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<Dictionary<DateTime, Data>>(json);

            if (data == null)
                throw new Exception($"Unable to deserialize data from this path: '{path}'.");

            this.path = path;
            this.data = data;

            Display();
        }

        public void OnQuestionAnswered(bool success)
        {
            var today = DateTime.Today;

            if (!this.data.ContainsKey(today))
                this.data[today] = new Data();

            var data = this.data[today];

            data.Answers++;

            if (success)
                data.GoodAnswers++;
            else
                data.BadAnswers++;

            data.SuccessRatio = (float)data.GoodAnswers / data.Answers;

            Save();
        }

        private void Display()
        {
            var total = new Data();

            foreach (var (_, data) in data)
            {
                total.Answers += data.Answers;
                total.GoodAnswers += data.GoodAnswers;
                total.BadAnswers += data.BadAnswers;
            }

            total.SuccessRatio = (float)total.GoodAnswers / total.Answers;

            var answers = total.Answers;
            var successRatio = (total.SuccessRatio * 100).ToString("0.00");

            Utility.WriteLine($"You answered already {answers} flashcards with {successRatio}% success ratio!");

            Utility.Write("Press enter to continue... ");
            Utility.ReadLine();
            Console.Clear();
        }

        private void Save()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }
    }
}
