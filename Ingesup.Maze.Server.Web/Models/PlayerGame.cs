using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class PlayerGame : Game
    {
        public PlayerGame()
            : base()
        {
        }

        [DataMember]
        public Player Player { get; set; }
    }
}