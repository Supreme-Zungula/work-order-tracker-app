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

### Tech Stack
- **Framework**: Vue 3 (Composition API, `<script setup>`)
- **Language**: TypeScript
- **Build Tool**: Vite
- **UI Library**: Vuetify 3 (Material Design components)
- **Styling**: Tailwind CSS v4
- **Routing**: Vue Router 4
- **State Management**: Pinia
- **HTTP Client**: Native Fetch API (via composable)
- **Linting/Formatting**: Oxlint, ESLint, oxfmt
- **Package Manager**: pnpm

### Project Structure
```
work-order-vue-app/
├── public/
│   └── favicon.ico
├── src/
│   ├── assets/
│   │   ├── base.css
│   │   └── main.css          # Tailwind imports
│   ├── components/
│   │   ├── index.ts          # Component exports
│   │   ├── Navbar.vue        # Navigation bar
│   │   └── work-orders/
│   │       └── WorkOrderModal.vue  # Create/Edit work order modal
│   ├── composables/
│   │   └── useApiBaseUrl.ts  # API base URL configuration
│   ├── plugins/
│   │   └── vuetify.ts        # Vuetify configuration
│   ├── router/
│   │   └── index.ts          # Vue Router configuration
│   ├── services/
│   │   └── api.ts            # API service layer
│   ├── stores/
│   │   └── counter.ts        # Pinia store example
│   ├── types/
│   │   └── work-order.ts     # TypeScript interfaces
│   ├── views/
│   │   ├── HomeView.vue      # Work orders list with table
│   │   ├── ActivitiesView.vue       # All activities list
│   │   ├── WorkOrderActivitiesView.vue  # Activities for specific work order
│   │   └── AboutView.vue
│   ├── App.vue               # Root component
│   └── main.ts               # Application entry point
├── .env.local
├── index.html
├── package.json
├── tsconfig.json
├── tsconfig.app.json
├── tsconfig.node.json
├── vite.config.ts
└── pnpm-lock.yaml
```

### Routes
| Path | Name | Description |
|------|------|-------------|
| `/` | `home` | Work orders list with create/update/delete |
| `/about` | `about` | About page |
| `/activities` | `activities` | All activities across work orders |
| `/work-orders/:id/activities` | `workOrderActivities` | Activities for a specific work order |

### Setup & Run

#### Prerequisites
- Node.js 22.18+ or 24.12+
- pnpm 9+

#### Installation
```bash
cd work-order-vue-app
pnpm install
```

#### Development
```bash
pnpm dev
```
Starts Vite dev server at `http://localhost:5173` (or next available port).

#### Build for Production
```bash
pnpm build
```
Outputs to `dist/` directory.

#### Preview Production Build
```bash
pnpm preview
```

#### Type Checking
```bash
pnpm type-check
```

#### Linting
```bash
pnpm lint
```

#### Formatting
```bash
pnpm format
```

### Environment Variables
Create `.env.local` in `work-order-vue-app/`:
```env
VITE_API_BASE_URL=http://localhost:5039
```
Defaults to `http://localhost:5039` if not set.

### Features
- **Work Orders**: List (paginated), Create, Update status, Delete
- **Activities**: View all activities or filter by work order
- **Responsive Design**: Works on desktop and mobile
- **Modal Forms**: Create/edit work orders in a modal dialog
- **Kebab Menu**: Row actions (update, view activities, delete)
- **Status Chips**: Color-coded work order status indicators
- **Loading States**: Skeleton loaders and disabled buttons during fetch

## Running Both Together

1. Start the backend:
```bash
cd work-order-api
dotnet run
```

2. In a new terminal, start the frontend:
```bash
cd work-order-vue-app
pnpm dev
```

3. Open `http://localhost:5173` in your browser.

## License
MIT