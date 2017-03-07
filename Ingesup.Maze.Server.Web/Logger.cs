using System;

namespace Ingesup.Maze.Server.Web
{
    public static class Logger
    {
        private static NLog.Logger Current = NLog.LogManager.GetCurrentClassLogger();

        public static class Messages
        {
            public const string ApplicationStart = "Starting Application Server...";
            public const string ApplicationError = "An application error has occurred.";
            public const string ApplicationEnd = "Application Server Stopped.";

            public const string ControllerGetSecretMessageFailed = "An error occurred while getting secret message. [key={0}]";
            public const string ControllerGetGameFailed = "An error occurred while getting game. [key={0}, lastUpdateDate={1}]";
            public const string ControllerGetPlayerFailed = "An error occurred while getting player. [key={0}, player={1}, direction={2}]";

            public const string ServiceCreatingGame = "{0} - Creating game... [Difficulty={1}]";
            public const string ServiceCreatedGame = "{0} - Game Created. [Difficulty={1}, Game Key={2}]";
            public const string ServiceCreateGameFailed = "{0} - An error occurred while creating game. [difficulty={1}]";

            public const string ServiceLoadingGame = "{0} - Loading game... [Game Key={1}]";
            public const string ServiceLoadedGame = "{0} - Game loaded. [Game Key={1}]";
            public const string ServiceLoadGameFailed = "{0} - An error occurred while loading a game. [Game Key={1}]";

            public const string ServiceResetingGame = "{0} - Reseting game... [Game Key={1}]";
            public const string ServiceGameReset = "{0} - Game reset. [Game Key={1}]";
            public const string ServiceResetGameFailed = "{0} - An error occurred while reseting a game. [Game Key={1}]";

            public const string ServiceAddingPlayer = "{0} - Adding player... [Game Key={1}, IsCreator={2}, Player Name='{3}', Power={4}]";
            public const string ServicePlayerAdded = "{0} - Player added. [Game Key={1}, IsCreator={2}, Player Name='{3}', Power={4}, Player Key={5}]";
            public const string ServiceAddPlayerFailed = "{0} - An error occurred while adding player. [Game Key={1}, IsCreator={2}, Player Name='{3}', Power={4}]";

            public const string ServiceMovingPlayer = "{0} - Moving player... [Game Key={1}, Player Key={2}, Direction={3}]";
            public const string ServicePlayerMoved = "{0} - Player moved. [Game Key={1}, Player Key={2}, Direction={3}, X={4}, Y={5}]";
            public const string ServiceMovePlayerFailed = "{0} - An error occurred while moving player. [Game Key={1}, Player Key={2}, Direction={3}]";

            public const string ServiceAddingClone = "{0} - Adding clone... [Game Key={1}, Player Key={2}]";
            public const string ServiceCloneAdded = "{0} - Clone added. [Game Key={1}, Player Key={2}, Clone Key={3}]";
            public const string ServiceAddCloneFailed = "{0} - An error occurred while adding clone. [Game Key={1}, Player Key={2}]";

            public const string ServiceRemovingClone = "{0} - Removing clone... [Game Key={1}, Player Key={2}]";
            public const string ServiceCloneRemoved = "{0} - Clone removed. [Game Key={1}, Player Key={2}]";
            public const string ServiceRemoveCloneFailed = "{0} - An error occurred while removing clone. [Game Key={1}, Player Key={2}]";
        }

        public static void Debug(string message)
        {
            Current.Debug(message);
        }

        public static void Debug(string format, params object[] args)
        {
            Current.Debug(format, args);
        }

        public static void Info(string message)
        {
            Current.Info(message);
        }

        public static void Info(string format, params object[] args)
        {
            Current.Info(format, args);
        }

        public static void Warn(Exception ex, string message)
        {
            Current.Warn(ex, message);
        }

        public static void Warn(Exception ex, string format, params object[] args)
        {
            Current.Warn(ex, format, args);
        }

        public static void Error(Exception ex, string message)
        {
            Current.Error(ex, message);
        }

        public static void Error(Exception ex, string format, params object[] args)
        {
            Current.Error(ex, format, args);
        }
    }
}