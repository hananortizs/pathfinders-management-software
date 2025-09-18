import type { Components, Theme } from "@mui/material/styles";

/**
 * Overrides e variants globais para componentes MUI
 *
 * Centraliza todas as customizações de componentes para manter
 * consistência visual e facilitar manutenção
 */
export const components: Components<Theme> = {
  // Botões customizados
  MuiButton: {
    styleOverrides: {
      root: {
        borderRadius: 12,
        textTransform: "none",
        fontWeight: 500,
        minHeight: 44, // Tamanho mínimo para acessibilidade
        padding: "8px 16px",
        "&:focus": {
          outline: "2px solid",
          outlineOffset: "2px",
        },
      },
      contained: {
        boxShadow: "0px 2px 4px rgba(0,0,0,0.1)",
        "&:hover": {
          boxShadow: "0px 4px 8px rgba(0,0,0,0.15)",
        },
      },
      outlined: {
        borderWidth: "2px",
        "&:hover": {
          borderWidth: "2px",
        },
      },
    },
    variants: [
      {
        props: { size: "small" },
        style: {
          minHeight: 36,
          padding: "6px 12px",
          fontSize: "0.8rem",
        },
      },
      {
        props: { size: "large" },
        style: {
          minHeight: 52,
          padding: "12px 24px",
          fontSize: "1rem",
        },
      },
    ],
  },

  // Diálogos customizados
  MuiDialog: {
    styleOverrides: {
      paper: {
        borderRadius: 16,
        boxShadow: "0px 8px 32px rgba(0,0,0,0.12)",
      },
    },
  },
  MuiDialogTitle: {
    styleOverrides: {
      root: {
        padding: "24px 24px 16px 24px",
        fontWeight: 600,
        fontSize: "1.25rem",
      },
    },
  },
  MuiDialogContent: {
    styleOverrides: {
      root: {
        padding: "0 24px 16px 24px",
      },
    },
  },
  MuiDialogActions: {
    styleOverrides: {
      root: {
        padding: "16px 24px 24px 24px",
        gap: "12px",
      },
    },
  },

  // Tabelas customizadas
  MuiTable: {
    styleOverrides: {
      root: {
        "& .MuiTableHead-root": {
          "& .MuiTableCell-head": {
            fontWeight: 600,
            backgroundColor: "rgba(0,0,0,0.04)",
            borderBottom: "2px solid",
            borderBottomColor: "rgba(0,0,0,0.12)",
          },
        },
      },
    },
  },
  MuiTableCell: {
    styleOverrides: {
      root: {
        borderBottom: "1px solid rgba(0,0,0,0.08)",
        padding: "12px 16px",
      },
      head: {
        fontWeight: 600,
      },
    },
  },
  MuiTableRow: {
    styleOverrides: {
      root: {
        "&:hover": {
          backgroundColor: "rgba(0,0,0,0.02)",
        },
        "&.Mui-selected": {
          backgroundColor: "rgba(13, 71, 161, 0.08)",
          "&:hover": {
            backgroundColor: "rgba(13, 71, 161, 0.12)",
          },
        },
      },
    },
  },

  // Campos de entrada customizados
  MuiTextField: {
    styleOverrides: {
      root: {
        "& .MuiOutlinedInput-root": {
          borderRadius: 12,
          minHeight: 44,
          "&:hover .MuiOutlinedInput-notchedOutline": {
            borderColor: "rgba(13, 71, 161, 0.5)",
          },
          "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
            borderWidth: "2px",
          },
        },
        "& .MuiInputLabel-root": {
          "&.Mui-focused": {
            color: "#0D47A1",
          },
        },
      },
    },
  },
  MuiInputBase: {
    styleOverrides: {
      root: {
        borderRadius: 12,
        minHeight: 44,
        "&.Mui-focused": {
          "& .MuiOutlinedInput-notchedOutline": {
            borderWidth: "2px",
          },
        },
      },
    },
  },

  // Snackbar e Alert customizados
  MuiSnackbar: {
    styleOverrides: {
      root: {
        "& .MuiAlert-root": {
          borderRadius: 12,
          boxShadow: "0px 4px 12px rgba(0,0,0,0.15)",
        },
      },
    },
  },
  MuiAlert: {
    styleOverrides: {
      root: {
        borderRadius: 12,
        fontWeight: 500,
        "& .MuiAlert-icon": {
          fontSize: "1.25rem",
        },
      },
      standardSuccess: {
        backgroundColor: "#E8F5E8",
        color: "#1B5E20",
        "& .MuiAlert-icon": {
          color: "#388E3C",
        },
      },
      standardError: {
        backgroundColor: "#FFEBEE",
        color: "#B71C1C",
        "& .MuiAlert-icon": {
          color: "#D32F2F",
        },
      },
      standardWarning: {
        backgroundColor: "#FFF8E1",
        color: "#C8A600",
        "& .MuiAlert-icon": {
          color: "#FFD700",
        },
      },
      standardInfo: {
        backgroundColor: "#E3F2FD",
        color: "#0D47A1",
        "& .MuiAlert-icon": {
          color: "#1976D2",
        },
      },
    },
  },

  // Cards customizados
  MuiCard: {
    styleOverrides: {
      root: {
        borderRadius: 16,
        boxShadow: "0px 2px 8px rgba(0,0,0,0.1)",
        "&:hover": {
          boxShadow: "0px 4px 16px rgba(0,0,0,0.15)",
        },
      },
    },
  },
  MuiCardContent: {
    styleOverrides: {
      root: {
        padding: "20px",
        "&:last-child": {
          paddingBottom: "20px",
        },
      },
    },
  },

  // Menu e Popover customizados
  MuiMenu: {
    styleOverrides: {
      paper: {
        borderRadius: 12,
        boxShadow: "0px 4px 20px rgba(0,0,0,0.15)",
        marginTop: "8px",
      },
    },
  },
  MuiPopover: {
    styleOverrides: {
      paper: {
        borderRadius: 12,
        boxShadow: "0px 4px 20px rgba(0,0,0,0.15)",
      },
    },
  },
  MuiMenuItem: {
    styleOverrides: {
      root: {
        borderRadius: 8,
        margin: "4px 8px",
        minHeight: 44,
        "&:hover": {
          backgroundColor: "rgba(13, 71, 161, 0.08)",
        },
        "&.Mui-selected": {
          backgroundColor: "rgba(13, 71, 161, 0.12)",
          "&:hover": {
            backgroundColor: "rgba(13, 71, 161, 0.16)",
          },
        },
      },
    },
  },

  // AppBar customizado
  MuiAppBar: {
    styleOverrides: {
      root: {
        boxShadow: "0px 2px 8px rgba(0,0,0,0.1)",
      },
    },
  },

  // Toolbar customizado
  MuiToolbar: {
    styleOverrides: {
      root: {
        minHeight: "64px !important",
        padding: "0 16px",
      },
    },
  },

  // Chip customizado
  MuiChip: {
    styleOverrides: {
      root: {
        borderRadius: 8,
        fontWeight: 500,
      },
    },
  },

  // Paper customizado
  MuiPaper: {
    styleOverrides: {
      root: {
        borderRadius: 12,
      },
      elevation1: {
        boxShadow: "0px 2px 4px rgba(0,0,0,0.1)",
      },
      elevation2: {
        boxShadow: "0px 4px 8px rgba(0,0,0,0.12)",
      },
      elevation3: {
        boxShadow: "0px 6px 12px rgba(0,0,0,0.15)",
      },
    },
  },
};
