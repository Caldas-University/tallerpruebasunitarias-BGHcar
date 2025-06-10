using Cooperativa.Domain.Interfaces;
using Cooperativa.Domain.Models;

namespace Cooperativa.Domain.Services
{
    /// <summary>
    /// Implementación del validador de retiros para cuentas de ahorros básicas
    /// </summary>
    public class RetiroValidator : IRetiroValidator
    {        /// <summary>
        /// Valida si un retiro puede ser realizado según todas las reglas de negocio
        /// </summary>
        /// <param name="request">Solicitud con los datos del retiro</param>
        /// <returns>Resultado de la validación</returns>
        /// <exception cref="ArgumentNullException">Se lanza cuando request es null</exception>
        public RetiroValidationResult ValidarRetiro(RetiroValidationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "La solicitud de retiro no puede ser nula");

            var errores = new List<string>();

            // Validación 1: La cuenta debe estar activa
            if (!request.EstaCuentaActiva)
            {
                errores.Add("La cuenta no está activa");
            }

            // Validación 2: Debe tener saldo suficiente
            if (request.SaldoActual < request.MontoSolicitado)
            {
                errores.Add("Saldo insuficiente para realizar el retiro");
            }

            // Validación 3: No debe exceder el límite de retiro diario
            if (request.MontoSolicitado > request.LimiteRetiroDiario)
            {
                errores.Add("El monto solicitado excede el límite de retiro diario");
            }

            // Validación 4: La cuenta no debe estar bloqueada por fraude
            if (request.EstaBloqueadaPorFraude)
            {
                errores.Add("La cuenta está bloqueada por sospecha de fraude");
            }

            // Validación 5: El monto debe ser múltiplo de 10
            if (request.MontoSolicitado % 10 != 0)
            {
                errores.Add("El monto debe ser múltiplo de 10");
            }

            // Si no hay errores, el retiro es válido
            if (errores.Count == 0)
            {
                return RetiroValidationResult.Exitoso();
            }

            // Si hay errores, retornar el resultado con los errores
            return RetiroValidationResult.ConErrores(errores);
        }
    }
}
