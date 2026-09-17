<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { VDataTable } from 'vuetify/components'
import { workOrderService, activityService } from '@/services/api'
import type { WorkOrder, Activity, WorkOrderStatus } from '@/types/work-order'

const route = useRoute()
const router = useRouter()

const workOrder = ref<WorkOrder | null>(null)
const activities = ref<Activity[]>([])
const loading = ref(false)
const activityLoading = ref(false)
const totalItems = ref(0)
const currentPage = ref(1)
const pageSize = 10

const headers = [
  { title: 'Action', key: 'action', sortable: false },
  { title: 'Details', key: 'details', sortable: false },
  { title: 'Performed By', key: 'performedBy', sortable: false },
  { title: 'Timestamp', key: 'timestamp', sortable: false, width: 180 },
]

const statusColors: Record<WorkOrderStatus, string> = {
  Pending: 'warning',
  InProgress: 'info',
  Completed: 'success',
  Cancelled: 'error',
}

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

async function fetchWorkOrder() {
  const id = Number(route.params.id)
  if (!id) return
  
  loading.value = true
  try {
    workOrder.value = await workOrderService.getWorkOrder(id)
  } catch (error) {
    console.error('Failed to fetch work order:', error)
  } finally {
    loading.value = false
  }
}

async function fetchActivities() {
  const id = Number(route.params.id)
  if (!id) return
  
  activityLoading.value = true
  try {
    const response = await activityService.getActivities(currentPage.value, pageSize, id)
    activities.value = response.items
    totalItems.value = response.totalCount
  } catch (error) {
    console.error('Failed to fetch activities:', error)
  } finally {
    activityLoading.value = false
  }
}

function onPageChange(page: number) {
  currentPage.value = page
  fetchActivities()
}

function goBack() {
  router.push({ name: 'home' })
}

watch(() => route.params.id, () => {
  currentPage.value = 1
  fetchWorkOrder()
  fetchActivities()
}, { immediate: true })

onMounted(() => {
  fetchWorkOrder()
  fetchActivities()
})
</script>

<template>
  <v-container fluid class="pa-4">
    <v-btn variant="outlined" @click="goBack" class="mb-4" start>
      <v-icon>mdi-arrow-left</v-icon>
      Back to Work Orders
    </v-btn>

    <v-card v-if="!loading && workOrder" class="mb-4">
      <v-card-title class="pb-2">
        <div class="text-h5">{{ workOrder.title }}</div>
        <div class="text-body-2 text-gray-500 mt-1">{{ workOrder.description || 'No description' }}</div>
        <div class="d-flex align-center gap-4 mt-2">
          <v-chip
            :color="statusColors[workOrder.status]"
            size="small"
            variant="tonal"
          >
            {{ workOrder.status }}
          </v-chip>
          <span class="text-caption">Assigned to: {{ workOrder.assignedTo }}</span>
          <span class="text-caption">Created: {{ formatDate(workOrder.createdAt) }}</span>
          <span v-if="workOrder.dueDate" class="text-caption">Due: {{ formatDate(workOrder.dueDate) }}</span>
        </div>
      </v-card-title>
    </v-card>

    <v-card v-if="loading" class="mb-4">
      <v-card-text class="pa-8 text-center">
        <v-progress-circular indeterminate color="primary" />
      </v-card-text>
    </v-card>

    <v-card>
      <v-card-title class="d-flex align-center justify-space-between">
        <span class="text-h6">Activities</span>
        <span class="text-caption text-gray-500">{{ totalItems }} total</span>
      </v-card-title>

      <v-data-table
        :headers="headers"
        :items="activities"
        :loading="activityLoading"
        :items-per-page="pageSize"
        :server-items-length="totalItems"
        :current-page="currentPage"
        @update:current-page="onPageChange"
        :disable-sort="true"
        density="compact"
        hover
      >
        <template #[`item.timestamp`]="{ item }">
          {{ formatDate(item.timestamp) }}
        </template>

        <template #[`item.details`]="{ item }">
          <span v-if="item.details">{{ item.details }}</span>
          <span v-else class="text-gray-400">-</span>
        </template>

        <template #no-data>
          <v-alert type="info" variant="tonal" class="ma-4">
            No activities found for this work order
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
  </v-container>
</template>

<style scoped>
</style>