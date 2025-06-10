namespace Cooperativa.Domain.Models
{
    /// <summary>
    /// Representa el resultado de una validación de retiro
    /// </summary>
    public class RetiroValidationResult
    {
        /// <summary>
        /// Indica si el retiro es válido
        /// </summary>
        public bool EsValido { get; set; }

        /// <summary>
        /// Mensaje de error principal si la validación falla
        /// </summary>
        public string MensajeError { get; set; } = string.Empty;

        /// <summary>
        /// Lista detallada de todos los errores encontrados
        /// </summary>
        public List<string> ErroresDetallados { get; set; } = new List<string>();

        /// <summary>
        /// Constructor para resultado exitoso
        /// </summary>
        /// <returns>Resultado válido</returns>
        public static RetiroValidationResult Exitoso()
        {
            return new RetiroValidationResult { EsValido = true };
        }

        /// <summary>
        /// Constructor para resultado con error
        /// </summary>
        /// <param name="mensajeError">Mensaje de error principal</param>
        /// <returns>Resultado inválido</returns>
        public static RetiroValidationResult ConError(string mensajeError)
        {
            return new RetiroValidationResult
            {
                EsValido = false,
                MensajeError = mensajeError,
                ErroresDetallados = new List<string> { mensajeError }
            };
        }

        /// <summary>
        /// Constructor para resultado con múltiples errores
        /// </summary>
        /// <param name="errores">Lista de errores</param>
        /// <returns>Resultado inválido con múltiples errores</returns>
        public static RetiroValidationResult ConErrores(List<string> errores)
        {
            return new RetiroValidationResult
            {
                EsValido = false,
                MensajeError = errores.FirstOrDefault() ?? "Error desconocido",
                ErroresDetallados = errores
            };
        }
    }
}
