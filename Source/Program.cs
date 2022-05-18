using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var app = new Application();
                app.ProcessArgs(args);
            }
            catch (Exception exception)
            {
                Utility.WriteRedLine($"Error! {exception.Message}");
            }
        }
    }
}
