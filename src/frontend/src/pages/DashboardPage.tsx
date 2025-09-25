import React from "react";
import {
  Box,
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  LinearProgress,
  Alert,
  Chip,
  Button,
  IconButton,
  Tooltip,
} from "@mui/material";
import {
  Dashboard as DashboardIcon,
  Event as EventIcon,
  School as SchoolIcon,
  Person as PersonIcon,
  Refresh as RefreshIcon,
  Settings as SettingsIcon,
} from "@mui/icons-material";
import { useQuery } from "@tanstack/react-query";
import { useAuth } from "../hooks/useAuth";
import { DashboardKPIs } from "../components/dashboard/DashboardKPIs";
import { DashboardSections } from "../components/dashboard/DashboardSections";
import { Navbar } from "../components/layout/Navbar";
import { Sidebar } from "../components/layout/Sidebar";
import { dashboardService } from "../services/dashboardService";

/**
 * Página principal da Dashboard
 * Exibe KPIs e seções específicas baseadas no papel do usuário
 */
const DashboardPage: React.FC = () => {
  const { user } = useAuth();

  // Buscar dados da dashboard
  const { isLoading, error, refetch } = useQuery({
    queryKey: ["dashboard", user?.id],
    queryFn: () => dashboardService.getDashboard(),
    staleTime: 5 * 60 * 1000, // 5 minutos
    gcTime: 10 * 60 * 1000, // 10 minutos
  });

  // Buscar KPIs específicos
  const { data: kpisData, isLoading: kpisLoading } = useQuery({
    queryKey: ["dashboard-kpis", user?.id],
    queryFn: () => dashboardService.getKPIs(),
    staleTime: 5 * 60 * 1000,
    gcTime: 10 * 60 * 1000,
  });

  // Buscar seções específicas por papel
  const { data: sectionsData, isLoading: sectionsLoading } = useQuery({
    queryKey: ["dashboard-sections", user?.id],
    queryFn: () => dashboardService.getSections(),
    staleTime: 1 * 60 * 1000, // 1 minuto
    gcTime: 5 * 60 * 1000,
  });

  const handleRefresh = () => {
    refetch();
  };

  if (error) {
    return (
      <Box
        sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}
      >
        <Navbar />
        <Box sx={{ display: "flex", flex: 1 }}>
          <Sidebar />
          <Box sx={{ flex: 1, p: 3 }}>
            <Alert
              severity="error"
              action={
                <Button color="inherit" size="small" onClick={handleRefresh}>
                  Tentar Novamente
                </Button>
              }
            >
              Erro ao carregar dashboard. Tente novamente.
            </Alert>
          </Box>
        </Box>
      </Box>
    );
  }

  return (
    <Box sx={{ display: "flex", flexDirection: "column", minHeight: "100vh" }}>
      <Navbar />
      <Box sx={{ display: "flex", flex: 1 }}>
        <Sidebar />
        <Box sx={{ flex: 1, p: 3 }}>
          <Container maxWidth="xl">
            {/* Header da Dashboard */}
            <Box sx={{ mb: 4 }}>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  mb: 2,
                }}
              >
                <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                  <DashboardIcon sx={{ fontSize: 32, color: "primary.main" }} />
                  <Typography
                    variant="h4"
                    component="h1"
                    sx={{ fontWeight: "bold" }}
                  >
                    Dashboard
                  </Typography>
                  <Chip
                    label={user?.roles?.[0] || "Membro"}
                    color="primary"
                    variant="outlined"
                    size="small"
                  />
                </Box>
                <Box sx={{ display: "flex", gap: 1 }}>
                  <Tooltip title="Atualizar dados">
                    <span>
                      <IconButton onClick={handleRefresh} disabled={isLoading}>
                        <RefreshIcon />
                      </IconButton>
                    </span>
                  </Tooltip>
                  <Tooltip title="Configurações">
                    <IconButton>
                      <SettingsIcon />
                    </IconButton>
                  </Tooltip>
                </Box>
              </Box>

              <Typography variant="body1" color="text.secondary">
                Bem-vindo,{" "}
                {user?.fullName ||
                  user?.firstName + " " + user?.lastName ||
                  "Usuário"}
                ! Aqui você pode acompanhar suas atividades e progresso.
              </Typography>
            </Box>

            {/* Loading State */}
            {isLoading && (
              <Box sx={{ mb: 3 }}>
                <LinearProgress />
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mt: 1 }}
                >
                  Carregando dashboard...
                </Typography>
              </Box>
            )}

            {/* KPIs Section */}
            <Box sx={{ mb: 4 }}>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: "bold" }}>
                Visão Geral do Clube
              </Typography>
              <DashboardKPIs
                data={kpisData as any}
                isLoading={kpisLoading}
                userRole={user?.roles?.[0]}
              />
            </Box>

            {/* Sections específicas por papel */}
            <Box sx={{ mb: 4 }}>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: "bold" }}>
                Minhas Atividades
              </Typography>
              <DashboardSections
                data={sectionsData as any}
                isLoading={sectionsLoading}
                userRole={user?.roles?.[0]}
                userId={user?.id}
              />
            </Box>

            {/* Quick Actions */}
            <Box sx={{ mb: 4 }}>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: "bold" }}>
                Ações Rápidas
              </Typography>
              <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                  <Card
                    sx={{
                      height: "100%",
                      cursor: "pointer",
                      "&:hover": { boxShadow: 3 },
                    }}
                  >
                    <CardContent sx={{ textAlign: "center", p: 2 }}>
                      <EventIcon
                        sx={{ fontSize: 40, color: "primary.main", mb: 1 }}
                      />
                      <Typography variant="h6" sx={{ mb: 1 }}>
                        Meus Eventos
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Ver eventos próximos e inscrições
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                  <Card
                    sx={{
                      height: "100%",
                      cursor: "pointer",
                      "&:hover": { boxShadow: 3 },
                    }}
                  >
                    <CardContent sx={{ textAlign: "center", p: 2 }}>
                      <SchoolIcon
                        sx={{ fontSize: 40, color: "secondary.main", mb: 1 }}
                      />
                      <Typography variant="h6" sx={{ mb: 1 }}>
                        Minhas Classes
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Acompanhar progresso nas classes
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                  <Card
                    sx={{
                      height: "100%",
                      cursor: "pointer",
                      "&:hover": { boxShadow: 3 },
                    }}
                  >
                    <CardContent sx={{ textAlign: "center", p: 2 }}>
                      <SchoolIcon
                        sx={{ fontSize: 40, color: "success.main", mb: 1 }}
                      />
                      <Typography variant="h6" sx={{ mb: 1 }}>
                        Especialidades
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Gerenciar especialidades
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                  <Card
                    sx={{
                      height: "100%",
                      cursor: "pointer",
                      "&:hover": { boxShadow: 3 },
                    }}
                  >
                    <CardContent sx={{ textAlign: "center", p: 2 }}>
                      <PersonIcon
                        sx={{ fontSize: 40, color: "info.main", mb: 1 }}
                      />
                      <Typography variant="h6" sx={{ mb: 1 }}>
                        Meu Perfil
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Editar dados pessoais
                      </Typography>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Box>
          </Container>
        </Box>
      </Box>
    </Box>
  );
};

export default DashboardPage;
