import React, { useState } from "react";
import {
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Box,
  Typography,
  Collapse,
  useTheme,
  useMediaQuery,
} from "@mui/material";
import {
  Dashboard as DashboardIcon,
  People as PeopleIcon,
  School as SchoolIcon,
  Event as EventIcon,
  Assessment as AssessmentIcon,
  Settings as SettingsIcon,
  ExpandLess as ExpandLessIcon,
  ExpandMore as ExpandMoreIcon,
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
} from "@mui/icons-material";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate, useLocation } from "react-router-dom";
import { useSidebar } from "../../contexts/SidebarContext";

interface SidebarItem {
  id: string;
  label: string;
  icon: React.ReactNode;
  path: string;
  children?: SidebarItem[];
  roles?: string[];
}

/**
 * Componente Sidebar com navegação baseada no papel do usuário
 * Colapsável em telas pequenas
 */
export const Sidebar: React.FC = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const { collapsed, setCollapsed, drawerWidth, collapsedDrawerWidth } =
    useSidebar();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [expandedItems, setExpandedItems] = useState<string[]>([]);

  // Definição dos itens do menu baseados no papel do usuário
  const getMenuItems = (): SidebarItem[] => {
    const baseItems: SidebarItem[] = [
      {
        id: "dashboard",
        label: "Dashboard",
        icon: <DashboardIcon />,
        path: "/dashboard",
      },
      {
        id: "profile",
        label: "Meu Perfil",
        icon: <PeopleIcon />,
        path: "/profile",
        roles: [
          "member",
          "director",
          "secretary",
          "distrital",
          "regional",
          "admin",
        ],
      },
    ];

    const role = user?.roles?.[0]?.toLowerCase();

    switch (role) {
      case "admin":
        return [
          ...baseItems,
          {
            id: "members",
            label: "Membros",
            icon: <PeopleIcon />,
            path: "/members",
            roles: ["admin"],
          },
          {
            id: "units",
            label: "Unidades",
            icon: <SchoolIcon />,
            path: "/units",
            roles: ["admin"],
          },
          {
            id: "events",
            label: "Eventos",
            icon: <EventIcon />,
            path: "/events",
            roles: ["admin"],
          },
          {
            id: "reports",
            label: "Relatórios",
            icon: <AssessmentIcon />,
            path: "/reports",
            roles: ["admin"],
          },
          {
            id: "settings",
            label: "Configurações",
            icon: <SettingsIcon />,
            path: "/settings",
            roles: ["admin"],
          },
        ];

      case "regional":
        return [
          ...baseItems,
          {
            id: "members",
            label: "Membros (Regional)",
            icon: <PeopleIcon />,
            path: "/members",
            roles: ["regional", "admin"],
          },
          {
            id: "events",
            label: "Eventos Regionais",
            icon: <EventIcon />,
            path: "/events",
            roles: ["regional", "admin"],
          },
          {
            id: "reports",
            label: "Relatórios Regionais",
            icon: <AssessmentIcon />,
            path: "/reports",
            roles: ["regional", "admin"],
          },
          {
            id: "delegations",
            label: "Delegações",
            icon: <SettingsIcon />,
            path: "/delegations",
            roles: ["regional", "admin"],
          },
        ];

      case "distrital":
        return [
          ...baseItems,
          {
            id: "members",
            label: "Membros (Distrital)",
            icon: <PeopleIcon />,
            path: "/members",
            roles: ["distrital", "regional", "admin"],
          },
          {
            id: "events",
            label: "Eventos Distritais",
            icon: <EventIcon />,
            path: "/events",
            roles: ["distrital", "regional", "admin"],
          },
          {
            id: "reports",
            label: "Relatórios Distritais",
            icon: <AssessmentIcon />,
            path: "/reports",
            roles: ["distrital", "regional", "admin"],
          },
        ];

      case "director":
      case "secretary":
        return [
          ...baseItems,
          {
            id: "members",
            label: "Membros do Clube",
            icon: <PeopleIcon />,
            path: "/members",
            roles: ["director", "secretary", "distrital", "regional", "admin"],
          },
          {
            id: "units",
            label: "Unidades",
            icon: <SchoolIcon />,
            path: "/units",
            roles: ["director", "secretary", "admin"],
          },
          {
            id: "events",
            label: "Eventos",
            icon: <EventIcon />,
            path: "/events",
            roles: ["director", "secretary", "distrital", "regional", "admin"],
          },
          {
            id: "reports",
            label: "Relatórios (CSV)",
            icon: <AssessmentIcon />,
            path: "/reports",
            roles: ["director", "secretary", "admin"],
          },
          {
            id: "specializations",
            label: "Especialidades",
            icon: <SchoolIcon />,
            path: "/specializations",
            roles: ["director", "secretary", "admin"],
          },
          {
            id: "classes",
            label: "Classes",
            icon: <SchoolIcon />,
            path: "/classes",
            roles: ["director", "secretary", "admin"],
          },
        ];

      default: // Membro
        return [
          ...baseItems,
          {
            id: "specializations",
            label: "Minhas Especialidades",
            icon: <SchoolIcon />,
            path: "/specializations",
            roles: [
              "member",
              "director",
              "secretary",
              "distrital",
              "regional",
              "admin",
            ],
          },
          {
            id: "classes",
            label: "Minhas Classes",
            icon: <SchoolIcon />,
            path: "/classes",
            roles: [
              "member",
              "director",
              "secretary",
              "distrital",
              "regional",
              "admin",
            ],
          },
          {
            id: "events",
            label: "Meus Eventos",
            icon: <EventIcon />,
            path: "/events",
            roles: [
              "member",
              "director",
              "secretary",
              "distrital",
              "regional",
              "admin",
            ],
          },
        ];
    }
  };

  const menuItems = getMenuItems();

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const handleCollapseToggle = () => {
    setCollapsed(!collapsed);
  };

  const handleItemClick = (item: SidebarItem) => {
    if (item.children) {
      // Toggle expand/collapse for items with children
      setExpandedItems((prev) =>
        prev.includes(item.id)
          ? prev.filter((id) => id !== item.id)
          : [...prev, item.id]
      );
    } else {
      // Navigate to the item's path
      navigate(item.path);
      if (isMobile) {
        setMobileOpen(false);
      }
    }
  };

  const isItemActive = (item: SidebarItem): boolean => {
    if (item.children) {
      return item.children.some((child) => location.pathname === child.path);
    }
    return location.pathname === item.path;
  };

  const canAccessItem = (item: SidebarItem): boolean => {
    if (!item.roles || item.roles.length === 0) {
      return true;
    }
    return item.roles.includes(user?.roles?.[0]?.toLowerCase() || "member");
  };

  const renderMenuItem = (item: SidebarItem, level: number = 0) => {
    if (!canAccessItem(item)) {
      return null;
    }

    const isActive = isItemActive(item);
    const isExpanded = expandedItems.includes(item.id);
    const hasChildren = item.children && item.children.length > 0;

    return (
      <React.Fragment key={item.id}>
        <ListItem disablePadding>
          <ListItemButton
            onClick={() => handleItemClick(item)}
            selected={isActive}
            sx={{
              pl: 2 + level * 2,
              "&.Mui-selected": {
                backgroundColor: "primary.main",
                color: "white",
                "&:hover": {
                  backgroundColor: "primary.dark",
                },
                "& .MuiListItemIcon-root": {
                  color: "white",
                },
              },
            }}
          >
            <ListItemIcon>{item.icon}</ListItemIcon>
            <ListItemText
              primary={item.label}
              primaryTypographyProps={{
                fontSize: level > 0 ? "0.875rem" : "1rem",
                fontWeight: isActive ? "bold" : "normal",
              }}
            />
            {hasChildren &&
              (isExpanded ? <ExpandLessIcon /> : <ExpandMoreIcon />)}
          </ListItemButton>
        </ListItem>
        {hasChildren && (
          <Collapse in={isExpanded} timeout="auto" unmountOnExit>
            <List component="div" disablePadding>
              {item.children?.map((child) => renderMenuItem(child, level + 1))}
            </List>
          </Collapse>
        )}
      </React.Fragment>
    );
  };

  const drawer = (
    <Box sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
      {/* Header da Sidebar */}
      <Box sx={{ p: 2, borderBottom: 1, borderColor: "divider" }}>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
          }}
        >
          {!collapsed && (
            <Box>
              <Typography
                variant="h6"
                sx={{ fontWeight: "bold", color: "primary.main" }}
              >
                PMS
              </Typography>
              <Typography variant="caption" color="text.secondary">
                Pathfinders Management
              </Typography>
            </Box>
          )}
          <Box
            onClick={handleCollapseToggle}
            sx={{
              cursor: "pointer",
              p: 0.5,
              borderRadius: 1,
              "&:hover": { backgroundColor: "action.hover" },
              display: { xs: "none", sm: "block" },
            }}
          >
            {collapsed ? <ChevronRightIcon /> : <ChevronLeftIcon />}
          </Box>
        </Box>
      </Box>

      {/* Menu Items */}
      <Box sx={{ flex: 1, overflow: "auto" }}>
        <List>{menuItems.map((item) => renderMenuItem(item))}</List>
      </Box>

      {/* Footer da Sidebar */}
      {!collapsed && (
        <Box sx={{ p: 2, borderTop: 1, borderColor: "divider" }}>
          <Typography
            variant="caption"
            color="text.secondary"
            sx={{ display: "block", textAlign: "center" }}
          >
            v1.0.0 - MVP0
          </Typography>
        </Box>
      )}
    </Box>
  );

  return (
    <>
      {/* Mobile Drawer */}
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={handleDrawerToggle}
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: "block", sm: "none" },
          "& .MuiDrawer-paper": {
            boxSizing: "border-box",
            width: drawerWidth,
          },
        }}
      >
        {drawer}
      </Drawer>

      {/* Desktop Drawer */}
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: "none", sm: "block" },
          "& .MuiDrawer-paper": {
            boxSizing: "border-box",
            width: collapsed ? collapsedDrawerWidth : drawerWidth,
            transition: "width 0.3s ease",
            overflowX: "hidden",
          },
        }}
        open
      >
        {drawer}
      </Drawer>
    </>
  );
};
