<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { VDataTable } from 'vuetify/components'
import { activityService } from '@/services/api'
import type { Activity } from '@/types/work-order'

const activities = ref<Activity[]>([])
const loading = ref(false)
const totalItems = ref(0)
const currentPage = ref(1)
const pageSize = 10

const headers = [
  { title: 'Work Order', key: 'workOrderId', sortable: false, width: 120 },
  { title: 'Action', key: 'action', sortable: false },
  { title: 'Details', key: 'details', sortable: false },
  { title: 'Performed By', key: 'performedBy', sortable: false, width: 160 },
  { title: 'Timestamp', key: 'timestamp', sortable: false, width: 180 },
]

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

async function fetchActivities() {
  loading.value = true
  try {
    const response = await activityService.getActivities(currentPage.value, pageSize)
    activities.value = response.items
    totalItems.value = response.totalCount
  } catch (error) {
    console.error('Failed to fetch activities:', error)
  } finally {
    loading.value = false
  }
}

function onPageChange(page: number) {
  currentPage.value = page
  fetchActivities()
}

onMounted(() => {
  fetchActivities()
})
</script>

<template>
  <v-container fluid class="pa-4">
    <v-card>
      <v-card-title class="d-flex align-center justify-space-between">
        <span class="text-h5">All Activities</span>
        <v-btn color="primary" @click="fetchActivities" :disabled="loading">
          <v-icon start>mdi-refresh</v-icon>
          Refresh
        </v-btn>
      </v-card-title>

      <v-data-table
        :headers="headers"
        :items="activities"
        :loading="loading"
        :items-per-page="pageSize"
        :server-items-length="totalItems"
        :current-page="currentPage"
        @update:current-page="onPageChange"
        :disable-sort="true"
        density="compact"
        hover
      >
        <template #[`item.workOrderId`]="{ item }">
          <span class="font-monospace">#{{ item.workOrderId }}</span>
        </template>

        <template #[`item.details`]="{ item }">
          <span v-if="item.details">{{ item.details }}</span>
          <span v-else class="text-gray-400">-</span>
        </template>

        <template #[`item.timestamp`]="{ item }">
          {{ formatDate(item.timestamp) }}
        </template>

        <template #no-data>
          <v-alert type="info" variant="tonal" class="ma-4">
            No activities found
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