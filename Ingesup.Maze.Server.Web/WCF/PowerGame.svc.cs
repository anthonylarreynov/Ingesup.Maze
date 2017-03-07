using Ingesup.Maze.Server.Web.Assemblers;
using Ingesup.Maze.Server.Web.WCF.Contract;
using Data = Ingesup.Maze.Server.Core.Entities;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.WCF
{
    public class PowerGame : BaseGame, IPowerGame
    {
        protected override string ServiceName
        {
            get { return "PowerGame.svc"; }
        }

        public Model.UrlPlayerGame CreateGame(Model.Difficulty difficulty, string playerName, Model.Power power)
        {
            CheckPlayerName(playerName);
            CheckSinglePower(power);

            Data.Game game = InvokeCreateGame(difficulty);
            Data.Player player = InvokeAddPlayer(game.Key, true, playerName, power.ToPowers());
            return game.ToUrlPlayerGameResponse(player, true, true);
        }

        public Model.UrlGame LoadGame(string gameKey)
        {
            return InvokeLoadGame(gameKey).ToUrlGameResponse();
        }

        public Model.UrlGame ResetGame(string gameKey)
        {
            return InvokeResetGame(gameKey).ToUrlGameResponse();
        }

        public Model.Player AddPlayer(string gameKey, string playerName, Model.Power power)
        {
            CheckPlayerName(playerName);
            CheckSinglePower(power);

            return InvokeAddPlayer(gameKey, false, playerName, power.ToPowers()).ToPlayerResponse(true, true);
        }

        public Model.Player MovePlayer(string gameKey, string playerKey, Model.Direction direction)
        {
            return InvokeMovePlayer(gameKey, playerKey, direction).ToPlayerResponse(true, true);
        }

        public Model.Player AddClone(string gameKey, string playerKey)
        {
            return InvokeAddClone(gameKey, playerKey).ToPlayerResponse(true, true);
        }

        public void RemoveClone(string gameKey, string playerKey)
        {
            InvokeRemoveClone(gameKey, playerKey);
        }
    }
}