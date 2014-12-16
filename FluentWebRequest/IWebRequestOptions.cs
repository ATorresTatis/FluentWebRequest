//-----------------------------------------------------------------------
// <copyright file="IWebRequestOptions.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 2:41:57 PM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    using System;
    using System.Collections.Specialized;
    using System.Net;

    #endregion using

    /// <summary>
    /// Permite establecer las opciones de configuración del WebRequest.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    public interface IWebRequestOptions<T> where T : new()
    {
        /// <summary>
        /// Establece el valor del campo Content-Type en la cabecera de la solicitud.
        /// </summary>
        /// <param name="contentType">Tipo de contenido que se debe establecer.
        /// <example>text/plain, application/octet-stream, application/json</example>
        /// </param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> ContentType(string contentType);

        /// <summary>
        /// Establece el valor del campo Accept en la cabecera de la solicitud.
        /// </summary>
        /// <param name="accept">Valor de la propiedad Accept que se debe establecer.
        /// <example>image/*</example>
        /// </param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> Accept(string accept);

        /// <summary>
        /// Establece el valor del campo User-agent en la cabecera de la solicitud.
        /// </summary>
        /// <param name="userAgent">Valor que se debe establecer.</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> UserAgent(string userAgent);

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="proxy">Información del proxy para establecer.</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> Proxy(IWebProxy proxy);

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="address">Identificador URI del servidor proxy. </param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> Proxy(string address);

        /// <summary>
        /// Establece la información del proxy para la solicitud.
        /// </summary>
        /// <param name="host">Nombre de host del proxy</param>
        /// <param name="port">Número de puerto en el Host que se va a utilizar.</param>
        /// <returns>
        /// Instancia de IWebRequestOptions{T}.
        /// </returns>
        IWebRequestOptions<T> Proxy(string host, int port);

        /// <summary>
        /// Especifica una colección de los pares de nombre / valor que componen los encabezados HTTP.
        /// </summary>
        /// <param name="headers">Colección de los pares de nombre / valor</param>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> Headers(NameValueCollection headers);

        /// <summary>
        /// Finaliza la configuración de opciones del WebRequest.
        /// </summary>
        /// <returns>Instancia de IWebRequestFluent{T}.</returns>
        IWebRequestFluent<T> EndOptions();
    }
}
