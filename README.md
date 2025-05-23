# Proyecto Final DAM - Aplicación de Seguimiento Fitness

Este es el Trabajo de Fin de Grado de Desarrollo de Aplicaciones Multiplataforma (DAM) de **Adrián Párraga Sáez**.  
Una aplicación multiplataforma desarrollada con **.NET MAUI**, orientada al **seguimiento de actividad física, alimentación y progreso personal**.

---

## Tecnologías utilizadas

- [.NET MAUI](https://learn.microsoft.com/dotnet/maui/) - Framework multiplataforma para crear apps nativas
- **MongoDB** - Base de datos NoSQL para el almacenamiento de usuarios, sesiones, rutinas, etc.
- **MVVM (Model-View-ViewModel)** - Arquitectura que separa la lógica de negocio de la interfaz
- **Microcharts** - Para visualización de gráficos y estadísticas
- **C#** y **XAML** como lenguajes principales

## Funcionalidades principales

- Registro y autenticación de usuarios  
- Gestión de rutinas de ejercicio  
- Control de consumo alimenticio diario  
- Seguimiento del progreso físico  
- Estadísticas mediante gráficos  
- Interfaz adaptada a roles (usuario/admin) usando navegación con Shell para separar flujos  
- Uso intensivo de programación asíncrona para operaciones de red y base de datos, garantizando respuesta fluida en la interfaz
- Soporte básico de accesibilidad: cambio de tema claro/oscuro, teclas de acceso rapido y compatibilidad inicial con lectores de pantalla


## Autor

**Adrián Párraga Sáez**  
Trabajo de Fin de Grado – Desarrollo de Aplicaciones Multiplataforma (DAM)

## Limitación conocida
Actualmente, la generación del instalador (.msix) para Windows no está disponible debido a una incompatibilidad con los paquetes Microsoft.NETCore.App.Runtime.Mono.win-x86 en la versión preview de .NET 9.0 utilizada por MAUI en este proyecto.

Esta es una limitación conocida en el ecosistema .NET MAUI y no tiene una solución directa en el alcance del TFG.

A pesar de esta limitación, la aplicación funciona correctamente cuando se ejecuta desde Visual Studio.
Migrar el proyecto a .NET 8 habría implicado rehacer y volver a probar gran parte del código, incluyendo dependencias y configuración.
Dado que .NET 9 está prácticamente completo y mi proyecto corre sin problemas en el entorno de desarrollo, opté por entregar una solución funcional, aunque sin empaquetar como instalador (.msix).
