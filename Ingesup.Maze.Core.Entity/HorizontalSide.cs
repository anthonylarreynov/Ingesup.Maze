using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public enum HorizontalSide
    {
        [EnumMember]
        Left,
        [EnumMember]
        Right,
    }
}