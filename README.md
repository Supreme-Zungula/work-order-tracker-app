# Work Order Tracker

Full-stack work order tracking application with ASP.NET Core Web API backend and Vue 3 + TypeScript frontend.

## Backend (ASP.NET Core Web API)

### Tech Stack
- .NET 10
- Entity Framework Core with SQLite
- RESTful API with proper HTTP semantics

### Project Structure
```
work-order-api/
├── Controllers/
│   ├── WorkOrdersController.cs
│   └── ActivitiesController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── DbInitializer.cs
│   └── Migrations/
├── Models/
│   ├── WorkOrder.cs
│   ├── Activity.cs
│   └── WorkOrderStatus.cs
├── Dtos/
│   ├── WorkOrderDto.cs
│   ├── CreateWorkOrderDto.cs
│   ├── UpdateWorkOrderStatusDto.cs
│   └── ActivityDto.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── .env
├── Program.cs
└── WorkOrderApi.csproj
```

### API Endpoints

#### Work Orders
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workorders` | List work orders (paginated, filterable by status) |
| GET | `/api/workorders/{id}` | Get work order detail with activities |
| POST | `/api/workorders` | Create new work order (status: Pending) |
| PATCH | `/api/workorders/{id}/status` | Update work order status |
| DELETE | `/api/workorders/{id}` | Delete work order |

#### Activities
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/activities` | List activities (paginated, filterable by workOrderId) |

### Query Parameters

**Work Orders List**
- `page` (default: 1) - Page number, must be >= 1
- `pageSize` (default: 10) - Items per page, 1-100
- `status` (optional) - Filter by status: `Pending`, `InProgress`, `Completed`, `Cancelled`

**Activities List**
- `page` (default: 1) - Page number, must be >= 1
- `pageSize` (default: 10) - Items per page, 1-100
- `workOrderId` (optional) - Filter by work order ID

### Running the Backend
```bash
cd work-order-api
dotnet run
```
The API will be available at `http://localhost:5039` (HTTP) and `https://localhost:7219` (HTTPS).

### Database
- SQLite database file: `workorders.db`
- Auto-migrates on startup
- Seeded with sample data on first run

### CORS
Configured for Vue dev server at `http://localhost:5173` and `http://localhost:5174`.

## Frontend (Vue 3 + TypeScript)
*To be implemented in `work-order-vue-app/`*

## License
MIT