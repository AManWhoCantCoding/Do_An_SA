# BÁO CÁO LUẬN THIẾT KẾ KIẾN TRÚC PHẦN MỀM
## MediSphere — Hospital Management System

**Môn học:** Thiết kế Kiến trúc Phần mềm  
**Tài liệu tham khảo:** *Fundamentals of Software Architecture* — Mark Richards & Neal Ford  
**Công nghệ:** ASP.NET Core 6.0, Entity Framework Core, SQL Server  
**Ngày:** 06/2026

---

## Mục lục

1. [Introduction](#1-introduction)
2. [System Requirements](#2-system-requirements)
3. [Architecture Selection](#3-architecture-selection)
4. [Architecture Design](#4-architecture-design)
5. [Technical Design](#5-technical-design)
6. [Implementation](#6-implementation)
7. [Evaluation](#7-evaluation)
8. [Conclusion](#8-conclusion)

---

## 1. Introduction

### 1.1 Giới thiệu hệ thống

**MediSphere** là hệ thống quản lý bệnh viện (Hospital Management System) hỗ trợ:

- Quản lý hồ sơ bệnh nhân
- Đặt lịch hẹn khám bệnh
- Quản lý đơn thuốc (Prescriptions)
- Tạo và xuất báo cáo y tế
- Quản trị nhân viên và phân quyền

Hệ thống phục vụ bệnh viện quy mô vừa và nhỏ, triển khai dạng **monolithic web application** với giao diện Razor Pages và **REST API** cho tích hợp bên thứ ba.

### 1.2 Mục tiêu

| Mục tiêu | Mô tả |
|---|---|
| Nghiệp vụ | Tự động hóa quy trình hành chính y tế |
| Kỹ thuật | Áp dụng Layered Architecture, Repository Pattern, REST API |
| Bảo mật | ASP.NET Identity, JWT, phân quyền theo vai trò |
| Chất lượng | Logging, health checks, unit tests, CI/CD |

---

## 2. System Requirements

### 2.1 Business Context

Bệnh viện cần một hệ thống tập trung thay thế quy trình giấy tờ, giảm sai sót và tăng hiệu quả phối hợp giữa bác sĩ, y tá và quản trị viên.

### 2.2 Stakeholders

| Stakeholder | Vai trò | Nhu cầu chính |
|---|---|---|
| **Administrator** | Quản trị hệ thống | Quản lý user, phân quyền, khóa tài khoản |
| **Staff (Bác sĩ/Y tá)** | Nhân viên y tế | Quản lý bệnh nhân, lịch hẹn, đơn thuốc, báo cáo |
| **Bệnh nhân** | Đối tượng được phục vụ | Hồ sơ chính xác, lịch hẹn đúng giờ (gián tiếp) |
| **IT Department** | Vận hành kỹ thuật | Triển khai, monitoring, backup, bảo mật |

### 2.3 Functional Requirements (FR)

| ID | Yêu cầu | Trạng thái |
|---|---|---|
| FR-01 | Đăng nhập / đăng xuất / quên mật khẩu | ✅ |
| FR-02 | Phân quyền Administrator và Staff | ✅ |
| FR-03 | CRUD bệnh nhân | ✅ |
| FR-04 | CRUD lịch hẹn + lịch FullCalendar | ✅ |
| FR-05 | CRUD đơn thuốc | ✅ |
| FR-06 | Tạo báo cáo, template, xuất Excel | ✅ |
| FR-07 | Quản trị nhân viên (Admin) | ✅ |
| FR-08 | REST API cho tất cả module chính | ✅ |
| FR-09 | JWT authentication cho API | ✅ |
| FR-10 | Tìm kiếm bệnh nhân (client-side) | ✅ |

### 2.4 Non-Functional Requirements (NFR)

| Thuộc tính | Yêu cầu | Giải pháp trong MediSphere |
|---|---|---|
| **Scalability** | Hỗ trợ tăng tải theo thời gian | Layered monolith; có thể tách API/Microservices sau; Docker-ready |
| **Performance** | Phản hồi < 3s cho thao tác thường | EF Core + SQL Server; Repository pattern; logging đo thời gian request |
| **Availability** | Uptime cao trong giờ làm việc | Health checks `/health`, `/health/ready`; CI/CD tự động build |
| **Security** | Bảo vệ dữ liệu y tế | Identity, roles, lockout, JWT, HTTPS, `[Authorize]` |
| **Maintainability** | Dễ bảo trì, mở rộng | Tách Models/Repositories/Services/API; unit tests; migrations |

---

## 3. Architecture Selection

### 3.1 Các lựa chọn kiến trúc

| Kiến trúc | Ưu điểm | Nhược điểm |
|---|---|---|
| **Layered (N-tier)** | Đơn giản, dễ hiểu, phù hợp team nhỏ | Khó scale độc lập từng layer |
| **Microservices** | Scale độc lập, deploy riêng | Phức tạp, cần DevOps mạnh |
| **Event-driven** | Loose coupling, async | Overkill cho quy mô hiện tại |
| **Services-based** | Tách service theo domain | Cần service mesh, API gateway |

### 3.2 Quyết định: Layered Architecture + REST API

**Lý do chọn:**

1. Phù hợp quy mô bệnh viện vừa/nhỏ và team phát triển nhỏ
2. ASP.NET Core Razor Pages + Web API trong cùng project — triển khai đơn giản
3. Repository Pattern tách biệt data access — dễ test và bảo trì
4. Có thể evolve sang Microservices khi cần scale

### 3.3 Trade-offs

| Trade-off | Quyết định | Hệ quả |
|---|---|---|
| Monolith vs Microservices | Monolith | Deploy nhanh; scale vertical trước |
| SSR vs SPA | Razor Pages + jQuery | SEO tốt, dev nhanh; UX kém hơn SPA |
| Cookie vs JWT | Cả hai | Cookie cho web UI; JWT cho API clients |
| SQL Server vs NoSQL | SQL Server | ACID, quan hệ phức tạp; scale horizontal khó hơn |

---

## 4. Architecture Design

### 4.1 C4 Model — Context Diagram

```mermaid
C4Context
    title System Context - MediSphere
    Person(staff, "Hospital Staff", "Manages patients and appointments")
    Person(admin, "Administrator", "Manages users and settings")
    System(docdocgo, "MediSphere", "Hospital Management System")
    System_Ext(email, "SMTP Server", "Sends notification emails")
    SystemDb(db, "SQL Server", "Stores all application data")

    Rel(staff, docdocgo, "Uses web UI and API")
    Rel(admin, docdocgo, "Administers system")
    Rel(docdocgo, db, "Reads/Writes")
    Rel(docdocgo, email, "Sends emails")
```

### 4.2 C4 Model — Container Diagram

```mermaid
C4Container
    title Container Diagram - MediSphere
    Person(user, "User", "Staff or Admin")
    Container(web, "Razor Pages UI", "ASP.NET Core", "Server-side rendered web interface")
    Container(api, "REST API", "ASP.NET Core Web API", "JSON endpoints with JWT")
    Container(app, "Application Core", ".NET 6", "Business logic, repositories, services")
    ContainerDb(db, "SQL Server", "Relational DB", "Patients, Appointments, Reports, Identity")

    Rel(user, web, "HTTPS")
    Rel(user, api, "HTTPS + JWT")
    Rel(web, app, "In-process")
    Rel(api, app, "In-process")
    Rel(app, db, "EF Core")
```

### 4.3 Component Diagram

```mermaid
flowchart TB
    subgraph Presentation
        RP[Razor Pages]
        API[API Controllers]
        SW[Swagger UI]
    end
    subgraph Business
        VM[ViewModels / DTOs]
        SVC[EmailSender Service]
        ID[ASP.NET Identity]
    end
    subgraph DataAccess
        REPO[Repositories]
        CTX[ApplicationDBContext]
    end
    subgraph Data
        SQL[(SQL Server)]
    end

    RP --> REPO
    API --> REPO
    RP --> ID
    API --> ID
    RP --> SVC
    REPO --> CTX
    CTX --> SQL
```

### 4.4 Data Flow Diagram — Tạo lịch hẹn

```mermaid
sequenceDiagram
    participant U as User
    participant P as Razor Page / API
    participant R as AppointmentRepository
    participant DB as SQL Server

    U->>P: POST appointment data
    P->>P: Validate + Authorize
    P->>R: CreateAsync(model)
    R->>DB: INSERT
    DB-->>R: OK
    R-->>P: AppointmentModel
    P-->>U: Redirect / 201 Created
```

### 4.5 Domain Partitioning

| Domain | Components |
|---|---|
| **Identity** | UserModel, Account pages, AuthController |
| **Patient** | PatientModel, PatientRepository, Patient pages, PatientsController |
| **Appointment** | AppointmentModel, AppointmentRepository, Appointments pages, AppointmentsController |
| **Prescription** | PrescriptionModel, PrescriptionRepository, Prescription pages, PrescriptionsController |
| **Reporting** | ReportModel, ReportRepository, Reports pages, ReportsController |

---

## 5. Technical Design

### 5.1 REST API Design

**Base URL:** `/api`  
**Authentication:** `POST /api/auth/login` → JWT Bearer token  
**Documentation:** Swagger UI tại `/api/docs` (Development)

| Method | Endpoint | Mô tả |
|---|---|---|
| POST | `/api/auth/login` | Đăng nhập, nhận JWT |
| GET | `/api/patients` | Danh sách bệnh nhân |
| GET | `/api/patients/{id}` | Chi tiết bệnh nhân |
| POST | `/api/patients` | Tạo bệnh nhân |
| PUT | `/api/patients/{id}` | Cập nhật |
| DELETE | `/api/patients/{id}` | Xóa (Admin only) |
| GET/POST/PUT/DELETE | `/api/appointments` | CRUD lịch hẹn |
| GET/POST/PUT/DELETE | `/api/prescriptions` | CRUD đơn thuốc |
| GET | `/api/reports` | Danh sách báo cáo |

### 5.2 Data Model (ER Overview)

```
User (Identity) ── Staff accounts
Patient ──< Appointment
Patient ──< Prescription
Patient ──< Report
ReportType ──< Report
```

**Entities:** UserModel, PatientModel, AppointmentModel, PrescriptionModel, ReportModel, ReportTypeModel

### 5.3 Integration

| Integration | Công nghệ |
|---|---|
| Email | SMTP (EmailSender) |
| Excel Export | ClosedXML |
| Calendar UI | FullCalendar (CDN) |
| API Clients | REST + JWT |

### 5.4 Architectural Concepts Applied

| Concept | Application |
|---|---|
| **Modularity** | Tách folder Models, Repositories, Api, Pages, Services |
| **Coupling** | Repository interface giảm coupling PageModel ↔ DbContext |
| **Cohesion** | Mỗi repository phục vụ một entity |
| **Data Consistency** | EF Core transactions, SQL Server ACID |

---

## 6. Implementation

### 6.1 Công nghệ sử dụng

| Layer | Technology |
|---|---|
| Frontend | Razor Pages, Bootstrap 5, jQuery, FullCalendar |
| Backend | ASP.NET Core 6.0, Web API |
| ORM | Entity Framework Core 6 |
| Database | SQL Server |
| Auth | ASP.NET Identity + JWT Bearer |
| Logging | Serilog (console + file) |
| Monitoring | Health Checks, Request Logging Middleware |
| Testing | xUnit, EF InMemory |
| DevOps | GitHub Actions CI/CD, Docker, docker-compose |

### 6.2 Cấu trúc project

```
MediSphere/
├── Api/Controllers/       # REST API
├── Dto/                   # Data Transfer Objects
├── DAL/                   # DbContext
├── Models/                # Entities
├── Repositories/          # Data access
├── Services/              # Email, etc.
├── Pages/                 # Razor UI
├── Middleware/            # Request logging
├── MediSphere.Tests/        # Unit tests
├── docs/SA_REPORT.md      # Báo cáo này
├── Dockerfile
└── docker-compose.yml
```

### 6.3 Demo

1. Chạy ứng dụng: `dotnet run`
2. Web UI: `https://localhost:7170`
3. API docs: `https://localhost:7170/api/docs`
4. Health: `https://localhost:7170/health`
5. Login API:
   ```json
   POST /api/auth/login
   { "email": "pavel.sanjah-staff@hospitaltrust.com", "password": "Password123-_" }
   ```

### 6.4 CI/CD & Cloud Readiness

- **GitHub Actions:** build, test, Docker image
- **Docker:** containerized deployment
- **docker-compose:** SQL Server + App stack
- **Azure-ready:** có thể deploy container lên Azure App Service / ACI

---

## 7. Evaluation

### 7.1 Điểm mạnh kiến trúc

- Kiến trúc phân tầng rõ ràng, dễ onboard developer mới
- Dual interface: Web UI + REST API
- Security đa lớp (Identity, roles, JWT, HTTPS)
- Observability cơ bản (Serilog, health checks, request timing)
- Testable qua Repository + unit tests

### 7.2 Điểm yếu và rủi ro

| Rủi ro | Mức độ | Giảm thiểu |
|---|---|---|
| Monolith scaling | Trung bình | Docker, vertical scale, tách API sau |
| Single DB bottleneck | Trung bình | Indexing, read replicas khi cần |
| No distributed tracing | Thấp | Thêm Application Insights / OpenTelemetry |
| Secret in appsettings | Cao (prod) | Azure Key Vault / env variables |

### 7.3 Quality Attributes Assessment

| Attribute | Score (1-5) | Ghi chú |
|---|---|---|
| Scalability | 3 | Monolith; API-ready for split |
| Performance | 4 | Đủ cho quy mô vừa |
| Availability | 3 | Health checks; chưa HA cluster |
| Security | 4 | Identity + JWT + roles |
| Maintainability | 4 | Clean structure, tests |

### 7.4 Evolution khi Scale

1. **Giai đoạn 1 (hiện tại):** Monolith + SQL Server
2. **Giai đoạn 2:** Tách read API, cache Redis
3. **Giai đoạn 3:** Microservices theo domain (Patient, Appointment, Report)
4. **Giai đoạn 4:** Event-driven cho notifications (RabbitMQ/Azure Service Bus)

---

## 8. Conclusion

### 8.1 Tổng kết

MediSphere đáp ứng yêu cầu bài luận SA bằng cách:

- Chọn domain cụ thể (quản lý bệnh viện)
- Phân tích đầy đủ FR/NFR và stakeholders
- Lựa chọn Layered Architecture với trade-off analysis
- Thiết kế sơ đồ C4, component, data flow
- Thiết kế REST API với Swagger
- Triển khai code hoàn chỉnh với tests, logging, CI/CD, Docker

### 8.2 Hướng phát triển

- Mobile app qua REST API
- Azure deployment với Application Insights
- Microservices decomposition
- HL7/FHIR integration cho chuẩn y tế quốc tế

---

## Phụ lục: Rubrics Self-Assessment

| Tiêu chí | Điểm tối đa | Tự đánh giá |
|---|---|---|
| Phân tích yêu cầu | 15 | 14 |
| Lựa chọn kiến trúc | 15 | 14 |
| Thiết kế kiến trúc | 20 | 17 |
| Áp dụng kiến thức sách | 15 | 14 |
| Implementation | 10 | 9 |
| Đánh giá & trade-offs | 10 | 9 |
| Báo cáo & trình bày | 10 | 8 |
| Sáng tạo & mở rộng | 5 | 4 |
| **Bonus (CI/CD, Docker, Logging)** | +10 | +6 |
| **Tổng ước lượng** | **110** | **~95** |

---

*Báo cáo này có thể export sang PDF để nộp bài. Source code: repository MediSphere.*
