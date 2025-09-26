import { useEffect } from 'react';

/**
 * Hook para gerenciar títulos das páginas seguindo o padrão:
 * <Nome da Página> — Pathfinder Management
 * 
 * @param pageName - Nome da página em português (ex: "Dashboard", "Membros")
 * @param entityName - Nome da entidade (opcional, para páginas de detalhes)
 * @param moduleName - Nome do módulo (opcional, para páginas de detalhes)
 */
export const usePageTitle = (
  pageName: string,
  entityName?: string,
  moduleName?: string
) => {
  useEffect(() => {
    let title: string;

    if (entityName && moduleName) {
      // Para páginas de detalhes: "João Silva — Membros — Pathfinder Management"
      title = `${entityName} — ${moduleName} — Pathfinder Management`;
    } else {
      // Para páginas normais: "Dashboard — Pathfinder Management"
      title = `${pageName} — Pathfinder Management`;
    }

    document.title = title;

    // Cleanup: restaurar título padrão quando o componente for desmontado
    return () => {
      document.title = 'Pathfinder Management';
    };
  }, [pageName, entityName, moduleName]);
};

/**
 * Hook para títulos de páginas de detalhes
 * 
 * @param entityName - Nome da entidade (ex: "João Silva")
 * @param moduleName - Nome do módulo (ex: "Membros")
 */
export const useEntityPageTitle = (entityName: string, moduleName: string) => {
  return usePageTitle('', entityName, moduleName);
};
