FluentWebRequest
================

A simple wrapper of WebRequest class using Fluent style

The need to consume services increases every day. It is common for a single project have to exchange information with multiple (2, 3, 5, 10) services, particularly REST type services.

Using .NET [WebRequest class](http://msdn.microsoft.com/en-us/library/system.net.webrequest(v=vs.100).aspx) it is relatively simple, so it's normal to find code like this in our projects:

```csharp
string serviceUrl = @"https://www.acme.com/services/data/url";

HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
httpRequest.ContentType = "application/json; charset=utf-8";
httpRequest.Method = WebRequestMethods.Http.Post;

JObject jsonRequest = new JObject(new JProperty("dataForRequestBody", someValue));
string requestBody = JsonConvert.SerializeObject(jsonRequest);

using (StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
{
	streamWriter.Write(requestBody);
}

using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
{
	using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
	{
		string response = streamReader.ReadToEnd();
		// initialize an object to wrap the response
		// A lot of code to create an object response
	}
}
```

But, what if instead of use the "traditional" style, we use the Fluent style that make it code easier to read and self-expressive? Then, the same above code would look like this:

```csharp
JObject jsonRequest = new JObject(new JProperty("dataForRequestBody", someValue));

SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.WithBody(jsonRequest)
	.Submit()
	.Post();
```

I implemented some classes and interfaces that allow it. They are fairly simple and easy to understand (I hope).

### Code Samples

#### Add parameters to URL (querystring)

```csharp
SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.AddParameter("param1").WithValue("abc")
	.AddParameter("param2").WithValue(123)
	.Submit()
	.Get();
```

#### Use IFormatProvider for parameter value

```csharp
NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
nfi.NumberDecimalSeparator = ".";

DateTimeFormatInfo dfi = (DateTimeFormatInfo)CultureInfo.CurrentCulture.DateTimeFormat.Clone();
dfi.ShortTimePattern = "yyyyMMdd";

SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.AddParameter("param1").WithValue("abc")
	.AddParameter("date").WithValue(DateTime.Now, dfi)
	.AddParameter("param2").WithValue(123.4, nfi)
	.Submit()
	.Get();
```

#### Use options to setup Request object

```csharp
SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.WithOptions
		.ContentType("text/plain")
		.UserAgent("IExplorer")
		.Proxy(myProxy)
	.EndOptions()
	.AddParameter("param1").WithValue("abc")
	.Submit()
	.Get();
```
#### Send a DELETE

```csharp
SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.AddParameter("lang").WithValue("de")
	.Submit()
	.Delete();
```

#### Add custom fields to request header

```csharp
NameValueCollection headers = new NameValueCollection()
{
	{ "APPLICATION_KEY", "abc123" }
};

SomeResponseClass response = WebRequestBuilder<SomeResponseClass>
	.ForUrl("https://www.acme.com/services/data/url")
	.WithOptions.Headers(headers).EndOptions()
	.Submit()
	.Delete();
```