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
### Add Unity as IoC Container
Install NuGet Package using *Package Manager Console* :
```Install-Package Unity```

Create a class that derives from ```IDependencyResolver``` and implement it:
```csharp
public class UnityResolver : IDependencyResolver
{
	private readonly IUnityContainer mUnityContainer;

	public UnityResolver(IUnityContainer aUnityContainer)
	{
		mUnityContainer = aUnityContainer;
	}

	#region Implementation of IDisposable

	public void Dispose()
	{
		mUnityContainer.Dispose();
	}

	#endregion

	#region Implementation of IDependencyScope

	public object GetService(Type serviceType)
	{
		try
		{
			return mUnityContainer.Resolve(serviceType);
		}
		catch (ResolutionFailedException)
		{
			return null;
		}
	}

	public IEnumerable<object> GetServices(Type serviceType)
	{
		try
		{
			return mUnityContainer.ResolveAll(serviceType);
		}
		catch (ResolutionFailedException)
		{
			return new List<object>();
		}
	}

	public IDependencyScope BeginScope()
	{
		return new UnityResolver(mUnityContainer.CreateChildContainer());
	}

	#endregion
}
```
Add a class to configure the new dependency resolver:
```csharp
public static class UnityConfig
{
	public static void Register(HttpConfiguration config)
	{
		var container = new UnityContainer();
		container.RegisterType<IMessageRepository, MessageRepository>(new ContainerControlledLifetimeManager());
		config.DependencyResolver = new UnityResolver(container);
	}
}
```
Finally, invoke the configuration within the Global.asax:
```csharp
GlobalConfiguration.Configure(UnityConfig.Register);
```

### Enable Swagger
Install NuGet Package using *Package Manager Console* :
```Install-Package Swashbuckle```

### Enable xml documentation
 Right click Web API project —> "Properties" -> "Build" tab. Check the "XML documentation file" checkbox and set the file path:
 
![GitHub Logo](/Resources/xmldocumentation.png)

Within the ```SwaggerConfig``` add the following method:
```csharp
protected static string GetXmlCommentsPath()
{
    return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\WebApi.XML";
}
```
and provide the path to the swagger config:
```csharp
c.IncludeXmlComments(GetXmlCommentsPath());
```
[Source](http://bitoftech.net/2014/08/25/asp-net-web-api-documentation-using-swagger/)

## Common Gotchas:
* At most one parameter is allowed to read from the message body thus multiple ```FromBody``` attributes are not allowed. So this will not work:
```public async Task<IHttpActionResult> AddMessageAsync([FromBody]Guid id, [FromBody]string message) { ... }```
The reason for this rule is that the request body might be stored in a non-buffered stream that can only be read once. [Source](http://www.asp.net/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api)

## Some snippets from [MICROSOFT REST API GUIDELINES](https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md)
### 5.1 ERRORS
Errors, or more specifically Service Errors, are defined as a client passing invalid data to the service and the service _correctly_ rejecting that data. Examples include invalid credentials, incorrect parameters, unknown version IDs, or similar. These are generally "4xx" HTTP error codes and are the result of a client passing incorrect or invalid data.

Errors do _not_ contribute to overall API availability.

### 5.2 FAULTS
Faults, or more specifically Service Faults, are defined as the service failing to correctly return in response to a valid client request. These are generally "5xx" HTTP error codes.

Faults _do_ contribute to the overall API availability.

Calls that fail due to rate limiting or quota failures MUST NOT count as faults. Calls that fail as the result of a service fast-failing requests (often for its own protection) do count as faults.


## Link list:
* [Microsoft Best Practices Web API](https://azure.microsoft.com/en-us/documentation/articles/best-practices-api-implementation/)
* [Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2)
* [Create a REST API with Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing)
* [How do I get ASP.NET Web API to return JSON instead of XML](http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome)
* [HTTP Status Codes](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)
* [Swashbuckle 5.0](https://github.com/domaindrivendev/Swashbuckle)
* [Dependency Injection in ASP.NET Web API 2](http://www.asp.net/web-api/overview/advanced/dependency-injection)
