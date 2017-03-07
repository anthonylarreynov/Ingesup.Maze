using Ingesup.Maze.Server.Core.Entities;
using System;
using System.Collections.Generic;

namespace Ingesup.Maze.Server.Core.Comparers
{
    public class FinishTimePlayerComparer : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x != null && y == null)
            {
                return 1;
            }

            if (x == null && y != null)
            {
                return -1;
            }

            return TimeSpan.Compare(x.FinishTime, y.FinishTime);
        }
    }

    public class MovePlayerComparer : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x != null && y == null)
            {
                return 1;
            }

            if (x == null && y != null)
            {
                return -1;
            }

            return x.NbMove.CompareTo(y.NbMove);
        }
    }
}