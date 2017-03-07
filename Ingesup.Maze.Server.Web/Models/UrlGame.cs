using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class UrlGame : Game
    {
        public UrlGame()
            : base()
        {
        }

        [DataMember]
        public string Url { get; set; }
    }
}