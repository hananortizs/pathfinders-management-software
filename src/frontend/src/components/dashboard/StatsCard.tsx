/**
 * Componente de Card de Estatísticas
 *
 * Card especializado para exibir métricas e estatísticas
 * com ícone, valor, label e indicador de mudança
 */

import React from "react";
import { Box, Typography, Avatar } from "@mui/material";
import { AppCard } from "../styled/cards";

interface StatsCardProps {
  title: string;
  value: string;
  change: string;
  changeType: "positive" | "negative" | "neutral";
  icon: React.ReactNode;
  color: "primary" | "secondary" | "success" | "warning" | "error" | "info";
}

const StatsCard: React.FC<StatsCardProps> = ({
  title,
  value,
  change,
  changeType,
  icon,
  color,
}) => {
  const getChangeColor = () => {
    switch (changeType) {
      case "positive":
        return "success.main";
      case "negative":
        return "error.main";
      case "neutral":
        return "text.secondary";
      default:
        return "text.secondary";
    }
  };

  const getChangeSymbol = () => {
    switch (changeType) {
      case "positive":
        return "+";
      case "negative":
        return "-";
      case "neutral":
        return "";
      default:
        return "";
    }
  };

  return (
    <AppCard sx={{ width: "100%", maxWidth: "100%" }}>
      <Box
        sx={{
          p: { xs: 0.75, sm: 1, md: 1.5 },
          textAlign: "center",
          height: "100%",
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          minHeight: { xs: 70, sm: 80, md: 90 },
          width: "100%",
          maxWidth: "100%",
        }}
      >
        <Avatar
          sx={{
            width: { xs: 24, sm: 28, md: 32 },
            height: { xs: 24, sm: 28, md: 32 },
            margin: "0 auto",
            marginBottom: { xs: 0.5, sm: 0.75, md: 1 },
            backgroundColor: `${color}.main`,
            color: `${color}.contrastText`,
          }}
        >
          {icon}
        </Avatar>

        <Typography
          variant="h4"
          component="div"
          sx={{
            fontWeight: 600,
            color: "text.primary",
            marginBottom: { xs: 0.25, sm: 0.5 },
            fontSize: { xs: "1rem", sm: "1.2rem", md: "1.4rem" },
          }}
        >
          {value}
        </Typography>

        <Typography
          variant="body2"
          sx={{
            color: "text.secondary",
            marginBottom: { xs: 0.25, sm: 0.5 },
            fontSize: { xs: "0.6rem", sm: "0.65rem", md: "0.7rem" },
            lineHeight: 1.2,
          }}
        >
          {title}
        </Typography>

        <Typography
          variant="body2"
          sx={{
            color: getChangeColor(),
            fontWeight: 500,
            fontSize: { xs: "0.55rem", sm: "0.6rem", md: "0.65rem" },
          }}
        >
          {getChangeSymbol()}
          {change}
        </Typography>
      </Box>
    </AppCard>
  );
};

export default StatsCard;
