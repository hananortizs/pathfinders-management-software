# Script PowerShell para desenvolvimento
# Executa tanto o backend quanto o frontend simultaneamente

Write-Host "🚀 Iniciando desenvolvimento do Pathfinder Management Software..." -ForegroundColor Green
Write-Host ""

# Verificar se o .NET está instalado
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET encontrado: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET não encontrado. Instale o .NET 8.0 SDK" -ForegroundColor Red
    exit 1
}

# Verificar se o Node.js está instalado
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js encontrado: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js não encontrado. Instale o Node.js 18+ SDK" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "📦 Instalando dependências..." -ForegroundColor Yellow

# Instalar dependências do frontend
Write-Host "Instalando dependências do frontend..." -ForegroundColor Cyan
Set-Location "src/frontend"
npm install
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro ao instalar dependências do frontend" -ForegroundColor Red
    exit 1
}

# Voltar para o diretório raiz
Set-Location "../.."

Write-Host ""
Write-Host "🔧 Restaurando dependências do backend..." -ForegroundColor Yellow
Set-Location "src/backend"
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro ao restaurar dependências do backend" -ForegroundColor Red
    exit 1
}

# Voltar para o diretório raiz
Set-Location "../.."

Write-Host ""
Write-Host "🎯 Iniciando aplicação..." -ForegroundColor Green
Write-Host "Backend: http://localhost:5000" -ForegroundColor Blue
Write-Host "Frontend: http://localhost:5173" -ForegroundColor Blue
Write-Host "Swagger: http://localhost:5000/swagger" -ForegroundColor Blue
Write-Host ""
Write-Host "Pressione Ctrl+C para parar ambos os serviços" -ForegroundColor Yellow
Write-Host ""

# Executar ambos os serviços
npm run dev
