using Ingesup.Maze.Server.Core.Exceptions;
using Ingesup.Maze.Server.Web.Assemblers;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using Data = Ingesup.Maze.Server.Core.Entities;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.WCF
{
    public abstract class BaseGame
    {
        private const string PlayerNamePattern = @"^(?=[A-Za-z0-9])[A-Za-z0-9]{3,15}$";

        protected abstract string ServiceName { get; }

        protected Data.Game InvokeCreateGame(Model.Difficulty difficulty)
        {
            try
            {
                Logger.Debug(Logger.Messages.ServiceCreatingGame, ServiceName, difficulty);

                Data.Game game = GameSuperviser.Instance.CreateGame(difficulty);

                Logger.Info(Logger.Messages.ServiceCreatedGame, ServiceName, difficulty, game.Key);

                return game;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceCreateGameFailed, ServiceName, difficulty);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceCreateGameFailed, ServiceName, difficulty);
                throw new FaultException("An error occurred while creating a game. Please contact administrator if you need more information.");
            }
        }

        protected Data.Game InvokeLoadGame(string gameKey)
        {
            CheckGame(gameKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceLoadingGame, ServiceName, gameKey);

                Data.Game game = GameSuperviser.Instance.LoadGame(gameKey);

                Logger.Info(Logger.Messages.ServiceLoadedGame, ServiceName, game.Key);

                return game;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceLoadGameFailed, ServiceName, gameKey);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceLoadGameFailed, ServiceName, gameKey);
                throw new FaultException("An error occurred while loading a game. Please contact administrator if you need more information.");
            }
        }

        protected Data.Game InvokeResetGame(string gameKey)
        {
            CheckGame(gameKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceResetingGame, ServiceName, gameKey);

                Data.Game game = GameSuperviser.Instance.ResetGame(gameKey);

                Logger.Info(Logger.Messages.ServiceGameReset, ServiceName, game.Key);

                return game;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceResetGameFailed, ServiceName, gameKey);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceResetGameFailed, ServiceName, gameKey);
                throw new FaultException("An error occurred while reseting a game. Please contact administrator if you need more information.");
            }
        }

        protected Data.Player InvokeAddPlayer(string gameKey, bool isCreator, string playerName, Data.Powers powers)
        {
            CheckGame(gameKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceAddingPlayer, ServiceName, gameKey, isCreator, playerName, powers);

                Data.Player player = GameSuperviser.Instance.AddPlayer(gameKey, isCreator, playerName, powers);

                Logger.Info(Logger.Messages.ServicePlayerAdded, ServiceName, gameKey, isCreator, playerName, powers, player.Key);

                return player;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceAddPlayerFailed, ServiceName, gameKey, isCreator, playerName, powers);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceAddPlayerFailed, ServiceName, gameKey, isCreator, playerName, powers);
                throw new FaultException("An error occurred while adding player. Please contact administrator if you need more information.");
            }
        }

        protected Data.Player InvokeMovePlayer(string gameKey, string playerKey, Model.Direction direction)
        {
            CheckGameAndPlayer(gameKey, playerKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceMovingPlayer, ServiceName, gameKey, playerKey, direction);

                Data.Player player = GameSuperviser.Instance.MovePlayer(gameKey, playerKey, direction.ToDirection());

                Logger.Info(Logger.Messages.ServicePlayerMoved, ServiceName, gameKey, playerKey, direction, player.CurrentLocation.X, player.CurrentLocation.Y);

                return player;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceMovePlayerFailed, ServiceName, gameKey, playerKey, direction);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceMovePlayerFailed, ServiceName, gameKey, playerKey, direction);
                throw new FaultException("An error occurred while moving player. Please contact administrator if you need more information.");
            }
        }

        protected Data.Player InvokeAddClone(string gameKey, string playerKey)
        {
            CheckGameAndPlayer(gameKey, playerKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceAddingClone, ServiceName, gameKey, playerKey);

                Data.Player clone = GameSuperviser.Instance.AddClone(gameKey, playerKey);

                Logger.Info(Logger.Messages.ServiceCloneAdded, ServiceName, gameKey, playerKey, clone.Key);

                return clone;
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceAddCloneFailed, ServiceName, gameKey, playerKey);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceAddCloneFailed, ServiceName, gameKey, playerKey);
                throw new FaultException("An error occurred while adding clone. Please contact administrator if you need more information.");
            }
        }

        protected void InvokeRemoveClone(string gameKey, string playerKey)
        {
            CheckGameAndPlayer(gameKey, playerKey);

            try
            {
                Logger.Debug(Logger.Messages.ServiceRemovingClone, ServiceName, gameKey, playerKey);

                GameSuperviser.Instance.RemoveClone(gameKey, playerKey);

                Logger.Info(Logger.Messages.ServiceCloneRemoved, ServiceName, gameKey, playerKey);
            }
            catch (GameException gex)
            {
                Logger.Warn(gex, Logger.Messages.ServiceRemoveCloneFailed, ServiceName, gameKey, playerKey);
                throw new FaultException(gex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Logger.Messages.ServiceRemoveCloneFailed, ServiceName, gameKey, playerKey);
                throw new FaultException("An error occurred while removing clone. Please contact administrator if you need more information.");
            }
        }

        protected void CheckGame(string gameKey)
        {
            if (string.IsNullOrEmpty(gameKey))
            {
                throw new FaultException("The Game Key must be specified.");
            }
        }

        protected void CheckPlayerName(string value)
        {
            if (!Regex.IsMatch(value, PlayerNamePattern, RegexOptions.Singleline))
            {
                throw new FaultException("The player name is not authorized.");
            }
        }

        protected void CheckPlayer(string playerKey)
        {
            if (string.IsNullOrEmpty(playerKey))
            {
                throw new FaultException("The Player Key must be specified.");
            }
        }

        protected void CheckGameAndPlayer(string gameKey, string playerKey)
        {
            CheckGame(gameKey);
            CheckPlayer(playerKey);
        }

        protected void CheckSinglePower(Model.Power value)
        {
            if (CountPower(value) > 1)
            {
                throw new FaultException("Maybe one day you will be strong enought to get more than 1 power...");
            }
        }

        private int CountPower(Model.Power value)
        {
            int count = 0;
            foreach (Model.Power power in Enum.GetValues(typeof(Model.Power)))
            {
                if (value.HasFlag(power))
                {
                    count++;
                }
            }

            return count;
        }
    }
}