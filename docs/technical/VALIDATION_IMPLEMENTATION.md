# Sistema de Validação - Pathfinder Management System

## 📋 Visão Geral

Este documento descreve o sistema de validação implementado no PMS, incluindo validações de frontend, backend e regras de negócio.

## 🏗️ Arquitetura de Validação

### Camadas de Validação

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend       │    │   Database      │
│   (UX/UI)       │    │   (Business)    │    │   (Integrity)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🎯 Validações Implementadas

### 1. **Validações de Contatos**

#### Frontend (React/TypeScript)
- **Validação de Formato**: Telefones com formatação internacional
- **Validação de Comprimento**: Mínimo 3 caracteres para todos os contatos
- **Validação de Email**: Formato básico de email
- **Validação de Telefone**: Suporte a números internacionais com DDI
- **Validação de Primário**: Apenas um telefone e um email podem ser primários

#### Backend (C# .NET)
- **Validação de Dados de Entrada**: Token obrigatório, lista de contatos não vazia
- **Validação de Token**: Verificação de autenticação via JWT
- **Validação de Comprimento**: Valor mínimo de 3 caracteres
- **Validação de Tipos**: Enum de tipos de contato válidos
- **Validação de Existência**: Verificação se o membro existe

### 2. **Validações de Telefone**

#### Componente PhoneInputWithDDI
```typescript
// Validação de formato internacional
const isValid = libIsValidPhoneNumber(fullNumber);

// Validação de comprimento por país
const maxLength = selectedCountry === "BR" ? 11 : 15;

// Formatação automática
const formatted = parsePhoneNumber(fullNumber)?.formatInternational();
```

#### Mapeamento de Tipos
```typescript
// Frontend → Backend
const typeMap = {
  "Email": 3,
  "Phone": 1,    // Mobile
  "WhatsApp": 4,
  "Mobile": 1,
  "Landline": 2,
  // ... outros tipos
};
```

### 3. **Validações de Responsividade**

#### Design Mobile-First
```typescript
// Breakpoints responsivos
sx={{
  flexDirection: { xs: "column", sm: "row" },
  minHeight: { xs: 48, sm: 40 },
  fontSize: { xs: "16px", sm: "14px" },
  padding: { xs: "12px 14px", sm: "8px 14px" }
}}
```

#### Espaçamento Otimizado
```typescript
// Gaps e margens responsivos
sx={{
  gap: { xs: 2, sm: 2 },
  mb: { xs: 3, sm: 1 },
  p: { xs: 3, sm: 4 }
}}
```

## 🔧 Implementação Técnica

### 1. **Validação no Frontend**

#### ProfileService
```typescript
// Validação de valor mínimo
if (!contact.value || contact.value.trim().length < 3) {
  throw new Error(`Valor do contato deve ter pelo menos 3 caracteres`);
}

// Validação de tipo
if (mappedType === 99 && !["Other"].includes(contact.type)) {
  throw new Error(`Tipo de contato inválido: "${contact.type}"`);
}
```

#### Componente ContactsSection
```typescript
// Validação de contatos primários
const canBePrimary = (index: number) => {
  const targetContact = formData[index];
  const contactType = targetContact.type;
  
  const hasOtherPrimaryOfSameType = formData.some((contact, i) =>
    i !== index && contact.type === contactType && contact.isPrimary
  );
  
  return !hasOtherPrimaryOfSameType;
};
```

### 2. **Validação no Backend**

#### DTOs com Data Annotations
```csharp
public class UpdateMyContactsRequestDto
{
    [Required(ErrorMessage = "Token é obrigatório")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "A lista de contatos não pode ser nula.")]
    public IEnumerable<MemberContactDto> Contacts { get; set; } = new List<MemberContactDto>();
}
```

#### Service Layer Validation
```csharp
public async Task<BaseResponse<IEnumerable<MemberContactDto>>> UpdateMyContactsAsync(
    UpdateMyContactsRequestDto request, CancellationToken cancellationToken = default)
{
    // Validação de dados de entrada
    if (string.IsNullOrEmpty(request.Token))
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult("Token é obrigatório", statusCode: 400);
    }

    // Validação de token
    var userInfo = _authService.GetUserInfoFromToken(request.Token);
    if (!userInfo.IsSuccess || userInfo.Data == null)
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.UnauthorizedResult("Token inválido ou expirado");
    }

    // Validação de contatos
    foreach (var contact in request.Contacts)
    {
        if (string.IsNullOrWhiteSpace(contact.Value))
        {
            return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult($"Valor do contato {contact.Type} não pode estar vazio", statusCode: 400);
        }

        if (contact.Value.Length < 3)
        {
            return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult($"Valor do contato {contact.Type} deve ter pelo menos 3 caracteres", statusCode: 400);
        }
    }
}
```

## 📊 Status das Validações

### ✅ Implementadas e Funcionando

1. **Validação de Formato de Telefone**
   - ✅ Formatação internacional automática
   - ✅ Validação de DDI por país
   - ✅ Suporte a números brasileiros e internacionais

2. **Validação de Email**
   - ✅ Formato básico de email
   - ✅ Validação de comprimento mínimo

3. **Validação de Contatos Primários**
   - ✅ Apenas um telefone primário
   - ✅ Apenas um email primário
   - ✅ Validação visual no frontend

4. **Validação de Responsividade**
   - ✅ Design mobile-first
   - ✅ Breakpoints responsivos
   - ✅ Espaçamento otimizado

5. **Validação de Autenticação**
   - ✅ Verificação de token JWT
   - ✅ Validação de usuário existente

### 🔄 Em Desenvolvimento

1. **Validação de Unicidade**
   - 🔄 Email único por membro
   - 🔄 Telefone único por membro

2. **Validação de Formato Avançada**
   - 🔄 Validação de CPF
   - 🔄 Validação de RG

3. **Validação de Regras de Negócio**
   - 🔄 Idade mínima para membros
   - 🔄 Status de membro para operações

## 🧪 Testes de Validação

### Cenários Testados

1. **Telefone Válido**
   - ✅ `+5511999999999` (Brasil)
   - ✅ `+1234567890` (EUA)
   - ✅ `+351123456789` (Portugal)

2. **Telefone Inválido**
   - ✅ Números muito curtos
   - ✅ Caracteres não numéricos
   - ✅ DDI inválido

3. **Email Válido**
   - ✅ `usuario@exemplo.com`
   - ✅ `teste.email@dominio.org`

4. **Email Inválido**
   - ✅ Sem @
   - ✅ Muito curto
   - ✅ Formato incorreto

5. **Contatos Primários**
   - ✅ Apenas um telefone primário
   - ✅ Apenas um email primário
   - ✅ Validação visual de botões

## 📝 Próximos Passos

1. **Implementar Validação de Unicidade**
   - Verificar email único no banco
   - Verificar telefone único no banco

2. **Melhorar Validação de Formato**
   - Validação de CPF com dígito verificador
   - Validação de RG por estado

3. **Adicionar Testes Automatizados**
   - Testes unitários para validações
   - Testes de integração para fluxos completos

4. **Documentar Regras de Negócio**
   - Criar documentação de regras específicas
   - Mapear validações por tipo de usuário

## 🔗 Referências

- [Documentação do React Hook Form](https://react-hook-form.com/)
- [Documentação do Material-UI](https://mui.com/)
- [Documentação do react-phone-number-input](https://github.com/catamphetamine/react-phone-number-input)
- [Documentação do Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Documentação do ASP.NET Core Validation](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation)

---

**Última atualização**: Dezembro 2024  
**Versão**: 1.0.0  
**Status**: Em Desenvolvimento Ativo
