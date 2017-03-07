using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public class Maze
    {
        public Maze()
        {
        }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }
    }
}