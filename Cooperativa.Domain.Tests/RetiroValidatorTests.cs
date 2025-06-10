using Cooperativa.Domain.Interfaces;
using Cooperativa.Domain.Models;
using Cooperativa.Domain.Services;
using NUnit.Framework;

namespace Cooperativa.Domain.Tests
{
    /// <summary>
    /// Pruebas unitarias para el validador de retiros de cuentas de ahorros
    /// Implementa los casos de prueba definidos en el análisis de requisitos
    /// </summary>
    [TestFixture]
    public class RetiroValidatorTests
    {
        private IRetiroValidator _retiroValidator;

        [SetUp]
        public void Setup()
        {
            _retiroValidator = new RetiroValidator();
        }

        /// <summary>
        /// TC1: Retiro exitoso - todas las condiciones válidas
        /// Verifica que un retiro con todas las condiciones válidas sea aprobado
        /// Entrada: Cuenta activa, saldo $1000, monto $100, límite $500, no bloqueada
        /// Resultado esperado: Éxito
        /// </summary>
        [Test]
        public void TC1_RetiroExitoso_TodasLasCondicionesValidas()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.True, "El retiro debería ser válido");
            Assert.That(resultado.MensajeError, Is.Empty, "No debería haber mensaje de error");
            Assert.That(resultado.ErroresDetallados, Is.Empty, "No debería haber errores detallados");
        }

        /// <summary>
        /// TC2: Falla por cuenta inactiva
        /// Verifica rechazo cuando la cuenta está inactiva
        /// Entrada: Cuenta inactiva, saldo $1000, monto $100, límite $500, no bloqueada
        /// Resultado esperado: Falla con mensaje "La cuenta no está activa"
        /// </summary>
        [Test]
        public void TC2_FallaPorCuentaInactiva()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = false, // Cuenta inactiva
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.MensajeError, Is.EqualTo("La cuenta no está activa"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("La cuenta no está activa"));
        }

        /// <summary>
        /// TC3: Falla por saldo insuficiente
        /// Verifica rechazo cuando el saldo es menor al monto solicitado
        /// Entrada: Cuenta activa, saldo $50, monto $100, límite $500, no bloqueada
        /// Resultado esperado: Falla con mensaje "Saldo insuficiente"
        /// </summary>
        [Test]
        public void TC3_FallaPorSaldoInsuficiente()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 50m, // Saldo insuficiente
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.MensajeError, Is.EqualTo("Saldo insuficiente para realizar el retiro"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("Saldo insuficiente para realizar el retiro"));
        }

        /// <summary>
        /// TC4: Falla por exceder límite diario
        /// Verifica rechazo cuando se excede el límite de retiro diario
        /// Entrada: Cuenta activa, saldo $1000, monto $600, límite $500, no bloqueada
        /// Resultado esperado: Falla con mensaje "Excede límite diario"
        /// </summary>
        [Test]
        public void TC4_FallaPorExcederLimiteDiario()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 600m, // Excede límite diario
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.MensajeError, Is.EqualTo("El monto solicitado excede el límite de retiro diario"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("El monto solicitado excede el límite de retiro diario"));
        }

        /// <summary>
        /// TC5: Falla por cuenta bloqueada
        /// Verifica rechazo cuando la cuenta está bloqueada por fraude
        /// Entrada: Cuenta activa, saldo $1000, monto $100, límite $500, bloqueada
        /// Resultado esperado: Falla con mensaje "Cuenta bloqueada"
        /// </summary>
        [Test]
        public void TC5_FallaPorCuentaBloqueada()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = true // Cuenta bloqueada
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.MensajeError, Is.EqualTo("La cuenta está bloqueada por sospecha de fraude"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("La cuenta está bloqueada por sospecha de fraude"));
        }

        /// <summary>
        /// TC6: Falla por monto no múltiplo de 10
        /// Verifica rechazo cuando el monto no es múltiplo de 10
        /// Entrada: Cuenta activa, saldo $1000, monto $105, límite $500, no bloqueada
        /// Resultado esperado: Falla con mensaje "Monto debe ser múltiplo de 10"
        /// </summary>
        [Test]
        public void TC6_FallaPorMontoNoMultiploDe10()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 105m, // No es múltiplo de 10
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.MensajeError, Is.EqualTo("El monto debe ser múltiplo de 10"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("El monto debe ser múltiplo de 10"));
        }

        /// <summary>
        /// TC7: Falla múltiple - saldo y múltiplo
        /// Verifica comportamiento con múltiples errores simultáneos
        /// Entrada: Cuenta activa, saldo $50, monto $75, límite $500, no bloqueada
        /// Resultado esperado: Falla con múltiples errores
        /// </summary>
        [Test]
        public void TC7_FallaMultiple_SaldoYMultiplo()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 50m, // Saldo insuficiente
                MontoSolicitado = 75m, // No es múltiplo de 10
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.ErroresDetallados.Count, Is.EqualTo(2), "Debería haber exactamente 2 errores");
            Assert.That(resultado.ErroresDetallados, Contains.Item("Saldo insuficiente para realizar el retiro"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("El monto debe ser múltiplo de 10"));
        }

        /// <summary>
        /// TC8: Caso límite - monto igual al saldo
        /// Verifica caso límite donde el monto solicitado es igual al saldo disponible
        /// Entrada: Cuenta activa, saldo $200, monto $200, límite $500, no bloqueada
        /// Resultado esperado: Éxito
        /// </summary>
        [Test]
        public void TC8_CasoLimite_MontoIgualAlSaldo()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 200m,
                MontoSolicitado = 200m, // Monto igual al saldo
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.True, "El retiro debería ser válido");
            Assert.That(resultado.MensajeError, Is.Empty, "No debería haber mensaje de error");
            Assert.That(resultado.ErroresDetallados, Is.Empty, "No debería haber errores detallados");
        }

        /// <summary>
        /// Prueba adicional: Validación de entrada nula
        /// Verifica el comportamiento cuando se pasa una solicitud nula
        /// </summary>
        [Test]
        public void ValidarRetiro_ConSolicitudNula_DeberiaLanzarExcepcion()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _retiroValidator.ValidarRetiro(null!));
        }

        /// <summary>
        /// Prueba adicional: Múltiples errores simultáneos
        /// Verifica que se capturen todos los errores cuando múltiples condiciones fallan
        /// </summary>
        [Test]
        public void ValidarRetiro_ConTodosLosErrores_DeberiaRetornarTodosLosErrores()
        {
            // Arrange
            var request = new RetiroValidationRequest
            {
                EstaCuentaActiva = false, // Error 1
                SaldoActual = 50m, // Error 2: saldo insuficiente
                MontoSolicitado = 675m, // Error 3: excede límite diario, Error 4: no múltiplo de 10
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = true // Error 5
            };

            // Act
            var resultado = _retiroValidator.ValidarRetiro(request);

            // Assert
            Assert.That(resultado.EsValido, Is.False, "El retiro no debería ser válido");
            Assert.That(resultado.ErroresDetallados.Count, Is.EqualTo(5), "Debería haber exactamente 5 errores");
            
            // Verificar que todos los errores esperados estén presentes
            Assert.That(resultado.ErroresDetallados, Contains.Item("La cuenta no está activa"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("Saldo insuficiente para realizar el retiro"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("El monto solicitado excede el límite de retiro diario"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("La cuenta está bloqueada por sospecha de fraude"));
            Assert.That(resultado.ErroresDetallados, Contains.Item("El monto debe ser múltiplo de 10"));
        }
    }
}
