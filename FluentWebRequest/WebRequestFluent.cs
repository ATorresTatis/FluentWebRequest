//-----------------------------------------------------------------------
// <copyright file="WebRequestFluent.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 11:36:32 AM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion using

    /// <summary>
    /// Expone método para generar una petición HTTP Request.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    internal class WebRequestFluent<T> : IWebRequestFluent<T>, IWebRequestOperation<T>, IWebRequestParameter<T>, IWebRequestOptions<T> where T : new()
    {
        #region Fields

        /// <summary>
        /// Almacena la lista de parámetros que se deben enviar en la solicitud.
        /// </summary>
        private readonly Dictionary<string, Tuple<object, IFormatProvider>> parameters = new Dictionary<string, Tuple<object, IFormatProvider>>();

        /// <summary>
        /// Almacena el valor por defecto del campo Content-Type que se envia en la cabecera de la solicitud.
        /// </summary>
        private string contentType = "application/json; charset=utf-8";

        /// <summary>
        /// Almacena el valor que se debe enviar en el cuerpo de la solicitud.
        /// </summary>
        private string jsonBody;

        /// <summary>
        /// Almacena el nombre del último parámetro que se agrego a la lista de parámetros.
        /// </summary>
        private string lastKey;

        /// <summary>
        /// Almacena la URL a donde se debe enviar la solicitud.
        /// </summary>
        private string url;

        /// <summary>
        /// Almacena el valor que se debe establer en el campo User-agent en la cabecera de la solicitud.
        /// </summary>
        private string userAgent;

        /// <summary>
        /// Almacena el valor que se debe establer en el campo Accept en la cabecera de la solicitud.
        /// </summary>
        private string accept;

        /// <summary>
        /// Establece el método HTTP que se envia en la solicitud.
        /// </summary>
        private string webRequestMethod;

        /// <summary>
        /// Almacena el valor del proxy que se debe utilizar para realizar la solicitud.
        /// </summary>
        private IWebProxy proxy;

        /// <summary>
        /// Almacena una colección de los pares de nombre / valor que componen los encabezados HTTP.
        /// </summary>
        private NameValueCollection headers;

        /// <summary>
        /// Almacena el formato a utilizar para la construcción de la URL.
        /// </summary>
        private readonly UrlFormat urlFormat;

        #endregion

        #region WebRequestFluent

        /// <summary>
        /// Incializa una nueva instancia de la clase <see cref="WebRequestFluent{T}" />.
        /// </summary>
        /// <param name="url">URL a donde se debe enviar la solicitud.</param>
        /// <param name="urlFormat">Formato a utilizar para la construcción de la URL.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="url" /> es <c>null</c> o una cadena vacia.</exception>
        public WebRequestFluent(string url, UrlFormat urlFormat)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            this.url = url;
            this.urlFormat = urlFormat;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtiene un objeto que permite establecer las opciones de configuración del WebRequest.
        /// </summary>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> WithOptions
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region AddParameter

        /// <summary>
        /// Agrega un parámetro a la solicitud.
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro en la solicitud.</param>
        /// <returns>Instancia de IWebRequestParameter{T}.</returns>
        public IWebRequestParameter<T> AddParameter(string parameterName)
        {
            this.lastKey = parameterName;
            this.parameters.Add(parameterName, null);
            return this;
        }

        #endregion

        #region WithValue

        /// <summary>
        /// Establece el valor de un parámetro en la solicitud.
        /// </summary>
        /// <param name="value">Valor del parámetro.</param>
        /// <returns>Instancia de <see cref="IWebRequestFluent{T}"/>.</returns>
        public IWebRequestFluent<T> WithValue(object value)
        {
            return this.WithValue(value, null);
        }

        /// <summary>
        /// Establece el valor de un parámetro en la solicitud.
        /// </summary>
        /// <param name="value">Valor del parámetro.</param>
        /// <param name="provider">Objeto que proporciona información específica de la cultura de formato.</param>
        /// <returns>
        /// Instancia de <see cref="IWebRequestFluent{T}" />.
        /// </returns>
        public IWebRequestFluent<T> WithValue(object value, IFormatProvider provider)
        {
            IFormatProvider defaultProvider = provider ?? CultureInfo.CurrentCulture;
            this.parameters[this.lastKey] = new Tuple<object, IFormatProvider>(value, defaultProvider);
            return this;
        }

        #endregion

        #region Submit

        /// <summary>
        /// Permite finalizar la construcción de la petición para luego llamar al método de la solicitud.
        /// </summary>
        /// <returns>Instancia de IWebRequestOperation{T}.</returns>
        public IWebRequestOperation<T> Submit()
        {
            return this;
        }

        #endregion

        #region EndOptions

        /// <summary>
        /// Finaliza la configuración de opciones del WebRequest.
        /// </summary>
        /// <returns>Instancia de IWebRequestFluent{T}.</returns>
        public IWebRequestFluent<T> EndOptions()
        {
            return this;
        }

        #endregion

        #region SetRequestBody

        /// <summary>
        /// Establece el valor que debe enviarse en el cuerpo de la solicitud.
        /// </summary>
        /// <param name="jsonBody">Cadena en formato JSON que se debe establecer en el cuerpo de la solicitud.</param>
        /// <returns>Instancia de IWebRequestFluent{T}.</returns>
        public IWebRequestFluent<T> WithBody(string jsonBody)
        {
            this.jsonBody = jsonBody;
            return this;
        }

        /// <summary>
        /// Establece el valor que debe enviarse en el cuerpo de la solicitud. Se utiliza comúnmente en operaciones POST.
        /// </summary>
        /// <param name="instance">Instancia del objeto que se debe convertir a formato JSON qpara enviar en el cuerpo de la solicitud.</param>
        /// <param name="formatting">Especifica las opciones de formato.</param>
        /// <returns>
        /// Instancia de IWebRequestFluent{T}.
        /// </returns>
        public IWebRequestFluent<T> WithBody(object instance, Formatting formatting = Formatting.None)
        {
            if (instance != null)
            {
                this.jsonBody = JsonConvert.SerializeObject(instance, formatting);
            }

            return this;
        }

        #endregion

        #region ContentType

        /// <summary>
        /// Establece el valor del campo Content-Type en la cabecera de la solicitud.
        /// </summary>
        /// <param name="contentType">Tipo de contenido que se debe establecer.
        /// <example>text/plain, application/octet-stream, application/json</example>
        /// </param>
        /// <returns>Instancia de IWebRequestFluent{T}.</returns>
        public IWebRequestOptions<T> ContentType(string contentType)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                this.contentType = contentType;
            }

            return this;
        }

        #endregion

        #region Accept

        /// <summary>
        /// Establece el valor del campo Accept en la cabecera de la solicitud.
        /// </summary>
        /// <param name="accept">Valor de la propiedad Accept que se debe establecer.
        /// <example>image/*</example>
        /// </param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> Accept(string accept)
        {
            if (!string.IsNullOrWhiteSpace(accept))
            {
                this.accept = accept;
            }

            return this;
        }

        #endregion

        #region UserAgent

        /// <summary>
        /// Establece el valor del campo User-agent en la cabecera de la solicitud.
        /// </summary>
        /// <param name="userAgent">Valor que se debe establecer.</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> UserAgent(string userAgent)
        {
            this.userAgent = userAgent;
            return this;
        }

        #endregion

        #region Proxy

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="proxy">Información del proxy para establecer.</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> Proxy(IWebProxy proxy)
        {
            this.proxy = proxy;
            return this;
        }

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="address">Identificador URI del servidor proxy. </param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> Proxy(string address)
        {
            this.proxy = new WebProxy(address);
            return this;
        }

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="host">Nombre de host del proxy</param>
        /// <param name="port">Número de puerto en el Host que se va a utilizar.</param>
        /// <returns>
        /// Instancia de IWebRequestOptions{T}.
        /// </returns>
        public IWebRequestOptions<T> Proxy(string host, int port)
        {
            this.proxy = new WebProxy(host, port);
            return this;
        }

        #endregion

        #region Headers

        /// <summary>
        /// Especifica una colección de los pares de nombre / valor que componen los encabezados HTTP.
        /// </summary>
        /// <param name="headers">Colección de los pares de nombre / valor</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        public IWebRequestOptions<T> Headers(NameValueCollection headers)
        {
            this.headers = headers;
            return this;
        }

        #endregion

        #region Post

        /// <summary>
        /// Establece la operación POST y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        public T Post()
        {
            this.webRequestMethod = WebRequestMethods.Http.Post;
            return this.GetResponse();
        }

        #endregion

        #region Get

        /// <summary>
        /// Establece la operación GET y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        public T Get()
        {
            this.webRequestMethod = WebRequestMethods.Http.Get;
            return this.GetResponse();
        }

        #endregion

        #region Delete

        /// <summary>
        /// Establece la operación DELETE y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        public T Delete()
        {
            this.webRequestMethod = "DELETE";
            return this.GetResponse();
        }

        #endregion

        #region Put

        /// <summary>
        /// Establece la operación PUT y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        public T Put()
        {
            this.webRequestMethod = WebRequestMethods.Http.Put;
            return this.GetResponse();
        }

        #endregion

        #region GetResponse

        /// <summary>
        /// Realiza la petición, procesa la respuesta e instancia del tipo del objeto de retorno.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        private T GetResponse()
        {
            this.FixUrl();
            this.AppendQueryString();

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(this.url);
            httpRequest.ContentType = this.contentType;
            httpRequest.Method = this.webRequestMethod;
            httpRequest.Date = DateTime.Now;

            if (!string.IsNullOrEmpty(this.accept))
            {
                httpRequest.Accept = this.accept;
            }

            httpRequest.Proxy = this.proxy ?? httpRequest.Proxy;
            httpRequest.UserAgent = this.userAgent ?? ".NET HttpWebRequest";

            this.SetHeaders(httpRequest);

            httpRequest.ContentLength = string.IsNullOrWhiteSpace(this.jsonBody) ? 0 : Encoding.UTF8.GetBytes(this.jsonBody).Length;

            if (!string.IsNullOrWhiteSpace(this.jsonBody))
            {
                using (StreamWriter requestStream = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    requestStream.Write(this.jsonBody);
                    Debug.WriteLine("BODY:");
                    Debug.WriteLine(JsonConvert.SerializeObject(JObject.Parse(this.jsonBody), Formatting.Indented));
                }
            }

            try
            {
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (StreamReader responseStream = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string response = responseStream.ReadToEnd();
                        Debug.WriteLine("RESPONSE:");
                        Debug.WriteLine(JsonConvert.SerializeObject(JObject.Parse(response), Formatting.Indented));
                        return JsonConvert.DeserializeObject<T>(response);
                    }
                }
            }
            catch (WebException exception)
            {
                PrintException(exception);
                throw;
            }
        }

        /// <summary>
        /// Establece la información de la cabecera HTTP.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        private void SetHeaders(HttpWebRequest httpRequest)
        {
            if (this.headers != null)
            {
                Debug.WriteLine("HEADERS: ");
                foreach (string key in this.headers)
                {
                    httpRequest.Headers.Add(key, this.headers[key]);
                    Debug.WriteLine("{0}:\r\n{1}", key, (object)this.headers[key]);
                }
            }
        }

        /// <summary>
        /// Imprime la excepción en la salida Debug.
        /// </summary>
        /// <param name="exception">Excepción que se produjo al procesar la solicitud HTTP.</param>
        private static void PrintException(WebException exception)
        {
            Debug.WriteLine("WEB EXCEPTION");
            Debug.WriteLine(
                "Message: {0}\r\nStatus: {1}\r\nInnerException: {2}\r\nStackTrace: {3}", 
                exception.Message, 
                exception.Status, 
                exception.InnerException, 
                exception.StackTrace);

            if (exception.Response != null)
            {
                using (StreamReader exceptionStream = new StreamReader(exception.Response.GetResponseStream()))
                {
                    string response = exceptionStream.ReadToEnd();
                    Debug.WriteLine(JsonConvert.SerializeObject(JObject.Parse(response), Formatting.Indented));
                }
            }
        }

        #endregion

        #region FixUrl

        /// <summary>
        /// Corrige el valor de la URL de ser necesario.
        /// </summary>
        private void FixUrl()
        {
            if (!Regex.IsMatch(this.url, @"^https?://", RegexOptions.IgnoreCase))
            {
                this.url = string.Concat("http://", this.url);
            }
        }

        #endregion

        #region AppendQueryString

        /// <summary>
        /// Genera la parte de querystring en la URL de ser necesario.
        /// </summary>
        private void AppendQueryString()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;

            if (this.parameters.Any())
            {
                string keyValueSeparator = this.urlFormat == UrlFormat.Questions ? "?" : "/";
                string pathSeparator = this.urlFormat == UrlFormat.Questions ? "&" : "/";

                IEnumerable<string> paramList = this.parameters.Select(item => string.Format(ci, "{0}{1}{2}", item.Key, keyValueSeparator, Convert.ToString(item.Value.Item1, item.Value.Item2)));
                string queryString = string.Join(pathSeparator, paramList);
                this.url = string.Format(ci, "{0}{1}{2}", this.url, keyValueSeparator, queryString);
            }

            Debug.WriteLine("URL: {0}", (object)this.url);
        }

        #endregion
    }
}
