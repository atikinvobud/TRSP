import { createRouter, createWebHistory } from 'vue-router'
import RegisterPage from '@/pages/RegisterPage.vue'
import LoginPage from '@/pages/LoginPage.vue'

const routes = [
  {
    path: '/register',
    component: RegisterPage,
    name: 'RegisterPage'
  },
  {
    path: '/login',
    component: LoginPage,
    name: 'LoginPage'
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router