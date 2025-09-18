/**
 * Componente de Inicialização de Autenticação
 * 
 * Verifica se há um token salvo e valida com o backend
 */

import { useEffect } from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';
import { useAuth } from '../../hooks/useAuth';

interface AuthInitializerProps {
  children: React.ReactNode;
}

const AuthInitializer: React.FC<AuthInitializerProps> = ({ children }) => {
  const { initializeAuth, isLoading } = useAuth();

  useEffect(() => {
    // Inicializar autenticação quando o componente montar
    initializeAuth();
  }, [initializeAuth]);

  // Mostrar loading enquanto inicializa
  if (isLoading) {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          minHeight: '100vh',
          gap: 2,
        }}
      >
        <CircularProgress size={48} />
        <Typography variant="body1" color="text.secondary">
          Verificando autenticação...
        </Typography>
      </Box>
    );
  }

  return <>{children}</>;
};

export default AuthInitializer;
