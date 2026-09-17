<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { VDataTable, VMenu } from 'vuetify/components'
import { workOrderService } from '@/services/api'
import type { WorkOrder, WorkOrderStatus } from '@/types/work-order'
import WorkOrderModal from '@/components/work-orders/WorkOrderModal.vue'

const workOrders = ref<WorkOrder[]>([])
const loading = ref(false)
const totalItems = ref(0)
const currentPage = ref(1)
const pageSize = 10
const showModal = ref(false)
const selectedWorkOrder = ref<WorkOrder | null>(null)

const headers = [
  { title: 'ID', key: 'id', sortable: false, width: 60 },
  { title: 'Title', key: 'title', sortable: false },
  { title: 'Status', key: 'status', sortable: false, width: 140 },
  { title: 'Assigned To', key: 'assignedTo', sortable: false, width: 160 },
  { title: 'Created', key: 'createdAt', sortable: false, width: 160 },
  { title: 'Due Date', key: 'dueDate', sortable: false, width: 160 },
  { title: 'Actions', key: 'actions', sortable: false, width: 100 },
]

const statusColors: Record<WorkOrderStatus, string> = {
  Pending: 'warning',
  InProgress: 'info',
  Completed: 'success',
  Cancelled: 'error',
}

function formatDate(dateString: string | null): string {
  if (!dateString) return '-'
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

async function fetchWorkOrders() {
  loading.value = true
  try {
    const response = await workOrderService.getWorkOrders(currentPage.value, pageSize)
    workOrders.value = response.items
    totalItems.value = response.totalCount
  } catch (error) {
    console.error('Failed to fetch work orders:', error)
  } finally {
    loading.value = false
  }
}

function onPageChange(page: number) {
  currentPage.value = page
  fetchWorkOrders()
}

function handleWorkOrderCreated(newWorkOrder: WorkOrder) {
  workOrders.value.unshift(newWorkOrder)
  totalItems.value += 1
}

function handleUpdateOrder(workOrder: WorkOrder) {
  selectedWorkOrder.value = workOrder
  showModal.value = true
}

function handleViewActivities(workOrder: WorkOrder) {
  // Navigate to activities view with work order filter
  // For now, just log - can be implemented with router
  console.log('View activities for:', workOrder.id)
}

async function handleDeleteOrder(workOrder: WorkOrder) {
  if (!confirm(`Are you sure you want to delete work order "${workOrder.title}"?`)) {
    return
  }
  try {
    await workOrderService.deleteWorkOrder(workOrder.id)
    workOrders.value = workOrders.value.filter(wo => wo.id !== workOrder.id)
    totalItems.value -= 1
  } catch (error) {
    console.error('Failed to delete work order:', error)
    alert('Failed to delete work order')
  }
}

onMounted(() => {
  fetchWorkOrders()
})
</script>

<template>
  <v-container fluid class="pa-4">
    <v-card>
      <v-card-title class="d-flex align-center justify-space-between">
        <span class="text-h5">Work Orders</span>
        <div class="d-flex gap-2">
          <v-btn color="primary" @click="showModal = true">
            <v-icon start>mdi-plus</v-icon>
            Add Work Order
          </v-btn>
          <v-btn color="primary" @click="fetchWorkOrders" :disabled="loading">
            <v-icon start>mdi-refresh</v-icon>
            Refresh
          </v-btn>
        </div>
      </v-card-title>

      <v-data-table
        v-model="workOrders"
        :headers="headers"
        :items="workOrders"
        :loading="loading"
        :items-per-page="pageSize"
        :server-items-length="totalItems"
        :current-page="currentPage"
        @update:current-page="onPageChange"
        :disable-sort="true"
        density="compact"
        hover
      >
        <template #[`item.status`]="{ item }">
          <v-chip
            :color="statusColors[item.status as WorkOrderStatus]"
            size="x-small"
            variant="tonal"
          >
            {{ item.status }}
          </v-chip>
        </template>

        <template #[`item.createdAt`]="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>

        <template #[`item.dueDate`]="{ item }">
          {{ formatDate(item.dueDate) }}
        </template>
        <!-- Actions column -->
      <template #[`item.actions`]="{ item }">
          <v-menu location="bottom end" offset-y>
            <template v-slot:activator="{ props }">
              <v-btn
                v-bind="props"
                icon
                variant="text"
                class="text-white hover:text-gray-300"
                aria-label="More options"
              >
                <v-icon>mdi-dots-vertical</v-icon>
              </v-btn>
            </template>
            <v-list class="py-2" min-width="180">
              <v-list-item
                @click="handleUpdateOrder(item)"
                class="px-3"
              >
                <v-list-item-title class="text-sm">
                  <v-icon start class="mr-2" size="18">mdi-pencil</v-icon>
                  Update Order
                </v-list-item-title>
              </v-list-item>
              <v-list-item
                @click="handleViewActivities(item)"
                class="px-3"
              >
                <v-list-item-title class="text-sm">
                  <v-icon start class="mr-2" size="18">mdi-clock-outline</v-icon>
                  View Activities
                </v-list-item-title>
              </v-list-item>
              <v-divider class="my-2" />
              <v-list-item
                @click="handleDeleteOrder(item)"
                class="px-3 text-error"
              >
                <v-list-item-title class="text-sm">
                  <v-icon start class="mr-2" size="18">mdi-delete</v-icon>
                  Delete
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </template>

        <template #no-data>
          <v-alert type="info" variant="tonal" class="ma-4">
            No work orders found
          </v-alert>
        </template>
      </v-data-table>

      <v-card-actions class="pa-4">
        <v-pagination
          v-model="currentPage"
          :length="Math.ceil(totalItems / pageSize)"
          @update:model-value="onPageChange"
        />
      </v-card-actions>
    </v-card>

    <WorkOrderModal
      v-model="showModal"
      @created="handleWorkOrderCreated"
    />
  </v-container>
</template>

<style scoped>
</style>
