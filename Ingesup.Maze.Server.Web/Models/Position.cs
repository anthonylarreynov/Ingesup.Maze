using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Web.Models
{
    [DataContract]
    public class Position
    {
        public Position()
        {
        }

        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
}