/**
 * Página principal de gerenciamento de membros
 * Implementa agrupamento hierárquico para Administrador
 */

import React, { useState, useCallback } from "react";
import {
  Box,
  Typography,
  Card,
  CardContent,
  Button,
  IconButton,
  Toolbar,
  Chip,
  Menu,
  MenuItem,
  Divider,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert,
  Snackbar,
  LinearProgress,
  Tooltip,
} from "@mui/material";
import {
  Refresh,
  MoreVert,
  ViewList,
  ViewModule,
  GroupWork,
  PersonAdd,
  FileDownload,
  Delete,
} from "@mui/icons-material";
import { useMembersList } from "../hooks/useMembersList";
import { MemberFiltersComponent as MemberFilters } from "../components/members/MemberFilters";
import { MembersList } from "../components/members/MembersList";
import { UserLevel } from "../types/members";

const MembersPage: React.FC = () => {
  const [groupingStrategy, setGroupingStrategy] = useState<
    "hierarchical" | "flat" | "by_club" | "by_unit"
  >("hierarchical");
  const [viewMode, setViewMode] = useState<"list" | "grid">("list");
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const [bulkActionMenu, setBulkActionMenu] = useState<HTMLElement | null>(
    null
  );
  const [confirmDialog, setConfirmDialog] = useState<{
    open: boolean;
    title: string;
    message: string;
    onConfirm: () => void;
  }>({
    open: false,
    title: "",
    message: "",
    onConfirm: () => {},
  });
  const [snackbar, setSnackbar] = useState<{
    open: boolean;
    message: string;
    severity: "success" | "error" | "warning" | "info";
  }>({
    open: false,
    message: "",
    severity: "success",
  });

  // Hook para gerenciar lista de membros
  const {
    members,
    groups,
    selectedMembers,
    isLoading,
    error,
    currentPage,
    totalPages,
    totalCount,
    hasNextPage,
    hasPreviousPage,
    filters,
    setFilters,
    clearFilters,
    selectMember,
    selectAllMembers,
    clearSelection,
    refresh,
    nextPage,
    previousPage,
    stats,
  } = useMembersList({
    userLevel: UserLevel.Admin, // Admin para MVP0
    groupingStrategy,
    pageSize: 20,
    autoFetch: true,
  });

  // Handlers
  const handleMemberAction = useCallback((action: string, memberId: string) => {
    switch (action) {
      case "view":
        // Implementar visualização de membro
        setSnackbar({
          open: true,
          message: `Visualizando membro ${memberId}`,
          severity: "info",
        });
        break;
      case "edit":
        // Implementar edição de membro
        setSnackbar({
          open: true,
          message: `Editando membro ${memberId}`,
          severity: "info",
        });
        break;
      case "transfer":
        // Implementar transferência de membro
        setSnackbar({
          open: true,
          message: `Transferindo membro ${memberId}`,
          severity: "info",
        });
        break;
      case "delete":
        setConfirmDialog({
          open: true,
          title: "Confirmar Exclusão",
          message:
            "Tem certeza que deseja excluir este membro? Esta ação não pode ser desfeita.",
          onConfirm: () => {
            // Implementar exclusão
            setSnackbar({
              open: true,
              message: "Membro excluído com sucesso",
              severity: "success",
            });
            setConfirmDialog((prev) => ({ ...prev, open: false }));
          },
        });
        break;
    }
  }, []);

  const handleBulkAction = useCallback(
    (action: string) => {
      if (selectedMembers.length === 0) {
        setSnackbar({
          open: true,
          message: "Selecione pelo menos um membro",
          severity: "warning",
        });
        return;
      }

      switch (action) {
        case "export":
          setSnackbar({
            open: true,
            message: `Exportando ${selectedMembers.length} membros`,
            severity: "info",
          });
          break;
        case "delete":
          setConfirmDialog({
            open: true,
            title: "Confirmar Exclusão em Massa",
            message: `Tem certeza que deseja excluir ${selectedMembers.length} membros? Esta ação não pode ser desfeita.`,
            onConfirm: () => {
              setSnackbar({
                open: true,
                message: `${selectedMembers.length} membros excluídos com sucesso`,
                severity: "success",
              });
              clearSelection();
              setConfirmDialog((prev) => ({ ...prev, open: false }));
            },
          });
          break;
      }
      setBulkActionMenu(null);
    },
    [selectedMembers, clearSelection]
  );

  const handleCreateMember = useCallback(() => {
    setSnackbar({
      open: true,
      message: "Abrindo formulário de criação de membro",
      severity: "info",
    });
  }, []);

  const handleExport = useCallback(() => {
    setSnackbar({
      open: true,
      message: "Exportando dados dos membros",
      severity: "info",
    });
  }, []);

  const handleRefresh = useCallback(() => {
    refresh();
    setSnackbar({
      open: true,
      message: "Lista atualizada com sucesso",
      severity: "success",
    });
  }, [refresh]);

  const getGroupingStrategyLabel = (strategy: string) => {
    switch (strategy) {
      case "hierarchical":
        return "Hierárquico";
      case "flat":
        return "Lista Simples";
      case "byClub":
        return "Por Clube";
      case "byUnit":
        return "Por Unidade";
      default:
        return strategy;
    }
  };

  return (
    <Box sx={{ width: "100%", minHeight: "100vh", bgcolor: "grey.50", p: 3 }}>
      {/* Header */}
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Gerenciamento de Membros
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Gerencie membros com agrupamento hierárquico por Divisão → União →
          Região → Associação → Distrito → Clube
        </Typography>
      </Box>

      {/* Estatísticas */}
      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
          gap: 2,
          mb: 3,
        }}
      >
        <Card>
          <CardContent sx={{ textAlign: "center" }}>
            <Typography variant="h4" color="primary.main">
              {stats.total}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Total de Membros
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent sx={{ textAlign: "center" }}>
            <Typography variant="h4" color="success.main">
              {stats.active}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Ativos
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent sx={{ textAlign: "center" }}>
            <Typography variant="h4" color="warning.main">
              {stats.pending}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Pendentes
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent sx={{ textAlign: "center" }}>
            <Typography variant="h4" color="error.main">
              {stats.inactive}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Inativos
            </Typography>
          </CardContent>
        </Card>
      </Box>

      {/* Filtros */}
      <MemberFilters
        filters={filters}
        onFiltersChange={setFilters}
        onClearFilters={clearFilters}
        userLevel={UserLevel.Admin}
        loading={isLoading}
      />

      {/* Toolbar */}
      <Card sx={{ mb: 2 }}>
        <Toolbar sx={{ px: 2 }}>
          <Box
            sx={{ display: "flex", alignItems: "center", gap: 2, flexGrow: 1 }}
          >
            <Typography variant="h6">Membros ({totalCount})</Typography>

            {selectedMembers.length > 0 && (
              <Chip
                label={`${selectedMembers.length} selecionado(s)`}
                color="primary"
                onDelete={clearSelection}
              />
            )}
          </Box>

          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            {/* Estratégia de agrupamento */}
            <Tooltip title="Estratégia de agrupamento">
              <Button
                startIcon={<GroupWork />}
                onClick={(e) => setAnchorEl(e.currentTarget)}
                size="small"
              >
                {getGroupingStrategyLabel(groupingStrategy)}
              </Button>
            </Tooltip>

            {/* Modo de visualização */}
            <Tooltip title="Modo de visualização">
              <Button
                startIcon={viewMode === "list" ? <ViewList /> : <ViewModule />}
                onClick={() =>
                  setViewMode(viewMode === "list" ? "grid" : "list")
                }
                size="small"
              >
                {viewMode === "list" ? "Lista" : "Grade"}
              </Button>
            </Tooltip>

            {/* Ações em massa */}
            {selectedMembers.length > 0 && (
              <Button
                startIcon={<MoreVert />}
                onClick={(e) => setBulkActionMenu(e.currentTarget)}
                size="small"
              >
                Ações
              </Button>
            )}

            {/* Ações individuais */}
            <Button
              startIcon={<PersonAdd />}
              onClick={handleCreateMember}
              variant="contained"
              size="small"
            >
              Novo Membro
            </Button>

            <Button
              startIcon={<FileDownload />}
              onClick={handleExport}
              size="small"
            >
              Exportar
            </Button>

            <Tooltip title="Atualizar">
              <span>
                <IconButton onClick={handleRefresh} disabled={isLoading}>
                  <Refresh />
                </IconButton>
              </span>
            </Tooltip>
          </Box>
        </Toolbar>
      </Card>

      {/* Lista de membros */}
      {isLoading && <LinearProgress sx={{ mb: 2 }} />}

      <MembersList
        members={members}
        groups={groups}
        selectedMembers={selectedMembers}
        onSelectMember={selectMember}
        onSelectAll={selectAllMembers}
        onMemberAction={handleMemberAction}
        userLevel={UserLevel.Admin}
        groupingStrategy={groupingStrategy}
        loading={isLoading}
        error={error?.message}
      />

      {/* Paginação */}
      {totalPages > 1 && (
        <Card sx={{ mt: 2 }}>
          <CardContent
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: 2,
            }}
          >
            <Button
              onClick={previousPage}
              disabled={!hasPreviousPage || isLoading}
              size="small"
            >
              Anterior
            </Button>

            <Typography variant="body2" color="text.secondary">
              Página {currentPage} de {totalPages}
            </Typography>

            <Button
              onClick={nextPage}
              disabled={!hasNextPage || isLoading}
              size="small"
            >
              Próxima
            </Button>
          </CardContent>
        </Card>
      )}

      {/* Menu de estratégia de agrupamento */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={() => setAnchorEl(null)}
      >
        <MenuItem
          onClick={() => {
            setGroupingStrategy("hierarchical");
            setAnchorEl(null);
          }}
        >
          Hierárquico
        </MenuItem>
        <MenuItem
          onClick={() => {
            setGroupingStrategy("by_club");
            setAnchorEl(null);
          }}
        >
          Por Clube
        </MenuItem>
        <MenuItem
          onClick={() => {
            setGroupingStrategy("by_unit");
            setAnchorEl(null);
          }}
        >
          Por Unidade
        </MenuItem>
        <MenuItem
          onClick={() => {
            setGroupingStrategy("flat");
            setAnchorEl(null);
          }}
        >
          Lista Simples
        </MenuItem>
      </Menu>

      {/* Menu de ações em massa */}
      <Menu
        anchorEl={bulkActionMenu}
        open={Boolean(bulkActionMenu)}
        onClose={() => setBulkActionMenu(null)}
      >
        <MenuItem onClick={() => handleBulkAction("export")}>
          <FileDownload sx={{ mr: 1 }} />
          Exportar Selecionados
        </MenuItem>
        <Divider />
        <MenuItem
          onClick={() => handleBulkAction("delete")}
          sx={{ color: "error.main" }}
        >
          <Delete sx={{ mr: 1 }} />
          Excluir Selecionados
        </MenuItem>
      </Menu>

      {/* Dialog de confirmação */}
      <Dialog
        open={confirmDialog.open}
        onClose={() => setConfirmDialog((prev) => ({ ...prev, open: false }))}
      >
        <DialogTitle>{confirmDialog.title}</DialogTitle>
        <DialogContent>
          <Typography>{confirmDialog.message}</Typography>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() =>
              setConfirmDialog((prev) => ({ ...prev, open: false }))
            }
          >
            Cancelar
          </Button>
          <Button
            onClick={confirmDialog.onConfirm}
            color="error"
            variant="contained"
          >
            Confirmar
          </Button>
        </DialogActions>
      </Dialog>

      {/* Snackbar de feedback */}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar((prev) => ({ ...prev, open: false }))}
      >
        <Alert
          onClose={() => setSnackbar((prev) => ({ ...prev, open: false }))}
          severity={snackbar.severity}
          sx={{ width: "100%" }}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default MembersPage;
