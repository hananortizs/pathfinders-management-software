import { styled, TextField, InputLabel, FormControl } from "@mui/material";
import type {
  TextFieldProps,
  InputLabelProps,
  FormControlProps,
} from "@mui/material";

/**
 * Campo de texto padrão da aplicação
 *
 * TextField customizado com bordas arredondadas, altura consistente
 * e estados de erro/sucesso alinhados ao tema
 */
export const AppTextField = styled(TextField)<TextFieldProps>(({ theme }) => ({
  "& .MuiOutlinedInput-root": {
    borderRadius: theme.shape.borderRadius,
    minHeight: 44,
    backgroundColor: theme.palette.background.paper,
    "&:hover .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.palette.primary.light,
    },
    "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
      borderWidth: "2px",
      borderColor: theme.palette.primary.main,
    },
    "&.Mui-error .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.palette.error.main,
    },
    "&.Mui-disabled .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.palette.action.disabled,
    },
  },
  "& .MuiInputLabel-root": {
    "&.Mui-focused": {
      color: theme.palette.primary.main,
    },
    "&.Mui-error": {
      color: theme.palette.error.main,
    },
  },
  "& .MuiFormHelperText-root": {
    "&.Mui-error": {
      color: theme.palette.error.main,
      fontWeight: 500,
    },
  },
}));

/**
 * Campo de texto para busca
 *
 * TextField especializado para funcionalidades de busca com ícone
 */
export const SearchTextField = styled(AppTextField)<TextFieldProps>(
  ({ theme }) => ({
    "& .MuiOutlinedInput-root": {
      paddingRight: "12px",
      "& .MuiInputAdornment-root": {
        color: theme.palette.text.secondary,
      },
    },
    "& .MuiInputBase-input": {
      padding: "12px 14px",
    },
  })
);

/**
 * Campo de texto para senha
 *
 * TextField especializado para entrada de senhas com toggle de visibilidade
 */
export const PasswordTextField = styled(AppTextField)<TextFieldProps>(
  ({ theme }) => ({
    "& .MuiInputAdornment-root": {
      "& .MuiIconButton-root": {
        color: theme.palette.text.secondary,
        "&:hover": {
          color: theme.palette.primary.main,
        },
      },
    },
  })
);

/**
 * Campo de texto multilinha
 *
 * TextField especializado para textos longos com altura ajustável
 */
export const MultilineTextField = styled(AppTextField)<TextFieldProps>(() => ({
  "& .MuiOutlinedInput-root": {
    minHeight: "auto",
    "& textarea": {
      minHeight: "100px",
      resize: "vertical",
    },
  },
}));

/**
 * Label customizado para formulários
 *
 * InputLabel com estilo consistente e acessibilidade aprimorada
 */
export const AppInputLabel = styled(InputLabel)<InputLabelProps>(
  ({ theme }) => ({
    color: theme.palette.text.primary,
    fontWeight: 500,
    marginBottom: "8px",
    "&.Mui-focused": {
      color: theme.palette.primary.main,
    },
    "&.Mui-error": {
      color: theme.palette.error.main,
    },
    "&.Mui-disabled": {
      color: theme.palette.action.disabled,
    },
  })
);

/**
 * Container de formulário customizado
 *
 * FormControl com espaçamento e layout otimizado
 */
export const AppFormControl = styled(FormControl)<FormControlProps>(
  ({ theme }) => ({
    marginBottom: theme.spacing(2),
    "&:last-child": {
      marginBottom: 0,
    },
    "& .MuiFormLabel-root": {
      color: theme.palette.text.primary,
      fontWeight: 500,
      marginBottom: "8px",
    },
    "& .MuiFormHelperText-root": {
      marginTop: "4px",
      fontSize: "0.75rem",
      "&.Mui-error": {
        color: theme.palette.error.main,
        fontWeight: 500,
      },
    },
  })
);

/**
 * Campo de texto com validação em tempo real
 *
 * TextField com indicadores visuais de validação
 */
export const ValidatedTextField = styled(AppTextField)<
  TextFieldProps & { isValid?: boolean }
>(({ theme, isValid }) => ({
  "& .MuiOutlinedInput-root": {
    ...(isValid === true && {
      "& .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.palette.success.main,
      },
      "&:hover .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.palette.success.main,
      },
    }),
    ...(isValid === false && {
      "& .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.palette.error.main,
      },
      "&:hover .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.palette.error.main,
      },
    }),
  },
  "& .MuiInputLabel-root": {
    ...(isValid === true && {
      color: theme.palette.success.main,
    }),
    ...(isValid === false && {
      color: theme.palette.error.main,
    }),
  },
}));

/**
 * Campo de texto compacto
 *
 * TextField com altura reduzida para uso em tabelas e listas
 */
export const CompactTextField = styled(AppTextField)<TextFieldProps>(() => ({
  "& .MuiOutlinedInput-root": {
    minHeight: 36,
    "& .MuiInputBase-input": {
      padding: "8px 12px",
      fontSize: "0.875rem",
    },
  },
  "& .MuiInputLabel-root": {
    fontSize: "0.875rem",
    "&.MuiInputLabel-shrink": {
      fontSize: "0.75rem",
    },
  },
}));
