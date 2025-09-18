/**
 * Página de Dashboard
 *
 * Página principal do sistema com visão geral das métricas
 * e ações rápidas disponíveis
 */

import React from "react";
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
  ListItemText,
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
import { AppCard } from "../components/styled";
import StatsCard from "../components/dashboard/StatsCard";

const DashboardPage: React.FC = () => {
  const { user } = useAuth();

  // Dados mockados para demonstração
  const stats = [
    {
      title: "Total de Membros",
      value: "1,234",
      change: "+12%",
      changeType: "positive" as const,
      icon: <People />,
      color: "primary" as const,
    },
    {
      title: "Eventos Este Mês",
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
  ];

  const recentActivities = [
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
  ];

  const upcomingEvents = [
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
      title: "Feira de Especialidades",
      date: "25 Setembro",
      location: "Ginásio Municipal",
      participants: 120,
    },
  ];

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

  return (
    <Box
      sx={{
        width: "100%",
        maxWidth: "100vw",
        overflow: "hidden",
        px: { xs: 1, sm: 2, md: 3 },
      }}
    >
      {/* Header */}
      <Box sx={{ mb: { xs: 1.5, sm: 2, md: 2 } }}>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            mb: { xs: 1, sm: 1.5 },
            flexWrap: "wrap",
            gap: { xs: 0.5, sm: 1 },
          }}
        >
          <Box
            sx={{
              width: { xs: 24, sm: 28, md: 32 },
              height: { xs: 24, sm: 28, md: 32 },
              background: "linear-gradient(135deg, #0D47A1, #1976D2)",
              borderRadius: "50%",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              marginRight: { xs: 0.5, sm: 1 },
              boxShadow: "0 2px 8px rgba(13, 71, 161, 0.3)",
              "&::before": {
                content: '"🏕️"',
                fontSize: { xs: "0.8rem", sm: "0.9rem", md: "1rem" },
              },
            }}
          />
          <Box sx={{ flex: 1, minWidth: 0 }}>
            <Typography
              variant="h4"
              component="h1"
              sx={{
                background: "linear-gradient(135deg, #0D47A1, #B71C1C)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
                fontWeight: 700,
                margin: 0,
                fontSize: {
                  xs: "1rem",
                  sm: "1.2rem",
                  md: "1.5rem",
                  lg: "1.8rem",
                },
              }}
            >
              Dashboard
            </Typography>
          </Box>
        </Box>
        <Typography
          variant="body1"
          color="text.secondary"
          sx={{
            fontSize: { xs: "0.7rem", sm: "0.8rem", md: "0.9rem" },
            mb: 0.25,
            lineHeight: 1.3,
          }}
        >
          Bem-vindo de volta, <strong>{user?.firstName || user?.email}</strong>!
          Aqui está um resumo do seu clube.
        </Typography>
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{
            fontStyle: "italic",
            fontSize: { xs: "0.6rem", sm: "0.65rem", md: "0.7rem" },
          }}
        >
          "Por amor, serviço e aventura" - Desbravadores
        </Typography>
      </Box>

      {/* Stats Cards */}
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "repeat(2, 1fr)",
            sm: "repeat(2, 1fr)",
            md: "repeat(4, 1fr)",
            lg: "repeat(4, 1fr)",
            xl: "repeat(4, 1fr)",
          },
          gap: { xs: 0.5, sm: 1, md: 1.5 },
          mb: { xs: 1.5, sm: 2, md: 2 },
          width: "100%",
          maxWidth: "100%",
          boxSizing: "border-box",
        }}
      >
        {stats.map((stat, index) => (
          <StatsCard
            key={index}
            title={stat.title}
            value={stat.value}
            change={stat.change}
            changeType={stat.changeType}
            icon={stat.icon}
            color={stat.color}
          />
        ))}
      </Box>

      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            sm: "repeat(2, 1fr)",
            md: "repeat(2, 1fr)",
          },
          gap: { xs: 0.5, sm: 1, md: 1.5 },
          width: "100%",
          maxWidth: "100%",
          boxSizing: "border-box",
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
            <List sx={{ py: 0 }}>
              {recentActivities.map((activity, index) => (
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
                    <ListItemText
                      component="div"
                      primary={
                        <Typography
                          variant="body2"
                          sx={{
                            fontSize: {
                              xs: "0.7rem",
                              sm: "0.75rem",
                              md: "0.8rem",
                            },
                            lineHeight: 1.2,
                          }}
                        >
                          {activity.title}
                        </Typography>
                      }
                      secondary={
                        <Typography
                          variant="caption"
                          sx={{
                            fontSize: { xs: "0.6rem", sm: "0.65rem" },
                          }}
                        >
                          {activity.time}
                        </Typography>
                      }
                    />
                  </ListItem>
                  {index < recentActivities.length - 1 && (
                    <Divider variant="inset" component="li" sx={{ ml: 5 }} />
                  )}
                </React.Fragment>
              ))}
            </List>
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
            <List sx={{ py: 0 }}>
              {upcomingEvents.map((event, index) => (
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
                        <CalendarToday />
                      </Avatar>
                    </ListItemAvatar>
                    <ListItemText
                      component="div"
                      primary={
                        <Typography
                          variant="body2"
                          sx={{
                            fontSize: {
                              xs: "0.7rem",
                              sm: "0.75rem",
                              md: "0.8rem",
                            },
                            lineHeight: 1.2,
                            mb: 0.25,
                          }}
                        >
                          {event.title}
                        </Typography>
                      }
                      secondary={
                        <Box>
                          <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{
                              fontSize: { xs: "0.6rem", sm: "0.65rem" },
                              display: "block",
                              mb: 0.25,
                            }}
                          >
                            {event.date}
                          </Typography>
                          <Box
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
                      }
                    />
                  </ListItem>
                  {index < upcomingEvents.length - 1 && (
                    <Divider variant="inset" component="li" sx={{ ml: 5 }} />
                  )}
                </React.Fragment>
              ))}
            </List>
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
