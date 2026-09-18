## Problem statement
A field services company needs a simple system for dispatchers to manage work orders and for technicians to process them
on the go. The organization hosts services on Azure App Services, builds the web UI with Vue.js, and uses C# .NET Core
for backend APIs with a relational database. The mobile app is packaged with Capacitor.

IN SHORT: I must build and app that can be used to manage and  track work order activity, this include creation, update and removal of work orders.
## Stack
### Backend: 
- ASP.Net webapi
- Sqlite Database
- Packages: Entity framework (Sqlite and Tools)
- API documentation: Swagger or Something else if I have time
### Models:
- WorkOrder 
- Activity
- WorkOrderStatus (enum)

### API Endpoints
#### Work Orders
| Method | Endpoint                      | Description                                        |
| ------ | ----------------------------- | -------------------------------------------------- |
| GET    | `/api/workorders`             | List work orders (paginated, filterable by status) |
| GET    | `/api/workorders/{id}`        | Get work order detail with activities              |
| POST   | `/api/workorders`             | Create new work order (status: Pending)            |
| PATCH  | `/api/workorders/{id}/status` | Update work order status                           |
| DELETE | `/api/workorders/{id}`        | Delete work order                                  |
|        |                               |                                                    |

#### Activities
| Method | Endpoint          | Description                                            |
| ------ | ----------------- | ------------------------------------------------------ |
| GET    | `/api/activities` | List activities (paginated, filterable by workOrderId) |

### Frontend:
- Vue 3 with Typescript
- Pinia for state management.
- VueUse for composables.
- Tailwindcss for styling
- Vuetify for components. (Other options: Shadcn-vue, PrimeVue, ElementUI)
- API calls will be done use fetch API. (Other options are Axios, TanStack vueQuery)

## Containerisation and CI/CD
I will generate the following with AI tools once I have a basic project.
- Dockerfile,
- Docker compose 
- GitHub workflows file 
## Project architecture.
Project will use mono repo that has two main folder for APS.NET backend and Vue client.

**Reason**: *It's a small project. Different folder for backend and UI provide nice isolation of concerns.*

### Project folder structure

work-order-tracker/
├── work-order-api/                       # .NET Web API Backend
│   ├── Controllers/
│   │   ├── WorkOrdersController.cs
│   │   └── ActivitiesController.cs
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── DbInitializer.cs
│   │   └── Migrations/
│   ├── Models/
│   │   ├── WorkOrder.cs
│   │   ├── Activity.cs
│   │   └── WorkOrderStatus.cs
│   ├── Dtos/
│   │   ├── WorkOrderDto.cs
│   │   ├── CreateWorkOrderDto.cs
│   │   ├── UpdateWorkOrderStatusDto.cs
│   │   └── ActivityDto.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── .env
│   ├── Program.cs
│   └── WorkOrderApi.csproj
│
├── work-order-vue-app/                   # Vue 3 + TypeScript 
│   ├── src/
│   │   ├── assets/
│   │   ├── components/
│   │   │   ├── work-orders/
│   │   │   │   ├── WorkOrderCard.vue
│   │   │   │   ├── WorkOrderForm.vue
│   │   │   │   ├── WorkOrderList.vue
│   │   │   │   └── WorkOrderStatusBadge.vue
│   │   │   └── activities/
│   │   │       ├── ActivityItem.vue
│   │   │       └── ActivityLog.vue
│   │   ├── router/
│   │   │   └── index.ts
│   │   ├── services/
│   │   │   ├── api.ts
│   │   │   ├── workOrderService.ts
│   │   │   └── activityService.ts
│   │   ├── stores/
│   │   │   └── workOrderStore.ts
│   │   ├── types/
│   │   │   ├── work-order.ts
│   │   │   └── activity.ts
│   │   ├── views/
│   │   │   ├── WorkOrdersView.vue
│   │   │   └── WorkOrderDetailView.vue
│   │   ├── App.vue
│   │   └── main.ts
│   ├── public/
│   ├── .env
│   ├── .env.local
│   ├── package.json
│   ├── tsconfig.json
│   └── vite.config.ts
│
├── .gitignore
└── README.md