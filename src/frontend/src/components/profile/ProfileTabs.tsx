import React from "react";
import {
  Box,
  Tabs,
  Tab,
  Accordion,
  AccordionSummary,
  AccordionDetails,
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
  ExpandMore as ExpandMoreIcon,
} from "@mui/icons-material";
import type { ProfileTabsProps } from "../../types/profile";
import { PendencyIndicator } from "./PendencyIndicator";

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
    // Renderizar como acordeão em mobile
    return (
      <Box sx={{ mb: 3 }}>
        {sections.map((section) => (
          <Accordion
            key={section.id}
            expanded={activeTab === section.id}
            onChange={() => onTabChange(section.id)}
            sx={{
              "&:before": {
                display: "none",
              },
              "&.Mui-expanded": {
                margin: 0,
              },
              mb: 1,
            }}
          >
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              sx={{
                minHeight: 56,
                "&.Mui-expanded": {
                  minHeight: 56,
                },
                "& .MuiAccordionSummary-content": {
                  margin: "12px 0",
                  "&.Mui-expanded": {
                    margin: "12px 0",
                  },
                },
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: 2,
                  width: "100%",
                }}
              >
                {section.icon}
                <Box sx={{ flex: 1 }}>
                  <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
                      {section.label}
                    </Typography>
                    <PendencyIndicator count={section.pendencyCount} />
                  </Box>
                  <Typography variant="caption" color="text.secondary">
                    {section.description}
                  </Typography>
                </Box>
              </Box>
            </AccordionSummary>
            <AccordionDetails sx={{ pt: 0 }}>
              {/* Conteúdo da seção será renderizado aqui */}
            </AccordionDetails>
          </Accordion>
        ))}
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
          },
        }}
      >
        {sections.map((section) => (
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
                {section.icon}
                <Box sx={{ textAlign: "left", flex: 1 }}>
                  <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                    <Typography variant="body2" sx={{ fontWeight: "inherit" }}>
                      {section.label}
                    </Typography>
                    <PendencyIndicator
                      count={section.pendencyCount}
                      size="small"
                    />
                  </Box>
                  <Typography
                    variant="caption"
                    color="text.secondary"
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
            }}
          />
        ))}
      </Tabs>
    </Box>
  );
};
