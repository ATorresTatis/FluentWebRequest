//-----------------------------------------------------------------------
// <copyright file="UnitTest.cs" company="ACME">
// Copyright (c) 2014 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2014 12:24:53 PM</date>
//-----------------------------------------------------------------------

namespace FluentWebRequestTest
{
    using System;
    using System.Collections.Specialized;
    using FluentAssertions;
    using FluentWebRequest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Implementa las métodos de prueba de la clase WebRequestBuilder.
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// Verifica el funcionamiento de una solicitud GET simple.
        /// </summary>
        [TestMethod]
        public void TestSimpleGetMethod()
        {
            const string Url = "http://ip.jsontest.com/";

            JObject response = WebRequestBuilder<JObject>
                .ForUrl(Url)
                .Submit()
                .Get();

            response["ip"].ToObject<string>().Should().NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Verifica el funcionamiento de una solicitud POST simple.
        /// </summary>
        [TestMethod]
        public void TestSimplePostMethod()
        {
            const string Url = "http://ip.jsontest.com/";

            JObject response = WebRequestBuilder<JObject>
                .ForUrl(Url)
                .Submit()
                .Post();

            response["ip"].ToObject<string>().Should().NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Verifica el funcionamiento de una solicitud con encabezados.
        /// </summary>
        [TestMethod]
        public void TestHeaders()
        {
            const string Url = "http://headers.jsontest.com/";

            NameValueCollection headers = new NameValueCollection
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };

            JObject response = WebRequestBuilder<JObject>
                .ForUrl(Url)
                .WithOptions.Headers(headers).EndOptions()
                .Submit()
                .Post();

            response["key1"].ToObject<string>().Should().Be("value1");
            response["key2"].ToObject<string>().Should().Be("value2");
        }

        /// <summary>
        /// Verifica el funcionamiento de una solicitud GET con parámetros.
        /// </summary>
        [TestMethod]
        public void TestGetEchoMethod()
        {
            const string Url = "http://echo.jsontest.com/";

            JObject response = WebRequestBuilder<JObject>
                .ForUrl(Url, UrlFormat.Slashes)
                .AddParameter("param1").WithValue("abc")
                .AddParameter("param2").WithValue("123")
                .Submit()
                .Get();

            response["param1"].ToObject<string>().Should().Be("abc");
            response["param2"].ToObject<string>().Should().Be("123");
        }

        /// <summary>
        /// Verifica el funcionamiento de una solicitud POST con parámetros.
        /// </summary>
        [TestMethod]
        public void TestPostEchoMethod()
        {
            const string Url = "http://echo.jsontest.com/";

            JObject response = WebRequestBuilder<JObject>
                .ForUrl(Url, UrlFormat.Slashes)
                .AddParameter("param1").WithValue("abc")
                .AddParameter("param2").WithValue("123")
                .Submit()
                .Post();

            response["param1"].ToObject<string>().Should().Be("abc");
            response["param2"].ToObject<string>().Should().Be("123");
        }

        /// <summary>
        /// Verifica el funcionamiento de una solicitud GET con parámetros retornado un objeto tipado.
        /// </summary>
        [TestMethod]
        public void TestGetTypedEchoMethod()
        {
            const string Url = "http://echo.jsontest.com/";

            Person response = WebRequestBuilder<Person>
                .ForUrl(Url, UrlFormat.Slashes)
                .AddParameter("FirstName").WithValue("Jhon")
                .AddParameter("LastName").WithValue("Doe")
                .Submit()
                .Get();

            response.FirstName.Should().Be("Jhon");
            response.LastName.Should().Be("Doe");
        }

    }
}
