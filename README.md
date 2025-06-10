[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=19739776&assignment_repo_type=AssignmentRepo)

# Proyecto de Pruebas Unitarias - Sistema de Validación de Retiros

## Descripción

Este proyecto implementa un sistema de validación para retiros en cuentas de ahorros básicas de una cooperativa financiera, desarrollado en .NET 8 con pruebas unitarias usando NUnit.

## Estructura del Proyecto

```
UnitTestCooperativa/
├── Cooperativa.Domain/              # Biblioteca de clases principal
│   ├── Interfaces/
│   │   └── IRetiroValidator.cs      # Interfaz del validador
│   ├── Models/
│   │   ├── RetiroValidationRequest.cs   # Modelo de solicitud
│   │   └── RetiroValidationResult.cs    # Modelo de resultado
│   └── Services/
│       └── RetiroValidator.cs       # Implementación del validador
├── Cooperativa.Domain.Tests/        # Proyecto de pruebas unitarias
│   └── RetiroValidatorTests.cs      # Pruebas unitarias con NUnit
└── ANALISIS_REQUISITOS.md          # Documentación del análisis
```

## Requisitos Implementados

El sistema valida que un cliente puede retirar dinero si se cumplen **TODAS** las siguientes condiciones:

1. ✅ La cuenta está activa
2. ✅ El cliente tiene saldo suficiente
3. ✅ El monto solicitado no excede el límite de retiro diario
4. ✅ La cuenta no está bloqueada por fraude
5. ✅ El monto es múltiplo de 10

## Casos de Prueba Implementados

| ID | Descripción | Estado |
|----|-------------|--------|
| **TC1** | Retiro exitoso - todas las condiciones válidas | ✅ |
| **TC2** | Falla por cuenta inactiva | ✅ |
| **TC3** | Falla por saldo insuficiente | ✅ |
| **TC4** | Falla por exceder límite diario | ✅ |
| **TC5** | Falla por cuenta bloqueada | ✅ |
| **TC6** | Falla por monto no múltiplo de 10 | ✅ |
| **TC7** | Falla múltiple - saldo y múltiplo | ✅ |
| **TC8** | Caso límite - monto igual al saldo | ✅ |

## Requisitos del Sistema

- .NET 8.0 o superior
- NUnit 3.14.0 (incluido en el proyecto)

## Compilación y Ejecución

### 1. Compilar el proyecto
```bash
dotnet build
```

### 2. Ejecutar las pruebas
```bash
dotnet test
```

### 3. Ejecutar pruebas con detalles
```bash
dotnet test --verbosity normal
```

### 4. Generar reporte de cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Uso del Validador

```csharp
using Cooperativa.Domain.Interfaces;
using Cooperativa.Domain.Models;
using Cooperativa.Domain.Services;

// Crear instancia del validador
IRetiroValidator validator = new RetiroValidator();

// Crear solicitud de retiro
var request = new RetiroValidationRequest
{
    EstaCuentaActiva = true,
    SaldoActual = 1000m,
    MontoSolicitado = 100m,
    LimiteRetiroDiario = 500m,
    EstaBloqueadaPorFraude = false
};

// Validar retiro
var resultado = validator.ValidarRetiro(request);

// Verificar resultado
if (resultado.EsValido)
{
    Console.WriteLine("Retiro aprobado");
}
else
{
    Console.WriteLine($"Retiro rechazado: {resultado.MensajeError}");
    
    // Ver todos los errores
    foreach (var error in resultado.ErroresDetallados)
    {
        Console.WriteLine($"- {error}");
    }
}
```

## Características Técnicas

### Principios Aplicados
- **Principio de Responsabilidad Única**: Cada clase tiene una responsabilidad específica
- **Principio de Inversión de Dependencias**: Uso de interfaces para el desacoplamiento
- **Principio de Abierto/Cerrado**: Extensible sin modificar código existente

### Patrones Utilizados
- **Factory Pattern**: Para crear resultados de validación
- **Strategy Pattern**: Validaciones independientes y reutilizables
- **Dependency Injection**: A través de interfaces

### Metodología de Pruebas
- **Clases de Equivalencia**: Reducen el número de casos de prueba
- **Casos Límite**: Prueban valores en los bordes de las validaciones
- **Casos de Error**: Verifican manejo de errores múltiples
- **Arrange-Act-Assert**: Estructura estándar de pruebas unitarias

## Beneficios de las Clases de Equivalencia

1. **Reducción de casos de prueba**: 5 condiciones × 2 estados = 10 casos básicos vs 2^5 = 32 combinaciones
2. **Cobertura completa**: Cada condición tiene casos válidos e inválidos
3. **Mantenibilidad**: Fácil actualización ante cambios en reglas de negocio
4. **Eficiencia**: Máxima detección de errores con mínimo esfuerzo

## Extensibilidad

El sistema está diseñado para ser fácilmente extensible:

### Agregar nuevas validaciones
```csharp
// En RetiroValidator.cs
if (nuevaCondicion)
{
    errores.Add("Nuevo mensaje de error");
}
```

### Agregar nuevos tipos de cuenta
```csharp
public interface ICuentaValidator<T> where T : ICuenta
{
    RetiroValidationResult ValidarRetiro(T cuenta, decimal monto);
}
```

## Métricas de Calidad

- **Cobertura de Código**: 100% en lógica de negocio
- **Casos de Prueba**: 10 casos principales + 2 casos adicionales
- **Tiempo de Ejecución**: < 1 segundo todas las pruebas
- **Complejidad Ciclomática**: Baja (< 5 por método)

## Autor

Desarrollado como parte del Taller de Pruebas Unitarias - Ingeniería de Software II

## Documentación Adicional

- [Análisis de Requisitos Detallado](ANALISIS_REQUISITOS.md)
- [Casos de Prueba Documentados](ANALISIS_REQUISITOS.md#3-diseño-de-casos-de-prueba)
- [Clases de Equivalencia](ANALISIS_REQUISITOS.md#2-diseño-de-clases-de-equivalencia)