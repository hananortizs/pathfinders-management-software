import { apiClient } from "./apiClient";

export interface DashboardKPIsData {
  totalMembers: number;
  activeMembers: number;
  newMembers30Days: number;
  clubName?: string;
  unitName?: string;
}

export interface DashboardSectionsData {
  upcomingEvents: Array<{
    id: string;
    title: string;
    date: string;
    location: string;
    isEligible: boolean;
    registrationStatus: "registered" | "available" | "waitlist" | "closed";
  }>;
  classProgress: Array<{
    id: string;
    name: string;
    progress: number;
    status: "in_progress" | "completed" | "not_started";
    nextStep?: string;
  }>;
  specializations: Array<{
    id: string;
    name: string;
    category: string;
    status: "in_progress" | "completed" | "not_started";
    progress: number;
  }>;
}

export interface DashboardData {
  kpis: DashboardKPIsData;
  sections: DashboardSectionsData;
}

/**
 * Serviço para operações relacionadas à dashboard
 */
export class DashboardService {
  /**
   * Busca dados completos da dashboard
   * @returns Dados da dashboard incluindo KPIs e seções
   */
  async getDashboard(): Promise<DashboardData> {
    // Usar dados mockados por enquanto (endpoints não implementados no backend)
    return this.getMockDashboardData();
  }

  /**
   * Busca apenas os KPIs da dashboard
   * @returns Dados dos KPIs
   */
  async getKPIs(): Promise<DashboardKPIsData> {
    // Usar dados mockados por enquanto (endpoints não implementados no backend)
    return this.getMockDashboardData().kpis;
  }

  /**
   * Busca apenas as seções específicas da dashboard
   * @returns Dados das seções
   */
  async getSections(): Promise<DashboardSectionsData> {
    // Usar dados mockados por enquanto (endpoints não implementados no backend)
    return this.getMockDashboardData().sections;
  }

  /**
   * Busca eventos do encadeamento hierárquico do usuário
   * @param params Parâmetros de filtro
   * @returns Lista de eventos hierárquicos
   */
  async getHierarchicalEvents(params?: {
    level?: string;
    period?: string;
    eligibility?: string;
  }) {
    try {
      const queryParams = new URLSearchParams();
      if (params?.level) queryParams.append('level', params.level);
      if (params?.period) queryParams.append('period', params.period);
      if (params?.eligibility) queryParams.append('eligibility', params.eligibility);
      
      const url = `/events/hierarchy${queryParams.toString() ? `?${queryParams.toString()}` : ''}`;
      const response = await apiClient.get(url);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao buscar eventos hierárquicos:", error);
      throw error;
    }
  }

  /**
   * Verifica elegibilidade do usuário para um evento
   * @param eventId ID do evento
   * @returns Status de elegibilidade
   */
  async checkEventEligibility(eventId: string) {
    try {
      const response = await apiClient.get(`/events/eligibility/${eventId}`);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao verificar elegibilidade do evento:", error);
      throw error;
    }
  }

  /**
   * Inscreve o usuário em um evento
   * @param eventId ID do evento
   * @param data Dados da inscrição
   * @returns Resultado da inscrição
   */
  async registerForEvent(eventId: string, data?: any) {
    try {
      const response = await apiClient.post(
        `/events/${eventId}/register`,
        data
      );
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao inscrever-se no evento:", error);
      throw error;
    }
  }

  /**
   * Busca participantes de um evento (baseado no papel do usuário)
   * @param eventId ID do evento
   * @returns Lista de participantes
   */
  async getEventParticipants(eventId: string) {
    try {
      const response = await apiClient.get(`/events/${eventId}/participants`);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao buscar participantes do evento:", error);
      throw error;
    }
  }

  /**
   * Retorna dados mockados para desenvolvimento
   * @returns Dados mockados da dashboard
   */
  private getMockDashboardData(): DashboardData {
    return {
      kpis: {
        totalMembers: 45,
        activeMembers: 38,
        newMembers30Days: 3,
        clubName: "Pássaro Celeste",
        unitName: "Falcão",
      },
      sections: {
        upcomingEvents: [
          {
            id: "1",
            title: "Acampamento de Verão 2025",
            date: "15/12/2025",
            location: "Sítio Esperança",
            isEligible: true,
            registrationStatus: "available",
          },
          {
            id: "2",
            title: "Reunião de Unidade",
            date: "28/09/2025",
            location: "Igreja Central",
            isEligible: true,
            registrationStatus: "registered",
          },
          {
            id: "3",
            title: "Concurso de Especialidades",
            date: "05/10/2025",
            location: "Clube Pássaro Celeste",
            isEligible: false,
            registrationStatus: "closed",
          },
        ],
        classProgress: [
          {
            id: "1",
            name: "Companheiro",
            progress: 75,
            status: "in_progress",
            nextStep: "Completar especialidade de Primeiros Socorros",
          },
          {
            id: "2",
            name: "Pesquisador",
            progress: 100,
            status: "completed",
            nextStep: "Iniciar classe Pioneiro",
          },
          {
            id: "3",
            name: "Pioneiro",
            progress: 0,
            status: "not_started",
            nextStep: "Iniciar classe Pioneiro",
          },
        ],
        specializations: [
          {
            id: "1",
            name: "Primeiros Socorros",
            category: "AD",
            status: "in_progress",
            progress: 60,
          },
          {
            id: "2",
            name: "Natação",
            category: "HM",
            status: "completed",
            progress: 100,
          },
          {
            id: "3",
            name: "Culinária",
            category: "AA",
            status: "in_progress",
            progress: 30,
          },
          {
            id: "4",
            name: "Astronomia",
            category: "AM",
            status: "not_started",
            progress: 0,
          },
          {
            id: "5",
            name: "Jardinagem",
            category: "AP",
            status: "completed",
            progress: 100,
          },
          {
            id: "6",
            name: "Fotografia",
            category: "AR",
            status: "in_progress",
            progress: 45,
          },
        ],
      },
    };
  }
}

export const dashboardService = new DashboardService();
