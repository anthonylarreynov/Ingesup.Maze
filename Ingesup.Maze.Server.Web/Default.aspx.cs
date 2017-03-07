using Ingesup.Maze.Server.Core.Comparers;
using Ingesup.Maze.Server.Web.Helper;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Data = Ingesup.Maze.Server.Core.Entities;

namespace Ingesup.Maze.Server.Web
{
    public partial class Default : System.Web.UI.Page
    {
        public bool ShowHyperLink
        {
            get { return Request["link"] == "1"; }
        }

        public bool ShowLastGame
        {
            get { return Request["last"] == "1"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Data.Game> games = GameSuperviser.Instance.LoadAllGames();
                games.Sort(new CreateDateGameComparer());
                RptGames.DataSource = Filter(games);
                RptGames.DataBind();
            }
        }

        protected void RptGames_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            Data.Game game = (Data.Game)e.Item.DataItem;
            Data.Player creator = game.GetCreator();

            Literal litCreateDate = (Literal)e.Item.FindControl("LitCreateDate");
            litCreateDate.Text = DateTimeHelper.ToString(game.CreateDate);

            Literal litKey = (Literal)e.Item.FindControl("LitKey");
            litKey.Visible = !this.ShowHyperLink;
            if (litKey.Visible)
            {
                litKey.Text = game.Key;
            }

            HyperLink hplKey = (HyperLink)e.Item.FindControl("HplKey");
            hplKey.Visible = !litKey.Visible;
            if (hplKey.Visible)
            {
                hplKey.NavigateUrl = string.Concat("~/Game.aspx?key=", game.Key);
                hplKey.Text = game.Key;
            }

            Literal litDifficulty = (Literal)e.Item.FindControl("LitDifficulty");
            litDifficulty.Text = Convert.ToString(game.Difficulty);

            Literal litCreator = (Literal)e.Item.FindControl("LitCreator");
            litCreator.Text = (creator != null) ? creator.Name : "Foo";

            Literal litPlayersCount = (Literal)e.Item.FindControl("LitPlayersCount");
            litPlayersCount.Text = Convert.ToString(game.CountPlayers());
        }

        private List<Data.Game> Filter(List<Data.Game> games)
        {
            if (this.ShowLastGame)
            {
                return games;
            }

            List<Data.Game> filterGames = new List<Data.Game>();
            foreach (Data.Game game in games)
            {
                if (string.Equals(game.Key, Configuration.Instance.LastGameKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                filterGames.Add(game);
            }

            return filterGames;
        }
    }
}