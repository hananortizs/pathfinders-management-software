import React, { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";

interface SidebarContextType {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
  drawerWidth: number;
  collapsedDrawerWidth: number;
}

const SidebarContext = createContext<SidebarContextType | undefined>(undefined);

export const useSidebar = () => {
  const context = useContext(SidebarContext);
  if (context === undefined) {
    throw new Error("useSidebar must be used within a SidebarProvider");
  }
  return context;
};

interface SidebarProviderProps {
  children: ReactNode;
}

export const SidebarProvider: React.FC<SidebarProviderProps> = ({
  children,
}) => {
  const [collapsed, setCollapsed] = useState(false);
  const drawerWidth = 280;
  const collapsedDrawerWidth = 64;

  return (
    <SidebarContext.Provider
      value={{ collapsed, setCollapsed, drawerWidth, collapsedDrawerWidth }}
    >
      {children}
    </SidebarContext.Provider>
  );
};
