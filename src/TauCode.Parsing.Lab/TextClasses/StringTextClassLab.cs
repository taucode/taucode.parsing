namespace TauCode.Parsing.Lab.TextClasses
{
    [TextClass("string")]
    public class StringTextClassLab : TextClassBaseLab
    {
        public static StringTextClassLab Instance { get; } = new StringTextClassLab();

        private StringTextClassLab()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClassLab anotherClass) => null;
    }
}
