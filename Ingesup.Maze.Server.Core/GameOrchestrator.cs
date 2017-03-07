using Ingesup.Maze.Server.Core.Entities;

namespace Ingesup.Maze.Server.Core
{
    public static class GameOrchestrator
    {
        public static Game CreateGame(string key, Difficulty difficulty, int maxPlayersCount, int movePlayerMinInterval, int width, int height)
        {
            return Game.Create(key, difficulty, maxPlayersCount, movePlayerMinInterval, width, height);
        }

        public static void ResetGame(Game game, string path)
        {
            game.Reset(path);
        }

        public static Player AddPlayer(Game game, string key, bool isCreator, string name, Powers powers)
        {
            return game.AddPlayer(key, isCreator, name, powers);
        }

        public static Player AddClone(Player player, string cloneKey)
        {
            return player.Game.AddClone(player.Key, cloneKey);
        }

        public static Player RemoveClone(Player player)
        {
            return player.Game.RemoveClone(player.Key);
        }

        public static void MovePlayer(Player player, Direction direction)
        {
            player.Move(direction);
        }

        public static void SaveGame(Game game, string path)
        {
            game.SaveTo(path);
        }

        public static void SavePlayer(Player player, string path)
        {
            player.Game.SavePlayerTo(path, player);
        }

        public static Game LoadGame(string path, int maxPlayersCount, int movePlayerMinInterval)
        {
            return Game.LoadFrom(path, maxPlayersCount, movePlayerMinInterval);
        }
    }
}