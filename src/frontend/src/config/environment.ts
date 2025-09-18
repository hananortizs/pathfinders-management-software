/**
 * Configurações de ambiente para o frontend
 */

export interface EnvironmentConfig {
  apiBaseUrl: string;
  isDevelopment: boolean;
  isProduction: boolean;
  environment: string;
}

/**
 * Determina a configuração baseada no ambiente atual
 */
export function getEnvironmentConfig(): EnvironmentConfig {
  const isDevelopment = import.meta.env.DEV;
  const isProduction = import.meta.env.PROD;
  const environment = import.meta.env.MODE || 'development';

  // Se houver uma variável de ambiente definida, usar ela
  if (import.meta.env.VITE_API_BASE_URL) {
    return {
      apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
      isDevelopment,
      isProduction,
      environment,
    };
  }

  // Detectar se está rodando localmente
  const isLocalhost = window.location.hostname === 'localhost' || 
                     window.location.hostname === '127.0.0.1';
  const isLocalPort = window.location.port === '5173' || 
                     window.location.port === '3000' || 
                     window.location.port === '';

  let apiBaseUrl: string;

  if (isDevelopment && isLocalhost && isLocalPort) {
    // Desenvolvimento local: usar porta 5000 com prefixo pms-loc
    apiBaseUrl = "http://localhost:5000/pms-loc";
  } else if (isProduction) {
    // Produção: usar a URL padrão
    apiBaseUrl = "http://localhost:5000/api";
  } else {
    // Fallback para desenvolvimento
    apiBaseUrl = "http://localhost:5000/pms-loc";
  }

  return {
    apiBaseUrl,
    isDevelopment,
    isProduction,
    environment,
  };
}

/**
 * Configuração atual do ambiente
 */
export const env = getEnvironmentConfig();

/**
 * Log da configuração de ambiente (apenas em desenvolvimento)
 */
if (env.isDevelopment) {
  console.log("🌍 Environment Configuration:", {
    apiBaseUrl: env.apiBaseUrl,
    environment: env.environment,
    isDevelopment: env.isDevelopment,
    isProduction: env.isProduction,
  });
}
