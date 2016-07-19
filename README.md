# Web-API-2.0
The repository contains some Web API 2.0 snippets.

## Common tasks:
###  Make JSON the default response for a web browser (which sends Accept: text/html)
```csharp
public static void Register(HttpConfiguration config)
{
	// enable attribute routing.
	config.MapHttpAttributeRoutes();
	
	// change default media type text/html to return json instead of xml.
	config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
}
```
### Enable Swagger
Install NuGet Package using *Package Manager Console* :
```Install-Package Swashbuckle```

## Link list:
* [Microsoft Best Practices Web API](https://azure.microsoft.com/en-us/documentation/articles/best-practices-api-implementation/)
* [Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2)
* [Create a REST API with Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing)
* [How do I get ASP.NET Web API to return JSON instead of XML](http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome)
* [HTTP Status Codes](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)
* [Swashbuckle 5.0](https://github.com/domaindrivendev/Swashbuckle)
