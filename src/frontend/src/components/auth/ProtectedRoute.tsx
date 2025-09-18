/**
 * Componente de Rota Protegida
 *
 * Wrapper que protege rotas baseado no estado de autenticação
 * e roles do usuário
 */

import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { Box, CircularProgress, Typography } from "@mui/material";
import { useAuth } from "../../hooks/useAuth";
import type { ProtectedRouteProps } from "../../types";

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  requiredRole,
  fallback,
}) => {
  const { isAuthenticated, user, isLoading, canAccess } = useAuth();
  const location = useLocation();

  // Mostrar loading enquanto verifica autenticação
  if (isLoading) {
    return (
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          minHeight: "50vh",
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

  // Redirecionar para login se não estiver autenticado
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Verificar se usuário tem a role necessária
  if (requiredRole && !canAccess(requiredRole)) {
    // Mostrar fallback personalizado ou página de acesso negado
    if (fallback) {
      return <>{fallback}</>;
    }

    return (
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          minHeight: "50vh",
          gap: 2,
          p: 4,
        }}
      >
        <Typography variant="h4" color="error" gutterBottom>
          Acesso Negado
        </Typography>
        <Typography variant="body1" color="text.secondary" textAlign="center">
          Você não tem permissão para acessar esta página.
        </Typography>
        <Typography variant="body2" color="text.secondary" textAlign="center">
          Role necessária: {requiredRole}
          <br />
          Suas roles: {user?.roles?.join(", ") || "Nenhuma"}
        </Typography>
      </Box>
    );
  }

  // Renderizar conteúdo protegido
  return <>{children}</>;
};

export default ProtectedRoute;
