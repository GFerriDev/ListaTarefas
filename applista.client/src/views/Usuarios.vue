<template>
  <div class="container">
    <h1>Lista de Usuários</h1>

    <form @submit.prevent="cadastrarUsuario">
      <label for="nome">Nome:</label>
      <input id="nome" v-model="usuario.nome" required class="input-desc" placeholder="Novo Usuário..." />
      <button type="submit">Cadastrar</button>
    </form>

    <h2>Usuários cadastrados</h2>
    <div class="user-container">
      <ul>
        <li v-for="usuario in usuarios" :key="usuario.id" class="task-item">
          <input
            type="text"
            v-model="usuario.nomeTemp"
            @change="editarUsuario(usuario)"
            class="task-input"
          />
          <button @click="excluirUsuario(usuario.id)" class="btn btn-danger">❌</button>
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
import toastr from 'toastr';
import 'toastr/build/toastr.min.css';

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

        const data = await res.json();
        this.usuarios = data.map(u => ({
          ...u,
          nomeTemp: u.nome
        }));

      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao carregar usuários!');
      }
    },

    async cadastrarUsuario() {
      const nomeDigitadoConferencia = this.usuario.nome.trim().toLowerCase();
      if (!nomeDigitadoConferencia) {
        toastr.warning('Nome é obrigatório.');
        return;
      }

      const nomeJaExiste = this.usuarios.some(
        u => u.nome.trim().toLowerCase() === nomeDigitadoConferencia
      );
      if (nomeJaExiste) {
        toastr.warning('Já existe um usuário com esse nome.');
        return;
      }

      try {
        const res = await fetch('https://localhost:7115/Usuarios', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ Nome: this.usuario.nome })
        });

        if (!res.ok) throw new Error('Erro ao cadastrar usuário');

        this.usuario.nome = '';
        await this.carregarUsuarios();
        toastr.success('Usuário cadastrado com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao cadastrar usuário!');
      }
    },

    async editarUsuario(usuario) {
      const nomeAtualizado = usuario.nomeTemp.trim();
      if (!nomeAtualizado) {
        toastr.warning('Nome não pode ser vazio.');
        return;
      }

      const nomeJaExiste = this.usuarios.some(
        u => u.id !== usuario.id && u.nome.trim().toLowerCase() === nomeAtualizado.toLowerCase()
      );

      if (nomeJaExiste) {
        toastr.warning('Já existe um usuário com esse nome.');
        usuario.nomeTemp = usuario.nome;
        return;
      }

      try {
        const res = await fetch(`https://localhost:7115/Usuarios/${usuario.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ ...usuario, nome: nomeAtualizado })
        });

        if (!res.ok) throw new Error('Erro ao editar usuário');

        usuario.nome = nomeAtualizado;
        toastr.success('Usuário editado com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao editar usuário!');
        usuario.nomeTemp = usuario.nome;
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
        toastr.success('Usuário excluído com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao excluir usuário!');
      }
    }
  },

  mounted() {
    this.carregarUsuarios();
  }
};
</script>
