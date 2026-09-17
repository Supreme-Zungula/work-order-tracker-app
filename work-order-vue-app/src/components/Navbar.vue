<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <header>
    <nav class="bg-indigo-600 font-bold text-green-500 shadow-md">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex items-center justify-between h-16">
          <div class="flex items-center">
            <RouterLink to="/" class="text-2xl font-bold text-amber-600 hover:text-amber-400 transition-colors">
              Work Order Tracker
            </RouterLink>
          </div>
          <div class="hidden md:block">
            <ul class="flex items-center space-x-4">
              <li>
                <RouterLink
                  to="/"
                  class="px-3 py-2 rounded-md text-sm font-medium transition-colors hover:bg-indigo-700"
                  :class="{ 'bg-indigo-700': route.name === 'home' }"
                >
                  Home
                </RouterLink>
              </li>
              <li>
                <RouterLink
                  to="/about"
                  class="px-3 py-2 rounded-md text-sm font-medium transition-colors hover:bg-indigo-700"
                  :class="{ 'bg-indigo-700 text-yellow-300': route.name === 'about' }"
                >
                  About
                </RouterLink>
              </li>
              <li>
                <RouterLink
                  to="/activities"
                  class="px-3 py-2 rounded-md text-sm font-medium transition-colors hover:bg-indigo-700"
                  :class="{ 'bg-indigo-700 text-yellow-300': route.name === 'activities' }"
                >
                  Activities
                </RouterLink>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </nav>
  </header>
</template>

<style scoped>
</style>
