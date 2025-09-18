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
  Drawer,
  List,
  ListItem,
  ListItemButton,
  useTheme,
  useMediaQuery,
  Tooltip,
} from "@mui/material";
import {
  Dashboard,
  People,
  Event,
  School,
  Settings,
  Logout,
  Notifications,
  AccountCircle,
  Menu as MenuIcon,
  ChevronLeft,
  ChevronRight,
} from "@mui/icons-material";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

const drawerWidth = 240;
const collapsedDrawerWidth = 64;

interface MainLayoutProps {
  children: React.ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));
  const navigate = useNavigate();
  const location = useLocation();
  const { user, logout } = useAuth();

  const [mobileOpen, setMobileOpen] = useState(false);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const handleSidebarToggle = () => {
    setSidebarCollapsed(!sidebarCollapsed);
  };

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

  const navigationItems = [
    {
      text: "Dashboard",
      icon: <Dashboard />,
      path: "/dashboard",
    },
    {
      text: "Membros",
      icon: <People />,
      path: "/members",
    },
    {
      text: "Eventos",
      icon: <Event />,
      path: "/events",
    },
    {
      text: "Especialidades",
      icon: <School />,
      path: "/specialties",
    },
    {
      text: "Configurações",
      icon: <Settings />,
      path: "/settings",
    },
  ];

  // Sidebar simples sem header próprio
  const drawer = (
    <Box>
      <List sx={{ px: 1, py: 2 }}>
        {navigationItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <Tooltip
              title={item.text}
              placement="right"
              disableHoverListener={!sidebarCollapsed}
              arrow
            >
              <ListItemButton
                selected={location.pathname === item.path}
                onClick={() => {
                  navigate(item.path);
                  if (isMobile) {
                    setMobileOpen(false);
                  }
                }}
                sx={{
                  minHeight: 48,
                  borderRadius: 1,
                  mb: 0.5,
                  justifyContent: sidebarCollapsed ? "center" : "flex-start",
                  px: sidebarCollapsed ? 1 : 2,
                  "&.Mui-selected": {
                    backgroundColor: "primary.light",
                    color: "primary.contrastText",
                    "& .MuiListItemIcon-root": {
                      color: "primary.contrastText",
                    },
                  },
                  "&:hover": {
                    backgroundColor: "primary.light",
                    "& .MuiListItemIcon-root": {
                      color: "primary.contrastText",
                    },
                  },
                }}
              >
                <ListItemIcon
                  sx={{
                    minWidth: sidebarCollapsed ? 0 : 40,
                    justifyContent: "center",
                    mr: sidebarCollapsed ? 0 : 1,
                  }}
                >
                  {item.icon}
                </ListItemIcon>
                <ListItemText
                  primary={item.text}
                  sx={{
                    display: sidebarCollapsed ? "none" : "block",
                    "& .MuiListItemText-primary": {
                      fontSize: "0.875rem",
                      fontWeight: 500,
                    },
                  }}
                />
              </ListItemButton>
            </Tooltip>
          </ListItem>
        ))}
      </List>
    </Box>
  );

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
          {/* Menu Hambúrguer */}
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{
              mr: { xs: 1, sm: 2 },
              display: { xs: "block", md: "none" },
              p: { xs: 0.5, sm: 1 },
            }}
          >
            <MenuIcon />
          </IconButton>

          {/* Toggle Sidebar (apenas desktop) */}
          <IconButton
            color="inherit"
            aria-label="toggle sidebar"
            edge="start"
            onClick={handleSidebarToggle}
            sx={{
              mr: 2,
              display: { xs: "none", md: "block" },
            }}
          >
            {sidebarCollapsed ? <ChevronLeft /> : <ChevronRight />}
          </IconButton>

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
      <Box
        component="nav"
        sx={{
          width: { md: sidebarCollapsed ? collapsedDrawerWidth : drawerWidth },
          flexShrink: { md: 0 },
          transition: "width 0.3s ease",
        }}
      >
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{ keepMounted: true }}
          sx={{
            display: { xs: "block", md: "none" },
            "& .MuiDrawer-paper": {
              boxSizing: "border-box",
              width: drawerWidth,
            },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: "none", md: "block" },
            "& .MuiDrawer-paper": {
              boxSizing: "border-box",
              width: sidebarCollapsed ? collapsedDrawerWidth : drawerWidth,
              transition: "width 0.3s ease",
              overflowX: "hidden",
            },
          }}
          open
        >
          {drawer}
        </Drawer>
      </Box>

      {/* Main Content */}
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: { xs: 0.5, sm: 1, md: 1.5 },
          width: {
            md: sidebarCollapsed
              ? `calc(100% - ${collapsedDrawerWidth}px)`
              : `calc(100% - ${drawerWidth}px)`,
          },
          minHeight: "100vh",
          maxWidth: "100vw",
          overflow: "hidden",
          boxSizing: "border-box",
          transition: "width 0.3s ease",
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
