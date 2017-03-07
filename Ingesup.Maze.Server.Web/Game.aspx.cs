using System;
using Data = Ingesup.Maze.Server.Core.Entities;

namespace Ingesup.Maze.Server.Web
{
    public partial class Game : System.Web.UI.Page
    {
        private Data.Game _currentGame = null;

        public string GameKey
        {
            get { return Request["key"]; }
        }

        public string PlayerKey
        {
            get { return Request["player"]; }
        }

        public Data.Game CurrentGame
        {
            get
            {
                if (_currentGame == null && !string.IsNullOrEmpty(GameKey))
                {
                    _currentGame = GameSuperviser.Instance.LoadGame(GameKey);
                }

                return _currentGame;
            }
        }

        public int? MovePlayerMinInterval
        {
            get
            {
                if (CurrentGame == null)
                {
                    return null;
                }

                return CurrentGame.MovePlayerMinInterval;
            }
        }

        public int? MazeWidth
        {
            get
            {
                if (CurrentGame == null)
                {
                    return null;
                }

                return CurrentGame.Maze.Width;
            }
        }

        public int? MazeHeight
        {
            get
            {
                if (CurrentGame == null)
                {
                    return null;
                }

                return CurrentGame.Maze.Height;
            }
        }

        public bool IsLastGame
        {
            get
            {
                return string.Equals(GameKey, Configuration.Instance.LastGameKey, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Title = string.Format("Ingesup - Maze - Game {0}", CurrentGame.Key);
                PnlGame.CssClass = IsLastGame ? "black-white" : "normal";
            }
        }
    }
}