using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI.Library.Filter;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable attribute routing.
            config.MapHttpAttributeRoutes();

            // change default media type text/html to return json instead of xml.
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // add filter to validate the model.
            config.Filters.Add(new ValidateModelAttribute());

            // convention based routing:
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
