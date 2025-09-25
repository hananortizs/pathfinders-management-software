/**
 * Componente Principal da Aplicação
 *
 * Configuração de rotas, providers e estrutura base da aplicação
 */

import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { Suspense, lazy } from "react";
import { ThemeProvider } from "./theme";
import MainLayout from "./components/layout/MainLayout";
import ProtectedRoute from "./components/auth/ProtectedRoute";
import AuthInitializer from "./components/auth/AuthInitializer";
import { SidebarProvider } from "./contexts/SidebarContext";

// Lazy loading para otimizar bundle size
const LoginPage = lazy(() => import("./pages/LoginPage"));
const DashboardPage = lazy(() => import("./pages/DashboardPage"));
const MembersPage = lazy(() => import("./pages/MembersPage"));
const ProfilePage = lazy(() => import("./pages/ProfilePage"));

// Configuração do React Query
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 3,
      refetchOnWindowFocus: false,
      staleTime: 0, // Sempre buscar dados frescos para debug
      gcTime: 0, // Sem cache para debug
    },
  },
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider>
        <AuthInitializer>
          <SidebarProvider>
            <Router>
              <Suspense
                fallback={
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      height: "100vh",
                    }}
                  >
                    <div>Carregando...</div>
                  </div>
                }
              >
                <Routes>
                  {/* Rota de Login */}
                  <Route path="/login" element={<LoginPage />} />

                  {/* Rotas Protegidas */}
                  <Route
                    path="/"
                    element={
                      <ProtectedRoute>
                        <MainLayout>
                          <Navigate to="/dashboard" replace />
                        </MainLayout>
                      </ProtectedRoute>
                    }
                  />

                  <Route
                    path="/dashboard"
                    element={
                      <ProtectedRoute>
                        <MainLayout>
                          <DashboardPage />
                        </MainLayout>
                      </ProtectedRoute>
                    }
                  />

                  <Route
                    path="/members"
                    element={
                      <ProtectedRoute>
                        <MainLayout>
                          <MembersPage />
                        </MainLayout>
                      </ProtectedRoute>
                    }
                  />

                  <Route
                    path="/profile"
                    element={
                      <ProtectedRoute>
                        <MainLayout>
                          <ProfilePage />
                        </MainLayout>
                      </ProtectedRoute>
                    }
                  />

                  {/* Rota de fallback */}
                  <Route
                    path="*"
                    element={<Navigate to="/dashboard" replace />}
                  />
                </Routes>
              </Suspense>
            </Router>
          </SidebarProvider>
        </AuthInitializer>
      </ThemeProvider>

      {/* DevTools do React Query (apenas em desenvolvimento) */}
      {import.meta.env.DEV && <ReactQueryDevtools initialIsOpen={false} />}
    </QueryClientProvider>
  );
}

export default App;

