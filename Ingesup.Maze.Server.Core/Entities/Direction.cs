using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public enum Direction
    {
        [EnumMember]
        Up,
        [EnumMember]
        Right,
        [EnumMember]
        Down,
        [EnumMember]
        Left
    }
}