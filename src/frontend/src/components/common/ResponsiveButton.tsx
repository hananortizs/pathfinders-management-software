import React from "react";
import { Button, useMediaQuery, useTheme } from "@mui/material";
import type { ButtonProps } from "@mui/material/Button";

interface ResponsiveButtonProps extends ButtonProps {
  responsiveSize?: "small" | "medium" | "large";
  mobileOptimized?: boolean;
  fullWidthOnMobile?: boolean;
}

/**
 * Componente Button responsivo que se adapta automaticamente ao tamanho da tela
 *
 * @param responsiveSize - Tamanho base do botão (small, medium, large)
 * @param mobileOptimized - Se true, otimiza especificamente para mobile
 * @param fullWidthOnMobile - Se true, ocupa largura total em mobile
 * @param ...props - Props padrão do Button do Material-UI
 */
export const ResponsiveButton: React.FC<ResponsiveButtonProps> = ({
  responsiveSize = "medium",
  mobileOptimized = true,
  fullWidthOnMobile = false,
  sx,
  ...props
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isXs = useMediaQuery(theme.breakpoints.down("xs"));

  // Determinar tamanho baseado no breakpoint
  const getSize = () => {
    if (isXs) return "small";
    if (isMobile && mobileOptimized) return "small";
    return responsiveSize;
  };

  // Determinar altura mínima para touch targets
  const getMinHeight = () => {
    if (isXs) return "44px"; // Touch target mínimo
    if (isMobile) return "40px";
    return "36px";
  };

  // Determinar tamanho da fonte
  const getFontSize = () => {
    if (isXs) return "14px";
    if (isMobile) return "14px";
    return "13px";
  };

  // Determinar padding
  const getPadding = () => {
    if (isXs) return "8px 16px";
    if (isMobile) return "6px 16px";
    return "4px 16px";
  };

  return (
    <Button
      size={getSize()}
      fullWidth={fullWidthOnMobile && isMobile}
      sx={{
        minHeight: getMinHeight(),
        fontSize: getFontSize(),
        padding: getPadding(),
        // Melhorar espaçamento entre ícone e texto
        "& .MuiButton-startIcon": {
          marginRight: isXs ? "6px" : "8px",
        },
        "& .MuiButton-endIcon": {
          marginLeft: isXs ? "6px" : "8px",
        },
        // Melhorar contraste em telas pequenas
        fontWeight: isMobile ? 500 : 400,
        ...sx,
      }}
      {...props}
    />
  );
};

export default ResponsiveButton;
