using System;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Core.Entity
{
    [DataContract]
    public class Player
    {
        public Player()
        {
        }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}