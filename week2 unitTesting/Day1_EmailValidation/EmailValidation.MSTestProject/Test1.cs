
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day1_EmailValidation;

namespace EmailValidation.MSTestProject
{
    [TestClass]
    public class Test1
    {
        private EmailValidator validator;


        [TestInitialize]
        public void TestInitialize()
        {
            validator = new EmailValidator();
        }



        [TestMethod]
        public void IsEmailValid_ShouldReturnFalseForInvalidEmail()
        {
            string input = "Invalid_email.com";
            bool result = validator.IsEmailValid(input);
            Assert.IsFalse(result);//we are checking for invalid Email value

        }

        [TestMethod]
        public void IsEmailValid_ShouldReturnTrueForValidEmail()
        {
            string input = "Valid@example.com";
            bool result = validator.IsEmailValid(input);
            Assert.IsTrue(result);//we are checking for invalid Email value

        }


    }
}
