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
  Lock as LockIcon,
  Info as InfoIcon,
} from "@mui/icons-material";
import type { ProfileSectionProps, ProfileData } from "../../../types/profile";
import { SensitiveDataField } from "../SensitiveDataField";

/**
 * Seção de Dados Pessoais do perfil
 * Inclui nome, data de nascimento, gênero, clube/unidade
 */
export const PersonalDataSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const [formData, setFormData] = useState<Partial<ProfileData>>(data);
  const [sensitiveFields, setSensitiveFields] = useState({
    dateOfBirth: false,
  });

  const handleRevealSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: true }));
  };

  const handleHideSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: false }));
  };

  const handleFieldChange = (field: keyof ProfileData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(data);
    onCancel();
  };

  const calculateAge = (dateOfBirth: string) => {
    const today = new Date();
    const birthDate = new Date(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < birthDate.getDate())
    ) {
      age--;
    }

    return age;
  };

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
          <Typography variant="h6" sx={{ fontWeight: "bold" }}>
            Dados Pessoais
          </Typography>

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

        {isEditing && (
          <Alert severity="info" sx={{ mb: 3 }}>
            Alguns campos como CPF e Data de Nascimento não podem ser alterados
            via interface. Para alterações, contate a secretaria.
          </Alert>
        )}

        <Grid container spacing={3}>
          {/* Nome Completo */}
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              fullWidth
              label="Nome Completo"
              value={`${data.firstName} ${data.middleNames || ""} ${
                data.lastName
              }`.trim()}
              disabled
              InputProps={{
                startAdornment: (
                  <LockIcon sx={{ color: "text.secondary", mr: 1 }} />
                ),
              }}
              helperText="Nome completo não pode ser alterado"
              sx={{
                "& .MuiInputBase-input": { color: "text.secondary" },
                "& .MuiInputBase-root": { backgroundColor: "grey.50" },
              }}
            />
          </Grid>

          {/* Nome Social */}
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              fullWidth
              label="Nome Social"
              value={
                isEditing ? formData.socialName || "" : data.socialName || ""
              }
              onChange={(e) => handleFieldChange("socialName", e.target.value)}
              disabled={!isEditing}
              placeholder="Como prefere ser chamado"
              error={!!errors.socialName}
              helperText={errors.socialName || "Nome social é opcional"}
            />
          </Grid>

          {/* Data de Nascimento (Sensível) */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Data de Nascimento
              </Typography>
              <SensitiveDataField
                value={data.dateOfBirth}
                fieldName="dateOfBirth"
                isRevealed={sensitiveFields.dateOfBirth}
                onReveal={() => handleRevealSensitive("dateOfBirth")}
                onHide={() => handleHideSensitive("dateOfBirth")}
                timeRemaining={sensitiveFields.dateOfBirth ? 10 : 0}
                isEditable={false}
                type="date"
                placeholder="••/••/••••"
              />
              {sensitiveFields.dateOfBirth && (
                <Typography
                  variant="caption"
                  color="text.secondary"
                  sx={{ mt: 1, display: "block" }}
                >
                  Idade: {calculateAge(data.dateOfBirth)} anos
                </Typography>
              )}
            </Box>
          </Grid>

          {/* Gênero */}
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              fullWidth
              label="Gênero"
              value={
                data.gender === "Male"
                  ? "Masculino"
                  : data.gender === "Female"
                  ? "Feminino"
                  : "Outro"
              }
              disabled
              InputProps={{
                startAdornment: (
                  <LockIcon sx={{ color: "text.secondary", mr: 1 }} />
                ),
              }}
              helperText="Gênero não pode ser alterado"
              sx={{
                "& .MuiInputBase-input": { color: "text.secondary" },
                "& .MuiInputBase-root": { backgroundColor: "grey.50" },
              }}
            />
          </Grid>

          {/* Status */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Status do Cadastro
              </Typography>
              <Chip
                label={
                  data.status === "Active"
                    ? "Ativo"
                    : data.status === "Pending"
                    ? "Pendente"
                    : "Arquivado"
                }
                color={
                  data.status === "Active"
                    ? "success"
                    : data.status === "Pending"
                    ? "warning"
                    : "error"
                }
                variant="outlined"
              />
            </Box>
          </Grid>

          {/* Clube/Unidade */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Clube e Unidade
              </Typography>
              <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
                {data.clubName ? (
                  <Chip
                    label={`Clube: ${data.clubName}`}
                    color="primary"
                    variant="outlined"
                    size="small"
                  />
                ) : (
                  <Box sx={{ display: "flex", alignItems: "center", gap: 0.5 }}>
                    <InfoIcon sx={{ fontSize: 16, color: "warning.main" }} />
                    <Typography variant="body2" color="warning.main">
                      Não associado a clube
                    </Typography>
                  </Box>
                )}
                {data.unitName ? (
                  <Chip
                    label={`Unidade: ${data.unitName}`}
                    color="secondary"
                    variant="outlined"
                    size="small"
                  />
                ) : (
                  <Box sx={{ display: "flex", alignItems: "center", gap: 0.5 }}>
                    <InfoIcon sx={{ fontSize: 16, color: "warning.main" }} />
                    <Typography variant="body2" color="warning.main">
                      Não associado a unidade
                    </Typography>
                  </Box>
                )}
              </Box>
            </Box>
          </Grid>

          {/* Cargos */}
          <Grid size={{ xs: 12 }}>
            <Box>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Cargos Ativos
              </Typography>
              <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1 }}>
                {data.roles && data.roles.length > 0 ? (
                  data.roles.map((role: string, index: number) => (
                    <Chip
                      key={index}
                      label={role}
                      color="success"
                      variant="outlined"
                      size="small"
                    />
                  ))
                ) : (
                  <Box sx={{ display: "flex", alignItems: "center", gap: 0.5 }}>
                    <InfoIcon sx={{ fontSize: 16, color: "warning.main" }} />
                    <Typography variant="body2" color="warning.main">
                      Nenhum cargo ativo
                    </Typography>
                  </Box>
                )}
              </Box>
            </Box>
          </Grid>
        </Grid>

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
