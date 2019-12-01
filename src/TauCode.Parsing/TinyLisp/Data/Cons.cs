namespace TauCode.Parsing.TinyLisp.Data
{
    public class Cons : Element
    {
        private readonly Place _car;
        private readonly Place _cdr;

        public Cons(Element carValue, Element cdrValue)
        {
            _car = new Place(carValue);
            _cdr = new Place(cdrValue);
        }

        public override bool Equals(Element other) => ReferenceEquals(this, other);

        public Place Car => _car;
        public Place Cdr => _cdr;
    }
}
