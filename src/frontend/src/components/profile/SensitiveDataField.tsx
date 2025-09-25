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
            setTimeout(() => {
              onHide();
              setLocalTimeRemaining(0);
            }, 0);
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

  // Sincronizar estado local com prop isRevealed
  useEffect(() => {
    if (!isRevealed) {
      setLocalTimeRemaining(0);
    }
  }, [isRevealed]);

  const handleReveal = async () => {
    if (isRevealing) return; // Previne múltiplos cliques

    setIsRevealing(true);
    try {
      await onReveal();
      setLocalTimeRemaining(10); // Reset para 10 segundos
    } catch (error) {
      console.error("Erro ao revelar dados sensíveis:", error);
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
        // Mascarar parte local do email
        const maskedLocal =
          localPart.length > 2
            ? `${localPart[0]}${"•".repeat(localPart.length - 2)}${
                localPart[localPart.length - 1]
              }`
            : localPart.length === 2
            ? `${localPart[0]}•`
            : "•";

        // Mascarar domínio mantendo a estrutura
        const domainParts = domain.split(".");
        const maskedDomain = domainParts
          .map((part, index) => {
            if (index === 0) {
              // Primeira parte do domínio: exemplo -> ex•••••
              return part.length > 2
                ? `${part[0]}${part[1]}${"•".repeat(part.length - 2)}`
                : part;
            }
            return part; // Extensões como .com, .org ficam visíveis
          })
          .join(".");

        return `${maskedLocal}@${maskedDomain}`;
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
    <Box sx={{ width: "100%" }}>
      {/* Campo de input */}
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
        }}
        sx={{
          "& .MuiInputBase-input": {
            fontFamily: isRevealed ? "inherit" : "monospace",
            letterSpacing: isRevealed ? "normal" : "0.1em",
            paddingRight: "16px", // Espaço padrão
          },
          "& .MuiInputBase-input:disabled": {
            color: "text.primary",
            WebkitTextFillColor: "unset",
          },
        }}
      />

      {/* Controles fora do campo */}
      <Box sx={{ 
        display: "flex", 
        alignItems: "center", 
        justifyContent: "space-between",
        mt: 1,
        gap: 1,
      }}>
        {/* Timer e status */}
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          {isRevealed && localTimeRemaining > 0 && (
            <Chip
              label={`${localTimeRemaining}s`}
              size="small"
              color="warning"
              variant="outlined"
              sx={{ 
                fontSize: "0.7rem", 
                height: 24,
                minWidth: "auto",
                px: 1,
              }}
            />
          )}
          
          {!isRevealed && (
            <Typography
              variant="caption"
              color="text.secondary"
              sx={{
                fontSize: "0.75rem",
              }}
            >
              Dados sensíveis
            </Typography>
          )}
        </Box>

        {/* Botão de revelar/ocultar */}
        <Tooltip title={isRevealed ? "Ocultar dados" : "Revelar por 10s"}>
          <span>
            <IconButton
              size="small"
              onClick={isRevealed ? handleHide : handleReveal}
              disabled={isRevealing}
              sx={{
                color: isRevealed ? "error.main" : "primary.main",
                width: 36,
                height: 36,
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
    </Box>
  );
};
