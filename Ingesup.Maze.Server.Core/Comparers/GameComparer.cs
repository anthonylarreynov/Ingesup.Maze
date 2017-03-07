using Ingesup.Maze.Server.Core.Entities;
using System;
using System.Collections.Generic;

namespace Ingesup.Maze.Server.Core.Comparers
{
    // From most recent to oldest...
    public class CreateDateGameComparer : IComparer<Game>
    {
        public int Compare(Game x, Game y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x != null && y == null)
            {
                return -1;
            }

            if (x == null && y != null)
            {
                return 1;
            }

            return DateTime.Compare(y.CreateDate, x.CreateDate);
        }
    }
}