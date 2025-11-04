# RestaurantAnalytics

Sistema de análise e visualização de dados para restaurantes.  
Permite acompanhar vendas, produtos mais vendidos, canais de venda, clientes e métricas gerais de desempenho.  
O projeto foi desenvolvido como um exercício de arquitetura, UI e manipulação de dados com foco em **aprendizado real**.

---

## Objetivo

Aprender e aplicar:

- **Blazor Server** para UI interativa
- **MudBlazor** para uma interface moderna e consistente
- **Dapper + PostgreSQL** para acesso rápido e eficiente a dados
- **Clean Architecture (versão simplificada e prática)**
- Deploy usando **Render.com**
- **IA com Groq API para geração de insights**
- **Ollama para desenvolvimento local** (quando desejado)

---

## Tecnologias
```plaintext
| Tecnologia | Função |
|---|---|
| C# 9 / .NET 9 | Backend + UI |
| Blazor Server | Interface Web |
| MudBlazor | Componentes visuais |
| Dapper | Acesso a banco |
| PostgreSQL (Render) | Banco de dados |
| Docker | Containerização |
| Render.com | Hospedagem |
| **Groq API** | IA em produção |
| **Ollama (opcional)** | IA em desenvolvimento local |
```
---

## Arquitetura (Clean-ish)

```plaintext
RestaurantAnalytics.sln
│
├── RestaurantAnalytics.Core/           → Entidades e contratos
├── RestaurantAnalytics.Infrastructure/ → Repositórios (Dapper), Queries SQL
├── RestaurantAnalytics.Application/    → Services + Models
└── RestaurantAnalytics.Web/            → UI Blazor  + Componentes
```

Fluxo:

```plaintext
UI (Blazor)
   → Serviços
       → Repositórios (Dapper)
           → PostgreSQL
```

---

## Autenticação

Login simples baseado em variável de ambiente.  
Foco do projeto é a visualização e análise de dados, não implementação de um sistema de auth completo.

---

## Dashboards

- Página principal com gráficos padrão
- Página de **dashboard customizado**, onde o usuário pode selecionar:
  - Métricas
  - Intervalos de data
  - Tipos de gráficos
  - Canal de venda

---

## IA para Insights

### Em Produção
A aplicação utiliza a **Groq API** para gerar insights sobre vendas.

Adicionar no Render:

| Key | Descrição |
|---|---|
| `AI__ApiKey` | Chave de acesso à API da Groq |

Modelo recomendado:
- `llama-3.1-8b-instant`

### Em Desenvolvimento Local (Opcional)
O projeto pode usar **Ollama** local para desenvolvimento e testes sem custo.

Exemplo:
```
ollama run gemma2:2b "quais insights posso extrair dessas vendas?"
```

---

## Deploy

Feito usando:

```
render.yaml + Dockerfile
```

Variáveis obrigatórias:

| Key | Descrição |
|---|---|
| `DB_CONNECTION` | Connection string do PostgreSQL |
| `AI__ApiKey` | Chave para IA em produção |
| `AI__BaseUrl` | URL para IA em produção |
| `AI__Model` | Model para IA em produção |

---

## Motivação

Esse projeto foi feito como **desafio de aprendizado**, com foco em:

- UI moderna com Blazor + MudBlazor
- Arquitetura clara e separada por camadas
- Aplicação real conectada a banco online
- Aplicação de IA para insights automáticos

---

## Autor

**Felipe Kubiak**  
https://github.com/gomiak
