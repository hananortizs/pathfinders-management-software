/**
 * Serviço de Autenticação
 *
 * Gerencia todas as operações de autenticação com o backend
 */

import type {
  LoginRequest,
  LoginResponse,
  UserInfo,
  ApiResponse,
  ChangePasswordRequest,
  ResetPasswordRequest,
  ResetPasswordConfirm,
  InviteMemberRequest,
} from "../types/auth";

// Importação temporária para evitar erro
interface ActivateMemberRequest {
  token: string;
  password: string;
  confirmPassword: string;
  memberInfo?: unknown;
}
import { apiClient } from "./apiClient";

class AuthService {
  /**
   * Realiza login do usuário
   * @param credentials - Credenciais de login
   * @returns Resposta com token JWT e informações do usuário
   */
  async login(credentials: LoginRequest): Promise<ApiResponse<LoginResponse>> {
    try {
      const data = await apiClient.post<ApiResponse<LoginResponse>>(
        "/auth/login",
        credentials
      );
      return data;
    } catch (error) {
      console.error("Erro no login:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Valida um token JWT
   * @param token - Token JWT
   * @returns Status da validação
   */
  async validateToken(
    token: string
  ): Promise<ApiResponse<{ isValid: boolean; user?: UserInfo }>> {
    try {
      const data = await apiClient.post<
        ApiResponse<{ isValid: boolean; user?: UserInfo }>
      >("/auth/validate", token);
      return data;
    } catch (error) {
      console.error("Erro na validação do token:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Atualiza um token JWT
   * @param token - Token atual
   * @returns Novo token JWT
   */
  async refreshToken(token: string): Promise<ApiResponse<LoginResponse>> {
    try {
      const data = await apiClient.post<ApiResponse<LoginResponse>>(
        "/auth/refresh",
        token
      );
      return data;
    } catch (error) {
      console.error("Erro ao atualizar token:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Obtém informações do usuário a partir do token
   * @param token - Token JWT
   * @returns Informações do usuário
   */
  async getUserInfo(token: string): Promise<ApiResponse<UserInfo>> {
    try {
      console.log(
        "🔍 Token sendo enviado para user-info:",
        token ? "Token presente" : "Token vazio"
      );
      console.log("🔍 Tamanho do token:", token?.length || 0);

      // Fazer requisição direta sem usar apiClient (que pega token do store)
      const response = await fetch(
        `${
          import.meta.env.VITE_API_BASE_URL || "http://localhost:5000/pms-loc"
        }/auth/user-info`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ token }),
        }
      );

      if (!response.ok) {
        throw new Error(`Erro ${response.status}: ${response.statusText}`);
      }

      const data = await response.json();
      return data;
    } catch (error) {
      console.error("Erro ao obter informações do usuário:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Altera a senha do usuário
   * @param request - Dados para alteração de senha
   * @returns Resultado da operação
   */
  async changePassword(
    request: ChangePasswordRequest
  ): Promise<ApiResponse<void>> {
    try {
      const data = await apiClient.post<ApiResponse<void>>(
        "/auth/change-password",
        request
      );
      return data;
    } catch (error) {
      console.error("Erro ao alterar senha:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Solicita reset de senha
   * @param request - Dados para reset de senha
   * @returns Resultado da operação
   */
  async resetPassword(
    request: ResetPasswordRequest
  ): Promise<ApiResponse<void>> {
    try {
      const data = await apiClient.post<ApiResponse<void>>(
        "/auth/reset-password",
        request
      );
      return data;
    } catch (error) {
      console.error("Erro ao solicitar reset de senha:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Confirma reset de senha
   * @param request - Dados para confirmação de reset
   * @returns Resultado da operação
   */
  async confirmResetPassword(
    request: ResetPasswordConfirm
  ): Promise<ApiResponse<void>> {
    try {
      const data = await apiClient.post<ApiResponse<void>>(
        "/auth/reset-password-confirm",
        request
      );
      return data;
    } catch (error) {
      console.error("Erro ao confirmar reset de senha:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Convida um novo membro
   * @param request - Dados do convite
   * @returns Resultado da operação
   */
  async inviteMember(request: InviteMemberRequest): Promise<ApiResponse<void>> {
    try {
      const data = await apiClient.post<ApiResponse<void>>(
        "/auth/invite-member",
        request
      );
      return data;
    } catch (error) {
      console.error("Erro ao convidar membro:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Ativa um membro convidado
   * @param request - Dados para ativação
   * @returns Resultado da operação
   */
  async activateMember(
    request: ActivateMemberRequest
  ): Promise<ApiResponse<LoginResponse>> {
    try {
      const data = await apiClient.post<ApiResponse<LoginResponse>>(
        "/auth/activate-member",
        request
      );
      return data;
    } catch (error) {
      console.error("Erro ao ativar membro:", error);
      return {
        isSuccess: false,
        message:
          error instanceof Error
            ? error.message
            : "Erro de conexão. Tente novamente.",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }
}

export const authService = new AuthService();
export default authService;
