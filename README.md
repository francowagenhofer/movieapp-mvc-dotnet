# MovieApp – ASP.NET MVC (.NET)

Aplicación web desarrollada con **ASP.NET MVC** que permite gestionar un catálogo de películas mediante operaciones CRUD. El proyecto demuestra buenas prácticas en arquitectura MVC, validaciones robustas, autenticación segura y una interfaz clara.

Desarrollado como proyecto portfolio para demostrar competencias en **C#**, **ASP.NET MVC**, **Entity Framework** y **SQL Server**.

---

## 🎬 Funcionalidades principales

- **Gestión completa de películas (CRUD)**  
  Alta, edición, eliminación y visualización de películas, con relaciones a géneros y plataformas.

- **Administración de entidades auxiliares**  
  Gestión de géneros, plataformas y usuarios (acceso restringido a administradores).

- **Autenticación y autorización**  
  Sistema de login y registro implementado con **ASP.NET Identity**, incluyendo control basado en roles.

- **Sistema de reviews y favoritos**  
  Los usuarios pueden ver sus reviews, gestionar películas favoritas y acceder a vistas personalizadas.

- **Validaciones y seguridad**  
  Validaciones de datos en formularios y control de acceso basado en roles.

- **Listados dinámicos y vistas personalizadas**  
  Diferentes vistas según el tipo de usuario (administrador / usuario estándar).

---

## 🛠 Tecnologías utilizadas

| Componente | Tecnología |
|-----------|-----------|
| **Lenguaje** | C# |
| **Framework** | ASP.NET MVC (.NET 9) |
| **ORM** | Entity Framework Core |
| **Base de datos** | SQL Server |
| **Frontend** | Razor Views, HTML, CSS, Bootstrap |
| **Control de versiones** | Git / GitHub |

### Paquetes NuGet

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (v9.0.11) – Autenticación y autorización
- `Microsoft.EntityFrameworkCore` (v9.0.11) – ORM y acceso a datos
- `Microsoft.EntityFrameworkCore.Design` (v9.0.11) – Herramientas de diseño
- `Microsoft.EntityFrameworkCore.SqlServer` (v9.0.11) – Proveedor SQL Server
- `Microsoft.EntityFrameworkCore.Tools` (v9.0.11) – Migraciones de base de datos
- `Microsoft.VisualStudio.Web.CodeGeneration.Design` (v9.0.11) – Generación de código scaffolding
- `MailKit` (v4.14.1) – Envío de correos
- `SixLabors.ImageSharp` (v3.1.12) – Procesamiento de imágenes
- `OpenAI` (v2.8.0) – Integración con API de IA

---

## ⚙️ Instalación y ejecución local

### Requisitos previos

- **.NET SDK 9** instalado
- **SQL Server** (LocalDB o instancia local)
- **Visual Studio 2022** o **VS Code**

### Configuración inicial

1. **Clonar el repositorio:**

````````bash
git clone https://github.com/tu-usuario/MovieApp.git

````````

2. **Restaurar dependencias:**

````````bash
cd MovieApp
dotnet restore

````````

3. **Configurar la cadena de conexión en `appsettings.json`:**

4. **Ejecutar migraciones de base de datos:**

````````bash
dotnet ef database update

````````

5. **Ejecutar la aplicación:**

````````bash
dotnet run

````````

6. **Abrir en el navegador:**

````````markdown
http://localhost:5000
````````

---

## 👨‍💻 Autor

**Franco Wagenhöfer**  
Desarrollador .NET

- **Portafolio:** [francowagenhofer.github.io](https://francowagenhofer.github.io/)
- **GitHub:** [Franco Wagenhöfer](https://github.com/francowagenhofer)
- **LinkedIn:** [Franco Wagenhöfer](https://www.linkedin.com/in/franco-wagenh%C3%B6fer/)

---

## 📄 Licencia

Este proyecto se distribuye bajo la licencia **MIT**.
