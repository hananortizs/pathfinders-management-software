import type { SxProps, Theme } from "@mui/material/styles";

/**
 * Utilitários de estilo comuns para a aplicação
 *
 * Centraliza estilos reutilizáveis que podem ser aplicados
 * via sx prop em componentes MUI
 */

/**
 * Estilos para layout de página
 */
export const pageStyles: SxProps<Theme> = {
  minHeight: "100vh",
  display: "flex",
  flexDirection: "column",
};

/**
 * Estilos para container principal
 */
export const containerStyles: SxProps<Theme> = {
  flex: 1,
  py: 4,
  px: { xs: 2, sm: 3, md: 4 },
};

/**
 * Estilos para seções de conteúdo
 */
export const sectionStyles: SxProps<Theme> = {
  mb: 4,
  "&:last-child": {
    mb: 0,
  },
};

/**
 * Estilos para cards de conteúdo
 */
export const cardStyles: SxProps<Theme> = {
  p: 3,
  borderRadius: 2,
  boxShadow: 2,
  "&:hover": {
    boxShadow: 4,
  },
};

/**
 * Estilos para formulários
 */
export const formStyles: SxProps<Theme> = {
  display: "flex",
  flexDirection: "column",
  gap: 3,
  "& .MuiFormControl-root": {
    mb: 0,
  },
};

/**
 * Estilos para botões de ação
 */
export const actionButtonStyles: SxProps<Theme> = {
  minWidth: 120,
  minHeight: 44,
  borderRadius: 1.5,
  textTransform: "none",
  fontWeight: 500,
};

/**
 * Estilos para tabelas
 */
export const tableStyles: SxProps<Theme> = {
  "& .MuiTableHead-root": {
    "& .MuiTableCell-head": {
      fontWeight: 600,
      backgroundColor: "rgba(0,0,0,0.04)",
    },
  },
  "& .MuiTableRow-root": {
    "&:hover": {
      backgroundColor: "rgba(0,0,0,0.02)",
    },
  },
};

/**
 * Estilos para modais/diálogos
 */
export const modalStyles: SxProps<Theme> = {
  "& .MuiDialog-paper": {
    borderRadius: 2,
    boxShadow: "0px 8px 32px rgba(0,0,0,0.12)",
  },
  "& .MuiDialogTitle-root": {
    fontWeight: 600,
    fontSize: "1.25rem",
  },
};

/**
 * Estilos para navegação
 */
export const navigationStyles: SxProps<Theme> = {
  "& .MuiAppBar-root": {
    boxShadow: "0px 2px 8px rgba(0,0,0,0.1)",
  },
  "& .MuiToolbar-root": {
    minHeight: "64px !important",
  },
};

/**
 * Estilos para loading/spinner
 */
export const loadingStyles: SxProps<Theme> = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  minHeight: 200,
};

/**
 * Estilos para mensagens de erro
 */
export const errorStyles: SxProps<Theme> = {
  color: "error.main",
  fontWeight: 500,
  fontSize: "0.875rem",
};

/**
 * Estilos para mensagens de sucesso
 */
export const successStyles: SxProps<Theme> = {
  color: "success.main",
  fontWeight: 500,
  fontSize: "0.875rem",
};

/**
 * Estilos para texto de ajuda
 */
export const helperTextStyles: SxProps<Theme> = {
  color: "text.secondary",
  fontSize: "0.75rem",
  mt: 0.5,
};

/**
 * Estilos para badges/chips
 */
export const badgeStyles: SxProps<Theme> = {
  borderRadius: 1,
  fontWeight: 500,
  fontSize: "0.75rem",
};

/**
 * Estilos para avatares
 */
export const avatarStyles: SxProps<Theme> = {
  width: 40,
  height: 40,
  fontSize: "1rem",
  fontWeight: 600,
};

/**
 * Estilos para ícones
 */
export const iconStyles: SxProps<Theme> = {
  fontSize: "1.25rem",
  color: "text.secondary",
};

/**
 * Estilos para divisores
 */
export const dividerStyles: SxProps<Theme> = {
  my: 2,
  borderColor: "rgba(0,0,0,0.08)",
};

/**
 * Estilos para listas
 */
export const listStyles: SxProps<Theme> = {
  "& .MuiListItem-root": {
    borderRadius: 1,
    mb: 0.5,
    "&:hover": {
      backgroundColor: "rgba(0,0,0,0.02)",
    },
  },
};

/**
 * Estilos para tooltips
 */
export const tooltipStyles: SxProps<Theme> = {
  "& .MuiTooltip-tooltip": {
    backgroundColor: "rgba(0,0,0,0.8)",
    fontSize: "0.75rem",
    borderRadius: 1,
  },
};

/**
 * Estilos para progress bars
 */
export const progressStyles: SxProps<Theme> = {
  "& .MuiLinearProgress-root": {
    borderRadius: 1,
    height: 6,
  },
  "& .MuiCircularProgress-root": {
    color: "primary.main",
  },
};

/**
 * Estilos para tabs
 */
export const tabStyles: SxProps<Theme> = {
  "& .MuiTab-root": {
    textTransform: "none",
    fontWeight: 500,
    minHeight: 48,
  },
  "& .MuiTabs-indicator": {
    height: 3,
    borderRadius: "3px 3px 0 0",
  },
};

/**
 * Estilos para accordion
 */
export const accordionStyles: SxProps<Theme> = {
  "& .MuiAccordionSummary-root": {
    minHeight: 48,
  },
  "& .MuiAccordionDetails-root": {
    pt: 0,
  },
};

/**
 * Estilos para stepper
 */
export const stepperStyles: SxProps<Theme> = {
  "& .MuiStepLabel-root": {
    "& .MuiStepLabel-label": {
      fontWeight: 500,
    },
  },
  "& .MuiStepConnector-root": {
    "&.Mui-active, &.Mui-completed": {
      "& .MuiStepConnector-line": {
        borderColor: "primary.main",
      },
    },
  },
};
