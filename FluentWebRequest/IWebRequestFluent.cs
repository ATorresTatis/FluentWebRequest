//-----------------------------------------------------------------------
// <copyright file="IWebRequestFluent.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 10:00:24 AM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    using System;
    using System.Net;
    using Newtonsoft.Json;

    #endregion using

    /// <summary>
    /// Expone una interfaz Fluent para al clase <see cref="WebRequest" />.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    public interface IWebRequestFluent<T> where T : new()
    {
        /// <summary>
        /// Obtiene un objeto que permite establecer las opciones de configuración del WebRequest.
        /// </summary>
        /// <returns>Instancia de IWebRequestOptions{T}.</returns>
        IWebRequestOptions<T> WithOptions
        {
            get;
        }

        /// <summary>
        /// Agrega un parámetro a la solicitud.
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro en la solicitud.</param>
        /// <returns>Instancia de IWebRequestParameter{T}.</returns>
        IWebRequestParameter<T> AddParameter(string parameterName);

        /// <summary>
        /// Establece el valor que debe enviarse en el cuerpo de la solicitud.
        /// </summary>
        /// <param name="jsonBody">Cadena en formato JSON que se debe establecer en el cuerpo de la solicitud.</param>
        /// <returns>Instancia de IWebRequestFluent{T}.</returns>
        IWebRequestFluent<T> WithBody(string jsonBody);

        /// <summary>
        /// Establece el valor que debe enviarse en el cuerpo de la solicitud. Se utiliza comúnmente en operaciones POST.
        /// </summary>
        /// <param name="instance">Instancia del objeto que se debe convertir a formato JSON qpara enviar en el cuerpo de la solicitud.</param>
        /// <param name="formatting">Especifica las opciones de formato.</param>
        /// <returns>
        /// Instancia de IWebRequestFluent{T}.
        /// </returns>
        IWebRequestFluent<T> WithBody(object instance, Formatting formatting);

        /// <summary>
        /// Permite finalizar la construcción de la petición para luego llamar al método de la solicitud.
        /// </summary>
        /// <returns>Instancia de IWebRequestOperation{T}.</returns>
        IWebRequestOperation<T> Submit();
    }
}
