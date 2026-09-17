import { useApiBaseUrl } from '@/composables/useApiBaseUrl'
import type { WorkOrder, WorkOrderListResponse, CreateWorkOrderRequest, UpdateWorkOrderStatusRequest, Activity } from '@/types/work-order'

const { apiBaseUrl } = useApiBaseUrl()

async function fetchApi<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`${apiBaseUrl.value}${endpoint}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
    ...options,
  })

  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: 'Request failed' }))
    throw new Error(error.error || error.message || `HTTP ${response.status}`)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return response.json()
}

export const workOrderService = {
  async getWorkOrders(page = 1, pageSize = 10, status?: string): Promise<WorkOrderListResponse> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    })
    if (status) {
      params.append('status', status)
    }
    return fetchApi<WorkOrderListResponse>(`/workorders?${params.toString()}`)
  },

  async getWorkOrder(id: number): Promise<WorkOrder> {
    return fetchApi<WorkOrder>(`/workorders/${id}`)
  },

  async createWorkOrder(data: CreateWorkOrderRequest): Promise<WorkOrder> {
    return fetchApi<WorkOrder>('/workorders', {
      method: 'POST',
      body: JSON.stringify(data),
    })
  },

  async updateWorkOrderStatus(id: number, data: UpdateWorkOrderStatusRequest): Promise<WorkOrder> {
    return fetchApi<WorkOrder>(`/workorders/${id}/status`, {
      method: 'PATCH',
      body: JSON.stringify(data),
    })
  },

  async deleteWorkOrder(id: number): Promise<void> {
    return fetchApi<void>(`/workorders/${id}`, {
      method: 'DELETE',
    })
  },
}

export const activityService = {
  async getActivities(page = 1, pageSize = 10, workOrderId?: number): Promise<{ items: Activity[], page: number, pageSize: number, totalCount: number }> {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    })
    if (workOrderId) {
      params.append('workOrderId', workOrderId.toString())
    }
    return fetchApi<{ items: Activity[], page: number, pageSize: number, totalCount: number }>(`/activities?${params.toString()}`)
  },
}