# Análisis de Requisitos y Diseño de Pruebas
## Sistema de Validación para Retiros en Cuentas de Ahorros Básicas

### 1. Análisis del Requisito

**Requisito funcional:**
"Un cliente puede retirar dinero de su cuenta de ahorros si se cumplen todas las siguientes condiciones:
- La cuenta está activa.
- El cliente tiene saldo suficiente.
- El monto solicitado no excede el límite de retiro diario.
- La cuenta no está bloqueada por fraude.
- El monto es múltiplo de 10."

#### 1.1 Identificación de Condiciones Lógicas

1. **Condición 1 (C1):** La cuenta está activa
2. **Condición 2 (C2):** El cliente tiene saldo suficiente
3. **Condición 3 (C3):** El monto solicitado no excede el límite de retiro diario
4. **Condición 4 (C4):** La cuenta no está bloqueada por fraude
5. **Condición 5 (C5):** El monto es múltiplo de 10

#### 1.2 Reformulación como Validaciones Independientes

- **V1:** Validar estado de cuenta (activa/inactiva)
- **V2:** Validar saldo disponible vs monto solicitado
- **V3:** Validar límite de retiro diario vs monto solicitado
- **V4:** Validar estado de bloqueo por fraude
- **V5:** Validar que el monto sea múltiplo de 10

### 2. Diseño de Clases de Equivalencia

| Condición | Clases Válidas | Clases Inválidas | Justificación |
|-----------|----------------|------------------|---------------|
| **C1: Estado de cuenta** | CV1: Cuenta activa | CI1: Cuenta inactiva | Reduce pruebas binarias (activa/inactiva) |
| **C2: Saldo suficiente** | CV2: Saldo ≥ monto solicitado | CI2: Saldo < monto solicitado | Simplifica validación de saldo |
| **C3: Límite diario** | CV3: Monto ≤ límite diario | CI3: Monto > límite diario | Enfoca en límites de negocio |
| **C4: Bloqueo por fraude** | CV4: Cuenta no bloqueada | CI4: Cuenta bloqueada por fraude | Validación de seguridad binaria |
| **C5: Múltiplo de 10** | CV5: Monto múltiplo de 10 | CI5: Monto no múltiplo de 10 | Regla de negocio específica |

### 3. Diseño de Casos de Prueba

| ID | Descripción | C1 | C2 | C3 | C4 | C5 | Entrada | Resultado Esperado |
|----|-------------|----|----|----|----|----|---------|--------------------|
| **TC1** | Retiro exitoso - todas las condiciones válidas | CV1 | CV2 | CV3 | CV4 | CV5 | Cuenta activa, saldo $1000, monto $100, límite $500, no bloqueada | Éxito |
| **TC2** | Falla por cuenta inactiva | CI1 | CV2 | CV3 | CV4 | CV5 | Cuenta inactiva, saldo $1000, monto $100, límite $500, no bloqueada | Falla: "Cuenta inactiva" |
| **TC3** | Falla por saldo insuficiente | CV1 | CI2 | CV3 | CV4 | CV5 | Cuenta activa, saldo $50, monto $100, límite $500, no bloqueada | Falla: "Saldo insuficiente" |
| **TC4** | Falla por exceder límite diario | CV1 | CV2 | CI3 | CV4 | CV5 | Cuenta activa, saldo $1000, monto $600, límite $500, no bloqueada | Falla: "Excede límite diario" |
| **TC5** | Falla por cuenta bloqueada | CV1 | CV2 | CV3 | CI4 | CV5 | Cuenta activa, saldo $1000, monto $100, límite $500, bloqueada | Falla: "Cuenta bloqueada" |
| **TC6** | Falla por monto no múltiplo de 10 | CV1 | CV2 | CV3 | CV4 | CI5 | Cuenta activa, saldo $1000, monto $105, límite $500, no bloqueada | Falla: "Monto debe ser múltiplo de 10" |
| **TC7** | Falla múltiple - saldo y múltiplo | CV1 | CI2 | CV3 | CV4 | CI5 | Cuenta activa, saldo $50, monto $75, límite $500, no bloqueada | Falla: Primera condición que falle |
| **TC8** | Caso límite - monto igual al saldo | CV1 | CV2 | CV3 | CV4 | CV5 | Cuenta activa, saldo $200, monto $200, límite $500, no bloqueada | Éxito |

### 4. Definición de la Firma del Método

```csharp
public class RetiroValidationRequest
{
    public bool EstaCuentaActiva { get; set; }
    public decimal SaldoActual { get; set; }
    public decimal MontoSolicitado { get; set; }
    public decimal LimiteRetiroDiario { get; set; }
    public bool EstaBloqueadaPorFraude { get; set; }
}

public class RetiroValidationResult
{
    public bool EsValido { get; set; }
    public string MensajeError { get; set; }
    public List<string> ErroresDetallados { get; set; }
}

public interface IRetiroValidator
{
    RetiroValidationResult ValidarRetiro(RetiroValidationRequest request);
}
```

### 5. Diseño de Pruebas Automatizadas

#### Métodos de Prueba Planificados:

```csharp
[TestFixture]
public class RetiroValidatorTests
{
    [Test]
    public void TC1_RetiroExitoso_TodasLasCondicionesValidas()
    // Verifica que un retiro válido sea aprobado

    [Test]
    public void TC2_FallaPorCuentaInactiva()
    // Verifica rechazo cuando la cuenta está inactiva

    [Test]
    public void TC3_FallaPorSaldoInsuficiente()
    // Verifica rechazo cuando el saldo es menor al monto

    [Test]
    public void TC4_FallaPorExcederLimiteDiario()
    // Verifica rechazo cuando se excede el límite diario

    [Test]
    public void TC5_FallaPorCuentaBloqueada()
    // Verifica rechazo cuando la cuenta está bloqueada

    [Test]
    public void TC6_FallaPorMontoNoMultiploDe10()
    // Verifica rechazo cuando el monto no es múltiplo de 10

    [Test]
    public void TC7_FallaMultiple_SaldoYMultiplo()
    // Verifica comportamiento con múltiples errores

    [Test]
    public void TC8_CasoLimite_MontoIgualAlSaldo()
    // Verifica caso límite donde monto = saldo
}
```

### 6. Beneficios de las Clases de Equivalencia

1. **Reducción de casos de prueba:** En lugar de probar todos los valores posibles, se prueban representantes de cada clase.
2. **Cobertura completa:** Cada condición de negocio tiene representación válida e inválida.
3. **Mantenibilidad:** Cambios en reglas de negocio requieren actualizaciones mínimas en las pruebas.
4. **Eficiencia:** Se maximiza la detección de errores con el mínimo número de pruebas.

### 7. Estrategia de Implementación

1. **Fase 1:** Implementar las clases del dominio (CuentaAhorros, RetiroValidator)
2. **Fase 2:** Crear las pruebas automatizadas siguiendo los casos definidos
3. **Fase 3:** Implementar la lógica de validación
4. **Fase 4:** Ejecutar pruebas y refinar implementación
