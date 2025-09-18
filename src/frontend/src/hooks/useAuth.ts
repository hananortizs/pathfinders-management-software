/**
 * Hook personalizado para gerenciamento de autenticação
 */

import { useCallback } from 'react';
import { useAuthStore } from '../store/authStore';
import { authService } from '../services/authService';
import type { LoginRequest, UserRole } from '../types';

export const useAuth = () => {
  const {
    user,
    token,
    refreshToken,
    isAuthenticated,
    isLoading,
    error,
    login: storeLogin,
    logout: storeLogout,
    setUser,
    setToken,
    setRefreshToken,
    setLoading,
    setError,
    clearError,
  } = useAuthStore();

  /**
   * Realiza login do usuário
   */
  const login = useCallback(async (credentials: LoginRequest) => {
    setLoading(true);
    setError(null);

    try {
      const response = await authService.login(credentials);
      
      // Atualizar store com dados de autenticação
      storeLogin(credentials);
      
      // Armazenar tokens no localStorage
      localStorage.setItem('auth-token', response.token);
      localStorage.setItem('auth-refresh-token', response.refreshToken);
      
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Erro ao fazer login';
      setError(errorMessage);
      throw error;
    } finally {
      setLoading(false);
    }
  }, [storeLogin, setLoading, setError]);

  /**
   * Realiza logout do usuário
   */
  const logout = useCallback(async () => {
    try {
      await authService.logout();
    } catch (error) {
      console.error('Erro ao fazer logout:', error);
    } finally {
      // Limpar dados locais
      localStorage.removeItem('auth-token');
      localStorage.removeItem('auth-refresh-token');
      storeLogout();
    }
  }, [storeLogout]);

  /**
   * Renova a autenticação
   */
  const refreshAuth = useCallback(async () => {
    if (!refreshToken) {
      storeLogout();
      return;
    }

    setLoading(true);

    try {
      const response = await authService.refreshToken(refreshToken);
      
      setToken(response.token);
      setRefreshToken(response.refreshToken);
      setUser(response.user);
      
      // Atualizar tokens no localStorage
      localStorage.setItem('auth-token', response.token);
      localStorage.setItem('auth-refresh-token', response.refreshToken);
      
    } catch (error) {
      console.error('Erro ao renovar autenticação:', error);
      storeLogout();
    } finally {
      setLoading(false);
    }
  }, [refreshToken, storeLogout, setToken, setRefreshToken, setUser, setLoading]);

  /**
   * Verifica se o usuário tem uma role específica
   */
  const hasRole = useCallback((role: UserRole): boolean => {
    return user?.role === role;
  }, [user]);

  /**
   * Verifica se o usuário tem uma das roles especificadas
   */
  const hasAnyRole = useCallback((roles: UserRole[]): boolean => {
    return user ? roles.includes(user.role) : false;
  }, [user]);

  /**
   * Verifica se o usuário é administrador
   */
  const isAdmin = useCallback((): boolean => {
    return hasRole('admin' as UserRole);
  }, [hasRole]);

  /**
   * Verifica se o usuário pode acessar uma funcionalidade
   */
  const canAccess = useCallback((requiredRole?: UserRole): boolean => {
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
    clearError,
    
    // Utilities
    hasRole,
    hasAnyRole,
    isAdmin,
    canAccess,
  };
};
