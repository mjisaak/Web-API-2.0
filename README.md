# Web-API-2.0
The repository contains some Web API 2.0 snippets.

## Common tasks:
###  Make JSON the default response for a web browser (which sends Accept: text/html)
		public static void Register(HttpConfiguration config)
		{
			// enable attribute routing.
			config.MapHttpAttributeRoutes();

			// change default media type text/html to return json instead of xml.
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
		}

## Link list:
* [Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2)
* [Create a REST API with Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing)
* [How do I get ASP.NET Web API to return JSON instead of XML](http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome)
