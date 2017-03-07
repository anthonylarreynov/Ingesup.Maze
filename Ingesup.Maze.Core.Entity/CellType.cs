using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public enum CellType
    {
        [EnumMember]
        Empty,
        [EnumMember]
        Wall
    }
}