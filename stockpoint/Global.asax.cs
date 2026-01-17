using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace stockpoint
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        // En WebApiConfig.cs
        public static void Register(HttpConfiguration config)
        {
            // Habilitar CORS
            var cors = new EnableCorsAttribute(
                origins: "*", // Para desarrollo, en producción especifica tus dominios
                headers: "*",
                methods: "*");
            config.EnableCors(cors);

            // Resto de tu configuración...
        }
        protected void Application_BeginRequest()
        {
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.AddHeader("Access-Control-Allow-Origin", "*");
                Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization");
                Response.End();
            }
        }
    }

}
