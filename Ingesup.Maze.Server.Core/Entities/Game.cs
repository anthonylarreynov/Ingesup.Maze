using Ingesup.Maze.Core;
using Ingesup.Maze.Server.Core.Exceptions;
using System;
using System.IO;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public class Game
    {
        private const string XmlGameFile = "Game.xml";
        private const string XmlMazeFile = "Maze.xml";
        private const string TextMazeFile = "Maze.txt";
        private const string XmlPlayerFile = "Player.{0}.xml";

        public Game()
        {
            SyncObj = new object();
        }

        [IgnoreDataMember]
        private object SyncObj { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public Difficulty Difficulty { get; set; }

        [IgnoreDataMember]
        public int MaxPlayersCount { get; set; }

        [IgnoreDataMember]
        public int MovePlayerMinInterval { get; set; }

        [IgnoreDataMember]
        public Maze Maze { get; set; }

        [IgnoreDataMember]
        public PlayerList Players { get; set; }

        internal void Reset(string path)
        {
            Entity.Position startCell = this.Maze.StartCell.Position;
            lock (SyncObj)
            {
                for (int i = this.Players.Count - 1; i >= 0; i--)
                {
                    Player player = this.Players[i];
                    if (!player.IsEnabled || player.IsClone)
                    {
                        string filePath = Path.Combine(path, string.Format(XmlPlayerFile, player.Key));
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        this.Players.RemoveAt(i);

                        continue;
                    }

                    player.CurrentLocation = Location.Create(startCell);
                    player.StartDate = DateTime.Now;
                    player.FinishDate = null;
                    player.PreviousLocations.Clear();
                }
            }
        }

        internal Player AddPlayer(string key, bool isCreator, string name, Powers powers)
        {
            Player player = null;
            lock (SyncObj)
            {
                if (this.CountPlayers() >= MaxPlayersCount)
                {
                    throw new GameException(this, string.Format("{0} players maximum are authorized to play on the same maze.", MaxPlayersCount));
                }

                int id = this.CountPlayers() + 1;
                Entity.Position startCell = this.Maze.StartCell.Position;
                player = Player.Create(this, id, isCreator, false, key, name, powers, startCell);
                this.Players.Add(player);
            }

            return player;
        }

        internal Player AddClone(string playerKey, string cloneKey)
        {
            Player player = FindPlayer(playerKey);
            if (!player.Powers.HasFlag(Powers.Duplicate))
            {
                throw new InvalidOperationException(string.Format("The player '{0}' does not have the Duplicate power.", playerKey));
            }

            Player clone = null;
            lock (SyncObj)
            {
                if (FindClone(player.Id) != null)
                {
                    throw new InvalidOperationException(string.Format("The player '{0}' has already a clone.", playerKey));
                }

                clone = Player.Create(this, player.Id, false, true, cloneKey, string.Concat(player.Name, " (C)"), Powers.None, player.CurrentLocation);
                this.Players.Add(clone);
            }

            return clone;
        }

        internal Player RemoveClone(string playerKey)
        {
            Player player = FindPlayer(playerKey);
            if (!player.Powers.HasFlag(Powers.Duplicate))
            {
                throw new InvalidOperationException(string.Format("The player '{0}' does not have the Duplicate power.", playerKey));
            }

            Player clone = null;
            lock (SyncObj)
            {
                clone = FindClone(player.Id);
                if (clone != null)
                {
                    clone.IsEnabled = false;
                }
            }

            return clone;
        }

        public int CountPlayers()
        {
            int count = 0;
            foreach (Player player in this.Players)
            {
                if (!player.IsEnabled || player.IsClone)
                {
                    continue;
                }

                count++;
            }

            return count;
        }

        public Player FindPlayer(string key)
        {
            foreach (Player player in this.Players)
            {
                if (player.IsEnabled && string.Equals(player.Key, key, StringComparison.InvariantCultureIgnoreCase))
                {
                    return player;
                }
            }

            return null;
        }

        public Player FindClone(int id)
        {
            foreach (Player player in this.Players)
            {
                if (player.IsEnabled && player.IsClone && player.Id == id)
                {
                    return player;
                }
            }

            return null;
        }

        public bool ContainsPlayer(string key)
        {
            return FindPlayer(key) != null;
        }

        public Player GetCreator()
        {
            foreach (Player player in this.Players)
            {
                if (player.IsCreator)
                {
                    return player;
                }
            }

            return null;
        }

        public Player GetWinner()
        {
            Player winner = null;
            foreach (Player player in this.Players)
            {
                if (!player.HasFinished || player.IsClone || !player.IsEnabled)
                {
                    continue;
                }

                if (winner != null && winner.FinishTime < player.FinishTime)
                {
                    continue;
                }

                winner = player;
            }

            return winner;
        }

        public bool ContainsWinner()
        {
            foreach (Player player in this.Players)
            {
                if (player.HasFinished)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsPowerPlayer()
        {
            foreach (Player player in this.Players)
            {
                if (player.IsEnabled && !player.IsClone && player.Powers != Powers.None)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsPowerPlayer(Powers powers)
        {
            foreach (Player player in this.Players)
            {
                if (player.IsEnabled && !player.IsClone && player.Powers.HasFlag(powers))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsAllPowerPlayers()
        {
            foreach (Powers power in Enum.GetValues(typeof(Powers)))
            {
                if (power == Powers.None || power == Powers.All)
                {
                    continue;
                }

                if (!ContainsPowerPlayer(power))
                {
                    return false;
                }
            }

            return true;
        }

        internal void SaveTo(string path)
        {
            lock (SyncObj)
            {
                Serializer.ToFile<Game>(Path.Combine(path, XmlGameFile), this);
            }

            this.Maze.SaveTo(Path.Combine(path, XmlMazeFile));
            foreach (Player player in this.Players)
            {
                SavePlayerTo(path, player);
            }

            this.Maze.PaintTo(Path.Combine(path, TextMazeFile));
        }

        internal void SavePlayerTo(string path, Player player)
        {
            player.SaveTo(Path.Combine(path, string.Format(XmlPlayerFile, player.Key)));
        }

        internal static Game LoadFrom(string path, int maxPlayersCount, int movePlayerMinInterval)
        {
            Game game = Serializer.FromFile<Game>(Path.Combine(path, XmlGameFile));
            game.SyncObj = new object();
            game.MaxPlayersCount = maxPlayersCount;
            game.MovePlayerMinInterval = movePlayerMinInterval;

            game.Maze = Maze.LoadFrom(Path.Combine(path, XmlMazeFile));
            game.Maze.Game = game;

            game.Players = new PlayerList();
            foreach (string file in Directory.GetFiles(path, XmlPlayerFile.Replace("{0}", "*"), SearchOption.TopDirectoryOnly))
            {
                Player player = Player.LoadFrom(file);
                player.Game = game;
                game.Players.Add(player);
            }

            return game;
        }

        internal static Game Create(string key, Difficulty difficulty, int maxPlayersCount, int movePlayerMinInterval, int width, int height)
        {
            Game game = new Game { Key = key, CreateDate = DateTime.Now, Difficulty = difficulty, MaxPlayersCount = maxPlayersCount, MovePlayerMinInterval = movePlayerMinInterval, Players = new PlayerList() };
            game.Maze = Maze.Generate(game, width, height);
            return game;
        }
    }
}