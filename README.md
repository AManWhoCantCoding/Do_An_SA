<h1 align="center">MediSphere</h1>
<a id="top"></a>

<br />
<div align="center">
  <img src="wwwroot/resources/logo-two.svg" alt="MediSphere Logo">
</div>

MediSphere is a Hospital Management System — a modern web application for hospital administration, patient care workflows, and staff management. Previously developed as **DocDocGo**, the project has been rebranded and fully redesigned with a clean, professional UI.

**Vietnamese project documentation:** [TAI_LIEU_DU_AN_MediSphere.txt](TAI_LIEU_DU_AN_MediSphere.txt)

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#introduction">Introduction</a></li>
    <li><a href="#features">Features</a></li>
    <li><a href="#ui-design">UI Design</a></li>
    <li><a href="#logo-images">Logo Images</a></li>
    <li><a href="#software-architecture">Software Architecture</a></li>
    <li><a href="#technologies-used">Tech Stack</a></li>
    <li><a href="#email-smtp">Email (SMTP)</a></li>
    <li><a href="#layered-architecture">Layered Architecture</a></li>
    <li><a href="#installation">Installation</a></li>
    <li><a href="#api-usage">API Usage</a></li>
    <li><a href="#docker">Docker</a></li>
    <li><a href="#running-tests">Running Tests</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#sa-requirements-evaluation">SA Requirements Evaluation</a></li>
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

### Authentication & Registration
- Public login at `/Account/Login` and staff self-registration at `/Account/Register` (auth card with tab navigation).
- Administrators can also register staff from **Administrator Settings** (`/Administrator/Register`, admin-only).
- Password recovery, email confirmation, and two-factor authentication via ASP.NET Core Identity.

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

### Recent UI updates (Jun 2026)

- Status dropdowns use **1-based** sequential numbering (e.g. `1 - Created`, `1 - Scheduled`) instead of 100-based values.
- Removed **(API)** labels from modal titles and submit buttons (the UI still calls the REST API via `api-client.js`).
- Fixed appointment **date/time picker** controls: Bootstrap 5 compatibility for increment/decrement buttons, correct script load order, and modal z-index styling.

## Logo Images

Place branding assets in `wwwroot/resources/`:

| File | Purpose | Format |
|---|---|---|
| `logo-two.svg` | Sidebar and public pages | SVG (recommended) or PNG |
| `main-logo.svg` | Browser tab favicon | SVG or ICO/PNG (32×32) |
| `hero-placeholder.svg` | Landing page hero (optional) | SVG, JPG, or PNG |

**To replace logos:**

1. Copy your image into `wwwroot/resources/` (keep the same filename, or rename and update references).
2. **Sidebar:** `Pages/Shared/_DashboardLayout.cshtml` — `src="~/resources/logo-two.svg"`
3. **Favicon:** `_DashboardLayout.cshtml` and `Pages/Shared/_Layout.cshtml` — `href="~/resources/main-logo.svg"`
4. **Public layout:** `Pages/Shared/_Layout.cshtml` — `logo-two.svg`
5. Hard-refresh the browser (Ctrl+F5) after replacing files.

If you use PNG instead of SVG, update the file extension in the `.cshtml` references above.

## Software Architecture

- **Report:** [docs/SA_REPORT.md](docs/SA_REPORT.md)
- **Architecture:** 5-layer monolith (Presentation → Business → Services → Persistence → Database) + REST API
- **Patterns:** Business layer, service layer, repository pattern, DTOs, dual auth (Cookie + JWT)
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
| Libraries | ClosedXML, System.Net.Mail (SMTP) |

## Email (SMTP)

Identity flows (confirm email, forgot password, account notifications) use `Services/EmailSender.cs` via `IEmailSender`.

Configure `SmtpSettings` in `appsettings.json`:

```json
"SmtpSettings": {
  "SmtpServer": "smtp.example.com",
  "SmtpPort": 587,
  "SmtpUsername": "your-user",
  "SmtpPassword": "your-password",
  "FromEmail": "noreply@hospitaltrust.com",
  "FromDisplayName": "MediSphere"
}
```

For local testing, create a free inbox at [Ethereal Email](https://ethereal.email/create) and paste the generated SMTP credentials. Sent messages appear in the Ethereal web inbox (not a real mailbox).

## Layered Architecture

MediSphere follows a **5-layer** architecture. Requests flow top-down; each layer only talks to the layer directly below it.

```
Presentation → Business → Services → Persistence → Database
```

| Layer | Folder | Responsibility |
|---|---|---|
| **Presentation** | `Pages/`, `Api/`, `Dto/`, `ViewModels/`, `wwwroot/` | Razor UI, REST controllers, DTOs, client JS |
| **Business** | `Business/` | Validation, business rules, DTO mapping (`*Business`) |
| **Services** | `Services/` | Application services orchestrating persistence (`*Service`, `EmailSender`, `NotificationService`) |
| **Persistence** | `Persistence/` | Repository pattern — CRUD over EF Core (`*Repository`) |
| **Database** | `Database/`, `Migrations/` | Entities (`Database/Models/`), `ApplicationDBContext`, SQL Server schema |

**New services added:**
- `PatientService`, `AppointmentService`, `PrescriptionService`, `ReportService`, `ReportTypeService`
- `DashboardService` — aggregate counts for the Dashboard
- `NotificationService` — appointment/report email notifications via SMTP
- `EmailSender` — ASP.NET Identity email flows

DI registration: `DependencyInjection/ServiceCollectionExtensions.cs` → `AddMediSphereLayers()` (called from `Program.cs`).

## Architecture Notes

- API controllers inject `*Business` interfaces (not repositories directly).
- Report Razor pages use `IReportBusiness`; Dashboard uses `IDashboardBusiness`.
- Patient/Appointment/Prescription list pages still call the REST API via `api-client.js` (Presentation → API → Business).

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

3. Configure `ConnectionStrings:HospitalManagementSQLConnection` and `SmtpSettings` in `appsettings.json` (or `appsettings.Development.json` for local overrides).

4. Set up the database (see below).

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

### Registration

1. Open `/Account/Login` and click the **Register** tab (or go directly to `/Account/Register`).
2. Complete the form and accept the terms. New accounts receive the **Staff** role.
3. Confirm email if required by Identity settings, then sign in.

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

## SA Requirements Evaluation

Full evaluation (Vietnamese): [docs/SA_EVALUATION.md](docs/SA_EVALUATION.md) — rubric from `SA requirements.docx` at repo root. **Estimated score: ~97/110** (updated 09/06/2026).

Architecture report: [docs/SA_REPORT.md](docs/SA_REPORT.md).

| Category | Status | Notes |
|---|---|---|
| **Requirements analysis** (FR/NFR, stakeholders) | Met | FR-01–FR-10 documented; stakeholders and NFRs in SA report |
| **Architecture selection** (trade-offs) | Met | Layered monolith + REST API with documented trade-offs |
| **Architecture design** (C4, components, data flow) | Met | Context/container/component diagrams in SA report |
| **Technical design** (API, security, data model) | Met | JWT + Cookie auth, EF Core, Identity, Swagger |
| **Implementation** (working code, layers) | Met | 5-layer structure; build succeeds; 4 unit tests pass |
| **Evaluation & evolution** | Partial | Quality attributes scored; HA/microservices left as future work |
| **DevOps & quality** (CI/CD, Docker, logging) | Met | GitHub Actions, Docker, Serilog, health checks |
| **Documentation & presentation** | Met | README, Vietnamese docs, SA report |

**Functional highlights:** patient/appointment/prescription/report CRUD, admin portal, REST API with JWT, Excel export, FullCalendar scheduling.

**Gaps / recommendations:** upgrade from .NET 6 (EOL); expand test coverage beyond repositories; consider approval workflow for public staff registration in production.

## License

MediSphere is open-source software licensed under the [MIT License](LICENSE).

<p align="right">(<a href="#top">back to top</a>)</p>
