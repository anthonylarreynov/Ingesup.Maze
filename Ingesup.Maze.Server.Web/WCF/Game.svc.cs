using Ingesup.Maze.Server.Web.Assemblers;
using Ingesup.Maze.Server.Web.WCF.Contract;
using Data = Ingesup.Maze.Server.Core.Entities;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.WCF
{
    public class Game : BaseGame, IGame
    {
        protected override string ServiceName
        {
            get { return "Game.svc"; }
        }

        public Model.PlayerGame CreateGame(Model.Difficulty difficulty, string playerName)
        {
            CheckPlayerName(playerName);

            Data.Game game = InvokeCreateGame(difficulty);
            Data.Player player = InvokeAddPlayer(game.Key, true, playerName, Data.Powers.None);
            return game.ToPlayerGameResponse(player, true, true);
        }

        public Model.Game LoadGame(string gameKey)
        {
            return InvokeLoadGame(gameKey).ToGameResponse();
        }

        public Model.Game ResetGame(string gameKey)
        {
            return InvokeResetGame(gameKey).ToGameResponse();
        }

        public Model.Player AddPlayer(string gameKey, string playerName)
        {
            CheckPlayerName(playerName);

            return InvokeAddPlayer(gameKey, false, playerName, Data.Powers.None).ToPlayerResponse(true, true);
        }

        public Model.Player MovePlayer(string gameKey, string playerKey, Model.Direction direction)
        {
            return InvokeMovePlayer(gameKey, playerKey, direction).ToPlayerResponse(true, true);
        }
    }
}