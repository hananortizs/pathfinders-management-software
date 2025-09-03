# Pathfinder Management Software - Backend API

Sistema de gerenciamento para clubes de desbravadores, desenvolvido em ASP.NET Core .NET 8 com PostgreSQL e Docker.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue.svg)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Compose-blue.svg)](https://docs.docker.com/compose/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## 🚀 Execução Rápida

### Pré-requisitos
- .NET 8 SDK
- Docker Desktop
- Git

### 1. Clone o repositório
```bash
git clone git@github.com:hananortizs/pathfinders-management-software.git
cd pathfinders-management-software
```

### 2. Execute com Docker Compose
```bash
# Iniciar PostgreSQL
docker-compose up postgres -d

# Executar a API
dotnet run --project src/Pms.Backend.Api
```

### 3. Acesse a API
- **Swagger UI**: http://localhost:5000/swagger
- **API Base URL**: http://localhost:5000/pms
- **Health Check**: http://localhost:5000/health

## 🛠️ Desenvolvimento Local

### 1. Configure o banco de dados
```bash
# Inicie apenas o PostgreSQL
docker-compose up postgres -d

# Verificar se está rodando
docker ps
```

### 2. Restaure as dependências
```bash
dotnet restore
```

### 3. Execute a aplicação
```bash
# Desenvolvimento (porta 5000)
dotnet run --project src/Pms.Backend.Api

# Ou usando Docker (porta 5001)
docker-compose up
```

### 4. Execute os testes
```bash
# Todos os testes
dotnet test

# Testes de aceite
dotnet test tests/Pms.Backend.Tests
```

### 5. Configuração de Portas
- **Desenvolvimento**: http://localhost:5000
- **Docker**: http://localhost:5001
- **PostgreSQL**: localhost:5432

## 📁 Estrutura do Projeto

```
src/
├── Pms.Backend.Domain/          # Entidades de domínio
├── Pms.Backend.Application/     # Casos de uso e DTOs
├── Pms.Backend.Infrastructure/  # EF Core, repositórios, seeds
└── Pms.Backend.Api/            # Controllers, autenticação, Swagger

tests/
└── Pms.Backend.Tests/          # Testes unitários e de integração
```

## 🏗️ Arquitetura

- **Clean Architecture** com separação clara de responsabilidades
- **Domain-Driven Design** com entidades ricas
- **CQRS** com MediatR para casos de uso
- **Repository Pattern** para acesso a dados
- **Unit of Work** para transações

## 🔧 Configuração

### Variáveis de Ambiente
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: String de conexão PostgreSQL
- `Jwt__SecretKey`: Chave secreta para JWT
- `Jwt__ExpirationMinutes`: Tempo de expiração do token

### Configurações do Banco
- **Host**: localhost
- **Port**: 5432
- **Database**: pms_backend_dev (dev) / pms_backend (prod)
- **Username**: pms_user
- **Password**: pms_password

## 📊 MVP-0 - Funcionalidades Implementadas

### ✅ Hierarquia Organizacional
- **Divisão** → **União** → **Associação** → **Região** → **Distrito** → **Clube** → **Unidade** → **Membro**
- Códigos únicos e CodePath automático
- Relacionamento Clube ↔ Igreja (1:1)
- Controllers organizados por entidade

### ✅ Membros e Autenticação
- Cadastro de membros (≥10 anos)
- Sistema de convite/ativação
- JWT com RBAC + escopo organizacional
- Política de senhas e lockout
- Timeline de atividades do membro

### ✅ Clubes e Unidades
- Unidades por gênero e faixa etária
- Regra 1º/06 para idade de referência
- Alocação automática de membros
- Capacidade de unidades

### ✅ Cargos e Eventos
- Sistema de cargos sem acúmulo
- Delegado de aprovação
- Eventos oficiais com elegibilidade
- Participação em eventos

### ✅ Exportações CSV
- **Membros**: Dados completos com filiação atual
- **Timeline**: Histórico de atividades
- **Participações**: Eventos e status
- Formato UTF-8 com separador ";"

### ✅ Testes de Aceite
- Testes de integração completos
- Validação de fluxos end-to-end
- Cobertura de cenários críticos

## 🧪 Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar com cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Executar testes específicos
```bash
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
```

## 📝 Logs

Os logs são gerados em:
- **Console**: Durante desenvolvimento
- **Arquivo**: `logs/pms-backend-YYYY-MM-DD.log`
- **Nível**: Information (Development: Debug)

## 🔒 Segurança

- **JWT** com claims de papel e escopo
- **Rate Limiting** global (100 req/min)
- **CORS** configurado para frontend
- **Validação** com FluentValidation
- **Auditoria** com Timeline

## 🚀 Deploy

### Docker
```bash
# Build da imagem
docker build -t pms-backend .

# Executar container
docker run -p 5001:80 pms-backend

# Ou usar docker-compose
docker-compose up
```

### Produção
1. Configure variáveis de ambiente
2. Execute migrations: `dotnet ef database update`
3. Configure reverse proxy (nginx)
4. Configure SSL/TLS
5. Configure connection string para produção

## 📚 Documentação

- **Swagger**: http://localhost:5000/swagger
- **OpenAPI**: http://localhost:5000/swagger/v1/swagger.json
- **Health Check**: http://localhost:5000/health

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch: `git checkout -b feature/nova-funcionalidade`
3. Commit: `git commit -m 'Add: nova funcionalidade'`
4. Push: `git push origin feature/nova-funcionalidade`
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🆘 Suporte

Para suporte, abra uma issue no repositório ou entre em contato com a equipe de desenvolvimento.

## 🎯 Status do Projeto

- **MVP-0**: ✅ **Concluído**
- **Banco de Dados**: ✅ PostgreSQL configurado
- **Docker**: ✅ Containerização completa
- **Testes**: ✅ Testes de aceite implementados
- **API**: ✅ Endpoints funcionais
- **Documentação**: ✅ Swagger configurado

## 📋 Próximos Passos

- [ ] Frontend React/Next.js
- [ ] Autenticação JWT completa
- [ ] Dashboard administrativo
- [ ] Relatórios avançados
- [ ] Notificações por email
- [ ] App mobile

---

**Status**: ✅ MVP-0 Concluído
**Última atualização**: 2025-01-02
**Versão**: 1.0.0
