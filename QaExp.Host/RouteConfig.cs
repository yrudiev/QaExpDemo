using System.Web;
using System.Web.Routing;

namespace QaExp.Host
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.Ignore("handler/{*path}");

            //routes.MapPageRoute(
                //routeName: "Default",
                //routeUrl: "{controller}/{action}/{id}"
                /*true,
                defaults: new { controller = "Home", action = "Index"}*/
            //);
        }
    }
}