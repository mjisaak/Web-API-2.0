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
### 7.4 SUPPORTED VERBS
Operations MUST use the proper HTTP verbs whenever possible, and operation idempotency MUST be respected.

Below is a list of verbs that Microsoft REST services SHOULD support. Not all resources will support all verbs, but all resources using the verbs below MUST conform to their usage.  

Verb    | Description                                                                                                                | Is Idempotent
------- | -------------------------------------------------------------------------------------------------------------------------- | -------------
GET     | Return the current value of an object                                                                                      | True
PUT     | Replace an object, or create a named object, when applicable                                                               | True
DELETE  | Delete an object                                                                                                           | True
POST    | Create a new object based on the data provided, or submit a command                                                        | False
HEAD    | Return metadata of an object for a GET response. Resources that support the GET method MAY support the HEAD method as well | True
PATCH   | Apply a partial update to an object                                                                                        | False
OPTIONS | Get information about a request; see below for details.                                                                    | False

<small>Table 1</small>


According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH.

#### 7.4.1 POST
POST operations SHOULD support the Location response header to specify the location of any created object that was not explicitly named, via the Location header.

As an example, imagine a service that allows creation of hosted servers, which will be named by the service:

```http
POST http://api.contoso.com/account1/servers
```

The response would be something like:

```http
201 Created
Location: http://api.contoso.com/account1/servers/server321
```

Where "server321" is the service-allocated server name.

Services MAY also return the full metadata for the created item in the response.

#### 7.4.2 PATCH
PATCH has been standardized by IETF as the verb to be used for updating an existing object incrementally (see [RFC 5789][rfc-5789]). Microsoft REST API Guidelines compliant APIs SHOULD support PATCH.  

#### 7.4.3 CREATING RESOURCES VIA PATCH (UPSERT SEMANTICS)
Services that allow callers to specify key values on create SHOULD support UPSERT semantics, and those that do MUST support creating resources using PATCH. Because PUT is defined as a complete replacement of the content, it is dangerous for clients to use PUT to modify data. Clients that do not understand (and hence ignore) properties on a resource are not likely to provide them on a PUT when trying to update a resource, hence such properties MAY be inadvertently removed. Services MAY optionally support PUT to update existing resources, but if they do they MUST use replacement semantics (that is, after the PUT, the resource's properties MUST match what was provided in the request, including deleting any server properties that were not provided).

Under UPSERT semantics, a PATCH call to a nonexistent resource is handled by the server as a "create," and a PATCH call to an existing resource is handled as an "update." To ensure that an update request is not treated as a create or vice-versa, the client MAY specify precondition HTTP headers in the request. The service MUST NOT treat a PATCH request as an insert if it contains an If-Match header and MUST NOT treat a PATCH request as an update if it contains an If-None-Match header with a value of "*".

If a service does not support UPSERT, then a PATCH call against a resource that does not exist MUST result in an HTTP "409 Conflict" error.
#### 7.10 RESPONSE FORMATS
...
JSON property names SHOULD be camelCased.
...
Services SHOULD provide JSON as the default encoding.

### 8 CORS
Services compliant with the Microsoft REST API Guidelines MUST support CORS (Cross Origin Resource Sharing). Services SHOULD support an allowed origin of CORS * and enforce authorization through valid OAuth tokens

## Link list:
* [Microsoft Best Practices Web API](https://azure.microsoft.com/en-us/documentation/articles/best-practices-api-implementation/)
* [Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2)
* [Create a REST API with Attribute Routing in ASP.NET Web API 2](http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing)
* [How do I get ASP.NET Web API to return JSON instead of XML](http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome)
* [HTTP Status Codes](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)
* [Swashbuckle 5.0](https://github.com/domaindrivendev/Swashbuckle)
* [Dependency Injection in ASP.NET Web API 2](http://www.asp.net/web-api/overview/advanced/dependency-injection)
