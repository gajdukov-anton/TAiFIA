using System;
using System.Collections.Generic;
using System.IO;

namespace lexer.Lexer
{
    public class LetterAnalyzer
    {
        private int _numberOfColumn = 1;
        private int _numberOfStr = 1;
        private TextReader _textReader;
        private string _id;
        public List<Dictionary<IdComponents, int>> _table;
        private Dictionary<string, string> _specialWords;
       
        private const string CONFIG_DIR = "configs\\letter_analyzer\\letter_analyzer.txt";
        private const string SPECIAL_WORDS_DIR = "configs\\letter_analyzer\\special_words.txt";

        public LetterAnalyzer(TextReader textReader)
        {
            _textReader = textReader;
            _table = CreateTransitionTablesForLetter();
            _specialWords = ReadSpecialWords();
        }

        public Response AnalyzeLetter(int numberOfColumn)
        {
            _numberOfColumn = numberOfColumn;
            int currentLvl = 0;
            IdComponents component;
            string currentStr = _textReader.GetCurrentStr();
            while (currentLvl < 2 && _numberOfColumn < currentStr.Length)
            {
                _id += currentStr[_numberOfColumn];
                _numberOfColumn++;
                if (_numberOfColumn < currentStr.Length)
                {
                    component = GetComponentFromChar(currentStr[_numberOfColumn]);
                    currentLvl = _table[currentLvl][component];
                }
            }
            Response response = new Response();
            response.numberOfColumn = numberOfColumn + 1;
            response.numberOfCurColumn = _numberOfColumn;
            response.numberStr = _textReader.GetNumberStr();
            response.value = _id;
            _id = "";
            if (currentLvl <= 3)
                response.state = DefineTypeId(response.value);
            else
                response.state = States.Error;
            return response;
        }

        private IdComponents GetComponentFromChar(char symbol)
        {
            if (char.IsLetter(symbol))
                return IdComponents.Letter;
            else if (char.IsDigit(symbol))
                return IdComponents.Number;
            else if (symbol == ' ')
                return IdComponents.Separator;
            else
                return IdComponents.Error;
        }

        private States DefineTypeId(string id)
        {
            if (_specialWords.ContainsKey(id))
            {
                switch ( _specialWords [ id ] )
                {
                    case "command":
                        return States.Command;
                    case "dataType":
                        return States.DateType;
                    case "const":
                        return States.Const;
                }
               // return _specialWords[id] == "command" ? States.Command : States.Const;
            }
            return States.Id;
        }

       
        private List<Dictionary<IdComponents, int>> CreateTransitionTablesForLetter()
        {
            List<Dictionary<IdComponents, int>> table = new List<Dictionary<IdComponents, int>>();
            using (StreamReader streamReader = new StreamReader(CONFIG_DIR))
            {
                string str = "";
                while (str != null)
                {
                    str = streamReader.ReadLine();
                    if (str != null)
                    {
                        string[] configParams = str.Split(new char[] { ' ' });
                        Dictionary<IdComponents, int> component = new Dictionary<IdComponents, int>();
                        foreach (string paramStr in configParams)
                        {
                            string[] paramArr = paramStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);                        
                            component.Add((IdComponents)Convert.ToInt32(paramArr[1]), Convert.ToInt32(paramArr[0]));
                        }
                        table.Add(component);
                    }
                }
            }
            return table;
        }

        private Dictionary<string, string> ReadSpecialWords()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            using (StreamReader streamReader = new StreamReader(SPECIAL_WORDS_DIR))
            {
                string  str = streamReader.ReadLine();
                while (str != null)
                {
                    string[] configParams = str.Split(new char[] { ' ' });
                    dictionary.Add(configParams[0], configParams[1]);
                    str = streamReader.ReadLine();
                }
            }
            return dictionary;
        }

    }
}
