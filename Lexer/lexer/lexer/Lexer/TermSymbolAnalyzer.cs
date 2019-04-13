using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexer.Lexer
{
    public class TermSymbolAnalyzer
    {
        private Dictionary<string, int> _termSymblosDictionary;
        private List<string> _termSymbols;

        public TermSymbolAnalyzer(Dictionary<string, int> termSymbolsDictionary)
        {
            _termSymblosDictionary = termSymbolsDictionary;
            _termSymbols = createTermSymbols();
        }

        public string AnalyzeString(string str)
        {
            int counter = 0;
            string analyzedStr = "";
            while (counter < str.Length)
            {

                if (counter < str.Length - 1 && _termSymbols.IndexOf(getStringFromChar(str[counter])) != -1
                    && _termSymbols.IndexOf(getStringFromChar(str[counter + 1])) != -1)
                {
                    //_tempSymbols.Add(str.Substring(counter, counter + 1));
                    AddNewTempSymbolToDictionary(str.Substring(counter, counter + 1));
                    analyzedStr += "  ";
                    counter++;
                }
                else if (_termSymbols.IndexOf(getStringFromChar(str[counter])) != -1)
                {
                    AddNewTempSymbolToDictionary(getStringFromChar(str[counter]));
                    analyzedStr += " ";
                }
                else
                {
                    analyzedStr += str[counter];
                }
                counter++;
            }
            return analyzedStr;
        }

        public Dictionary<string, int> GetTermSymbols()
        {
            return _termSymblosDictionary;
        }

        private void AddNewTempSymbolToDictionary(string symbol)
        {
            if (_termSymblosDictionary.ContainsKey(symbol))
            {
                _termSymblosDictionary[symbol]++;
            }
            else
            {
                _termSymblosDictionary.Add(symbol, 0);
            }
        }

        private List<string> createTermSymbols()
        {
            List<string> list = new List<string>
            {
                "!",
                "!=",
                "=",
                "==",
                ">=",
                "<=",
                "<",
                ">",
                "<>",
                ",",
                ".",
                "/",
                ":",
                "-",
                "+",
                "*",
                "&",
                "%",
                "(",
                ")",
                ";"
            };
            return list;
        }

        private string getStringFromChar(char symbol)
        {
            string str = "";
            str += symbol;
            return str;
        }

    }
}
