using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexer.Lexer
{
    public enum NumberComponents
    {
        UnkowSymbol = 0,
        NotZero = 1,
        Number = 2,
        Separator = 3,
        Point = 4,
        E = 5,
        Zero = 6,
        O = 7,
        B = 8,
        H = 9,
        Error = 10,
        PlusMinus = 11,
        Integer = 12,
        Double = 13,
        Float = 14
    }
}
