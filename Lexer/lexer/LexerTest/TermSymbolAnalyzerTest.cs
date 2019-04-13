using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTest
{
    using lexer.Lexer;
    [TestClass]
    public class TermSymbolAnalyzerTest
    {
        [TestMethod]
        public void AnalyzeStringTest()
        {
            TextReader textReader = new TextReader();
            TermSymbolAnalyzer analyzer = new TermSymbolAnalyzer(new Dictionary<string, int>());
            textReader.SetFileName("testFiles\\TermSymbolsAnalyzerTest\\code.txt");
            string str = analyzer.AnalyzeString(textReader.ReadStringFromFile());
            Assert.AreEqual("if a  b a b ", str);
        }
    }
}
