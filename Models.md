# Backend Models & DTOs

This document describes the data models and Data Transfer Objects (DTOs) used in the Work Order Tracker API.

## Domain Models (Entities)

These are the Entity Framework Core entities that map to database tables.

### WorkOrderStatus (Enum)

```csharp
namespace WorkOrderApi.Models;

public enum WorkOrderStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
```

| Value | Name | Description |
|-------|------|-------------|
| 0 | Pending | Work order created but not started |
| 1 | InProgress | Work order is being worked on |
| 2 | Completed | Work order finished successfully |
| 3 | Cancelled | Work order was cancelled |

---

### WorkOrder

```csharp
namespace WorkOrderApi.Models;

public class WorkOrder
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Pending;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
```

| Property | Type | Nullable | Default | Description |
|----------|------|----------|---------|-------------|
| Id | `int` | No | Auto-generated | Primary key |
| Title | `string` | No | - | Work order title (max 200 chars) |
| Description | `string?` | Yes | null | Detailed description (max 2000 chars) |
| Status | `WorkOrderStatus` | No | `Pending` | Current status |
| AssignedTo | `string` | No | - | Assignee name (max 100 chars) |
| CreatedAt | `DateTime` | No | `DateTime.UtcNow` | Creation timestamp |
| DueDate | `DateTime?` | Yes | null | Optional due date |
| CompletedAt | `DateTime?` | Yes | null | Set when status = Completed |
| Activities | `ICollection<Activity>` | No | Empty list | Navigation property |

---

### Activity

```csharp
namespace WorkOrderApi.Models;

public class Activity
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
```

| Property | Type | Nullable | Default | Description |
|----------|------|----------|---------|-------------|
| Id | `int` | No | Auto-generated | Primary key |
| WorkOrderId | `int` | No | - | Foreign key to WorkOrder |
| WorkOrder | `WorkOrder?` | Yes | null | Navigation property |
| Action | `string` | No | - | Action performed (e.g., "Created", "StatusChanged") |
| Details | `string?` | Yes | null | Additional details |
| PerformedBy | `string` | No | - | User who performed the action |
| Timestamp | `DateTime` | No | `DateTime.UtcNow` | When the action occurred |

---

## Data Transfer Objects (DTOs)

DTOs are used for API request/response payloads.

### WorkOrderDto (Response)

```csharp
namespace WorkOrderApi.Dtos;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;  // String representation
    public string AssignedTo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<ActivityDto> Activities { get; set; } = new();
}
```

| Property | Type | Description |
|----------|------|-------------|
| Id | `int` | Work order identifier |
| Title | `string` | Work order title |
| Description | `string?` | Optional description |
| Status | `string` | Status as string (Pending, InProgress, Completed, Cancelled) |
| AssignedTo | `string` | Assignee name |
| CreatedAt | `DateTime` | Creation timestamp |
| DueDate | `DateTime?` | Optional due date |
| CompletedAt | `DateTime?` | Completion timestamp |
| Activities | `List<ActivityDto>` | Associated activities |

---

### CreateWorkOrderDto (Request)

```csharp
namespace WorkOrderApi.Dtos;

public class CreateWorkOrderDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required, MaxLength(100)]
    public string AssignedTo { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
}
```

| Property | Type | Required | Validation | Description |
|----------|------|----------|------------|-------------|
| Title | `string` | Yes | Max 200 chars | Work order title |
| Description | `string?` | No | Max 2000 chars | Optional description |
| AssignedTo | `string` | Yes | Max 100 chars | Assignee name |
| DueDate | `DateTime?` | No | - | Optional due date |

**Note**: Status is automatically set to `Pending` on creation.

---

### UpdateWorkOrderStatusDto (Request)

```csharp
namespace WorkOrderApi.Dtos;

public class UpdateWorkOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
```

| Property | Type | Required | Validation | Description |
|----------|------|----------|------------|-------------|
| Status | `string` | Yes | Must be valid enum value | New status value |

**Valid values**: `Pending`, `InProgress`, `Completed`, `Cancelled`

---

### ActivityDto (Response)

```csharp
namespace WorkOrderApi.Dtos;

public class ActivityDto
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
```

| Property | Type | Description |
|----------|------|-------------|
| Id | `int` | Activity identifier |
| WorkOrderId | `int` | Parent work order ID |
| Action | `string` | Action performed |
| Details | `string?` | Optional details |
| PerformedBy | `string` | User who performed action |
| Timestamp | `DateTime` | When action occurred |

---

## API Response Formats

### Paginated List Response

All list endpoints return paginated results:

```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 42
}
```

| Field | Type | Description |
|-------|------|-------------|
| items | `T[]` | Array of items for current page |
| page | `int` | Current page number (1-based) |
| pageSize | `int` | Items per page |
| totalCount | `int` | Total items across all pages |

---

## Entity Relationships

```
WorkOrder (1) ──────< (Many) Activity
     │
     └── Id (PK)
           │
           └── WorkOrderId (FK)
```

- **WorkOrder** has many **Activities**
- **Activity** belongs to one **WorkOrder**
- Cascade delete: Deleting a WorkOrder deletes its Activities

---

## Database Schema (SQLite)

### WorkOrders Table

```sql
CREATE TABLE WorkOrders (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NULL,
    Status INTEGER NOT NULL DEFAULT 0,
    AssignedTo TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    DueDate TEXT NULL,
    CompletedAt TEXT NULL
);
```

### Activities Table

```sql
CREATE TABLE Activities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    WorkOrderId INTEGER NOT NULL,
    Action TEXT NOT NULL,
    Details TEXT NULL,
    PerformedBy TEXT NOT NULL,
    Timestamp TEXT NOT NULL,
    FOREIGN KEY (WorkOrderId) REFERENCES WorkOrders(Id) ON DELETE CASCADE
);
```

---

## Example API Payloads

### Create Work Order Request

```json
POST /api/workorders
Content-Type: application/json

{
  "title": "Fix login page",
  "description": "Users cannot login with Google OAuth",
  "assignedTo": "john.doe",
  "dueDate": "2026-09-30T23:59:59Z"
}
```

### Create Work Order Response (201)

```json
{
  "id": 1,
  "title": "Fix login page",
  "description": "Users cannot login with Google OAuth",
  "status": "Pending",
  "assignedTo": "john.doe",
  "createdAt": "2026-09-17T10:30:00Z",
  "dueDate": "2026-09-30T23:59:59Z",
  "completedAt": null,
  "activities": []
}
```

### Update Status Request

```json
PATCH /api/workorders/1/status
Content-Type: application/json

{
  "status": "InProgress"
}
```

### Get Activities Response

```json
{
  "items": [
    {
      "id": 1,
      "workOrderId": 1,
      "action": "Created",
      "details": "Work order created",
      "performedBy": "system",
      "timestamp": "2026-09-17T10:30:00Z"
    },
    {
      "id": 2,
      "workOrderId": 1,
      "action": "StatusChanged",
      "details": "Status changed from Pending to InProgress",
      "performedBy": "john.doe",
      "timestamp": "2026-09-17T11:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 2
}
```