# ĐÁNH GIÁ ĐỒ ÁN MEDISPHERE THEO SA REQUIREMENTS

**Dự án:** MediSphere (Hospital Management System)  
**Môn học:** Thiết kế Kiến trúc Phần mềm (Software Architecture)  
**Tài liệu rubric:** `SA requirements.docx`  
**Ngày đánh giá:** 09/06/2026 (đánh giá lại sau cập nhật `SA_REPORT.md`)  
**Phương pháp:** Đối chiếu rubric trong đề bài với mã nguồn, báo cáo kiến trúc và kết quả build/test thực tế

---

## 1. Thông tin tài liệu yêu cầu

| Mục | Giá trị |
|---|---|
| **File rubric** | `SA requirements.docx` (thư mục gốc repo) |
| **Tham chiếu sách** | *Fundamentals of Software Architecture* — Mark Richards & Neal Ford |
| **Báo cáo dự án** | `docs/SA_REPORT.md` (đã đồng bộ kiến trúc 5 tầng, 09/06/2026) |
| **Tài liệu bổ sung** | `README.md`, `TAI_LIEU_DU_AN_MediSphere.txt` |

---

## 2. Tóm tắt đề bài (từ SA requirements.docx)

Sinh viên chọn một domain cụ thể, thực hiện đầy đủ chu trình: **phân tích yêu cầu → lựa chọn kiến trúc → thiết kế chi tiết → triển khai (implementation) → đánh giá trade-off**, trình bày báo cáo 15–35 trang theo outline 8 chương.

### 2.1. Yêu cầu chi tiết (6 nhóm)

| # | Nhóm yêu cầu | Nội dung chính |
|---|---|---|
| (1) | Phân tích hệ thống | Bài toán, stakeholders, FR, NFR (Scalability, Performance, Availability, Security, Maintainability) |
| (2) | Lựa chọn kiến trúc | Chọn style, giải thích, trade-offs, so sánh phương án |
| (3) | Thiết kế kiến trúc | C4/UML, component, data flow, API design |
| (4) | Áp dụng kỹ thuật kiến trúc | Coupling/cohesion, modularity, domain partitioning, data consistency |
| (5) | Implementation | Code chạy được, tối thiểu một phần quan trọng |
| (6) | Đánh giá & trade-off | Điểm mạnh/yếu, rủi ro, hướng scale |

### 2.2. Rubric chấm điểm (100 điểm + bonus 10)

| STT | Tiêu chí | Điểm tối đa |
|---|---|---|
| 1 | Phân tích yêu cầu | 15 |
| 2 | Lựa chọn kiến trúc | 15 |
| 3 | Thiết kế kiến trúc | 20 |
| 4 | Áp dụng kiến thức từ sách | 15 |
| 5 | Implementation | 10 |
| 6 | Đánh giá & trade-offs | 10 |
| 7 | Báo cáo & trình bày | 10 |
| 8 | Sáng tạo & mở rộng | 5 |
| **Bonus** | Cloud +3, CI/CD +3, Monitoring/logging +2, Distributed system +2 | +10 |

### 2.3. Tiêu chí đạt / không đạt

| Trạng thái | Điều kiện |
|---|---|
| **Đạt** | Kiến trúc rõ ràng + phân tích trade-offs + có implementation |
| **Không đạt** | Chỉ lý thuyết / không có kiến trúc rõ / không triển khai |

---

## 3. Xác minh kỹ thuật (09/06/2026)

| Kiểm tra | Kết quả |
|---|---|
| `dotnet build MediSphere.sln -c Release` | **Thành công** (0 lỗi, 30 cảnh báo nullable/EOL) |
| `dotnet test MediSphere.sln -c Release` | **4/4 test passed** |
| Docker | `Dockerfile`, `docker-compose.yml` có sẵn |
| CI/CD | `.github/workflows/dotnet.yml` (build + test + docker image) |
| Báo cáo ↔ codebase | `SA_REPORT.md` §4.3–4.6 khớp cấu trúc thư mục thực tế |

---

## 4. Đánh giá chi tiết theo từng tiêu chí rubric

### 4.1. Phân tích yêu cầu — **ĐẠT** (**14/15**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Mô tả bài toán & phạm vi | **Met** | `SA_REPORT.md` §1.1 — HMS monolith + REST API |
| Stakeholders | **Met** | §2.2 — Administrator, Staff, Bệnh nhân, IT Department |
| Functional requirements | **Met** | FR-01–FR-10 §2.3; triển khai trong `Pages/`, `Api/Controllers/` |
| NFR — Scalability | **Met** | §2.4, §7.3; Docker-ready, evolution path §7.4 |
| NFR — Performance | **Met** | §2.4; `Middleware/RequestLoggingMiddleware.cs` |
| NFR — Availability | **Met** | `Program.cs` — `/health`, `/health/ready` |
| NFR — Security | **Met** | Identity, roles, JWT, lockout; `[Authorize]` trên API |
| NFR — Maintainability | **Met** | 5-layer + `DependencyInjection/ServiceCollectionExtensions.cs` |

**Chức năng đã triển khai (đối chiếu FR):**

| ID | Mô tả | Bằng chứng mã nguồn |
|---|---|---|
| FR-01 | Auth + đăng ký công khai `/Account/Register` | `Pages/Account/Register.cshtml.cs` (`[AllowAnonymous]`, role Staff), `AuthController.cs` |
| FR-02 | Phân quyền Admin/Staff | `Program.cs`, `Pages/Administrator/*` |
| FR-03 | CRUD bệnh nhân | `Pages/Patient/*`, `PatientsController.cs`, `Business/PatientBusiness.cs` |
| FR-04 | CRUD lịch hẹn + FullCalendar | `Pages/Appointments/`, `wwwroot/js/appointments.js` |
| FR-05 | CRUD đơn thuốc | `Pages/Prescriptions/*`, `PrescriptionsController.cs` |
| FR-06 | Báo cáo, template, Excel | `Pages/Reports/*`, ClosedXML export |
| FR-07 | Quản trị nhân viên | `Pages/Administrator/Settings.cshtml`, Register/Lockout/Delete |
| FR-08 | REST API các module | `Api/Controllers/` — 5 controllers |
| FR-09 | JWT cho API | `AuthSchemes.JwtOrCookie`, `AddJwtBearer` |
| FR-10 | Tìm kiếm bệnh nhân | `wwwroot/js/patients.js` |

**Điểm trừ (-1):** Chưa có benchmark performance / SLA uptime đo thực nghiệm.

---

### 4.2. Lựa chọn kiến trúc — **ĐẠT** (**14/15**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Chọn architecture style | **Met** | Layered Architecture 5 tầng — `SA_REPORT.md` §3.2, §4.3 |
| So sánh nhiều phương án | **Met** | §3.1 — Layered, Microservices, Event-driven, Services-based |
| Giải thích lý do chọn | **Met** | §3.2 — quy mô bệnh viện vừa/nhỏ, team nhỏ |
| Trade-offs | **Met** | §3.3 — Monolith vs Microservices, SSR vs SPA, Cookie vs JWT |
| So sánh phương án khác | **Met** | Bảng ưu/nhược §3.1 |

**Điểm trừ (-1):** Chưa phân tích thêm Space-based, Microkernel, Pipeline (không bắt buộc).

---

### 4.3. Thiết kế kiến trúc — **ĐẠT** (**18/20**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Architecture diagram (C4) | **Met** | §4.1–4.2 — Context & Container (Mermaid) |
| Component diagram | **Met** | §4.3 — 5 tầng Presentation → Business → Services → Persistence → Database |
| Data flow diagram | **Met** | §4.4 — sequence qua Business → Service → Repository |
| Service decomposition | **Met** | §4.6 domain partitioning theo từng layer |
| Deployment diagram | **Met** | §4.5 — Dev, Docker Compose, CI, Azure-ready |
| API design (REST) | **Met** | §5.1; Swagger `/api/docs` |

**Cải thiện so với đánh giá trước:** Báo cáo đã đồng bộ kiến trúc 5 tầng thực tế và bổ sung deployment diagram.

**Điểm trừ (-2):** Chưa có UML class diagram chi tiết; Azure deploy vẫn placeholder.

---

### 4.4. Áp dụng kiến thức từ sách — **ĐẠT** (**14/15**)

| Khái niệm (đề bài) | Trạng thái | Bằng chứng |
|---|---|---|
| Architectural characteristics | **Met** | §7.3 — bảng 1–5 cho 5 thuộc tính chất lượng |
| Coupling & cohesion | **Met** | API inject `*Business`; Business inject `*Service`; không gọi DbContext trực tiếp |
| Modularity | **Met** | Folders `Business/`, `Services/`, `Persistence/`, `Database/` |
| Domain partitioning | **Met** | §4.6 mapping đầy đủ theo domain |
| Distributed system patterns | **N/A** | Monolith — hợp lý với quyết định kiến trúc |
| Data consistency | **Met** | EF Core + SQL Server ACID; validation tại `*Business` |

**Ví dụ code:**

- `Api/Controllers/PatientsController.cs` → `IPatientBusiness`
- `Business/PatientBusiness.cs` → `ValidatePatient`, `BusinessResult<T>`
- `DependencyInjection/ServiceCollectionExtensions.cs` → đăng ký 3 tầng logic

**Điểm trừ (-1):** Chưa có phân tích explicit về deployability / testability như trong sách (chỉ ám chỉ).

---

### 4.5. Implementation — **ĐẠT** (**9/10**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Triển khai tối thiểu một phần quan trọng | **Vượt yêu cầu** | End-to-end: Patient, Appointment, Prescription, Report, Admin, Auth |
| Backend APIs | **Met** | 5 controllers, JWT + Swagger |
| Công nghệ tự chọn | **Met** | ASP.NET Core 6.0, EF Core, SQL Server, Razor Pages |
| Chạy được | **Met** | Build Release OK; 4 unit test pass |

**Phạm vi implementation:**

| Thành phần | Mô tả |
|---|---|
| Web UI | Razor Pages, theme teal/slate, `_DashboardLayout`, status 1-based |
| REST API | Dual auth JwtOrCookie, CRUD đầy đủ |
| Auth | `/Account/Register` công khai + `/Administrator/Register` admin-only |
| Email | `EmailSender`, `NotificationService` |
| DevOps | GitHub Actions, Docker, Serilog, health checks |
| Branding | `wwwroot/resources/logo-two.svg`, `main-logo.svg` |
| Tests | 4 repository tests (InMemory EF) |

**Điểm trừ (-1):** Test coverage mỏng; .NET 6 EOL (NETSDK1138).

---

### 4.6. Đánh giá & trade-offs — **ĐẠT** (**9/10**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Điểm mạnh / yếu | **Met** | §7.1, §7.2 |
| Rủi ro kiến trúc | **Met** | Bảng rủi ro §7.2 |
| Scale / evolution | **Met** | §7.4 — 4 giai đoạn monolith → microservices → event-driven |

**Điểm trừ (-1):** Thiếu metric định lượng (load test, p95 latency).

---

### 4.7. Báo cáo & trình bày — **ĐẠT** (**9/10**)

| Yêu cầu đề bài | Trạng thái | Bằng chứng |
|---|---|---|
| Outline 8 chương | **Met** | `SA_REPORT.md` đủ 8 chương + phụ lục rubric |
| Viết rõ ràng, logic | **Met** | FR/NFR, Mermaid, bảng 5-layer, luồng auth |
| Trình bày đẹp | **Partial** | Markdown tốt; chưa có PDF nộp chính thức trong repo |
| Tài liệu bổ sung | **Met** | README, TAI_LIEU, SA_EVALUATION cross-link |

**Cải thiện:** Báo cáo đã đồng bộ codebase (09/06/2026); thêm §6.5 UI, §6.6 auth, §6.7 tests.

**Điểm trừ (-1):** Chưa export PDF 15–35 trang theo format nộp bài.

---

### 4.8. Sáng tạo & mở rộng — **ĐẠT MỘT PHẦN** (**4/5**)

| Hạng mục | Trạng thái | Bằng chứng |
|---|---|---|
| Ý tưởng / cải tiến | **Met** | Refactor 5-layer; dual auth; NotificationService |
| UI redesign | **Met** | Theme mới, auth card, logo SVG, status 1-based, datetime picker fix |
| Mở rộng tương lai | **Met** | §8.2 — mobile, Azure, HL7/FHIR |
| Đột phá kiến trúc | **Partial** | Monolith truyền thống, chưa pattern phân tán mới |

---

## 5. Đánh giá Bonus (+10 điểm)

| Hạng mục bonus | Điểm tối đa | Trạng thái | Bằng chứng | Điểm |
|---|---|---|---|---|
| Cloud (AWS/Azure/GCP) | +3 | **Partial** | `azure-deploy.yml` placeholder; Docker-ready | **+1** |
| CI/CD | +3 | **Met** | `.github/workflows/dotnet.yml` | **+3** |
| Monitoring / Logging | +2 | **Met** | Serilog, RequestLoggingMiddleware, health checks SQL | **+2** |
| Distributed system | +2 | **Not Met** | Monolith; docker-compose chỉ tách DB | **0** |
| **Tổng bonus** | **+10** | | | **+6** |

---

## 6. Đối chiếu yêu cầu chi tiết (mục 2.2 đề bài)

| Nhóm (1)–(6) | Kết luận |
|---|---|
| (1) Phân tích hệ thống | **Đạt** |
| (2) Lựa chọn kiến trúc | **Đạt** |
| (3) Thiết kế kiến trúc | **Đạt** |
| (4) Áp dụng kỹ thuật kiến trúc | **Đạt** |
| (5) Implementation | **Đạt** (vượt mức tối thiểu) |
| (6) Đánh giá & trade-off | **Đạt** |

| Tiêu chí đạt/không đạt | Đánh giá |
|---|---|
| Kiến trúc rõ ràng | **Có** — 5-layer + REST API |
| Phân tích trade-offs | **Có** |
| Implementation | **Có** — build/test pass |
| **Kết luận** | **ĐẠT** |

---

## 7. Bảng tổng hợp điểm

| Tiêu chí | Tối đa | SA_REPORT (tự) | **Đánh giá độc lập** | Trạng thái |
|---|---|---|---|---|
| 1. Phân tích yêu cầu | 15 | 14 | **14** | Met |
| 2. Lựa chọn kiến trúc | 15 | 14 | **14** | Met |
| 3. Thiết kế kiến trúc | 20 | 18 | **18** | Met |
| 4. Áp dụng kiến thức sách | 15 | 14 | **14** | Met |
| 5. Implementation | 10 | 9 | **9** | Met |
| 6. Đánh giá & trade-offs | 10 | 9 | **9** | Met |
| 7. Báo cáo & trình bày | 10 | 9 | **9** | Met |
| 8. Sáng tạo & mở rộng | 5 | 4 | **4** | Partial |
| **Cộng mục chính** | **100** | **91** | **91** | |
| Bonus | +10 | +6 | **+6** | Partial |
| **TỔNG ƯỚC LƯỢNG** | **110** | **~97** | **~97** | **Giỏi** |

**Xếp loại ước lượng:** ~88% trên thang 100 (+ bonus) → **Giỏi** (91/100 + 6 bonus = 97/110).

---

## 8. Khoảng trống & khuyến nghị

### 8.1. Khoảng trống còn lại

| # | Vấn đề | Mức độ | Khuyến nghị |
|---|---|---|---|
| 1 | Chưa có PDF báo cáo nộp bài | Thấp | Export `SA_REPORT.md` → PDF 15–35 trang |
| 2 | Azure deploy placeholder | Trung bình | Hoàn thiện ACR + App Service (+3 cloud bonus) |
| 3 | Test coverage mỏng | Trung bình | Thêm Business + API integration tests |
| 4 | .NET 6 EOL | Trung bình | Nâng cấp .NET 8 LTS |
| 5 | Secrets trong appsettings/docker-compose | Cao (prod) | User Secrets, env vars, Key Vault |
| 6 | Đăng ký Staff không cần duyệt Admin | Thấp (demo) | Approval workflow nếu triển khai thật |
| 7 | Chưa có load test / metrics | Thấp | Benchmark p95, Application Insights |

### 8.2. Điểm mạnh nổi bật

- Domain phù hợp đề bài (quản lý bệnh viện).
- Implementation **vượt** yêu cầu tối thiểu — hệ thống end-to-end.
- Kiến trúc 5 tầng có bằng chứng code và báo cáo đồng bộ.
- Dual interface Web + API; CI/CD + Docker + Serilog thực tế.
- UI redesign, đăng ký công khai, tài liệu đa ngôn ngữ.

---

## 9. Kết luận

Dự án **MediSphere đáp ứng đầy đủ** yêu cầu tối thiểu trong `SA requirements.docx`. Sau cập nhật `SA_REPORT.md` (09/06/2026), báo cáo và codebase **nhất quán** về kiến trúc 5 tầng, luồng auth, UI và DevOps. Điểm ước lượng **~97/110**, xếp loại **Giỏi**, đạt tiêu chí **ĐẠT** của đề bài.

Để tối đa hóa điểm khi nộp: export PDF, hoàn thiện triển khai cloud thực tế, mở rộng test coverage.

---

*Đánh giá đối chiếu mã nguồn — cập nhật 09/06/2026.*
