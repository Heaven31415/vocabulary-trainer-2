using VocabularyTrainer2.Source.Common;

namespace VocabularyTrainer2.Source
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var application = new Application();
                application.Run();
            }
            catch (Exception ex)
            {
                Utility.WriteRedLine(ex.Message);
            }
        }
    }
}
