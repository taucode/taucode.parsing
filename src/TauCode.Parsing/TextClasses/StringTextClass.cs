namespace TauCode.Parsing.TextClasses
{
    [TextClass("string")]
    public class StringTextClass : TextClassBase
    {
        public static StringTextClass Instance { get; } = new StringTextClass();

        private StringTextClass()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass) => null;
    }
}
