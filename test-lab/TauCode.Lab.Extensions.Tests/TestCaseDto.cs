namespace TauCode.Lab.Extensions.Tests
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

        public override string ToString()
        {
            // todo temp
            if (this.Email == @"first(12345678901234567890123456789012345678901234567890)last@(1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890)iana.org")
            {
                return "\"\"-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------"; 
            }

            return this.Email.ToString();
        }
    }
}
