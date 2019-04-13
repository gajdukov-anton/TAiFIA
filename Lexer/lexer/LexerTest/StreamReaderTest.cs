using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LexerTest
{

    [TestClass]
    public class TextReaderTest
    {
        [TestMethod]
        public void TestReadStringFromFile()
        {
            TextReader textReader = new TextReader();
            Assert.AreEqual(null, textReader.ReadStringFromFile());
            textReader.SetFileName("wwqr");
            Assert.AreEqual(null, textReader.ReadStringFromFile());
            textReader.SetFileName("testFiles\\TextReaderTestFiles\\test1.txt");
            Assert.AreEqual("В соответствии с ", textReader.ReadStringFromFile());
            Assert.AreEqual("принятыми соглашениями переименуем класс", textReader.ReadStringFromFile());
            Assert.AreEqual(null, textReader.ReadStringFromFile());
        }
    }
}
