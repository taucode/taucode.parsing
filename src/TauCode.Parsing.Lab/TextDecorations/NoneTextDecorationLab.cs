namespace TauCode.Parsing.Lab.TextDecorations
{
    public class NoneTextDecorationLab : ITextDecorationLab
    {
        public static NoneTextDecorationLab Instance { get; } = new NoneTextDecorationLab();
        private NoneTextDecorationLab()
        {
        }
    }
}
