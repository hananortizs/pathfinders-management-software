import React, { useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Stack,
  Grid,
  Chip,
  Divider,
  Alert,
} from "@mui/material";
import {
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Description as DocumentIcon,
  Warning as WarningIcon,
} from "@mui/icons-material";
import type { ProfileSectionProps, DocumentInfo } from "../../../types/profile";
import { SensitiveDataField } from "../SensitiveDataField";

/**
 * Seção de Documentos do perfil
 * Inclui CPF, RG e outros documentos (sensível)
 */
export const DocumentsSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const [formData, setFormData] = useState<DocumentInfo>(
    data || {
      id: "1",
      cpf: "",
      rg: "",
      rgIssuer: "",
      rgIssueDate: "",
      passport: "",
      passportIssueDate: "",
      passportExpiryDate: "",
      voterId: "",
      workPermit: "",
      otherDocuments: [],
    }
  );

  const [sensitiveFields, setSensitiveFields] = useState({
    cpf: false,
    rg: false,
    passport: false,
    voterId: false,
    workPermit: false,
  });

  const handleRevealSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: true }));
  };

  const handleHideSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: false }));
  };

  const handleFieldChange = (field: keyof DocumentInfo, value: string | string[]) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(
      data || {
        cpf: "",
        rg: "",
        rgIssuer: "",
        rgIssueDate: "",
      }
    );
    onCancel();
  };

  const formatRG = (value: string) => {
    // Remove tudo que não é dígito
    const numbers = value.replace(/\D/g, "");

    // Aplica a máscara 00.000.000-0
    if (numbers.length <= 2) {
      return numbers;
    } else if (numbers.length <= 5) {
      return `${numbers.slice(0, 2)}.${numbers.slice(2)}`;
    } else if (numbers.length <= 8) {
      return `${numbers.slice(0, 2)}.${numbers.slice(2, 5)}.${numbers.slice(
        5
      )}`;
    } else {
      return `${numbers.slice(0, 2)}.${numbers.slice(2, 5)}.${numbers.slice(
        5,
        8
      )}-${numbers.slice(8, 9)}`;
    }
  };

  const handleRGChange = (value: string) => {
    const formatted = formatRG(value);
    handleFieldChange("rg", formatted);
  };

  const hasDocuments = data && (data.cpf || data.rg);

  return (
    <Card>
      <CardContent>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            mb: 3,
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <DocumentIcon color="primary" />
            <Typography variant="h6" sx={{ fontWeight: "bold" }}>
              Documentos
            </Typography>
            <Chip
              label="Sensível"
              color="warning"
              size="small"
              variant="outlined"
            />
          </Box>

          {!isEditing && (
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={onEdit}
              size="small"
            >
              Editar
            </Button>
          )}
        </Box>

        <Alert severity="warning" sx={{ mb: 3 }}>
          <Typography variant="body2">
            <strong>Atenção:</strong> CPF e Data de Nascimento não podem ser
            alterados via interface. Para alterações, contate a secretaria. Os
            dados são exibidos com mascaramento por segurança.
          </Typography>
        </Alert>

        {!hasDocuments && !isEditing ? (
          <Box sx={{ textAlign: "center", py: 4 }}>
            <DocumentIcon
              sx={{ fontSize: 48, color: "text.secondary", mb: 2 }}
            />
            <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
              Nenhum documento cadastrado
            </Typography>
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={onEdit}
            >
              Adicionar Documentos
            </Button>
          </Box>
        ) : (
          <Grid container spacing={3}>
            {/* CPF */}
            <Grid size={{ xs: 12, md: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  CPF
                </Typography>
                <SensitiveDataField
                  value={formData.cpf}
                  fieldName="cpf"
                  isRevealed={sensitiveFields.cpf}
                  onReveal={() => handleRevealSensitive("cpf")}
                  onHide={() => handleHideSensitive("cpf")}
                  timeRemaining={sensitiveFields.cpf ? 10 : 0}
                  isEditable={false}
                  type="text"
                  mask="000.000.000-00"
                  placeholder="•••.•••.•••-••"
                />
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: 0.5,
                    mt: 0.5,
                  }}
                >
                  <WarningIcon color="warning" sx={{ fontSize: "0.875rem" }} />
                  <Typography
                    variant="caption"
                    color="text.secondary"
                    sx={{ fontSize: "0.75rem" }}
                  >
                    Para alterar CPF, contate a secretaria
                  </Typography>
                </Box>
              </Box>
            </Grid>

            {/* RG */}
            <Grid size={{ xs: 12, md: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  RG
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    value={formData.rg || ""}
                    onChange={(e) => handleRGChange(e.target.value)}
                    placeholder="00.000.000-0"
                    error={!!errors.rg}
                    helperText={errors.rg || "Opcional"}
                    inputProps={{
                      maxLength: 12,
                    }}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.rg || ""}
                    fieldName="rg"
                    isRevealed={sensitiveFields.rg}
                    onReveal={() => handleRevealSensitive("rg")}
                    onHide={() => handleHideSensitive("rg")}
                    timeRemaining={sensitiveFields.rg ? 10 : 0}
                    isEditable={false}
                    type="text"
                    mask="00.000.000-0"
                    placeholder="••.•••.•••-•"
                  />
                )}
              </Box>
            </Grid>

            {/* Órgão Emissor do RG */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Órgão Emissor"
                value={formData.rgIssuer || ""}
                onChange={(e) => handleFieldChange("rgIssuer", e.target.value)}
                disabled={!isEditing}
                placeholder="SSP, DETRAN, etc."
                error={!!errors.rgIssuer}
                helperText={errors.rgIssuer || "Opcional"}
              />
            </Grid>

            {/* Data de Emissão do RG */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Data de Emissão"
                type="date"
                value={formData.rgIssueDate || ""}
                onChange={(e) =>
                  handleFieldChange("rgIssueDate", e.target.value)
                }
                disabled={!isEditing}
                InputLabelProps={{
                  shrink: true,
                }}
                error={!!errors.rgIssueDate}
                helperText={errors.rgIssueDate || "Opcional"}
              />
            </Grid>

            {/* Passaporte */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Passaporte"
                value={formData.passport || ""}
                onChange={(e) => handleFieldChange("passport", e.target.value)}
                disabled={!isEditing}
                placeholder="BR123456789"
                error={!!errors.passport}
                helperText={errors.passport || "Opcional"}
                inputProps={{ maxLength: 20 }}
              />
            </Grid>

            {/* Data de Emissão do Passaporte */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Data de Emissão do Passaporte"
                type="date"
                value={formData.passportIssueDate || ""}
                onChange={(e) =>
                  handleFieldChange("passportIssueDate", e.target.value)
                }
                disabled={!isEditing}
                InputLabelProps={{ shrink: true }}
                error={!!errors.passportIssueDate}
                helperText={errors.passportIssueDate || "Opcional"}
              />
            </Grid>

            {/* Data de Vencimento do Passaporte */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Data de Vencimento do Passaporte"
                type="date"
                value={formData.passportExpiryDate || ""}
                onChange={(e) =>
                  handleFieldChange("passportExpiryDate", e.target.value)
                }
                disabled={!isEditing}
                InputLabelProps={{ shrink: true }}
                error={!!errors.passportExpiryDate}
                helperText={errors.passportExpiryDate || "Opcional"}
              />
            </Grid>

            {/* Título de Eleitor */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Título de Eleitor"
                value={formData.voterId || ""}
                onChange={(e) => handleFieldChange("voterId", e.target.value)}
                disabled={!isEditing}
                placeholder="000000000000"
                error={!!errors.voterId}
                helperText={errors.voterId || "Opcional"}
                inputProps={{ maxLength: 12 }}
              />
            </Grid>

            {/* Carteira de Trabalho */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Carteira de Trabalho"
                value={formData.workPermit || ""}
                onChange={(e) =>
                  handleFieldChange("workPermit", e.target.value)
                }
                disabled={!isEditing}
                placeholder="WP-2024-001"
                error={!!errors.workPermit}
                helperText={errors.workPermit || "Opcional"}
                inputProps={{ maxLength: 20 }}
              />
            </Grid>

            {/* Outros Documentos */}
            <Grid size={{ xs: 12 }}>
              <TextField
                fullWidth
                label="Outros Documentos"
                multiline
                rows={2}
                value={formData.otherDocuments?.join(", ") || ""}
                onChange={(e) => {
                  const values = e.target.value.split(", ").filter(Boolean);
                  handleFieldChange("otherDocuments", values);
                }}
                disabled={!isEditing}
                placeholder="CNH, Título de Eleitor, etc."
                error={!!errors.otherDocuments}
                helperText={
                  errors.otherDocuments || "Opcional - Separe por vírgula"
                }
              />
            </Grid>

            {/* Informações sobre documentos */}
            <Grid size={{ xs: 12 }}>
              <Alert severity="info" sx={{ mt: 2 }}>
                <Typography variant="body2">
                  <strong>Informações sobre documentos:</strong>
                </Typography>
                <ul style={{ margin: "8px 0", paddingLeft: "20px" }}>
                  <li>
                    O CPF é obrigatório e não pode ser alterado via interface
                  </li>
                  <li>O RG é opcional mas recomendado para identificação</li>
                  <li>
                    Para alterar CPF ou outros documentos, contate a secretaria
                  </li>
                  <li>Os dados são exibidos com mascaramento por segurança</li>
                </ul>
              </Alert>
            </Grid>
          </Grid>
        )}

        {/* Botões de ação */}
        {isEditing && (
          <>
            <Divider sx={{ my: 3 }} />
            <Stack direction="row" spacing={2} justifyContent="flex-end">
              <Button
                variant="outlined"
                startIcon={<CancelIcon />}
                onClick={handleCancel}
                disabled={isLoading}
              >
                Cancelar
              </Button>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={isLoading}
              >
                {isLoading ? "Salvando..." : "Salvar"}
              </Button>
            </Stack>
          </>
        )}
      </CardContent>
    </Card>
  );
};
