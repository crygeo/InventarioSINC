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
