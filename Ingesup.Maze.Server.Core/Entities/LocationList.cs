using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Server.Core.Entities
{
    [CollectionDataContract]
    public class LocationList : List<Location>
    {
        public LocationList()
            : base()
        {
        }
    }
}