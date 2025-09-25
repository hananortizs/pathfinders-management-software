import { apiClient } from "./apiClient";
import type {
  ProfileSections,
  ProfileData,
  ContactInfo,
  AddressInfo,
  MedicalInfo,
  DocumentInfo,
  Preferences,
  ChangePasswordData,
  AuditLogEntry,
} from "../types/profile";

/**
 * Serviço para comunicação com APIs de perfil do usuário
 * Inclui operações CRUD para dados pessoais, contatos, endereço, saúde, documentos e preferências
 */
export class ProfileService {
  /**
   * Busca todos os dados do perfil do usuário autenticado
   * @returns Promise<ProfileSections> - Dados completos do perfil
   */
  async getProfile(): Promise<ProfileSections> {
    try {
      // Obter token do Zustand store
      const authData = localStorage.getItem("auth-storage");
      if (!authData) {
        throw new Error("Token não encontrado");
      }

      const parsedAuthData = JSON.parse(authData);
      const token = parsedAuthData.state?.token;

      if (!token) {
        throw new Error("Token não encontrado");
      }

      const response = await apiClient.post("/members/me", { token });
      console.log("🔍 Response completa:", response);
      console.log("🔍 Response data:", (response as any).data);

      // O backend retorna os dados diretamente em response.data, não em response.data.data
      const memberData = (response as any).data;

      if (!memberData) {
        console.error(
          "❌ memberData é undefined. Response:",
          (response as any).data
        );
        throw new Error("Dados do membro não encontrados na resposta");
      }

      console.log("✅ memberData encontrado:", memberData);

      // Converter MemberDto para ProfileSections
      return this.convertMemberDtoToProfileSections(memberData);
    } catch (error) {
      console.error("Erro ao buscar perfil:", error);
      console.log("Usando dados mockados como fallback");
      return this.getMockProfile();
    }
  }

  /**
   * Converte MemberDto para ProfileSections
   * @param memberData - Dados do membro do backend
   * @returns ProfileSections - Dados formatados para o frontend
   */
  private convertMemberDtoToProfileSections(memberData: any): ProfileSections {
    console.log(
      "🔍 convertMemberDtoToProfileSections - memberData:",
      memberData
    );

    return {
      personalData: {
        id: memberData.Id || "",
        firstName: memberData.FirstName || "",
        middleNames: memberData.MiddleNames || "",
        lastName: memberData.LastName || "",
        socialName: memberData.SocialName || "",
        dateOfBirth: memberData.DateOfBirth
          ? new Date(memberData.DateOfBirth).toISOString().split("T")[0]
          : "",
        gender: memberData.Gender || "Male",
        status: memberData.Status || "Active",
        clubName: memberData.ClubName || "",
        unitName: memberData.UnitName || "",
        roles: memberData.Roles || [],
        createdAt: memberData.CreatedAtUtc || new Date().toISOString(),
        updatedAt: memberData.UpdatedAtUtc || new Date().toISOString(),
      },
      contacts: (memberData.Contacts || []).map((contact: any) => ({
        id: contact.Id || "",
        type: contact.Type === "Mobile" ? "Phone" : contact.Type,
        value: contact.Value || "",
        isPrimary: contact.IsPrimary || false,
        isVerified: contact.IsVerified || false,
        category: contact.Category || "Personal",
      })),
      address: (() => {
        const addresses = (memberData.Addresses || []).map((address: any) => ({
          id: address.Id || "",
          zipCode: address.Cep || "",
          street: address.Street || "",
          number: address.Number || "",
          complement: address.Complement || "",
          neighborhood: address.Neighborhood || "",
          city: address.City || "",
          state: address.State || "",
          country: address.Country || "Brasil",
          type: address.Type || "Residential",
        }));

        // Validar se há pelo menos um endereço válido
        const hasValidAddress = addresses.some((addr: any) => 
          addr.zipCode && 
          addr.street && 
          addr.number && 
          addr.city && 
          addr.state
        );

        if (!hasValidAddress && addresses.length === 0) {
          // Se não há endereços, adicionar um endereço vazio para validação
          addresses.push({
            id: "temp-1",
            zipCode: "",
            street: "",
            number: "",
            complement: "",
            neighborhood: "",
            city: "",
            state: "",
            country: "Brasil",
            type: "Residential",
          });
        }

        return addresses;
      })(),
      medical: memberData.MedicalInfo || {
        id: "1",
        allergies: "",
        medications: "",
        bloodType: "",
        medicalConditions: "",
        emergencyContactName: "",
        emergencyContactPhone: "",
        emergencyContactRelationship: "",
        observations: "",
      },
      documents: {
        id: "1",
        cpf: memberData.Cpf || "",
        rg: memberData.Rg || "",
        rgIssueDate: memberData.RgIssueDate || "",
        rgIssuer: memberData.RgIssuer || "",
        passport: memberData.Passport || "",
        passportIssueDate: memberData.PassportIssueDate || "",
        passportExpiryDate: memberData.PassportExpiryDate || "",
        voterId: memberData.VoterId || "",
        workPermit: memberData.WorkPermit || "",
        otherDocuments: memberData.OtherDocuments || [],
      },
      preferences: {
        language: "pt-BR",
        theme: "system",
        timezone: "America/Sao_Paulo",
        notifications: {
          email: true,
          sms: false,
          push: true,
        },
      },
    };
  }

  /**
   * Atualiza dados pessoais do usuário
   * @param data - Dados pessoais para atualizar
   * @returns Promise<ProfileData> - Dados atualizados
   */
  async updatePersonalData(data: Partial<ProfileData>): Promise<ProfileData> {
    try {
      const response = await apiClient.patch("/me/personal-data", data);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao atualizar dados pessoais:", error);
      throw error;
    }
  }

  /**
   * Converte tipos de contato do frontend para o formato do backend
   */
  private mapContactTypeToBackend(frontendType: string): number {
    const typeMap: Record<string, number> = {
      Email: 3,
      Phone: 1, // Mobile
      WhatsApp: 4,
      Mobile: 1,
      Landline: 2,
      Facebook: 5,
      Instagram: 6,
      YouTube: 7,
      TikTok: 8,
      LinkedIn: 9,
      Twitter: 10,
      Website: 11,
      Other: 99,
    };
    return typeMap[frontendType] || 99; // Default para 'Other'
  }

  /**
   * Converte tipos de contato do backend para o formato do frontend
   */
  private mapContactTypeFromBackend(backendType: number): string {
    const typeMap: Record<number, string> = {
      1: "Phone", // Mobile
      2: "Phone", // Landline
      3: "Email",
      4: "WhatsApp",
      5: "Facebook",
      6: "Instagram",
      7: "YouTube",
      8: "TikTok",
      9: "LinkedIn",
      10: "Twitter",
      11: "Website",
      99: "Other",
    };
    return typeMap[backendType] || "Other";
  }

  /**
   * Atualiza contatos do usuário
   * @param contacts - Lista de contatos para atualizar
   * @returns Promise<ContactInfo[]> - Contatos atualizados
   */
  async updateContacts(contacts: ContactInfo[]): Promise<ContactInfo[]> {
    try {
      // Converte os contatos para o formato do backend
      const backendContacts = contacts.map((contact) => {
        const mappedType = this.mapContactTypeToBackend(contact.type);

        // Validar se o valor tem pelo menos 3 caracteres
        if (!contact.value || contact.value.trim().length < 3) {
          throw new Error(
            `Valor do contato deve ter pelo menos 3 caracteres: "${contact.value}"`
          );
        }

        // Validar se o tipo é válido
        if (mappedType === 99 && !["Other"].includes(contact.type)) {
          throw new Error(`Tipo de contato inválido: "${contact.type}"`);
        }

        return {
          Type: mappedType, // Já é um número
          Value: contact.value.trim(), // Remover espaços em branco
          IsPrimary: Boolean(contact.isPrimary), // Garantir que é boolean
          Notes: contact.notes || null, // Usar null para campos opcionais
        };
      });

      // Obter token do Zustand store (mesmo método usado em getProfile)
      const authData = localStorage.getItem("auth-storage");
      if (!authData) {
        throw new Error("Token não encontrado");
      }

      const parsedAuthData = JSON.parse(authData);
      const token = parsedAuthData.state?.token;

      if (!token) {
        throw new Error("Token não encontrado");
      }

      const requestBody = {
        token: token,
        contacts: backendContacts,
      };

      // Log temporário para debug do erro 400
      console.log(
        "🔍 DEBUG - Request body sendo enviado:",
        JSON.stringify(requestBody, null, 2)
      );
      console.log("🔍 DEBUG - Número de contatos:", backendContacts.length);
      backendContacts.forEach((contact, index) => {
        console.log(`🔍 DEBUG - Contato ${index + 1}:`, {
          Type: contact.Type,
          Value: contact.Value,
          ValueLength: contact.Value?.length,
          IsPrimary: contact.IsPrimary,
          Notes: contact.Notes,
        });
      });

      const response = await apiClient.patch(
        "/members/me/contacts",
        requestBody
      );

      // Converte a resposta de volta para o formato do frontend
      const frontendContacts = (response as any).data.map((contact: any) => ({
        id: contact.id || `temp-${Date.now()}`,
        type: this.mapContactTypeFromBackend(contact.type),
        value: contact.value,
        isPrimary: contact.isPrimary,
        isVerified: contact.isVerified || false,
        category: contact.category || "Personal",
        notes: contact.notes,
      }));

      return frontendContacts;
    } catch (error) {
      console.error("Erro ao atualizar contatos:", error);
      throw error;
    }
  }

  /**
   * Atualiza endereço do usuário
   * @param address - Dados de endereço para atualizar
   * @returns Promise<AddressInfo> - Endereço atualizado
   */
  async updateAddress(address: AddressInfo): Promise<AddressInfo> {
    try {
      const response = await apiClient.patch("/me/address", address);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao atualizar endereço:", error);
      throw error;
    }
  }

  /**
   * Atualiza dados médicos do usuário
   * @param medical - Dados médicos para atualizar
   * @returns Promise<MedicalInfo> - Dados médicos atualizados
   */
  async updateMedicalData(medical: MedicalInfo): Promise<MedicalInfo> {
    try {
      const response = await apiClient.patch("/me/medical", medical);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao atualizar dados médicos:", error);
      throw error;
    }
  }

  /**
   * Atualiza documentos do usuário
   * @param documents - Dados de documentos para atualizar
   * @returns Promise<DocumentInfo> - Documentos atualizados
   */
  async updateDocuments(documents: DocumentInfo): Promise<DocumentInfo> {
    try {
      const response = await apiClient.patch("/me/documents", documents);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao atualizar documentos:", error);
      throw error;
    }
  }

  /**
   * Atualiza preferências do usuário
   * @param preferences - Preferências para atualizar
   * @returns Promise<Preferences> - Preferências atualizadas
   */
  async updatePreferences(preferences: Preferences): Promise<Preferences> {
    try {
      const response = await apiClient.patch("/me/preferences", preferences);
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao atualizar preferências:", error);
      throw error;
    }
  }

  /**
   * Altera a senha do usuário
   * @param passwordData - Dados para alteração de senha
   * @returns Promise<void>
   */
  async changePassword(passwordData: ChangePasswordData): Promise<void> {
    try {
      await apiClient.post("/me/change-password", passwordData);
    } catch (error) {
      console.error("Erro ao alterar senha:", error);
      throw error;
    }
  }

  /**
   * Faz upload do avatar do usuário
   * @param file - Arquivo de imagem do avatar
   * @returns Promise<string> - URL do avatar
   */
  async uploadAvatar(file: File): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("avatar", file);

      const response = await apiClient.post("/me/avatar", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      return (response as any).data.avatarUrl;
    } catch (error) {
      console.error("Erro ao fazer upload do avatar:", error);
      throw error;
    }
  }

  /**
   * Registra revelação de campo sensível para auditoria
   * @param fieldName - Nome do campo revelado
   * @param entityId - ID da entidade
   * @param reason - Motivo da revelação (opcional)
   * @returns Promise<void>
   */
  async logSensitiveFieldReveal(
    fieldName: string,
    entityId: string,
    reason?: string
  ): Promise<void> {
    try {
      await apiClient.post("/audit/reveal", {
        fieldName,
        entityId,
        reason,
        timestamp: new Date().toISOString(),
      });
    } catch (error) {
      console.error("Erro ao registrar revelação de campo sensível:", error);
      // Não falhar a operação principal por causa do log de auditoria
    }
  }

  /**
   * Busca logs de auditoria do usuário
   * @param page - Página atual
   * @param pageSize - Tamanho da página
   * @returns Promise<{ logs: AuditLogEntry[], total: number, hasMore: boolean }>
   */
  async getAuditLogs(
    page: number = 1,
    pageSize: number = 20
  ): Promise<{ logs: AuditLogEntry[]; total: number; hasMore: boolean }> {
    try {
      const queryParams = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString(),
      });
      const response = await apiClient.get(
        `/me/audit-logs?${queryParams.toString()}`
      );
      return (response as any).data;
    } catch (error) {
      console.error("Erro ao buscar logs de auditoria:", error);
      throw error;
    }
  }

  /**
   * Valida CPF único
   * @param cpf - CPF para validar
   * @param excludeId - ID do usuário atual (para edição)
   * @returns Promise<boolean> - True se CPF é único
   */
  async validateCPF(cpf: string, excludeId?: string): Promise<boolean> {
    try {
      const response = await apiClient.post("/me/validate-cpf", {
        cpf,
        excludeId,
      });
      return (response as any).data.isValid;
    } catch (error) {
      console.error("Erro ao validar CPF:", error);
      return false;
    }
  }

  /**
   * Valida email único
   * @param email - Email para validar
   * @param excludeId - ID do usuário atual (para edição)
   * @returns Promise<boolean> - True se email é único
   */
  async validateEmail(email: string, excludeId?: string): Promise<boolean> {
    try {
      const response = await apiClient.post("/me/validate-email", {
        email,
        excludeId,
      });
      return (response as any).data.isValid;
    } catch (error) {
      console.error("Erro ao validar email:", error);
      return false;
    }
  }

  /**
   * Valida telefone único
   * @param phone - Telefone para validar
   * @param excludeId - ID do usuário atual (para edição)
   * @returns Promise<boolean> - True se telefone é único
   */
  async validatePhone(phone: string, excludeId?: string): Promise<boolean> {
    try {
      const response = await apiClient.post("/me/validate-phone", {
        phone,
        excludeId,
      });
      return (response as any).data.isValid;
    } catch (error) {
      console.error("Erro ao validar telefone:", error);
      return false;
    }
  }

  /**
   * Busca dados mockados para desenvolvimento
   * @returns Promise<ProfileSections> - Dados mockados do perfil
   */
  async getMockProfile(): Promise<ProfileSections> {
    // Simular delay de API
    await new Promise((resolve) => setTimeout(resolve, 1000));

    return {
      personalData: {
        id: "1",
        firstName: "Ricardo",
        lastName: "Gonzaga",
        middleNames: "Ferreira Ortiz",
        socialName: "Ricardo",
        dateOfBirth: "2012-09-14",
        gender: "Male",
        status: "Active",
        clubName: "Pássaro Celeste",
        unitName: "Falcão",
        roles: ["Membro"],
        createdAt: "2024-01-15T10:30:00Z",
        updatedAt: "2024-01-15T10:30:00Z",
      },
      contacts: [
        {
          id: "1",
          type: "Email",
          value: "ricardo@example.com",
          isPrimary: true,
          isVerified: true,
          category: "Personal",
        },
        {
          id: "2",
          type: "Phone",
          value: "+5511999999999",
          isPrimary: false,
          isVerified: false,
          category: "Personal",
        },
      ],
      address: [
        {
          id: "1",
          zipCode: "01234-567",
          street: "Rua das Flores",
          number: "123",
          complement: "Apto 45",
          neighborhood: "Centro",
          city: "São Paulo",
          state: "SP",
          country: "Brasil",
          type: "Residential",
        },
      ],
      medical: {
        id: "1",
        allergies: "Pólen, amendoim",
        medications: "Vitamina D",
        bloodType: "O+",
        medicalConditions: "Asma leve",
        emergencyContactName: "Maria Gonzaga",
        emergencyContactPhone: "+5511888888888",
        emergencyContactRelationship: "Mãe",
        notes: "Usar inalador em caso de crise",
      },
      documents: {
        id: "1",
        cpf: "12345678901",
        rg: "123456789",
        rgIssuer: "SSP",
        rgIssueDate: "2020-01-15",
        passport: "BR123456789",
        passportIssueDate: "2022-03-10",
        passportExpiryDate: "2032-03-10",
        voterId: "123456789012",
        workPermit: "WP-2024-001",
        otherDocuments: ["CNH", "Título de Eleitor"],
      },
      preferences: {
        language: "pt-BR",
        theme: "system",
        notifications: {
          email: true,
          sms: false,
          push: true,
        },
        timezone: "America/Sao_Paulo",
      },
    };
  }
}

// Instância singleton do serviço
export const profileService = new ProfileService();
