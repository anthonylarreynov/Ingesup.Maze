using Ingesup.Maze.Server.Core.Entities;
using System;

namespace Ingesup.Maze.Server.Core.Exceptions
{
    public class PlayerGameException : GameException
    {
        public PlayerGameException(Game game, Player player)
            : base(game)
        {
            this.Player = player;
        }

        public PlayerGameException(Game game, Player player, string message)
            : base(game, message)
        {
            this.Player = player;
        }

        public PlayerGameException(Game game, Player player, string message, Exception innerException)
            : base(game, message, innerException)
        {
            this.Player = player;
        }

        public Player Player { get; set; }
    }
}