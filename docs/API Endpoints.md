#### Work Orders
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workorders` | List work orders (paginated, filterable by status) |
| GET | `/api/workorders/{id}` | Get work order detail with activities |
| POST | `/api/workorders` | Create new work order (status: Pending) |
| PATCH | `/api/workorders/{id}/status` | Update work order status |
| DELETE | `/api/workorders/{id}` | Delete work order |

#### Activities
| Method | Endpoint          | Description                                            |
| ------ | ----------------- | ------------------------------------------------------ |
| GET    | `/api/activities` | List activities (paginated, filterable by workOrderId) |
