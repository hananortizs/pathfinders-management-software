import React from 'react';
import {
  Box,
  Avatar,
  Typography,
  Chip,
  Button,
  Stack,
  Divider,
  Paper,
} from '@mui/material';
import {
  Edit as EditIcon,
  Lock as LockIcon,
  History as HistoryIcon,
} from '@mui/icons-material';
import type { ProfileHeaderProps } from '../../types/profile';

/**
 * Componente Header do Perfil
 * Exibe avatar, nome, papel, status e ações rápidas
 */
export const ProfileHeader: React.FC<ProfileHeaderProps> = ({
  user,
  onEditProfile,
  onChangePassword,
  onViewAudit,
}) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Active':
        return 'success';
      case 'Pending':
        return 'warning';
      case 'Archived':
        return 'error';
      default:
        return 'default';
    }
  };

  const getStatusLabel = (status: string) => {
    switch (status) {
      case 'Active':
        return 'Ativo';
      case 'Pending':
        return 'Pendente';
      case 'Archived':
        return 'Arquivado';
      default:
        return status;
    }
  };

  const getRoleColor = (role: string) => {
    switch (role?.toLowerCase()) {
      case 'admin':
        return 'error';
      case 'regional':
        return 'warning';
      case 'distrital':
        return 'info';
      case 'director':
        return 'primary';
      case 'secretary':
        return 'secondary';
      default:
        return 'default';
    }
  };

  const getRoleLabel = (role: string) => {
    switch (role?.toLowerCase()) {
      case 'admin':
        return 'Administrador';
      case 'regional':
        return 'Regional';
      case 'distrital':
        return 'Distrital';
      case 'director':
        return 'Diretor';
      case 'secretary':
        return 'Secretário';
      default:
        return 'Membro';
    }
  };

  const fullName = user.socialName || `${user.firstName} ${user.lastName}`;
  const primaryRole = user.roles?.[0] || 'member';

  return (
    <Paper 
      elevation={1} 
      sx={{ 
        p: 3, 
        mb: 3,
        background: 'linear-gradient(135deg, #0D47A1 0%, #1976D2 100%)',
        color: 'white',
        position: 'sticky',
        top: 64, // Altura da navbar
        zIndex: 1,
      }}
    >
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 3, mb: 2 }}>
        {/* Avatar */}
        <Avatar
          sx={{
            width: 80,
            height: 80,
            bgcolor: 'rgba(255, 255, 255, 0.2)',
            fontSize: '2rem',
            fontWeight: 'bold',
            border: '3px solid rgba(255, 255, 255, 0.3)',
          }}
        >
          {fullName.charAt(0).toUpperCase()}
        </Avatar>

        {/* Informações do usuário */}
        <Box sx={{ flex: 1 }}>
          <Typography variant="h4" sx={{ fontWeight: 'bold', mb: 1 }}>
            {fullName}
          </Typography>
          
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
            <Chip
              label={getRoleLabel(primaryRole)}
              color={getRoleColor(primaryRole) as any}
              size="small"
              sx={{ 
                color: 'white',
                fontWeight: 'bold',
                '& .MuiChip-label': {
                  color: 'white',
                }
              }}
            />
            
            <Chip
              label={getStatusLabel(user.status)}
              color={getStatusColor(user.status) as any}
              size="small"
              variant="outlined"
              sx={{ 
                color: 'white',
                borderColor: 'rgba(255, 255, 255, 0.5)',
                '& .MuiChip-label': {
                  color: 'white',
                }
              }}
            />
          </Box>

          {/* Clube/Unidade */}
          {(user.clubName || user.unitName) && (
            <Typography variant="body2" sx={{ opacity: 0.9 }}>
              {user.clubName && user.unitName 
                ? `${user.clubName} • ${user.unitName}`
                : user.clubName || user.unitName
              }
            </Typography>
          )}
        </Box>

        {/* Ações rápidas */}
        <Stack direction="row" spacing={1}>
          <Button
            variant="contained"
            startIcon={<EditIcon />}
            onClick={onEditProfile}
            sx={{
              bgcolor: 'rgba(255, 255, 255, 0.2)',
              color: 'white',
              '&:hover': {
                bgcolor: 'rgba(255, 255, 255, 0.3)',
              },
            }}
          >
            Editar Perfil
          </Button>
          
          <Button
            variant="outlined"
            startIcon={<LockIcon />}
            onClick={onChangePassword}
            sx={{
              borderColor: 'rgba(255, 255, 255, 0.5)',
              color: 'white',
              '&:hover': {
                borderColor: 'white',
                bgcolor: 'rgba(255, 255, 255, 0.1)',
              },
            }}
          >
            Alterar Senha
          </Button>
          
          <Button
            variant="outlined"
            startIcon={<HistoryIcon />}
            onClick={onViewAudit}
            sx={{
              borderColor: 'rgba(255, 255, 255, 0.5)',
              color: 'white',
              '&:hover': {
                borderColor: 'white',
                bgcolor: 'rgba(255, 255, 255, 0.1)',
              },
            }}
          >
            Auditoria
          </Button>
        </Stack>
      </Box>

      <Divider sx={{ borderColor: 'rgba(255, 255, 255, 0.2)', my: 2 }} />

      {/* Informações adicionais */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="body2" sx={{ opacity: 0.8 }}>
          Membro desde {new Date(user.createdAt).toLocaleDateString('pt-BR')}
        </Typography>
        
        <Typography variant="body2" sx={{ opacity: 0.8 }}>
          Última atualização: {new Date(user.updatedAt).toLocaleDateString('pt-BR')}
        </Typography>
      </Box>
    </Paper>
  );
};
