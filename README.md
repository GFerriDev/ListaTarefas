![image](https://github.com/user-attachments/assets/f96a1d0d-87f4-4253-b3ca-89dd80df0115)
---
![image](https://github.com/user-attachments/assets/f00cd642-796a-40cb-bdac-27af087d5df1)


# 📋 Lista de Tarefas

Este é um projeto simples de **cadastro e gerenciamento de tarefas**, com:

- ✨ **Frontend** em [Vue 3](https://vuejs.org/)
- 🚀 **Backend** em [ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- 💾 Integração com banco de dados via **Entity Framework Core**

---

## 📁 Estrutura do Projeto

- `appLista.client` → Aplicação Web em Vue 3 (interface do usuário)
- `appLista.server` → API ASP.NET Core (serviço e persistência de dados)

---

## ⚙️ Como rodar o projeto

### 🔷 Frontend - `appLista.client`

```bash
cd appLista.client
npm install       # Instala as dependências
npm start         # Inicia o servidor de desenvolvimento (Vite)
```


### 🔶 Backend - `appLista.server`

```bash
cd appLista.server
dotnet build                         # Compila o projeto
dotnet ef migrations add InitialCreate   # Cria a migration inicial
dotnet ef database update                # Aplica a migration no banco de dados
dotnet run                              # Inicia a API
```


## 📝 Funcionalidades

- Criar, editar e excluir tarefas
- Atribuir tarefas a usuários
- Marcar tarefas como concluídas
- Reabrir tarefas concluídas
- Persistência via banco de dados
