/**
 * Tipos TypeScript para autenticação
 * Baseados nos DTOs do backend
 */

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  AccessToken: string;
  TokenType: string;
  ExpiresIn: number;
  Member: MemberBasicInfo;
  Pending?: PendingData;
}

export interface MemberBasicInfo {
  id: string;
  status: string;
}

export interface PendingData {
  activationRequired: string[];
  operationRequired: string[];
  optional: string[];
  blockingWrites: boolean;
}

export interface UserInfo {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  roles: string[];
  scopes: string[];
  isActive: boolean;
  createdAtUtc: string;
  updatedAtUtc: string;
}

// Interface para compatibilidade com o store existente
export interface User {
  id: string;
  email: string;
  name: string;
  role: string;
  unitId: string;
  unitName: string;
  isActive: boolean;
  lastLoginAt: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface AuthState {
  user: UserInfo | null;
  token: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ResetPasswordRequest {
  email: string;
}

export interface ResetPasswordConfirm {
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface InviteMemberRequest {
  email: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: string;
  phone?: string;
  clubId: string;
}

export interface ActivateMemberRequest {
  token: string;
  password: string;
  confirmPassword: string;
  memberInfo?: CompleteMemberInfo;
}

export interface CompleteMemberInfo {
  cpf?: string;
  rg?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelationship?: string;
  medicalInfo?: string;
  allergies?: string;
  medications?: string;
  baptismDate?: string;
  baptismChurch?: string;
  baptismPastor?: string;
  scarfDate?: string;
  scarfChurch?: string;
  scarfPastor?: string;
}

// Tipos para resposta da API
export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  data?: T;
  statusCode: number;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}