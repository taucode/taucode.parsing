namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingSplitter : IParsingUnit
    {
        void AddWay(IParsingUnit way);
    }
}
