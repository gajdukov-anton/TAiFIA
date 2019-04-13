using System;
using System.IO;


public class TextReader : ITextReader
{
    private StreamReader _streamReader = null;
    private string _currentStr = "";
    private int _numberStr = 0;
    public TextReader()
    {

    }

    public TextReader(string fileName)
    {
        SetFileName(fileName);
    }

    public void SetFileName(string fileName)
    {
        try
        {
            _streamReader = new StreamReader(fileName);
            _numberStr = 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Файл: {fileName} не найден.");
        }
    }

    public string ReadStringFromFile()
    {
        // return _streamReader != null ? _streamReader.ReadLine() : null;
        if (_streamReader != null)
        {
            _currentStr = _streamReader.ReadLine();
            _numberStr++;
        }
        else
            _currentStr = null;
        return _currentStr;
    }

    public bool isGone()
    {
        return _currentStr == null ? true : false;
    }

    public string GetCurrentStr()
    {
        return _currentStr == "" ? ReadStringFromFile() : _currentStr;
    }

    public int GetNumberStr()
    {
        return _numberStr;
    }

    public void CloseStreamReader()
    {
        _streamReader.Close();
        _streamReader = null;
    }
}
