using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using TauCode.Extensions;
using TauCode.Lab.Extensions.EmailValidation;

namespace TauCode.Lab.Extensions.Tests
{
    [TestFixture]
    public class EmailValidationExtensionsTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void IsValidEmail_InputProvided_ExpectedResult(TestCaseDto testCase)
        {
            // Arrange
            
            // Act
            var isEmail = testCase.Email.IsValidEmail();

            // Assert
            Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        }

        //[TestCaseSource(nameof(TestCases))]
        //public void TestTodoHere(TestCaseDto testCase)
        //{
        //    var isEmail = testCase.Email.IsValidEmail();
        //    Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        //}

        public static IList<TestCaseDto> TestCases
        {
            get
            {
                var json = typeof(TodoEmailFixture).Assembly.GetResourceText("TestCases.json", true);
                var list = JsonConvert.DeserializeObject<IList<TestCaseDto>>(json);
                return list;
            }
        }
    }
}
