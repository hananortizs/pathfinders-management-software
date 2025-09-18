import { styled, Button } from '@mui/material';
import type { ButtonProps } from '@mui/material';

/**
 * Botão primário padrão da aplicação
 * 
 * Utiliza a cor primária do tema (#0D47A1) com estilo contained
 * e bordas arredondadas para manter consistência visual
 */
export const PrimaryButton = styled(Button)<ButtonProps>(({ theme }) => ({
  backgroundColor: theme.palette.primary.main,
  color: theme.palette.primary.contrastText,
  borderRadius: theme.shape.borderRadius,
  textTransform: 'none',
  fontWeight: 500,
  minHeight: 44,
  padding: '8px 16px',
  boxShadow: '0px 2px 4px rgba(0,0,0,0.1)',
  '&:hover': {
    backgroundColor: theme.palette.primary.dark,
    boxShadow: '0px 4px 8px rgba(0,0,0,0.15)',
  },
  '&:focus': {
    outline: '2px solid',
    outlineColor: theme.palette.primary.light,
    outlineOffset: '2px',
  },
  '&:disabled': {
    backgroundColor: theme.palette.action.disabled,
    color: theme.palette.action.disabled,
  },
}));

/**
 * Botão secundário padrão da aplicação
 * 
 * Utiliza a cor secundária do tema (#B71C1C) com estilo contained
 * para ações de destaque ou perigo
 */
export const SecondaryButton = styled(Button)<ButtonProps>(({ theme }) => ({
  backgroundColor: theme.palette.secondary.main,
  color: theme.palette.secondary.contrastText,
  borderRadius: theme.shape.borderRadius,
  textTransform: 'none',
  fontWeight: 500,
  minHeight: 44,
  padding: '8px 16px',
  boxShadow: '0px 2px 4px rgba(0,0,0,0.1)',
  '&:hover': {
    backgroundColor: theme.palette.secondary.dark,
    boxShadow: '0px 4px 8px rgba(0,0,0,0.15)',
  },
  '&:focus': {
    outline: '2px solid',
    outlineColor: theme.palette.secondary.light,
    outlineOffset: '2px',
  },
  '&:disabled': {
    backgroundColor: theme.palette.action.disabled,
    color: theme.palette.action.disabled,
  },
}));

/**
 * Botão de perigo para ações destrutivas
 * 
 * Utiliza a cor de erro do tema com estilo contained
 * para ações que requerem atenção especial
 */
export const DangerButton = styled(Button)<ButtonProps>(({ theme }) => ({
  backgroundColor: theme.palette.error.main,
  color: theme.palette.error.contrastText,
  borderRadius: theme.shape.borderRadius,
  textTransform: 'none',
  fontWeight: 500,
  minHeight: 44,
  padding: '8px 16px',
  boxShadow: '0px 2px 4px rgba(0,0,0,0.1)',
  '&:hover': {
    backgroundColor: theme.palette.error.dark,
    boxShadow: '0px 4px 8px rgba(0,0,0,0.15)',
  },
  '&:focus': {
    outline: '2px solid',
    outlineColor: theme.palette.error.light,
    outlineOffset: '2px',
  },
  '&:disabled': {
    backgroundColor: theme.palette.action.disabled,
    color: theme.palette.action.disabled,
  },
}));

/**
 * Botão outline primário
 * 
 * Versão outline do botão primário para ações secundárias
 * que ainda precisam de destaque
 */
export const OutlinePrimaryButton = styled(Button)<ButtonProps>(({ theme }) => ({
  borderColor: theme.palette.primary.main,
  color: theme.palette.primary.main,
  borderRadius: theme.shape.borderRadius,
  textTransform: 'none',
  fontWeight: 500,
  minHeight: 44,
  padding: '8px 16px',
  borderWidth: '2px',
  '&:hover': {
    borderColor: theme.palette.primary.dark,
    backgroundColor: theme.palette.primary.light,
    color: theme.palette.primary.contrastText,
    borderWidth: '2px',
  },
  '&:focus': {
    outline: '2px solid',
    outlineColor: theme.palette.primary.light,
    outlineOffset: '2px',
  },
  '&:disabled': {
    borderColor: theme.palette.action.disabled,
    color: theme.palette.action.disabled,
  },
}));

/**
 * Botão de texto para ações menos importantes
 * 
 * Estilo minimalista para ações que não precisam de destaque visual
 */
export const TextButton = styled(Button)<ButtonProps>(({ theme }) => ({
  color: theme.palette.primary.main,
  borderRadius: theme.shape.borderRadius,
  textTransform: 'none',
  fontWeight: 500,
  minHeight: 44,
  padding: '8px 12px',
  '&:hover': {
    backgroundColor: theme.palette.primary.light,
    color: theme.palette.primary.contrastText,
  },
  '&:focus': {
    outline: '2px solid',
    outlineColor: theme.palette.primary.light,
    outlineOffset: '2px',
  },
  '&:disabled': {
    color: theme.palette.action.disabled,
  },
}));

/**
 * Botão de loading com estado de carregamento
 * 
 * Extensão do PrimaryButton com suporte a estado de loading
 */
export const LoadingButton = styled(PrimaryButton)<ButtonProps & { loading?: boolean }>(({ theme, loading }) => ({
  position: 'relative',
  '& .MuiCircularProgress-root': {
    position: 'absolute',
    top: '50%',
    left: '50%',
    marginTop: '-12px',
    marginLeft: '-12px',
  },
  ...(loading && {
    color: 'transparent',
    '&:hover': {
      backgroundColor: theme.palette.primary.main,
    },
  }),
}));
