import React from "react";
import {
  Box,
  Grid,
  Card,
  CardContent,
  CardHeader,
  Typography,
  LinearProgress,
  Chip,
  Button,
  List,
  ListItem,
  ListItemIcon,
  Divider,
  Alert,
  Badge,
} from "@mui/material";
import {
  Event as EventIcon,
  School as SchoolIcon,
  CheckCircle as CheckCircleIcon,
  Schedule as ScheduleIcon,
  Warning as WarningIcon,
  ArrowForward as ArrowForwardIcon,
} from "@mui/icons-material";

interface DashboardSectionsProps {
  data?: {
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
  };
  isLoading: boolean;
  userRole?: string;
  userId?: string;
}

/**
 * Componente para exibir seções específicas da dashboard baseadas no papel do usuário
 * Para membros: eventos próximos, progresso em classes e especialidades
 */
export const DashboardSections: React.FC<DashboardSectionsProps> = ({
  data,
  isLoading,
}) => {
  if (isLoading) {
    return (
      <Box sx={{ mb: 3 }}>
        <LinearProgress />
        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          Carregando seções...
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

  const getEligibilityBadge = (isEligible: boolean, status: string) => {
    if (status === "registered") {
      return <Chip label="Inscrito" color="success" size="small" />;
    }
    if (status === "waitlist") {
      return <Chip label="Lista de Espera" color="warning" size="small" />;
    }
    if (status === "closed") {
      return <Chip label="Fechado" color="error" size="small" />;
    }
    if (isEligible) {
      return <Chip label="Elegível" color="primary" size="small" />;
    }
    return <Chip label="Não Elegível" color="default" size="small" />;
  };

  const getProgressColor = (status: string) => {
    switch (status) {
      case "completed":
        return "success";
      case "in_progress":
        return "primary";
      case "not_started":
        return "default";
      default:
        return "default";
    }
  };

  const getProgressIcon = (status: string) => {
    switch (status) {
      case "completed":
        return <CheckCircleIcon sx={{ fontSize: 16 }} />;
      case "in_progress":
        return <ScheduleIcon sx={{ fontSize: 16 }} />;
      case "not_started":
        return <WarningIcon sx={{ fontSize: 16 }} />;
      default:
        return <SchoolIcon sx={{ fontSize: 16 }} />;
    }
  };

  return (
    <Grid container spacing={3}>
      {/* Próximos Eventos */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Card sx={{ height: "100%" }}>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <EventIcon sx={{ color: "primary.main" }} />
                <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                  Próximos Eventos
                </Typography>
                <Badge
                  badgeContent={data.upcomingEvents?.length || 0}
                  color="primary"
                >
                  <Box />
                </Badge>
              </Box>
            }
            action={
              <Button size="small" endIcon={<ArrowForwardIcon />}>
                Ver Todos
              </Button>
            }
          />
          <CardContent>
            {!data.upcomingEvents || data.upcomingEvents.length === 0 ? (
              <Alert severity="info" sx={{ mb: 2 }}>
                Nenhum evento próximo encontrado
              </Alert>
            ) : (
              <List sx={{ p: 0 }}>
                {data.upcomingEvents.slice(0, 3).map((event, index) => (
                  <React.Fragment key={event.id}>
                    <ListItem sx={{ px: 0 }}>
                      <ListItemIcon>
                        <EventIcon sx={{ color: "text.secondary" }} />
                      </ListItemIcon>
                      <Box sx={{ flex: 1, minWidth: 0 }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            mb: 0.5,
                          }}
                        >
                          <Typography
                            variant="subtitle2"
                            sx={{ fontWeight: "bold" }}
                          >
                            {event.title}
                          </Typography>
                          {getEligibilityBadge(
                            event.isEligible,
                            event.registrationStatus
                          )}
                        </Box>
                        <Typography variant="body2" color="text.secondary">
                          {event.date} • {event.location}
                        </Typography>
                      </Box>
                    </ListItem>
                    {index < data.upcomingEvents.slice(0, 3).length - 1 && (
                      <Divider />
                    )}
                  </React.Fragment>
                ))}
              </List>
            )}
          </CardContent>
        </Card>
      </Grid>

      {/* Progresso em Classes */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Card sx={{ height: "100%" }}>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <SchoolIcon sx={{ color: "secondary.main" }} />
                <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                  Minhas Classes
                </Typography>
                <Badge
                  badgeContent={data.classProgress?.length || 0}
                  color="secondary"
                >
                  <Box />
                </Badge>
              </Box>
            }
            action={
              <Button size="small" endIcon={<ArrowForwardIcon />}>
                Ver Todas
              </Button>
            }
          />
          <CardContent>
            {!data.classProgress || data.classProgress.length === 0 ? (
              <Alert severity="info" sx={{ mb: 2 }}>
                Nenhuma classe em andamento
              </Alert>
            ) : (
              <List sx={{ p: 0 }}>
                {data.classProgress.slice(0, 3).map((classItem, index) => (
                  <React.Fragment key={classItem.id}>
                    <ListItem sx={{ px: 0 }}>
                      <ListItemIcon>
                        {getProgressIcon(classItem.status)}
                      </ListItemIcon>
                      <Box sx={{ flex: 1, minWidth: 0 }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            mb: 0.5,
                          }}
                        >
                          <Typography
                            variant="subtitle2"
                            sx={{ fontWeight: "bold" }}
                          >
                            {classItem.name}
                          </Typography>
                          <Chip
                            label={
                              classItem.status === "in_progress"
                                ? "Em Andamento"
                                : classItem.status === "completed"
                                ? "Concluída"
                                : "Não Iniciada"
                            }
                            color={getProgressColor(classItem.status) as any}
                            size="small"
                          />
                        </Box>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            mb: 1,
                          }}
                        >
                          <Typography variant="body2" color="text.secondary">
                            Progresso: {classItem.progress}%
                          </Typography>
                        </Box>
                        <LinearProgress
                          variant="determinate"
                          value={classItem.progress}
                          sx={{ height: 4, borderRadius: 2 }}
                        />
                        {classItem.nextStep && (
                          <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mt: 0.5, display: "block" }}
                          >
                            Próximo: {classItem.nextStep}
                          </Typography>
                        )}
                      </Box>
                    </ListItem>
                    {index < data.classProgress.slice(0, 3).length - 1 && (
                      <Divider />
                    )}
                  </React.Fragment>
                ))}
              </List>
            )}
          </CardContent>
        </Card>
      </Grid>

      {/* Especialidades */}
      <Grid size={{ xs: 12 }}>
        <Card>
          <CardHeader
            title={
              <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                <SchoolIcon sx={{ color: "success.main" }} />
                <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                  Minhas Especialidades
                </Typography>
                <Badge
                  badgeContent={data.specializations?.length || 0}
                  color="success"
                >
                  <Box />
                </Badge>
              </Box>
            }
            action={
              <Button size="small" endIcon={<ArrowForwardIcon />}>
                Gerenciar
              </Button>
            }
          />
          <CardContent>
            {!data.specializations || data.specializations.length === 0 ? (
              <Alert severity="info">Nenhuma especialidade em andamento</Alert>
            ) : (
              <Grid container spacing={2}>
                {data.specializations.slice(0, 6).map((specialization) => (
                  <Grid size={{ xs: 12, sm: 6, md: 4 }} key={specialization.id}>
                    <Card variant="outlined" sx={{ height: "100%" }}>
                      <CardContent>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            mb: 1,
                          }}
                        >
                          {getProgressIcon(specialization.status)}
                          <Typography
                            variant="subtitle2"
                            sx={{ fontWeight: "bold" }}
                          >
                            {specialization.name}
                          </Typography>
                        </Box>
                        <Typography
                          variant="caption"
                          color="text.secondary"
                          sx={{ mb: 1, display: "block" }}
                        >
                          {specialization.category}
                        </Typography>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            mb: 1,
                          }}
                        >
                          <Typography variant="body2" color="text.secondary">
                            {specialization.progress}%
                          </Typography>
                          <Chip
                            label={
                              specialization.status === "in_progress"
                                ? "Em Andamento"
                                : specialization.status === "completed"
                                ? "Concluída"
                                : "Não Iniciada"
                            }
                            color={
                              getProgressColor(specialization.status) as any
                            }
                            size="small"
                          />
                        </Box>
                        <LinearProgress
                          variant="determinate"
                          value={specialization.progress}
                          sx={{ height: 4, borderRadius: 2 }}
                        />
                      </CardContent>
                    </Card>
                  </Grid>
                ))}
              </Grid>
            )}
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
};
