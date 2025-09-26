import React from "react";
import { Box } from "@mui/material";
import type { SxProps, Theme } from "@mui/material";

interface PmsIconProps {
  size?: number;
  variant?: "circular" | "square";
  sx?: SxProps<Theme>;
}

/**
 * Componente do ícone oficial do PMS
 * Pode ser usado em diferentes tamanhos e variantes
 */
export const PmsIcon: React.FC<PmsIconProps> = ({
  size = 40,
  variant = "circular",
  sx = {},
}) => {
  const iconSvg =
    variant === "circular" ? (
      <img
        src="/pms-icon-circular.png"
        alt="PMS Icon"
        width={size}
        height={size}
        style={{
          borderRadius: "50%",
          filter: "drop-shadow(0 4px 12px rgba(13, 71, 161, 0.3))",
          objectFit: "contain",
        }}
      />
    ) : (
      <svg
        width={size}
        height={size}
        viewBox="0 0 1024 1024"
        xmlns="http://www.w3.org/2000/svg"
      >
        <rect width="1024" height="1024" fill="#0D47A1" rx="8" />
        <g transform="translate(512, 512)">
          <path
            fill="#FFFFFF"
            d="M-160,-120 L160,-120 L160,120 L-160,120 Z"
            opacity="0.9"
          />
          <path fill="#0D47A1" d="M-120,-80 L120,-80 L120,80 L-120,80 Z" />
          <path fill="#FFFFFF" d="M-80,-40 L80,-40 L40,40 L-80,40 Z" />
          <circle cx="0" cy="-60" r="16" fill="#FFD700" />
          <circle cx="-40" cy="0" r="12" fill="#FFD700" />
          <circle cx="40" cy="0" r="12" fill="#FFD700" />
          <circle cx="0" cy="60" r="16" fill="#FFD700" />
        </g>
      </svg>
    );

  return (
    <Box
      sx={{
        display: "inline-flex",
        alignItems: "center",
        justifyContent: "center",
        ...sx,
      }}
    >
      {iconSvg}
    </Box>
  );
};
