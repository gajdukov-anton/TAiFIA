
using lexer.Lexer;
using SyntacticalAnalyzer.OperatorAnalyzers;
using System;

namespace SyntacticalAnalyzer
{
    class Program
    {
        static void Main( string [] args )
        {
            if ( args.Length >= 0 )
            {
                TextReader textReader = new TextReader( "var.txt" );
                LexAnalyzer lexer = new LexAnalyzer( textReader );
                VarOperatorAnalyzer varOperatorAnalyzer = new VarOperatorAnalyzer( lexer );
                Console.WriteLine( varOperatorAnalyzer.IsVar() );
            }
            else
            {
                Console.WriteLine( "Отсутствует имя файла" );
            }
        }
    }
}
