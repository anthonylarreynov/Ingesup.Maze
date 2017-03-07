using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    [Flags]
    public enum Powers
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        SpeedUp = 1,
        [EnumMember]
        EagleVision = 2,
        [EnumMember]
        WallVision = 4,
        [EnumMember]
        Duplicate = 8,
        [EnumMember]
        All = SpeedUp | EagleVision | WallVision | Duplicate
    }
}