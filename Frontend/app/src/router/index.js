import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '@/views/Home.vue';
import Sensors from '@/views/Sensors.vue';
import Orders from '@/views/Orders.vue';
import Invoices from '@/views/Invoices.vue';
import ManagementPanel from '@/views/ManagementPanel.vue';

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/sensors',
    name: 'Sensors',
    component: Sensors
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Orders
  },
]

const router = new VueRouter({
  routes
})

export default router
