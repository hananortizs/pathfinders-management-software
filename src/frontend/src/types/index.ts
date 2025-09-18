/**
 * Exportações centralizadas de tipos
 */

export * from "./auth";

// Tipos para componentes
export interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: string;
  fallback?: React.ReactNode;
}
