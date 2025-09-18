import type { ApiResponse } from "../types/auth";

// Tipos para a dashboard
export interface DashboardStats {
  totalActiveMembers: number;
  totalPendingMembers: number;
  totalInactiveMembers: number;
  totalActiveClubs: number;
  totalUpcomingEvents: number;
  totalEventsThisMonth: number;
  participationRate: number;
  newMembersThisMonth: number;
  specialtiesEarnedThisMonth: number;
  scarfPromotionsThisMonth: number;
}

export interface RecentActivity {
  id: string;
  type: string;
  description: string;
  date: string;
  memberName?: string;
  clubName?: string;
  status: string;
  priority: string;
}

export interface UpcomingEvent {
  id: string;
  title: string;
  date: string;
  location: string;
  participants: number;
  maxParticipants: number;
  eventType: string;
  status: string;
  clubName?: string;
  districtName?: string;
  description?: string;
  requiresRegistration: boolean;
  registrationDeadline?: string;
}

export interface DashboardData {
  stats: DashboardStats;
  recentActivities: RecentActivity[];
  upcomingEvents: UpcomingEvent[];
  userAccessLevel: string;
  userScope: string;
  userScopeId?: string;
  userScopeName?: string;
  lastUpdated: string;
}

class DashboardService {
  /**
   * Obtém dados completos da dashboard
   */
  async getDashboardData(token: string): Promise<ApiResponse<DashboardData>> {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/dashboard/data`,
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

      return await response.json();
    } catch (error) {
      console.error("Erro ao obter dados da dashboard:", error);
      return {
        isSuccess: false,
        message: error instanceof Error ? error.message : "Erro de conexão",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Obtém apenas estatísticas da dashboard
   */
  async getDashboardStats(token: string): Promise<ApiResponse<DashboardStats>> {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/dashboard/stats`,
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

      return await response.json();
    } catch (error) {
      console.error("Erro ao obter estatísticas:", error);
      return {
        isSuccess: false,
        message: error instanceof Error ? error.message : "Erro de conexão",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Obtém atividades recentes
   */
  async getRecentActivities(
    token: string,
    limit: number = 10
  ): Promise<ApiResponse<RecentActivity[]>> {
    try {
      const response = await fetch(
        `${
          import.meta.env.VITE_API_BASE_URL
        }/dashboard/activities?limit=${limit}`,
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

      return await response.json();
    } catch (error) {
      console.error("Erro ao obter atividades recentes:", error);
      return {
        isSuccess: false,
        message: error instanceof Error ? error.message : "Erro de conexão",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Obtém próximos eventos
   */
  async getUpcomingEvents(
    token: string,
    limit: number = 5
  ): Promise<ApiResponse<UpcomingEvent[]>> {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/dashboard/events?limit=${limit}`,
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

      return await response.json();
    } catch (error) {
      console.error("Erro ao obter próximos eventos:", error);
      return {
        isSuccess: false,
        message: error instanceof Error ? error.message : "Erro de conexão",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }

  /**
   * Obtém dashboard de administrador de sistema
   */
  async getSystemAdminDashboard(
    token: string
  ): Promise<ApiResponse<DashboardData>> {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/dashboard/system-admin`,
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

      return await response.json();
    } catch (error) {
      console.error("Erro ao obter dashboard de admin:", error);
      return {
        isSuccess: false,
        message: error instanceof Error ? error.message : "Erro de conexão",
        statusCode: 500,
        errors: ["Erro de rede"],
      };
    }
  }
}

export const dashboardService = new DashboardService();
