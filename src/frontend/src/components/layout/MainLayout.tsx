/**
 * Layout Principal
 *
 * Layout base da aplicação com header, navegação lateral
 * e área de conteúdo principal
 */

import React, { useState } from "react";
import {
  Box,
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Avatar,
  Menu,
  MenuItem,
  Divider,
  Badge,
  ListItemIcon,
  ListItemText,
  useTheme,
} from "@mui/material";
import {
  Logout,
  Notifications,
  AccountCircle,
  Settings,
} from "@mui/icons-material";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { Sidebar } from "./Sidebar";
import { useSidebar } from "../../contexts/SidebarContext";

interface MainLayoutProps {
  children: React.ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const theme = useTheme();
  const navigate = useNavigate();
  const { user, logout } = useAuth();
  const { collapsed, drawerWidth, collapsedDrawerWidth } = useSidebar();

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleProfileMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = async () => {
    handleProfileMenuClose();
    await logout();
    navigate("/login");
  };

  return (
    <Box sx={{ display: "flex" }}>
      {/* App Bar */}
      <AppBar
        position="fixed"
        sx={{
          width: "100%",
          borderRadius: 0,
          zIndex: theme.zIndex.drawer + 1,
        }}
      >
        <Toolbar sx={{ px: { xs: 1, sm: 2 } }}>
          {/* Logo e Título */}
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              flexGrow: 1,
              minWidth: 0,
            }}
          >
            <Box
              sx={{
                width: { xs: 32, sm: 40 },
                height: { xs: 32, sm: 40 },
                background: "linear-gradient(135deg, #0D47A1, #1976D2)",
                borderRadius: "50%",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                marginRight: { xs: 1, sm: 2 },
                boxShadow: "0 2px 8px rgba(13, 71, 161, 0.3)",
                "&::before": {
                  content: '"🏕️"',
                  fontSize: { xs: "1rem", sm: "1.2rem" },
                },
              }}
            />
            <Typography
              variant="h6"
              noWrap
              component="div"
              sx={{
                fontSize: { xs: "0.875rem", sm: "1.25rem" },
                display: { xs: "none", sm: "block" },
              }}
            >
              Pathfinder Management
            </Typography>
            <Typography
              variant="h6"
              noWrap
              component="div"
              sx={{
                fontSize: "0.875rem",
                display: { xs: "block", sm: "none" },
              }}
            >
              PMS
            </Typography>
          </Box>

          {/* Notificações e Perfil */}
          <IconButton
            color="inherit"
            sx={{
              mr: { xs: 0.5, sm: 1 },
              p: { xs: 0.5, sm: 1 },
            }}
          >
            <Badge badgeContent={4} color="error">
              <Notifications fontSize="small" />
            </Badge>
          </IconButton>

          <IconButton
            size="small"
            edge="end"
            aria-label="account of current user"
            aria-controls="primary-search-account-menu"
            aria-haspopup="true"
            onClick={handleProfileMenuOpen}
            color="inherit"
            sx={{ p: { xs: 0.5, sm: 1 } }}
          >
            <Avatar
              sx={{ width: { xs: 28, sm: 32 }, height: { xs: 28, sm: 32 } }}
            >
              {user?.firstName?.charAt(0).toUpperCase() ||
                user?.email?.charAt(0).toUpperCase()}
            </Avatar>
          </IconButton>
        </Toolbar>
      </AppBar>

      {/* Profile Menu */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleProfileMenuClose}
        onClick={handleProfileMenuClose}
        PaperProps={{
          elevation: 0,
          sx: {
            overflow: "visible",
            filter: "drop-shadow(0px 2px 8px rgba(0,0,0,0.32))",
            mt: 1.5,
            "& .MuiAvatar-root": {
              width: 32,
              height: 32,
              ml: -0.5,
              mr: 1,
            },
            "&:before": {
              content: '""',
              display: "block",
              position: "absolute",
              top: 0,
              right: 14,
              width: 10,
              height: 10,
              bgcolor: "background.paper",
              transform: "translateY(-50%) rotate(45deg)",
              zIndex: 0,
            },
          },
        }}
        transformOrigin={{ horizontal: "right", vertical: "top" }}
        anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
      >
        <MenuItem onClick={() => navigate("/profile")}>
          <ListItemIcon>
            <AccountCircle fontSize="small" />
          </ListItemIcon>
          <ListItemText>Perfil</ListItemText>
        </MenuItem>
        <MenuItem onClick={() => navigate("/settings")}>
          <ListItemIcon>
            <Settings fontSize="small" />
          </ListItemIcon>
          <ListItemText>Configurações</ListItemText>
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <Logout fontSize="small" />
          </ListItemIcon>
          <ListItemText>Sair</ListItemText>
        </MenuItem>
      </Menu>

      {/* Sidebar */}
      <Sidebar />

      {/* Main Content */}
      <Box
        component="main"
        sx={{
          p: { xs: 0.5, sm: 1, md: 1.5 },
          minHeight: "100vh",
          overflow: "hidden",
          boxSizing: "border-box",
          width: {
            xs: "100%",
            sm: `calc(100% - ${
              collapsed ? collapsedDrawerWidth : drawerWidth
            }px)`,
          },
          marginLeft: {
            xs: 0,
            sm: `${collapsed ? collapsedDrawerWidth : drawerWidth}px`,
          },
          transition: "width 0.3s ease, margin-left 0.3s ease",
        }}
      >
        <Toolbar />
        <Box
          sx={{
            width: "100%",
            maxWidth: "100%",
            overflow: "hidden",
            boxSizing: "border-box",
          }}
        >
          {children}
        </Box>
      </Box>
    </Box>
  );
};

export default MainLayout;
