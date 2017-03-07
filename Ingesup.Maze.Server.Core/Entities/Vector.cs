using System;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    public class Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static Vector Create(Entity.Position source, Entity.Position destination)
        {
            return new Vector(destination.X - source.X, destination.Y - source.Y);
        }
    }
}