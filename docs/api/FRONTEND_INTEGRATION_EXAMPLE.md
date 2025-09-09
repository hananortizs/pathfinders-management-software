# Frontend Integration - EntityType Field

## Visão Geral

Todos os DTOs de entidades que podem ter endereços agora incluem automaticamente o campo `EntityType`, facilitando a integração do frontend sem necessidade de tabelas de mapeamento.

## Estrutura dos DTOs

### Classe Base

```typescript
interface IAddressableEntityDto {
  id: string;
  entityType: string;
  entityTypeDisplayName: string;
  entityTypeDescription: string;
}
```

### DTOs que Implementam a Interface

- `MemberDto`
- `ChurchDto`
- `ClubDto`
- `UnitDto`
- `DistrictDto`
- `AssociationDto`
- `UnionDto`
- `DivisionDto`
- `RegionDto`

## Valores de EntityType

| EntityType    | DisplayName | Description                      |
| ------------- | ----------- | -------------------------------- |
| `Member`      | Membro      | Membro do clube de desbravadores |
| `Church`      | Igreja      | Igreja local                     |
| `Club`        | Clube       | Clube de desbravadores           |
| `Unit`        | Unidade     | Unidade dentro de um clube       |
| `District`    | Distrito    | Distrito da igreja               |
| `Association` | Associação  | Associação da igreja             |
| `Union`       | União       | União da igreja                  |
| `Division`    | Divisão     | Divisão da igreja                |
| `Region`      | Região      | Região da igreja                 |

## Exemplo de Uso no Frontend

### 1. Buscar Tipos de Entidades Válidas

```typescript
// GET /address/entity-types
const response = await fetch("/api/address/entity-types");
const { data } = await response.json();

// Resultado:
[
  {
    value: "Member",
    displayName: "Membro",
    description: "Membro do clube de desbravadores",
  },
  {
    value: "Church",
    displayName: "Igreja",
    description: "Igreja local",
  },
  // ... outros tipos
];
```

### 2. Usar EntityType em Formulários

```typescript
// Ao criar um endereço, o frontend já tem o EntityType
const member = await getMemberById(memberId);
const addressData = {
  entityId: member.id,
  entityType: member.entityType, // "Member"
  street: "Rua das Flores, 123",
  city: "São Paulo",
  state: "SP",
};
```

### 3. Exibir Informações de Entidade

```typescript
// Exibir informações da entidade com endereços
const entity = await getEntityById(entityId, entityType);

console.log(`Tipo: ${entity.entityTypeDisplayName}`); // "Membro"
console.log(`Descrição: ${entity.entityTypeDescription}`); // "Membro do clube de desbravadores"
```

### 4. Validação no Frontend

```typescript
const validEntityTypes = [
  "Member",
  "Church",
  "Club",
  "Unit",
  "District",
  "Association",
  "Union",
  "Division",
  "Region",
];

function validateEntityType(entityType: string): boolean {
  return validEntityTypes.includes(entityType);
}
```

## Benefícios

1. **Sem Tabelas de Mapeamento**: O frontend não precisa manter tabelas de "de/para"
2. **Consistência**: Todos os DTOs seguem o mesmo padrão
3. **Facilidade de Uso**: Informações prontas para exibição
4. **Manutenibilidade**: Mudanças centralizadas na classe base
5. **Type Safety**: Interface TypeScript para validação

## Endpoints Relacionados

- `GET /address/entity-types` - Lista todos os tipos de entidades válidas
- `GET /address/by-entity/{entityId}?entityType={entityType}` - Busca endereços por entidade
- `POST /address` - Cria novo endereço (usa EntityType do DTO)

## Exemplo Completo

```typescript
// 1. Buscar membro
const member = await getMemberById("123e4567-e89b-12d3-a456-426614174000");

// 2. Usar EntityType automaticamente
const newAddress = {
  entityId: member.id,
  entityType: member.entityType, // "Member"
  street: "Rua das Flores, 123",
  city: "São Paulo",
  state: "SP",
  cep: "01234-567",
  type: "Home",
  isPrimary: true,
};

// 3. Criar endereço
const createdAddress = await createAddress(newAddress);

// 4. Exibir informações
console.log(
  `Endereço criado para ${member.entityTypeDisplayName}: ${member.fullName}`
);
```
