using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public class Position
    {
        public Position()
        {
        }

        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Position p = obj as Position;
            if ((object)p == null)
            {
                return false;
            }

            return (X == p.X) && (Y == p.Y);
        }

        public bool Equals(Position p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static bool operator ==(Position a, Position b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
    }
}