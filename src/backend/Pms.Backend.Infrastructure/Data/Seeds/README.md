# Seeds de Dados

Esta pasta contém arquivos de dados de exemplo e seeds para inicialização do banco de dados.

## Arquivos

### `region_data.json`

Dados de exemplo para uma região específica:

- **Código**: APL3RT
- **Nome**: 3ª Região - Tigres
- **Descrição**: 3ª Região de Desbravadores da Associação Paulista Leste
- **AssociationId**: ab1eb215-cfba-42bc-a192-297d8798c891

Este arquivo é usado para:

- Testes de desenvolvimento
- Validação da cadeia hierárquica
- Dados de referência para testes de aceitação

## Cadeia Hierárquica de Referência

```
DSA → UCB → APL → APL3RT → DVM → PAC
```

Onde:

- **DSA**: Divisão Sul Americana
- **UCB**: União Central Brasileira
- **APL**: Associação Paulista Leste
- **APL3RT**: 3ª Região - Tigres
- **DVM**: Distrito (exemplo)
- **PAC**: Clube (exemplo)

## Uso

Estes arquivos podem ser utilizados por:

- Serviços de seed do banco de dados
- Testes de integração
- Validação de dados
- Documentação de exemplos
