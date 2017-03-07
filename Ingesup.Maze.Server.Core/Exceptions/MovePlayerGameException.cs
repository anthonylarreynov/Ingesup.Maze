using Ingesup.Maze.Server.Core.Entities;
using System;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Exceptions
{
    public class MovePlayerGameException : PlayerGameException
    {
        public MovePlayerGameException(Game game, Player player, Direction direction)
            : base(game, player)
        {
            this.Direction = direction;
        }

        public MovePlayerGameException(Game game, Player player, Direction direction, string message)
            : base(game, player, message)
        {
            this.Direction = direction;
        }

        public MovePlayerGameException(Game game, Player player, Direction direction, string message, Exception innerException)
            : base(game, player, message, innerException)
        {
            this.Direction = direction;
        }

        public Direction Direction { get; set; }
    }
}