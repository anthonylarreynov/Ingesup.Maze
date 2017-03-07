using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class PlayersGame : Game
    {
        public PlayersGame()
        {
            Players = new List<Player>();
        }

        [DataMember]
        public string ServerDate { get; set; }

        [DataMember]
        public List<Player> Players { get; set; }

        [DataMember]
        public List<Cell> VisitedCells { get; set; }
    }
}