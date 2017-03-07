using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public enum Difficulty
    {
        [EnumMember]
        Easy,
        [EnumMember]
        Medium,
        [EnumMember]
        Hard,
        [EnumMember]
        Extreme
    }
}