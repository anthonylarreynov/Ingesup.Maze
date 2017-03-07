using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Core.Entities
{
    [CollectionDataContract]
    public class PlayerList : List<Player>
    {
        public PlayerList()
            : base()
        {
        }
    }
}