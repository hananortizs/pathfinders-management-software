/**
 * Página de Login
 *
 * Interface de autenticação com formulário de login,
 * validação de campos e integração com o sistema de autenticação
 */

import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import {
  Container,
  Paper,
  Box,
  Typography,
  FormControlLabel,
  Checkbox,
  Alert,
  CircularProgress,
  Divider,
  Link,
  IconButton,
  InputAdornment,
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useForm, Controller } from "react-hook-form";
import { usePageTitle } from "../hooks/usePageTitle";
import { useAuth } from "../hooks/useAuth";
import { AppTextField, PrimaryButton } from "../components/styled";
import { PmsIcon } from "../components/common/PmsIcon";

// Tipo do formulário
interface LoginFormData {
  email: string;
  password: string;
  rememberMe: boolean;
}

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { login, isAuthenticated, isLoading, error, clearError } = useAuth();

  // Definir título da página
  usePageTitle("Login");

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  // Redirecionar se já estiver autenticado
  useEffect(() => {
    if (isAuthenticated) {
      const from =
        (location.state as { from?: { pathname: string } })?.from?.pathname ||
        "/dashboard";
      navigate(from, { replace: true });
    }
  }, [isAuthenticated, navigate, location]);

  // Limpar erro quando componente montar
  useEffect(() => {
    clearError();
  }, [clearError]);

  const {
    control,
    handleSubmit,
    formState: { errors, isValid },
  } = useForm<LoginFormData>({
    defaultValues: {
      email: "",
      password: "",
      rememberMe: false,
    },
    mode: "onChange", // Validação em tempo real para melhor UX
  });

  const onSubmit = async (data: LoginFormData) => {
    setIsSubmitting(true);
    clearError();

    try {
      await login({
        email: data.email,
        password: data.password,
      });
    } catch (error) {
      // Erro já é tratado no hook useAuth
      console.error("Erro no login:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleTogglePasswordVisibility = () => {
    // Debounce para evitar cliques múltiplos
    if (isSubmitting) return;
    setShowPassword((prev) => !prev);
  };

  if (isLoading) {
    return (
      <Container maxWidth="sm" sx={{ py: 8 }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            minHeight: "50vh",
          }}
        >
          <CircularProgress size={48} />
          <Typography variant="h6" sx={{ mt: 2 }}>
            Carregando...
          </Typography>
        </Box>
      </Container>
    );
  }

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background:
          "linear-gradient(135deg, #0D47A1 0%, #1976D2 50%, #B71C1C 100%)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        position: "relative",
        overflow: "hidden",
        "&::before": {
          content: '""',
          position: "absolute",
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          background:
            'url("data:image/svg+xml,%3Csvg width="60" height="60" viewBox="0 0 60 60" xmlns="http://www.w3.org/2000/svg"%3E%3Cg fill="none" fill-rule="evenodd"%3E%3Cg fill="%23ffffff" fill-opacity="0.05"%3E%3Ccircle cx="30" cy="30" r="2"/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")',
          animation: "float 20s ease-in-out infinite",
        },
        "@keyframes float": {
          "0%, 100%": { transform: "translateY(0px)" },
          "50%": { transform: "translateY(-20px)" },
        },
      }}
    >
      <Container maxWidth="sm" sx={{ position: "relative", zIndex: 1 }}>
        <Paper
          elevation={8}
          sx={{
            p: 4,
            borderRadius: 3,
            background: "rgba(255, 255, 255, 0.95)",
            backdropFilter: "blur(10px)",
            border: "1px solid rgba(255, 255, 255, 0.2)",
            boxShadow: "0 8px 32px rgba(0, 0, 0, 0.1)",
          }}
        >
          <Box sx={{ textAlign: "center", mb: 4 }}>
            {/* Logo/Ícone dos Desbravadores */}
            <Box
              sx={{
                margin: "0 auto 24px auto",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <PmsIcon 
                size={80} 
                variant="circular"
                sx={{
                  filter: "drop-shadow(0 4px 20px rgba(13, 71, 161, 0.3))",
                }}
              />
            </Box>

            <Typography
              variant="h4"
              component="h1"
              gutterBottom
              sx={{
                background: "linear-gradient(135deg, #0D47A1, #B71C1C)",
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
                fontWeight: 700,
                letterSpacing: "0.5px",
              }}
            >
              Pathfinder Management
            </Typography>

            <Typography
              variant="h6"
              color="text.secondary"
              sx={{
                fontWeight: 500,
                mb: 1,
              }}
            >
              Sistema de Gerenciamento
            </Typography>

            <Typography
              variant="body2"
              color="text.secondary"
              sx={{
                opacity: 0.8,
                fontStyle: "italic",
              }}
            >
              "Por amor, serviço e aventura"
            </Typography>
          </Box>

          {error && (
            <Alert severity="error" sx={{ mb: 3 }} onClose={clearError}>
              {error}
            </Alert>
          )}

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Controller
              name="email"
              control={control}
              rules={{
                required: "Email é obrigatório",
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: "Email deve ser válido",
                },
              }}
              render={({ field }) => (
                <AppTextField
                  {...field}
                  fullWidth
                  label="Email"
                  type="email"
                  placeholder="seu@email.com"
                  error={!!errors.email}
                  helperText={errors.email?.message}
                  sx={{ mb: 3 }}
                  disabled={isSubmitting}
                  inputProps={{
                    autoComplete: "email",
                    "data-testid": "email-input",
                  }}
                />
              )}
            />

            <Controller
              name="password"
              control={control}
              rules={{
                required: "Senha é obrigatória",
                minLength: {
                  value: 6,
                  message: "Senha deve ter pelo menos 6 caracteres",
                },
              }}
              render={({ field }) => (
                <AppTextField
                  {...field}
                  fullWidth
                  label="Senha"
                  type={showPassword ? "text" : "password"}
                  placeholder="Digite sua senha"
                  error={!!errors.password}
                  helperText={errors.password?.message}
                  sx={{ mb: 2 }}
                  disabled={isSubmitting}
                  inputProps={{
                    autoComplete: "current-password",
                    "data-testid": "password-input",
                  }}
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          aria-label="toggle password visibility"
                          onClick={handleTogglePasswordVisibility}
                          onMouseDown={(e) => e.preventDefault()}
                          edge="end"
                          disabled={isSubmitting}
                          sx={{
                            color: "text.secondary",
                            "&:hover": {
                              color: "primary.main",
                            },
                          }}
                        >
                          {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  }}
                />
              )}
            />

            <Controller
              name="rememberMe"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      {...field}
                      checked={field.value}
                      disabled={isSubmitting}
                    />
                  }
                  label="Lembrar de mim"
                  sx={{ mb: 3 }}
                />
              )}
            />

            <PrimaryButton
              type="submit"
              fullWidth
              size="large"
              disabled={!isValid || isSubmitting}
              sx={{
                mb: 3,
                background: "linear-gradient(135deg, #0D47A1, #1976D2)",
                "&:hover": {
                  background: "linear-gradient(135deg, #002171, #0D47A1)",
                  transform: "translateY(-2px)",
                  boxShadow: "0 8px 25px rgba(13, 71, 161, 0.4)",
                },
                "&:active": {
                  transform: "translateY(0px)",
                },
                transition: "all 0.3s ease",
                fontWeight: 600,
                textTransform: "none",
                fontSize: "1.1rem",
                py: 1.5,
              }}
            >
              {isSubmitting ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                "Entrar no Sistema"
              )}
            </PrimaryButton>

            <Divider sx={{ my: 3 }} />

            <Box sx={{ textAlign: "center" }}>
              <Typography variant="body2" color="text.secondary">
                Esqueceu sua senha?{" "}
                <Link
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    // TODO: Implementar recuperação de senha
                    console.log("Recuperar senha");
                  }}
                  sx={{
                    textDecoration: "none",
                    color: "#0D47A1",
                    fontWeight: 500,
                    "&:hover": {
                      color: "#B71C1C",
                      textDecoration: "underline",
                    },
                    transition: "all 0.2s ease",
                  }}
                >
                  Clique aqui
                </Link>
              </Typography>
            </Box>
          </Box>
        </Paper>

        {/* Elementos decorativos dos Desbravadores */}
        <Box
          sx={{
            position: "absolute",
            bottom: 20,
            left: "50%",
            transform: "translateX(-50%)",
            display: "flex",
            alignItems: "center",
            gap: 2,
            opacity: 0.7,
          }}
        >
          <Typography
            variant="body2"
            sx={{
              color: "rgba(255, 255, 255, 0.8)",
              fontWeight: 500,
            }}
          >
            🏕️ 🌲 ⭐ 🌟 🏕️
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};

export default LoginPage;
