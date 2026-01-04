🎬 MovieApp – ASP.NET MVC (.NET)
Aplicación web desarrollada con ASP.NET MVC. Permite gestionar un catálogo de películas mediante operaciones CRUD, con foco en buenas prácticas de arquitectura MVC, validaciones y una interfaz clara.
Este proyecto fue desarrollado con el objetivo de demostrar conocimientos en C#, ASP.NET MVC, Entity Framework y SQL Server, y forma parte de mi portfolio como desarrollador .NET.
---

🚀 Funcionalidades principales
•	🎬 Gestión completa de películas (CRUD)
Alta, edición, eliminación y visualización de películas, con relaciones a géneros y plataformas.
•	🏷 Administración de entidades auxiliares (CRUD)
Gestión de géneros, plataformas y usuarios, accesible únicamente para usuarios con rol Administrador.
•	👤 Autenticación y autorización de usuarios
Sistema de login y registro implementado con ASP.NET Identity, incluyendo manejo de roles.
•	🛡 Validaciones y seguridad
Validaciones de datos en formularios y control de acceso basado en roles utilizando Identity.
•	⭐ Sistema de reviews y favoritos
Los usuarios pueden:
o	Ver sus reviews realizadas
o	Administrar su lista de películas favoritas
o	Acceder a vistas personalizadas según su cuenta
•	📋 Listados dinámicos y vistas personalizadas
Diferentes vistas según el tipo de usuario (administrador / usuario estándar).
•	🎨 Interfaz web basada en MVC
Arquitectura ASP.NET MVC, Razor Views y separación clara de responsabilidades.
---

🛠 Tecnologías utilizadas
•	Lenguaje: C#
•	Framework: ASP.NET MVC (.NET)
•	ORM: Entity Framework
•	Base de datos: SQL Server
•	Frontend: Razor Views, HTML, CSS, Bootstrap
•	Control de versiones: Git / GitHub
---

⚙️ Instalación y ejecución local
Requisitos previos
•	.NET SDK instalado
•	SQL Server (LocalDB o instancia local)
•	Visual Studio / VS Code
📦 Paquetes NuGet necesarios
Este proyecto utiliza los siguientes paquetes NuGet:
•	MailKit
•	Microsoft.AspNetCore.Identity.EntityFrameworkCore
•	Microsoft.EntityFrameworkCore
•	Microsoft.EntityFrameworkCore.Design
•	Microsoft.EntityFrameworkCore.SqlServer
•	Microsoft.EntityFrameworkCore.Tools
•	Microsoft.VisualStudio.Web.CodeGeneration.Design
•	OpenAI
•	SixLabors.ImageSharp
Al clonar el repositorio, estos paquetes se restauran automáticamente ejecutando dotnet restore.
🔧 Configuración requerida
Es necesario completar la cadena de conexión MovieDbContext en el archivo appsettings.json, apuntando a una instancia válida de SQL Server.
Ejemplo:
"ConnectionStrings": {
  "MovieDbContext": "Server=(localdb)\\mssqllocaldb;Database=MovieAppDb;Trusted_Connection=True;"
}
Pasos
1.	Clonar el repositorio:
git clone https://github.com/francowagenhofer/movieapp-mvc-dotnet.git
2.	Abrir la solución en Visual Studio
3.	Restaurar dependencias: dotnet restore
4.	Configurar la cadena de conexión en appsettings.json
5.	Ejecutar migraciones (si aplica):
dotnet ef database update
6.	Ejecutar la aplicación:
dotnet run
7.	Abrir en el navegador:
https://localhost:xxxx
---


👨‍💻 Autor
Franco Wagenhöfer
Desarrollador .NET
•	GitHub: https://github.com/francowagenhofer
•	LinkedIn: (agregar link)
___

📄 Licencia
Este proyecto se distribuye bajo la licencia MIT.
