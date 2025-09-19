/**
 * Hook flexível para gerenciar lista de membros
 * Suporta diferentes níveis de usuário e estratégias de agrupamento
 */

import { useState, useMemo, useCallback } from "react";
import { useQuery } from "@tanstack/react-query";
import type {
  UserLevel,
  MemberFilters,
  MemberGroup,
  MemberSummary,
  MemberListResponse,
} from "../types/members";
import { membersService } from "../services/membersService";

export interface UseMembersListOptions {
  userLevel: UserLevel;
  groupingStrategy: "hierarchical" | "flat" | "by_club" | "by_unit";
  initialFilters?: Partial<MemberFilters>;
  pageSize?: number;
  autoFetch?: boolean;
}

export interface UseMembersListReturn {
  // Dados
  members: MemberSummary[];
  groups: MemberGroup[];
  filteredMembers: MemberSummary[];
  selectedMembers: string[];

  // Estado
  isLoading: boolean;
  isError: boolean;
  error: Error | null;

  // Paginação
  currentPage: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;

  // Filtros
  filters: MemberFilters;
  setFilters: (filters: Partial<MemberFilters>) => void;
  clearFilters: () => void;

  // Seleção
  selectMember: (memberId: string) => void;
  selectAllMembers: () => void;
  deselectMember: (memberId: string) => void;
  clearSelection: () => void;

  // Ações
  refresh: () => void;
  nextPage: () => void;
  previousPage: () => void;
  goToPage: (page: number) => void;

  // Agrupamento
  groupedMembers: Record<string, MemberSummary[]>;
  groupByLevel: (level: number) => void;

  // Estatísticas
  stats: {
    total: number;
    active: number;
    pending: number;
    inactive: number;
    byGender: Record<string, number>;
    byClub: Record<string, number>;
  };
}

const DEFAULT_FILTERS: MemberFilters = {
  search: "",
  status: [],
  gender: [],
  clubIds: [],
  unitIds: [],
  divisionIds: [],
  unionIds: [],
  regionIds: [],
  associationIds: [],
  districtIds: [],
  ageRange: undefined,
  dateRange: undefined,
};

export const useMembersList = (
  options: UseMembersListOptions
): UseMembersListReturn => {
  const {
    userLevel,
    groupingStrategy,
    initialFilters = {},
    pageSize = 20,
    autoFetch = true,
  } = options;

  // Estado local
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedMembers, setSelectedMembers] = useState<string[]>([]);
  const [filters, setFiltersState] = useState<MemberFilters>({
    ...DEFAULT_FILTERS,
    ...initialFilters,
  });

  // Query para buscar membros
  const {
    data: membersData,
    isLoading,
    isError,
    error,
    refetch,
  } = useQuery({
    queryKey: [
      "members",
      userLevel,
      groupingStrategy,
      currentPage,
      pageSize,
      filters,
    ],
    queryFn: async () => {
      console.log("🔍 useMembersList: Iniciando busca de membros", {
        userLevel,
        groupingStrategy,
        currentPage,
        pageSize,
        filters,
      });

      const result = await membersService.getMembers({
        userLevel,
        groupingStrategy,
        page: currentPage,
        pageSize,
        filters,
      });

      console.log("🔍 useMembersList: Dados recebidos", result);
      return result;
    },
    enabled: autoFetch,
    staleTime: 0, // Sempre buscar dados frescos para debug
    gcTime: 0, // Sem cache para debug
    refetchOnWindowFocus: false, // Evita refetch desnecessário
    refetchOnMount: true, // Sempre buscar ao montar para debug
  });

  // Dados processados - CORRIGIDO: usar capitalização correta do backend
  const members =
    (membersData as any)?.Members ||
    (membersData as MemberListResponse)?.members ||
    [];
  const groups =
    (membersData as any)?.Groups ||
    (membersData as MemberListResponse)?.groups ||
    [];
  const totalCount =
    (membersData as any)?.TotalCount ||
    (membersData as MemberListResponse)?.totalCount ||
    0;
  const totalPages =
    (membersData as any)?.TotalPages ||
    (membersData as MemberListResponse)?.totalPages ||
    0;
  const stats =
    (membersData as any)?.Stats || (membersData as MemberListResponse)?.stats;

  // Debug: Log dos dados brutos
  console.log("🔍 useMembersList: Dados brutos do backend", {
    membersData,
    membersRaw: (membersData as any)?.Members,
    groupsRaw: (membersData as any)?.Groups,
    totalCountRaw: (membersData as any)?.TotalCount,
    statsRaw: (membersData as any)?.Stats,
  });

  // Debug logs
  console.log("🔍 useMembersList: Dados processados", {
    membersData,
    members,
    groups,
    totalCount,
    totalPages,
    isLoading,
    isError,
    error,
    membersLength: members.length,
    groupsLength: groups.length,
    membersType: typeof members,
    groupsType: typeof groups,
    membersIsArray: Array.isArray(members),
    groupsIsArray: Array.isArray(groups),
    membersFirst: members[0],
    groupsFirst: groups[0],
    membersKeys: members.length > 0 ? Object.keys(members[0]) : [],
    groupsKeys: groups.length > 0 ? Object.keys(groups[0]) : [],
  });

  // Debug detalhado do primeiro membro
  if (members.length > 0) {
    console.log("🔍 useMembersList: Primeiro membro detalhado", {
      member: members[0],
      fullName: members[0].fullName,
      displayName: members[0].displayName,
      age: members[0].age,
      createdAt: members[0].createdAt,
      clubName: members[0].clubName,
      unitName: members[0].unitName,
      status: members[0].status,
      gender: members[0].gender,
      primaryEmail: members[0].primaryEmail,
      allKeys: Object.keys(members[0]),
      allValues: Object.values(members[0]),
    });

    // Debug da estrutura completa do objeto
    console.log("🔍 useMembersList: Estrutura completa do membro", {
      rawMember: JSON.stringify(members[0], null, 2),
      memberKeys: Object.keys(members[0]),
      memberValues: Object.values(members[0]),
    });

    // Debug das propriedades em PascalCase
    console.log("🔍 useMembersList: Propriedades em PascalCase", {
      FullName: members[0].FullName,
      DisplayName: members[0].DisplayName,
      Age: members[0].Age,
      CreatedAt: members[0].CreatedAt,
      ClubName: members[0].ClubName,
      UnitName: members[0].UnitName,
      Status: members[0].Status,
      Gender: members[0].Gender,
      PrimaryEmail: members[0].PrimaryEmail,
    });
  }

  // Filtros aplicados
  const setFilters = useCallback((newFilters: Partial<MemberFilters>) => {
    setFiltersState((prev) => ({ ...prev, ...newFilters }));
    setCurrentPage(1); // Reset para primeira página
  }, []);

  const clearFilters = useCallback(() => {
    setFiltersState(DEFAULT_FILTERS);
    setCurrentPage(1);
  }, []);

  // Seleção de membros
  const selectMember = useCallback((memberId: string) => {
    setSelectedMembers((prev) =>
      prev.includes(memberId)
        ? prev.filter((id) => id !== memberId)
        : [...prev, memberId]
    );
  }, []);

  const selectAllMembers = useCallback(() => {
    setSelectedMembers(members.map((member: MemberSummary) => member.id));
  }, [members]);

  const deselectMember = useCallback((memberId: string) => {
    setSelectedMembers((prev) => prev.filter((id) => id !== memberId));
  }, []);

  const clearSelection = useCallback(() => {
    setSelectedMembers([]);
  }, []);

  // Helpers para paginação
  const hasNextPage = currentPage < totalPages;
  const hasPreviousPage = currentPage > 1;

  // Navegação de páginas
  const nextPage = useCallback(() => {
    if (hasNextPage) {
      setCurrentPage((prev) => prev + 1);
    }
  }, [hasNextPage]);

  const previousPage = useCallback(() => {
    if (hasPreviousPage) {
      setCurrentPage((prev) => prev - 1);
    }
  }, [hasPreviousPage]);

  const goToPage = useCallback(
    (page: number) => {
      if (page >= 1 && page <= totalPages) {
        setCurrentPage(page);
      }
    },
    [totalPages]
  );

  // Agrupamento de membros
  const groupedMembers = useMemo(() => {
    if (groupingStrategy === "flat") {
      return { all: members };
    }

    const grouped: Record<string, MemberSummary[]> = {};

    members.forEach((member) => {
      const groupKey = getGroupKey(member, groupingStrategy);
      if (!grouped[groupKey]) {
        grouped[groupKey] = [];
      }
      grouped[groupKey].push(member);
    });

    return grouped;
  }, [members, groupingStrategy]);

  // Estatísticas
  const computedStats = useMemo(() => {
    // Usar estatísticas do backend se disponíveis
    if (stats) {
      return {
        total: stats.totalMembers || totalCount || members.length,
        active: stats.activeMembers || 0,
        pending: stats.pendingMembers || 0,
        inactive: (stats.inactiveMembers || 0) + (stats.suspendedMembers || 0),
        byGender: stats.byGender || {},
        byClub: stats.byClub || {},
      };
    }

    // Calcular estatísticas localmente se não houver dados do backend
    const localStats = {
      total: totalCount || members.length,
      active: 0,
      pending: 0,
      inactive: 0,
      byGender: {} as Record<string, number>,
      byClub: {} as Record<string, number>,
    };

    members.forEach((member: MemberSummary) => {
      // Status
      switch (member.status) {
        case "Active":
          localStats.active++;
          break;
        case "Pending":
          localStats.pending++;
          break;
        case "Inactive":
        case "Suspended":
          localStats.inactive++;
          break;
      }

      // Gênero
      localStats.byGender[member.gender] =
        (localStats.byGender[member.gender] || 0) + 1;

      // Clube
      localStats.byClub[member.clubName] =
        (localStats.byClub[member.clubName] || 0) + 1;
    });

    return localStats;
  }, [members, totalCount, stats]);

  // Membros filtrados (aplicação de filtros locais adicionais)
  const filteredMembers = useMemo(() => {
    return members; // Filtros principais são aplicados no backend
  }, [members]);

  const refresh = useCallback(() => {
    refetch();
  }, [refetch]);

  const groupByLevel = useCallback((level: number) => {
    // Implementar lógica de agrupamento por nível hierárquico
    console.log(`Agrupando por nível ${level}`);
  }, []);

  return {
    // Dados
    members,
    groups,
    filteredMembers,
    selectedMembers,

    // Estado
    isLoading,
    isError,
    error: error as Error | null,

    // Paginação
    currentPage,
    totalPages,
    totalCount,
    hasNextPage,
    hasPreviousPage,

    // Filtros
    filters,
    setFilters,
    clearFilters,

    // Seleção
    selectMember,
    selectAllMembers,
    deselectMember,
    clearSelection,

    // Ações
    refresh,
    nextPage,
    previousPage,
    goToPage,

    // Agrupamento
    groupedMembers,
    groupByLevel,

    // Estatísticas
    stats: computedStats,
  };
};

// Helper para determinar chave de agrupamento
const getGroupKey = (member: MemberSummary, strategy: string): string => {
  switch (strategy) {
    case "byClub":
      return member.clubName;
    case "byUnit":
      return member.unitName;
    case "hierarchical":
      return `${member.clubName} > ${member.unitName}`;
    default:
      return "all";
  }
};
