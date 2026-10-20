# AI Prompts Log

This project was developed with the help of an AI coding assistant, as allowed by the challenge rules.
This file lists the prompts that shaped the design and the code, in chronological order. Logistics, tool setup and Git housekeeping questions were left out.

- **Tool:** Claude Code (desktop app), model Claude Opus 5.5
- **How it was used:** planning, design review, diagrams, SQL scripts, the Figma mockup, guided implementation, deployment guidance, the bonus features and Git operations.
  I wrote the backend domain layer and the first part of the frontend following guides; the assistant completed the remaining frontend steps and the bonus features. Every change was reviewed, built and tested before committing.
- **Language:** prompts are kept in their original language (Spanish). 
Each entry includes a short English summary of the result.
  Screenshots or files attached to a prompt are described in brackets.

## 1. Design (Day 1)

**1.1**
> El enunciado distingue dos roles: el usuario crea el ticket y el operador gestiona sus cambios de estado. Propongo separar la aplicación en dos vistas, una para cada rol. ¿Es el enfoque adecuado?

*Result:* Two roles and two views (report page for users, board for operators) with simulated roles instead of authentication; added `reported_by` to the data model.

**1.2**
> Quiero aplicar buenas prácticas de diseño. Cuento con la documentación de un curso de .NET basado en Domain-Driven Design, aunque no sé si es demasiado para el alcance de este reto. ¿Es recomendable aplicar ese enfoque, organizando el proyecto en una carpeta para el backend y otra para el frontend?

*Result:* Decision to use layered architecture with a rich domain model (lightweight DDD) instead of plain MVC, in a single repository.

**1.3**
> [Attached my course reference document for .NET with DDD]
> Esta es la documentación del curso; tómala como referencia para la arquitectura del backend.

*Result:* Reviewed the course conventions (bounded contexts, aggregates, value objects, command/query services, Result pattern, assemblers) and adapted them: Dapper and stored procedures instead of EF Core, no localization. The reference document is not included in this repository.

**1.4**
> Genera los diagramas de secuencia de los dos flujos principales. Para el ERD, ¿qué herramienta recomiendas? Normalmente uso Redgate Data Modeler; ¿sería una buena opción?

*Result:* Generated the two PlantUML sequence diagrams (create ticket, change status). Redgate Data Modeler confirmed for the ERD.

**1.5**
> Genera el script del esquema de la base de datos para importarlo en Redgate y obtener el ERD.

*Result:* Generated `database/01_schema.sql` (tables, foreign key, CHECK constraints, indexes) to import into Redgate.

## 2. Repository and database (Day 2)

**2.1**
> Antes de hacer el push y crear el pull request, muéstrame los cambios que se van a subir para revisarlos.

*Result:* Preview of commits, files and PR description; secrets check.

**2.2**
> Escribe los stored procedures y compártelos conmigo para revisarlos.

*Result:* Generated `02_stored_procedures.sql` (transactions, rollback, optimistic concurrency with error 50409) and `03_seed.sql`.

**2.3**
> [Screenshot of a MySQL Workbench version warning]
> ¿A qué se debe esta advertencia?

*Result:* Explained the Workbench 8.0 / MySQL 8.4 compatibility warning.

**2.4**
> [Screenshot of the MySQL Workbench output with error 1267 when running the stored procedures]
> ¿Cómo se soluciona este error?

*Result:* Collation conflict between tables and procedure parameters; tables changed to `utf8mb4_0900_ai_ci`.

## 3. Backend: domain layer

**3.1**
> Detalla paso a paso la implementación de la capa de dominio, incluyendo el código.

*Result:* Detailed guide with code: Result pattern, value objects, state machine, Ticket aggregate, repository contract and unit tests.

**3.2**
> ¿Es correcto que la máquina de estados quede directamente dentro de `Model`, o debería ubicarse en una carpeta propia?

*Result:* State machine moved to a `Policies` folder.

**3.3**
> Mueve `Repositories` y `Exceptions` a la raíz de `Domain`, siguiendo la estructura de la plantilla del curso.

*Result:* `Repositories` and `Exceptions` moved to the domain root, matching the course structure.

## 4. Backend: ticket creation and status workflow

**4.1**
> Prepara la guía detallada de la rama `feature/ticket-creation`, con el código de cada paso. Los paquetes NuGet los agregaré desde Rider.

*Result:* Detailed guide with code for the Dapper repository, application services, REST controller, user secrets and Swagger.

**4.2**
> [Rider build output with errors CS1061: AddOpenApi and MapOpenApi not found in Program.cs]
> ¿Cómo se soluciona este error de compilación?

*Result:* The OpenApi package was removed before replacing Program.cs; replacing Program.cs with the Swagger-based version from the guide fixes the build.

**4.3**
> El proyecto ya compila, pero solo hay tres endpoints. ¿Cómo se realiza el cambio de estado del ticket? Verifica que la implementación coincida con los diagramas de secuencia.

*Result:* Code reviewed against both sequence diagrams: the create flow matches; the status change endpoint was planned for the next branch (`feature/ticket-status-workflow`), and the repository side of that flow was already implemented.

**4.4**
> Prepara la guía con el código para el cambio de estado y la consulta del historial del ticket.

*Result:* Step-by-step guide with code for the status change and history use cases: ChangeTicketStatusError, command and query handlers, PATCH /tickets/{id}/status and GET /tickets/{id}/history endpoints, and the sequence diagram update for the concurrency error.

**4.5**
> Ejecuta las pruebas de los endpoints de la API.

*Result:* Ran the API test cases from the guide (create, query, status changes, history and error responses) against the local API and MySQL.

## 5. UI mockup (Figma)

**5.1**
> Crea el mockup en Figma. Dado que el documento presenta el diseño solo como referencia, ¿qué propuesta sugieres? Acordamos dos vistas, una para el usuario y otra para el operador, por lo que serían dos pantallas. Créalo en el archivo `maintenance-tickets-mockup`, dentro de la carpeta `maintenance-tickets` de mi equipo en Figma.

*Result:* Design proposal based on the reference prototype: two screens, a report form for the user and a status board with a detail panel for the operator.

**5.2**
> [Link to the Figma file: https://www.figma.com/design/yfST2frwJIU2kBTWH70BxW/maintenance-tickets-mockup]
> La propuesta está bien. Aplica heurísticas de usabilidad y buenas prácticas de diseño.

*Result:* Created the mockup in Figma (design tokens, components, User and Operator views) applying usability heuristics: visible system status, only the allowed actions on each ticket, error prevention and accessible contrast.

## 6. Frontend (Angular)

**6.1**
> [Attached my course reference document for the Angular frontend structure]
> El mockup ya está terminado. Antes de pasar al despliegue, quiero completar el frontend. Esta es la estructura que aprendí en el curso; al igual que con el backend, indícame si es adecuada para este proyecto.

*Result:* Reviewed the course structure (bounded context with domain, infrastructure, application and presentation layers) and confirmed it matches the backend organization. The reference document is not included in this repository.

**6.2**
> Prepara la guía de implementación desde la creación del proyecto Angular.

*Result:* Step-by-step guide from the creation of the Angular project: Angular Material and design tokens, shared layer, domain entities, one endpoint class per operation with assemblers, `TicketsApi` facade, signals store, components and views.

**6.3**
> Elimina también la clase `BaseEntity` de shared, ya que no aporta nada.

*Result:* Removed `BaseEntity`; entities keep their own fields.

**6.4**
> Corrige los defectos encontrados durante las pruebas.

*Result:* Fixed the report form showing validation errors after a successful submit, and a stale comment validation that blocked the "Start work" action.

## 7. Deployment and release

**7.1**
> El pull request ya está fusionado. Prepara la guía para desplegar la aplicación en Railway.

*Result:* Deployment guide: Dockerfiles for the API (ASP.NET runtime) and the frontend (Nginx), a MySQL service on Railway, environment variables and CORS.

## 8. Bonus features

**8.1**
> Crea una nueva rama a partir de `develop` e implementa las funcionalidades bonus que no alteren la lógica de la aplicación: la notificación simulada y los filtros por estado y fecha. Sigue la misma estructura del resto del proyecto.

*Result:* Implemented the board filters (search, status and creation date) and a simulated status change notification: an `ITicketNotificationService` outbound contract with a logging implementation in the API, and a notification center in the Angular header. Tested locally with the API and MySQL.

## 9. Documentation

**9.1**
> Continúa con el README. En lugar del diagrama de arquitectura actual, ¿no sería mejor un diagrama C4 de contexto? Agrega también los diagramas de clases.

*Result:* Replaced the text architecture sketch with C4 context and container diagrams, added three PlantUML class diagrams (domain, backend layers, frontend) and updated the status change sequence diagram with the notification step.

**9.2**
> Escribe el modelo C4 en DSL para compilarlo en Structurizr.

*Result:* C4 model written in Structurizr DSL with the system context and container views.
