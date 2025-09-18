# 📊 Sistema de Atividades Recentes - Pathfinders Management Software

## 🎯 **VISÃO GERAL**

O sistema de atividades recentes foi projetado para fornecer uma visão contextualizada das atividades baseada no nível hierárquico do usuário. Cada nível da hierarquia vê atividades relevantes ao seu escopo de responsabilidade.

## 🏛️ **ESTRUTURA HIERÁRQUICA**

```
Divisão → União → Associação → Região → Distrito → Clube → Unidade → Membro
```

## 📋 **ATIVIDADES POR NÍVEL**

### 🏘️ **NÍVEL CLUBE** (Diretor/Secretário)

**Atividades mostradas:**

- ✅ **Novos membros** ingressando no clube
- ✅ **Conclusão de especialidades** por membros
- ✅ **Investiduras de lenço** realizadas
- ✅ **Alocações/realocações** de unidades
- ✅ **Eventos do clube** criados/realizados
- ✅ **Tarefas pendentes** (alocação, aprovações)
- ✅ **Promoções automáticas** de membros

### 🏢 **NÍVEIS SUPERIORES** (Distrito, Região, Associação, União, Divisão)

**Atividades mostradas:**

- 📈 **Timeline consolidada** de todos os clubes sob sua jurisdição
- 📊 **Estatísticas agregadas** (novos membros, especialidades, investiduras)
- 🎯 **Eventos regionais/divisórios** criados
- 👥 **Mudanças de liderança** (pastores, regionais, distritais)
- 📋 **Relatórios de progresso** dos clubes
- ⚠️ **Alertas de clubes inativos** ou com problemas
- 🏆 **Conquistas especiais** (primeira especialidade, investidura especial)

## 🔄 **SISTEMA DE TIMELINE**

### **Tipos de Atividades Disponíveis**

```csharp
public enum TimelineEntryType
{
    MembershipStarted,      // Membro ingressou
    MembershipEnded,        // Membro saiu
    UnitAllocated,          // Alocado em unidade
    UnitReallocated,        // Realocado entre unidades
    UnitOverride,           // Alocação manual
    RoleAssigned,           // Cargo atribuído
    RoleRemoved,            // Cargo removido
    EventParticipation,     // Participação em evento
    ScarfInvested,          // Investidura de lenço
    AutoPromotion,          // Promoção automática
    ManualPromotion,        // Promoção manual
    ManualDemotion          // Rebaixamento
}
```

### **Prioridades das Atividades**

- **🔴 Alta Prioridade**: Investiduras de lenço, novos membros, atribuições de cargo
- **🟡 Média Prioridade**: Participações em eventos, alocações de unidade
- **🟢 Baixa Prioridade**: Outras atividades gerais

## 🛠️ **IMPLEMENTAÇÃO TÉCNICA**

### **Arquitetura**

```
RecentActivitiesService
├── IRecentActivitiesService (Interface)
├── AccessLevel (Enum)
└── Métodos por Nível Hierárquico
    ├── GetClubActivitiesAsync()
    ├── GetDistrictActivitiesAsync()
    ├── GetRegionActivitiesAsync()
    ├── GetAssociationActivitiesAsync()
    ├── GetUnionActivitiesAsync()
    ├── GetDivisionActivitiesAsync()
    └── GetSystemAdminActivitiesAsync()
```

### **Fluxo de Funcionamento**

1. **Determinação do Nível**: Baseado nos `userScopes` do usuário
2. **Busca de Dados**: Query otimizada na tabela `TimelineEntry`
3. **Filtragem**: Apenas atividades relevantes ao escopo
4. **Mapeamento**: Conversão para `RecentActivityDto`
5. **Priorização**: Ordenação por data e prioridade

### **Otimizações**

- **Índices**: Criados para `MemberId`, `Type`, `EventDateUtc`
- **Includes**: Carregamento eficiente de relacionamentos
- **Limits**: Controle de quantidade de resultados
- **Caching**: Preparado para implementação futura

## 📊 **EXEMPLOS DE ATIVIDADES**

### **Para Diretor de Clube**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "type": "NovoMembro",
  "description": "João Silva ingressou no clube",
  "date": "2024-01-15T10:30:00Z",
  "memberName": "João Silva",
  "clubName": "Pássaro Celeste",
  "status": "Concluido",
  "priority": "High"
}
```

### **Para Regional**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174001",
  "type": "InvestiduraLenco",
  "description": "Maria Santos recebeu investidura de lenço",
  "date": "2024-01-14T15:45:00Z",
  "memberName": "Maria Santos",
  "clubName": "Pássaro Celeste",
  "status": "Concluido",
  "priority": "High"
}
```

## 🔧 **CONFIGURAÇÃO**

### **Registro de Serviços**

```csharp
// Program.cs
builder.Services.AddScoped<IRecentActivitiesService, RecentActivitiesService>();
```

### **Injeção de Dependência**

```csharp
// DashboardService.cs
public DashboardService(
    ILogger<DashboardService> logger,
    IRecentActivitiesService recentActivitiesService)
{
    _logger = logger;
    _recentActivitiesService = recentActivitiesService;
}
```

## 🚀 **BENEFÍCIOS**

1. **Contextualização**: Cada usuário vê apenas o que é relevante
2. **Performance**: Queries otimizadas por nível hierárquico
3. **Escalabilidade**: Suporta crescimento da organização
4. **Flexibilidade**: Fácil adição de novos tipos de atividades
5. **Auditoria**: Timeline completa de todas as atividades

## 📈 **PRÓXIMOS PASSOS**

1. **Cache Redis**: Implementar cache para melhor performance
2. **Notificações**: Sistema de notificações em tempo real
3. **Filtros Avançados**: Filtros por data, tipo, prioridade
4. **Exportação**: Exportar atividades para CSV/PDF
5. **Analytics**: Dashboard de métricas e tendências

## 🔍 **MONITORAMENTO**

- **Logs**: Todas as operações são logadas
- **Métricas**: Tempo de resposta e volume de dados
- **Alertas**: Notificações para erros críticos
- **Auditoria**: Rastreamento de acessos e modificações
