using System;

namespace TauCode.Parsing
{
    public struct Position : IEquatable<Position>, IComparable<Position>
    {
        public static readonly Position Zero = new Position(0, 0);
        public static Position TodoErrorPosition => throw new NotImplementedException();

        public Position(int line, int column)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(line));
            }

            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            this.Line = line;
            this.Column = column;
        }

        public int Line { get; }
        public int Column { get; }

        public bool Equals(Position other)
        {
            return
                this.Line == other.Line &&
                this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return
                obj is Position other &&
                this.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Line * 397) ^ this.Column;
            }
        }

        public int CompareTo(Position other)
        {
            var lineComparison = this.Line.CompareTo(other.Line);
            if (lineComparison != 0)
            {
                return lineComparison;
            }

            return this.Column.CompareTo(other.Column);
        }

        public override string ToString() => $"Line: {this.Line} Column: {this.Column}";

        public static bool operator ==(Position a, Position b) => a.Equals(b);
        public static bool operator !=(Position a, Position b) => !a.Equals(b);

        public static bool operator <(Position a, Position b) => a.CompareTo(b) > 0;

        public static bool operator >(Position a, Position b) => a.CompareTo(b) < 0;

        public static bool operator >=(Position a, Position b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Position a, Position b) => a.CompareTo(b) <= 0;
    }
}
