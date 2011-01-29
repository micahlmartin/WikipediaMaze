using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using WikipediaMaze.App;
using WikipediaMaze.Core;
using WikipediaMaze.Core.Properties;
using WikipediaMaze.Core.StructureMap;
using WikipediaMaze.Services;

namespace WikipediaMaze
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeRouting();

            InitializeModelBinders();

            InitializeStructureMap();

            InitializeControllerFactory();

            InitializeServices();

        }

        #region Initialization

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("*.xml");
            routes.IgnoreRoute("google3fc84e9417e29775.html");
            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute("flair",
                "players/{id}/flairimage.png",
                new { controller = "players", action = "flairimage" },
                new { id = @"\d+" }
                );
            routes.MapRoute(
                "wiki",
                "wiki/{topic}",
                new { controller = "game", action = "continue", topic = "" }
                );

            routes.MapRoute(
                "UserDisplay",
                "{controller}/{id}/{userName}",
                new { controller = "players", action = "display", userName = "" },
                new { id = @"\d+" }
                );

            routes.MapRoute(
                "Cache",
                "cache/{key}/{version}/{type}",
                new { controller = "cache", action = "cacheContent", key = "", version = "", type = "" }
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = "" } // Parameter defaults
                );
        }

        private static void InitializeModelBinders()
        {
            ModelBinders.Binders.DefaultBinder = new StructureMapSubControllerBinder();
        }

        private static void InitializeRouting()
        {
            RegisterRoutes(RouteTable.Routes);
            //MvcContrib.Routing.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        private static void InitializeStructureMap()
        {
            try
            {
                var bootStrapper = new Bootstrapper();
                bootStrapper.BootstrapStructureMap();
            }
            catch (Exception)
            {
#if DEBUG
                //For debugging mapping errors.
                Debugger.Break();
#endif
                throw;
            }
        }

        private static void InitializeControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new StructureMapControllerFactory());
        }

        private static void InitializeServices()
        {
            var services = ObjectFactory.GetAllInstances<IRecurringService>();
            foreach (var service in services)
            {
                //#if !DEBUG
                service.StartService();
                //#endif
            }
        }

        #endregion

        protected void Session_Start()
        {

        }
    }
}