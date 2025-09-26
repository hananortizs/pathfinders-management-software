import React, { useState } from "react";
import {
  Box,
  Container,
  Typography,
  Alert,
  CircularProgress,
  Snackbar,
  useMediaQuery,
  useTheme,
  IconButton,
  Breadcrumbs,
  Link,
  Button,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import HomeIcon from "@mui/icons-material/Home";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth } from "../hooks/useAuth";
import { usePageTitle } from "../hooks/usePageTitle";
import { useProfilePendencies } from "../hooks/useProfilePendencies";
import { profileService } from "../services/profileService";
import type { ProfileSections } from "../types/profile";
import { ProfileHeader } from "../components/profile/ProfileHeader";
import { ProfileTabsWithModal } from "../components/profile/ProfileTabs";
import { PersonalDataSection } from "../components/profile/sections/PersonalDataSection";
import { ContactsSection } from "../components/profile/sections/ContactsSection";
import { AddressSection } from "../components/profile/sections/AddressSection";
import { HealthSection } from "../components/profile/sections/HealthSection";
import { DocumentsSection } from "../components/profile/sections/DocumentsSection";
import { PreferencesSection } from "../components/profile/sections/PreferencesSection";
import { SecuritySection } from "../components/profile/sections/SecuritySection";

/**
 * Página principal do perfil do usuário
 * Inclui todas as seções: dados pessoais, contatos, endereço, saúde, documentos, preferências e segurança
 */
const ProfilePage: React.FC = () => {
  const { user } = useAuth();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isXs = useMediaQuery(theme.breakpoints.down("xs"));
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  // Definir título da página
  usePageTitle("Meu Perfil");

  // Estado local
  const [activeTab, setActiveTab] = useState("personal");
  const [editingSections, setEditingSections] = useState<
    Record<string, boolean>
  >({});
  const [snackbar, setSnackbar] = useState<{
    open: boolean;
    message: string;
    severity: "success" | "error" | "warning" | "info";
  }>({
    open: false,
    message: "",
    severity: "info",
  });

  // Funções de navegação
  const handleBackToDashboard = () => {
    navigate("/dashboard");
  };

  const handleTabChange = (tabId: string) => {
    setActiveTab(tabId);

    // Scroll suave para a seção ativa apenas em desktop
    if (!isMobile) {
      setTimeout(() => {
        const element = document.getElementById(tabId);
        if (element) {
          element.scrollIntoView({
            behavior: "smooth",
            block: "start",
          });
        }
      }, 100);
    }

    // Para mobile (carrossel), não fazer scroll automático
  };

  // Função para detectar campos incompletos e gerar pontos de atenção
  const getIncompleteFields = (data: ProfileSections) => {
    const incomplete: string[] = [];

    // Dados pessoais
    if (!data.personalData.firstName) incomplete.push("Nome");
    if (!data.personalData.lastName) incomplete.push("Sobrenome");
    if (!data.personalData.dateOfBirth) incomplete.push("Data de nascimento");
    if (!data.personalData.gender) incomplete.push("Gênero");

    // Contatos
    const hasEmail = data.contacts.some((c) => c.type === "Email" && c.value);
    const hasPhone = data.contacts.some((c) => c.type === "Phone" && c.value);
    if (!hasEmail) incomplete.push("Email");
    if (!hasPhone) incomplete.push("Telefone");

    // Endereço
    const address = data.address[0];
    if (
      !address?.zipCode ||
      !address?.street ||
      !address?.number ||
      !address?.city
    ) {
      incomplete.push("Endereço completo");
    }

    // Documentos
    if (!data.documents.cpf) incomplete.push("CPF");

    return incomplete;
  };

  // Buscar dados do perfil
  const {
    data: profileData,
    isLoading,
    error,
    refetch,
  } = useQuery<ProfileSections>({
    queryKey: ["profile", user?.id],
    queryFn: async () => {
      // Se não há usuário, retornar dados vazios
      if (!user) {
        return {
          personalData: {
            id: "",
            firstName: "",
            lastName: "",
            socialName: "",
            dateOfBirth: "",
            gender: "Male" as const,
            status: "Pending" as const,
            clubName: "",
            unitName: "",
            roles: [],
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString(),
          },
          contacts: [],
          address: [
            {
              id: "1",
              name: "Endereço Principal",
              zipCode: "",
              street: "",
              number: "",
              complement: "",
              neighborhood: "",
              city: "",
              state: "",
              country: "Brasil",
              type: "Residential" as const,
            },
          ],
          medical: {
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
            cpf: "",
            rg: "",
            rgIssueDate: "",
            rgIssuer: "",
            passport: "",
            passportIssueDate: "",
            passportExpiryDate: "",
            voterId: "",
            workPermit: "",
            otherDocuments: [],
          },
          preferences: {
            id: "1",
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

      // Tentar buscar dados reais do backend primeiro
      try {
        const realData = await profileService.getProfile();
        if (realData) {
          return realData;
        }
      } catch (error) {
        console.log(
          "Dados reais não disponíveis, usando dados do usuário autenticado"
        );
      }

      // Criar perfil baseado nos dados do usuário autenticado
      return {
        personalData: {
          id: user.id || "",
          firstName: user.firstName || "",
          lastName: user.lastName || "",
          socialName: (user as any).socialName || "",
          dateOfBirth: (user as any).dateOfBirth || "",
          gender: (user as any).gender || "Male",
          status: (user as any).status || "Active",
          clubName: (user as any).clubName || "",
          unitName: (user as any).unitName || "",
          roles: user.roles || [],
          createdAt: (user as any).createdAt || new Date().toISOString(),
          updatedAt: (user as any).updatedAt || new Date().toISOString(),
        },
        contacts: [
          {
            id: "1",
            type: "Email" as const,
            value: user.email || "",
            isPrimary: true,
            isVerified: true,
            category: "Personal" as const,
          },
          {
            id: "2",
            type: "Phone" as const,
            value: (user as any).phone || "",
            isPrimary: false,
            isVerified: false,
            category: "Personal" as const,
          },
        ],
        address: [
          {
            id: "1",
            name: "Endereço Principal",
            zipCode: "",
            street: "",
            number: "",
            complement: "",
            neighborhood: "",
            city: "",
            state: "",
            country: "Brasil",
            type: "Residential" as const,
          },
        ],
        medical: {
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
          cpf: "",
          rg: "",
          rgIssueDate: "",
          rgIssuer: "",
          passport: "",
          passportIssueDate: "",
          passportExpiryDate: "",
          voterId: "",
          workPermit: "",
          otherDocuments: [],
        },
        preferences: {
          id: "1",
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
    },
    enabled: true,
    // Controle de cache para testes
    staleTime: 0, // Sempre considera os dados como "stale" (obsoletos)
    gcTime: 0, // Não mantém cache após o componente ser desmontado
    refetchOnWindowFocus: false, // Não refaz a consulta ao focar na janela
    refetchOnMount: true, // Sempre refaz a consulta ao montar o componente
  });

  // Calcular pendências de dados
  const pendencies = useProfilePendencies(profileData);

  // Mutação para atualizar seções
  const updateSectionMutation = useMutation({
    mutationFn: async ({ section, data }: { section: string; data: any }) => {
      switch (section) {
        case "personal":
          return profileService.updatePersonalData(data);
        case "contacts":
          return profileService.updateContacts(data);
        case "address":
          return profileService.updateAddress(data);
        case "health":
          return profileService.updateMedicalData(data);
        case "documents":
          return profileService.updateDocuments(data);
        case "preferences":
          return profileService.updatePreferences(data);
        case "security":
          return profileService.changePassword(data);
        default:
          throw new Error("Seção não encontrada");
      }
    },
    onSuccess: (_, { section }) => {
      setEditingSections((prev) => ({ ...prev, [section]: false }));
      setSnackbar({
        open: true,
        message: "Dados atualizados com sucesso!",
        severity: "success",
      });
      queryClient.invalidateQueries({ queryKey: ["profile"] });
    },
    onError: (error: any) => {
      setSnackbar({
        open: true,
        message: error.message || "Erro ao atualizar dados",
        severity: "error",
      });
    },
  });

  // Handlers

  const handleEditSection = (section: string) => {
    setEditingSections((prev) => ({ ...prev, [section]: true }));
  };

  const handleSaveSection = (section: string, data: any) => {
    updateSectionMutation.mutate({ section, data });
  };

  const handleCancelSection = (section: string) => {
    setEditingSections((prev) => ({ ...prev, [section]: false }));
  };

  const handleEditProfile = () => {
    setActiveTab("personal");
    handleEditSection("personal");
  };

  const handleChangePassword = () => {
    setActiveTab("security");
    handleEditSection("security");
  };

  const handleViewAudit = () => {
    setSnackbar({
      open: true,
      message: "Funcionalidade de auditoria será implementada no MVP1",
      severity: "info",
    });
  };

  const handleCloseSnackbar = () => {
    setSnackbar((prev) => ({ ...prev, open: false }));
  };

  // Renderizar seção ativa
  const renderActiveSection = () => {
    if (!profileData) return null;

    const commonProps = {
      isEditing: editingSections[activeTab] || false,
      onEdit: () => handleEditSection(activeTab),
      onSave: (data: any) => handleSaveSection(activeTab, data),
      onCancel: () => handleCancelSection(activeTab),
      isLoading: updateSectionMutation.isPending,
      errors: {},
    };

    switch (activeTab) {
      case "personal":
        return (
          <PersonalDataSection
            data={profileData.personalData}
            {...commonProps}
          />
        );
      case "contacts":
        return <ContactsSection data={profileData.contacts} {...commonProps} />;
      case "address":
        return (
          <AddressSection data={profileData.address} {...commonProps} />
        );
      case "health":
        return <HealthSection data={profileData.medical} {...commonProps} />;
      case "documents":
        return (
          <DocumentsSection data={profileData.documents} {...commonProps} />
        );
      case "preferences":
        return (
          <PreferencesSection data={profileData.preferences} {...commonProps} />
        );
      case "security":
        return <SecuritySection data={null} {...commonProps} />;
      case "audit":
        return (
          <Box sx={{ textAlign: "center", py: 4 }}>
            <Typography variant="h6" color="text.secondary">
              Funcionalidade de Auditoria
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
              Será implementada no MVP1
            </Typography>
          </Box>
        );
      default:
        return null;
    }
  };

  // Loading state
  if (isLoading) {
    return (
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          minHeight: "50vh",
        }}
      >
        <CircularProgress size={48} />
      </Box>
    );
  }

  // Error state
  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          Erro ao carregar dados do perfil. Tente novamente.
        </Alert>
        <Box sx={{ textAlign: "center" }}>
          <button onClick={() => refetch()}>Tentar Novamente</button>
        </Box>
      </Container>
    );
  }

  // No data state
  if (!profileData) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            minHeight: "50vh",
          }}
        >
          <CircularProgress size={48} />
        </Box>
      </Container>
    );
  }

  return (
    <Container
      maxWidth="lg"
      sx={{
        py: isMobile ? 2 : 4,
        px: isXs ? 1 : 2,
      }}
    >
      {/* Breadcrumb e Navegação */}
      <Box sx={{ mb: 3 }}>
        <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 2 }}>
          <Link
            component="button"
            variant="body2"
            onClick={handleBackToDashboard}
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 0.5,
              textDecoration: "none",
              fontSize: isXs ? "0.75rem" : "0.875rem",
              "&:hover": { textDecoration: "underline" },
            }}
          >
            <HomeIcon fontSize="small" />
            Dashboard
          </Link>
          <Typography
            variant="body2"
            color="text.primary"
            sx={{ fontSize: isXs ? "0.75rem" : "0.875rem" }}
          >
            Meu Perfil
          </Typography>
        </Breadcrumbs>

        {/* Botão de Voltar */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: isMobile ? 1 : 2,
            mb: 2,
          }}
        >
          <IconButton
            onClick={handleBackToDashboard}
            size={isMobile ? "small" : "medium"}
            sx={{
              bgcolor: "primary.main",
              color: "white",
              "&:hover": { bgcolor: "primary.dark" },
            }}
          >
            <ArrowBackIcon fontSize={isMobile ? "small" : "medium"} />
          </IconButton>
          <Typography
            variant={isMobile ? "h6" : "h5"}
            component="h1"
            sx={{
              fontWeight: "bold",
              fontSize: isXs ? "1.1rem" : isMobile ? "1.25rem" : "1.5rem",
            }}
          >
            Meu Perfil
          </Typography>
        </Box>

        {/* Pontos de Atenção */}
        {profileData && getIncompleteFields(profileData).length > 0 && (
          <Alert
            severity="warning"
            sx={{
              mb: 2,
              fontSize: isXs ? "0.75rem" : "0.875rem",
            }}
            action={
              <Button
                color="inherit"
                size={isMobile ? "small" : "medium"}
                onClick={() => setActiveTab("personal")}
                sx={{ fontSize: isXs ? "0.7rem" : "0.75rem" }}
              >
                {isMobile ? "Completar" : "Completar Dados"}
              </Button>
            }
          >
            <Typography
              variant="body2"
              sx={{
                fontWeight: "bold",
                mb: 1,
                fontSize: isXs ? "0.75rem" : "0.875rem",
              }}
            >
              ⚠️ Informações incompletas detectadas:
            </Typography>
            <Typography
              variant="body2"
              sx={{ fontSize: isXs ? "0.75rem" : "0.875rem" }}
            >
              {getIncompleteFields(profileData).join(", ")} - Complete estes
              campos para ativar todas as funcionalidades.
            </Typography>
          </Alert>
        )}
      </Box>

      {/* Header do Perfil */}
      <ProfileHeader
        user={profileData.personalData}
        onEditProfile={handleEditProfile}
        onChangePassword={handleChangePassword}
        onViewAudit={handleViewAudit}
      />

      {/* Navegação por Tabs */}
      <ProfileTabsWithModal
        activeTab={activeTab}
        onTabChange={handleTabChange}
        isMobile={isMobile}
        pendencies={pendencies}
        profileData={profileData}
      />

      {/* Conteúdo da Seção Ativa */}
      <Box
        sx={{
          mt: 3,
          minHeight: isMobile ? "50vh" : "auto",
        }}
      >
        <Box
          id={activeTab}
          sx={{
            scrollMarginTop: isMobile ? "80px" : "0px", // Espaço para scroll suave
          }}
        >
          {renderActiveSection()}
        </Box>
      </Box>

      {/* Snackbar para feedback */}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        message={snackbar.message}
      />
    </Container>
  );
};

export default ProfilePage;
