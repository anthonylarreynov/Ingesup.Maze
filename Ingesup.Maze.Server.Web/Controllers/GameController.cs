using Ingesup.Maze.Server.Core.Exceptions;
using Ingesup.Maze.Server.Web.Assemblers;
using Ingesup.Maze.Server.Web.Helper;
using Ingesup.Maze.Server.Web.Models;
using System;
using System.Web.Http;

namespace Ingesup.Maze.Server.Web.Controllers
{
    public class GameController : ApiController
    {
        public string GetSecretMessage([FromUri] string key)
        {
            try
            {
                return SecretMessageHelper.ForWebApiResponse(GameSuperviser.Instance.LoadGame(key));
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ControllerGetSecretMessageFailed, key);
                throw new InvalidOperationException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ControllerGetSecretMessageFailed, key);
                throw new InvalidOperationException("An error occurred while getting secret message. Please contact administrator if you need more information.");
            }
        }

        public PlayersGame GetGame([FromUri] string key, [FromUri] string lastUpdateDate)
        {
            try
            {
                return GameSuperviser.Instance.LoadGame(key).ToPlayersGameResponse(DateTimeHelper.FromUniversalDateTime(lastUpdateDate), false, false);
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ControllerGetGameFailed, key, lastUpdateDate);
                throw new InvalidOperationException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ControllerGetGameFailed, key, lastUpdateDate);
                throw new InvalidOperationException("An error occurred while getting game. Please contact administrator if you need more information.");
            }
        }

        public Player GetPlayer([FromUri] string key, [FromUri] string player, [FromUri] Direction direction)
        {
            try
            {
                return GameSuperviser.Instance.MovePlayer(key, player, direction.ToDirection()).ToPlayerResponse(false, false);
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ControllerGetPlayerFailed, key, player, direction);
                throw new InvalidOperationException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ControllerGetPlayerFailed, key, player, direction);
                throw new InvalidOperationException("An error occurred while getting player. Please contact administrator if you need more information.");
            }
        }
    }
}