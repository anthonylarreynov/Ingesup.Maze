using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public enum Power
    {
        [EnumMember]
        SpeedUp = 1,
        [EnumMember]
        EagleVision = 2,
        [EnumMember]
        WallVision = 4,
        [EnumMember]
        Duplicate = 8
    }
}