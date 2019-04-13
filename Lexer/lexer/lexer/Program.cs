using lexer.Lexer;
using System;
using System.Collections.Generic;

namespace lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 0)
            {
                List<Response> responses = new List<Response>();
                TextReader textReader = new TextReader("var.txt");
                LexAnalyzer lexer = new LexAnalyzer(textReader);
                while (!textReader.isGone())
                {
                    Response response = lexer.GetNextLex();
                    if (response.state != States.Empty)
                        responses.Add(response);
                }
                Console.WriteLine(responses.Count);
                PrintResult(responses);
            }
            else
            {
                Console.WriteLine("Отсутствует имя файла");
            }
        }

        private static void PrintResult(List<Response> responses)
        {
            foreach(Response response in responses)
            {
                Console.WriteLine($"Type: {(States)response.state}");
                Console.WriteLine($"Value: {response.value}");
                if (response.numberSystem != "" && response.state != States.Error)
                    Console.WriteLine($"NumberSystem: {response.numberSystem}");
                Console.WriteLine($"str: {response.numberStr}, column: {response.numberOfColumn}");
                Console.WriteLine("");
            }
           
        }
    }
}
