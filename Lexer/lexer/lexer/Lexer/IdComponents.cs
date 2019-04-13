using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexer.Lexer
{
    public enum IdComponents
    {
        Letter = 1,
        Number = 2,
        Separator = 3,
        Id = 4, 
        Error = 5
    }
}
