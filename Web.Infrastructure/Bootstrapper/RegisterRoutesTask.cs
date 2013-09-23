namespace Boilerplate.Bootstrapper {
    using System.Web.Mvc;
    using System.Web.Routing;
    using Bootstrap.Extensions.StartupTasks;

    public class RegisterRoutesTask : IStartupTask {
        public RegisterRoutesTask(RouteCollection routes) {
            _routes = routes;
        }
        public RegisterRoutesTask()
            : this(RouteTable.Routes) {
        }

        private readonly RouteCollection _routes;

        public void Run() {
            // Turns off the unnecessary file exists check
            _routes.RouteExistingFiles = true;

            // Ignore API pathways
            _routes.IgnoreRoute("api/{*pathInfo}");

            // Ignore axd files such as assest, image, sitemap etc
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Exclude favicon (google toolbar request gif file as fav icon)
            _routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            // Ignore the content directories which contains images, scripts, & css
            _routes.IgnoreRoute("Content/{*pathInfo}");
            _routes.IgnoreRoute("Scripts/{*pathInfo}");

            // Register default routes for the application
            _routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Ignore status, html, xml files.
            _routes.IgnoreRoute("{file}.ashx");
            _routes.IgnoreRoute("{file}.txt");
            _routes.IgnoreRoute("{file}.htm");
            _routes.IgnoreRoute("{file}.html");
            _routes.IgnoreRoute("{file}.xml");
        }
        public void Reset() {
        }
    }
}