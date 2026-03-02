# 🚀 IncidentManagement.API

API REST desenvolvida em *.NET 10* para gerenciamento de lojas, usuários e indicadores financeiros, com autenticação JWT e arquitetura em camadas.

---

## 📌 Sobre o Projeto

O *IncidentManagement.API* é uma API backend criada para centralizar o gerenciamento de informações de lojas, incluindo:

* Cadastro e autenticação de usuários
* Consulta de lojas
* Indicadores financeiros
* Dashboard resumido
* Controle de acesso via JWT
* Tratamento global de exceções

O projeto segue boas práticas modernas de desenvolvimento backend utilizando *Clean Architecture simplificada*, separando responsabilidades entre Controllers, Services e Repositories.

---

## 🧱 Arquitetura

O projeto está organizado em camadas:


IncidentManagement.API
│
├── Controllers        → Endpoints da API
├── Application
│   └── Services       → Regras de negócio
├── Infrastructure
│   ├── Data           → DbContext
│   └── Repositories   → Acesso ao banco
├── Middleware         → Tratamento global de erros
└── Program.cs         → Configuração da aplicação


---

## ⚙️ Tecnologias Utilizadas

* ✅ .NET 10
* ✅ ASP.NET Core Web API
* ✅ Entity Framework Core
* ✅ PostgreSQL
* ✅ JWT Authentication
* ✅ Swagger (OpenAPI)
* ✅ Dependency Injection
* ✅ Middleware de Exception Handling
* ✅ BCrypt (Hash de senha)

---

## 🔐 Autenticação

A API utiliza *JWT (JSON Web Token)* para autenticação.

Fluxo:

1. Usuário realiza login
2. API retorna:

   * Access Token
   * Refresh Token
3. Token deve ser enviado nos endpoints protegidos:


Authorization: Bearer SEU_TOKEN


---

## 📡 Principais Endpoints

### 🔑 Auth

| Método | Endpoint             | Descrição         |
| ------ | -------------------- | ----------------- |
| POST   | /api/Auth/register | Registrar usuário |
| POST   | /api/Auth/login    | Login             |
| POST   | /api/Auth/refresh  | Renovar token     |

---

### 🏪 Store

| Método | Endpoint          | Descrição          |
| ------ | ----------------- | ------------------ |
| GET    | /api/Store      | Listar lojas       |
| GET    | /api/Store/{id} | Buscar loja por ID |

---

### 📊 Dashboard

| Método | Endpoint                 | Descrição    |
| ------ | ------------------------ | ------------ |
| GET    | /api/Dashboard/summary | Resumo geral |

---

### 💰 Financial

| Método | Endpoint                                 | Descrição                 |
| ------ | ---------------------------------------- | ------------------------- |
| POST   | /api/Financial/seed                    | Popular dados financeiros |
| GET    | /api/Financial/store/{storeId}         | Dados financeiros         |
| GET    | /api/Financial/store/{storeId}/growth  | Crescimento               |
| GET    | /api/Financial/store/{storeId}/metrics | Métricas                  |

---

## 🧠 Middleware de Exceções

A aplicação possui um middleware global responsável por:

* Capturar exceções não tratadas
* Retornar respostas padronizadas
* Evitar exposição de erros internos

Exemplo de resposta:

json
{
  "status": 500,
  "message": "Ocorreu um erro interno no servidor."
}


---

## 🗄️ Banco de Dados

Banco utilizado:


PostgreSQL


ORM:


Entity Framework Core


Connection String configurada em:


appsettings.json


---

## ▶️ Executando o Projeto

### 1️⃣ Clonar repositório

bash
git clone https://github.com/seu-usuario/IncidentManagement.API.git


---

### 2️⃣ Entrar na pasta

bash
cd IncidentManagement.API


---

### 3️⃣ Restaurar dependências

bash
dotnet restore


---

### 4️⃣ Executar migrations (se aplicável)

bash
dotnet ef database update


---

### 5️⃣ Rodar API

bash
dotnet run


---

## 📘 Documentação Swagger

Após iniciar o projeto:


https://localhost:7061/swagger


Permite:

✅ Testar endpoints
✅ Visualizar contratos
✅ Simular requisições

---

## 🔄 Fluxo Geral da Aplicação


Request
   ↓
Controller
   ↓
Service (Regra de negócio)
   ↓
Repository
   ↓
Database
   ↓
Response


---

## ✅ Boas Práticas Aplicadas

* Separação de responsabilidades
* Injeção de dependência
* Repository Pattern
* Service Layer Pattern
* Tratamento global de erros
* Autenticação segura
* Hash de senha
* API documentada

---

## 📈 Possíveis Melhorias Futuras

* ✅ Roles e Policies
* ✅ Logs estruturados
* ✅ Cache Redis
* ✅ Testes unitários
* ✅ CI/CD Pipeline
* ✅ Dockerização
* ✅ Rate Limiting

---

## 👨‍💻 Autor

Desenvolvido por *Guilherme Nazario*

Backend Developer | .NET | APIs REST | Automação

---
