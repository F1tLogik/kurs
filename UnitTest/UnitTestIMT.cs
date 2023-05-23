using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp1.Entities;

namespace UnitTest
{
    [TestClass]
    public class UnitTestIMT
    {

        [TestMethod]
        public void CalculationIMT_Int70AndInt180()
        {
            int weight = 70;
            int height = 180;

            double expected = 21.6;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculationIMT_Int10AndInt0()
        {
            int weight = 10;
            int height = 0;

            double expected = 0;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculationIMT_Int0AndInt10()
        {
            int weight = 0;
            int height = 10;

            double expected = 0;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculationIMT_Double67_1AndDouble165_5()
        {
            double weight = 67.1;
            double height = 165.5;

            double expected = 24.5;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculationIMT_DoubleMaxValueAndDoubleMaxValue()
        {
            double weight = double.MaxValue;
            double height = double.MaxValue;

            double expected = 0;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculationIMT_DoubleMinus100AndDoubleMinius200()
        {
            double weight = -100;
            double height = -200;

            double expected = 0;
            double actual = CalculationClass.CalculationIMT(weight, height);

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class UnitTestPasswordCheck
    {
        [TestMethod]
        public void PasswordCheck_null()
        {
            string password = "";
            string excpected = "";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }
        [TestMethod]
        public void PasswordCheck_qwerty()
        {
            string password = "qwerty";
            string excpected = "Слабый пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_Qwerty()
        {
            string password = "Qwerty";
            string excpected = "Простой пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_Qwerty10()
        {
            string password = "Qwerty10";
            string excpected = "Простой пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_Qwerty100()
        {
            string password = "Qwerty100";
            string excpected = "Хороший пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_QwertySymbol10()
        {
            string password = "Qwerty$_10";
            string excpected = "Сложный пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_QwertyйцSymbol_10()
        {
            string password = "Qwertyйц$у_10";
            string excpected = "Очень сложный пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }
        [TestMethod]
        public void PasswordCheck_QwertЙЦуSymbol_10()
        {
            string password = "QwertЙЦу$у_10";
            string excpected = "Ты такой запомнишь вообще?";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_QwertЙЦуSymbol_123456()
        {
            string password = "QwertЙЦу$у_123456";
            string excpected = "Нуу ладно...";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }

        [TestMethod]
        public void PasswordCheck_qQйЙ12Symbol()
        {
            string password = "qQйЙ$";
            string excpected = "Хороший пароль";
            string actual = CalculationClass.PasswordCheck(password);

            Assert.AreEqual(excpected, actual);
        }
    }

}
