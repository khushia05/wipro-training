
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator_Assignment1;
using System;

namespace calculator_Assignment1.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator calculator;

        [TestInitialize]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [TestMethod]
        public void Add_ValidInputs_ReturnsCorrectResult()
        {
            double result = calculator.Add(2, 3);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Subtract_ValidInputs_ReturnsCorrectResult()
        {
            double result = calculator.Subtract(5, 2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Multiply_ValidInputs_ReturnsCorrectResult()
        {
            double result = calculator.Multiply(4, 2.5);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Divide_ValidInputs_ReturnsCorrectResult()
        {
            double result = calculator.Divide(10, 2);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Divide_ByZero_ThrowsException()
        {
            calculator.Divide(5, 0);
        }

        [TestMethod]
        public void Add_WithZero_ReturnsSameNumber()
        {
            double result = calculator.Add(0, 7);
            Assert.AreEqual(7, result);
        }
    }
}
