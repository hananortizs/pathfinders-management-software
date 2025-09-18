import { styled, Card, CardContent, CardActions } from '@mui/material';
import type { CardProps, CardContentProps, CardActionsProps } from '@mui/material';

/**
 * Card padrão da aplicação
 * 
 * Card base com bordas arredondadas, sombra sutil e hover effect
 * para manter consistência visual em toda a aplicação
 */
export const AppCard = styled(Card)<CardProps>(({ theme }) => ({
  borderRadius: 16,
  boxShadow: '0px 2px 8px rgba(0,0,0,0.1)',
  transition: 'box-shadow 0.2s ease-in-out, transform 0.2s ease-in-out',
  '&:hover': {
    boxShadow: '0px 4px 16px rgba(0,0,0,0.15)',
    transform: 'translateY(-2px)',
  },
  '&:focus-within': {
    boxShadow: '0px 4px 16px rgba(0,0,0,0.15)',
    outline: '2px solid',
    outlineColor: theme.palette.primary.light,
    outlineOffset: '2px',
  },
}));

/**
 * Card de membro para exibição de informações de membros
 * 
 * Especializado para mostrar dados de membros com layout otimizado
 */
export const MemberCard = styled(AppCard)<CardProps>(({ theme }) => ({
  minHeight: 200,
  display: 'flex',
  flexDirection: 'column',
  '& .member-avatar': {
    width: 64,
    height: 64,
    margin: '0 auto 16px auto',
    backgroundColor: theme.palette.primary.light,
    color: theme.palette.primary.contrastText,
    fontSize: '1.5rem',
    fontWeight: 600,
  },
  '& .member-name': {
    fontSize: '1.1rem',
    fontWeight: 600,
    color: theme.palette.text.primary,
    textAlign: 'center',
    marginBottom: '8px',
  },
  '& .member-role': {
    fontSize: '0.9rem',
    color: theme.palette.text.secondary,
    textAlign: 'center',
    marginBottom: '16px',
  },
  '& .member-actions': {
    marginTop: 'auto',
    padding: '16px',
    justifyContent: 'center',
    gap: '8px',
  },
}));

/**
 * Card de unidade para exibição de informações de unidades
 * 
 * Especializado para mostrar dados de unidades (clubes, distritos, etc.)
 */
export const UnitCard = styled(AppCard)<CardProps>(({ theme }) => ({
  minHeight: 180,
  display: 'flex',
  flexDirection: 'column',
  '& .unit-icon': {
    width: 48,
    height: 48,
    margin: '0 auto 12px auto',
    backgroundColor: theme.palette.secondary.light,
    color: theme.palette.secondary.contrastText,
    fontSize: '1.25rem',
  },
  '& .unit-name': {
    fontSize: '1rem',
    fontWeight: 600,
    color: theme.palette.text.primary,
    textAlign: 'center',
    marginBottom: '8px',
  },
  '& .unit-description': {
    fontSize: '0.85rem',
    color: theme.palette.text.secondary,
    textAlign: 'center',
    marginBottom: '12px',
    flexGrow: 1,
  },
  '& .unit-stats': {
    display: 'flex',
    justifyContent: 'space-around',
    marginBottom: '16px',
    '& .stat': {
      textAlign: 'center',
      '& .stat-number': {
        fontSize: '1.2rem',
        fontWeight: 600,
        color: theme.palette.primary.main,
      },
      '& .stat-label': {
        fontSize: '0.75rem',
        color: theme.palette.text.secondary,
        textTransform: 'uppercase',
        letterSpacing: '0.5px',
      },
    },
  },
}));

/**
 * Card de estatísticas para exibição de métricas
 * 
 * Especializado para mostrar dados numéricos e estatísticas
 */
export const StatsCard = styled(AppCard)<CardProps>(({ theme }) => ({
  padding: '20px',
  textAlign: 'center',
  '& .stats-icon': {
    width: 56,
    height: 56,
    margin: '0 auto 16px auto',
    backgroundColor: theme.palette.primary.light,
    color: theme.palette.primary.contrastText,
    fontSize: '1.5rem',
    borderRadius: '50%',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
  '& .stats-value': {
    fontSize: '2rem',
    fontWeight: 600,
    color: theme.palette.text.primary,
    marginBottom: '8px',
  },
  '& .stats-label': {
    fontSize: '0.9rem',
    color: theme.palette.text.secondary,
    marginBottom: '8px',
  },
  '& .stats-change': {
    fontSize: '0.8rem',
    fontWeight: 500,
    '&.positive': {
      color: theme.palette.success.main,
    },
    '&.negative': {
      color: theme.palette.error.main,
    },
    '&.neutral': {
      color: theme.palette.text.secondary,
    },
  },
}));

/**
 * Card de conteúdo customizado
 * 
 * Card com padding e espaçamento otimizado para conteúdo
 */
export const AppCardContent = styled(CardContent)<CardContentProps>(({ theme }) => ({
  padding: '20px',
  '&:last-child': {
    paddingBottom: '20px',
  },
  '& .card-title': {
    fontSize: '1.25rem',
    fontWeight: 600,
    color: theme.palette.text.primary,
    marginBottom: '16px',
  },
  '& .card-subtitle': {
    fontSize: '0.9rem',
    color: theme.palette.text.secondary,
    marginBottom: '20px',
  },
  '& .card-body': {
    fontSize: '0.95rem',
    lineHeight: 1.6,
    color: theme.palette.text.primary,
  },
}));

/**
 * Card de ações customizado
 * 
 * Container para ações do card com espaçamento consistente
 */
export const AppCardActions = styled(CardActions)<CardActionsProps>(() => ({
  padding: '16px 20px 20px 20px',
  gap: '12px',
  justifyContent: 'flex-end',
  '&.center': {
    justifyContent: 'center',
  },
  '&.space-between': {
    justifyContent: 'space-between',
  },
  '&.start': {
    justifyContent: 'flex-start',
  },
}));
