# TaskMgmt.API

## Visão Geral

TaskMgmt.API é uma aplicação ASP.NET Core 8 para gerenciamento de tarefas, utilizando SQLite como banco de dados e Swagger para documentação da API.

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (opcional, para rodar via container)
- [Git](https://git-scm.com/) (para clonar o repositório)

---

## Configuração Local

1. **Clone o repositório:**
git clone https://github.com/thiagom3005/TaskMgmt cd <PASTA_DO_PROJETO>
2. **Configuração do Banco de Dados:**
- O projeto já está configurado para usar SQLite.
- O arquivo de configuração está em `TaskMgmt.API/appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=taskmgmt.db"
  }
  ```

3. **Restaurar dependências:**
dotnet restore

4. **Rodar as migrações e popular o banco:**
   - O banco será migrado e populado automaticamente ao iniciar a aplicação.

5. **Executar a aplicação:**
dotnet run --project TaskMgmt.API

6. **Acessar a documentação Swagger:**
   - [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - Ou [http://localhost:8081/swagger](http://localhost:8081/swagger) (dependendo da porta configurada)

---

## Executando com Docker

1. **Build da imagem:**
docker build -t taskmgmt-api -f TaskMgmt.API/Dockerfile .

2. **Rodar o container:**
docker run -d -p 8080:8080 -p 8081:8081 --name taskmgmt-api taskmgmt-api

3. **Acessar o Swagger:**
   - [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - Ou [http://localhost:8081/swagger](http://localhost:8081/swagger)

---

## Estrutura do Projeto

- `TaskMgmt.API` - API principal (controllers, configuração)
- `TaskMgmt.Application` - Serviços de aplicação
- `TaskMgmt.Domain` - Entidades, enums, interfaces e validadores
- `TaskMgmt.Infrastructure` - Repositórios e contexto de dados

---

## Testes

Para rodar os testes automatizados:
dotnet test

---

## Observações

- O banco de dados será criado e populado automaticamente com mais de 1000 tarefas na primeira execução.
- O Swagger está habilitado em todos os ambientes para facilitar o uso e testes da API.

---
