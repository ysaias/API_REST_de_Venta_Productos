# Proyecto Backend - API RESTful

## Descripción
Este proyecto es un backend desarrollado con el patrón MVC, que ofrece una API RESTful para gestionar entidades clave de un sistema de comercio electrónico, incluyendo:

- **Producto**: Manejo de productos disponibles para la venta.
- **Role**: Gestión de roles de usuario y permisos.
- **Categoría**: Organización de productos por categorías.
- **Cart**: Funcionalidad para crear y gestionar carritos de compra.
- **CartItem**: Manejo de items dentro de un carrito de compra.
- **Usuario**: Registro y autenticación de usuarios.

## Funcionalidades
- Implementación de endpoints para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) en las entidades mencionadas.
- Validaciones de datos para asegurar la integridad y calidad de la información.
- Soporte para relaciones entre entidades, permitiendo operaciones complejas.

## Tecnologías Utilizadas
- ASP.NET Core
- Entity Framework Core
- AutoMapper
- SQL Server (o la base de datos utilizada)

## Instalación
1. Clona el repositorio.
2. Restaura las dependencias usando `dotnet restore`.
3. Ejecuta las migraciones para configurar la base de datos con `dotnet ef database update`.
4. Inicia el servidor con `dotnet run`.

## Contribuciones
Las contribuciones son bienvenidas. Si deseas colaborar, por favor abre un *issue* o un *pull request*.

## Licencia
Este proyecto está bajo la Licencia MIT.
