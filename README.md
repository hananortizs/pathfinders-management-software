# Pathfinder Management System (PMS)

Sistema de gerenciamento para clubes de desbravadores, desenvolvido com ASP.NET Core e PostgreSQL.

## 🚀 Início Rápido

### Pré-requisitos

- .NET 8.0 SDK
- Docker Desktop
- PostgreSQL (via Docker)

### Executando o Projeto

1. **Clone o repositório**

```bash
git clone <repository-url>
cd PathfinderManagementProject
```

2. **Execute o banco de dados**

```bash
docker-compose up -d
```

3. **Execute a aplicação**

```bash
cd src/Pms.Backend.Api
dotnet run
```

4. **Acesse a API**

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

## 📚 Documentação

### 📖 Guias do Usuário

- [README Completo](docs/user-guides/README.md) - Documentação completa do projeto
- [Progresso do Desenvolvimento](docs/user-guides/PROGRESS.md) - Status atual do desenvolvimento
- [Critérios de Aceitação](docs/user-guides/ACCEPTANCE.md) - Requisitos e critérios de aceitação

### 🔧 Documentação Técnica

- [Padronização de CEP](docs/technical/CEP_STANDARDIZATION.md) - Padrões para CEPs brasileiros
- [Arquitetura do Sistema](docs/architecture/) - Documentação arquitetural (em desenvolvimento)

### 🌐 Documentação da API

- [Integração Frontend](docs/api/FRONTEND_INTEGRATION_EXAMPLE.md) - Guia para integração com frontend
- [Swagger UI](http://localhost:5000/swagger) - Documentação interativa da API

## 🏗️ Arquitetura

### Estrutura do Projeto (Monorepo)

```
pathfinders-management-software/
├── src/
│   ├── backend/                    # Backend .NET
│   │   ├── Pms.Backend.Api/        # Camada de API (Controllers, Middleware)
│   │   ├── Pms.Backend.Application/ # Camada de Aplicação (Services, DTOs, Interfaces)
│   │   ├── Pms.Backend.Domain/     # Camada de Domínio (Entities, Enums, Helpers)
│   │   ├── Pms.Backend.Infrastructure/ # Camada de Infraestrutura (Data, Repositories)
│   │   ├── tests/                  # Testes do Backend
│   │   └── Pms.Backend.sln         # Solution File
│   └── frontend/                   # Frontend React (em desenvolvimento)
├── docs/                          # Documentação compartilhada
├── scripts/                       # Scripts compartilhados
├── docker-compose.yml             # Orquestração de containers
├── docker-compose.dev.yml         # Desenvolvimento local
└── package.json                   # Workspace root
```

### Tecnologias Utilizadas

- **Backend**: ASP.NET Core 8.0
- **Banco de Dados**: PostgreSQL
- **ORM**: Entity Framework Core
- **Mapeamento**: AutoMapper
- **Validação**: Data Annotations + Custom Validators
- **Containerização**: Docker

## 🚀 Funcionalidades

### ✅ Implementadas

- Sistema de hierarquia (Região, Associação, Distrito, União, Divisão, Igreja, Clube, Unidade)
- Gerenciamento de membros
- Sistema de endereços centralizado
- Autenticação e autorização
- Validação robusta de dados
- Padronização de CEPs brasileiros

### 🔄 Em Desenvolvimento

- Sistema de investiduras
- Relatórios e exportação
- Sistema de tarefas e eventos

## 🛠️ Desenvolvimento

### Comandos Úteis

#### Desenvolvimento Local
```bash
# Instalar dependências
npm run install:all

# Desenvolvimento (backend + frontend)
npm run dev

# Apenas backend
npm run dev:backend

# Apenas frontend
npm run dev:frontend

# Docker (banco + API)
npm run docker:up

# Docker desenvolvimento (apenas banco)
npm run docker:dev
```

#### Backend (.NET)
```bash
# Executar migrações
cd src/backend && dotnet ef database update

# Criar nova migração
cd src/backend && dotnet ef migrations add NomeDaMigracao

# Executar testes
cd src/backend && dotnet test

# Build do projeto
cd src/backend && dotnet build
```

### Padrões de Código

- **Nomenclatura**: PascalCase para classes, camelCase para variáveis
- **Documentação**: XML Documentation para métodos públicos
- **Validação**: Data Annotations + Custom Validators
- **Mapeamento**: AutoMapper profiles

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Suporte

Para suporte, entre em contato através de:

- **Email**: [seu-email@exemplo.com]
- **Issues**: [GitHub Issues](https://github.com/seu-usuario/pathfinder-management/issues)

---

**Desenvolvido com ❤️ para a comunidade de desbravadores**
