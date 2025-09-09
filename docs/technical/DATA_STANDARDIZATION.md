# Padronização de Dados - Sistema de Validação

## Visão Geral

O sistema implementa padronizações robustas para campos de dados críticos, garantindo consistência, validação e facilidade de uso em todo o sistema.

## Campos Padronizados

### 📧 **E-mail**

#### **Padrão de Armazenamento**
- **Formato**: Lowercase, trimmed
- **Exemplo**: `usuario@dominio.com`
- **Validação**: RFC 5322 compliant
- **Tipo**: `VARCHAR(255)`
- **Índice**: Único globalmente

#### **Formatos Aceitos na Entrada**
```typescript
// Todos estes formatos são aceitos:
"usuario@dominio.com"     // ✅ Formato padrão
"USUARIO@DOMINIO.COM"     // ✅ Convertido para lowercase
" usuario@dominio.com "   // ✅ Trimmed automaticamente
```

#### **Validação**
```csharp
[ValidEmail(ErrorMessage = "Email deve estar em um formato válido")]
public string Email { get; set; } = string.Empty;
```

### 📱 **Telefone**

#### **Padrão de Armazenamento**
- **Formato**: Apenas dígitos com código do país (13 caracteres)
- **Exemplo**: `5511999999999`
- **Validação**: Formato brasileiro
- **Tipo**: `VARCHAR(15)`

#### **Formatos Aceitos na Entrada**
```typescript
// Todos estes formatos são aceitos:
"(11) 99999-9999"         // ✅ Formato brasileiro
"11999999999"             // ✅ Apenas dígitos
"+55 11 99999-9999"       // ✅ Com código do país
"11 9999-9999"            // ✅ Formato antigo
```

#### **Validação**
```csharp
[ValidPhone(ErrorMessage = "Telefone deve estar no formato (11) 99999-9999")]
public string? Phone { get; set; }
```

### 🆔 **CPF (Cadastro de Pessoa Física)**

#### **Padrão de Armazenamento**
- **Formato**: Apenas dígitos (11 caracteres)
- **Exemplo**: `12345678901`
- **Validação**: Dígitos verificadores
- **Tipo**: `VARCHAR(11)`

#### **Formatos Aceitos na Entrada**
```typescript
// Todos estes formatos são aceitos:
"123.456.789-01"          // ✅ Formato brasileiro
"12345678901"             // ✅ Apenas dígitos
"123456789-01"            // ✅ Formato parcial
```

#### **Validação**
```csharp
[ValidCpf(ErrorMessage = "CPF deve estar no formato 123.456.789-10")]
public string? Cpf { get; set; }
```

### 🆔 **RG (Registro Geral)**

#### **Padrão de Armazenamento**
- **Formato**: Apenas dígitos e X (8-9 caracteres)
- **Exemplo**: `123456789`
- **Validação**: Formato brasileiro
- **Tipo**: `VARCHAR(9)`

#### **Formatos Aceitos na Entrada**
```typescript
// Todos estes formatos são aceitos:
"12.345.678-9"            // ✅ Formato brasileiro
"123456789"               // ✅ Apenas dígitos
"12345678-X"              // ✅ Com X no final
```

#### **Validação**
```csharp
[ValidRg(ErrorMessage = "RG deve estar no formato 12.345.678-9")]
public string? Rg { get; set; }
```

## Implementação Técnica

### **1. Helpers de Validação**

#### **EmailHelper**
```csharp
public static class EmailHelper
{
    public static string? NormalizeEmail(string? email)
    public static bool IsValidEmail(string? email)
    public static string? GetValidationError(string? email)
    public static string? ExtractDomain(string? email)
    public static string? MaskEmail(string? email)
}
```

#### **PhoneHelper**
```csharp
public static class PhoneHelper
{
    public static string? NormalizePhone(string? phone)
    public static string? FormatPhoneForDisplay(string? phone)
    public static bool IsValidPhone(string? phone)
    public static string? GetValidationError(string? phone)
    public static string? GetPhoneType(string? phone)
}
```

#### **CpfHelper**
```csharp
public static class CpfHelper
{
    public static string? NormalizeCpf(string? cpf)
    public static string? FormatCpfForDisplay(string? cpf)
    public static bool IsValidCpf(string? cpf)
    public static string? GetValidationError(string? cpf)
    public static bool ValidateCheckDigits(string cpf)
}
```

#### **RgHelper**
```csharp
public static class RgHelper
{
    public static string? NormalizeRg(string? rg)
    public static string? FormatRgForDisplay(string? rg)
    public static bool IsValidRg(string? rg)
    public static string? GetValidationError(string? rg)
    public static string? ExtractStateCode(string? rg)
}
```

### **2. Atributos de Validação**

```csharp
[ValidEmail(ErrorMessage = "Email deve estar em um formato válido")]
[ValidPhone(ErrorMessage = "Telefone deve estar no formato (11) 99999-9999")]
[ValidCpf(ErrorMessage = "CPF deve estar no formato 123.456.789-10")]
[ValidRg(ErrorMessage = "RG deve estar no formato 12.345.678-9")]
```

### **3. Conversão no EF Core**

```csharp
builder.Property(e => e.Email)
    .HasConversion(
        v => NormalizeEmailForDatabase(v),
        v => v);

builder.Property(e => e.Phone)
    .HasConversion(
        v => NormalizePhoneForDatabase(v),
        v => v);
```

## Propriedades Formatadas

### **Na Entidade Member**
```csharp
public class Member
{
    public string Email { get; set; }                    // Formato do banco
    public string? EmailFormatted => EmailHelper.NormalizeEmail(Email);
    
    public string? Phone { get; set; }                   // Formato do banco
    public string? PhoneFormatted => PhoneHelper.FormatPhoneForDisplay(Phone);
    
    public string? Cpf { get; set; }                     // Formato do banco
    public string? CpfFormatted => CpfHelper.FormatCpfForDisplay(Cpf);
    
    public string? Rg { get; set; }                      // Formato do banco
    public string? RgFormatted => RgHelper.FormatRgForDisplay(Rg);
}
```

### **No DTO**
```csharp
public class MemberDto
{
    public string Email { get; set; }                    // Formato do banco
    public string? EmailFormatted { get; set; }          // Para exibição
    
    public string? Phone { get; set; }                   // Formato do banco
    public string? PhoneFormatted { get; set; }          // Para exibição
    
    public string? Cpf { get; set; }                     // Formato do banco
    public string? CpfFormatted { get; set; }            // Para exibição
    
    public string? Rg { get; set; }                      // Formato do banco
    public string? RgFormatted { get; set; }             // Para exibição
}
```

## Exemplos Práticos

### **Entrada do Usuário**
```json
{
  "email": "USUARIO@DOMINIO.COM",
  "phone": "(11) 99999-9999",
  "cpf": "123.456.789-01",
  "rg": "12.345.678-9"
}
```

### **Armazenamento no Banco**
```sql
INSERT INTO Members (Email, Phone, Cpf, Rg) 
VALUES ('usuario@dominio.com', '5511999999999', '12345678901', '123456789');
```

### **Retorno da API**
```json
{
  "email": "usuario@dominio.com",
  "emailFormatted": "usuario@dominio.com",
  "phone": "5511999999999",
  "phoneFormatted": "(11) 99999-9999",
  "cpf": "12345678901",
  "cpfFormatted": "123.456.789-01",
  "rg": "123456789",
  "rgFormatted": "12.345.678-9"
}
```

## Benefícios Alcançados

### **1. ✅ Consistência Total**
- **Formato único** no banco de dados
- **Validação robusta** na entrada
- **Exibição padronizada** na saída

### **2. ✅ Flexibilidade Máxima**
- **Aceita múltiplos formatos** na entrada
- **Conversão automática** entre formatos
- **Propriedades prontas** para exibição

### **3. ✅ Validação Robusta**
- **Validação de formato** automática
- **Validação de dígitos verificadores** (CPF)
- **Validação de regras de negócio** (RG)

### **4. ✅ Performance Otimizada**
- **Índices otimizados** para consultas
- **Comparação eficiente** de dados
- **Validação rápida** com regex compilada

## Campos Recomendados para Padronização

### **✅ Implementados**
- **E-mail**: Validação RFC 5322, normalização lowercase
- **Telefone**: Formato brasileiro, normalização com código do país
- **CPF**: Validação de dígitos verificadores, normalização
- **RG**: Formato brasileiro, normalização
- **CEP**: Formato brasileiro, normalização (já implementado)
- **Data de Nascimento**: Validação de idade mínima (10 anos até 1º de junho)
- **Nome Completo**: Validação e normalização de nomes (First, Middle, Last)

### **🔄 Recomendados para Implementação**
- **CNPJ**: Validação de dígitos verificadores
- **PIS/PASEP**: Validação de dígitos verificadores
- **Título de Eleitor**: Validação de dígitos verificadores
- **Cartão de Crédito**: Validação de dígitos verificadores (Luhn)
- **Data de Nascimento**: Validação de idade mínima/máxima
- **Nome Completo**: Normalização de espaços, validação de caracteres
- **Endereço**: Normalização de espaços, validação de caracteres especiais

### **📋 Considerações para Novos Campos**
- **Unicidade**: Se o campo deve ser único
- **Formato**: Se há padrão específico a seguir
- **Validação**: Se há regras de negócio específicas
- **Normalização**: Se precisa ser normalizado para armazenamento
- **Formatação**: Se precisa ser formatado para exibição
- **Máscara**: Se precisa de máscara para entrada do usuário

## Conclusão

A padronização garante:
- ✅ **Consistência** no armazenamento
- ✅ **Flexibilidade** na entrada
- ✅ **Facilidade** na exibição
- ✅ **Validação robusta** de dados
- ✅ **Performance** nas consultas
- ✅ **Manutenibilidade** do código

