import { useState, useMemo } from "react";
import type { PendencyItem } from "../components/profile/PendencyModal";
import type { ProfileSections } from "../types/profile";

export interface SectionPendencies {
  personalData: PendencyItem[];
  contacts: PendencyItem[];
  address: PendencyItem[];
  health: PendencyItem[];
  documents: PendencyItem[];
}

/**
 * Hook para gerenciar pendências das seções do perfil
 * Combina dados reais do perfil com descrições mockadas para o modal
 */
export const usePendencies = (profileData?: ProfileSections) => {
  const [pendencies] = useState<SectionPendencies>({
    personalData: [
      {
        id: "name-complete",
        title: "Nome completo obrigatório",
        description: "O nome completo é necessário para completar o cadastro",
        severity: "warning",
        actionRequired: "Preencha o campo 'Nome completo'",
      },
      {
        id: "birth-date",
        title: "Data de nascimento inválida",
        description:
          "A data de nascimento deve ser válida e o usuário deve ter pelo menos 10 anos",
        severity: "error",
        actionRequired: "Corrija a data de nascimento",
      },
    ],
    contacts: [
      {
        id: "phone-validation",
        title: "Validar número de telefone",
        description: "O número de telefone precisa ser verificado via SMS",
        severity: "warning",
        actionRequired: "Clique em 'Verificar' para enviar código SMS",
      },
      {
        id: "email-validation",
        title: "Validar e-mail",
        description: "O endereço de e-mail precisa ser verificado",
        severity: "warning",
        actionRequired:
          "Clique em 'Verificar' para enviar e-mail de confirmação",
      },
      {
        id: "primary-contact",
        title: "Definir contato primário",
        description: "Pelo menos um contato deve ser marcado como primário",
        severity: "info",
        actionRequired: "Marque um contato como 'Primário'",
      },
    ],
    address: [
      {
        id: "zipcode-validation",
        title: "Validar CEP",
        description: "O CEP informado não foi encontrado ou é inválido",
        severity: "error",
        actionRequired: "Digite um CEP válido",
      },
      {
        id: "address-complete",
        title: "Endereço incompleto",
        description: "Preencha todos os campos obrigatórios do endereço",
        severity: "warning",
        actionRequired: "Complete os campos: Rua, Número, Bairro, Cidade",
      },
    ],
    health: [
      {
        id: "emergency-contact",
        title: "Contato de emergência obrigatório",
        description:
          "Para menores de idade, é obrigatório informar um contato de emergência",
        severity: "warning",
        actionRequired: "Preencha os dados do contato de emergência",
      },
      {
        id: "medical-info",
        title: "Informações médicas recomendadas",
        description: "Alergias e medicações são importantes para eventos",
        severity: "info",
        actionRequired: "Considere preencher as informações médicas",
      },
    ],
    documents: [
      {
        id: "cpf-validation",
        title: "CPF inválido",
        description: "O CPF informado não é válido ou já está em uso",
        severity: "error",
        actionRequired: "Verifique o CPF ou contate a secretaria",
      },
      {
        id: "rg-missing",
        title: "RG não informado",
        description: "O RG é recomendado para identificação",
        severity: "info",
        actionRequired: "Informe o número do RG",
      },
    ],
  });

  // Gerar pendências baseadas nos dados reais do perfil
  const generateRealPendencies = useMemo(() => {
    if (!profileData) return {};

    const { personalData, contacts, address, medical, documents } = profileData;

    return {
      personal: [
        ...(personalData.firstName
          ? []
          : [
              {
                id: "name-complete",
                title: "Nome completo obrigatório",
                description:
                  "O nome completo é necessário para completar o cadastro",
                severity: "warning" as const,
                actionRequired: "Preencha o campo 'Nome completo'",
              },
            ]),
        ...(personalData.lastName
          ? []
          : [
              {
                id: "lastname-complete",
                title: "Sobrenome obrigatório",
                description:
                  "O sobrenome é necessário para completar o cadastro",
                severity: "warning" as const,
                actionRequired: "Preencha o campo 'Sobrenome'",
              },
            ]),
        ...(personalData.dateOfBirth
          ? []
          : [
              {
                id: "birth-date",
                title: "Data de nascimento obrigatória",
                description:
                  "A data de nascimento é necessária para completar o cadastro",
                severity: "warning" as const,
                actionRequired: "Preencha o campo 'Data de nascimento'",
              },
            ]),
        ...(personalData.gender
          ? []
          : [
              {
                id: "gender-required",
                title: "Gênero obrigatório",
                description: "O gênero é necessário para completar o cadastro",
                severity: "warning" as const,
                actionRequired: "Selecione o gênero",
              },
            ]),
      ],
      contacts: [
        ...(contacts && contacts.length > 0
          ? []
          : [
              {
                id: "no-contacts",
                title: "Nenhum contato cadastrado",
                description: "É necessário cadastrar pelo menos um contato",
                severity: "error" as const,
                actionRequired: "Adicione um contato (e-mail ou telefone)",
              },
            ]),
        ...(contacts?.some((c) => c.type === "Email" && c.isPrimary)
          ? []
          : [
              {
                id: "primary-email",
                title: "E-mail primário obrigatório",
                description: "É necessário ter um e-mail marcado como primário",
                severity: "warning" as const,
                actionRequired: "Marque um e-mail como primário",
              },
            ]),
        ...(contacts?.some((c) => c.type === "Phone" && c.isPrimary)
          ? []
          : [
              {
                id: "primary-phone",
                title: "Telefone primário obrigatório",
                description:
                  "É necessário ter um telefone marcado como primário",
                severity: "warning" as const,
                actionRequired: "Marque um telefone como primário",
              },
            ]),
      ],
      address: [
        ...(address && address.length > 0
          ? []
          : [
              {
                id: "no-address",
                title: "Endereço obrigatório",
                description: "O usuário deve ter pelo menos um endereço válido",
                severity: "error" as const,
                actionRequired: "Adicione um endereço completo",
              },
            ]),
        ...(address?.some(
          (a) => a.zipCode && a.street && a.number && a.city && a.state
        )
          ? []
          : [
              {
                id: "incomplete-address",
                title: "Endereço incompleto",
                description:
                  "Preencha todos os campos obrigatórios do endereço",
                severity: "warning" as const,
                actionRequired:
                  "Complete os campos: CEP, Rua, Número, Cidade, Estado",
              },
            ]),
      ],
      health: [
        ...(medical?.emergencyContactName && medical?.emergencyContactPhone
          ? []
          : [
              {
                id: "emergency-contact",
                title: "Contato de emergência obrigatório",
                description: "É necessário informar um contato de emergência",
                severity: "warning" as const,
                actionRequired: "Preencha os dados do contato de emergência",
              },
            ]),
      ],
      documents: [
        ...(documents?.cpf
          ? []
          : [
              {
                id: "cpf-required",
                title: "CPF obrigatório",
                description: "O CPF é obrigatório para completar o cadastro",
                severity: "error" as const,
                actionRequired: "Preencha o campo 'CPF'",
              },
            ]),
      ],
    };
  }, [profileData]);

  const getPendenciesForSection = (sectionId: string): PendencyItem[] => {
    // Usar dados reais se disponíveis, senão usar dados mockados
    const realPendencies =
      generateRealPendencies[sectionId as keyof typeof generateRealPendencies];
    if (realPendencies) return realPendencies;

    // Fallback para dados mockados
    switch (sectionId) {
      case "personal":
        return pendencies.personalData;
      case "contacts":
        return pendencies.contacts;
      case "address":
        return pendencies.address;
      case "health":
        return pendencies.health;
      case "documents":
        return pendencies.documents;
      default:
        return [];
    }
  };

  const getPendencyCount = (sectionId: string): number => {
    return getPendenciesForSection(sectionId).filter((p) => !p.isCompleted)
      .length;
  };

  const getTotalPendencyCount = (): number => {
    return Object.values(pendencies)
      .flat()
      .filter((p) => !p.isCompleted).length;
  };

  const getSectionName = (sectionId: string): string => {
    const names: Record<string, string> = {
      personal: "Dados Pessoais",
      contacts: "Contatos",
      address: "Endereço",
      health: "Saúde",
      documents: "Documentos",
    };
    return names[sectionId] || "Seção";
  };

  return {
    pendencies,
    getPendenciesForSection,
    getPendencyCount,
    getTotalPendencyCount,
    getSectionName,
  };
};
