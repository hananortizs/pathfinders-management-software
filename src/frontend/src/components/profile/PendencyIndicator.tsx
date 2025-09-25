import React from "react";
import { Chip, Tooltip } from "@mui/material";
import {
  Warning as WarningIcon,
  Error as ErrorIcon,
} from "@mui/icons-material";

interface PendencyIndicatorProps {
  count: number;
  maxCount?: number;
  severity?: "warning" | "error";
  size?: "small" | "medium";
  showIcon?: boolean;
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
}) => {
  if (count === 0) return null;

  const isHigh = count >= maxCount;
  const actualSeverity = isHigh ? "error" : severity;

  const color = actualSeverity === "error" ? "error" : "warning";
  const icon = actualSeverity === "error" ? <ErrorIcon /> : <WarningIcon />;

  return (
    <Tooltip
      title={`${count} campo${count > 1 ? "s" : ""} pendente${
        count > 1 ? "s" : ""
      }`}
    >
      <Chip
        icon={showIcon ? icon : undefined}
        label={count}
        color={color}
        size={size}
        variant="filled"
        sx={{
          minWidth: 24,
          height: 24,
          fontSize: "0.75rem",
          fontWeight: "bold",
          "& .MuiChip-icon": {
            fontSize: "0.875rem",
          },
        }}
      />
    </Tooltip>
  );
};
