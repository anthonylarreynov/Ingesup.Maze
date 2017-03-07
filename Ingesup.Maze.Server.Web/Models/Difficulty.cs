using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
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