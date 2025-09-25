import React, { useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Grid,
  Chip,
  Divider,
} from "@mui/material";
import {
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Add as AddIcon,
  Delete as DeleteIcon,
  LocationOn as LocationIcon,
} from "@mui/icons-material";
import type { ProfileSectionProps, AddressInfo } from "../../../types/profile";

/**
 * Seção de Endereços do perfil
 * Inclui múltiplos endereços com CEP, rua, número, complemento, bairro, cidade, estado, país
 */
export const AddressSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const [formData, setFormData] = useState<AddressInfo[]>(data || []);

  const handleFieldChange = (
    index: number,
    field: keyof AddressInfo,
    value: string
  ) => {
    setFormData((prev) =>
      prev.map((address, i) =>
        i === index ? { ...address, [field]: value } : address
      )
    );
  };

  const handleAddAddress = () => {
    const newAddress: AddressInfo = {
      id: `temp-${Date.now()}`,
      zipCode: "",
      street: "",
      number: "",
      complement: "",
      neighborhood: "",
      city: "",
      state: "",
      country: "Brasil",
      type: "Residential",
    };
    setFormData((prev) => [...prev, newAddress]);
  };

  const handleRemoveAddress = (index: number) => {
    setFormData((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(data || []);
    onCancel();
  };

  const formatZipCode = (value: string) => {
    // Remove tudo que não é dígito
    const numbers = value.replace(/\D/g, "");

    // Aplica a máscara 00000-000
    if (numbers.length <= 5) {
      return numbers;
    } else {
      return `${numbers.slice(0, 5)}-${numbers.slice(5, 8)}`;
    }
  };

  const handleZipCodeChange = (index: number, value: string) => {
    const formatted = formatZipCode(value);
    handleFieldChange(index, "zipCode", formatted);
  };

  const getAddressTypeLabel = (type: string) => {
    switch (type) {
      case "Residential":
        return "Residencial";
      case "Commercial":
        return "Comercial";
      case "Other":
        return "Outro";
      default:
        return type;
    }
  };

  const getAddressTypeColor = (type: string) => {
    switch (type) {
      case "Residential":
        return "primary";
      case "Commercial":
        return "secondary";
      case "Other":
        return "default";
      default:
        return "default";
    }
  };

  const hasAddress =
    data &&
    data.length > 0 &&
    data.some((addr: AddressInfo) => addr.street || addr.city || addr.state);

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
            Endereços
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

        {!hasAddress && !isEditing ? (
          <Box sx={{ textAlign: "center", py: 4 }}>
            <LocationIcon
              sx={{ fontSize: 48, color: "text.secondary", mb: 2 }}
            />
            <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
              Nenhum endereço cadastrado
            </Typography>
            <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={onEdit}
            >
              Adicionar Endereço
            </Button>
          </Box>
        ) : (
          <Box>
            {formData.map((address, index) => (
              <Box key={address.id || index} sx={{ mb: 3 }}>
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    mb: 2,
                  }}
                >
                  <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
                    Endereço {index + 1}
                  </Typography>
                  {isEditing && formData.length > 1 && (
                    <Button
                      variant="outlined"
                      color="error"
                      size="small"
                      startIcon={<DeleteIcon />}
                      onClick={() => handleRemoveAddress(index)}
                    >
                      Remover
                    </Button>
                  )}
                </Box>

                <Grid container spacing={3}>
                  {/* CEP */}
                  <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <TextField
                      fullWidth
                      label="CEP"
                      value={address.zipCode}
                      onChange={(e) =>
                        handleZipCodeChange(index, e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="00000-000"
                      error={!!errors.zipCode}
                      helperText={errors.zipCode || "Digite apenas números"}
                      inputProps={{
                        maxLength: 9,
                      }}
                    />
                  </Grid>

                  {/* Rua */}
                  <Grid size={{ xs: 12, sm: 6, md: 6 }}>
                    <TextField
                      fullWidth
                      label="Rua"
                      value={address.street}
                      onChange={(e) =>
                        handleFieldChange(index, "street", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome da rua"
                      error={!!errors.street}
                      helperText={errors.street}
                    />
                  </Grid>

                  {/* Número */}
                  <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <TextField
                      fullWidth
                      label="Número"
                      value={address.number}
                      onChange={(e) =>
                        handleFieldChange(index, "number", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="123"
                      error={!!errors.number}
                      helperText={errors.number}
                    />
                  </Grid>

                  {/* Complemento */}
                  <Grid size={{ xs: 12, sm: 6, md: 6 }}>
                    <TextField
                      fullWidth
                      label="Complemento"
                      value={address.complement || ""}
                      onChange={(e) =>
                        handleFieldChange(index, "complement", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Apartamento, bloco, etc."
                      error={!!errors.complement}
                      helperText={errors.complement}
                    />
                  </Grid>

                  {/* Bairro */}
                  <Grid size={{ xs: 12, sm: 6, md: 6 }}>
                    <TextField
                      fullWidth
                      label="Bairro"
                      value={address.neighborhood}
                      onChange={(e) =>
                        handleFieldChange(index, "neighborhood", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome do bairro"
                      error={!!errors.neighborhood}
                      helperText={errors.neighborhood}
                    />
                  </Grid>

                  {/* Cidade */}
                  <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <TextField
                      fullWidth
                      label="Cidade"
                      value={address.city}
                      onChange={(e) =>
                        handleFieldChange(index, "city", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome da cidade"
                      error={!!errors.city}
                      helperText={errors.city}
                    />
                  </Grid>

                  {/* Estado */}
                  <Grid size={{ xs: 12, sm: 6, md: 2 }}>
                    <TextField
                      fullWidth
                      label="Estado"
                      value={address.state}
                      onChange={(e) =>
                        handleFieldChange(index, "state", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="SP"
                      error={!!errors.state}
                      helperText={errors.state}
                      inputProps={{
                        maxLength: 2,
                      }}
                    />
                  </Grid>

                  {/* País */}
                  <Grid size={{ xs: 12, sm: 6, md: 6 }}>
                    <TextField
                      fullWidth
                      label="País"
                      value={address.country}
                      onChange={(e) =>
                        handleFieldChange(index, "country", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Brasil"
                      error={!!errors.country}
                      helperText={errors.country}
                    />
                  </Grid>

                  {/* Tipo de Endereço */}
                  <Grid size={{ xs: 12, sm: 6, md: 6 }}>
                    <TextField
                      fullWidth
                      label="Tipo de Endereço"
                      value={address.type}
                      onChange={(e) =>
                        handleFieldChange(index, "type", e.target.value)
                      }
                      disabled={!isEditing}
                      select
                      SelectProps={{
                        native: true,
                      }}
                    >
                      <option value="Residential">Residencial</option>
                      <option value="Commercial">Comercial</option>
                      <option value="Other">Outro</option>
                    </TextField>
                  </Grid>
                </Grid>

                {/* Endereço Completo (Visualização) */}
                {!isEditing &&
                  (address.street || address.city || address.state) && (
                    <Box
                      sx={{
                        p: 2,
                        mt: 2,
                        bgcolor: "grey.50",
                        borderRadius: 1,
                        border: 1,
                        borderColor: "divider",
                      }}
                    >
                      <Typography variant="body2" color="text.secondary">
                        <strong>Endereço completo:</strong>
                        <br />
                        {[
                          address.street &&
                            address.number &&
                            `${address.street}, ${address.number}`,
                          address.complement && `, ${address.complement}`,
                          address.neighborhood && `, ${address.neighborhood}`,
                          address.city &&
                            address.state &&
                            `, ${address.city}/${address.state}`,
                          address.zipCode && `, CEP: ${address.zipCode}`,
                          address.country && `, ${address.country}`,
                        ]
                          .filter(Boolean)
                          .join("")}
                      </Typography>
                      <Chip
                        label={getAddressTypeLabel(address.type)}
                        color={getAddressTypeColor(address.type) as any}
                        size="small"
                        sx={{ mt: 1 }}
                      />
                    </Box>
                  )}

                {index < formData.length - 1 && <Divider sx={{ my: 2 }} />}
              </Box>
            ))}

            {/* Botão para adicionar novo endereço */}
            {isEditing && (
              <Box sx={{ mt: 2, textAlign: "center" }}>
                <Button
                  variant="outlined"
                  startIcon={<AddIcon />}
                  onClick={handleAddAddress}
                  size="small"
                >
                  Adicionar Endereço
                </Button>
              </Box>
            )}
          </Box>
        )}

        {/* Botões de ação */}
        {isEditing && (
          <Box
            sx={{ display: "flex", gap: 2, mt: 3, justifyContent: "flex-end" }}
          >
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
              Salvar
            </Button>
          </Box>
        )}
      </CardContent>
    </Card>
  );
};
