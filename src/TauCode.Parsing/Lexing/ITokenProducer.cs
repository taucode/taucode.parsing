namespace TauCode.Parsing.Lexing
{
    public interface ITokenProducer
    {
        LexingContext Context { get; set; }

        IToken Produce();
    }
}
