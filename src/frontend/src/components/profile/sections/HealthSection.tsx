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
  HealthAndSafety as HealthIcon,
  Emergency as EmergencyIcon,
} from "@mui/icons-material";
import type { ProfileSectionProps, MedicalInfo } from "../../../types/profile";
import { SensitiveDataField } from "../SensitiveDataField";
import { PhoneInputWithDDI } from "../../common/PhoneInputWithDDI";

/**
 * Seção de Saúde do perfil
 * Inclui dados médicos e contato de emergência (sensível)
 */
export const HealthSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const [formData, setFormData] = useState<MedicalInfo>(
    data || {
      id: "",
      allergies: "",
      medications: "",
      bloodType: "",
      medicalConditions: "",
      emergencyContactName: "",
      emergencyContactPhone: "",
      emergencyContactRelationship: "",
      notes: "",
    }
  );

  const [sensitiveFields, setSensitiveFields] = useState({
    allergies: false,
    medications: false,
    medicalConditions: false,
    emergencyContactName: false,
    emergencyContactPhone: false,
    notes: false,
  });

  const handleRevealSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: true }));
  };

  const handleHideSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: false }));
  };

  const handleFieldChange = (field: keyof MedicalInfo, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(
      data || {
        id: "",
        allergies: "",
        medications: "",
        bloodType: "",
        medicalConditions: "",
        emergencyContactName: "",
        emergencyContactPhone: "",
        emergencyContactRelationship: "",
        notes: "",
      }
    );
    onCancel();
  };

  const hasHealthData =
    data &&
    (data.allergies ||
      data.medications ||
      data.bloodType ||
      data.medicalConditions ||
      data.emergencyContactName ||
      data.emergencyContactPhone);

  const getBloodTypeColor = (bloodType: string) => {
    switch (bloodType?.toUpperCase()) {
      case "A+":
      case "A-":
        return "error";
      case "B+":
      case "B-":
        return "warning";
      case "AB+":
      case "AB-":
        return "info";
      case "O+":
      case "O-":
        return "success";
      default:
        return "default";
    }
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
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <HealthIcon color="primary" />
            <Typography variant="h6" sx={{ fontWeight: "bold" }}>
              Dados de Saúde
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
            <strong>Atenção:</strong> Os dados de saúde são informações
            sensíveis e são exibidos com mascaramento. Use o botão "Revelar por
            10s" para visualizar os dados quando necessário.
          </Typography>
        </Alert>

        {!hasHealthData && !isEditing ? (
          <Box sx={{ textAlign: "center", py: 4 }}>
            <HealthIcon sx={{ fontSize: 48, color: "text.secondary", mb: 2 }} />
            <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
              Nenhum dado de saúde cadastrado
            </Typography>
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={onEdit}
            >
              Adicionar Dados de Saúde
            </Button>
          </Box>
        ) : (
          <Grid container spacing={3}>
            {/* Tipo Sanguíneo */}
            <Grid size={{ xs: 12, sm: 6, md: 3 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Tipo Sanguíneo
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    select
                    value={formData.bloodType || ""}
                    onChange={(e) =>
                      handleFieldChange("bloodType", e.target.value)
                    }
                    SelectProps={{
                      native: true,
                    }}
                  >
                    <option value="">Selecione</option>
                    <option value="A+">A+</option>
                    <option value="A-">A-</option>
                    <option value="B+">B+</option>
                    <option value="B-">B-</option>
                    <option value="AB+">AB+</option>
                    <option value="AB-">AB-</option>
                    <option value="O+">O+</option>
                    <option value="O-">O-</option>
                  </TextField>
                ) : (
                  <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                    {formData.bloodType ? (
                      <Chip
                        label={formData.bloodType}
                        color={getBloodTypeColor(formData.bloodType) as any}
                        variant="outlined"
                      />
                    ) : (
                      <Typography variant="body2" color="text.secondary">
                        Não informado
                      </Typography>
                    )}
                  </Box>
                )}
              </Box>
            </Grid>

            {/* Alergias */}
            <Grid size={{ xs: 12, md: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Alergias
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    multiline
                    rows={2}
                    value={formData.allergies || ""}
                    onChange={(e) =>
                      handleFieldChange("allergies", e.target.value)
                    }
                    placeholder="Liste suas alergias conhecidas"
                    error={!!errors.allergies}
                    helperText={errors.allergies || "Opcional"}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.allergies || ""}
                    fieldName="allergies"
                    isRevealed={sensitiveFields.allergies}
                    onReveal={() => handleRevealSensitive("allergies")}
                    onHide={() => handleHideSensitive("allergies")}
                    timeRemaining={sensitiveFields.allergies ? 10 : 0}
                    isEditable={false}
                    placeholder="Dados mascarados"
                  />
                )}
              </Box>
            </Grid>

            {/* Medicações */}
            <Grid size={{ xs: 12, md: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Medicações
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    multiline
                    rows={2}
                    value={formData.medications || ""}
                    onChange={(e) =>
                      handleFieldChange("medications", e.target.value)
                    }
                    placeholder="Medicações em uso regular"
                    error={!!errors.medications}
                    helperText={errors.medications || "Opcional"}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.medications || ""}
                    fieldName="medications"
                    isRevealed={sensitiveFields.medications}
                    onReveal={() => handleRevealSensitive("medications")}
                    onHide={() => handleHideSensitive("medications")}
                    timeRemaining={sensitiveFields.medications ? 10 : 0}
                    isEditable={false}
                    placeholder="Dados mascarados"
                  />
                )}
              </Box>
            </Grid>

            {/* Condições Médicas */}
            <Grid size={{ xs: 12 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Condições Médicas
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    multiline
                    rows={2}
                    value={formData.medicalConditions || ""}
                    onChange={(e) =>
                      handleFieldChange("medicalConditions", e.target.value)
                    }
                    placeholder="Condições médicas relevantes"
                    error={!!errors.medicalConditions}
                    helperText={errors.medicalConditions || "Opcional"}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.medicalConditions || ""}
                    fieldName="medicalConditions"
                    isRevealed={sensitiveFields.medicalConditions}
                    onReveal={() => handleRevealSensitive("medicalConditions")}
                    onHide={() => handleHideSensitive("medicalConditions")}
                    timeRemaining={sensitiveFields.medicalConditions ? 10 : 0}
                    isEditable={false}
                    placeholder="Dados mascarados"
                  />
                )}
              </Box>
            </Grid>

            <Divider sx={{ my: 2 }} />

            {/* Contato de Emergência */}
            <Grid size={{ xs: 12 }}>
              <Box
                sx={{ display: "flex", alignItems: "center", gap: 1, mb: 2 }}
              >
                <EmergencyIcon color="error" />
                <Typography variant="h6" sx={{ fontWeight: "bold" }}>
                  Contato de Emergência
                </Typography>
              </Box>
            </Grid>

            {/* Nome do Contato de Emergência */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Nome do Contato
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    value={formData.emergencyContactName || ""}
                    onChange={(e) =>
                      handleFieldChange("emergencyContactName", e.target.value)
                    }
                    placeholder="Nome completo do contato"
                    error={!!errors.emergencyContactName}
                    helperText={errors.emergencyContactName}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.emergencyContactName || ""}
                    fieldName="emergencyContactName"
                    isRevealed={sensitiveFields.emergencyContactName}
                    onReveal={() =>
                      handleRevealSensitive("emergencyContactName")
                    }
                    onHide={() => handleHideSensitive("emergencyContactName")}
                    timeRemaining={
                      sensitiveFields.emergencyContactName ? 10 : 0
                    }
                    isEditable={false}
                    placeholder="Dados mascarados"
                  />
                )}
              </Box>
            </Grid>

            {/* Telefone do Contato de Emergência */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Telefone do Contato
                </Typography>
                {isEditing ? (
                  <PhoneInputWithDDI
                    fullWidth
                    value={formData.emergencyContactPhone || ""}
                    onChange={(value) =>
                      handleFieldChange("emergencyContactPhone", value)
                    }
                    error={!!errors.emergencyContactPhone}
                    helperText={errors.emergencyContactPhone}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.emergencyContactPhone || ""}
                    fieldName="emergencyContactPhone"
                    isRevealed={sensitiveFields.emergencyContactPhone}
                    onReveal={() =>
                      handleRevealSensitive("emergencyContactPhone")
                    }
                    onHide={() => handleHideSensitive("emergencyContactPhone")}
                    timeRemaining={
                      sensitiveFields.emergencyContactPhone ? 10 : 0
                    }
                    isEditable={false}
                    type="tel"
                    placeholder="••••••••••"
                  />
                )}
              </Box>
            </Grid>

            {/* Relacionamento */}
            <Grid size={{ xs: 12, sm: 6 }}>
              <TextField
                fullWidth
                label="Relacionamento"
                value={formData.emergencyContactRelationship || ""}
                onChange={(e) =>
                  handleFieldChange(
                    "emergencyContactRelationship",
                    e.target.value
                  )
                }
                disabled={!isEditing}
                placeholder="Pai, mãe, cônjuge, etc."
                error={!!errors.emergencyContactRelationship}
                helperText={errors.emergencyContactRelationship || "Opcional"}
              />
            </Grid>

            {/* Observações */}
            <Grid size={{ xs: 12 }}>
              <Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mb: 1 }}
                >
                  Observações Médicas
                </Typography>
                {isEditing ? (
                  <TextField
                    fullWidth
                    multiline
                    rows={3}
                    value={formData.notes || ""}
                    onChange={(e) => handleFieldChange("notes", e.target.value)}
                    placeholder="Outras informações médicas relevantes"
                    error={!!errors.notes}
                    helperText={errors.notes || "Opcional"}
                  />
                ) : (
                  <SensitiveDataField
                    value={formData.notes || ""}
                    fieldName="notes"
                    isRevealed={sensitiveFields.notes}
                    onReveal={() => handleRevealSensitive("notes")}
                    onHide={() => handleHideSensitive("notes")}
                    timeRemaining={sensitiveFields.notes ? 10 : 0}
                    isEditable={false}
                    placeholder="Dados mascarados"
                  />
                )}
              </Box>
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
