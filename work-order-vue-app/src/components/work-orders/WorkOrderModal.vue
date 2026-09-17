<script setup lang="ts">
import { ref, watch } from 'vue'
import type { CreateWorkOrderRequest, WorkOrder } from '@/types/work-order'
import { workOrderService } from '@/services/api'

interface Props {
  modelValue: boolean
}

interface Emits {
  (e: 'update:modelValue', value: boolean): void
  (e: 'created', workOrder: WorkOrder): void
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const isSubmitting = ref(false)
const error = ref<string | null>(null)

const form = ref<CreateWorkOrderRequest>({
  title: '',
  description: '',
  assignedTo: '',
  dueDate: '',
})

const minDueDate = new Date().toISOString().split('T')[0]

function resetForm() {
  form.value = {
    title: '',
    description: '',
    assignedTo: '',
    dueDate: '',
  }
  error.value = null
}

watch(
  () => props.modelValue,
  (isOpen) => {
    if (isOpen) {
      resetForm()
    }
  },
)

async function handleSubmit() {
  if (!form.value.title || !form.value.assignedTo) {
    error.value = 'Title and Assigned To are required'
    return
  }

  isSubmitting.value = true
  error.value = null

  try {
    const payload: CreateWorkOrderRequest = {
      title: form.value.title,
      description: form.value.description || undefined,
      assignedTo: form.value.assignedTo,
      dueDate: form.value.dueDate || undefined,
    }
    const created = await workOrderService.createWorkOrder(payload)
    emit('created', created)
    emit('update:modelValue', false)
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to create work order'
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  emit('update:modelValue', false)
}
</script>

<template>
  <Transition name="modal-fade">
    <div v-if="modelValue" class="fixed inset-0 z-50 overflow-y-auto" @click.self="handleCancel">
      <div class="flex min-h-full items-center justify-center p-4">
        <div class="fixed inset-0 bg-black/50 transition-opacity" aria-hidden="true" />

        <div
          class="relative w-full max-w-lg bg-white rounded-lg shadow-xl transform transition-all"
        >
          <div class="flex items-center justify-between p-4 border-b border-gray-200">
            <h3 class="text-lg font-semibold text-gray-900">Create Work Order</h3>
            <button
              type="button"
              class="text-gray-400 hover:text-gray-500 transition-colors"
              @click="handleCancel"
              aria-label="Close modal"
            >
              <svg class="h-6 w-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </button>
          </div>

          <form @submit.prevent="handleSubmit" class="p-4 space-y-4">
            <div
              v-if="error"
              class="p-3 text-sm text-red-600 bg-red-50 border border-red-200 rounded-md"
              role="alert"
            >
              {{ error }}
            </div>

            <div>
              <label for="title" class="block text-sm font-medium text-gray-700 mb-1"
                >Title <span class="text-red-500">*</span></label
              >
              <input
                id="title"
                v-model="form.title"
                type="text"
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                placeholder="Enter work order title"
                autocomplete="off"
              />
            </div>

            <div>
              <label for="description" class="block text-sm font-medium text-gray-700 mb-1"
                >Description</label
              >
              <textarea
                id="description"
                v-model="form.description"
                rows="3"
                class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                placeholder="Enter description (optional)"
              />
            </div>

            <div>
              <label for="assignedTo" class="block text-sm font-medium text-gray-700 mb-1"
                >Assigned To <span class="text-red-500">*</span></label
              >
              <input
                id="assignedTo"
                v-model="form.assignedTo"
                type="text"
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                placeholder="Enter assignee name"
                autocomplete="off"
              />
            </div>

            <div>
              <label for="dueDate" class="block text-sm font-medium text-gray-700 mb-1"
                >Due Date</label
              >
              <input
                id="dueDate"
                v-model="form.dueDate"
                type="date"
                :min="minDueDate"
                class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>

            <div class="flex justify-end space-x-3 pt-4 border-t border-gray-200">
              <button
                type="button"
                @click="handleCancel"
                class="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                :disabled="isSubmitting"
                class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
              >
                <span v-if="isSubmitting" class="flex items-center space-x-2">
                  <svg class="animate-spin h-4 w-4" viewBox="0 0 24 24">
                    <circle
                      class="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      stroke-width="4"
                      fill="none"
                    />
                    <path
                      class="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    />
                  </svg>
                  <span>Creating...</span>
                </span>
                <span v-else>Create</span>
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </Transition>
</template>

<style scoped>
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.2s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}
</style>
