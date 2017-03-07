using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class Game
    {
        public Game()
        {
        }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string CreateDate { get; set; }

        [DataMember]
        public string Creator { get; set; }

        [DataMember]
        public Difficulty Difficulty { get; set; }

        [DataMember]
        public int MovePlayerMinInterval { get; set; }

        [DataMember]
        public Maze Maze { get; set; }
    }
}