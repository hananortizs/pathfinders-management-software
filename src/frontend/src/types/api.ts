/**
 * Tipos relacionados à comunicação com a API
 */

export interface ApiResponse<T = unknown> {
  data: T;
  message?: string;
  success: boolean;
  errors?: string[];
}

export interface PaginatedResponse<T = unknown> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface ApiError {
  message: string;
  statusCode: number;
  errors?: Record<string, string[]>;
  timestamp: string;
  path: string;
}

export interface RequestConfig {
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
  url: string;
  data?: unknown;
  params?: Record<string, unknown>;
  headers?: Record<string, string>;
  timeout?: number;
}

export interface ApiClient {
  get: <T = unknown>(url: string, config?: Omit<RequestConfig, 'method' | 'url'>) => Promise<ApiResponse<T>>;
  post: <T = unknown>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method' | 'url' | 'data'>) => Promise<ApiResponse<T>>;
  put: <T = unknown>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method' | 'url' | 'data'>) => Promise<ApiResponse<T>>;
  delete: <T = unknown>(url: string, config?: Omit<RequestConfig, 'method' | 'url'>) => Promise<ApiResponse<T>>;
  patch: <T = unknown>(url: string, data?: unknown, config?: Omit<RequestConfig, 'method' | 'url' | 'data'>) => Promise<ApiResponse<T>>;
}
