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
            var email = testCase.Email.Replace('␀', '\0');

            // Act
            var isEmail = email.IsValidEmail();

            // Assert
            Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        }

        [TestCaseSource(nameof(ExtraTestCases))]
        public void IsValidEmail_ExtraCases_ExpectedResult(TestCaseDto testCase)
        {
            // Arrange
            var email = testCase.Email.Replace('␀', '\0');

            // Act
            var isEmail = email.IsValidEmail();

            // Assert
            Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        }


        public static IList<TestCaseDto> TestCases
        {
            get
            {
                var json = typeof(EmailValidationExtensionsTests).Assembly.GetResourceText("TestCases.json", true);
                var list = JsonConvert.DeserializeObject<IList<TestCaseDto>>(json);
                return list;
            }
        }

        public static IList<TestCaseDto> ExtraTestCases
        {
            get
            {
                var json = typeof(EmailValidationExtensionsTests).Assembly.GetResourceText("TestCases.Extra.json", true);
                var list = JsonConvert.DeserializeObject<IList<TestCaseDto>>(json);
                return list;
            }
        }
    }
}
