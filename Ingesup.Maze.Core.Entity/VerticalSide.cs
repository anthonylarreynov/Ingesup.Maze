using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public enum VerticalSide
    {
        [EnumMember]
        Top,
        [EnumMember]
        Bottom
    }
}