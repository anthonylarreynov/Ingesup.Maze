using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class UrlPlayerGame : PlayerGame
    {
        public UrlPlayerGame()
            : base()
        {
        }

        [DataMember]
        public string Url { get; set; }
    }
}