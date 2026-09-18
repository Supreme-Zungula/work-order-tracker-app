

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