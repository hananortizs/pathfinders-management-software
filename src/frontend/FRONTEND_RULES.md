# 📋 Regras de Desenvolvimento Frontend - Pathfinder Management

## 🎯 Padrão de Nomenclatura das Páginas (Desktop/Web)

### 1) Convenção Geral
- **Rotas**: sempre em **kebab-case**, em inglês (URLs legíveis, curtas).  
  - Ex.: `/dashboard`, `/members`, `/events`, `/profile`.  
- **Nomes de Página (títulos de aba do navegador)**: em português, **Title Case** (primeira letra maiúscula em cada palavra relevante).  
  - Ex.: *Dashboard — Pathfinder Management*, *Membros — Pathfinder Management*.  
- **Componentes de Página** (arquivos React): PascalCase, sufixo `Page`.  
  - Ex.: `DashboardPage.tsx`, `MembersPage.tsx`.  
- **Layout compartilhado**: `AppLayout.tsx` (navbar + sidebar).  
- **Breadcrumbs**: sempre em português, singular ou plural conforme contexto.  
  - Ex.: *Início / Membros / Detalhes do Membro*.

### 2) Padrão de Rotas (Desktop/Web)
- `/dashboard` → **DashboardPage**  
- `/profile` → **Meu Perfil**  
- `/members` → **MembrosPage**  
  - `/members/:id` → **Detalhes do Membro**  
- `/units` → **UnidadesPage**  
  - `/units/:id` → **Detalhes da Unidade**  
- `/events` → **EventosPage**  
  - `/events/:id` → **Detalhes do Evento**  
- `/specialties` → **EspecialidadesPage**  
  - `/specialties/:id` → **Detalhes da Especialidade**  
- `/classes` → **ClassesPage**  
  - `/classes/:id` → **Detalhes da Classe**  
- `/reports` → **RelatóriosPage**  
- `/settings` → **ConfiguraçõesPage**  
- `/login` → **LoginPage**  
- `/register` (quando aplicável) → **RegisterPage**

### 3) Títulos de Aba (document.title)
- Sempre formatados como:  
  **`<Nome da Página> — Pathfinder Management`**  
  - Ex.: `Membros — Pathfinder Management`.  
- Atualizados em `useEffect` ao entrar na rota.  
- Caso detalhe de entidade (ex.: membro, evento):  
  **`<Nome da Entidade> — <Módulo> — Pathfinder Management`**  
  - Ex.: `João Silva — Membros — Pathfinder Management`.

### 4) Sidebar (Labels de Navegação)
- Sempre em português, no **plural** (quando aplicável).  
- Padrão visual consistente com ícones do MUI.  
- Ex.: *Dashboard*, *Membros*, *Unidades*, *Eventos*, *Especialidades*, *Classes*, *Relatórios*, *Configurações*.

### 5) Futuro — Mobile App
- No mobile app (instalável), **não existirá título de aba**.  
- Nome das "páginas" será usado em:
  - **App bar (header)** do app → português, título curto.  
    - Ex.: *Dashboard*, *Membros*, *Meu Perfil*.  
  - **Rotas internas (React Navigation ou similar)**: kebab-case (igual ao web), mas sem `document.title`.  
- **Identidade visual**: manter consistência com web, mas priorizar espaço e clareza no mobile.

### 6) Resumo das Regras
- Rotas → **kebab-case, inglês**.  
- Componentes → **PascalCase + Page**.  
- Títulos de aba → **Português + "— Pathfinder Management"**.  
- Sidebar/menu → **Português, plural, curtos**.  
- Breadcrumbs → **Português, contexto hierárquico**.  
- Mobile (futuro) → **sem aba**, usar títulos curtos no header.

---

## 🎨 Identidade Visual

### Favicon e Ícone
- **Favicon**: `pms-icon.svg` (localizado em `/dist/pms-icon.svg`)
- **Título padrão**: "Pathfinder Management"
- **Idioma**: `pt-BR` (definido no HTML)

### Hook para Títulos
```typescript
// Hook para páginas normais
usePageTitle("Dashboard"); // → "Dashboard — Pathfinder Management"

// Hook para páginas de detalhes
useEntityPageTitle("João Silva", "Membros"); // → "João Silva — Membros — Pathfinder Management"
```

---

## 📁 Estrutura de Arquivos

### Páginas
```
src/pages/
├── DashboardPage.tsx
├── MembersPage.tsx
├── ProfilePage.tsx
├── LoginPage.tsx
└── ...
```

### Hooks
```
src/hooks/
├── usePageTitle.ts
├── useAuth.ts
└── ...
```

### Componentes
```
src/components/
├── layout/
│   ├── AppLayout.tsx
│   ├── Navbar.tsx
│   └── Sidebar.tsx
└── ...
```

---

## 🚀 Implementação

### 1. Adicionar Título a uma Página
```typescript
import { usePageTitle } from "../hooks/usePageTitle";

const MinhaPage: React.FC = () => {
  usePageTitle("Minha Página");
  
  return (
    // JSX da página
  );
};
```

### 2. Adicionar Título a Página de Detalhes
```typescript
import { useEntityPageTitle } from "../hooks/usePageTitle";

const DetalhesPage: React.FC = () => {
  const { entityName } = useParams();
  
  useEntityPageTitle(entityName, "Módulo");
  
  return (
    // JSX da página
  );
};
```

### 3. Atualizar HTML Base
```html
<!doctype html>
<html lang="pt-BR">
  <head>
    <link rel="icon" type="image/svg+xml" href="/pms-icon.svg" />
    <title>Pathfinder Management</title>
  </head>
</html>
```

---

## ✅ Checklist de Implementação

- [x] Favicon atualizado (`pms-icon.svg`)
- [x] Idioma HTML definido como `pt-BR`
- [x] Hook `usePageTitle` criado
- [x] Hook `useEntityPageTitle` criado
- [x] Páginas principais atualizadas:
  - [x] DashboardPage
  - [x] MembersPage
  - [x] ProfilePage
  - [x] LoginPage
- [ ] Páginas restantes (quando criadas)
- [ ] Breadcrumbs implementados
- [ ] Sidebar com labels em português
- [ ] Testes de títulos das páginas

---

**Última atualização**: 25 de Janeiro de 2025
