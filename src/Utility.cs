namespace VocabularyTrainer2
{
    internal class Utility
    {
        public static void Write(string? value = null, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteLine(string? value = null, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static string ReadLine()
        {
            var line = Console.ReadLine();

            if (line == null)
                throw new Exception("Unable to read line from standard input.");

            return line;
        }
    }
}
