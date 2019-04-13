using System.Collections.Generic;
using lexer.Lexer.Separator;

namespace lexer.Lexer
{
    public class LexAnalyzer
    {
        private TextReader _textReader;
        private LetterAnalyzer _letterAnalyzer;
        private NumberAnalyzer _numberAnalyzer;
        private SeparatorAnalyzer _separatorAnalyzer;
        private int _numberOfColumn = 0;

        public LexAnalyzer( TextReader textReader )
        {
            _textReader = textReader;
            _letterAnalyzer = new LetterAnalyzer( _textReader );
            _numberAnalyzer = new NumberAnalyzer( _textReader );
            _separatorAnalyzer = new SeparatorAnalyzer( _textReader );
        }

        public void SetFileName( string fileName )
        {
            _textReader.SetFileName( fileName );
        }

        public Response GetNextLex()
        {
            string str = _textReader.GetCurrentStr();
            if ( str == null )
            {
                _numberOfColumn = 0;
                str = _textReader.ReadStringFromFile();
            }
            Response response = new Response();
            while ( str != null )
            {
                while ( str == "" )
                    str = _textReader.ReadStringFromFile();
                if ( str.Length <= _numberOfColumn )
                {
                    _numberOfColumn = 0;
                    str = _textReader.ReadStringFromFile();
                }
                if ( str != null && str != "" )
                {
                    if ( str [ _numberOfColumn ] != ' ' )
                    {
                        response = Analyze( str [ _numberOfColumn ] );
                        if ( response.state != States.Empty )
                            break;

                    }
                }
                _numberOfColumn++;

            }

            return response;
        }

        private Response Analyze( char symbol )
        {
            Response response = new Response();
            if ( char.IsLetter( symbol ) )
            {
                response = _letterAnalyzer.AnalyzeLetter( _numberOfColumn );
                _numberOfColumn = response.numberOfCurColumn;
                return response;
            }
            if ( char.IsDigit( symbol ) )
            {
                response = _numberAnalyzer.AnalyzeNumber( _numberOfColumn );
                _numberOfColumn = response.numberOfCurColumn;
                return response;
            }
            if (IsSeparator(symbol))
            {
                response = _separatorAnalyzer.Analyze( _numberOfColumn );
                _numberOfColumn = response.numberOfCurColumn;
                return response;
            }

            response.state = States.Error;
            response.numberOfColumn = _numberOfColumn + 1;
            response.numberStr = _textReader.GetNumberStr();
            response.value += symbol;
            _numberOfColumn++;
            return response;
        }

        private bool IsSeparator( char symbol )
        {
            return _separatorAnalyzer.Separators.Contains( symbol );
        }

        private void DropCommitedText()
        {
            string str = _textReader.GetCurrentStr();
            while ( str != null )
            {
                if ( _numberOfColumn < str.Length - 1 )
                {
                    if ( str [ _numberOfColumn + 1 ] == '/' && str [ _numberOfColumn ] == '*' )
                    {
                        _numberOfColumn += 2;
                        break;
                    }
                }
                _numberOfColumn++;
                if ( _numberOfColumn >= str.Length )
                {
                    str = _textReader.ReadStringFromFile();
                    _numberOfColumn = 0;
                }
            }
        }

        private bool isEndCommit( string str )
        {
            if ( _numberOfColumn < str.Length - 1 )
            {
                if ( str [ _numberOfColumn + 1 ] == '/' && str [ _numberOfColumn ] == '*' )
                {
                    _numberOfColumn += 2;
                    return true;
                }
            }
            return false;
        }

        private string ReadErrorConstruction( string str )
        {
            string result = "";
            char symbol = str [ _numberOfColumn ];
            while ( symbol != ' ' && _numberOfColumn < str.Length )
            {
                symbol = str [ _numberOfColumn ];
                result += symbol;
                _numberOfColumn++;
            }
            return result;

        }


    }
}
