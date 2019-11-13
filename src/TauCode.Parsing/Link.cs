//using System;
//using TauCode.Parsing.Units;

//namespace TauCode.Parsing
//{
//    public class Link : IEquatable<Link>
//    {
//        public Link(string address)
//        {
//            this.Address = address ?? throw new ArgumentNullException(nameof(address));
//        }

//        public string Address { get; }
//        public IUnit ResolvedTarget { get; private set; }

//        public static implicit operator Link(string address)
//        {
//            return new Link(address);
//        }

//        public bool Equals(Link other)
//        {
//            if (ReferenceEquals(null, other)) return false;
//            if (ReferenceEquals(this, other)) return true;
//            return string.Equals(Address, other.Address);
//        }

//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(null, obj)) return false;
//            if (ReferenceEquals(this, obj)) return true;
//            if (obj.GetType() != this.GetType()) return false;
//            return Equals((Link)obj);
//        }

//        public override int GetHashCode()
//        {
//            return this.Address.GetHashCode();
//        }
//    }
//}
