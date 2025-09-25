import React from "react";
import { Chip, Tooltip, Box } from "@mui/material";
import {
  Warning as WarningIcon,
  Error as ErrorIcon,
  Check as CheckIcon,
} from "@mui/icons-material";

interface PendencyIndicatorProps {
  count: number;
  maxCount?: number;
  severity?: "warning" | "error";
  size?: "small" | "medium";
  showIcon?: boolean;
  showSuccess?: boolean;
  onClick?: () => void; // Added onClick prop
  clickable?: boolean; // Added clickable prop
}

/**
 * Componente para exibir indicador de pendências
 * Mostra contador com cores baseadas na severidade
 */
export const PendencyIndicator: React.FC<PendencyIndicatorProps> = ({
  count,
  maxCount = 5,
  severity = "warning",
  size = "small",
  showIcon = true,
  showSuccess = true,
  onClick,
  clickable = false,
}) => {
  // Se não há pendências e showSuccess é true, mostrar tick verde
  if (count === 0) {
    if (showSuccess) {
      return (
        <Tooltip title="Seção completa">
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              width: 24,
              height: 24,
              borderRadius: "50%",
              backgroundColor: "success.light",
              color: "success.contrastText",
              margin: "0 auto", // Centralizar o tick verde
            }}
          >
            <CheckIcon sx={{ fontSize: "0.875rem" }} />
          </Box>
        </Tooltip>
      );
    }
    return null;
  }

  const isHigh = count >= maxCount;
  const actualSeverity = isHigh ? "error" : severity;

  const color = actualSeverity === "error" ? "error" : "warning";
  const icon = actualSeverity === "error" ? <ErrorIcon /> : <WarningIcon />;

  const chipElement = (
    <Chip
      icon={showIcon ? icon : undefined}
      label={count}
      color={color}
      size={size}
      variant="filled"
      onClick={clickable && onClick ? onClick : undefined}
      sx={{
        minWidth: 24,
        height: 24,
        fontSize: "0.75rem",
        fontWeight: "bold",
        cursor: clickable ? "pointer" : "default",
        "& .MuiChip-icon": {
          fontSize: "0.875rem",
        },
        ...(clickable && {
          "&:hover": {
            backgroundColor: `${color}.dark`,
            transform: "scale(1.05)",
          },
          transition: "all 0.2s ease-in-out",
        }),
      }}
    />
  );

  return (
    <Tooltip
      title={
        clickable
          ? `Clique para ver detalhes das ${count} pendência${
              count > 1 ? "s" : ""
            }`
          : `${count} campo${count > 1 ? "s" : ""} pendente${
              count > 1 ? "s" : ""
            }`
      }
    >
      {chipElement}
    </Tooltip>
  );
};
