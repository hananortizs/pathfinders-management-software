# Validação de Contatos - Pathfinder Management System

## 📋 Visão Geral

Este documento detalha o sistema de validação de contatos implementado no PMS, incluindo validações de frontend, backend e regras de negócio específicas.

## 🎯 Regras de Negócio

### 1. **Contatos Primários**
- ✅ **Apenas um telefone pode ser primário** por membro
- ✅ **Apenas um email pode ser primário** por membro
- ✅ **Um telefone primário + um email primário** podem coexistir
- ✅ **Validação visual** no frontend com botões desabilitados
- ✅ **Validação no backend** antes de salvar

### 2. **Tipos de Contato Suportados**
```typescript
enum ContactType {
  Email = 3,
  Mobile = 1,      // Telefone celular
  Landline = 2,    // Telefone fixo
  WhatsApp = 4,
  Facebook = 5,
  Instagram = 6,
  YouTube = 7,
  TikTok = 8,
  LinkedIn = 9,
  Twitter = 10,
  Website = 11,
  Other = 99
}
```

### 3. **Validações de Formato**

#### Telefone
- ✅ **Formato internacional**: `+5511999999999`
- ✅ **DDI automático**: Seleção por país
- ✅ **Formatação visual**: `+55 11 99999-9999`
- ✅ **Validação de comprimento**: 10-15 dígitos
- ✅ **Validação de país**: DDI válido

#### Email
- ✅ **Formato básico**: `usuario@dominio.com`
- ✅ **Comprimento mínimo**: 5 caracteres
- ✅ **Validação de @**: Obrigatório
- ✅ **Validação de domínio**: Pelo menos 3 caracteres

## 🔧 Implementação Técnica

### Frontend (React/TypeScript)

#### 1. **Componente PhoneInputWithDDI**
```typescript
interface PhoneInputWithDDIProps {
  value: string;                    // Número internacional completo
  onChange: (value: string) => void;
  onValidationChange?: (isValid: boolean) => void;
  defaultCountry?: CountryCode;
}

// Validação em tempo real
useEffect(() => {
  const fullNumber = selectedCountry === "BR" 
    ? `+55${phoneNumber}` 
    : `+${getCountryCallingCode(selectedCountry)}${phoneNumber}`;
  
  const valid = !phoneNumber || libIsValidPhoneNumber(fullNumber);
  setIsValid(valid);
  onValidationChange?.(valid);
}, [phoneNumber, selectedCountry]);
```

#### 2. **Validação de Contatos Primários**
```typescript
const canBePrimary = (index: number) => {
  const targetContact = formData[index];
  const contactType = targetContact.type;
  
  // Verifica se já existe outro contato do mesmo tipo marcado como primário
  const hasOtherPrimaryOfSameType = formData.some((contact, i) =>
    i !== index && contact.type === contactType && contact.isPrimary
  );
  
  return !hasOtherPrimaryOfSameType;
};
```

#### 3. **Mapeamento de Tipos Frontend ↔ Backend**
```typescript
// Frontend → Backend
const mapContactTypeToBackend = (frontendType: string): number => {
  const typeMap: Record<string, number> = {
    "Email": 3,
    "Phone": 1,      // Mobile
    "WhatsApp": 4,
    "Mobile": 1,
    "Landline": 2,
    "Facebook": 5,
    "Instagram": 6,
    "YouTube": 7,
    "TikTok": 8,
    "LinkedIn": 9,
    "Twitter": 10,
    "Website": 11,
    "Other": 99
  };
  return typeMap[frontendType] || 99;
};

// Backend → Frontend
const mapContactTypeFromBackend = (backendType: number): string => {
  const typeMap: Record<number, string> = {
    1: "Phone",      // Mobile
    2: "Phone",      // Landline
    3: "Email",
    4: "WhatsApp",
    5: "Facebook",
    6: "Instagram",
    7: "YouTube",
    8: "TikTok",
    9: "LinkedIn",
    10: "Twitter",
    11: "Website",
    99: "Other"
  };
  return typeMap[backendType] || "Other";
};
```

### Backend (C# .NET)

#### 1. **DTOs com Validação**
```csharp
public class UpdateMyContactsRequestDto
{
    [Required(ErrorMessage = "Token é obrigatório")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "A lista de contatos não pode ser nula.")]
    public IEnumerable<MemberContactDto> Contacts { get; set; } = new List<MemberContactDto>();
}

public class MemberContactDto
{
    [Required]
    public ContactType Type { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Valor deve ter entre 3 e 255 caracteres")]
    public string Value { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
    public string? Notes { get; set; }
}
```

#### 2. **Validação na Camada de Serviço**
```csharp
public async Task<BaseResponse<IEnumerable<MemberContactDto>>> UpdateMyContactsAsync(
    UpdateMyContactsRequestDto request, CancellationToken cancellationToken = default)
{
    // 1. Validação de dados de entrada
    if (string.IsNullOrEmpty(request.Token))
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult("Token é obrigatório", statusCode: 400);
    }

    if (request.Contacts == null || !request.Contacts.Any())
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult("Lista de contatos não pode ser vazia", statusCode: 400);
    }

    // 2. Validação de token
    var userInfo = _authService.GetUserInfoFromToken(request.Token);
    if (!userInfo.IsSuccess || userInfo.Data == null)
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.UnauthorizedResult("Token inválido ou expirado");
    }

    // 3. Validação de cada contato
    foreach (var contact in request.Contacts)
    {
        if (string.IsNullOrWhiteSpace(contact.Value))
        {
            return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult(
                $"Valor do contato {contact.Type} não pode estar vazio", statusCode: 400);
        }

        if (contact.Value.Length < 3)
        {
            return BaseResponse<IEnumerable<MemberContactDto>>.ErrorResult(
                $"Valor do contato {contact.Type} deve ter pelo menos 3 caracteres", statusCode: 400);
        }
    }

    // 4. Validação de existência do membro
    var member = await _unitOfWork.Repository<Member>().GetByIdAsync(userId, cancellationToken);
    if (member == null)
    {
        return BaseResponse<IEnumerable<MemberContactDto>>.NotFoundResult("Membro não encontrado");
    }

    // 5. Processar atualização...
}
```

## 🎨 Interface de Usuário

### 1. **Design Responsivo**
```typescript
// Layout responsivo
<Box sx={{
  display: "flex",
  gap: { xs: 2, sm: 2 },
  alignItems: "flex-start",
  flexDirection: { xs: "column", sm: "row" },
  width: "100%"
}}>
  {/* Seletor de País */}
  <FormControl sx={{
    minWidth: { xs: "100%", sm: 140, md: 160 },
    width: { xs: "100%", sm: "auto" }
  }}>
    {/* ... */}
  </FormControl>

  {/* Input de Telefone */}
  <TextField sx={{
    flex: 1,
    minWidth: { xs: "100%", sm: 200 },
    "& .MuiInputBase-input": {
      minHeight: { xs: 48, sm: 40 },
      fontSize: { xs: "16px", sm: "14px" }
    }
  }} />
</Box>
```

### 2. **Validação Visual**
```typescript
// Botão primário com validação
<Button
  variant={contact.isPrimary ? "contained" : "outlined"}
  disabled={!canBePrimary(index)}
  title={!canBePrimary(index) 
    ? `Já existe um ${contact.type} marcado como primário`
    : contact.isPrimary 
    ? "Este contato é primário"
    : "Tornar este contato primário"
}
>
  {contact.isPrimary ? "Primário" : "Tornar Primário"}
</Button>

// Chip de aviso
{!canBePrimary(index) && !contact.isPrimary && (
  <Chip
    label="Não pode ser primário"
    color="warning"
    size="small"
    variant="outlined"
  />
)}
```

### 3. **Exibição de Dados Sensíveis**
```typescript
// SensitiveDataField com formatação
const formatValue = (val: string) => {
  if (type === 'tel' && val.startsWith('+')) {
    try {
      const parsed = parsePhoneNumber(val);
      return parsed ? parsed.formatInternational() : val;
    } catch (error) {
      return val;
    }
  }
  return val;
};

const getMaskedValue = () => {
  if (type === 'email') {
    const [localPart, domain] = value.split('@');
    if (localPart && domain) {
      const maskedLocal = localPart.length > 2
        ? `${localPart[0]}${'•'.repeat(localPart.length - 2)}${localPart[localPart.length - 1]}`
        : '••';
      return `${maskedLocal}@${domain}`;
    }
  }
  
  if (type === 'tel') {
    return value.startsWith('+') 
      ? value.replace(/\d/g, '•')  // +•• •• •••••-••••
      : value.replace(/\d/g, '•'); // (••) •••••-••••
  }
  
  return '•'.repeat(Math.min(value.length, 8));
};
```

## 🧪 Testes de Validação

### Cenários de Teste

#### 1. **Telefone Válido**
```typescript
// Brasil
"+5511999999999" → ✅ Válido
"+5511987654321" → ✅ Válido

// EUA
"+1234567890" → ✅ Válido
"+15551234567" → ✅ Válido

// Portugal
"+351123456789" → ✅ Válido
```

#### 2. **Telefone Inválido**
```typescript
"+5511999" → ❌ Muito curto
"+551199999999999" → ❌ Muito longo
"11999999999" → ❌ Sem DDI
"abc123" → ❌ Caracteres inválidos
```

#### 3. **Email Válido**
```typescript
"usuario@exemplo.com" → ✅ Válido
"teste.email@dominio.org" → ✅ Válido
"a@b.co" → ✅ Válido (mínimo)
```

#### 4. **Email Inválido**
```typescript
"usuario@" → ❌ Sem domínio
"@exemplo.com" → ❌ Sem usuário
"usuario" → ❌ Sem @
"a@b" → ❌ Domínio muito curto
```

#### 5. **Contatos Primários**
```typescript
// ✅ Válido: Um telefone + um email primários
[
  { type: "Phone", value: "+5511999999999", isPrimary: true },
  { type: "Email", value: "user@example.com", isPrimary: true }
]

// ❌ Inválido: Dois telefones primários
[
  { type: "Phone", value: "+5511999999999", isPrimary: true },
  { type: "Phone", value: "+5511888888888", isPrimary: true }
]

// ❌ Inválido: Dois emails primários
[
  { type: "Email", value: "user1@example.com", isPrimary: true },
  { type: "Email", value: "user2@example.com", isPrimary: true }
]
```

## 📊 Status de Implementação

### ✅ Implementado e Funcionando

1. **Validação de Formato**
   - ✅ Telefone internacional com DDI
   - ✅ Email com formato básico
   - ✅ Comprimento mínimo de 3 caracteres

2. **Validação de Primários**
   - ✅ Apenas um telefone primário
   - ✅ Apenas um email primário
   - ✅ Validação visual no frontend
   - ✅ Validação no backend

3. **Interface Responsiva**
   - ✅ Design mobile-first
   - ✅ Breakpoints responsivos
   - ✅ Espaçamento otimizado

4. **Validação de Autenticação**
   - ✅ Token JWT obrigatório
   - ✅ Verificação de usuário existente

### 🔄 Próximos Passos

1. **Validação de Unicidade**
   - 🔄 Email único global
   - 🔄 Telefone único global

2. **Validação Avançada**
   - 🔄 Validação de CPF
   - 🔄 Validação de RG

3. **Testes Automatizados**
   - 🔄 Testes unitários
   - 🔄 Testes de integração

## 🔗 Referências

- [Documentação do react-phone-number-input](https://github.com/catamphetamine/react-phone-number-input)
- [Documentação do libphonenumber-js](https://github.com/catamphetamine/libphonenumber-js)
- [Documentação do Material-UI](https://mui.com/)
- [Documentação do Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

---

**Última atualização**: Dezembro 2024  
**Versão**: 1.0.0  
**Status**: Implementado e Funcionando
