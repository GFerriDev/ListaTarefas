<template>
    <header class="app-header">
      <nav class="nav-container">
        <h3 class="logo">📋 Organizador de Tarefas</h3>
        <div class="nav-buttons">
          <router-link to="/" class="nav-btn">Tarefas</router-link>
          <router-link to="/usuarios" class="nav-btn">Usuários</router-link>
        </div>
      </nav>
    </header>


    <main class="container-main">
      <router-view />
    </main>

    <footer class="app-footer">
      <p>© {{ new Date().getFullYear() }} - Desenvolvido por <a href="https://github.com/GFerriDev" target="_blank" rel="noopener">Giovani André Ferri</a></p>
    </footer>
</template>
