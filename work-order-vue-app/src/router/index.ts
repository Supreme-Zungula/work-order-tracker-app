import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/activities',
      name: 'activities',
      component: () => import('../views/ActivitiesView.vue'),
    },
    {
      path: '/work-orders/:id/activities',
      name: 'workOrderActivities',
      component: () => import('../views/WorkOrderActivitiesView.vue'),
      props: true,
    },
  ],
})

export default router
