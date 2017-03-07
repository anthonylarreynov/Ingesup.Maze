using Ingesup.Maze.Server.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [CollectionDataContract]
    public class CellList : List<Cell>
    {
        public CellList()
            : base()
        {
        }

        public Cell FindCell(int x, int y)
        {
            foreach (Cell cell in this)
            {
                if (cell.Position.Is(x, y))
                {
                    return cell;
                }
            }

            return null;
        }

        public Cell FindCell(Entity.Position position)
        {
            if (position == null)
            {
                return null;
            }

            return FindCell(position.X, position.Y);
        }

        public bool ContainsCell(int x, int y)
        {
            return FindCell(x, y) != null;
        }

        public bool ContainsCell(Entity.Position position)
        {
            return FindCell(position) != null;
        }

        public void AddMissingRange(ICollection<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                if (this.ContainsCell(cell.Position))
                {
                    continue;
                }

                this.Add(cell);
            }
        }
    }
}