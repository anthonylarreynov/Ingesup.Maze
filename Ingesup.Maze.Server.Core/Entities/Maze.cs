using Ingesup.Maze.Core;
using Ingesup.Maze.Server.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public class Maze : Ingesup.Maze.Core.Entity.Maze
    {
        public Maze()
            : base()
        {
            SyncObj = new object();
        }

        [IgnoreDataMember]
        private object SyncObj { get; set; }

        [IgnoreDataMember]
        public Game Game { get; set; }

        [DataMember]
        public CellList Cells { get; set; }

        [DataMember]
        public Cell StartCell { get; set; }

        [DataMember]
        public Cell EndCell { get; set; }

        internal bool IsOutOfBound(int x, int y)
        {
            if (x < 0 || x >= this.Width)
            {
                return true;
            }

            if (y < 0 || y >= this.Height)
            {
                return true;
            }

            return false;
        }

        internal bool IsOutOfBound(Entity.Position position)
        {
            if (position == null)
            {
                return true;
            }

            return IsOutOfBound(position.X, position.Y);
        }

        internal Cell FindCell(int x, int y)
        {
            if (IsOutOfBound(x, y))
            {
                return null;
            }

            return this.Cells.FindCell(x, y);
        }

        internal Cell FindCell(Entity.Position position)
        {
            if (position == null)
            {
                return null;
            }

            return FindCell(position.X, position.Y);
        }

        internal bool ContainsCell(int x, int y)
        {
            return FindCell(x, y) != null;
        }

        internal bool ContainsCell(Entity.Position position)
        {
            return FindCell(position) != null;
        }

        internal Cell FindCell(Entity.CellType cellType)
        {
            foreach (Cell cell in this.Cells)
            {
                if (cell.CellType == cellType)
                {
                    return cell;
                }
            }

            return null;
        }

        internal bool ContainsCell(Entity.CellType cellType)
        {
            return FindCell(cellType) != null;
        }

        internal CellList GetAdjacentCells(Entity.Position position, Entity.CellType? cellType = null)
        {
            CellList cells = new CellList();

            Cell leftCell = FindCell(position.Left());
            if (leftCell != null && (!cellType.HasValue || leftCell.CellType == cellType.Value))
            {
                cells.Add(leftCell);
            }

            Cell rightCell = FindCell(position.Right());
            if (rightCell != null && (!cellType.HasValue || rightCell.CellType == cellType.Value))
            {
                cells.Add(rightCell);
            }

            Cell topCell = FindCell(position.Top());
            if (topCell != null && (!cellType.HasValue || topCell.CellType == cellType.Value))
            {
                cells.Add(topCell);
            }

            Cell bottomCell = FindCell(position.Bottom());
            if (bottomCell != null && (!cellType.HasValue || bottomCell.CellType == cellType.Value))
            {
                cells.Add(bottomCell);
            }

            return cells;
        }

        internal CellList GetNeighborCells(Entity.Position position, Entity.CellType? cellType = null)
        {
            CellList cells = new CellList();

            cells.AddRange(GetAdjacentCells(position, cellType));

            Cell leftTopCell = FindCell(position.Left().Top());
            if (leftTopCell != null && (!cellType.HasValue || leftTopCell.CellType == cellType.Value))
            {
                cells.Add(leftTopCell);
            }

            Cell leftBottomCell = FindCell(position.Left().Bottom());
            if (leftBottomCell != null && (!cellType.HasValue || leftBottomCell.CellType == cellType.Value))
            {
                cells.Add(leftBottomCell);
            }

            Cell rightTopCell = FindCell(position.Right().Top());
            if (rightTopCell != null && (!cellType.HasValue || rightTopCell.CellType == cellType.Value))
            {
                cells.Add(rightTopCell);
            }

            Cell rightBottomCell = FindCell(position.Right().Bottom());
            if (rightBottomCell != null && (!cellType.HasValue || rightBottomCell.CellType == cellType.Value))
            {
                cells.Add(rightBottomCell);
            }

            return cells;
        }

        internal CellList GetVisibleCells(Entity.Position position, Powers powers)
        {
            CellList cells = new CellList();

            var currentCell = FindCell(position);
            var neighborCells = GetNeighborCells(position);

            if (powers.HasFlag(Powers.WallVision))
            {
                for (int x = -3; x <= 3; x++)
                {
                    for (int y = -3; y <= 3; y++)
                    {
                        Vector vector = new Vector(x, y);
                        var cell = currentCell + vector;
                        if (cell == null)
                        {
                            continue;
                        }

                        cells.Add(cell);
                    }
                }
            }
            else
            {
                cells.Add(currentCell);
                cells.AddRange(neighborCells);
            }

            if (powers.HasFlag(Powers.EagleVision))
            {
                foreach (var cell in neighborCells)
                {
                    if (cell.CellType == Entity.CellType.Wall)
                    {
                        continue;
                    }

                    Vector vector = Vector.Create(position, cell.Position);
                    var nextCell = cell;
                    do
                    {
                        nextCell += vector;
                        if (nextCell != null)
                        {
                            cells.Add(nextCell);

                            if (nextCell.CellType == Entity.CellType.Empty)
                            {
                                cells.AddMissingRange(GetNeighborCells(nextCell.Position));
                            }
                        }
                    }
                    while (nextCell != null && nextCell.CellType != Entity.CellType.Wall);
                }
            }

            return cells;
        }

        internal Cell FindCornerCell(Entity.VerticalSide vSide, Entity.HorizontalSide hSide, Entity.CellType cellType)
        {
            int x = (hSide == Entity.HorizontalSide.Left) ? 0 : this.Width - 1;
            int y = (vSide == Entity.VerticalSide.Top) ? 0 : this.Height - 1;
            Cell firstCell = FindCell(x, y);
            if (firstCell.CellType == cellType)
            {
                return firstCell;
            }

            List<Entity.Position> visitedPositions = new List<Entity.Position>();
            visitedPositions.Add(PositionExtension.Create(x, y));
            CellList adjacentCells = GetAdjacentCells(firstCell.Position);
            do
            {
                CellList nextAdjacentCells = new CellList();
                foreach (Cell cell in adjacentCells)
                {
                    if (cell.CellType == cellType)
                    {
                        return cell;
                    }

                    visitedPositions.Add(cell.Position);

                    CellList currentAdjacentCells = GetAdjacentCells(cell.Position);
                    foreach (Cell currentAdjacentCell in currentAdjacentCells)
                    {
                        if (visitedPositions.Contains(currentAdjacentCell.Position))
                        {
                            continue;
                        }

                        nextAdjacentCells.Add(currentAdjacentCell);
                    }
                }

                adjacentCells.Clear();
                adjacentCells.AddRange(nextAdjacentCells);

            } while (adjacentCells.Count > 0);

            return null;
        }

        public void PaintTo(string path)
        {
            lock (SyncObj)
            {
                using (FileStream fs = File.Create(path))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        for (int y = 0; y < this.Height; y++)
                        {
                            for (int x = 0; x < this.Width; x++)
                            {
                                sw.Write(FindCell(x, y).ToChar());
                            }

                            sw.Write(Environment.NewLine);
                        }

                        sw.Close();
                    }

                    fs.Close();
                }
            }
        }

        public void SaveTo(string path)
        {
            lock (SyncObj)
            {
                Serializer.ToFile<Maze>(path, this);
            }
        }

        public static Maze LoadFrom(string path)
        {
            Maze maze = Serializer.FromFile<Maze>(path);
            maze.SyncObj = new object();
            foreach (Cell cell in maze.Cells)
            {
                cell.Maze = maze;
            }

            return maze;
        }

        public static Maze ParseImage(string path)
        {
            Bitmap image = new Bitmap(path);
            Maze maze = new Maze { Width = image.Width, Height = image.Height, Cells = new CellList() };
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    int average = ((color.R + color.B + color.G) / 3);

                    if (average < 200)
                    {
                        maze.Cells.Add(Cell.CreateWall(maze, x, y));
                    }
                    else
                    {
                        maze.Cells.Add(Cell.CreateEmpty(maze, x, y));
                    }
                }
            }

            // Flagging the race...
            maze.StartCell = maze.FindCornerCell(Entity.VerticalSide.Top, Entity.HorizontalSide.Left, Entity.CellType.Empty);
            maze.EndCell = maze.FindCornerCell(Entity.VerticalSide.Bottom, Entity.HorizontalSide.Right, Entity.CellType.Empty);

            return maze;
        }

        internal static Maze Generate(Game game, int width, int height)
        {
            Maze maze = new Maze { Game = game, Width = width, Height = height, Cells = new CellList() };
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    maze.Cells.Add(Cell.CreateWall(maze, x, y));
                }
            }

            // Free the first cell...
            Cell firstCell = maze.FindCell(1, 1);
            firstCell.CellType = Entity.CellType.Empty;

            // Digging the maze...
            Random random = new Random(DateTime.Now.Millisecond);
            CellList walls = maze.GetAdjacentCells(firstCell.Position, Entity.CellType.Wall);
            while (walls.Count > 0)
            {
                int i = random.Next(walls.Count);

                CellList emptyCells = maze.GetAdjacentCells(walls[i].Position, Entity.CellType.Empty);
                if (emptyCells.Count == 0)
                {
                    throw new InvalidOperationException("There is a bug within the generator...");
                }

                if (emptyCells.Count == 1)
                {
                    int deltaX = walls[i].Position.X - emptyCells[0].Position.X;
                    int deltaY = walls[i].Position.Y - emptyCells[0].Position.Y;
                    Cell cell = maze.FindCell(walls[i].Position.X + deltaX, walls[i].Position.Y + deltaY);
                    if (cell != null)
                    {
                        walls[i].CellType = Entity.CellType.Empty;
                        cell.CellType = Entity.CellType.Empty;

                        walls.AddRange(maze.GetAdjacentCells(cell.Position, Entity.CellType.Wall));
                    }
                }

                walls.RemoveAt(i);
            }

            // Flagging the race...
            maze.StartCell = maze.FindCornerCell(Entity.VerticalSide.Top, Entity.HorizontalSide.Left, Entity.CellType.Empty);
            maze.EndCell = maze.FindCornerCell(Entity.VerticalSide.Bottom, Entity.HorizontalSide.Right, Entity.CellType.Empty);

            return maze;
        }

        public static Maze Generate(int width, int height)
        {
            return Generate(null, width, height);
        }
    }
}