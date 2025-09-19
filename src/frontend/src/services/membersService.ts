/**
 * Serviço para comunicação com a API de membros
 */

import { apiClient } from "./apiClient";
import type {
  Member,
  MemberListResponse,
  CreateMemberDto,
  UpdateMemberDto,
  MemberFilters,
  UserLevel,
  MemberBulkAction,
} from "../types/members";
import type { ApiResponse } from "../types/auth";

export interface GetMembersParams {
  userLevel: UserLevel;
  groupingStrategy: "hierarchical" | "flat" | "by_club" | "by_unit";
  page: number;
  pageSize: number;
  filters?: MemberFilters;
}

class MembersService {
  private readonly baseUrl = "/members";

  /**
   * Busca lista de membros com filtros e paginação (GET simples)
   */
  async getMembers(params: GetMembersParams): Promise<MemberListResponse> {
    console.log('🔍 MembersService.getMembers: Parâmetros recebidos', params);
    
    // Construir query string corretamente
    const queryParams = new URLSearchParams();
    queryParams.append('userLevel', params.userLevel);
    queryParams.append('groupingStrategy', params.groupingStrategy);
    queryParams.append('page', params.page.toString());
    queryParams.append('pageSize', params.pageSize.toString());

    const url = `${this.baseUrl}?${queryParams.toString()}`;
    console.log('🔍 MembersService.getMembers: URL da requisição', url);

    const response = await apiClient.get<ApiResponse<MemberListResponse>>(url);
    
    console.log('🔍 MembersService.getMembers: Resposta da API', {
      response,
      data: response.data,
      members: response.data?.members,
      groups: response.data?.groups,
      totalCount: response.data?.totalCount
    });

    // Debug detalhado dos membros
    if (response.data?.members && response.data.members.length > 0) {
      console.log('🔍 MembersService.getMembers: Primeiro membro da API', {
        member: response.data.members[0],
        fullName: response.data.members[0].fullName,
        displayName: response.data.members[0].displayName,
        age: response.data.members[0].age,
        createdAt: response.data.members[0].createdAt,
        clubName: response.data.members[0].clubName,
        unitName: response.data.members[0].unitName,
        status: response.data.members[0].status,
        gender: response.data.members[0].gender,
        primaryEmail: response.data.members[0].primaryEmail,
        allKeys: Object.keys(response.data.members[0]),
        allValues: Object.values(response.data.members[0])
      });
    }
    
    const result = response.data || {
      members: [],
      groups: [],
      totalCount: 0,
      page: 1,
      pageSize: 20,
      totalPages: 0,
      hasNextPage: false,
      hasPreviousPage: false,
      stats: {
        totalMembers: 0,
        activeMembers: 0,
        pendingMembers: 0,
        inactiveMembers: 0,
        suspendedMembers: 0,
        byGender: {},
        byStatus: {},
        byClub: {},
        byUnit: {},
      },
    };
    
    // Mapear estatísticas para o formato esperado pelo frontend
    if (result.stats) {
      result.stats = {
        totalMembers: result.stats.totalMembers || result.totalCount || 0,
        activeMembers: result.stats.activeMembers || 0,
        pendingMembers: result.stats.pendingMembers || 0,
        inactiveMembers: result.stats.inactiveMembers || 0,
        suspendedMembers: result.stats.suspendedMembers || 0,
        byGender: result.stats.byGender || {},
        byStatus: result.stats.byStatus || {},
        byClub: result.stats.byClub || {},
        byUnit: result.stats.byUnit || {},
      };
    }
    
    console.log('🔍 MembersService.getMembers: Resultado final', {
      result,
      membersCount: result.members?.length || 0,
      groupsCount: result.groups?.length || 0,
      totalCount: result.totalCount,
      stats: result.stats
    });
    return result;
  }

  /**
   * Busca membros com filtros avançados (POST)
   */
  async searchMembers(params: GetMembersParams): Promise<MemberListResponse> {
    const response = await apiClient.post<ApiResponse<MemberListResponse>>(
      `${this.baseUrl}/search`,
      params
    );
    return (
      response.data || {
        members: [],
        groups: [],
        totalCount: 0,
        page: 1,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false,
        stats: {
          totalMembers: 0,
          activeMembers: 0,
          pendingMembers: 0,
          inactiveMembers: 0,
          suspendedMembers: 0,
          byGender: {},
          byStatus: {},
          byClub: {},
          byUnit: {},
        },
      }
    );
  }

  /**
   * Busca membro por ID
   */
  async getMemberById(id: string): Promise<Member> {
    const response = await apiClient.get<ApiResponse<Member>>(
      `${this.baseUrl}/${id}`
    );
    return response.data || ({} as Member);
  }

  /**
   * Cria novo membro
   */
  async createMember(memberData: CreateMemberDto): Promise<Member> {
    const response = await apiClient.post<ApiResponse<Member>>(
      `${this.baseUrl}`,
      memberData
    );
    return response.data || ({} as Member);
  }

  /**
   * Atualiza membro existente
   */
  async updateMember(id: string, memberData: UpdateMemberDto): Promise<Member> {
    const response = await apiClient.put<ApiResponse<Member>>(
      `${this.baseUrl}/${id}`,
      memberData
    );
    return response.data || ({} as Member);
  }

  /**
   * Remove membro (soft delete)
   */
  async deleteMember(id: string): Promise<void> {
    await apiClient.delete(`${this.baseUrl}/${id}`);
  }

  /**
   * Ativa membro
   */
  async activateMember(id: string): Promise<Member> {
    const response = await apiClient.patch<ApiResponse<Member>>(
      `${this.baseUrl}/${id}/activate`
    );
    return response.data || ({} as Member);
  }

  /**
   * Desativa membro
   */
  async deactivateMember(id: string): Promise<Member> {
    const response = await apiClient.patch<ApiResponse<Member>>(
      `${this.baseUrl}/${id}/deactivate`
    );
    return response.data || ({} as Member);
  }

  /**
   * Ações em massa
   */
  async bulkAction(action: MemberBulkAction): Promise<void> {
    await apiClient.post(`${this.baseUrl}/bulk-action`, action);
  }

  /**
   * Exporta membros para CSV
   */
  async exportMembers(filters: MemberFilters): Promise<Blob> {
    const response = await apiClient.get(
      `${this.baseUrl}/export?${new URLSearchParams(filters as any).toString()}`
    );
    return response as Blob;
  }

  /**
   * Busca estatísticas de membros
   */
  async getMemberStats(userLevel: UserLevel): Promise<{
    total: number;
    active: number;
    pending: number;
    inactive: number;
    byGender: Record<string, number>;
    byClub: Record<string, number>;
    byStatus: Record<string, number>;
  }> {
    const response = await apiClient.get<ApiResponse<any>>(
      `${this.baseUrl}/stats?userLevel=${userLevel}`
    );
    return response.data || { isValid: false, isUnique: false };
  }

  /**
   * Busca hierarquia para agrupamento
   */
  async getHierarchy(userLevel: UserLevel): Promise<{
    divisions: Array<{ id: string; name: string; memberCount: number }>;
    unions: Array<{ id: string; name: string; memberCount: number }>;
    regions: Array<{ id: string; name: string; memberCount: number }>;
    associations: Array<{ id: string; name: string; memberCount: number }>;
    districts: Array<{ id: string; name: string; memberCount: number }>;
    clubs: Array<{ id: string; name: string; memberCount: number }>;
    units: Array<{ id: string; name: string; memberCount: number }>;
  }> {
    const response = await apiClient.get<ApiResponse<any>>(
      `${this.baseUrl}/hierarchy?userLevel=${userLevel}`
    );
    return response.data || { isValid: false, isUnique: false };
  }

  /**
   * Valida CPF único
   */
  async validateCpf(
    cpf: string,
    excludeId?: string
  ): Promise<{ isValid: boolean; isUnique: boolean }> {
    const response = await apiClient.post<
      ApiResponse<{ isValid: boolean; isUnique: boolean }>
    >(`${this.baseUrl}/validate-cpf`, { cpf, excludeId });
    return response.data || { isValid: false, isUnique: false };
  }

  /**
   * Valida email único
   */
  async validateEmail(
    email: string,
    excludeId?: string
  ): Promise<{ isValid: boolean; isUnique: boolean }> {
    const response = await apiClient.post<
      ApiResponse<{ isValid: boolean; isUnique: boolean }>
    >(`${this.baseUrl}/validate-email`, { email, excludeId });
    return response.data || { isValid: false, isUnique: false };
  }

  /**
   * Busca sugestões de unidade baseada na idade
   */
  async getUnitSuggestions(
    age: number,
    clubId: string
  ): Promise<
    Array<{
      id: string;
      name: string;
      ageRange: { min: number; max: number };
      currentCapacity: number;
      maxCapacity: number;
      suitability: "perfect" | "good" | "acceptable";
    }>
  > {
    const response = await apiClient.get<ApiResponse<any>>(
      `${this.baseUrl}/unit-suggestions?age=${age}&clubId=${clubId}`
    );
    return response.data || { isValid: false, isUnique: false };
  }

  /**
   * Transfere membro entre clubes
   */
  async transferMember(
    memberId: string,
    targetClubId: string,
    targetUnitId: string,
    reason?: string
  ): Promise<Member> {
    const response = await apiClient.post<ApiResponse<Member>>(
      `${this.baseUrl}/${memberId}/transfer`,
      { targetClubId, targetUnitId, reason }
    );
    return (
      response.data || {
        id: "",
        firstName: "",
        lastName: "",
        dateOfBirth: "",
        cpf: "",
        status: "Pending" as any,
        gender: "Other" as any,
        clubId: "",
        clubName: "",
        unitId: "",
        unitName: "",
        hierarchyPath: {} as any,
        contacts: [],
        createdAt: "",
        updatedAt: "",
      }
    );
  }

  /**
   * Busca histórico de transferências do membro
   */
  async getMemberTransferHistory(memberId: string): Promise<
    Array<{
      id: string;
      fromClub: string;
      toClub: string;
      fromUnit: string;
      toUnit: string;
      reason?: string;
      transferredAt: string;
      transferredBy: string;
    }>
  > {
    const response = await apiClient.get<ApiResponse<any>>(
      `${this.baseUrl}/${memberId}/transfer-history`
    );
    return response.data || { isValid: false, isUnique: false };
  }
}

export const membersService = new MembersService();
