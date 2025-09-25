/**
 * Hook personalizado para gerenciamento de autenticação
 */

import { useCallback } from 'react';
import { useAuthStore } from '../store/authStore';
import type { LoginRequest } from '../types/auth';

export const useAuth = () => {
  const {
    user,
    token,
    isAuthenticated,
    isLoading,
    error,
    login: storeLogin,
    logout: storeLogout,
    refreshAuth: storeRefreshAuth,
    validateToken: storeValidateToken,
    initializeAuth: storeInitializeAuth,
    clearError,
  } = useAuthStore();

  /**
   * Realiza login do usuário
   */
  const login = useCallback(async (credentials: LoginRequest) => {
    await storeLogin(credentials);
  }, [storeLogin]);

  /**
   * Realiza logout do usuário
   */
  const logout = useCallback(() => {
    storeLogout();
  }, [storeLogout]);

  /**
   * Renova a autenticação
   */
  const refreshAuth = useCallback(async () => {
    await storeRefreshAuth();
  }, [storeRefreshAuth]);

  /**
   * Valida o token atual
   */
  const validateToken = useCallback(async () => {
    return await storeValidateToken();
  }, [storeValidateToken]);

  /**
   * Inicializa a autenticação verificando token salvo
   */
  const initializeAuth = useCallback(async () => {
    await storeInitializeAuth();
  }, [storeInitializeAuth]);

  /**
   * Verifica se o usuário tem uma role específica
   */
  const hasRole = useCallback((role: string): boolean => {
    return user?.roles?.includes(role) || false;
  }, [user]);

  /**
   * Verifica se o usuário tem uma das roles especificadas
   */
  const hasAnyRole = useCallback((roles: string[]): boolean => {
    return user ? roles.some(role => user.roles?.includes(role)) : false;
  }, [user]);

  /**
   * Verifica se o usuário é administrador
   */
  const isAdmin = useCallback((): boolean => {
    return hasRole('admin');
  }, [hasRole]);

  /**
   * Verifica se o usuário pode acessar uma funcionalidade
   */
  const canAccess = useCallback((requiredRole?: string): boolean => {
    if (!isAuthenticated || !user) return false;
    if (!requiredRole) return true;
    return hasRole(requiredRole);
  }, [isAuthenticated, user, hasRole]);

  return {
    // State
    user,
    token,
    isAuthenticated,
    isLoading,
    error,
    
    // Actions
    login,
    logout,
    refreshAuth,
    validateToken,
    initializeAuth,
    clearError,
    
    // Utilities
    hasRole,
    hasAnyRole,
    isAdmin,
    canAccess,
    
    // Computed properties for dashboard
    name: user?.fullName || user?.firstName + ' ' + user?.lastName || 'Usuário',
    email: user?.email || '',
    role: user?.roles?.[0] || 'member',
    id: user?.id || '',
  };
};
