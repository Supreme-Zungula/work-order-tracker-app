export type WorkOrderStatus = 'Pending' | 'InProgress' | 'Completed' | 'Cancelled'

export interface WorkOrder {
  id: number
  title: string
  description: string | null
  status: WorkOrderStatus
  assignedTo: string
  createdAt: string
  dueDate: string | null
  completedAt: string | null
  activities: Activity[]
}

export interface Activity {
  id: number
  workOrderId: number
  action: string
  details: string | null
  performedBy: string
  timestamp: string
}

export interface WorkOrderListResponse {
  items: WorkOrder[]
  page: number
  pageSize: number
  totalCount: number
}

export interface CreateWorkOrderRequest {
  title: string
  description?: string
  assignedTo: string
  dueDate?: string
}

export interface UpdateWorkOrderStatusRequest {
  status: WorkOrderStatus
}
