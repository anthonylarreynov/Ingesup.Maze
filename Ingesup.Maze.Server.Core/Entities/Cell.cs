using Ingesup.Maze.Server.Core.Extensions;
using System;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public class Cell : Entity.Cell
    {
        public Cell()
            : base()
        {
        }

        [IgnoreDataMember]
        public Maze Maze { get; set; }

        internal static Cell Create(Maze maze, int x, int y, Entity.CellType cellType)
        {
            return new Cell { Maze = maze, Position = PositionExtension.Create(x, y), CellType = cellType };
        }

        internal static Cell CreateEmpty(Maze maze, int x, int y)
        {
            return Create(maze, x, y, Entity.CellType.Empty);
        }

        internal static Cell CreateWall(Maze maze, int x, int y)
        {
            return Create(maze, x, y, Entity.CellType.Wall);
        }

        public static Cell operator +(Cell cell, Vector vector)
        {
            return cell.Maze.FindCell(cell.Position.Move(vector));
        }

        internal char ToChar()
        {
            if (this.Maze != null && this.Maze.StartCell != null && this.Position.Equals(this.Maze.StartCell.Position))
            {
                return 'S';
            }

            if (this.Maze != null && this.Maze.EndCell != null && this.Position.Equals(this.Maze.EndCell.Position))
            {
                return 'E';
            }

            switch (this.CellType)
            {
                case Entity.CellType.Empty: return ' ';
                case Entity.CellType.Wall: return '█';
                default: throw new NotImplementedException(string.Format("The cell type '{0}' is not recognized.", this.CellType));
            }
        }
    }
}