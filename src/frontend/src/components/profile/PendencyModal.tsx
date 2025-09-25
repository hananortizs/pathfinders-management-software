import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Typography,
  Button,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Box,
  Chip,
  Divider,
} from "@mui/material";
import {
  Warning as WarningIcon,
  CheckCircle as CheckIcon,
  Close as CloseIcon,
} from "@mui/icons-material";

export interface PendencyItem {
  id: string;
  title: string;
  description?: string;
  severity: "warning" | "error" | "info";
  isCompleted?: boolean;
  actionRequired?: string;
}

export interface PendencyModalProps {
  open: boolean;
  onClose: () => void;
  sectionName: string;
  pendencies: PendencyItem[];
  totalCount: number;
}

/**
 * Modal para exibir pendências específicas de uma seção
 * Mostra lista detalhada de itens pendentes com descrições e ações necessárias
 */
export const PendencyModal: React.FC<PendencyModalProps> = ({
  open,
  onClose,
  sectionName,
  pendencies,
  totalCount,
}) => {
  const getSeverityColor = (severity: string) => {
    switch (severity) {
      case "error":
        return "error";
      case "warning":
        return "warning";
      case "info":
        return "info";
      default:
        return "warning";
    }
  };

  const getSeverityIcon = (severity: string) => {
    switch (severity) {
      case "error":
        return <WarningIcon color="error" />;
      case "warning":
        return <WarningIcon color="warning" />;
      case "info":
        return <WarningIcon color="info" />;
      default:
        return <WarningIcon color="warning" />;
    }
  };

  const completedCount = pendencies.filter((p) => p.isCompleted).length;
  const pendingCount = pendencies.filter((p) => !p.isCompleted).length;

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="sm"
      fullWidth
      PaperProps={{
        sx: {
          borderRadius: 2,
          maxHeight: "80vh",
        },
      }}
    >
      <DialogTitle
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          pb: 1,
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <WarningIcon color="warning" />
          <Typography variant="h6" sx={{ fontWeight: "bold" }}>
            Pendências - {sectionName}
          </Typography>
        </Box>
        <Button
          onClick={onClose}
          size="small"
          sx={{ minWidth: "auto", p: 0.5 }}
        >
          <CloseIcon />
        </Button>
      </DialogTitle>

      <DialogContent sx={{ px: 2, py: 1 }}>
        {/* Resumo */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: 2,
            mb: 2,
            p: 2,
            backgroundColor: "grey.50",
            borderRadius: 1,
          }}
        >
          <Chip
            label={`${totalCount} pendência${totalCount > 1 ? "s" : ""}`}
            color="warning"
            size="small"
            sx={{ fontWeight: "bold" }}
          />
          {completedCount > 0 && (
            <Chip
              label={`${completedCount} concluída${
                completedCount > 1 ? "s" : ""
              }`}
              color="success"
              size="small"
              variant="outlined"
            />
          )}
          <Typography variant="body2" color="text.secondary">
            {pendingCount} ainda pendente{pendingCount > 1 ? "s" : ""}
          </Typography>
        </Box>

        <Divider sx={{ mb: 2 }} />

        {/* Lista de pendências */}
        <List sx={{ p: 0 }}>
          {pendencies.map((pendency, index) => (
            <ListItem
              key={pendency.id}
              sx={{
                px: 0,
                py: 1.5,
                borderBottom:
                  index < pendencies.length - 1 ? "1px solid" : "none",
                borderColor: "divider",
              }}
            >
              <ListItemIcon sx={{ minWidth: 40 }}>
                {pendency.isCompleted ? (
                  <CheckIcon color="success" />
                ) : (
                  getSeverityIcon(pendency.severity)
                )}
              </ListItemIcon>
              <ListItemText
                primary={
                  <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                    <Typography
                      variant="body2"
                      sx={{
                        fontWeight: pendency.isCompleted ? "normal" : "bold",
                        textDecoration: pendency.isCompleted
                          ? "line-through"
                          : "none",
                        color: pendency.isCompleted
                          ? "text.secondary"
                          : "text.primary",
                      }}
                    >
                      {pendency.title}
                    </Typography>
                    {!pendency.isCompleted && (
                      <Chip
                        label={pendency.severity}
                        size="small"
                        color={getSeverityColor(pendency.severity)}
                        variant="outlined"
                        sx={{ fontSize: "0.65rem", height: 20 }}
                      />
                    )}
                  </Box>
                }
                secondary={
                  <Box sx={{ mt: 0.5 }}>
                    {pendency.description && (
                      <Typography
                        variant="caption"
                        color="text.secondary"
                        sx={{ display: "block", mb: 0.5 }}
                      >
                        {pendency.description}
                      </Typography>
                    )}
                    {pendency.actionRequired && !pendency.isCompleted && (
                      <Typography
                        variant="caption"
                        color="primary.main"
                        sx={{
                          display: "block",
                          fontWeight: "medium",
                          fontStyle: "italic",
                        }}
                      >
                        Ação necessária: {pendency.actionRequired}
                      </Typography>
                    )}
                  </Box>
                }
              />
            </ListItem>
          ))}
        </List>

        {pendencies.length === 0 && (
          <Box
            sx={{
              textAlign: "center",
              py: 4,
              color: "text.secondary",
            }}
          >
            <CheckIcon sx={{ fontSize: 48, color: "success.main", mb: 1 }} />
            <Typography variant="body1">
              Nenhuma pendência encontrada!
            </Typography>
            <Typography variant="body2">Esta seção está completa.</Typography>
          </Box>
        )}
      </DialogContent>

      <DialogActions sx={{ p: 2, pt: 1 }}>
        <Button
          onClick={onClose}
          variant="contained"
          fullWidth
          sx={{ borderRadius: 2 }}
        >
          Entendi
        </Button>
      </DialogActions>
    </Dialog>
  );
};
