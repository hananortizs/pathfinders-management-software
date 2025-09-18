import React from "react";
import { ThemeProvider as MuiThemeProvider, CssBaseline } from "@mui/material";
import { appTheme } from "./theme";

/**
 * Provedor de tema customizado para a aplicação
 *
 * Envolve a aplicação com o tema MUI customizado e configurações globais
 * como CssBaseline para reset de estilos
 */
interface ThemeProviderProps {
  children: React.ReactNode;
}

export const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
  return (
    <MuiThemeProvider theme={appTheme}>
      <CssBaseline />
      {children}
    </MuiThemeProvider>
  );
};

export default ThemeProvider;
