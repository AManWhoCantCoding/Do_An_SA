<h1 align="center">MediSphere</h1>
<a id="top"></a>

<br />
<div align="center">
  <img src="wwwroot/resources/logo-two.png" alt="MediSphere Logo">
</div>

MediSphere is a Hospital Management System — a modern web application for hospital administration, patient care workflows, and staff management. Previously developed as **DocDocGo**, the project has been rebranded and fully redesigned with a clean, professional UI.

**Vietnamese project documentation:** [TAI_LIEU_DU_AN_MediSphere.txt](TAI_LIEU_DU_AN_MediSphere.txt)

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#introduction">Introduction</a></li>
    <li><a href="#features">Features</a></li>
    <li><a href="#ui-design">UI Design</a></li>
    <li><a href="#software-architecture">Software Architecture</a></li>
    <li><a href="#technologies-used">Tech Stack</a></li>
    <li><a href="#unused-files">Unused / Redundant Files</a></li>
    <li><a href="#installation">Installation</a></li>
    <li><a href="#api-usage">API Usage</a></li>
    <li><a href="#docker">Docker</a></li>
    <li><a href="#running-tests">Running Tests</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## Introduction

MediSphere was developed as a university assignment to demonstrate software engineering practices and the Software Development Life Cycle (SDLC).

The system manages patient records, appointments, prescriptions, medical staff, and reports — via a redesigned Razor Pages UI and a REST API with JWT authentication.

## Features

### Patient Management
- Register and manage patient information.
- Assign unique patient IDs for easy identification.
- Track appointments, personal information, and reports associated with each patient.

### Appointment Scheduling
- Book appointments with patients online.
- Calendar view powered by FullCalendar.
- Reschedule and cancel appointments.

### Prescription Management
- Create, view, update, and delete prescriptions linked to patients.
- Available via web UI and REST API.

### Staff Management
- Maintain a database of doctors, nurses, and other personnel.
- Track contact information, roles, and account settings via the administrative portal.

### Administrative Security
- User management with ASP.NET Core Identity.
- Reassign roles, lock accounts, two-factor authentication, and password recovery.

### Reporting and Exporting
- Generate customizable reports from patient data.
- Save report templates for reuse.
- Export to `.xlsx` format (ClosedXML).

### REST API
- Full REST API with JWT authentication under `/api/*`.
- Swagger documentation at `/api/docs` (Development mode).
- Health checks at `/health` and `/health/ready`.
- Web UI modules call the API via `api-client.js` (`MediSphereApi`).

## UI Design

The interface was rebuilt from scratch (replacing the Bootswatch *Morph* neumorphic theme):

| Aspect | New design |
|---|---|
| Layout | Fixed dark sidebar + sticky top bar |
| Typography | Plus Jakarta Sans |
| Colors | Teal/slate medical palette |
| Components | Card-based pages, modern tables, stat cards on Dashboard |
| Auth | Centered card with tab navigation |

**Widget names preserved:** Patients, Add A Patient, Appointments, Prescriptions, Add Prescription, Reports, Report Types, Generate A Report, Administrator Settings, Dashboard.

## Software Architecture

- **Report:** [docs/SA_REPORT.md](docs/SA_REPORT.md)
- **Architecture:** Layered (N-tier) monolith + REST API
- **Patterns:** Repository pattern, DTOs, dual auth (Cookie + JWT)
- **DevOps:** GitHub Actions CI/CD, Docker, docker-compose
- **Quality:** Unit tests (xUnit), Serilog, health checks

## Technologies Used

| Layer | Technologies |
|---|---|
| Frontend | HTML, CSS, JavaScript, jQuery, Razor Pages, Bootstrap 5, FullCalendar |
| Backend | ASP.NET Core 6.0, Web API, ASP.NET Core Identity |
| API | JWT Bearer, Swagger/OpenAPI, DTOs |
| Database | SQL Server, Entity Framework Core |
| DevOps | GitHub Actions, Docker, Serilog, Health Checks, xUnit |
| Libraries | ClosedXML, SendGrid |

## Unused / Redundant Files

These items are **not used** by the running application:

| Item | Reason |
|---|---|
| `Models/RolesModel.cs` | Excluded in `.csproj`; file does not exist (Identity roles used instead) |
| `Models/UserRolesModel.cs` | Excluded in `.csproj`; file does not exist |
| `Repositories/Interfaces/IPatientRepository.cs` | Excluded in `.csproj`; file does not exist (`IRepository<T>` used) |
| `Repositories/Interfaces/IPrescriptionRepository.cs` | Excluded in `.csproj`; file does not exist |
| `Repositories/Interfaces/IReportRepository.cs` | Excluded in `.csproj`; file does not exist |
| `Pages/Account/Pages/**` | Excluded in `.csproj`; scaffold folder never created |
| `itext7` NuGet package | Removed — no PDF generation code in the project |
| Old `siteTheme.css` (Bootswatch Morph, ~334 KB) | Replaced by custom MediSphere theme (~15 KB) |

> **Note:** `Compile Remove` entries in `MediSphere.csproj` can be deleted safely — they reference files that no longer exist on disk.

## Installation

1. Clone this repository:

```sh
git clone https://github.com/Wraami/MediSphere.git
cd MediSphere
```

2. Restore and build:

```sh
dotnet restore MediSphere.sln
dotnet build MediSphere.sln
```

3. Set up the database (see below).

### Restore the Database

Connect to `(localdb)\MSSQLLocalDB` or your SQL Server instance in SSMS.

Restore `Database-Copy/DocDocGoDB.bak` (original backup filename). On restore, set the database name to **MediSphereDB**, or keep **DocDocGoDB** and update `HospitalManagementSQLConnection` in `appsettings.json`.

![SSMS Starting Screenshot](Instruction-images/startingConnection.png)
![SSMS Context Menu Screenshot](Instruction-images/restoreDatabase.png)
![SSMS Selection Screenshot](Instruction-images/restoreDatabaseSelection.png)
![SSMS Final Dialog Screenshot](Instruction-images/finalDatabaseRestore.png)

### Run Locally

```sh
dotnet run --project MediSphere.csproj
```

Default URLs: https://localhost:7170 · http://localhost:5144

## API Usage

```http
POST /api/auth/login
Content-Type: application/json

{ "email": "pavel.sanjah-staff@hospitaltrust.com", "password": "Password123-_" }
```

```http
GET /api/patients
Authorization: Bearer <your-token>
```

Swagger UI: `/api/docs` (Development)

| Resource | Route |
|---|---|
| Auth | `/api/auth` |
| Patients | `/api/patients` |
| Appointments | `/api/appointments` |
| Prescriptions | `/api/prescriptions` |
| Reports | `/api/reports` |

## Docker

```sh
docker-compose up --build
```

- Application: http://localhost:8080
- SQL Server: localhost:1433

## Running Tests

```sh
dotnet test MediSphere.sln
```

## Usage

### Administrator

```
Email: sarah-admin@hospitaltrust.com
Password: Password123-_
```

### Staff member

```
Email: pavel.sanjah-staff@hospitaltrust.com
Password: Password123-_
```

## License

MediSphere is open-source software licensed under the [MIT License](LICENSE).

<p align="right">(<a href="#top">back to top</a>)</p>
