/**
 * Página de Dashboard
 *
 * Página principal do sistema com visão geral das métricas
 * e ações rápidas disponíveis
 */

import React, { memo, useMemo } from "react";
import {
  Typography,
  Box,
  CardContent,
  CardActions,
  Button,
  Chip,
  Avatar,
  List,
  ListItem,
  ListItemAvatar,
  Divider,
} from "@mui/material";
import {
  People,
  Event,
  TrendingUp,
  School,
  LocationOn,
  CalendarToday,
} from "@mui/icons-material";
import { useAuth } from "../hooks/useAuth";
import { AppCard } from "../components/styled/cards";
import StatsCard from "../components/dashboard/StatsCard";

// Componente memoizado para atividades recentes
const RecentActivitiesList = memo(({ activities }: { activities: any[] }) => (
  <List sx={{ py: 0 }}>
    {activities.map((activity, index) => (
      <React.Fragment key={activity.id}>
        <ListItem
          alignItems="flex-start"
          sx={{
            py: { xs: 0.5, sm: 0.75 },
            px: 0,
          }}
        >
          <ListItemAvatar sx={{ minWidth: { xs: 28, sm: 32 } }}>
            <Avatar
              sx={{
                width: { xs: 24, sm: 28 },
                height: { xs: 24, sm: 28 },
                bgcolor: `${getActivityColor(activity.type)}.main`,
              }}
            >
              {getActivityIcon(activity.type)}
            </Avatar>
          </ListItemAvatar>
          <Box component="div" sx={{ flex: 1 }}>
            <Typography
              variant="body2"
              component="div"
              sx={{
                fontSize: {
                  xs: "0.7rem",
                  sm: "0.75rem",
                  md: "0.8rem",
                },
                lineHeight: 1.2,
                mb: 0.5,
              }}
            >
              {activity.title}
            </Typography>
            <Typography
              variant="caption"
              component="div"
              sx={{
                fontSize: { xs: "0.6rem", sm: "0.65rem" },
              }}
            >
              {activity.time}
            </Typography>
          </Box>
        </ListItem>
        {index < activities.length - 1 && (
          <Divider variant="inset" component="li" sx={{ ml: 5 }} />
        )}
      </React.Fragment>
    ))}
  </List>
));

// Componente memoizado para eventos próximos
const UpcomingEventsList = memo(({ events }: { events: any[] }) => (
  <List sx={{ py: 0 }}>
    {events.map((event, index) => (
      <React.Fragment key={event.id}>
        <ListItem
          alignItems="flex-start"
          sx={{
            py: { xs: 0.5, sm: 0.75 },
            px: 0,
          }}
        >
          <ListItemAvatar sx={{ minWidth: { xs: 28, sm: 32 } }}>
            <Avatar
              sx={{
                width: { xs: 24, sm: 28 },
                height: { xs: 24, sm: 28 },
                bgcolor: "primary.main",
              }}
            >
              <CalendarToday fontSize="small" />
            </Avatar>
          </ListItemAvatar>
          <Box component="div" sx={{ flex: 1 }}>
            <Typography
              variant="body2"
              component="div"
              sx={{
                fontSize: {
                  xs: "0.7rem",
                  sm: "0.75rem",
                  md: "0.8rem",
                },
                lineHeight: 1.2,
                mb: 0.5,
              }}
            >
              {event.title}
            </Typography>
            <Typography
              variant="caption"
              color="text.secondary"
              component="div"
              sx={{
                fontSize: { xs: "0.6rem", sm: "0.65rem" },
                display: "block",
                mb: 0.25,
              }}
            >
              {event.date}
            </Typography>
            <Box
              component="div"
              sx={{
                display: "flex",
                alignItems: "center",
                gap: 0.25,
                flexWrap: "wrap",
              }}
            >
              <LocationOn fontSize="small" color="action" />
              <Typography
                variant="caption"
                color="text.secondary"
                component="span"
                sx={{
                  fontSize: { xs: "0.55rem", sm: "0.6rem" },
                }}
              >
                {event.location}
              </Typography>
              <Chip
                label={`${event.participants} participantes`}
                size="small"
                color="primary"
                variant="outlined"
                sx={{
                  fontSize: { xs: "0.5rem", sm: "0.55rem" },
                  height: { xs: 16, sm: 18 },
                }}
              />
            </Box>
          </Box>
        </ListItem>
        {index < events.length - 1 && (
          <Divider variant="inset" component="li" sx={{ ml: 5 }} />
        )}
      </React.Fragment>
    ))}
  </List>
));

// Funções auxiliares memoizadas
const getActivityIcon = (type: string) => {
  switch (type) {
    case "achievement":
      return <School color="success" />;
    case "registration":
      return <Event color="primary" />;
    case "meeting":
      return <People color="info" />;
    case "promotion":
      return <TrendingUp color="warning" />;
    default:
      return <Event />;
  }
};

const getActivityColor = (type: string) => {
  switch (type) {
    case "achievement":
      return "success";
    case "registration":
      return "primary";
    case "meeting":
      return "info";
    case "promotion":
      return "warning";
    default:
      return "default";
  }
};

const DashboardPage: React.FC = () => {
  const { user } = useAuth();

  // Dados mockados para demonstração - memoizados para performance
  const stats = useMemo(
    () => [
      {
        title: "Total de Membros",
        value: "1,234",
        change: "+12%",
        changeType: "positive" as const,
        icon: <People />,
        color: "primary" as const,
      },
      {
        title: "Eventos Ativos",
        value: "8",
        change: "+2",
        changeType: "positive" as const,
        icon: <Event />,
        color: "secondary" as const,
      },
      {
        title: "Especialidades Conquistadas",
        value: "156",
        change: "+23",
        changeType: "positive" as const,
        icon: <School />,
        color: "success" as const,
      },
      {
        title: "Taxa de Presença",
        value: "87%",
        change: "+5%",
        changeType: "positive" as const,
        icon: <TrendingUp />,
        color: "warning" as const,
      },
    ],
    []
  );

  const recentActivities = useMemo(
    () => [
      {
        id: "1",
        title: "João Silva completou especialidade de Culinária",
        time: "2 horas atrás",
        type: "achievement",
      },
      {
        id: "2",
        title: "Maria Santos se inscreveu no evento de acampamento",
        time: "4 horas atrás",
        type: "registration",
      },
      {
        id: "3",
        title: "Clube Central realizou reunião mensal",
        time: "1 dia atrás",
        type: "meeting",
      },
      {
        id: "4",
        title: "Pedro Costa foi promovido para Companheiro",
        time: "2 dias atrás",
        type: "promotion",
      },
    ],
    []
  );

  const upcomingEvents = useMemo(
    () => [
      {
        id: "1",
        title: "Acampamento de Verão",
        date: "15-17 Setembro",
        location: "Parque Nacional",
        participants: 45,
      },
      {
        id: "2",
        title: "Reunião de Liderança",
        date: "20 Setembro",
        location: "Igreja Central",
        participants: 12,
      },
      {
        id: "3",
        title: "Evento de Especialidades",
        date: "25 Setembro",
        location: "Centro Comunitário",
        participants: 78,
      },
    ],
    []
  );

  return (
    <Box
      sx={{
        width: "100%",
        minHeight: "100vh",
        bgcolor: "grey.50",
        p: { xs: 1, sm: 2, md: 3 },
      }}
    >
      {/* Header */}
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          mb: { xs: 1, sm: 1.5 },
          flexWrap: "wrap",
        }}
      >
        <Typography
          variant="h4"
          component="h1"
          sx={{
            fontSize: { xs: "1.5rem", sm: "2rem", md: "2.5rem" },
            fontWeight: 600,
            color: "primary.main",
            mr: 2,
          }}
        >
          Dashboard
        </Typography>
        <Typography
          variant="body1"
          color="text.secondary"
          sx={{
            fontSize: { xs: "0.8rem", sm: "0.9rem" },
          }}
        >
          Bem-vindo, {user?.firstName || "Usuário"}!
        </Typography>
      </Box>

      {/* Stats Cards */}
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            sm: "repeat(2, 1fr)",
            md: "repeat(4, 1fr)",
          },
          gap: { xs: 1, sm: 1.5, md: 2 },
          mb: { xs: 2, sm: 3 },
        }}
      >
        {stats.map((stat, index) => (
          <StatsCard key={index} {...stat} />
        ))}
      </Box>

      {/* Content Grid */}
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            md: "1fr 1fr",
          },
          gap: { xs: 2, sm: 2.5, md: 3 },
        }}
      >
        {/* Atividades Recentes */}
        <AppCard>
          <CardContent sx={{ p: { xs: 1.5, sm: 2, md: 2.5 } }}>
            <Typography
              variant="h6"
              component="h2"
              gutterBottom
              sx={{
                fontSize: { xs: "0.9rem", sm: "1rem", md: "1.1rem" },
                mb: { xs: 1, sm: 1.5 },
              }}
            >
              Atividades Recentes
            </Typography>
            <RecentActivitiesList activities={recentActivities} />
          </CardContent>
          <CardActions>
            <Button size="small" color="primary">
              Ver Todas
            </Button>
          </CardActions>
        </AppCard>

        {/* Próximos Eventos */}
        <AppCard>
          <CardContent sx={{ p: { xs: 1.5, sm: 2, md: 2.5 } }}>
            <Typography
              variant="h6"
              component="h2"
              gutterBottom
              sx={{
                fontSize: { xs: "0.9rem", sm: "1rem", md: "1.1rem" },
                mb: { xs: 1, sm: 1.5 },
              }}
            >
              Próximos Eventos
            </Typography>
            <UpcomingEventsList events={upcomingEvents} />
          </CardContent>
          <CardActions>
            <Button size="small" color="primary">
              Ver Calendário
            </Button>
          </CardActions>
        </AppCard>
      </Box>
    </Box>
  );
};

export default DashboardPage;
