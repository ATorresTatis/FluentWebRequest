//-----------------------------------------------------------------------
// <copyright file="IWebRequestParameter.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 10:00:11 AM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    using System;

    #endregion using

    /// <summary>
    /// Permite establecer el valor de un parámetro en la solictud.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    public interface IWebRequestParameter<T> where T : new()
    {
        /// <summary>
        /// Establece el valor de un parámetro en la solicitud.
        /// </summary>
        /// <param name="value">Valor del parámetro.</param>
        /// <returns>Instancia de <see cref="IWebRequestFluent{T}"/>.</returns>
        IWebRequestFluent<T> WithValue(object value);

        /// <summary>
        /// Establece el valor de un parámetro en la solicitud.
        /// </summary>
        /// <param name="value">Valor del parámetro.</param>
        /// <param name="provider">Objeto que proporciona información específica de la cultura de formato.</param>
        /// <returns>
        /// Instancia de <see cref="IWebRequestFluent{T}" />.
        /// </returns>
        IWebRequestFluent<T> WithValue(object value, IFormatProvider provider);
    }
}
