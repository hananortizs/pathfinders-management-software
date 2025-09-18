# Sistema de Design - PathfinderManagementSoftware

Este documento descreve o sistema de design implementado para o frontend da aplicação PathfinderManagementSoftware, seguindo as diretrizes estabelecidas.

## 🎨 Paleta de Cores

### Cores Principais
- **Primary**: `#0D47A1` - Azul principal para ações primárias
- **Secondary**: `#B71C1C` - Vermelho para ações secundárias e destaque
- **Warning**: `#FFD700` - Amarelo para avisos e alertas
- **Background**: `#F5F5F5` - Cinza claro para fundo da aplicação
- **Text Primary**: `#212121` - Cinza escuro para textos principais

### Cores de Estado
- **Success**: `#388E3C` - Verde para sucesso
- **Error**: `#D32F2F` - Vermelho para erros
- **Info**: `#1976D2` - Azul para informações

## 🔤 Tipografia

- **Fonte**: Roboto (Google Fonts)
- **Pesos**: 300 (Light), 400 (Regular), 500 (Medium), 600 (Bold)
- **Responsiva**: Tamanhos adaptáveis para mobile-first

## 📐 Espaçamento e Formas

- **Border Radius**: 12px (padrão), 16px (cards)
- **Spacing**: Múltiplos de 8px
- **Breakpoints**: MUI padrão (xs: 0, sm: 600, md: 900, lg: 1200, xl: 1536)

## 🧩 Componentes Customizados

### Botões
- `PrimaryButton` - Ação principal
- `SecondaryButton` - Ação secundária
- `DangerButton` - Ação destrutiva
- `OutlinePrimaryButton` - Ação outline
- `TextButton` - Ação minimalista
- `LoadingButton` - Botão com estado de loading

### Cards
- `AppCard` - Card base reutilizável
- `MemberCard` - Card para exibição de membros
- `UnitCard` - Card para exibição de unidades
- `StatsCard` - Card para exibição de estatísticas

### Inputs
- `AppTextField` - Campo de texto padrão
- `SearchTextField` - Campo de busca
- `PasswordTextField` - Campo de senha
- `MultilineTextField` - Campo multilinha
- `ValidatedTextField` - Campo com validação visual

## ♿ Acessibilidade

- **Tamanho mínimo de toque**: 44x44px
- **Contraste AA**: Garantido para todos os elementos
- **Estados de foco**: Visíveis para navegação por teclado
- **Semantic HTML**: Estrutura semântica adequada

## 🎯 Diretrizes de Uso

### Quando usar `sx` prop
- Ajustes rápidos e locais
- Responsividade (`display: { xs: 'block', md: 'flex' }`)
- Valores únicos que não se repetem

### Quando usar `styled` components
- Padrões reutilizáveis
- Lógica de estado complexa
- Componentes que aparecem em múltiplos lugares

### Quando usar CSS global
- Apenas para bibliotecas de terceiros
- Animações complexas (preferir framer-motion)
- Reset básico e fontes

## 📱 Responsividade

O sistema é mobile-first com breakpoints MUI padrão:
- **xs**: 0px+ (mobile)
- **sm**: 600px+ (tablet pequeno)
- **md**: 900px+ (tablet)
- **lg**: 1200px+ (desktop)
- **xl**: 1536px+ (desktop grande)

## 🚀 Como Usar

```tsx
import { ThemeProvider } from '@mui/material';
import { appTheme } from './theme/theme';
import { PrimaryButton, MemberCard } from './components/styled';

function App() {
  return (
    <ThemeProvider theme={appTheme}>
      <PrimaryButton>Meu Botão</PrimaryButton>
      <MemberCard>
        {/* Conteúdo do card */}
      </MemberCard>
    </ThemeProvider>
  );
}
```

## 🔧 Customização

Para adicionar novos componentes ou modificar existentes:

1. **Tema**: Edite `src/theme/theme.ts`
2. **Componentes globais**: Edite `src/theme/components.ts`
3. **Componentes estilizados**: Adicione em `src/components/styled/`

## 📋 Checklist de Implementação

- [x] Tema base configurado
- [x] Paleta de cores definida
- [x] Tipografia responsiva
- [x] Componentes globais customizados
- [x] Botões estilizados
- [x] Cards estilizados
- [x] Inputs estilizados
- [x] Estilos globais mínimos
- [x] Acessibilidade implementada
- [x] Documentação criada
