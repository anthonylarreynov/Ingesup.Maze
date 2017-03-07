using Ingesup.Maze.Core;
using Ingesup.Maze.Server.Core.Exceptions;
using Ingesup.Maze.Server.Core.Extensions;
using System;
using System.IO;
using System.Runtime.Serialization;
using Entity = Ingesup.Maze.Core.Entity;

namespace Ingesup.Maze.Server.Core.Entities
{
    [DataContract]
    public class Player : Ingesup.Maze.Core.Entity.Player
    {
        public Player()
            : base()
        {
            SyncObj = new object();
        }

        [IgnoreDataMember]
        private object SyncObj { get; set; }

        [IgnoreDataMember]
        public Game Game { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool IsCreator { get; set; }

        [DataMember]
        public bool IsClone { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public Powers Powers { get; set; }

        [DataMember]
        public Location CurrentLocation { get; set; }

        [DataMember]
        public LocationList PreviousLocations { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public int NbMove { get; set; }

        [DataMember]
        public DateTime? FinishDate { get; set; }

        [IgnoreDataMember]
        public bool HasFinished
        {
            get { return this.IsEnabled && !this.IsClone && this.FinishDate.HasValue; }
        }

        [IgnoreDataMember]
        public TimeSpan FinishTime
        {
            get { return HasFinished ? this.FinishDate.Value.Subtract(this.StartDate) : TimeSpan.MaxValue; }
        }

        [IgnoreDataMember]
        public bool IsGameWinner
        {
            get { return (this.Game != null) && (this == this.Game.GetWinner()); }
        }

        [IgnoreDataMember]
        public bool HasPower
        {
            get { return this.Powers != Powers.None; }
        }

        [IgnoreDataMember]
        public bool HasAllPowers
        {
            get { return this.Powers == Powers.All; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Player p = obj as Player;
            if ((object)p == null)
            {
                return false;
            }

            return string.Equals(Key, p.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(Player p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return string.Equals(Key, p.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static bool operator ==(Player a, Player b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return string.Equals(a.Key, b.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(Player a, Player b)
        {
            return !(a == b);
        }

        public CellList GetVisibleCells()
        {
            CellList cells = new CellList();
            cells.AddRange(this.Game.Maze.GetVisibleCells(CurrentLocation, Powers));
            return cells;
        }

        public CellList GetVisitedCells(DateTime minDate)
        {
            CellList cells = new CellList();
            for (int i = PreviousLocations.Count - 1; i >= 0; i--)
            {
                Location location = PreviousLocations[i];
                if (location.CreateDate < minDate)
                {
                    return cells;
                }

                CellList visibleCells = this.Game.Maze.GetVisibleCells(location, this.Powers);
                foreach (Cell visibleCell in visibleCells)
                {
                    if (cells.ContainsCell(visibleCell.Position))
                    {
                        continue;
                    }

                    cells.Add(visibleCell);
                }
            }

            return cells;
        }

        internal void Move(Direction direction)
        {
            this.CheckBeforeMoving(direction);

            lock (SyncObj)
            {
                Entity.Position destinationPosition = this.CurrentLocation.Move(direction);
                Cell destinationCell = this.Game.Maze.FindCell(destinationPosition);
                if (destinationCell == null || destinationCell.CellType != Entity.CellType.Empty)
                {
                    throw new MovePlayerGameException(this.Game, this, direction, "The destination cell is not authorized.");
                }

                this.PreviousLocations.Add(this.CurrentLocation);
                this.CurrentLocation = Location.Create(destinationPosition);

                if (!this.HasFinished)
                {
                    this.NbMove++;
                }

                if (!this.FinishDate.HasValue && this.Game.Maze.EndCell != null && destinationPosition == this.Game.Maze.EndCell.Position)
                {
                    this.FinishDate = DateTime.Now;
                }
            }
        }

        internal void SaveTo(string path)
        {
            lock (SyncObj)
            {
                Serializer.ToFile<Player>(path, this);
            }
        }

        private void CheckBeforeMoving(Direction direction)
        {
            if (!this.IsEnabled)
            {
                throw new MovePlayerGameException(this.Game, this, direction, "Cannot move a disabled player.");
            }

            if (this.Game.MovePlayerMinInterval <= 0)
            {
                return;
            }

            double milliseconds = DateTime.Now.Subtract(this.CurrentLocation.CreateDate).TotalMilliseconds;
            if (this.Powers.HasFlag(Powers.SpeedUp))
            {
                milliseconds *= 2;
            }

            if (milliseconds < this.Game.MovePlayerMinInterval)
            {
                throw new MovePlayerGameException(this.Game, this, direction, string.Format("You must wait {0} milliseconds between each player move.", this.Game.MovePlayerMinInterval));
            }
        }

        internal static Player LoadFrom(string path)
        {
            Player player = Serializer.FromFile<Player>(path);
            player.SyncObj = new object();
            return player;
        }

        internal static Player Create(Game game, int id, bool isCreator, bool isClone, string key, string name, Powers powers, Entity.Position currentPosition)
        {
            return new Player
            {
                Game = game,
                Id = id,
                IsCreator = isCreator,
                IsClone = isClone,
                IsEnabled = true,
                Key = key,
                CreateDate = DateTime.Now,
                Name = name,
                Powers = powers,
                CurrentLocation = Location.Create(currentPosition),
                PreviousLocations = new LocationList(),
                StartDate = DateTime.Now,
                NbMove = 0,
                FinishDate = null
            };
        }
    }
}