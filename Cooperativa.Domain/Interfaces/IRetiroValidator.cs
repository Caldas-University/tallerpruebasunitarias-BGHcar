using Cooperativa.Domain.Models;

namespace Cooperativa.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para validador de retiros de cuentas de ahorros
    /// </summary>
    public interface IRetiroValidator
    {
        /// <summary>
        /// Valida si un retiro puede ser realizado según las reglas de negocio
        /// </summary>
        /// <param name="request">Solicitud de validación con los datos necesarios</param>
        /// <returns>Resultado de la validación indicando si es válido y errores si los hay</returns>
        RetiroValidationResult ValidarRetiro(RetiroValidationRequest request);
    }
}
