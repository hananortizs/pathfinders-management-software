# 🌐 Documentação da API

Documentação específica para integração com a API do Pathfinder Management System.

## 📋 Documentos Disponíveis

### ✅ Implementados

| Documento                                              | Descrição                                  | Última Atualização |
| ------------------------------------------------------ | ------------------------------------------ | ------------------ |
| [Integração Frontend](FRONTEND_INTEGRATION_EXAMPLE.md) | Guia completo para integração com frontend | 04/09/2025         |

### 🔄 Em Desenvolvimento

| Documento                  | Descrição                                    | Status       |
| -------------------------- | -------------------------------------------- | ------------ |
| Endpoints da API           | Documentação detalhada de todos os endpoints | 📋 Planejado |
| Autenticação e Autorização | Sistema de autenticação e permissões         | 📋 Planejado |
| Códigos de Status HTTP     | Guia de códigos de resposta                  | 📋 Planejado |
| Rate Limiting              | Limitações de taxa e throttling              | 📋 Planejado |

## 🚀 Recursos da API

### ✅ Funcionalidades Implementadas

#### **Sistema de Hierarquia**

- **Regiões**: Gerenciamento de regiões
- **Associações**: Gerenciamento de associações
- **Distritos**: Gerenciamento de distritos
- **Uniões**: Gerenciamento de uniões
- **Divisões**: Gerenciamento de divisões
- **Igrejas**: Gerenciamento de igrejas
- **Clubes**: Gerenciamento de clubes
- **Unidades**: Gerenciamento de unidades

#### **Sistema de Membros**

- **Cadastro**: Criação e atualização de membros
- **Busca**: Pesquisa e filtros avançados
- **Validação**: Validação robusta de dados
- **Endereços**: Sistema centralizado de endereços

#### **Sistema de Endereços**

- **Relacionamento Polimórfico**: Endereços para qualquer entidade
- **Padronização de CEP**: Validação e normalização automática
- **Validação**: Validação de dados de endereço
- **Formatação**: Formatação automática para exibição

### 🔄 Em Desenvolvimento

#### **Sistema de Investiduras**

- **Cadastro**: Criação de investiduras
- **Testemunhas**: Sistema de testemunhas
- **Validação**: Validação de regras de negócio

#### **Sistema de Relatórios**

- **Exportação**: Exportação de dados
- **Filtros**: Filtros avançados
- **Formatação**: Múltiplos formatos de saída

## 📚 Como Usar Esta Documentação

### Para Desenvolvedores Frontend

#### **1. Integração Básica**

```typescript
// Configuração base da API
const API_BASE_URL = "http://localhost:5000/pms-dev";

// Headers padrão
const defaultHeaders = {
  "Content-Type": "application/json",
  Accept: "application/json",
};
```

#### **2. Exemplo de Uso**

```typescript
// Buscar distritos
const response = await fetch(`${API_BASE_URL}/hierarchy-district`, {
  headers: defaultHeaders,
});

const data = await response.json();
console.log(data.data.items); // Lista de distritos
```

### Para Desenvolvedores Backend

#### **1. Estrutura de Resposta**

```json
{
  "success": true,
  "message": "Operação realizada com sucesso",
  "data": {
    "items": [...],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 10
  },
  "errors": null
}
```

#### **2. Códigos de Status**

- **200 OK**: Operação bem-sucedida
- **201 Created**: Recurso criado com sucesso
- **400 Bad Request**: Dados inválidos
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro interno do servidor

## 🔧 Recursos Técnicos

### **Validação de Dados**

- **Data Annotations**: Validação automática de DTOs
- **Custom Validators**: Validadores personalizados
- **Validação de CEP**: Validação específica para CEPs brasileiros
- **Validação de EntityType**: Validação de tipos de entidade

### **Mapeamento de Dados**

- **AutoMapper**: Mapeamento automático entre entidades e DTOs
- **Profiles**: Configurações de mapeamento organizadas
- **Custom Mappings**: Mapeamentos personalizados quando necessário

### **Padronização**

- **EntityType**: Campo padronizado para relacionamentos polimórficos
- **CEP**: Padronização automática de CEPs brasileiros
- **Respostas**: Estrutura padronizada de respostas da API

## 📖 Guias de Implementação

### **1. Criando um Novo Endpoint**

```csharp
[HttpGet]
[ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<EntityDto>>>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetEntities(int pageNumber = 1, int pageSize = 10)
{
    // Implementação
}
```

### **2. Validação de Dados**

```csharp
[HttpPost]
[ProducesResponseType(typeof(BaseResponse<EntityDto>), StatusCodes.Status201Created)]
public async Task<IActionResult> CreateEntity([FromBody] CreateEntityDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // Implementação
}
```

### **3. Tratamento de Erros**

```csharp
try
{
    // Operação
    return Ok(BaseResponse<EntityDto>.SuccessResult(result));
}
catch (Exception ex)
{
    return StatusCode(500, BaseResponse<EntityDto>.ErrorResult("Erro interno do servidor"));
}
```

## 🚀 Próximos Passos

### **Documentação Planejada**

1. **Endpoints Detalhados**: Documentação completa de todos os endpoints
2. **Autenticação**: Sistema de autenticação e autorização
3. **Rate Limiting**: Limitações de taxa e throttling
4. **Webhooks**: Sistema de notificações em tempo real

### **Melhorias Planejadas**

1. **Versionamento**: Versionamento da API
2. **Cache**: Sistema de cache para melhor performance
3. **Logging**: Sistema de logging estruturado
4. **Monitoring**: Monitoramento e métricas da API

## 📞 Suporte

Para dúvidas sobre a API:

- **Swagger UI**: http://localhost:5000/swagger
- **Issues**: [GitHub Issues](https://github.com/seu-usuario/pathfinder-management/issues)
- **Email**: [seu-email@exemplo.com]

---

**Última atualização**: 04/09/2025  
**Versão da API**: 1.0.0  
**Swagger UI**: http://localhost:5000/swagger
