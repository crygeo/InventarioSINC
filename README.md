
# 🧾 Reestructuración del Modelo de Datos — InventarioSINC

**📅 Fecha:** Octubre 2025  
**🚀 Versión:** v2.0.0 — Reestructuración completa del modelo de datos y arquitectura modular

---

## 📘 Resumen

**InventarioSINC** es un sistema modular de gestión de inventario basado en una arquitectura **cliente-servidor desacoplada**.  
El cliente está desarrollado en **WPF (MVVM)** y el servidor en **ASP.NET Core** con **MongoDB**.  
Esta versión incluye una **reestructuración profunda del modelo de datos** para mejorar la **escalabilidad**, **eficiencia** y **reutilización de componentes**, tanto en el servidor como en el cliente.

---

## 🎯 Objetivos

- Eliminar redundancia en las interfaces (`IAtributo`, `IValor`, etc.).  
- Implementar un modelo **jerárquico genérico** (`IElementoJerarquico`) para representar atributos, valores y relaciones dinámicas.  
- Optimizar entidades que contienen grandes colecciones (`Identificador`, `RecepcionCarga`, etc.).  
- Separar responsabilidades entre entidades de configuración y datos operativos.  
- Reducir la sobrecarga en el tráfico de red (especialmente en SignalR).  

---

## 🧱 Arquitectura (Resumen)

| Capa | Descripción | Tecnologías |
|------|--------------|--------------|
| **Cliente** | UI desacoplada basada en WPF (MVVM) y CommunityToolkit.Mvvm. Formularios generados dinámicamente a partir de metadatos. | WPF, C# |
| **Servidor** | API REST genérica con repositorios y servicios abstractos. | ASP.NET Core, MongoDB |
| **Compartido** | Define contratos, modelos base e interfaces reutilizables. | .NET Standard |

---

## ⚙️ Cambios principales y motivos

### 🧩 Modelo jerárquico genérico (`IElementoJerarquico`)
**Motivo:** Unificar la representación de elementos que antes se modelaban como distintas entidades (atributos, valores, identificadores, etc.), evitando duplicación.  
**Beneficio:** Simplifica la creación y extensión de relaciones dinámicas entre entidades.

```csharp
public interface IElementoJerarquico : IModelObj
{
    string IdPerteneciente { get; set; }
    string Nombre { get; set; }
    string? Descripcion { get; set; }
}
```

---

### 📦 Reestructuración de `RecepcionCarga`
**Motivo:** Reducir el tamaño y complejidad de entidades con grandes colecciones y referencias.  
**Enfoque:** Usar referencias a `IElementoJerarquico` para identificadores y valores, separando datos pesados como `byte[]` (guías) en entidades independientes.

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

---

### 🔄 Separación de valores (`Valor`)
**Motivo:** Los valores ya no están embebidos dentro de las entidades (por ejemplo, `Identificador` o `Atributo`).  
**Beneficio:** Reduce duplicación y tamaño de documentos, mejora consultas y velocidad de serialización.

---

### ⚡ Optimización de notificaciones (SignalR)
**Motivo:** Evitar enviar objetos completos en actualizaciones en tiempo real.  
**Enfoque:** Enviar únicamente los identificadores y los cambios mínimos requeridos; el cliente realiza *lazy loading* cuando es necesario.

---

## 🌐 API y Servicios

- Nuevos endpoints REST para `ElementoJerarquico` y `Valor`.  
- Repositorios y servicios genéricos reutilizables (`RepositorioBase<T>`, `ServiceBase<T>`).  
- Controladores desacoplados mediante inyección de dependencias.  
- Autenticación basada en JWT con validación de roles y permisos.  

---

## 💻 Impacto en Cliente y Servidor

### 🧩 Cliente (WPF)
- Formularios y vistas adaptados al nuevo modelo jerárquico (`IElementoJerarquico`).  
- Implementación de carga diferida (*lazy loading*) para valores relacionados.  
- Actualización de controles dinámicos (`AtributesAdd`, `FormularioDinamico`) para usar las nuevas APIs.  
- Mantiene compatibilidad con `ModelBase<T>`, `INotifyPropertyChanged` y validaciones en tiempo real.

### 🛠️ Servidor (ASP.NET Core + MongoDB)
- Nuevos modelos/colecciones (`ElementoJerarquico`, `Valor`).  
- CRUD desacoplado y persistencia eficiente con `IMongoCollection<T>`.  
- Notificaciones SignalR más ligeras y orientadas a eventos.  
- Separación entre datos operativos y de configuración.

---

## 🔁 Migración y pasos recomendados

1. Definir y documentar el nuevo esquema para:
   - `IElementoJerarquico`
   - `IRecepcionCarga`
   - `Valor`
2. Crear servicios y repositorios REST para las nuevas colecciones.  
3. Migrar datos embebidos a colecciones separadas (`Valor`).  
4. Actualizar las vistas del cliente para usar carga diferida.  
5. Validar SignalR con mensajes reducidos y selectivos.  
6. Ejecutar pruebas unitarias, de integración y de rendimiento.  
7. Fusionar con `main` o `develop` tras validación completa.

---

## 🧠 Consideraciones de diseño

- **Compatibilidad con MongoDB:** se aprovecha su naturaleza no relacional; las referencias son preferibles en colecciones grandes.  
- **Rendimiento:** separar valores reduce E/S y peso de transferencia, aunque incrementa lecturas diferidas.  
- **Consistencia:** si se requiere atomicidad, considerar operaciones compensatorias o transacciones.  
- **Auditoría:** mantener el patrón `Clone()` para ediciones locales seguras antes de confirmar cambios.

---

## 🧪 Pruebas y consumo de API

- Se recomienda usar **Postman** para probar autenticación y endpoints CRUD.  
- Implementar pruebas automatizadas (unitarias e integración).  
- Verificar rendimiento en operaciones de escritura y consultas masivas.

---

## 🧰 Próximos pasos

- Completar migración de modelos concretos.  
- Ajustar vistas para jerarquía dinámica.  
- Documentar relaciones entre entidades.  
- Implementar endpoints REST para `ElementoJerarquico` y `Valor`.  
- Optimizar notificaciones y pruebas de carga.  

---

## 📂 Estado de la rama

- **Tipo:** Rama de trabajo (no en producción).  
- **Fecha de inicio:** Junio 2025.  
- Una vez validada la migración, se fusionará con `main` o `develop`.

---

## 🤝 Contribuciones

- Crea una rama por cada conjunto de cambios.  
- Describe el impacto en PRs y actualiza documentación de interfaces compartidas.  
- Mantén consistencia con las políticas de licencia del proyecto.  

---

## ⚠️ Breaking Changes

- Se modificaron **namespaces**, **contratos** y **rutas**.  
- Los proyectos dependientes deben actualizar referencias y bindings.  

---

## 📜 Licencia y notas finales

Esta reestructuración prioriza la **escalabilidad**, la **limpieza de contratos** y la **eficiencia en las comunicaciones** para soportar el crecimiento del sistema.
