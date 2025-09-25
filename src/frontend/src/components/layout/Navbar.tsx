import React, { useState } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Avatar,
  Menu,
  MenuItem,
  Box,
  Chip,
  Divider,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import {
  Logout as LogoutIcon,
  Settings as SettingsIcon,
} from "@mui/icons-material";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";

/**
 * Componente Navbar fixa no topo
 * Inclui avatar menu com opções de perfil e logout
 */
export const Navbar: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    handleMenuClose();
    logout();
    navigate("/login");
  };

  const handleSettings = () => {
    handleMenuClose();
    navigate("/settings");
  };

  const getRoleColor = (role?: string) => {
    switch (role?.toLowerCase()) {
      case "admin":
        return "error";
      case "regional":
        return "warning";
      case "distrital":
        return "info";
      case "director":
        return "primary";
      case "secretary":
        return "secondary";
      default:
        return "default";
    }
  };

  const getRoleLabel = (role?: string) => {
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

  return (
    <AppBar
      position="fixed"
      sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
    >
      <Toolbar>
        {/* Logo/Título */}
        <Box
          sx={{ display: "flex", alignItems: "center", gap: 2, flexGrow: 1 }}
        >
          <Typography variant="h6" component="div" sx={{ fontWeight: "bold" }}>
            PMS - Pathfinders
          </Typography>
          <Chip
            label={getRoleLabel(user?.roles?.[0])}
            color={getRoleColor(user?.roles?.[0]) as any}
            size="small"
            variant="outlined"
            sx={{
              color: "white",
              borderColor: "rgba(255, 255, 255, 0.5)",
              "&:hover": {
                borderColor: "white",
              },
            }}
          />
        </Box>

        {/* Avatar Menu */}
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <Typography
            variant="body2"
            sx={{ display: { xs: "none", sm: "block" } }}
          >
            Olá,{" "}
            {(user as any)?.FullName ||
              user?.firstName + " " + user?.lastName ||
              "Usuário"}
          </Typography>
          <IconButton
            size="large"
            edge="end"
            aria-label="menu do usuário"
            aria-controls="user-menu"
            aria-haspopup="true"
            onClick={handleMenuOpen}
            color="inherit"
          >
            <Avatar
              sx={{
                width: 32,
                height: 32,
                bgcolor: "secondary.main",
                fontSize: "0.875rem",
              }}
            >
              {((user as any)?.FullName || user?.firstName || "U")
                .charAt(0)
                .toUpperCase()}
            </Avatar>
          </IconButton>
        </Box>

        {/* Menu Dropdown */}
        <Menu
          id="user-menu"
          anchorEl={anchorEl}
          anchorOrigin={{
            vertical: "bottom",
            horizontal: "right",
          }}
          keepMounted
          transformOrigin={{
            vertical: "top",
            horizontal: "right",
          }}
          open={Boolean(anchorEl)}
          onClose={handleMenuClose}
          PaperProps={{
            sx: {
              minWidth: 200,
              mt: 1,
            },
          }}
        >
          {/* Header do Menu */}
          <Box sx={{ px: 2, py: 1.5, borderBottom: 1, borderColor: "divider" }}>
            <Box
              sx={{ display: "flex", alignItems: "center", gap: 1, mb: 0.5 }}
            >
              <Typography variant="subtitle2" sx={{ fontWeight: "bold" }}>
                {(user as any)?.FullName ||
                  user?.firstName + " " + user?.lastName ||
                  "Usuário"}
              </Typography>
              <Chip
                label={getRoleLabel(user?.roles?.[0])}
                color={getRoleColor(user?.roles?.[0]) as any}
                size="small"
              />
            </Box>
            <Typography variant="caption" color="text.secondary">
              {(user as any)?.Email || user?.email || "email@exemplo.com"}
            </Typography>
          </Box>

          {/* Opções do Menu */}

          <MenuItem onClick={handleSettings}>
            <ListItemIcon>
              <SettingsIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Configurações</ListItemText>
          </MenuItem>

          <Divider />

          <MenuItem onClick={handleLogout} sx={{ color: "error.main" }}>
            <ListItemIcon>
              <LogoutIcon fontSize="small" sx={{ color: "error.main" }} />
            </ListItemIcon>
            <ListItemText>Sair</ListItemText>
          </MenuItem>
        </Menu>
      </Toolbar>
    </AppBar>
  );
};
