import React from "react";
import {
  Box,
  Grid,
  Card,
  CardContent,
  Typography,
  LinearProgress,
  Chip,
  Tooltip,
  IconButton,
} from "@mui/material";
import {
  People as PeopleIcon,
  PersonAdd as PersonAddIcon,
  TrendingUp as TrendingUpIcon,
  Info as InfoIcon,
} from "@mui/icons-material";

interface DashboardKPIsProps {
  data?: {
    totalMembers: number;
    activeMembers: number;
    newMembers30Days: number;
    clubName?: string;
    unitName?: string;
  };
  isLoading: boolean;
  userRole?: string;
}

/**
 * Componente para exibir KPIs da dashboard
 * Mostra métricas agregadas baseadas no papel do usuário
 */
export const DashboardKPIs: React.FC<DashboardKPIsProps> = ({
  data,
  isLoading,
  userRole,
}) => {
  if (isLoading) {
    return (
      <Box sx={{ mb: 3 }}>
        <LinearProgress />
        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          Carregando métricas...
        </Typography>
      </Box>
    );
  }

  if (!data) {
    return (
      <Box sx={{ textAlign: "center", py: 4 }}>
        <Typography variant="body1" color="text.secondary">
          Nenhum dado disponível
        </Typography>
      </Box>
    );
  }

  const kpis = [
    {
      title: "Total de Membros",
      value: data.totalMembers || 0,
      icon: <PeopleIcon sx={{ fontSize: 24 }} />,
      color: "primary.main",
      description: `No clube ${data.clubName || "atual"}`,
    },
    {
      title: "Membros Ativos",
      value: data.activeMembers || 0,
      icon: <TrendingUpIcon sx={{ fontSize: 24 }} />,
      color: "success.main",
      description: "Com status ativo",
    },
    {
      title: "Novos (30 dias)",
      value: data.newMembers30Days || 0,
      icon: <PersonAddIcon sx={{ fontSize: 24 }} />,
      color: "info.main",
      description: "Cadastrados recentemente",
    },
  ];

  return (
    <Grid container spacing={3}>
      {kpis.map((kpi, index) => (
        <Grid size={{ xs: 12, sm: 6, md: 4 }} key={index}>
          <Card
            sx={{
              height: "100%",
              position: "relative",
              "&:hover": {
                boxShadow: 3,
                transform: "translateY(-2px)",
                transition: "all 0.2s ease-in-out",
              },
            }}
          >
            <CardContent>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "space-between",
                  mb: 2,
                }}
              >
                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  <Box sx={{ color: kpi.color }}>{kpi.icon}</Box>
                  <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                    {kpi.title}
                  </Typography>
                </Box>
                <Tooltip title={kpi.description}>
                  <IconButton size="small">
                    <InfoIcon sx={{ fontSize: 16 }} />
                  </IconButton>
                </Tooltip>
              </Box>

              <Typography
                variant="h3"
                sx={{ fontWeight: "bold", color: kpi.color, mb: 1 }}
              >
                {kpi.value.toLocaleString("pt-BR")}
              </Typography>

              <Typography variant="body2" color="text.secondary">
                {kpi.description}
              </Typography>

              {/* Barra de progresso visual (opcional) */}
              {kpi.title === "Membros Ativos" && data.totalMembers > 0 && (
                <Box sx={{ mt: 2 }}>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      mb: 1,
                    }}
                  >
                    <Typography variant="caption" color="text.secondary">
                      Taxa de ativação
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {Math.round(
                        (data.activeMembers / data.totalMembers) * 100
                      )}
                      %
                    </Typography>
                  </Box>
                  <LinearProgress
                    variant="determinate"
                    value={(data.activeMembers / data.totalMembers) * 100}
                    sx={{
                      height: 6,
                      borderRadius: 3,
                      backgroundColor: "grey.200",
                      "& .MuiLinearProgress-bar": {
                        backgroundColor: kpi.color,
                      },
                    }}
                  />
                </Box>
              )}
            </CardContent>
          </Card>
        </Grid>
      ))}

      {/* Card de contexto do usuário */}
      <Grid size={{ xs: 12, sm: 6, md: 4 }}>
        <Card sx={{ height: "100%", backgroundColor: "grey.50" }}>
          <CardContent>
            <Box sx={{ display: "flex", alignItems: "center", gap: 1, mb: 2 }}>
              <PeopleIcon sx={{ fontSize: 24, color: "text.secondary" }} />
              <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                Meu Contexto
              </Typography>
            </Box>

            <Box sx={{ mb: 2 }}>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Clube:
              </Typography>
              <Chip
                label={data.clubName || "Não definido"}
                color="primary"
                variant="outlined"
                size="small"
              />
            </Box>

            {data.unitName && (
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Unidade:
                </Typography>
                <Chip
                  label={data.unitName}
                  color="secondary"
                  variant="outlined"
                  size="small"
                />
              </Box>
            )}

            <Box sx={{ mt: 2 }}>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Papel:
              </Typography>
              <Chip
                label={userRole || "Membro"}
                color="default"
                variant="filled"
                size="small"
              />
            </Box>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
};
