//-----------------------------------------------------------------------
// <copyright file="UrlFormat.cs" company="Processa"> 
// Copyright (c) 2014 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>7/29/2014 3:27:11 PM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Define la forma en que se construye la URL del servicio.
    /// </summary>
    public enum UrlFormat
    {
        /// <summary>
        /// Se utiliza <c>slash</c> como separador de valores.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// http://example.com/products/123 
        /// ]]>
        /// </example>
        Slashes,

        /// <summary>
        /// Se utiliza <c>question</c> como separador de valores.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// http://example.com/?products=123 
        /// ]]>
        /// </example>        
        Questions        
    }
}
