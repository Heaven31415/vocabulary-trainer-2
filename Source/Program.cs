namespace VocabularyTrainer2.Source
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var application = new Source.Application();
                application.Run();
            }
            catch (Exception ex)
            {
                Source.Common.Utility.WriteRedLine(ex.Message);
            }
        }
    }
}
