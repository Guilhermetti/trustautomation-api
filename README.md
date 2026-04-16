# Trust Developer — Backend API

API desenvolvida em **.NET 8** responsável pela captação, armazenamento e gerenciamento de leads da Trust Developer.

A aplicação está em produção e integrada à landing page oficial, utilizando arquitetura desacoplada e infraestrutura serverless de baixo custo.

---

## 🚀 Stack

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core 8
- PostgreSQL (Neon)
- Docker
- Render (Deploy)
- Cloudflare (proxy)

---

## 📦 Executar Localmente

```bash
dotnet restore
dotnet run
```

---

## 🔐 Variáveis de Ambiente

Obrigatórias para execução:

```
DATABASE_URL=Host=...;Port=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true
ALLOWED_ORIGIN=...
ADMIN_API_KEY=sua-chave-secreta
```

Em ambiente local, recomenda-se utilizar:

```bash
dotnet user-secrets set "DATABASE_URL" "..."
dotnet user-secrets set "ALLOWED_ORIGIN" "..."
dotnet user-secrets set "ADMIN_API_KEY" "..."
```

---

## 📌 Endpoints

### Health Check

```
GET /health
```

---

### Criar Lead

```
POST /api/leads
Content-Type: application/json
```

Body:

```json
{
  "name": "Nome",
  "email": "email@email.com",
  "company": "Empresa",
  "whatsapp": "11999999999",
  "needType": "automacao",
  "deadline": "2-4sem",
  "idea": "Descrição da necessidade",
  "consent": true,
  "sourceUrl": "https://dev.com",
  "honey": null
}
```

---

### Listar Leads (Protegido)

```
GET /api/admin/leads
Header: x-api-key: SUA_CHAVE
```

---

### Exportar Leads em CSV (Protegido)

```
GET /api/admin/leads/export.csv
Header: x-api-key: SUA_CHAVE
```

---

## 🗄 Estrutura da Tabela

Tabela: `Leads`

- Id (Guid)
- Name
- Email
- Company
- Whatsapp
- NeedType
- Deadline
- Idea
- Consent
- SourceUrl
- Ip
- CreatedAtUtc

---

## 🛡 Segurança Implementada

- CORS restrito ao domínio da landing page
- Endpoint administrativo protegido por API Key
- Rate limiting
- Validação de campos obrigatórios
- Honeypot anti-bot

---

## 🏗 Arquitetura

Cloudflare Pages (Frontend)  
↓  
Render (.NET API)  
↓  
Neon PostgreSQL  

---

## 🎯 Objetivo

Fornecer uma API leve, segura e escalável para:

- Captação estruturada de leads
- Gestão administrativa protegida
- Exportação de dados
- Base para futura expansão SaaS

---

## 📄 Licença

Uso privado — Trust Developer.
