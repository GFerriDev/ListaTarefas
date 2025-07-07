<template>
  <div class="container">
    <h1>Lista de Tarefas</h1>

    <!-- FORMULÁRIO DE NOVA TAREFA -->
    <form @submit.prevent="adicionarTarefa" class="form">
      <label>Descrição:</label>
      <input v-model="novaDescricao" class="input-desc" placeholder="Nova tarefa..." required />
      <select v-model="usuarioSelecionadoId" required>
        <option disabled value="">Selecione um usuário</option>
        <option v-for="usuario in usuarios" :key="usuario.id" :value="usuario.id">
          {{ usuario.nome }}
        </option>
      </select>
      <button type="submit" class="btn-primary">Cadastrar</button>
    </form>

    <!-- LISTA DE PENDENTES -->
    <h2>Pendentes</h2>
    <div class="task-container">
      <ul class="task-list">
        <li v-for="tarefa in tarefasPendentes" :key="tarefa.id" class="task-item">
          <input type="text"
                v-model="tarefa.descricao"
                @change="editarTarefa(tarefa)"
                class="task-input" />
          <select v-model="tarefa.usuarioId" @change="editarTarefa(tarefa)">
            <option v-for="usuario in usuarios" :key="usuario.id" :value="usuario.id">
              {{ usuario.nome }}
            </option>
          </select>

          <div class="actions">
            <button class="btn btn-success" @click="concluirTarefa(tarefa.id)">✔️</button>
            <button class="btn btn-danger" @click="excluirTarefa(tarefa.id)">❌</button>
          </div>
        </li>
      </ul>
    </div>


    <!-- LISTA DE CONCLUÍDAS -->
    <h2 id="concluidas-section">Concluídas</h2>
    <div class="task-container">
      <ul class="task-list">
        <li v-for="tarefa in tarefasConcluidas" :key="tarefa.id" class="task-item completed">
          <span>
            {{ tarefa.descricao }}
            <br />
            <small><strong>Usuário:</strong> {{ tarefa.usuario?.nome || 'Não encontrado' }}</small>
            <br />
            <small><strong>Data de Conclusão:</strong> {{ formatarData(tarefa.dataConclusao) }}</small>
          </span>
          <div class="actions">
            <button class="btn btn-warning" @click="reabrirTarefa(tarefa.id)">🔁</button>
            <button class="btn btn-danger" @click="excluirTarefa(tarefa.id)">❌</button>
        </div>
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
      novaDescricao: '',
      usuarioSelecionadoId: '',
      tarefas: [],
      usuarios: []
    };
  },
  computed: {
    tarefasPendentes() {
      return this.tarefas.filter(t => !t.flConcluida);
    },
    tarefasConcluidas() {
      return this.tarefas.filter(t => t.flConcluida);
    }
  },
  methods: {
    async carregarTarefas() {
      try {
        const res = await fetch('https://localhost:7115/Tarefas');
        if (!res.ok) throw new Error('Erro ao carregar tarefas');
        this.tarefas = await res.json();
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao carregar tarefas!');
      }
    },

    async excluirTarefa(id) {
      if (!confirm('Tem certeza que deseja excluir esta tarefa?')) return;
      try {
        const res = await fetch(`https://localhost:7115/Tarefas/${id}`, {
          method: 'DELETE'
        });
        if (!res.ok) throw new Error('Erro ao excluir tarefa');
        await this.carregarTarefas();
        toastr.success('Tarefa excluída com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao excluir tarefa!');
      }
    },

    async reabrirTarefa(id) {
    try {
      const res = await fetch(`https://localhost:7115/Tarefas/${id}/reabrir`, {
        method: 'PUT'
      });

      if (!res.ok) throw new Error('Erro ao reabrir tarefa');

      await this.carregarTarefas();
      toastr.success('Tarefa reaberta com sucesso!');
    } catch (err) {
      console.error('Erro:', err);
      toastr.error('Erro ao reabrir tarefa!');
    }
},

    async carregarUsuarios() {
      try {
        const res = await fetch('https://localhost:7115/Usuarios');
        if (!res.ok) throw new Error('Erro ao carregar usuários');
        this.usuarios = await res.json();
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao carregar usuários!');
      }
    },

    async adicionarTarefa() {
      if (!this.usuarioSelecionadoId) {
        toastr.warning('Selecione um usuário');
        return;
      }

      if (!this.novaDescricao.trim()) {
        toastr.warning('Descrição da tarefa é obrigatória');
        return;
      }

      const tarefa = {
        Descricao: this.novaDescricao,
        DataCriacao: new Date().toISOString(),
        DataConclusao: null,
        FlConcluida: false,
        UsuarioId: this.usuarioSelecionadoId
      };

      try {
        const res = await fetch('https://localhost:7115/Tarefas', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(tarefa)
        });

        if (!res.ok) throw new Error('Erro ao adicionar tarefa');

        this.novaDescricao = '';
        this.usuarioSelecionadoId = '';
        await this.carregarTarefas();
        toastr.success('Tarefa adicionada com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao adicionar tarefa!');
      }
    },

    async editarTarefa(tarefa) {
      try {
        const res = await fetch(`https://localhost:7115/Tarefas/${tarefa.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(tarefa)
        });

        if (!res.ok) throw new Error('Erro ao editar tarefa');

        toastr.success('Tarefa editada com sucesso!');
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao editar tarefa!');
      }
    },

    async concluirTarefa(id) {
      try {
        const res = await fetch(`https://localhost:7115/Tarefas/${id}/concluir`, {
          method: 'PUT'
        });

        if (!res.ok) throw new Error('Erro ao concluir tarefa');

        await this.carregarTarefas();
        toastr.success('Tarefa concluída com sucesso!');

        this.$nextTick(() => {
          const el = document.getElementById('concluidas-section');
          if (el) el.scrollIntoView({ behavior: 'smooth' });
        });
      } catch (err) {
        console.error('Erro:', err);
        toastr.error('Erro ao concluir tarefa!');
      }
    },

    formatarData(data) {
      if (!data) return '';
      return new Date(data).toLocaleDateString();
    }
  },
  mounted() {
    this.carregarTarefas();
    this.carregarUsuarios();
  }
};
</script>
