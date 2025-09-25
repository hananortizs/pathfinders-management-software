import React from "react";
import {
  Box,
  Avatar,
  Typography,
  Chip,
  Button,
  Paper,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import {
  Edit as EditIcon,
  Lock as LockIcon,
  History as HistoryIcon,
} from "@mui/icons-material";
import type { ProfileHeaderProps } from "../../types/profile";

/**
 * Componente Header do Perfil
 * Exibe avatar, nome, papel, status e ações rápidas
 */
export const ProfileHeader: React.FC<ProfileHeaderProps> = ({
  user,
  onEditProfile,
  onChangePassword,
  onViewAudit,
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isXs = useMediaQuery(theme.breakpoints.down("xs"));

  const getStatusColor = (status: string) => {
    switch (status) {
      case "Active":
        return "success";
      case "Pending":
        return "warning";
      case "Archived":
        return "error";
      default:
        return "default";
    }
  };

  const getStatusLabel = (status: string) => {
    switch (status) {
      case "Active":
        return "Ativo";
      case "Pending":
        return "Pendente";
      case "Archived":
        return "Arquivado";
      default:
        return status;
    }
  };


  const getRoleLabel = (role: string) => {
    switch (role?.toLowerCase()) {
      case "admin":
        return "Administrador";
      case "regional":
        return "Regional";
      case "distrital":
        return "Distrital";
      case "director":
        return "Diretor";
      case "secretary":
        return "Secretário";
      default:
        return "Membro";
    }
  };

  const fullName = user.socialName || `${user.firstName} ${user.lastName}`;
  const primaryRole = user.roles?.[0] || "member";

  return (
    <Paper
      elevation={0}
      sx={{
        p: isMobile ? 2 : 3,
        mb: 3,
        background: "linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%)",
        border: "1px solid #e2e8f0",
        borderRadius: 3,
        position: "relative",
        overflow: "hidden",
        boxShadow:
          "0 10px 25px rgba(0, 0, 0, 0.1), 0 4px 10px rgba(0, 0, 0, 0.06)",
        "&::before": {
          content: '""',
          position: "absolute",
          top: 0,
          left: 0,
          right: 0,
          height: "4px",
          background:
            "linear-gradient(90deg, #0D47A1 0%, #1976D2 50%, #42A5F5 100%)",
        },
        "&::after": {
          content: '""',
          position: "absolute",
          top: -50,
          right: -50,
          width: 100,
          height: 100,
          background:
            "radial-gradient(circle, rgba(13, 71, 161, 0.1) 0%, transparent 70%)",
          borderRadius: "50%",
        },
      }}
    >
      {/* Seção 1: Avatar + Nome + Status */}
      <Box sx={{ display: "flex", alignItems: "flex-start", gap: 2, mb: 3 }}>
        {/* Avatar */}
        <Box sx={{ position: "relative", flexShrink: 0 }}>
          <Avatar
            sx={{
              width: isMobile ? 72 : 96,
              height: isMobile ? 72 : 96,
              background: "linear-gradient(135deg, #0D47A1 0%, #1976D2 100%)",
              fontSize: isMobile ? "2rem" : "2.5rem",
              fontWeight: "600",
              color: "white",
              boxShadow: "0 8px 32px rgba(13, 71, 161, 0.3)",
              border: "4px solid white",
            }}
          >
            {fullName.charAt(0).toUpperCase()}
          </Avatar>
          {/* Indicador de status */}
          <Box
            sx={{
              position: "absolute",
              bottom: 2,
              right: 2,
              width: 20,
              height: 20,
              borderRadius: "50%",
              backgroundColor:
                getStatusColor(user.status) === "success"
                  ? "#4caf50"
                  : getStatusColor(user.status) === "warning"
                  ? "#ff9800"
                  : "#f44336",
              border: "3px solid white",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.2)",
            }}
          />
        </Box>

        {/* Informações do usuário */}
        <Box sx={{ flex: 1, minWidth: 0 }}>
          {/* Nome do usuário */}
          <Typography
            variant={isMobile ? "h5" : "h4"}
            sx={{
              fontWeight: "700",
              color: "#1e293b",
              mb: 1,
              wordBreak: "break-word",
              lineHeight: 1.2,
              letterSpacing: "-0.025em",
            }}
          >
            {fullName}
          </Typography>

          {/* Chips de Status */}
          <Box sx={{ display: "flex", gap: 1, mb: 1, flexWrap: "wrap" }}>
            <Chip
              label={getRoleLabel(primaryRole)}
              size="small"
              sx={{
                backgroundColor: "#0D47A1",
                color: "white",
                fontWeight: "600",
                fontSize: "0.75rem",
                height: 28,
                borderRadius: 2,
                boxShadow: "0 2px 8px rgba(13, 71, 161, 0.2)",
                "& .MuiChip-label": {
                  color: "white",
                  px: 1.5,
                },
              }}
            />

            <Chip
              label={getStatusLabel(user.status)}
              size="small"
              variant="outlined"
              sx={{
                color:
                  getStatusColor(user.status) === "success"
                    ? "#059669"
                    : getStatusColor(user.status) === "warning"
                    ? "#d97706"
                    : "#dc2626",
                borderColor:
                  getStatusColor(user.status) === "success"
                    ? "#10b981"
                    : getStatusColor(user.status) === "warning"
                    ? "#f59e0b"
                    : "#ef4444",
                backgroundColor:
                  getStatusColor(user.status) === "success"
                    ? "#ecfdf5"
                    : getStatusColor(user.status) === "warning"
                    ? "#fffbeb"
                    : "#fef2f2",
                fontWeight: "500",
                fontSize: "0.75rem",
                height: 28,
                borderRadius: 2,
                "& .MuiChip-label": {
                  px: 1.5,
                },
              }}
            />
          </Box>

          {/* Clube/Unidade */}
          {(user.clubName || user.unitName) && (
            <Typography
              variant="body2"
              sx={{
                color: "#64748b",
                fontSize: "0.875rem",
                fontWeight: "500",
                fontStyle: "italic",
                wordBreak: "break-word",
              }}
            >
              {user.clubName && user.unitName
                ? `${user.clubName} • ${user.unitName}`
                : user.clubName || user.unitName}
            </Typography>
          )}
        </Box>
      </Box>

      {/* Seção 2: Botões de Ação */}
      <Box
        sx={{
          display: "flex",
          gap: 1.5,
          mb: 3,
          justifyContent: isMobile ? "center" : "flex-start",
          flexWrap: "wrap",
        }}
      >
        <Button
          variant="contained"
          startIcon={<EditIcon />}
          onClick={onEditProfile}
          size={isMobile ? "small" : "medium"}
          sx={{
            background: "linear-gradient(135deg, #0D47A1 0%, #1976D2 100%)",
            color: "white",
            fontSize: isMobile ? "0.75rem" : "0.875rem",
            fontWeight: "600",
            borderRadius: 2,
            px: 3,
            py: 1.5,
            boxShadow: "0 4px 12px rgba(13, 71, 161, 0.3)",
            "&:hover": {
              background: "linear-gradient(135deg, #0a3d91 0%, #1565c0 100%)",
              boxShadow: "0 6px 16px rgba(13, 71, 161, 0.4)",
              transform: "translateY(-1px)",
            },
            transition: "all 0.2s ease-in-out",
          }}
        >
          {isMobile ? "Editar" : "Editar Perfil"}
        </Button>

        <Button
          variant="outlined"
          startIcon={<LockIcon />}
          onClick={onChangePassword}
          size={isMobile ? "small" : "medium"}
          sx={{
            borderColor: "#e2e8f0",
            color: "#475569",
            backgroundColor: "white",
            fontSize: isMobile ? "0.75rem" : "0.875rem",
            fontWeight: "500",
            borderRadius: 2,
            px: 3,
            py: 1.5,
            "&:hover": {
              borderColor: "#0D47A1",
              color: "#0D47A1",
              backgroundColor: "#f8fafc",
              transform: "translateY(-1px)",
            },
            transition: "all 0.2s ease-in-out",
          }}
        >
          Alterar Senha
        </Button>

        <Button
          variant="outlined"
          startIcon={<HistoryIcon />}
          onClick={onViewAudit}
          size={isMobile ? "small" : "medium"}
          sx={{
            borderColor: "#e2e8f0",
            color: "#475569",
            backgroundColor: "white",
            fontSize: isMobile ? "0.75rem" : "0.875rem",
            fontWeight: "500",
            borderRadius: 2,
            px: 3,
            py: 1.5,
            "&:hover": {
              borderColor: "#0D47A1",
              color: "#0D47A1",
              backgroundColor: "#f8fafc",
              transform: "translateY(-1px)",
            },
            transition: "all 0.2s ease-in-out",
          }}
        >
          Auditoria
        </Button>
      </Box>

      {/* Informações adicionais */}
      <Box
        sx={{
          display: "flex",
          justifyContent: isMobile ? "center" : "space-between",
          alignItems: "center",
          flexDirection: isMobile ? "column" : "row",
          gap: isMobile ? 1.5 : 0,
          mt: 2,
          pt: 2,
          borderTop: "1px solid #e2e8f0",
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <Box
            sx={{
              width: 4,
              height: 4,
              borderRadius: "50%",
              backgroundColor: "#10b981",
              flexShrink: 0,
            }}
          />
          <Typography
            variant="body2"
            sx={{
              color: "#64748b",
              fontSize: isXs ? "0.7rem" : "0.75rem",
              fontWeight: "500",
            }}
          >
            Membro desde {new Date(user.createdAt).toLocaleDateString("pt-BR")}
          </Typography>
        </Box>

        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <Box
            sx={{
              width: 4,
              height: 4,
              borderRadius: "50%",
              backgroundColor: "#6366f1",
              flexShrink: 0,
            }}
          />
          <Typography
            variant="body2"
            sx={{
              color: "#64748b",
              fontSize: isXs ? "0.7rem" : "0.75rem",
              fontWeight: "500",
            }}
          >
            Última atualização:{" "}
            {new Date(user.updatedAt).toLocaleDateString("pt-BR")}
          </Typography>
        </Box>
      </Box>
    </Paper>
  );
};
