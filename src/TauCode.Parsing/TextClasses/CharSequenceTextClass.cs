namespace TauCode.Parsing.TextClasses
{
    [TextClass("char-sequence")]
    public class CharSequenceTextClass : TextClassBase
    {
        public static CharSequenceTextClass Instance { get; } = new CharSequenceTextClass();

        private CharSequenceTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            return null;
        }
    }
}
