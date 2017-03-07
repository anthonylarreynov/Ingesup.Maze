using Ingesup.Maze.Server.Core.Extensions;
using Ingesup.Maze.Server.Web.Helper;
using System;
using System.Collections.Generic;
using Data = Ingesup.Maze.Server.Core.Entities;
using Entity = Ingesup.Maze.Core.Entity;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.Assemblers
{
    public static class GameAssembler
    {
        public static Data.Difficulty ToDifficulty(this Model.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Model.Difficulty.Easy: return Data.Difficulty.Easy;
                case Model.Difficulty.Medium: return Data.Difficulty.Medium;
                case Model.Difficulty.Hard: return Data.Difficulty.Hard;
                case Model.Difficulty.Extreme: return Data.Difficulty.Extreme;
                default: throw new ArgumentOutOfRangeException("difficulty", string.Format("The value '{0}' is not recognized as a correct difficulty.", difficulty));
            }
        }

        public static Model.Difficulty ToDifficulty(this Data.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Data.Difficulty.Easy: return Model.Difficulty.Easy;
                case Data.Difficulty.Medium: return Model.Difficulty.Medium;
                case Data.Difficulty.Hard: return Model.Difficulty.Hard;
                case Data.Difficulty.Extreme: return Model.Difficulty.Extreme;
                default: throw new ArgumentOutOfRangeException("difficulty", string.Format("The value '{0}' is not recognized as a correct difficulty.", difficulty));
            }
        }

        public static Data.Powers ToPowers(this Model.Power power)
        {
            switch (power)
            {
                case Model.Power.SpeedUp: return Data.Powers.SpeedUp;
                case Model.Power.EagleVision: return Data.Powers.EagleVision;
                case Model.Power.WallVision: return Data.Powers.WallVision;
                case Model.Power.Duplicate: return Data.Powers.Duplicate;
                default: throw new ArgumentOutOfRangeException("power", string.Format("The value '{0}' is not recognized as a correct power.", power));
            }
        }

        public static Data.Direction ToDirection(this Model.Direction direction)
        {
            switch (direction)
            {
                case Model.Direction.Up: return Data.Direction.Up;
                case Model.Direction.Right: return Data.Direction.Right;
                case Model.Direction.Down: return Data.Direction.Down;
                case Model.Direction.Left: return Data.Direction.Left;
                default: throw new ArgumentOutOfRangeException("direction", string.Format("The value '{0}' is not recognized as a correct direction.", direction));
            }
        }

        public static Model.Position ToPositionResponse(this Entity.Position position)
        {
            if (position == null)
            {
                return null;
            }

            Model.Position item = new Model.Position();
            item.X = position.X;
            item.Y = position.Y;
            return item;
        }

        public static Model.Cell ToCellResponse(this Data.Cell cell)
        {
            if (cell == null)
            {
                return null;
            }

            Model.Cell item = new Model.Cell();
            item.CellType = FindCellType(cell);
            item.Position = ToPositionResponse(cell.Position);
            return item;
        }

        public static List<Model.Cell> ToCellResponse(this Data.CellList cells)
        {
            if (cells == null)
            {
                return null;
            }

            List<Model.Cell> items = new List<Model.Cell>();
            foreach (Data.Cell cell in cells)
            {
                items.Add(ToCellResponse(cell));
            }

            return items;
        }

        public static Model.Maze ToMazeResponse(this Entity.Maze maze)
        {
            if (maze == null)
            {
                return null;
            }

            Models.Maze item = new Models.Maze();
            item.Width = maze.Width;
            item.Height = maze.Height;
            return item;
        }

        public static Model.Player ToPlayerResponse(this Data.Player player, bool includeKey, bool includeSecretMessage)
        {
            if (player == null || !player.IsEnabled)
            {
                return null;
            }

            Model.Player item = new Model.Player();
            item.Id = string.Concat(player.Id, player.IsClone ? "c" : "");
            if (includeKey)
            {
                item.Key = player.Key;
            }

            item.CreateDate = DateTimeHelper.ToString(player.CreateDate);
            item.Name = player.Name;
            item.CurrentPosition = ToPositionResponse(player.CurrentLocation);
            item.VisibleCells = ToCellResponse(player.GetVisibleCells());
            item.StartDate = DateTimeHelper.ToString(player.StartDate);
            item.FinishDate = player.FinishDate.HasValue ? DateTimeHelper.ToString(player.FinishDate.Value) : null;
            item.FinishTime = player.FinishDate.HasValue ? DateTimeHelper.ToString(player.FinishTime) : null;
            item.NbMove = player.NbMove;

            if (includeSecretMessage)
            {
                item.SecretMessage = SecretMessageHelper.ForWcfResponse(player);
            }

            return item;
        }

        public static Model.Game ToGameResponse(this Data.Game game)
        {
            if (game == null)
            {
                return null;
            }

            Model.Game item = new Model.Game();
            item.Key = game.Key;
            item.CreateDate = DateTimeHelper.ToString(game.CreateDate);
            item.Difficulty = ToDifficulty(game.Difficulty);
            item.MovePlayerMinInterval = game.MovePlayerMinInterval;
            item.Maze = ToMazeResponse(game.Maze);

            return item;
        }

        public static Model.PlayerGame ToPlayerGameResponse(this Data.Game game, Data.Player player, bool includeKey, bool includeSecretMessage)
        {
            if (game == null)
            {
                return null;
            }

            Model.PlayerGame item = new Model.PlayerGame();
            item.Key = game.Key;
            item.CreateDate = DateTimeHelper.ToString(game.CreateDate);
            item.Difficulty = ToDifficulty(game.Difficulty);
            item.MovePlayerMinInterval = game.MovePlayerMinInterval;
            item.Maze = ToMazeResponse(game.Maze);

            item.Player = ToPlayerResponse(player, includeKey, includeSecretMessage);

            return item;
        }

        public static Model.UrlGame ToUrlGameResponse(this Data.Game game)
        {
            if (game == null)
            {
                return null;
            }

            Model.UrlGame item = new Model.UrlGame();
            item.Key = game.Key;
            item.CreateDate = DateTimeHelper.ToString(game.CreateDate);
            item.Difficulty = ToDifficulty(game.Difficulty);
            item.MovePlayerMinInterval = game.MovePlayerMinInterval;
            item.Maze = ToMazeResponse(game.Maze);

            item.Url = UrlHelper.ResolveGameUrl(game.Key);

            return item;
        }

        public static Model.UrlPlayerGame ToUrlPlayerGameResponse(this Data.Game game, Data.Player player, bool includeKey, bool includeSecretMessage)
        {
            if (game == null)
            {
                return null;
            }

            Model.UrlPlayerGame item = new Model.UrlPlayerGame();
            item.Key = game.Key;
            item.CreateDate = DateTimeHelper.ToString(game.CreateDate);
            item.Difficulty = ToDifficulty(game.Difficulty);
            item.MovePlayerMinInterval = game.MovePlayerMinInterval;
            item.Maze = ToMazeResponse(game.Maze);

            item.Player = ToPlayerResponse(player, includeKey, includeSecretMessage);

            item.Url = UrlHelper.ResolveGameUrl(game.Key);

            return item;
        }

        public static Model.PlayersGame ToPlayersGameResponse(this Data.Game game, DateTime lastUpdateDate, bool includeKey, bool includeSecretMessage)
        {
            if (game == null)
            {
                return null;
            }

            Model.PlayersGame item = new Model.PlayersGame();
            item.ServerDate = DateTimeHelper.ToUniversalDateTime(DateTime.Now);
            item.Key = game.Key;
            item.CreateDate = DateTimeHelper.ToString(game.CreateDate);
            item.Difficulty = ToDifficulty(game.Difficulty);
            item.MovePlayerMinInterval = game.MovePlayerMinInterval;
            item.Maze = ToMazeResponse(game.Maze);

            item.Players = new List<Model.Player>();
            Data.CellList visitedCells = new Data.CellList();
            foreach (Data.Player player in game.Players)
            {
                visitedCells.AddMissingRange(player.GetVisitedCells(lastUpdateDate));

                Model.Player playerResponse = ToPlayerResponse(player, includeKey, includeSecretMessage);
                if (playerResponse == null)
                {
                    continue;
                }

                item.Players.Add(playerResponse);
            }

            item.VisitedCells = visitedCells.ToCellResponse();

            return item;
        }

        private static Model.CellType FindCellType(Data.Cell cell)
        {
            if (cell.Maze != null && cell.Maze.StartCell != null && cell.Maze.StartCell.Position == cell.Position)
            {
                return Model.CellType.Start;
            }

            if (cell.Maze != null && cell.Maze.EndCell != null && cell.Maze.EndCell.Position == cell.Position)
            {
                return Model.CellType.End;
            }

            switch (cell.CellType)
            {
                case Entity.CellType.Empty: return Model.CellType.Empty;
                case Entity.CellType.Wall: return Model.CellType.Wall;
                default: throw new NotImplementedException(string.Format("The cell type {0} is not recognized as a correct type.", cell.CellType));
            }
        }
    }
}