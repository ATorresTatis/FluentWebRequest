//-----------------------------------------------------------------------
// <copyright file="IWebRequestOperation.cs" company="ACME">
// Copyright (c) 2013 Todos los derechos reservados.
// </copyright>
// <author>Atorres</author>
// <date>1/18/2013 10:00:31 AM</date>
//-----------------------------------------------------------------------
namespace FluentWebRequest
{
    #region using

    #endregion using

    /// <summary>
    /// Permite establecer la operación que se debe enviar en la solicitud.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto que retorna la solicitud.</typeparam>
    public interface IWebRequestOperation<out T> where T : new()
    {
        /// <summary>
        /// Establece la operación POST y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        T Post();

        /// <summary>
        /// Establece la operación GET y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        T Get();

        /// <summary>
        /// Establece la operación DELETE y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        T Delete();

        /// <summary>
        /// Establece la operación PUT y envia la solicitud.
        /// </summary>
        /// <returns>Instancia de tipo <c>T</c> del objeto que retorna la solicitud.</returns>
        T Put();
    }
}
