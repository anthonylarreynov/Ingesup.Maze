using Ingesup.Maze.Server.Core;
using Ingesup.Maze.Server.Core.Exceptions;
using Ingesup.Maze.Server.Web.Assemblers;
using Ingesup.Maze.Server.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Data = Ingesup.Maze.Server.Core.Entities;

namespace Ingesup.Maze.Server.Web
{
    public class GameSuperviser
    {
        private static volatile GameSuperviser _instance;
        private static object _syncRoot = new Object();

        private GameSuperviser()
        {
            CurrentGames = new ConcurrentDictionary<string, Data.Game>();
        }

        private ConcurrentDictionary<string, Data.Game> CurrentGames { get; set; }

        public static GameSuperviser Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameSuperviser();
                        }
                    }
                }

                return _instance;
            }
        }

        public Data.Game CreateGame(Difficulty difficulty)
        {
            string key, path = string.Empty;
            do
            {
                key = Guid.NewGuid().ToString();
                path = Path.Combine(Configuration.Instance.AppDataDirectory, key);
            } while (Directory.Exists(path));

            Size size = Configuration.Instance.DifficultySizes[difficulty];
            Data.Game game = GameOrchestrator.CreateGame(key, difficulty.ToDifficulty(), Configuration.Instance.MaxPlayersCount, Configuration.Instance.MovePlayerMinInterval, size.Width, size.Height);

            Directory.CreateDirectory(path);
            GameOrchestrator.SaveGame(game, path);

            CurrentGames.TryAdd(key, game);

            return game;
        }

        public Data.Game LoadGame(string key)
        {
            Data.Game game = null;
            if (CurrentGames.TryGetValue(key, out game))
            {
                return game;
            }

            string path = Path.Combine(Configuration.Instance.AppDataDirectory, key);
            if (!Directory.Exists(path))
            {
                throw new GameException(null, string.Format("There is no game for key '{0}'.", key));
            }

            game = GameOrchestrator.LoadGame(path, Configuration.Instance.MaxPlayersCount, Configuration.Instance.MovePlayerMinInterval);
            CurrentGames.TryAdd(key, game);
            return game;
        }

        public Data.Game ResetGame(string key)
        {
            Data.Game game = LoadGame(key);

            GameOrchestrator.ResetGame(game, Configuration.Instance.AppDataDirectory);

            string path = Path.Combine(Configuration.Instance.AppDataDirectory, game.Key);
            GameOrchestrator.SaveGame(game, path);

            return game;
        }

        public List<Data.Game> LoadAllGames()
        {
            List<Data.Game> games = new List<Data.Game>();
            foreach (string directory in Directory.EnumerateDirectories(Configuration.Instance.AppDataDirectory))
            {
                games.Add(LoadGame(new DirectoryInfo(directory).Name));
            }

            return games;
        }

        public Data.Player AddPlayer(string key, bool isCreator, string playerName, Data.Powers playerPowers)
        {
            Data.Game game = LoadGame(key);
            Data.Player player = GameOrchestrator.AddPlayer(game, Guid.NewGuid().ToString(), isCreator, playerName, playerPowers);

            GameOrchestrator.SavePlayer(player, Path.Combine(Configuration.Instance.AppDataDirectory, key));

            return player;
        }

        public Data.Player LoadPlayer(string gameKey, string playerKey)
        {
            Data.Game game = LoadGame(gameKey);
            Data.Player player = game.FindPlayer(playerKey);
            if (player == null)
            {
                throw new PlayerGameException(game, null, string.Format("There is no player for key '{0}'.", playerKey));
            }

            return player;
        }

        public Data.Player MovePlayer(string gameKey, string playerKey, Data.Direction direction)
        {
            Data.Player player = LoadPlayer(gameKey, playerKey);

            GameOrchestrator.MovePlayer(player, direction);
            GameOrchestrator.SavePlayer(player, Path.Combine(Configuration.Instance.AppDataDirectory, gameKey));

            return player;
        }

        public Data.Player AddClone(string gameKey, string playerKey)
        {
            Data.Player player = LoadPlayer(gameKey, playerKey);

            Data.Player clone = GameOrchestrator.AddClone(player, Guid.NewGuid().ToString());
            GameOrchestrator.SavePlayer(clone, Path.Combine(Configuration.Instance.AppDataDirectory, gameKey));

            return clone;
        }

        public void RemoveClone(string gameKey, string playerKey)
        {
            Data.Player player = LoadPlayer(gameKey, playerKey);

            Data.Player clone = GameOrchestrator.RemoveClone(player);
            if (clone != null)
            {
                GameOrchestrator.SavePlayer(clone, Path.Combine(Configuration.Instance.AppDataDirectory, gameKey));
            }
        }
    }
}