import { useMemo } from "react";
import type { ProfileSections } from "../types/profile";

// Helper function to check if a person is a minor (under 18)
const isMinor = (dateOfBirth: string): boolean => {
  const birthDate = new Date(dateOfBirth);
  const today = new Date();
  const age = today.getFullYear() - birthDate.getFullYear();
  const monthDiff = today.getMonth() - birthDate.getMonth();

  if (
    monthDiff < 0 ||
    (monthDiff === 0 && today.getDate() < birthDate.getDate())
  ) {
    return age - 1 < 18;
  }

  return age < 18;
};

/**
 * Hook para calcular pendências de dados do perfil
 * Retorna contadores de campos pendentes por seção
 */
export const useProfilePendencies = (
  profileData: ProfileSections | undefined
) => {
  return useMemo(() => {
    if (!profileData) {
      return {
        personalData: 0,
        contacts: 0,
        address: 0,
        health: 0,
        documents: 0,
        total: 0,
      };
    }

    const { personalData, contacts, address, medical, documents } = profileData;

    // Pendências de Dados Pessoais
    const personalDataPendencies = [
      !personalData.firstName,
      !personalData.lastName,
      !personalData.dateOfBirth,
      !personalData.gender,
      !personalData.clubName,
      !personalData.unitName,
    ].filter(Boolean).length;

    // Pendências de Contatos
    const contactsPendencies = [
      !contacts || contacts.length === 0,
      !contacts?.some((c) => c.type === "Email" && c.isPrimary),
      !contacts?.some((c) => c.type === "Phone" && c.isPrimary),
      // Para menores de idade, verificar se tem contato de emergência
      personalData.dateOfBirth &&
        isMinor(personalData.dateOfBirth) &&
        !contacts?.some((c) => c.category === "Emergency"),
    ].filter(Boolean).length;

    // Pendências de Endereço
    const addressPendencies = [
      !address || address.length === 0,
      !address?.some(
        (a) => a.zipCode && a.street && a.number && a.city && a.state
      ),
    ].filter(Boolean).length;

    // Pendências de Saúde
    const healthPendencies = [
      !medical?.allergies,
      !medical?.medications,
      !medical?.bloodType,
      !medical?.emergencyContactName,
      !medical?.emergencyContactPhone,
    ].filter(Boolean).length;

    // Pendências de Documentos
    const documentsPendencies = [!documents?.cpf, !documents?.rg].filter(
      Boolean
    ).length;

    const total =
      personalDataPendencies +
      contactsPendencies +
      addressPendencies +
      healthPendencies +
      documentsPendencies;

    return {
      personalData: personalDataPendencies,
      contacts: contactsPendencies,
      address: addressPendencies,
      health: healthPendencies,
      documents: documentsPendencies,
      total,
    };
  }, [profileData]);
};
