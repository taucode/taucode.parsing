namespace TauCode.Parsing.Units
{
    public interface IWrapper : IUnit
    {
        IUnit Internal { get; set; }
    }
}
