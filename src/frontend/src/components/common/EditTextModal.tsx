import React, { useState, useEffect } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
  Typography,
  IconButton,
} from "@mui/material";
import {
  Close as CloseIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
} from "@mui/icons-material";

interface EditTextModalProps {
  open: boolean;
  onClose: () => void;
  onSave: (newValue: string) => void;
  currentValue: string;
  title: string;
  subtitle?: string;
  placeholder?: string;
  helperText?: string;
  label?: string;
  maxLength?: number;
  minLength?: number;
  required?: boolean;
  multiline?: boolean;
  rows?: number;
}

/**
 * Modal genérico para edição de texto
 * Pode ser reutilizado em qualquer contexto para editar dados
 *
 * @param open - Se o modal está aberto
 * @param onClose - Função chamada ao fechar o modal
 * @param onSave - Função chamada ao salvar (recebe o novo valor)
 * @param currentValue - Valor atual do campo
 * @param title - Título do modal
 * @param subtitle - Subtítulo opcional (ex: "Endereço #1")
 * @param placeholder - Placeholder do input
 * @param helperText - Texto de ajuda
 * @param label - Label do input
 * @param maxLength - Tamanho máximo do texto
 * @param minLength - Tamanho mínimo do texto
 * @param required - Se o campo é obrigatório
 * @param multiline - Se é um campo de múltiplas linhas
 * @param rows - Número de linhas (quando multiline)
 */
export const EditTextModal: React.FC<EditTextModalProps> = ({
  open,
  onClose,
  onSave,
  currentValue,
  title,
  subtitle,
  placeholder,
  helperText,
  label,
  maxLength = 100,
  minLength = 2,
  required = true,
  multiline = false,
  rows = 1,
}) => {
  const [value, setValue] = useState(currentValue);
  const [error, setError] = useState("");

  // Atualizar o valor quando o modal abrir
  useEffect(() => {
    if (open) {
      setValue(currentValue);
      setError("");
    }
  }, [open, currentValue]);

  const validateValue = (val: string): string => {
    const trimmedVal = val.trim();

    if (required && !trimmedVal) {
      return `${label || "Campo"} é obrigatório`;
    }

    if (trimmedVal && trimmedVal.length < minLength) {
      return `${label || "Campo"} deve ter pelo menos ${minLength} caracteres`;
    }

    if (trimmedVal.length > maxLength) {
      return `${label || "Campo"} deve ter no máximo ${maxLength} caracteres`;
    }

    return "";
  };

  const handleSave = () => {
    const trimmedValue = value.trim();
    const validationError = validateValue(trimmedValue);

    if (validationError) {
      setError(validationError);
      return;
    }

    onSave(trimmedValue);
    onClose();
  };

  const handleCancel = () => {
    setValue(currentValue);
    setError("");
    onClose();
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && !multiline) {
      handleSave();
    } else if (e.key === "Escape") {
      handleCancel();
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setValue(newValue);

    // Limpar erro quando o usuário começar a digitar
    if (error) {
      setError("");
    }
  };

  return (
    <Dialog
      open={open}
      onClose={handleCancel}
      maxWidth="sm"
      fullWidth
      PaperProps={{
        sx: {
          borderRadius: 2,
          boxShadow: "0 8px 32px rgba(0,0,0,0.12)",
        },
      }}
    >
      <DialogTitle
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          pb: 1,
        }}
      >
        <Typography variant="h6" component="div">
          {title}
        </Typography>
        <IconButton
          onClick={handleCancel}
          size="small"
          sx={{ color: "text.secondary" }}
        >
          <CloseIcon />
        </IconButton>
      </DialogTitle>

      <DialogContent sx={{ pt: 2 }}>
        <Box sx={{ mb: 2 }}>
          {subtitle && (
            <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
              {subtitle}
            </Typography>
          )}
          <TextField
            fullWidth
            value={value}
            onChange={handleChange}
            onKeyDown={handleKeyPress}
            placeholder={placeholder}
            label={label}
            error={!!error}
            helperText={error || helperText}
            autoFocus
            variant="outlined"
            multiline={multiline}
            rows={multiline ? rows : 1}
            inputProps={{
              maxLength: maxLength,
            }}
            sx={{
              "& .MuiOutlinedInput-root": {
                fontSize: "1rem",
              },
            }}
          />
          {maxLength && (
            <Typography
              variant="caption"
              color="text.secondary"
              sx={{ mt: 0.5, display: "block" }}
            >
              {value.length}/{maxLength} caracteres
            </Typography>
          )}
        </Box>
      </DialogContent>

      <DialogActions sx={{ p: 3, pt: 1 }}>
        <Button
          onClick={handleCancel}
          startIcon={<CancelIcon />}
          variant="outlined"
          color="inherit"
        >
          Cancelar
        </Button>
        <Button
          onClick={handleSave}
          startIcon={<SaveIcon />}
          variant="contained"
          color="primary"
          disabled={required && !value.trim()}
        >
          Salvar
        </Button>
      </DialogActions>
    </Dialog>
  );
};
