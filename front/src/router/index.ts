import { createRouter, createWebHistory } from 'vue-router'
import RegisterPage from '@/pages/RegisterPage.vue'
import LoginPage from '@/pages/LoginPage.vue'
import NotificationsPage from '@/pages/NotificationsPage.vue'

const CreateLotPage = () => import('@/pages/CreateLotPage.vue')
const MainPage = () => import('@/pages/MainPage.vue')
const MyLotsPage = () => import('@/pages/MyLotsPage.vue')
const MyBidsPage = () => import('@/pages/MyBidsPage.vue')
const LotPage = () => import('@/pages/LotPage.vue')

const routes = [
  {
    path: '/register',
    component: RegisterPage,
    name: 'RegisterPage',
  },
  {
    path: '/login',
    component: LoginPage,
    name: 'LoginPage',
  },
  {
    path: '/',
    component: MainPage,
    name: 'MainPage',
  },
  {
    path: '/lots/create',
    component: CreateLotPage,
    name: 'CreateLotPage',
  },
  {
    path: '/my-lots',
    component: MyLotsPage,
    name: 'MyLotsPage',
  },
  {
    path: '/my-bids',
    component: MyBidsPage,
    name: 'MyBidsPage',
  },
  {
    path: '/notifications',
    component: NotificationsPage,
    name: 'NotificationsPage',
  },
  {
    path: '/lots/:id',
    component: LotPage,
    name: 'LotPage',
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
