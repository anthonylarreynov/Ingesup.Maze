using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public class Cell
    {
        public Cell()
        {
        }

        [DataMember]
        public Position Position { get; set; }

        [DataMember]
        public CellType CellType { get; set; }
    }
}