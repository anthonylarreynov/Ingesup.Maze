using Ingesup.Maze.Server.Core.Entities;
using System;

namespace Ingesup.Maze.Server.Core.Exceptions
{
    public class GameException : Exception
    {
        public GameException(Game game)
            : base()
        {
            this.Game = game;
        }

        public GameException(Game game, string message)
            : base(message)
        {
            this.Game = game;
        }

        public GameException(Game game, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Game = game;
        }

        public Game Game { get; set; }
    }
}