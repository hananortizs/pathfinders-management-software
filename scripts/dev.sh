#!/bin/bash

# Script Bash para desenvolvimento
# Executa tanto o backend quanto o frontend simultaneamente

echo "🚀 Iniciando desenvolvimento do Pathfinder Management Software..."
echo ""

# Verificar se o .NET está instalado
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo "✅ .NET encontrado: $DOTNET_VERSION"
else
    echo "❌ .NET não encontrado. Instale o .NET 8.0 SDK"
    exit 1
fi

# Verificar se o Node.js está instalado
if command -v node &> /dev/null; then
    NODE_VERSION=$(node --version)
    echo "✅ Node.js encontrado: $NODE_VERSION"
else
    echo "❌ Node.js não encontrado. Instale o Node.js 18+ SDK"
    exit 1
fi

echo ""
echo "📦 Instalando dependências..."

# Instalar dependências do frontend
echo "Instalando dependências do frontend..."
cd src/frontend
npm install
if [ $? -ne 0 ]; then
    echo "❌ Erro ao instalar dependências do frontend"
    exit 1
fi

# Voltar para o diretório raiz
cd ../..

echo ""
echo "🔧 Restaurando dependências do backend..."
cd src/backend
dotnet restore
if [ $? -ne 0 ]; then
    echo "❌ Erro ao restaurar dependências do backend"
    exit 1
fi

# Voltar para o diretório raiz
cd ../..

echo ""
echo "🎯 Iniciando aplicação..."
echo "Backend: http://localhost:5000"
echo "Frontend: http://localhost:5173"
echo "Swagger: http://localhost:5000/swagger"
echo ""
echo "Pressione Ctrl+C para parar ambos os serviços"
echo ""

# Executar ambos os serviços
npm run dev
