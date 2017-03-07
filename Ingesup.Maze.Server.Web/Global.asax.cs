using System;
using System.Web.Http;

namespace Ingesup.Maze.Server.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Logger.Info(Logger.Messages.ApplicationStart);

            GlobalConfiguration.Configure(config => { WebApiConfig.Register(config); });
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Logger.Error(ex, Logger.Messages.ApplicationError);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logger.Info(Logger.Messages.ApplicationEnd);
        }
    }
}