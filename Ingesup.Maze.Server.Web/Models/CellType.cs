using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public enum CellType
    {
        [EnumMember]
        Empty,
        [EnumMember]
        Wall,
        [EnumMember]
        Start,
        [EnumMember]
        End
    }
}