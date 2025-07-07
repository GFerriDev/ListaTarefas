import { createRouter, createWebHistory } from 'vue-router';
import Tarefas from '../views/Tarefas.vue';
import Usuarios from '../views/Usuarios.vue';


const routes = [
  { path: '/', name: 'Tarefas', component: Tarefas },
  { path: '/usuarios', name: 'Usuarios', component: Usuarios }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
