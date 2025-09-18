# 🚀 Guia de Desenvolvimento - Pathfinder Management Software

Este guia contém todas as instruções necessárias para configurar e executar o projeto em ambiente de desenvolvimento.

## 📋 Pré-requisitos

### Backend (.NET)
- **.NET 8.0 SDK** ou superior
- **SQL Server** ou **SQL Server Express** (para desenvolvimento local)
- **Visual Studio 2022** ou **VS Code** com extensão C#

### Frontend (React + TypeScript)
- **Node.js 18+** e **npm 8+**
- **Git** para controle de versão

## 🛠️ Configuração Inicial

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/pathfinders-management-software.git
cd pathfinders-management-software
```

### 2. Instalar dependências
```bash
# Instalar dependências de todos os projetos
npm run install:all
```

### 3. Configurar banco de dados
```bash
# Executar migrações do banco de dados
cd src/backend
dotnet ef database update
cd ../..
```

### 4. Configurar variáveis de ambiente
```bash
# Copiar arquivo de exemplo do frontend
cp src/frontend/.env.example src/frontend/.env
```

Edite o arquivo `src/frontend/.env` com as configurações necessárias:
```env
VITE_API_BASE_URL=http://localhost:5000/api
VITE_NODE_ENV=development
VITE_DEBUG=true
```

## 🚀 Executando o Projeto

### Opção 1: Scripts Automatizados (Recomendado)

#### Windows (PowerShell)
```powershell
.\scripts\dev.ps1
```

#### Linux/macOS (Bash)
```bash
./scripts/dev.sh
```

### Opção 2: Comandos NPM

#### Executar ambos os serviços simultaneamente
```bash
npm run dev
# ou
npm start
```

#### Executar apenas o backend
```bash
npm run dev:backend-only
```

#### Executar apenas o frontend
```bash
npm run dev:frontend-only
```

### Opção 3: Executar separadamente

#### Terminal 1 - Backend
```bash
cd src/backend
dotnet run --project Pms.Backend.Api
```

#### Terminal 2 - Frontend
```bash
cd src/frontend
npm run dev
```

## 🌐 URLs de Acesso

Após executar o projeto, você terá acesso a:

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5000/api
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health

## 📝 Comandos Úteis

### Desenvolvimento
```bash
# Executar em modo desenvolvimento
npm run dev

# Executar apenas backend
npm run dev:backend-only

# Executar apenas frontend
npm run dev:frontend-only

# Ver logs de ambos os serviços
npm run logs
```

### Build e Deploy
```bash
# Build de produção
npm run build

# Build de desenvolvimento
npm run build:dev

# Verificar se tudo compila
npm run check
```

### Testes
```bash
# Executar todos os testes
npm run test

# Executar testes em modo watch
npm run test:watch

# Testes apenas do backend
npm run test:backend

# Testes apenas do frontend
npm run test:frontend
```

### Linting e Formatação
```bash
# Executar linting
npm run lint

# Corrigir problemas de linting
npm run lint:fix

# Formatar código
npm run format
```

### Limpeza
```bash
# Limpar builds
npm run clean

# Limpar tudo (incluindo Docker)
npm run clean:all

# Reset completo
npm run reset
```

### Docker
```bash
# Subir containers
npm run docker:up

# Parar containers
npm run docker:down

# Ver logs dos containers
npm run docker:logs

# Limpar containers
npm run docker:clean
```

## 🔧 Configurações Avançadas

### Backend
- **Porta**: 5000 (configurável em `src/backend/Pms.Backend.Api/Properties/launchSettings.json`)
- **Banco de dados**: SQL Server (configurável em `appsettings.json`)
- **Logs**: Serilog (configurável em `appsettings.json`)

### Frontend
- **Porta**: 5173 (configurável em `vite.config.ts`)
- **Hot reload**: Habilitado por padrão
- **TypeScript**: Configurado com strict mode

## 🐛 Solução de Problemas

### Erro de conexão com banco de dados
1. Verifique se o SQL Server está rodando
2. Confirme a string de conexão em `appsettings.json`
3. Execute as migrações: `dotnet ef database update`

### Erro de dependências do frontend
1. Delete `node_modules` e `package-lock.json`
2. Execute `npm run install:frontend`

### Erro de build do backend
1. Limpe o projeto: `npm run clean:backend`
2. Restaure dependências: `dotnet restore`
3. Rebuild: `dotnet build`

### Porta já em uso
1. Pare outros serviços na porta 5000 ou 5173
2. Ou configure portas diferentes nos arquivos de configuração

## 📚 Estrutura do Projeto

```
pathfinders-management-software/
├── src/
│   ├── backend/                 # API .NET
│   │   ├── Pms.Backend.Api/    # Projeto principal da API
│   │   ├── Pms.Backend.Application/ # Camada de aplicação
│   │   ├── Pms.Backend.Domain/ # Entidades e regras de negócio
│   │   └── Pms.Backend.Infrastructure/ # Acesso a dados
│   └── frontend/               # Aplicação React
│       ├── src/
│       │   ├── components/     # Componentes React
│       │   ├── pages/          # Páginas da aplicação
│       │   ├── services/       # Serviços de API
│       │   ├── store/          # Gerenciamento de estado
│       │   └── types/          # Tipos TypeScript
│       └── public/             # Arquivos estáticos
├── scripts/                    # Scripts de automação
├── docs/                       # Documentação
└── docker-compose.yml          # Configuração Docker
```

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

**Desenvolvido com ❤️ por Hanan Ortiz**
