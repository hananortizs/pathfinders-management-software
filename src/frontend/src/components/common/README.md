# 📦 Componentes Comuns Reutilizáveis

## 🎯 EditTextModal

Modal genérico para edição de texto que pode ser reutilizado em qualquer contexto da aplicação.

### 📋 Props

| Prop           | Tipo                         | Obrigatório | Padrão  | Descrição                                      |
| -------------- | ---------------------------- | ----------- | ------- | ---------------------------------------------- |
| `open`         | `boolean`                    | ✅          | -       | Se o modal está aberto                         |
| `onClose`      | `() => void`                 | ✅          | -       | Função chamada ao fechar o modal               |
| `onSave`       | `(newValue: string) => void` | ✅          | -       | Função chamada ao salvar (recebe o novo valor) |
| `currentValue` | `string`                     | ✅          | -       | Valor atual do campo                           |
| `title`        | `string`                     | ✅          | -       | Título do modal                                |
| `subtitle`     | `string`                     | ❌          | -       | Subtítulo opcional (ex: "Endereço #1")         |
| `placeholder`  | `string`                     | ❌          | -       | Placeholder do input                           |
| `helperText`   | `string`                     | ❌          | -       | Texto de ajuda                                 |
| `label`        | `string`                     | ❌          | -       | Label do input                                 |
| `maxLength`    | `number`                     | ❌          | `100`   | Tamanho máximo do texto                        |
| `minLength`    | `number`                     | ❌          | `2`     | Tamanho mínimo do texto                        |
| `required`     | `boolean`                    | ❌          | `true`  | Se o campo é obrigatório                       |
| `multiline`    | `boolean`                    | ❌          | `false` | Se é um campo de múltiplas linhas              |
| `rows`         | `number`                     | ❌          | `1`     | Número de linhas (quando multiline)            |

### 🚀 Exemplos de Uso

#### 1. Editar Nome do Endereço

```typescript
<EditTextModal
  open={editNameModalOpen}
  onClose={handleCloseNameModal}
  onSave={handleSaveName}
  currentValue={address.name}
  title="Editar Nome do Endereço"
  subtitle="Endereço #1"
  placeholder="Ex: Casa, Trabalho, Endereço Principal..."
  helperText="Digite um nome descritivo para este endereço"
  label="Nome do Endereço"
  maxLength={100}
  minLength={2}
  required={true}
/>
```

#### 2. Editar Descrição de Evento

```typescript
<EditTextModal
  open={editDescriptionModalOpen}
  onClose={handleCloseDescriptionModal}
  onSave={handleSaveDescription}
  currentValue={event.description}
  title="Editar Descrição do Evento"
  placeholder="Descreva o evento..."
  helperText="Forneça uma descrição detalhada do evento"
  label="Descrição"
  maxLength={500}
  minLength={10}
  multiline={true}
  rows={4}
  required={true}
/>
```

#### 3. Editar Observações (Opcional)

```typescript
<EditTextModal
  open={editNotesModalOpen}
  onClose={handleCloseNotesModal}
  onSave={handleSaveNotes}
  currentValue={member.notes || ""}
  title="Editar Observações"
  placeholder="Adicione observações sobre o membro..."
  helperText="Campo opcional para observações adicionais"
  label="Observações"
  maxLength={1000}
  required={false}
  multiline={true}
  rows={3}
/>
```

### ✨ Funcionalidades

- **Validação automática**: Valida comprimento mínimo, máximo e obrigatoriedade
- **Contador de caracteres**: Mostra quantos caracteres foram digitados
- **Atalhos de teclado**:
  - `Enter` para salvar (quando não é multiline)
  - `Escape` para cancelar
- **Feedback visual**: Mostra erros de validação em tempo real
- **Responsivo**: Adapta-se a diferentes tamanhos de tela
- **Acessível**: Suporte completo a leitores de tela

### 🎨 Estilo

- **Design consistente** com o Material-UI
- **Animações suaves** de abertura/fechamento
- **Sombras elegantes** para profundidade
- **Cores do tema** da aplicação

### 🔧 Implementação

1. **Importe o componente**:

   ```typescript
   import { EditTextModal } from "../common/EditTextModal";
   ```

2. **Adicione o estado**:

   ```typescript
   const [modalOpen, setModalOpen] = useState(false);
   const [editingIndex, setEditingIndex] = useState<number | null>(null);
   ```

3. **Crie as funções de controle**:

   ```typescript
   const handleEdit = (index: number) => {
     setEditingIndex(index);
     setModalOpen(true);
   };

   const handleSave = (newValue: string) => {
     // Lógica para salvar
     setModalOpen(false);
     setEditingIndex(null);
   };

   const handleClose = () => {
     setModalOpen(false);
     setEditingIndex(null);
   };
   ```

4. **Adicione o modal no JSX**:
   ```typescript
   <EditTextModal
     open={modalOpen}
     onClose={handleClose}
     onSave={handleSave}
     currentValue={editingIndex !== null ? data[editingIndex].field : ""}
     title="Editar Campo"
     // ... outras props
   />
   ```

### 🎯 Casos de Uso

- ✅ **Nomes de endereços**
- ✅ **Descrições de eventos**
- ✅ **Observações de membros**
- ✅ **Títulos de relatórios**
- ✅ **Comentários gerais**
- ✅ **Qualquer campo de texto editável**

### 🚀 Vantagens

- **Reutilizável**: Um componente para todos os casos
- **Consistente**: Mesma UX em toda a aplicação
- **Manutenível**: Mudanças em um lugar só
- **Flexível**: Configurável para diferentes necessidades
- **Testável**: Fácil de testar isoladamente
