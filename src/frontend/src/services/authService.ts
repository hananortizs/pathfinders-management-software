/**
 * Serviço de autenticação para comunicação com a API
 */

import type { LoginRequest, LoginResponse, User, ApiResponse } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

class AuthService {
  private baseUrl: string;

  constructor() {
    this.baseUrl = `${API_BASE_URL}/auth`;
  }

  /**
   * Realiza login do usuário
   * @param credentials - Credenciais de login
   * @returns Promise com dados de autenticação
   */
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    try {
      // TODO: Remover quando backend estiver funcionando
      // Por enquanto, usar dados mockados para desenvolvimento
      console.log('🔧 Usando dados mockados - Backend não está rodando');
      
      // Simular delay de rede
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Dados mockados para desenvolvimento
      const mockResponse: LoginResponse = {
        user: {
          id: '1',
          email: credentials.email,
          name: 'Usuário Teste',
          role: 'admin' as any,
          unitId: '1',
          unitName: 'Clube Central',
          isActive: true,
          lastLoginAt: new Date(),
          createdAt: new Date(),
          updatedAt: new Date(),
        },
        token: 'mock-jwt-token-' + Date.now(),
        refreshToken: 'mock-refresh-token-' + Date.now(),
        expiresIn: 3600, // 1 hora
      };

      return mockResponse;

      // Código original para quando backend estiver funcionando:
      /*
      const response = await fetch(`${this.baseUrl}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Erro ao fazer login');
      }

      const data: ApiResponse<LoginResponse> = await response.json();
      return data.data;
      */
    } catch (error) {
      console.error('Erro no serviço de login:', error);
      throw error;
    }
  }

  /**
   * Realiza logout do usuário
   */
  async logout(): Promise<void> {
    try {
      // TODO: Implementar logout real quando backend estiver funcionando
      console.log('🔧 Logout mockado - Backend não está rodando');
      
      // Simular delay de rede
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Código original para quando backend estiver funcionando:
      /*
      const token = localStorage.getItem('auth-token');
      
      if (token) {
        await fetch(`${this.baseUrl}/logout`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        });
      }
      */
    } catch (error) {
      console.error('Erro no serviço de logout:', error);
      // Não relançar erro para logout, pois é uma operação de limpeza
    }
  }

  /**
   * Renova o token de autenticação
   * @param refreshToken - Token de renovação
   * @returns Promise com novo token
   */
  async refreshToken(refreshToken: string): Promise<LoginResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/refresh`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ refreshToken }),
      });

      if (!response.ok) {
        throw new Error('Erro ao renovar token');
      }

      const data: ApiResponse<LoginResponse> = await response.json();
      return data.data;
    } catch (error) {
      console.error('Erro no serviço de refresh token:', error);
      throw error;
    }
  }

  /**
   * Obtém dados do usuário atual
   * @param token - Token de autenticação
   * @returns Promise com dados do usuário
   */
  async getCurrentUser(token: string): Promise<User> {
    try {
      const response = await fetch(`${this.baseUrl}/me`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error('Erro ao obter dados do usuário');
      }

      const data: ApiResponse<User> = await response.json();
      return data.data;
    } catch (error) {
      console.error('Erro no serviço de usuário atual:', error);
      throw error;
    }
  }

  /**
   * Verifica se o token é válido
   * @param token - Token para verificar
   * @returns Promise com status de validade
   */
  async validateToken(token: string): Promise<boolean> {
    try {
      const response = await fetch(`${this.baseUrl}/validate`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      return response.ok;
    } catch (error) {
      console.error('Erro na validação do token:', error);
      return false;
    }
  }
}

export const authService = new AuthService();
export default authService;
