o noá e# PROGRESS.md - PathfinderManagementSoftware

## Status do Projeto

- **MVP Atual**: MVP-0 (Núcleo operacional)
- **Status**: 🚧 EM DESENVOLVIMENTO
- **Última Atualização**: 2024-12-19

## Milestones MVP-0

### ✅ Concluído

- [x] Estrutura base da solution (Clean Architecture)
- [x] Configuração de projetos (.csproj)
- [x] EditorConfig e ferramentas dotnet

### 🚧 Em Progresso

- [ ] Configuração EF Core + PostgreSQL
- [ ] Configuração JWT + RBAC
- [ ] Configuração CORS + Swagger

### ⏳ Pendente

- [ ] Implementação hierarquia + códigos
- [ ] Implementação membros + auth
- [ ] Implementação clubes + membership
- [ ] Implementação cargos + eventos
- [ ] Implementação exportações CSV
- [ ] Testes de aceite

## Critérios de Aceite MVP-0

### Plataforma/Segurança

- [ ] JWT com claims (sub, email, roles[], scopes[], exp)
- [ ] RBAC + escopo organizacional
- [ ] Login e-mail + senha (Argon2/BCrypt)
- [ ] Convite/ativação de usuários
- [ ] Seed SystemAdmin com token

### Hierarquia + Igrejas

- [ ] Cadeia: Divisão → União → Associação → Região → Distrito → Clube → Unidade → Membro
- [ ] Code (≤5 chars) e CodePath (≤60) únicos
- [ ] Recalculo CodePath em cascata
- [ ] Clube ↔ Igreja 1:1 obrigatório
- [ ] CEP único global para Igreja

### Membros

- [ ] Membro ≥10 anos
- [ ] Endereço obrigatório
- [ ] Batismo + Lenço (opcional inicialmente)
- [ ] E-mail único global

### Clubes/Unidades/Membership

- [ ] Regra 1º/06 para idade de referência
- [ ] Alocação automática por gênero/faixa
- [ ] Capacidade de unidade (null=ilimitado)
- [ ] Realocação automática (aniversário)
- [ ] Override manual (Diretor/Secretário)
- [ ] Tarefas para alocação/realocação

### Cargos

- [ ] Sem acúmulo de cargos de diretoria no clube
- [ ] Requisitos espirituais (Batismo + Lenço)
- [ ] Promoções/baixas (encerrar memberships)
- [ ] Delegado de aprovação

### Eventos

- [ ] Elegibilidade multi-dia
- [ ] Clube inativo bloqueia participação
- [ ] Taxa em BRL (default gratuito)

### Timeline + Exportações

- [ ] Timeline básica (append-only)
- [ ] CSV UTF-8 para Director (membros, timeline, participações)

## Backlog MVP-1 (Programa/Aprovações/Investiduras)

- Classes regulares e avançadas
- Especialidades e mestrados
- Progressos e cadeias de aprovação
- Investiduras completas
- Reprovação/reabertura

## Riscos Identificados

- **Baixo**: Complexidade da regra 1º/06
- **Baixo**: Performance com recálculo CodePath em cascata

## BLOQUEIOS

- Nenhum bloqueio atual

## OPEN_QUESTIONS

- Nenhuma questão pendente

## Próximos Passos

1. Implementar configuração EF Core + PostgreSQL
2. Implementar JWT + RBAC
3. Implementar entidades de hierarquia
4. Implementar seeds básicos
5. Implementar testes de aceite

---

**MVP-0: NOT ACCEPTED** ⚠️
