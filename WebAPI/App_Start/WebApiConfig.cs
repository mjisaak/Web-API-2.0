using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            
            // add filter to validate the model.
            config.Filters.Add(new ValidateModelAttribute());

            ConfigureJsonFormatter(config);                        
        }

        /// <summary>
        /// Sets JSON as the default response for text/html and configures the formatter to use camel case ContractResolver
        /// and indented formatting.
        /// </summary>
        private static void ConfigureJsonFormatter(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
