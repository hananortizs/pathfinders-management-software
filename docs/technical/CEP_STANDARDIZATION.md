# Padronização de CEP - Sistema de Endereços

## Visão Geral

O sistema implementa uma padronização robusta para armazenamento e manipulação de CEPs (Código de Endereçamento Postal) brasileiros, garantindo consistência e facilidade de uso.

## Padrão de Armazenamento

### 🗄️ **No Banco de Dados**

- **Formato**: Apenas dígitos (8 caracteres)
- **Exemplo**: `01234567`
- **Zeros à esquerda**: **OBRIGATÓRIOS** (preenchidos automaticamente)
- **Tipo**: `VARCHAR(8)`
- **Índice**: Criado para consultas eficientes

### 📱 **No Frontend/API**

- **Formato**: Com hífen (formato brasileiro)
- **Exemplo**: `01234-567`
- **Validação**: Aceita ambos os formatos na entrada
- **Exibição**: Sempre formatado com hífen

## Validação e Normalização

### ✅ **Formatos Aceitos na Entrada**

```typescript
// Todos estes formatos são aceitos:
"01234567"; // ✅ 8 dígitos
"01234-567"; // ✅ 8 dígitos com hífen
"1234567"; // ✅ 7 dígitos (zero à esquerda adicionado)
"1234-567"; // ✅ 7 dígitos com hífen
```

### ❌ **Formatos Rejeitados**

```typescript
"12345"; // ❌ Menos de 7 dígitos
"123456789"; // ❌ Mais de 8 dígitos
"abc12345"; // ❌ Contém letras
"12.345-678"; // ❌ Formato inválido
```

## Implementação Técnica

### 1. **CepHelper Class**

```csharp
public static class CepHelper
{
    // Normaliza para banco de dados (8 dígitos)
    public static string? NormalizeCep(string? cep)

    // Formata para exibição (com hífen)
    public static string? FormatCepForDisplay(string? cep)

    // Valida formato
    public static bool IsValidCep(string? cep)

    // Compara CEPs (ignora formato)
    public static bool AreEquivalent(string? cep1, string? cep2)
}
```

### 2. **Validação Automática**

```csharp
[ValidCep(ErrorMessage = "CEP deve estar no formato 12345-678 ou 12345678")]
public string? Cep { get; set; }
```

### 3. **Conversão no EF Core**

```csharp
builder.Property(e => e.Cep)
    .HasMaxLength(8)
    .HasConversion(
        v => NormalizeCepForDatabase(v),  // Entrada → Banco
        v => v);                          // Banco → Aplicação
```

## Exemplos Práticos

### **Entrada do Usuário**

```json
{
  "cep": "1234-567",
  "street": "Rua das Flores",
  "city": "São Paulo"
}
```

### **Armazenamento no Banco**

```sql
INSERT INTO Addresses (Cep, Street, City)
VALUES ('01234567', 'Rua das Flores', 'São Paulo');
```

### **Retorno da API**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "cep": "01234567",
  "cepFormatted": "01234-567",
  "cepNormalized": "01234567",
  "street": "Rua das Flores",
  "city": "São Paulo",
  "fullAddress": "Rua das Flores, São Paulo, CEP: 01234-567"
}
```

## Propriedades Disponíveis

### **Na Entidade Address**

```csharp
public class Address
{
    public string? Cep { get; set; }                    // Formato do banco
    public string? CepFormatted => FormatCepForDisplay(Cep);    // Para exibição
    public string? CepNormalized => NormalizeCep(Cep);          // Normalizado
    public string FullAddress { get; }                  // Endereço completo
}
```

### **No AddressDto**

```csharp
public class AddressDto
{
    public string? Cep { get; set; }           // Formato do banco
    public string? CepFormatted { get; set; }  // Para exibição
    public string? CepNormalized { get; set; } // Normalizado
    public string FullAddress { get; set; }    // Endereço completo
}
```

## Regras de Negócio

### 1. **Obrigatoriedade**

- ✅ **Opcional**: CEP não é obrigatório
- ✅ **Validação**: Se informado, deve estar no formato correto

### 2. **Normalização Automática**

- ✅ **Zeros à esquerda**: Adicionados automaticamente
- ✅ **Formato único**: Sempre 8 dígitos no banco
- ✅ **Conversão**: Automática entre formatos

### 3. **Comparação**

- ✅ **Equivalência**: CEPs são comparados ignorando formato
- ✅ **Busca**: Funciona com qualquer formato de entrada

## Benefícios

### 1. **Consistência**

- ✅ **Formato único** no banco de dados
- ✅ **Validação robusta** na entrada
- ✅ **Exibição padronizada** na saída

### 2. **Facilidade de Uso**

- ✅ **Aceita múltiplos formatos** na entrada
- ✅ **Conversão automática** entre formatos
- ✅ **Propriedades prontas** para exibição

### 3. **Performance**

- ✅ **Índice otimizado** para consultas
- ✅ **Comparação eficiente** de CEPs
- ✅ **Validação rápida** com regex compilada

## Casos de Uso

### **1. Cadastro de Endereço**

```typescript
// Frontend envia qualquer formato
const addressData = {
  cep: "1234-567", // Usuário digita com hífen
  street: "Rua A",
  city: "São Paulo",
};

// Backend normaliza automaticamente
// Banco armazena: "01234567"
// API retorna: "01234-567"
```

### **2. Busca por CEP**

```typescript
// Busca funciona com qualquer formato
GET / address / by - cep / 1234 - 567;
GET / address / by - cep / 01234567;
GET / address / by - cep / 1234567;

// Todos retornam o mesmo resultado
```

### **3. Validação de Formulário**

```typescript
// Validação no frontend
const isValidCep = (cep: string) => {
  return /^\d{5}-?\d{3}$/.test(cep);
};

// Validação no backend (automática)
[ValidCep] public string? Cep { get; set; }
```

## Migração de Dados Existentes

Se houver dados existentes com CEPs em formato inconsistente, execute:

```sql
-- Normalizar CEPs existentes
UPDATE Addresses
SET Cep = LPAD(REGEXP_REPLACE(Cep, '[^0-9]', '', 'g'), 8, '0')
WHERE Cep IS NOT NULL
  AND LENGTH(REGEXP_REPLACE(Cep, '[^0-9]', '', 'g')) BETWEEN 7 AND 8;
```

## Conclusão

A padronização garante:

- ✅ **Consistência** no armazenamento
- ✅ **Flexibilidade** na entrada
- ✅ **Facilidade** na exibição
- ✅ **Performance** nas consultas
- ✅ **Manutenibilidade** do código
