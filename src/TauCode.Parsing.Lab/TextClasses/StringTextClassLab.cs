namespace TauCode.Parsing.Lab.TextClasses
{
    [TextClass("string")]
    public class StringTextClassLab : TextClassBaseLab
    {
        public static StringTextClassLab Instance { get; } = new StringTextClassLab();

        private StringTextClassLab()
        {   
        }

        public override string TryConvertFrom(string text, ITextClassLab anotherClass) => null;
    }
}
