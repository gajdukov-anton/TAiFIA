
using lexer.Lexer;

namespace SyntacticalAnalyzer.OperatorAnalyzers
{
    public class VarOperatorAnalyzer
    {
        private LexAnalyzer _lexer;

        public VarOperatorAnalyzer( LexAnalyzer lex )
        {
            _lexer = lex;
        }

        public bool IsVar()
        {
            return IsCorrectCommand( "var" ) && IsType() && IsCorrectSeparator( ":" ) && IsCorrectIdList();
        }

        private bool IsCorrectCommand( string commandValue )
        {
            Response response = _lexer.GetNextLex();
            return response.state == States.Command && response.value == commandValue;
        }

        private bool IsType()
        {
            Response response = _lexer.GetNextLex();
            return response.state == States.DateType;
        }

        private bool IsCorrectSeparator( string separatorValue )
        {
            Response response = _lexer.GetNextLex();
            return response.state == States.Separator && response.value == separatorValue;
        }

        private bool IsCorrectSeparator( Response response, string separatorValue )
        {
            return response.state == States.Separator && response.value == separatorValue;
        }

        private bool IsCorrectIdList()
        {
            return IsCorrectId() && IsCorrectRightPart();
        }

        private bool IsCorrectId()
        {
            Response response = _lexer.GetNextLex();
            return response.state == States.Id;
        }

        private bool IsCorrectRightPart()
        {
            Response response = _lexer.GetNextLex();
            if ( IsCorrectSeparator( response, "," ) )
            {
                if ( IsCorrectId() )
                {
                    return IsCorrectRightPart();
                }
            }
            else if ( IsCorrectSeparator( response, ";" ) )
            {
                return true;
            }

            return false;
        }
    }
}
