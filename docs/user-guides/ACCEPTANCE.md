# ACCEPTANCE.md - PathfinderManagementSoftware

## MVP-0 (Núcleo operacional) - Critérios de Aceite

### ✅ Status: NOT ACCEPTED

---

## 1. Plataforma/Segurança

### 1.1 Autenticação JWT

- [ ] **AC-001**: JWT contém claims obrigatórios (sub, email, roles[], scopes[], exp)
- [ ] **AC-002**: Login com e-mail único + senha
- [ ] **AC-003**: Senha com política (min 10 chars, maiúscula, minúscula, dígito)
- [ ] **AC-004**: Histórico de 5 senhas anteriores
- [ ] **AC-005**: Lockout após 5 tentativas (15 min)

### 1.2 Autorização RBAC

- [ ] **AC-006**: Autorização por papel + escopo organizacional
- [ ] **AC-007**: Escopo = {Level}:{Id} (ex: Region:123)
- [ ] **AC-008**: SystemAdmin com acesso total

### 1.3 Convite/Ativação

- [ ] **AC-009**: Convite via POST /members
- [ ] **AC-010**: Token de ativação 24h
- [ ] **AC-011**: Seed SystemAdmin com token
- [ ] **AC-012**: E-mail único global (inclusive arquivados)

---

## 2. Hierarquia + Igrejas

### 2.1 Estrutura Hierárquica

- [ ] **AC-013**: Cadeia: Divisão → União → Associação → Região → Distrito → Clube → Unidade → Membro
- [ ] **AC-014**: Cada nível tem 1 pai e 0..N filhos
- [ ] **AC-015**: Clube pertence a 1 Distrito (obrigatório)

### 2.2 Códigos

- [ ] **AC-016**: Code ≤5 chars (UPPERCASE A-Z0-9)
- [ ] **AC-017**: CodePath ≤60 chars (concatenação com ".")
- [ ] **AC-018**: Code único por nível
- [ ] **AC-019**: CodePath único global
- [ ] **AC-020**: Recalculo CodePath em cascata ao mover nó

### 2.3 Igrejas

- [ ] **AC-021**: Clube ↔ Igreja 1:1 obrigatório
- [ ] **AC-022**: CEP único global para Igreja
- [ ] **AC-023**: Unicidade de nomes por nível pai

---

## 3. Membros

### 3.1 Cadastro

- [ ] **AC-024**: Membro ≥10 anos (422 se <10)
- [ ] **AC-025**: Endereço obrigatório
- [ ] **AC-026**: Batismo obrigatório
- [ ] **AC-027**: Lenço opcional inicialmente
- [ ] **AC-028**: E-mail único global

### 3.2 Dados Pessoais

- [ ] **AC-029**: Gênero ∈ {Masculino, Feminino}
- [ ] **AC-030**: Data de nascimento
- [ ] **AC-031**: Endereço completo
- [ ] **AC-032**: Batismo (data, local)

---

## 4. Clubes/Unidades/Membership

### 4.1 Unidades

- [ ] **AC-033**: Gênero ∈ {Masculina, Feminina}
- [ ] **AC-034**: Faixa etária (AgeMin, AgeMax inclusivos)
- [ ] **AC-035**: Capacidade (null=ilimitado, >0=limitado)
- [ ] **AC-036**: Sobreposição de faixas permitida

### 4.2 Regra 1º/06

- [ ] **AC-037**: Idade de referência = idade em 1º/06 do ano da matrícula
- [ ] **AC-038**: Mudança de clube no mesmo ano usa 1º/06 do próprio ano
- [ ] **AC-039**: ≥16 anos não aloca em unidade infanto-juvenil

### 4.3 Alocação Automática

- [ ] **AC-040**: 1 match (gênero+faixa) → associar automaticamente
- [ ] **AC-041**: 0 matches → permitir sem unidade + tarefa
- [ ] **AC-042**: >1 matches → 422 com opções + tarefa
- [ ] **AC-043**: Opções ordenadas por menor lotação (tie-break nome)

### 4.4 Realocação

- [ ] **AC-044**: Job diário 00:05 para aniversários
- [ ] **AC-045**: Realocação on-login/on-write (idempotente)
- [ ] **AC-046**: 1 match → realoca + timeline + tarefa
- [ ] **AC-047**: >1 match → desassocia + tarefa

### 4.5 Membership

- [ ] **AC-048**: Máx 1 membership ativa por membro
- [ ] **AC-049**: 0 ativa para Regional/Distrital/Pastores
- [ ] **AC-050**: Capacidade excedida → 422 + tarefa

---

## 5. Cargos

### 5.1 Diretoria do Clube

- [ ] **AC-051**: 1 Diretor (≥18)
- [ ] **AC-052**: Até 2 Diretores Associados (gêneros distintos, ≥16)
- [ ] **AC-053**: 1 Secretário (≥16)
- [ ] **AC-054**: Sem acúmulo de cargos de diretoria
- [ ] **AC-055**: Clube sem Diretor → IsActive=false

### 5.2 Requisitos Espirituais

- [ ] **AC-056**: Batismo + Lenço obrigatórios para liderança
- [ ] **AC-057**: Promoção Regional/Distrital/Pastores encerra memberships
- [ ] **AC-058**: Rebaixamento perde cargo anterior

### 5.3 Delegado de Aprovação

- [ ] **AC-059**: Regional pode delegar a Distrital da mesma Região
- [ ] **AC-060**: Delegado substitui aprovador no período
- [ ] **AC-061**: Conflito de interesse → pular etapa

---

## 6. Eventos

### 6.1 Criação

- [ ] **AC-062**: Organizado por qualquer nível
- [ ] **AC-063**: Registrar OrganizerLevel e OrganizerId
- [ ] **AC-064**: Taxa em BRL (default gratuito)

### 6.2 Elegibilidade

- [ ] **AC-065**: Membro ativo em toda janela (Start→End)
- [ ] **AC-066**: Clube inativo bloqueia participação
- [ ] **AC-067**: Lideranças sem membership podem participar

---

## 7. Timeline + Exportações

### 7.1 Timeline

- [ ] **AC-068**: Append-only (memberships, cargos, realocações, eventos)
- [ ] **AC-069**: Datas em UTC, exibição em America/Sao_Paulo

### 7.2 Exportações CSV

- [ ] **AC-070**: UTF-8, separador ";"
- [ ] **AC-071**: Somente para Diretor
- [ ] **AC-072**: /reports/members.csv?clubId=...
- [ ] **AC-073**: /reports/timeline.csv?memberId=...
- [ ] **AC-074**: /reports/participations.csv?clubId=...&start=...&end=...

---

## 8. Tarefas

### 8.1 Criação Automática

- [ ] **AC-075**: "Alocar/Escolher Unidade" → Diretor e Secretário
- [ ] **AC-076**: "Realocação (múltiplas opções)" → Diretor e Secretário
- [ ] **AC-077**: "Capacidade excedida" → Diretor e Secretário

### 8.2 Status

- [ ] **AC-078**: Status ∈ {Open, InProgress, Blocked, Done}
- [ ] **AC-079**: Prioridade ∈ {Low, Normal, High}

---

## 9. Seeds

### 9.1 Sistema

- [ ] **AC-080**: SystemAdmin com e-mail e token
- [ ] **AC-081**: Cadeia: DSA → UCB → APL → APL3RT → DVM → PAC
- [ ] **AC-082**: Igreja placeholder com CEP exclusivo

### 9.2 Unidades

- [ ] **AC-083**: Masculinas: Gavião (11–12), Albatroz (10), Falcao (13–15)
- [ ] **AC-084**: Femininas: Araras-Azuis (10–12), Beija-Flor (13), Harpia (15)

---

## 10. Qualidade

### 10.1 Testes

- [ ] **AC-085**: ≥70% cobertura Domain+Application
- [ ] **AC-086**: ≥60% cobertura total
- [ ] **AC-087**: Testes para regras críticas implementados

### 10.2 Performance

- [ ] **AC-088**: Paginação obrigatória a partir de Região
- [ ] **AC-089**: Default 50 clubes por Região
- [ ] **AC-090**: Default 25 membros, 20 unidades

### 10.3 Segurança

- [ ] **AC-091**: Rate limit global habilitado
- [ ] **AC-092**: CORS configurado
- [ ] **AC-093**: OpenAPI 3.1 gerado

---

## Resumo de Aceite

**Total de Critérios**: 93
**Implementados**: 0
**Testados**: 0
**Aprovados**: 0

**Status**: ❌ NOT ACCEPTED

**Próxima Ação**: Implementar critérios de aceite e executar testes
