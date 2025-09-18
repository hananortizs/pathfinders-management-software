/**
 * Cliente HTTP configurado para comunicação com a API
 *
 * Inclui interceptors para adicionar automaticamente o token de autorização
 * e tratamento de erros centralizado
 */

import { useAuthStore } from "../store/authStore";
import { env } from "../config/environment";

class ApiClient {
  private baseUrl: string;

  constructor() {
    this.baseUrl = env.apiBaseUrl;
    console.log("🚀 API Client initialized with base URL:", this.baseUrl);
  }

  /**
   * Obtém o token de autorização do store
   */
  private getAuthToken(): string | null {
    const state = useAuthStore.getState();
    return state.token;
  }

  /**
   * Cria headers padrão para requisições
   */
  private getDefaultHeaders(): HeadersInit {
    const headers: HeadersInit = {
      "Content-Type": "application/json",
    };

    const token = this.getAuthToken();
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    return headers;
  }

  /**
   * Trata erros de resposta da API
   */
  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      let errorMessage = "Erro na requisição";
      let errorData: unknown = null;

      try {
        errorData = await response.json();
        errorMessage =
          (errorData as { message?: string })?.message || errorMessage;
      } catch {
        errorMessage = `Erro ${response.status}: ${response.statusText}`;
      }

      // Se for erro 401, limpar autenticação
      if (response.status === 401) {
        const { logout } = useAuthStore.getState();
        logout();
      }

      throw new Error(errorMessage);
    }

    return response.json();
  }

  /**
   * Realiza uma requisição GET
   */
  async get<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;

    const response = await fetch(url, {
      method: "GET",
      headers: this.getDefaultHeaders(),
      ...options,
    });

    return this.handleResponse<T>(response);
  }

  /**
   * Realiza uma requisição POST
   */
  async post<T>(
    endpoint: string,
    data?: unknown,
    options?: RequestInit
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;

    const response = await fetch(url, {
      method: "POST",
      headers: this.getDefaultHeaders(),
      body: data ? JSON.stringify(data) : undefined,
      ...options,
    });

    return this.handleResponse<T>(response);
  }

  /**
   * Realiza uma requisição PUT
   */
  async put<T>(
    endpoint: string,
    data?: unknown,
    options?: RequestInit
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;

    const response = await fetch(url, {
      method: "PUT",
      headers: this.getDefaultHeaders(),
      body: data ? JSON.stringify(data) : undefined,
      ...options,
    });

    return this.handleResponse<T>(response);
  }

  /**
   * Realiza uma requisição PATCH
   */
  async patch<T>(
    endpoint: string,
    data?: unknown,
    options?: RequestInit
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;

    const response = await fetch(url, {
      method: "PATCH",
      headers: this.getDefaultHeaders(),
      body: data ? JSON.stringify(data) : undefined,
      ...options,
    });

    return this.handleResponse<T>(response);
  }

  /**
   * Realiza uma requisição DELETE
   */
  async delete<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;

    const response = await fetch(url, {
      method: "DELETE",
      headers: this.getDefaultHeaders(),
      ...options,
    });

    return this.handleResponse<T>(response);
  }

  /**
   * Realiza upload de arquivo
   */
  async uploadFile<T>(
    endpoint: string,
    file: File,
    options?: RequestInit
  ): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const formData = new FormData();
    formData.append("file", file);

    const token = this.getAuthToken();
    const headers: HeadersInit = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    const response = await fetch(url, {
      method: "POST",
      headers,
      body: formData,
      ...options,
    });

    return this.handleResponse<T>(response);
  }
}

export const apiClient = new ApiClient();
export default apiClient;
