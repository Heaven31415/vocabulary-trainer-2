using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source
{
    public class Program
    {
        static void Main()
        {
            try
            {
                var app = new Application();
                app.Run();
            }
            catch (Exception exception)
            {
                Utility.WriteRedLine(exception.Message);
            }
        }
    }
}
