using System.Collections.Generic;

namespace lexer.Lexer.Separator
{
    public class SeparatorAnalyzer
    {
        public HashSet<char> Separators { private set; get; }
        private TextReader _textReader;


        public SeparatorAnalyzer( TextReader textReader )
        {
            Separators = new HashSet<char>() { ':', ';', ',' };
            _textReader = textReader;
        }

        public Response Analyze( int numberOfColumn )
        {
            string currentStr = _textReader.GetCurrentStr();
            char symbol = currentStr [ numberOfColumn ];
            return new Response()
            {
                state = States.Separator,
                value = symbol.ToString(),
                numberOfColumn = numberOfColumn + 1,
                numberOfCurColumn = numberOfColumn + 1,
                numberStr = _textReader.GetNumberStr()
            };
        }
    }
}
