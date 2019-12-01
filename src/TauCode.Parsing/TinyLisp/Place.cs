using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.TinyLisp
{
    public class Place
    {
        private Element _value;

        public Place(Element value)
        {
            this.SetValue(value);
        }

        public Element GetValue()
        {
            return _value;
        }

        public void SetValue(Element value)
        {
            // todo checks
            _value = value;
        }
    }
}
