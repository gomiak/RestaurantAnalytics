# RestaurantAnalytics

Aplicação desenvolvida para o desafio **God-Level Coder Challenge** da Nola.  
Sistema de análise e visualização de indicadores de vendas de restaurantes, utilizando dados operacionais reais simulados (500k+ vendas).

## Stack Tecnológico

- **.NET 9**
- **Blazor Server** (UI interativa em tempo real)
- **MudBlazor** (Componentes visuais)
- **Dapper** (Acesso a dados)
- **PostgreSQL** (Banco de dados principal)

---

## Estrutura do Projeto

```plaintext
RestaurantAnalytics/
│
├── RestaurantAnalytics.Core
│   ├── Entities              # Modelos de domínio (Store, Sale, ProductSale, etc.)
│   └── Interfaces            # Contratos de repositórios
│
├── RestaurantAnalytics.Application
│   └── Services              # Casos de uso, lógica de relatórios
│
├── RestaurantAnalytics.Infrastructure
│   ├── Database              # Conexão (Npgsql / Dapper)
│   └── Queries               # Consultas SQL otimizadas
│
└── RestaurantAnalytics.Web
    ├── Components            # Gráficos, filtros, tabelas
    ├── Pages                 # Páginas
    └── Program.cs            # Configuração Blazor + MudBlazor
