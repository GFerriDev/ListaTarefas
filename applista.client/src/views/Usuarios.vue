<template>
  <div class="container">
    <h1>Cadastro de Usuários</h1>

    <form @submit.prevent="cadastrarUsuario">
      <label for="nome">Nome:</label>
      <input id="nome" v-model="usuario.nome" required />

      <button type="submit">Cadastrar</button>
    </form>

    <h2>Usuários cadastrados</h2>
    <ul>
      <li v-for="usuario in usuarios" :key="usuario.id" style="display: flex; align-items: center;">
        <span style="flex-grow: 1;">{{ usuario.nome }}</span>
        <button @click="excluirUsuario(usuario.id)" style="margin-left: 10px; color: red; font-weight: bold; border: none; background: none; cursor: pointer;">
          X
        </button>
      </li>
    </ul>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        usuario: {
          nome: ''
        },
        usuarios: []
      };
    },
    methods: {
      async carregarUsuarios() {
        try {
          const res = await fetch('https://localhost:7115/Usuarios');
          if (!res.ok) throw new Error('Erro ao carregar usuários');
          this.usuarios = await res.json();
        } catch (err) {
          console.error('Erro:', err);
        }
      },

      async cadastrarUsuario() {
        const nomeDigitadoConferencia = this.usuario.nome.trim().toLowerCase();

        if (!nomeDigitadoConferencia) {
          alert('Nome é obrigatório.');
          return;
        }

        const nomeJaExiste = this.usuarios.some(u => u.nome.trim().toLowerCase() === nomeDigitadoConferencia);
        if (nomeJaExiste) {
          alert('Já existe um usuário com esse nome.');
          return;
        }

        try {
          const res = await fetch('https://localhost:7115/Usuarios', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              Nome: this.usuario.nome
            })
          });

          if (!res.ok) throw new Error('Erro ao cadastrar usuário');

          this.usuario.nome = '';
          await this.carregarUsuarios();
        } catch (err) {
          console.error('Erro:', err);
        }
      },

      async excluirUsuario(id) {
        if (!confirm('Tem certeza que deseja excluir este usuário?')) return;

        try {
          const res = await fetch(`https://localhost:7115/Usuarios/${id}`, {
            method: 'DELETE'
          });

          if (!res.ok) throw new Error('Erro ao excluir usuário');

          await this.carregarUsuarios();
        } catch (err) {
          console.error('Erro:', err);
        }
      }
    },

    mounted() {
      this.carregarUsuarios();
    }
  };
</script>

<style>
  .container {
    max-width: 600px;
    margin: 2rem auto;
  }

  form {
    margin-bottom: 2rem;
  }

  input {
    padding: 0.5rem;
    margin-right: 1rem;
  }
</style>
