namespace ErrorLog.WebApi
{
    using Owin;
    using System.Web.Http;

    public class Startup
    {
        // Write to Package Manager Console "Install-Package Microsoft.AspNet.WebApi.OwinSelfHost"

        // This code configures Web API. The Startup class is specified as a type parameter in the
        // WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MessageHandlers.Add(new LoggingHandler());
            appBuilder.UseWebApi(config);
            // GlobalConfiguration.Configuration.MessageHandlers.Add(New LoggingHandler())
        }
    }
}