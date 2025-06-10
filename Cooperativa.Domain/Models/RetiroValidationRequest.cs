namespace Cooperativa.Domain.Models
{
    /// <summary>
    /// Representa una solicitud de validación para un retiro de cuenta de ahorros
    /// </summary>
    public class RetiroValidationRequest
    {
        /// <summary>
        /// Indica si la cuenta está activa
        /// </summary>
        public bool EstaCuentaActiva { get; set; }

        /// <summary>
        /// Saldo actual disponible en la cuenta
        /// </summary>
        public decimal SaldoActual { get; set; }

        /// <summary>
        /// Monto que se desea retirar
        /// </summary>
        public decimal MontoSolicitado { get; set; }

        /// <summary>
        /// Límite máximo de retiro diario permitido
        /// </summary>
        public decimal LimiteRetiroDiario { get; set; }

        /// <summary>
        /// Indica si la cuenta está bloqueada por fraude
        /// </summary>
        public bool EstaBloqueadaPorFraude { get; set; }
    }
}
