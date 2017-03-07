using System.Web;

namespace Ingesup.Maze.Server.Web.Helper
{
    public class UrlHelper
    {
        public static string Resolve(string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
        }

        public static string ResolveGameUrl(string key)
        {
            return Resolve(string.Concat("~/Game.aspx?key=", key));
        }

        public static string AllGamesUrl
        {
            get { return Resolve("~/Default.aspx"); }
        }

        public static string PowerServiceUrl
        {
            get { return Resolve("~/WCF/PowerGame.svc"); }
        }
    }
}