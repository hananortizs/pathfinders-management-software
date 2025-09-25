import React, { useEffect, useRef } from "react";
import {
  Box,
  Tabs,
  Tab,
  Typography,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import {
  Person as PersonIcon,
  ContactPhone as ContactIcon,
  Home as HomeIcon,
  HealthAndSafety as HealthIcon,
  Description as DocumentIcon,
  Settings as PreferencesIcon,
  Security as SecurityIcon,
  History as AuditIcon,
} from "@mui/icons-material";
import type { ProfileTabsProps } from "../../types/profile";
import { PendencyIndicator } from "./PendencyIndicator";
import { HorizontalCarousel } from "../common/HorizontalCarousel";

/**
 * Componente de navegação por tabs/acordeão para seções do perfil
 * Responsivo: tabs em desktop, acordeão em mobile
 */
export const ProfileTabs: React.FC<ProfileTabsProps> = ({
  activeTab,
  onTabChange,
  isMobile,
  pendencies,
}) => {
  const theme = useTheme();
  const isXs = useMediaQuery(theme.breakpoints.down("sm"));
  const activeSectionRef = useRef<HTMLDivElement>(null);


  // Scroll suave para a seção ativa em mobile
  useEffect(() => {
    if (isMobile && activeSectionRef.current) {
      activeSectionRef.current.scrollIntoView({
        behavior: "smooth",
        block: "start",
      });
    }
  }, [activeTab, isMobile]);

  const sections = [
    {
      id: "personal",
      label: "Dados Pessoais",
      icon: <PersonIcon />,
      description: "Informações básicas e identificação",
      pendencyCount: pendencies?.personalData || 0,
    },
    {
      id: "contacts",
      label: "Contatos",
      icon: <ContactIcon />,
      description: "E-mail, telefone e outros contatos",
      pendencyCount: pendencies?.contacts || 0,
    },
    {
      id: "address",
      label: "Endereço",
      icon: <HomeIcon />,
      description: "Endereço residencial e comercial",
      pendencyCount: pendencies?.address || 0,
    },
    {
      id: "health",
      label: "Saúde",
      icon: <HealthIcon />,
      description: "Dados médicos e contato de emergência",
      pendencyCount: pendencies?.health || 0,
    },
    {
      id: "documents",
      label: "Documentos",
      icon: <DocumentIcon />,
      description: "CPF, RG e outros documentos",
      pendencyCount: pendencies?.documents || 0,
    },
    {
      id: "preferences",
      label: "Preferências",
      icon: <PreferencesIcon />,
      description: "Idioma, tema e notificações",
      pendencyCount: 0, // Preferências não têm pendências obrigatórias
    },
    {
      id: "security",
      label: "Segurança",
      icon: <SecurityIcon />,
      description: "Senha e configurações de segurança",
      pendencyCount: 0, // Segurança não tem pendências obrigatórias
    },
    {
      id: "audit",
      label: "Auditoria",
      icon: <AuditIcon />,
      description: "Histórico de atividades e acessos",
      pendencyCount: 0, // Auditoria não tem pendências obrigatórias
    },
  ];

  if (isMobile || isXs) {
    // Renderizar como carrossel horizontal em mobile
    const carouselItems = sections.map((section) => ({
      id: section.id,
      label: section.label,
      description: section.description,
      icon: section.icon,
      pendencyCount: section.pendencyCount,
    }));

    return (
      <Box sx={{ mb: 3 }}>
        <HorizontalCarousel
          items={carouselItems}
          activeItem={activeTab}
          onItemChange={onTabChange}
          itemWidth={isXs ? 260 : 280}
          showArrows={true}
          showDots={true}
          onPendencyClick={() => {}}
        />
      </Box>
    );
  }

  // Renderizar como tabs em desktop
  return (
    <Box sx={{ borderBottom: 1, borderColor: "divider", mb: 3 }}>
      <Tabs
        value={activeTab}
        onChange={(_, value) => onTabChange(value)}
        variant="scrollable"
        scrollButtons="auto"
        sx={{
          "& .MuiTab-root": {
            minHeight: 64,
            textTransform: "none",
            fontSize: "0.875rem",
            fontWeight: 500,
            "&.Mui-selected": {
              color: theme.palette.primary.main,
              backgroundColor: theme.palette.primary.light + "10",
            },
          },
          "& .MuiTabs-indicator": {
            backgroundColor: theme.palette.primary.main,
            height: 3,
          },
        }}
      >
        {sections.map((section) => {
          const isActive = activeTab === section.id;
          return (
            <Tab
              key={section.id}
              value={section.id}
              label={
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 1,
                    width: "100%",
                  }}
                >
                  <Box
                    sx={{
                      color: isActive ? theme.palette.primary.main : "inherit",
                      display: "flex",
                      alignItems: "center",
                    }}
                  >
                    {section.icon}
                  </Box>
                  <Box sx={{ textAlign: "left", flex: 1 }}>
                    <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                      <Typography
                        variant="body2"
                        sx={{
                          fontWeight: "inherit",
                          color: isActive
                            ? theme.palette.primary.main
                            : "inherit",
                        }}
                      >
                        {section.label}
                      </Typography>
                      <PendencyIndicator
                        count={section.pendencyCount}
                        size="small"
                        showSuccess={true}
                      />
                    </Box>
                    <Typography
                      variant="caption"
                      color={isActive ? "primary" : "text.secondary"}
                      sx={{ display: "block", lineHeight: 1.2 }}
                    >
                      {section.description}
                    </Typography>
                  </Box>
                </Box>
              }
              sx={{
                minWidth: 200,
                alignItems: "flex-start",
                py: 2,
                ...(isActive && {
                  backgroundColor: theme.palette.primary.light + "10",
                }),
              }}
            />
          );
        })}
      </Tabs>
    </Box>
  );
};

// Adicionar o modal de pendências no final do componente
export const ProfileTabsWithModal: React.FC<
  ProfileTabsProps & { profileData?: any }
> = (props) => {


  return (
    <>
      <ProfileTabs {...props} />
    </>
  );
};
