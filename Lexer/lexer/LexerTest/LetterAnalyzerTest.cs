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
    public class LetterAnalyzerTest
    {
        [TestMethod]
        public void TestCreateTransitionTablesForLetter()
        {
            Response response = new Response();
            response.numberOfColumn = 3;
            response.numberStr = 0;
            response.value = "qua";
            response.state = States.Id;
            TextReader textReader = new TextReader();
            textReader.SetFileName("testFiles//LetterAnalyzerTestFiles//test1.txt");
            LetterAnalyzer letterAnalyzer = new LetterAnalyzer(textReader);
            textReader.ReadStringFromFile();
           // Assert.AreEqual(response, compareResponse();
            textReader.ReadStringFromFile();
            //Assert.AreEqual(States.Id, letterAnalyzer.AnalyzeLetter(0));
            Assert.AreEqual(4, letterAnalyzer._table.Count);
        }

        private bool compareResponse(Response response1, Response response2)
        {
            return response1.numberOfColumn == response2.numberOfColumn &&
                response1.numberStr == response2.numberStr && response1.state == response2.state;
        }
    }
}
