# RestaurantAnalytics

Aplicação desenvolvida para o desafio **God-Level Coder Challenge** da Nola.  
Sistema de análise e visualização de indicadores de vendas de restaurantes, utilizando dados operacionais reais simulados (500k+ vendas).

## 🏗️ Stack Tecnológico

- **.NET 9**
- **Blazor Server** (UI interativa em tempo real)
- **MudBlazor** (Componentes visuais)
- **Dapper** (Acesso a dados)
- **PostgreSQL** (Banco de dados principal)

---

## 📦 Estrutura do Projeto

RestaurantAnalytics/
│
├── RestaurantAnalytics.Core
│ ├── Entities # Modelos de domínio
│ └── Interfaces # Contratos de repositórios e serviços
│
├── RestaurantAnalytics.Application
│ └── Services # Casos de uso, lógica de relatório
│
├── RestaurantAnalytics.Infrastructure
│ ├── Database # Conexão, configuração
│ └── Queries # Consultas otimizadas via Dapper
│
└── RestaurantAnalytics.Web
├── Components
├── Pages
└── Program.cs # Blazor + MudBlazor
