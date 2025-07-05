<template>
  <div class="container">
    <h1>Lista de Tarefas</h1>

    <!-- FORMULÁRIO DE NOVA TAREFA -->
    <form @submit.prevent="adicionarTarefa" class="form">
      <input v-model="novaDescricao" placeholder="Nova tarefa..." required />
      <select v-model="usuarioSelecionadoId" required>
        <option disabled value="">Selecione um usuário</option>
        <option v-for="usuario in usuarios" :key="usuario.id" :value="usuario.id">
          {{ usuario.nome }}
        </option>
      </select>
      <button type="submit" class="btn btn-primary">Adicionar</button>
    </form>

    <!-- LISTA DE PENDENTES -->
    <h2>Pendentes</h2>
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


    <!-- LISTA DE CONCLUÍDAS -->
    <h2 id="concluidas-section">Concluídas</h2>
    <ul class="task-list">
      <li v-for="tarefa in tarefasConcluidas" :key="tarefa.id" class="task-item completed">
        <span>
          {{ tarefa.descricao }}
          <br />
          <small><strong>Usuário:</strong> {{ tarefa.usuario?.nome || 'Não encontrado' }}</small>
          <br />
          <small><strong>Data de Conclusão:</strong> {{ formatarData(tarefa.dataConclusao) }}</small>
        </span>
        <button class="btn btn-danger" @click="excluirTarefa(tarefa.id)">❌</button>
      </li>
    </ul>

  </div>
</template>


<script>
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
        } catch (err) {
          console.error('Erro:', err);
        }
      },

      async carregarUsuarios() {
        try {
          const res = await fetch('https://localhost:7115/Usuarios');
          if (!res.ok) throw new Error('Erro ao carregar usuários');
          this.usuarios = await res.json();
        } catch (err) {
          console.error('Erro:', err);
        }
      },

      async adicionarTarefa() {
        if (!this.usuarioSelecionadoId) {
          alert('Selecione um usuário');
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
        } catch (err) {
          console.error('Erro:', err);
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
        } catch (err) {
          console.error('Erro:', err);
        }
      },

      async concluirTarefa(id) {
        try {
          const res = await fetch(`https://localhost:7115/Tarefas/${id}/concluir`, {
            method: 'PUT'
          });

          if (!res.ok) throw new Error('Erro ao concluir tarefa');

          await this.carregarTarefas();

          this.$nextTick(() => {
            const el = document.getElementById('concluidas-section');
            if (el) el.scrollIntoView({ behavior: 'smooth' });
          });
        } catch (err) {
          console.error('Erro:', err);
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
