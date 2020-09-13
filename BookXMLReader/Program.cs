using System;

namespace BookXMLReader
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleInterface consoleInterface = new ConsoleInterface();
            consoleInterface.Start();

            Console.ReadKey();
        }
    }
}
