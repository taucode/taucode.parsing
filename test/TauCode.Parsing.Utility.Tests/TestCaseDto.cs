﻿namespace TauCode.Parsing.Utility.Tests
{
    public class TestCaseDto
    {
        public TestCaseDto()
        {   
        }

        public TestCaseDto(
            string name,
            string email,
            bool expectedResult,
            string comment)
        {
            this.Name = name;
            this.Email = email;
            this.ExpectedResult = expectedResult;
            this.Comment = comment;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public bool ExpectedResult { get; set; }
        public string Comment { get; set; }

        public override string ToString() => this.Email;
    }
}
