/**
 * Store Zustand para gerenciamento de estado de autenticação
 */

import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { UserInfo, LoginRequest, AuthState } from "../types/auth";
import { authService } from "../services/authService";

interface AuthStore extends AuthState {
  // Actions
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => void;
  setUser: (user: UserInfo | null) => void;
  setToken: (token: string | null) => void;
  setRefreshToken: (refreshToken: string | null) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearError: () => void;
  refreshAuth: () => Promise<void>;
  validateToken: () => Promise<boolean>;
  initializeAuth: () => Promise<void>;
}

export const useAuthStore = create<AuthStore>()(
  persist(
    (set, get) => ({
      // Initial state
      user: null,
      token: null,
      refreshToken: null,
      isAuthenticated: false,
      isLoading: true, // Iniciar como loading para verificar token
      error: null,

      // Actions
      login: async (credentials: LoginRequest) => {
        set({ isLoading: true, error: null });

        try {
          const response = await authService.login(credentials);

          if (!response.isSuccess || !response.data) {
            set({
              error: response.message || "Erro ao fazer login",
              isLoading: false,
            });
            return;
          }

          const { AccessToken } = response.data;
          console.log("🔍 AccessToken recebido no login:", AccessToken ? "Token presente" : "Token vazio");
          console.log("🔍 Tamanho do AccessToken:", AccessToken?.length || 0);

          // Obter informações completas do usuário
          const userInfoResponse = await authService.getUserInfo(AccessToken);

          if (!userInfoResponse.isSuccess || !userInfoResponse.data) {
            set({
              error: "Erro ao obter informações do usuário",
              isLoading: false,
            });
            return;
          }

          set({
            user: userInfoResponse.data,
            token: AccessToken,
            refreshToken: AccessToken, // O backend não usa refresh token separado
            isAuthenticated: true,
            isLoading: false,
            error: null,
          });
        } catch (error) {
          set({
            error:
              error instanceof Error ? error.message : "Erro ao fazer login",
            isLoading: false,
          });
        }
      },

      logout: () => {
        set({
          user: null,
          token: null,
          refreshToken: null,
          isAuthenticated: false,
          error: null,
        });
      },

      setUser: (user: UserInfo | null) => {
        set({ user });
      },

      setToken: (token: string | null) => {
        set({ token });
      },

      setRefreshToken: (refreshToken: string | null) => {
        set({ refreshToken });
      },

      setLoading: (isLoading: boolean) => {
        set({ isLoading });
      },

      setError: (error: string | null) => {
        set({ error });
      },

      clearError: () => {
        set({ error: null });
      },

      refreshAuth: async () => {
        const { token } = get();

        if (!token) {
          set({ isAuthenticated: false });
          return;
        }

        set({ isLoading: true });

        try {
          const response = await authService.refreshToken(token);

          if (!response.isSuccess || !response.data) {
            set({
              error: "Erro ao renovar autenticação",
              isLoading: false,
              isAuthenticated: false,
            });
            return;
          }

          const { AccessToken } = response.data;

          // Obter informações atualizadas do usuário
          const userInfoResponse = await authService.getUserInfo(AccessToken);

          if (!userInfoResponse.isSuccess || !userInfoResponse.data) {
            set({
              error: "Erro ao obter informações do usuário",
              isLoading: false,
              isAuthenticated: false,
            });
            return;
          }

          set({
            user: userInfoResponse.data,
            token: AccessToken,
            refreshToken: AccessToken,
            isAuthenticated: true,
            isLoading: false,
            error: null,
          });
        } catch {
          set({
            error: "Erro ao renovar autenticação",
            isLoading: false,
            isAuthenticated: false,
          });
        }
      },

      validateToken: async () => {
        const { token } = get();

        if (!token) {
          set({ isAuthenticated: false });
          return false;
        }

        try {
          const response = await authService.validateToken(token);

          if (!response.isSuccess) {
            set({ isAuthenticated: false });
            return false;
          }

          return true;
        } catch {
          set({ isAuthenticated: false });
          return false;
        }
      },

      initializeAuth: async () => {
        const { token } = get();
        
        if (!token) {
          set({ isLoading: false, isAuthenticated: false });
          return;
        }

        try {
          const isValid = await get().validateToken();
          
          if (isValid) {
            // Token válido, obter informações do usuário
            const userInfoResponse = await authService.getUserInfo(token);
            
            if (userInfoResponse.isSuccess && userInfoResponse.data) {
              set({
                user: userInfoResponse.data,
                isAuthenticated: true,
                isLoading: false,
                error: null,
              });
            } else {
              set({
                user: null,
                token: null,
                refreshToken: null,
                isAuthenticated: false,
                isLoading: false,
                error: null,
              });
            }
          } else {
            set({
              user: null,
              token: null,
              refreshToken: null,
              isAuthenticated: false,
              isLoading: false,
              error: null,
            });
          }
        } catch {
          set({
            user: null,
            token: null,
            refreshToken: null,
            isAuthenticated: false,
            isLoading: false,
            error: null,
          });
        }
      },
    }),
    {
      name: "auth-storage",
      partialize: (state) => ({
        user: state.user,
        token: state.token,
        refreshToken: state.refreshToken,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);
