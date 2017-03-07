using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class Cell
    {
        public Cell()
        {
        }

        [DataMember]
        public CellType CellType { get; set; }

        [DataMember]
        public Position Position { get; set; }
    }
}