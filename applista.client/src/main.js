import './assets/style/geral.css'
import './assets/style/header.css'

import { createApp } from 'vue';
import App from './App.vue';
import router from './router';  

createApp(App)
  .use(router)  
  .mount('#app');  
