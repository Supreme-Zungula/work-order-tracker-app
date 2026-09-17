<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <header>
    <nav class="w-full bg-indigo-900 font-bold px-3 shadow-md lg:w-full">
      <div class="w-full flex items-center justify-between h-16">
        <div class="flex items-center">
          <RouterLink
            to="/"
            class="text-2xl font-bold text-amber-500 hover:text-amber-600 hover:bg-indigo-700 px-2 rounded-md transition-colors"
          >
            Work Order Tracker
          </RouterLink>
        </div>
        <div class="hidden md:block">
          <ul class="flex items-center gap-1 space-x-4">
            <li>
              <RouterLink
                to="/"
                class="px-3 p-2 rounded-md text-green-500 text-lg font-medium transition-colors hover:bg-indigo-700"
                :class="{ 'bg-indigo-700 font-bold': route.name === 'home' }"
              >
                Orders
              </RouterLink>
            </li>
            <li>
              <RouterLink
                to="/activities"
                class="px-3 py-2 rounded-md text-lg text-green-500 font-medium transition-colors hover:bg-indigo-700"
                :class="{ 'bg-indigo-700 text-yellow-300': route.name === 'activities' }"
              >
                Activities
              </RouterLink>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>

<style scoped></style>
