namespace TauCode.Parsing.TextProcessing
{
    public class EmptyPayload : IPayload
    {
        public static IPayload Value { get; } = new EmptyPayload();

        private EmptyPayload()
        {
        }
    }
}
