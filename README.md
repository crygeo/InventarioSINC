# Reestructuración del Modelo de Datos

Esta rama contiene una **reestructuración profunda** del modelo de datos del sistema, con el objetivo de mejorar la escalabilidad, eficiencia y reutilización de componentes tanto en el servidor como en el cliente.

---

## 🔄 Objetivos de la reestructuración

- Eliminar redundancia en las interfaces (`IAtributo`, `IValor`, etc.)
- Usar un modelo **jerárquico genérico** (`IElementoJerarquico`) para representar atributos, valores y relaciones dinámicas
- Optimizar las entidades que contienen grandes colecciones (`Identificador`, `RecepcionCarga`, etc.)
- Separar responsabilidades entre entidades de configuración y datos operativos
- Reducir la sobrecarga en el tráfico de red (especialmente en SignalR)

---

## 🧩 Cambios principales

### ✅ `IElementoJerarquico`

Modelo genérico para representar identificadores, atributos y valores relacionados entre sí.

```csharp
public interface IElementoJerarquico : IModelObj
{
    string IdPerteneciente { get; set; }
    string Name { get; set; }
    string? Descripcion { get; set; }
}
```
## ✅ RecepcionCarga
Simplificación de referencias y almacenamiento de datos de forma desacoplada.

```csharp
public interface IRecepcionCarga : IModelObj
{
    IEnumerable<IElementoJerarquico> Identificadores { get; set; }
    string IdProveedor { get; set; }
    DateTime FechaIngreso { get; set; }
    IEnumerable<ICarga> Camiones { get; set; }
    float PesoTotal { get; set; }
    byte[]? GuiaGlobal { get; set; }
    string Nota { get; set; }
}
```
## ✅ Separación de valores (Valor)
Los valores ya no están embebidos en las entidades (Identificador, Atributo, etc.), sino que se gestionan en una colección aparte para mejorar la eficiencia.
---
## 🧠 Consideraciones
Este diseño es compatible con MongoDB y aprovecha su naturaleza no relacional.

El cliente necesitará realizar consultas adicionales para obtener los valores relacionados (lazy-load).

Las notificaciones por SignalR deben optimizarse para enviar solo lo necesario (no el objeto completo).
---
## 🛠️ Próximos pasos
Completar migración de modelos concretos (class)

Implementar servicios REST separados para ElementoJerarquico y Valor

Ajustar vistas en el cliente para usar la nueva jerarquía dinámica

Documentar relaciones y estructura final del esquema
---
## 📂 Rama actual
Esta es una rama de trabajo: los cambios aún no están en producción.
Una vez finalizada y testeada, será fusionada con main o develop.

## 📅 Fecha de inicio
Junio 2025


---

## 🧑‍💻 Autor

Desarrollado por un programador fullstack .NET, apasionado por las buenas prácticas, arquitectura limpia y sistemas mantenibles.
