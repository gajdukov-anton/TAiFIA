
namespace lexer.Lexer
{
    public enum States
    {
        Id = 1,
        Error = 2,
        Command = 3, 
        Empty = 4, 
        Const = 5,
        Integer = 6,
        Float = 7,
        Double = 8,
        Separator = 9,
        DateType = 10
    }
}
