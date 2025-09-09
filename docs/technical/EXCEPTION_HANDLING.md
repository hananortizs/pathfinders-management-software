# Sistema de Exception Handling

## Visão Geral

O sistema de exception handling foi implementado para fornecer tratamento centralizado e consistente de erros em toda a aplicação. Ele inclui exceções personalizadas, middleware de tratamento global e helpers para facilitar o uso.

## Arquitetura

### 1. Exceções Personalizadas (Domain Layer)

#### BaseException
- Classe base para todas as exceções personalizadas
- Inclui `ErrorCode`, `Details` e `Message`
- Suporte a exceções internas

#### ValidationException
- Para erros de validação de dados
- Inclui lista de `ValidationError` com detalhes específicos
- Status HTTP: 400 (Bad Request)

#### NotFoundException
- Para recursos não encontrados
- Inclui `ResourceType` e `ResourceId`
- Status HTTP: 404 (Not Found)

#### BusinessRuleException
- Para violações de regras de negócio
- Inclui `BusinessRule` e detalhes adicionais
- Status HTTP: 422 (Unprocessable Entity)

#### DuplicateException
- Para tentativas de criar recursos duplicados
- Inclui `ResourceType`, `FieldName` e `FieldValue`
- Status HTTP: 409 (Conflict)

### 2. Middleware de Exception Handling (API Layer)

#### ExceptionHandlingMiddleware
- Captura todas as exceções não tratadas
- Converte exceções em respostas HTTP padronizadas
- Inclui logging estruturado
- Suporte a diferentes ambientes (Development/Production)

#### ErrorResponse
- Modelo padronizado para respostas de erro
- Inclui `StatusCode`, `ErrorCode`, `Message`, `Details`, `TraceId` e `Timestamp`

### 3. ExceptionHelper (Application Layer)

#### Métodos Estáticos
- `ThrowValidationException()` - Para erros de validação
- `ThrowNotFoundException()` - Para recursos não encontrados
- `ThrowBusinessRuleException()` - Para regras de negócio
- `ThrowDuplicateException()` - Para duplicatas
- `ThrowArgumentException()` - Para argumentos inválidos
- `ThrowUnauthorizedException()` - Para acesso negado
- `ThrowNotImplementedException()` - Para funcionalidades não implementadas

## Uso

### 1. Em Services

```csharp
public async Task<BaseResponse<AddressDto>> GetAddressAsync(Guid id, CancellationToken cancellationToken = default)
{
    var address = await _unitOfWork.Repository<AddressEntity>()
        .GetFirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

    if (address == null)
    {
        ExceptionHelper.ThrowNotFoundException<AddressEntity>(id);
    }

    var addressDto = _mapper.Map<AddressDto>(address);
    return BaseResponse<AddressDto>.SuccessResult(addressDto);
}
```

### 2. Em Controllers

```csharp
[HttpGet("test")]
public IActionResult TestException()
{
    ExceptionHelper.ThrowValidationException("FieldName", "Valor inválido", "invalidValue");
    return Ok(); // This will never be reached
}
```

### 3. Validação de Dados

```csharp
if (!EntityTypeHelper.IsValidEntityType(entityType))
{
    ExceptionHelper.ThrowValidationException("EntityType", "Tipo de entidade inválido", entityType);
}
```

## Configuração

### 1. Registro do Middleware

```csharp
// Program.cs
app.UseExceptionHandling();
```

### 2. Ordem dos Middlewares

```csharp
app.UseHttpsRedirection();
app.UseExceptionHandling(); // Deve ser cedo no pipeline
app.UseCors("DefaultPolicy");
app.UseResponseStandardization();
```

## Respostas de Erro

### 1. ValidationException

```json
{
  "statusCode": 400,
  "errorCode": "VALIDATION_ERROR",
  "message": "Validation failed for field 'FieldName'",
  "details": {
    "validationErrors": [
      {
        "fieldName": "FieldName",
        "errorMessage": "Valor inválido",
        "attemptedValue": "invalidValue"
      }
    ]
  },
  "traceId": "trace-id-123",
  "timestamp": "2024-01-01T00:00:00Z"
}
```

### 2. NotFoundException

```json
{
  "statusCode": 404,
  "errorCode": "NOT_FOUND",
  "message": "Resource 'Address' not found",
  "details": {
    "resourceType": "Address",
    "resourceId": "guid-123"
  },
  "traceId": "trace-id-123",
  "timestamp": "2024-01-01T00:00:00Z"
}
```

### 3. BusinessRuleException

```json
{
  "statusCode": 422,
  "errorCode": "BUSINESS_RULE_VIOLATION",
  "message": "Regra de negócio violada",
  "details": {
    "businessRule": "TestRule",
    "additionalInfo": "Detalhes adicionais"
  },
  "traceId": "trace-id-123",
  "timestamp": "2024-01-01T00:00:00Z"
}
```

## Testes

### Controller de Teste

O `TestExceptionController` fornece endpoints para testar todos os tipos de exceções:

- `GET /api/testexception/validation` - Testa ValidationException
- `GET /api/testexception/not-found` - Testa NotFoundException
- `GET /api/testexception/business-rule` - Testa BusinessRuleException
- `GET /api/testexception/duplicate` - Testa DuplicateException
- `GET /api/testexception/argument` - Testa ArgumentException
- `GET /api/testexception/unauthorized` - Testa UnauthorizedAccessException
- `GET /api/testexception/not-implemented` - Testa NotImplementedException
- `GET /api/testexception/generic` - Testa exceção genérica

### Exemplo de Teste

```bash
curl -X GET "https://localhost:5000/api/testexception/validation?fieldName=Email&value=invalid-email"
```

## Logging

O middleware registra automaticamente todas as exceções com:
- Nível de log: Error
- Mensagem da exceção
- Stack trace (em desenvolvimento)
- Trace ID para correlação

## Benefícios

1. **Consistência**: Todas as exceções são tratadas de forma uniforme
2. **Rastreabilidade**: Trace ID para correlação de logs
3. **Segurança**: Detalhes sensíveis são ocultados em produção
4. **Manutenibilidade**: Código centralizado e reutilizável
5. **Debugging**: Informações detalhadas em desenvolvimento
6. **Padronização**: Respostas de erro consistentes para o frontend

## Próximos Passos

1. **Logging Estruturado**: Implementar Serilog com correlação
2. **Métricas**: Adicionar métricas de exceções
3. **Alertas**: Configurar alertas para exceções críticas
4. **Documentação**: Swagger com exemplos de erros
5. **Testes**: Testes unitários para o middleware
