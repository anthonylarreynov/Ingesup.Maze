using System;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public class Location : Ingesup.Maze.Core.Entity.Position
    {
        public Location()
            : base()
        {
        }

        [DataMember]
        public DateTime CreateDate { get; set; }

        public static Location Create(Entity.Position position)
        {
            return new Location { X = position.X, Y = position.Y, CreateDate = DateTime.Now };
        }
    }
}