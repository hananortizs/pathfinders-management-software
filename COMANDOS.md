# 🚀 Comandos de Desenvolvimento - Pathfinder Management Software

## 🎯 Comandos Principais

### Executar Aplicação Completa

```bash
# Executar backend + frontend simultaneamente (RECOMENDADO)
npm run dev
# ou
npm start

# Usando scripts automatizados
# Windows:
.\scripts\dev.ps1

# Linux/macOS:
./scripts/dev.sh
```

### Executar Serviços Separadamente

```bash
# Apenas backend
npm run dev:backend-only

# Apenas frontend
npm run dev:frontend-only
```

## 🔧 Comandos de Build

```bash
# Build de produção (backend + frontend)
npm run build

# Build de desenvolvimento
npm run build:dev

# Build apenas backend
npm run build:backend

# Build apenas frontend
npm run build:frontend
```

## 🧪 Comandos de Teste

```bash
# Executar todos os testes
npm run test

# Executar testes em modo watch
npm run test:watch

# Testes apenas backend
npm run test:backend

# Testes apenas frontend
npm run test:frontend
```

## 🔍 Comandos de Qualidade

```bash
# Linting
npm run lint
npm run lint:fix

# Formatação
npm run format

# Verificar se compila
npm run check
```

## 🧹 Comandos de Limpeza

```bash
# Limpar builds
npm run clean

# Limpar tudo (incluindo Docker)
npm run clean:all

# Reset completo
npm run reset
```

## 🐳 Comandos Docker

```bash
# Subir containers
npm run docker:up

# Parar containers
npm run docker:down

# Ver logs
npm run docker:logs

# Limpar containers
npm run docker:clean
```

## 📦 Comandos de Instalação

```bash
# Instalar todas as dependências
npm run install:all

# Instalar apenas frontend
npm run install:frontend
```

## 🌐 URLs de Acesso

Após executar `npm run dev`:

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5000/api
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health

## ⚡ Comandos Rápidos

```bash
# Desenvolvimento completo
npm run dev

# Apenas backend
npm run dev:backend-only

# Apenas frontend
npm run dev:frontend-only

# Testes
npm run test

# Build
npm run build

# Limpar tudo
npm run clean:all
```

## 🆘 Solução de Problemas

### Erro de dependências

```bash
npm run install:all
```

### Erro de build

```bash
npm run clean
npm run build
```

### Reset completo

```bash
npm run reset
```

### Verificar se tudo está funcionando

```bash
npm run check
```

---

**💡 Dica**: Use `npm run dev` para desenvolvimento diário. Este comando executa tanto o backend quanto o frontend com logs coloridos e reinicialização automática.
