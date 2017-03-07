using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
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