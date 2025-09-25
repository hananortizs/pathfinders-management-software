import React, { useState, useEffect } from "react";
import {
  Box,
  TextField,
  IconButton,
  Typography,
  Chip,
  Tooltip,
  CircularProgress,
} from "@mui/material";
import {
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  Lock as LockIcon,
} from "@mui/icons-material";
import { parsePhoneNumber } from "react-phone-number-input";
import type { SensitiveFieldProps } from "../../types/profile";

/**
 * Componente para exibir campos sensíveis com funcionalidade "Revelar por 10s"
 * Inclui timer automático, auditoria e mascaramento de dados
 */
export const SensitiveDataField: React.FC<SensitiveFieldProps> = ({
  value,
  isRevealed,
  onReveal,
  onHide,
  timeRemaining = 0,
  isEditable = false,
  onChange,
  placeholder = "••••••••",
  type = "text",
  mask,
}) => {
  const [localTimeRemaining, setLocalTimeRemaining] = useState(timeRemaining);
  const [isRevealing, setIsRevealing] = useState(false);

  // Timer para contagem regressiva
  useEffect(() => {
    if (isRevealed && localTimeRemaining > 0) {
      const timer = setInterval(() => {
        setLocalTimeRemaining((prev) => {
          if (prev <= 1) {
            // Usar setTimeout para evitar setState durante render
            setTimeout(() => onHide(), 0);
            return 0;
          }
          return prev - 1;
        });
      }, 1000);

      return () => clearInterval(timer);
    }
  }, [isRevealed, localTimeRemaining, onHide]);

  // Atualizar timer local quando prop muda
  useEffect(() => {
    setLocalTimeRemaining(timeRemaining);
  }, [timeRemaining]);

  const handleReveal = async () => {
    setIsRevealing(true);
    try {
      await onReveal();
      setLocalTimeRemaining(10); // Reset para 10 segundos
    } finally {
      setIsRevealing(false);
    }
  };

  const handleHide = () => {
    onHide();
    setLocalTimeRemaining(0);
  };

  const formatValue = (val: string) => {
    if (!val) return "";

    if (mask) {
      // Aplicar máscara (ex: CPF: 000.000.000-00)
      return val.replace(/\d/g, (match, index) => {
        const maskChar = mask[index];
        return maskChar === "0" ? match : maskChar || match;
      });
    }

    // Para telefones, aplicar formatação se necessário
    if (type === "tel" && val.startsWith("+")) {
      try {
        const parsed = parsePhoneNumber(val);
        if (parsed) {
          return parsed.formatInternational();
        }
      } catch (error) {
        // Se não conseguir fazer parse, retorna o valor original
        console.warn("Erro ao formatar telefone:", error);
      }
    }

    return val;
  };

  const getMaskedValue = () => {
    if (!value) return "";

    if (type === "email") {
      const [localPart, domain] = value.split("@");
      if (localPart && domain) {
        const maskedLocal =
          localPart.length > 2
            ? `${localPart[0]}${"•".repeat(localPart.length - 2)}${
                localPart[localPart.length - 1]
              }`
            : "••";
        return `${maskedLocal}@${domain}`;
      }
      return "••••••••@••••••••";
    }

    if (type === "tel") {
      // Para telefones, manter a estrutura mas mascarar os números
      if (value.startsWith("+")) {
        // Telefone internacional: +55 11 99999-9999 -> +•• •• •••••-••••
        return value.replace(/\d/g, "•");
      } else {
        // Telefone nacional: (11) 99999-9999 -> (••) •••••-••••
        return value.replace(/\d/g, "•");
      }
    }

    if (type === "date") {
      return "••/••/••••";
    }

    // Para outros tipos, mascarar com bullets
    return "•".repeat(Math.min(value.length, 8));
  };

  const displayValue = isRevealed ? formatValue(value) : getMaskedValue();

  return (
    <Box sx={{ display: "flex", alignItems: "center", gap: 1, width: "100%" }}>
      <TextField
        fullWidth
        type={isRevealed ? type : "text"}
        value={displayValue}
        onChange={(e) => isEditable && onChange?.(e.target.value)}
        placeholder={placeholder}
        disabled={!isEditable || !isRevealed}
        InputProps={{
          startAdornment: (
            <LockIcon
              sx={{
                color: "text.secondary",
                mr: 1,
                fontSize: "1rem",
              }}
            />
          ),
          endAdornment: (
            <Box sx={{ display: "flex", alignItems: "center", gap: 0.5 }}>
              {isRevealed && localTimeRemaining > 0 && (
                <Chip
                  label={`${localTimeRemaining}s`}
                  size="small"
                  color="warning"
                  variant="outlined"
                  sx={{ fontSize: "0.75rem", height: 24 }}
                />
              )}

              <Tooltip title={isRevealed ? "Ocultar dados" : "Revelar por 10s"}>
                <span>
                  <IconButton
                    size="small"
                    onClick={isRevealed ? handleHide : handleReveal}
                    disabled={isRevealing}
                    sx={{
                      color: isRevealed ? "error.main" : "primary.main",
                      "&:hover": {
                        backgroundColor: isRevealed
                          ? "error.light"
                          : "primary.light",
                      },
                    }}
                  >
                    {isRevealing ? (
                      <CircularProgress size={16} />
                    ) : isRevealed ? (
                      <VisibilityOffIcon fontSize="small" />
                    ) : (
                      <VisibilityIcon fontSize="small" />
                    )}
                  </IconButton>
                </span>
              </Tooltip>
            </Box>
          ),
        }}
        sx={{
          "& .MuiInputBase-input": {
            fontFamily: isRevealed ? "inherit" : "monospace",
            letterSpacing: isRevealed ? "normal" : "0.1em",
          },
          "& .MuiInputBase-input:disabled": {
            color: "text.primary",
            WebkitTextFillColor: "unset",
          },
        }}
      />

      {!isRevealed && (
        <Typography
          variant="caption"
          color="text.secondary"
          sx={{
            whiteSpace: "nowrap",
            fontSize: "0.75rem",
          }}
        >
          Dados sensíveis
        </Typography>
      )}
    </Box>
  );
};
