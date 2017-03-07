using Ingesup.Maze.Server.Core.Entities;
using System;
using System.Collections.Generic;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Extensions
{
    public static class PositionExtension
    {
        public static Entity.Position Create(int x, int y)
        {
            return new Entity.Position { X = x, Y = y };
        }

        public static bool Is(this Entity.Position position, int x, int y)
        {
            return position == Create(x, y);
        }

        public static Entity.Position Left(this Entity.Position position)
        {
            return PositionExtension.Create(position.X - 1, position.Y);
        }

        public static Entity.Position Right(this Entity.Position position)
        {
            return PositionExtension.Create(position.X + 1, position.Y);
        }

        public static Entity.Position Top(this Entity.Position position)
        {
            return PositionExtension.Create(position.X, position.Y - 1);
        }

        public static Entity.Position Bottom(this Entity.Position position)
        {
            return PositionExtension.Create(position.X, position.Y + 1);
        }

        public static Entity.Position Move(this Entity.Position position, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Top(position);
                case Direction.Right: return Right(position);
                case Direction.Down: return Bottom(position);
                case Direction.Left: return Left(position);
                default: throw new ArgumentOutOfRangeException("direction", string.Format("The value '{0} is not recognized as a correction direction value.", direction));
            }
        }

        public static Entity.Position Move(this Entity.Position position, Vector vector)
        {
            return PositionExtension.Create(position.X + vector.X, position.Y + vector.Y);
        }

        public static Entity.Position Find(this List<Entity.Position> positions, int x, int y)
        {
            if (positions == null)
            {
                return null;
            }

            foreach (Entity.Position position in positions)
            {
                if (Is(position, x, y))
                {
                    return position;
                }
            }

            return null;
        }

        public static Entity.Position Find(this List<Entity.Position> positions, Entity.Position position)
        {
            if (position == null)
            {
                return null;
            }

            return Find(positions, position.X, position.Y);
        }

        public static bool Contains(this List<Entity.Position> positions, int x, int y)
        {
            return Find(positions, x, y) != null;
        }

        public static bool Contains(this List<Entity.Position> positions, Entity.Position position)
        {
            return Find(positions, position) != null;
        }
    }
}