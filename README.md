# 🚀 IncidentManagement.API

![.NET](https://img.shields.io/badge/.NET-10-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-purple)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-blue)
![JWT](https://img.shields.io/badge/Auth-JWT-green)
![Deploy](https://img.shields.io/badge/Deploy-Render-black)

API REST desenvolvida em *.NET 10* para gerenciamento de lojas, usuários e indicadores financeiros,
com autenticação JWT, auditoria automática de ações e arquitetura backend moderna em camadas.

---

## 🌐 API Online

Base URL:


https://incidentmanagement-api.onrender.com


Swagger:


https://incidentmanagement-api.onrender.com/swagger


---

## 📌 Sobre o Projeto

O *IncidentManagement.API* centraliza o gerenciamento operacional de lojas,
controle de usuários e indicadores financeiros.

A aplicação foi construída para simular um ambiente corporativo real,
com foco em:

- Segurança
- Auditoria
- Rastreabilidade
- Separação de responsabilidades
- Arquitetura limpa

---

## 🧱 Arquitetura


IncidentManagement.API
│
├── Controllers        → Endpoints HTTP
├── Application
│   └── Services       → Regras de negócio
├── Domain
│   ├── Entities       → Modelos do domínio
│   └── Enums          → Tipagens
├── Infrastructure
│   ├── Data           → DbContext / EF Core
│   └── Repositories   → Acesso ao banco
├── Middleware         → Auditoria + Exceptions
└── Program.cs         → Configuração da aplicação


---

## ⚙️ Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger (OpenAPI)
- BCrypt
- Clean Architecture
- Middleware Customizado
- Auditoria Estruturada
- Deploy via Render

---

## 🔐 Autenticação

A API utiliza *JWT (JSON Web Token)*.

### Fluxo

1. Login via /api/Auth/login
2. Recebe:
   - Access Token
   - Refresh Token
3. Enviar token nos endpoints protegidos:


Authorization: Bearer SEU_TOKEN


---

## 👤 Controle de Acesso

Sistema hierárquico de usuários:

- SuperAdmin
- Admin
- User

Apenas usuários autorizados podem:

- Criar usuários
- Alterar roles
- Executar ações administrativas

---

## 📡 Principais Endpoints

### 🔑 Auth

| Método | Endpoint |
|--------|----------|
| POST | /api/Auth/register |
| POST | /api/Auth/login |
| POST | /api/Auth/refresh |

---

### 👤 Users

| Método | Endpoint |
|--------|----------|
| PUT | /api/users/{username}/role |

Exemplo de body:


{
  "role": "Admin",
  "reason": "Promoção por mérito"
}


---

### 🏪 Store

| Método | Endpoint |
|--------|----------|
| GET | /api/Store |
| GET | /api/Store/{id} |

---

### 📊 Dashboard

| Método | Endpoint |
|--------|----------|
| GET | /api/Dashboard/summary |

---

### 💰 Financial

| Método | Endpoint |
|--------|----------|
| POST | /api/Financial/seed |
| GET | /api/Financial/store/{storeId} |
| GET | /api/Financial/store/{storeId}/growth |
| GET | /api/Financial/store/{storeId}/metrics |

---

## 🧠 Sistema de Auditoria

A aplicação possui um *Audit Middleware automático* que registra:

- Usuário responsável
- Endpoint acessado
- Método HTTP
- StatusCode
- Success (true/false)
- ErrorMessage (se houver)
- Reason (motivo da alteração)
- Source (API ou BusinessRule)
- Timestamp

---

### 🔎 Classificação de Logs

| Source | Significado |
|--------|-------------|
| API | Registro automático da chamada HTTP |
| BusinessRule | Alteração crítica de regra de negócio |

---

### 📄 Exemplo de Log


{
  "action": "PUT /api/users/testuser2/role",
  "performedBy": "admin",
  "statusCode": 200,
  "success": true,
  "reason": "Promoção interna",
  "source": "BusinessRule"
}


---

## 🧠 Middleware Global

### Exception Middleware

- Captura exceções não tratadas
- Retorna resposta padronizada
- Evita exposição de stack trace

Exemplo:


{
  "status": 500,
  "message": "Erro interno no servidor."
}


---

## 🗄️ Banco de Dados

Banco:


PostgreSQL


ORM:


Entity Framework Core


Executar migrations:


dotnet ef database update


Connection String configurada em:


appsettings.json
appsettings.Production.json


---

## 🐳 Docker

A aplicação é compatível com Docker.

Build:


docker build -t incidentmanagement-api .


Run:


docker run -p 8080:8080 incidentmanagement-api


---

## ▶️ Executando Localmente

Clonar:


git clone https://github.com/seu-usuario/IncidentManagement.API.git


Entrar na pasta:


cd IncidentManagement.API


Restaurar:


dotnet restore


Aplicar migrations:


dotnet ef database update


Rodar:


dotnet run


---

## 🔄 Fluxo da Aplicação


Request
   ↓
Audit Middleware
   ↓
Exception Middleware
   ↓
Controller
   ↓
Service
   ↓
Repository
   ↓
Database
   ↓
Response


---

## 🏗 Boas Práticas Aplicadas

- Clean Architecture
- Repository Pattern
- Service Layer
- JWT Security
- Auditoria estruturada
- Validação de entrada
- Controle hierárquico de usuários
- Separação de ambientes (Development / Production)
- Deploy Cloud

---

## 📈 Melhorias Futuras

- Rate Limiting
- Cache Redis
- Testes unitários
- OpenTelemetry
- CI/CD avançado
- Monitoramento
- Logs estruturados externos

---

## 👨‍💻 Autor

Guilherme Nazario  
Backend Developer | .NET | APIs REST | Cloud | Arquitetura Backend
