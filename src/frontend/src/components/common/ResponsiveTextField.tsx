import React from "react";
import { TextField, useMediaQuery, useTheme } from "@mui/material";
import type { TextFieldProps } from "@mui/material/TextField";

interface ResponsiveTextFieldProps extends Omit<TextFieldProps, "size"> {
  responsiveSize?: "small" | "medium" | "large";
  mobileOptimized?: boolean;
}

/**
 * Componente TextField responsivo que se adapta automaticamente ao tamanho da tela
 *
 * @param responsiveSize - Tamanho base do input (small, medium, large)
 * @param mobileOptimized - Se true, otimiza especificamente para mobile
 * @param ...props - Props padrão do TextField do Material-UI
 */
export const ResponsiveTextField: React.FC<ResponsiveTextFieldProps> = ({
  responsiveSize = "medium",
  mobileOptimized = true,
  sx,
  ...props
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isXs = useMediaQuery(theme.breakpoints.down("xs"));

  // Determinar tamanho baseado no breakpoint
  const getSize = (): "small" | "medium" => {
    if (isXs) return "small";
    if (isMobile && mobileOptimized) return "small";
    return responsiveSize === "large" ? "medium" : responsiveSize;
  };

  // Determinar altura mínima para touch targets
  const getMinHeight = () => {
    if (isXs) return "48px"; // 44px + padding para touch
    if (isMobile) return "44px";
    return "40px";
  };

  // Determinar tamanho da fonte
  const getFontSize = () => {
    if (isXs) return "16px"; // Previne zoom no iOS
    if (isMobile) return "16px";
    return "14px";
  };

  // Determinar padding
  const getPadding = () => {
    if (isXs) return "12px 14px";
    if (isMobile) return "10px 14px";
    return "8px 14px";
  };

  return (
    <TextField
      size={getSize()}
      sx={{
        "& .MuiInputBase-input": {
          minHeight: getMinHeight(),
          fontSize: getFontSize(),
          padding: getPadding(),
          // Melhorar legibilidade em telas pequenas
          lineHeight: isMobile ? 1.5 : 1.4,
        },
        "& .MuiInputLabel-root": {
          fontSize: isXs ? "14px" : isMobile ? "15px" : "14px",
          // Ajustar posição do label para telas pequenas
          transform: isXs ? "translate(14px, 12px) scale(1)" : undefined,
          "&.Mui-focused, &.MuiFormLabel-filled": {
            transform: isXs ? "translate(14px, -9px) scale(0.75)" : undefined,
          },
        },
        "& .MuiFormHelperText-root": {
          fontSize: isXs ? "12px" : "13px",
          marginTop: isXs ? "4px" : "2px",
        },
        // Melhorar espaçamento em telas pequenas
        marginBottom: isXs ? "16px" : "8px",
        ...sx,
      }}
      {...props}
    />
  );
};

export default ResponsiveTextField;
