using Cooperativa.Domain.Interfaces;
using Cooperativa.Domain.Models;
using Cooperativa.Domain.Services;

namespace Cooperativa.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE VALIDACIÓN DE RETIROS ===");
            Console.WriteLine("Cooperativa Financiera - Demo");
            Console.WriteLine();

            // Crear instancia del validador
            IRetiroValidator validator = new RetiroValidator();

            // Ejecutar casos de demostración
            EjecutarCasosDemo(validator);

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static void EjecutarCasosDemo(IRetiroValidator validator)
        {
            // Caso 1: Retiro exitoso
            Console.WriteLine("🟢 CASO 1: Retiro Exitoso");
            var caso1 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro de $100 con saldo de $1000", validator.ValidarRetiro(caso1));

            // Caso 2: Cuenta inactiva
            Console.WriteLine("\n🔴 CASO 2: Cuenta Inactiva");
            var caso2 = new RetiroValidationRequest
            {
                EstaCuentaActiva = false,
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro con cuenta inactiva", validator.ValidarRetiro(caso2));

            // Caso 3: Saldo insuficiente
            Console.WriteLine("\n🔴 CASO 3: Saldo Insuficiente");
            var caso3 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 50m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro de $100 con saldo de $50", validator.ValidarRetiro(caso3));

            // Caso 4: Excede límite diario
            Console.WriteLine("\n🔴 CASO 4: Excede Límite Diario");
            var caso4 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 600m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro de $600 con límite de $500", validator.ValidarRetiro(caso4));

            // Caso 5: Cuenta bloqueada por fraude
            Console.WriteLine("\n🔴 CASO 5: Cuenta Bloqueada por Fraude");
            var caso5 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 100m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = true
            };
            MostrarResultado("Retiro con cuenta bloqueada", validator.ValidarRetiro(caso5));

            // Caso 6: Monto no múltiplo de 10
            Console.WriteLine("\n🔴 CASO 6: Monto No Múltiplo de 10");
            var caso6 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 1000m,
                MontoSolicitado = 105m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro de $105 (no múltiplo de 10)", validator.ValidarRetiro(caso6));

            // Caso 7: Múltiples errores
            Console.WriteLine("\n🔴 CASO 7: Múltiples Errores");
            var caso7 = new RetiroValidationRequest
            {
                EstaCuentaActiva = false,
                SaldoActual = 50m,
                MontoSolicitado = 675m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = true
            };
            MostrarResultado("Múltiples condiciones inválidas", validator.ValidarRetiro(caso7));

            // Caso 8: Caso límite - monto igual al saldo
            Console.WriteLine("\n🟢 CASO 8: Caso Límite - Retiro Total");
            var caso8 = new RetiroValidationRequest
            {
                EstaCuentaActiva = true,
                SaldoActual = 200m,
                MontoSolicitado = 200m,
                LimiteRetiroDiario = 500m,
                EstaBloqueadaPorFraude = false
            };
            MostrarResultado("Retiro de todo el saldo ($200)", validator.ValidarRetiro(caso8));
        }

        static void MostrarResultado(string descripcion, RetiroValidationResult resultado)
        {
            Console.WriteLine($"📋 Descripción: {descripcion}");
            
            if (resultado.EsValido)
            {
                Console.WriteLine("✅ RESULTADO: RETIRO APROBADO");
            }
            else
            {
                Console.WriteLine("❌ RESULTADO: RETIRO RECHAZADO");
                Console.WriteLine($"   📝 Error principal: {resultado.MensajeError}");
                
                if (resultado.ErroresDetallados.Count > 1)
                {
                    Console.WriteLine("   📋 Errores detallados:");
                    foreach (var error in resultado.ErroresDetallados)
                    {
                        Console.WriteLine($"   • {error}");
                    }
                }
            }
            
            Console.WriteLine(new string('-', 80));
        }
    }
}
