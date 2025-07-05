<template>
  <div>
    <header class="app-header">
      <nav class="nav-container">
        <h1 class="logo">📋 Lista de Tarefas</h1>
        <div class="nav-buttons">
          <router-link to="/" class="nav-btn">Tarefas</router-link>
          <router-link to="/usuarios" class="nav-btn">Usuários</router-link>
        </div>
      </nav>
    </header>


    <main>
      <router-view />
    </main>
  </div>
</template>
