using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lexer.Lexer
{
    public class NumberAnalyzer
    {
        private int _numberOfColumn = 1;
        private int _numberOfStr = 1;
        private TextReader _textReader;
        private string _number;
        public List<Dictionary<NumberComponents, int>> _table;
        private List<int> _finalLvl;
        private List<char> _decimalNumbers;
        private List<char> _octalNumbers;
        private List<char> _hexadecimalNumbers;
        private List<char> _binaryNumbers;
        private NumberSystem _currentNumberSystem = NumberSystem.Decimal;

        private const string CONFIG_DIR = "configs\\number_analyzer\\number_analyzer.txt";
        private const string FINAL_LVL_DIR = "configs\\number_analyzer\\final_lvl.txt";

        private enum NumberSystem
        {
            Decimal = 0,
            Octal = 1,
            Hexadecimal = 2,
            Binary = 3
        }

        public NumberAnalyzer(TextReader textReader)
        {
            _textReader = textReader;
            _table = CreateTransitionTablesForNumber();
            FillNumbers();
        }

        public Response AnalyzeNumber(int numberOfColumn)
        {
            _numberOfColumn = numberOfColumn;
            _currentNumberSystem = NumberSystem.Decimal;
            int currentLvl = 0;
            string currentStr = _textReader.GetCurrentStr();
            NumberComponents component = NumberComponents.Integer;
            component = GetComponentFromChar(currentStr[_numberOfColumn]);
            currentLvl = _table[currentLvl][component];
            while (!IsFinalLvl(currentLvl) && _numberOfColumn < currentStr.Length)
            {
                _number += currentStr[_numberOfColumn];
                _numberOfColumn++;
                if (_numberOfColumn < currentStr.Length)
                {
                    component = GetComponentFromChar(currentStr[_numberOfColumn]);
                    currentLvl = _table[currentLvl][component];
                }
                else
                {
                    component = NumberComponents.Separator;
                    currentLvl = _table[currentLvl][component];
                }
            }

            Response response = new Response();
            response.numberOfColumn = numberOfColumn + 1;
           
            response.numberStr = _textReader.GetNumberStr();
            response.numberSystem = _currentNumberSystem.ToString();
            response.value = _number;
            response.state = GetStateFromComponent(currentLvl);
            while(response.state == States.Error && currentStr[_numberOfColumn] != ' ')
            {
                response.value += currentStr[_numberOfColumn];
                _numberOfColumn++;
                if (_numberOfColumn >= currentStr.Length)
                    break;
            }
            response.numberOfCurColumn = _numberOfColumn;
            _number = "";
            return response;
        }

        private bool IsFinalLvl(int lvl)
        {
            if (_table[lvl].Keys.Count == 1)
            {
                List<NumberComponents> list = _table[lvl].Keys.ToList();
                return _table[lvl][list[0]] == -1 ? true : false;
            }
            return false;
        }

        private States GetStateFromComponent(int lvl)
        {
            NumberComponents components;
            if (IsFinalLvl(lvl))
                components = _table[lvl].Keys.ToList()[0];
            else
                components = NumberComponents.Error;
            if (components == NumberComponents.Double)
                return States.Double;
            else if (components == NumberComponents.Float)
                return States.Float;
            else if (components == NumberComponents.Integer)
                return States.Integer;
            else
                return States.Error;
        }

        private void FillNumbers()
        {
            _decimalNumbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            _octalNumbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7' };
            _hexadecimalNumbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            _binaryNumbers = new List<char>() { '0', '1' };
        }

        private bool IsDigit(char symbol)
        {
            if (_currentNumberSystem == NumberSystem.Decimal)
                return _decimalNumbers.Contains(symbol);
            else if (_currentNumberSystem == NumberSystem.Binary)
                return _binaryNumbers.Contains(symbol);
            else if (_currentNumberSystem == NumberSystem.Hexadecimal)
                return _hexadecimalNumbers.Contains(symbol);
            else if (_currentNumberSystem == NumberSystem.Octal)
                return _octalNumbers.Contains(symbol);
            return false;
        }

        private NumberComponents GetComponentFromChar(char symbol)
        {
            if (IsDigit(symbol))
            {
                if (symbol == '0')
                    return NumberComponents.Zero;
                else
                    return NumberComponents.NotZero;
            }
            else if (symbol == '.')
                return NumberComponents.Point;
            else if (symbol == 'e')
                return NumberComponents.E;
            else if (symbol == 'O')
            {
                _currentNumberSystem = NumberSystem.Octal;
                return NumberComponents.O;
            }
            else if (symbol == 'B')
            {
                _currentNumberSystem = NumberSystem.Binary;
                return NumberComponents.B;
            }
            else if (symbol == 'H')
            {
                _currentNumberSystem = NumberSystem.Hexadecimal;
                return NumberComponents.H;
            }
            else if (symbol == '+' || symbol == '-')
                return NumberComponents.PlusMinus;
            else if (symbol == ' ')
                return NumberComponents.Separator;
            else
                return NumberComponents.UnkowSymbol;
        }

        private List<Dictionary<NumberComponents, int>> CreateTransitionTablesForNumber()
        {
            List<Dictionary<NumberComponents, int>> table = new List<Dictionary<NumberComponents, int>>();
            using (StreamReader streamReader = new StreamReader(CONFIG_DIR))
            {
                string str = "";
                while (str != null)
                {
                    str = streamReader.ReadLine();
                    if (str != null)
                    {
                        string[] configParams = str.Split(new char[] { ' ' });
                        Dictionary<NumberComponents, int> component = new Dictionary<NumberComponents, int>();
                        foreach (string paramStr in configParams)
                        {
                            string[] paramArr = paramStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            component.Add((NumberComponents)Convert.ToInt32(paramArr[1]), Convert.ToInt32(paramArr[0]));
                        }
                        table.Add(component);
                    }
                }
            }
            return table;
        }

        private List<int> ReadFinalLvl()
        {
            List<int> list = new List<int>();
            using (StreamReader streamReader = new StreamReader(FINAL_LVL_DIR))
            {
                string str = "";
                while (str != null)
                {
                    str += streamReader.ReadLine();
                }
                string[] configParams = str.Split(new char[] { ' ' });
                foreach (string item in configParams)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }
            return list;
        }

    }
}
