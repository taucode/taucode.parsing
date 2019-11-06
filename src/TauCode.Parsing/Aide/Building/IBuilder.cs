namespace TauCode.Parsing.Aide.Building
{
    public interface IBuilder
    {
        void AddLink(IBuilder to);
    }
}
