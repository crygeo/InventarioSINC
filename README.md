# InventarioSINC

Sistema de gestión de inventario modular y extensible, desarrollado con arquitectura **cliente-servidor desacoplada**, utilizando tecnologías modernas como **WPF (MVVM)** en el cliente y **ASP.NET Core + MongoDB** en el servidor.

---

## 🎯 Objetivo

El objetivo principal de este proyecto es construir una plataforma dinámica y mantenible para gestionar productos, atributos personalizados, variantes, usuarios y permisos, con la posibilidad de extenderlo fácilmente a nuevas entidades y funcionalidades.

---

## 🧱 Arquitectura

### 🧩 Cliente (WPF)
- **MVVM puro** con `CommunityToolkit.Mvvm`
- UI completamente desacoplada, reutilizable y extensible
- Sistema de formularios dinámicos (`FormularioDinamico`) generado en tiempo de ejecución a partir de atributos personalizados
- Componentes desacoplados como `AtributesAdd`, que permiten gestionar estructuras como `IAtributosEntity` y `IAtributo` dinámicamente
- `DialogService` centralizado con soporte para:
  - Diálogos principales y subdiálogos (`DialogHost`)
  - Formularios reutilizables y modales
  - Comandos asincrónicos (`IAsyncRelayCommand`) como propiedades de dependencia

### 🌐 Servidor (ASP.NET Core)
- API RESTful genérica basada en controladores abstractos
- Uso de **MongoDB** como base de datos principal, utilizando acceso tipado (`IMongoCollection<T>`)
- Repositorios genéricos (`RepositorioBase<T>`) con CRUD completo
- Servicios de dominio (`ServiceBase<T>`) con lógica adicional y notificaciones (`HubService`)
- Verificación de permisos por roles y acciones (`VerificarPermiso`)
- Respuestas estandarizadas con objetos como `ErrorResponse`

---

## ⚙️ Principios y prácticas

- ✅ **Inyección de responsabilidades**: los controladores no conocen detalles del repositorio
- ✅ **Inversión de dependencias** mediante interfaces como `IRepository<T>` y `IService<T>`
- ✅ **Programación declarativa** en el cliente, aprovechando atributos como `[Solicitar]` o `[NombreEntidad]`
- ✅ **Estilo fluido y seguro**: uso extensivo de `async/await`, validaciones y control de errores

---

## 🧠 Componentes destacados

### 🧾 `FormularioDinamico`
Formulario generador que construye su UI a partir de atributos decorados con `[Solicitar]`, para cualquier entidad del sistema. Permite alta reutilización de vistas y mantiene coherencia visual.

### 🧩 `AtributesAdd`
Control visual personalizado para manejar una estructura `IEnumerable<IAtributosEntity>` que internamente contiene `IEnumerable<IAtributo>`. Soporta:
- Agregado de atributos y valores
- Edición y eliminación mediante `ContextMenu` con `MaterialDesignThemes`
- Encapsulamiento de comandos como propiedades de dependencia

### 💬 `DialogService`
Servicio centralizado para abrir diálogos y subdiálogos con soporte para:
- Identificadores únicos
- Formularios dinámicos
- Progreso, confirmaciones y notificaciones

---

## 📦 Proyecto modular

- **Shared**: define modelos base, interfaces (`IModelObj`, `ISelectable`, `IAtributo`, etc.)
- **Servidor**: gestiona lógica de negocio, persistencia y endpoints REST
- **Cliente**: provee una UI robusta y flexible con WPF

---
# 🧩 Arquitectura del Proyecto: Separación Cliente/Servidor basada en Interfaces Compartidas

Este proyecto adopta una arquitectura **cliente/servidor desacoplada** que utiliza **interfaces compartidas** para garantizar la mantenibilidad, escalabilidad y reutilización del código. Se basa en una **estructura modular** que separa claramente las responsabilidades de cada capa.

---

## 📁 Estructura del Proyecto

| Capa     | Propósito                                    | Detalles técnicos |
|----------|----------------------------------------------|-------------------|
| **Cliente** | Interfaz de usuario (WPF + MVVM)              | Modelos enriquecidos con `ModelBase<T>`, soporte a `INotifyPropertyChanged`, validación con `INotifyDataErrorInfo` y `SetProperty` para bindings. |
| **Servidor** | API REST (ASP.NET Core + MongoDB)             | Clases simples (POCOs) con `[BsonId]`, sin lógica de UI, optimizadas para serialización JSON y persistencia MongoDB. |
| **Shared** | Contratos comunes reutilizables                 | Interfaces como `IModelObj`, `IProducto`, `IAtributosEntity`, etc., compartidas entre cliente y servidor. |

---

## 🎯 Objetivo de la Separación

La separación por capas permite:

- ✅ Evitar la duplicación de código.
- ✅ Respetar el principio de responsabilidad única (SRP).
- ✅ Reutilizar contratos compartidos sin acoplar las implementaciones.
- ✅ Simplificar validaciones, bindings y serialización/deserialización según el entorno.

---

## 🧠 Cliente (WPF)

Los modelos del cliente extienden `ModelBase<T>` y se benefician de:

- 🎯 **Cambio de estado reactivo**: `INotifyPropertyChanged`.
- 🛡️ **Validación dinámica**: `INotifyDataErrorInfo`.
- 🔄 **Soporte total a formularios**: Bindings de WPF y método `SetProperty`.

Esto permite una UI rica y validaciones contextuales directamente en el cliente.

---

## 🛠️ Servidor (ASP.NET Core)

Los modelos del servidor son simples objetos CLR (`POCOs`) diseñados para:

- 📦 **Serialización eficiente** con `System.Text.Json`.
- 🧱 **Persistencia directa** en MongoDB usando `IMongoCollection<T>`.
- 🧼 **Operaciones CRUD limpias** sin acoplar lógica de presentación.

---

## 🔄 Ventaja del Uso de Interfaces

El uso de interfaces como `IEnumerable<IAtributosEntity>` permite:

- 🔁 Intercambio flexible de modelos entre capas.
- 🧩 Implementaciones separadas con contratos comunes.
- 🧽 Limpieza de dependencias UI en el servidor.
- 🔐 Tipado fuerte sin sacrificar flexibilidad.

---

## ✅ Beneficios Generales

- ♻️ **Reutilización** de código y contratos.
- 🔌 **Desacoplamiento** entre cliente y servidor.
- 📐 **Escalabilidad** para nuevos módulos o cambios de tecnología.
- 📦 **Mantenibilidad** a largo plazo del proyecto.

---

## 📚 Ejemplo de Contrato Compartido

```csharp
public interface IProducto : IModelObj {
    string Nombre { get; set; }
    decimal Precio { get; set; }
    IEnumerable<IAtributosEntity> Atributos { get; set; }
}
```
## 🧪 Ejemplo Cliente (WPF)

```csharp
public class ProductoViewModel : ModelBase<ProductoViewModel>, IProducto {
    private string _nombre;
    public string Nombre {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    // Otros miembros...
}
```

## 🗄️ Ejemplo Servidor (ASP.NET Core)
```csharp
public class ProductoEntity : IProducto {
    [BsonId]
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public List<AtributoEntity> Atributos { get; set; }
}
```

📌 Esta arquitectura sigue los principios de Clean Architecture, promoviendo separación de responsabilidades y contratos independientes reutilizables entre frontend y backend.
---
## 🔐 Autenticación y autorización

Este proyecto implementa autenticación basada en **JWT (JSON Web Tokens)**. Todas las rutas protegidas requieren que el usuario esté autenticado y tenga los permisos adecuados.

### 🪪 Flujo de login (desde el cliente)

1. El usuario realiza login vía `POST /api/Auth/Login` con `user` y `password`.
2. El servidor valida credenciales y roles.
3. Si el usuario tiene permiso (`Auth.Login`), se genera y retorna un JWT.
4. El token se guarda en almacenamiento aislado (`IsolatedStorage`) en el cliente.
5. Todas las peticiones HTTP del cliente incluyen el header:


---

## 🧪 Pruebas en Postman

Para consumir los endpoints protegidos desde Postman:

1. **Login para obtener token**

```http
POST /api/Auth/Login
Content-Type: application/json

{
  "user": "admin",
  "password": "admin"
}
```
---
## 📌 Notas técnicas

- `Clone()` es utilizado para evitar edición directa hasta que se confirme
- Se emplea `BindingProxy` para exponer `UserControl` como contexto a menús contextuales
- Todo CRUD es auditado visualmente desde cliente y validado desde servidor
- La arquitectura permite escalar hacia nuevas entidades solo creando:
  1. Modelo (implementando `IModelObj`)
  2. Repositorio
  3. Servicio
  4. Controlador

---

## 🚀 Futuro

- Soporte para auditorías
- Historial de cambios por entidad
- Integración con SignalR para notificaciones en tiempo real
- Módulo de autenticación JWT o IdentityServer

---

## 🧑‍💻 Autor

Desarrollado por un programador fullstack .NET, apasionado por las buenas prácticas, arquitectura limpia y sistemas mantenibles.
