//-----------------------------------------------------------------------
// <copyright file="WebRequestBuilder.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 12:24:53 PM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    #endregion using

    /// <summary>
    /// Crea un objeto <see cref="WebRequestFluent{T}" /> para hacer una solicitud HTTP Request.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    public static class WebRequestBuilder<T> where T : new()
    {
        /// <summary>
        /// Creauna instancia de un objeto <see cref="WebRequestFluent{T}" /> para la URL especificada.
        /// </summary>
        /// <param name="url">URL a donde se debe enviar la solicitud.</param>
        /// <param name="urlFormat">Formato a utilizar para la construcción de la URL.</param>
        /// <returns>
        /// Instancia de <see cref="IWebRequestFluent{T}" />.
        /// </returns>
        public static IWebRequestFluent<T> ForUrl(string url, UrlFormat urlFormat = UrlFormat.Questions)
        {
            return new WebRequestFluent<T>(url, urlFormat);
        }
    }
}
