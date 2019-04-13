using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface ITextReader
{
    void SetFileName(string fileName);

    string ReadStringFromFile();

    void CloseStreamReader();
}

