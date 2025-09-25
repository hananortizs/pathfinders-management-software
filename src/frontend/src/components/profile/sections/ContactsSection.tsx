import React, { useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Stack,
  Chip,
  Divider,
  List,
  ListItem,
  Alert,
  useMediaQuery,
  useTheme,
} from "@mui/material";
import {
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Add as AddIcon,
  Delete as DeleteIcon,
  CheckCircle as VerifiedIcon,
  Cancel as UnverifiedIcon,
  Star as PrimaryIcon,
  StarBorder as NotPrimaryIcon,
} from "@mui/icons-material";
import type { ProfileSectionProps, ContactInfo } from "../../../types/profile";
import { SensitiveDataField } from "../SensitiveDataField";
import { PhoneInputWithDDI } from "../../common/PhoneInputWithDDI";
import { ResponsiveTextField } from "../../common/ResponsiveTextField";
import { ResponsiveButton } from "../../common/ResponsiveButton";

/**
 * Seção de Contatos do perfil
 * Inclui e-mail, telefone e outros contatos com verificação
 */
export const ContactsSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const [formData, setFormData] = useState<ContactInfo[]>(data || []);
  const [sensitiveFields, setSensitiveFields] = useState<
    Record<string, boolean>
  >({});

  const handleRevealSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: true }));
  };

  const handleHideSensitive = (fieldName: string) => {
    setSensitiveFields((prev) => ({ ...prev, [fieldName]: false }));
  };

  const handleContactChange = (
    index: number,
    field: keyof ContactInfo,
    value: string | boolean
  ) => {
    setFormData((prev) =>
      prev.map((contact, i) => {
        if (i === index) {
          return { ...contact, [field]: value };
        }
        return contact;
      })
    );
  };

  const handleAddContact = () => {
    const newContact: ContactInfo = {
      id: `temp-${Date.now()}`,
      type: "Email",
      value: "",
      isPrimary: false,
      isVerified: false,
      category: "Personal",
    };
    setFormData((prev) => [...prev, newContact]);
  };

  const handleRemoveContact = (index: number) => {
    setFormData((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSetPrimary = (index: number) => {
    setFormData((prev) => {
      const targetContact = prev[index];
      const contactType = targetContact.type;

      return prev.map((contact, i) => {
        if (i === index) {
          // Marca o contato selecionado como primário
          return { ...contact, isPrimary: true };
        } else if (contact.type === contactType) {
          // Remove o status primário de outros contatos do mesmo tipo
          return { ...contact, isPrimary: false };
        }
        // Mantém o status primário de contatos de tipos diferentes
        return contact;
      });
    });
  };

  const canBePrimary = (index: number) => {
    const targetContact = formData[index];
    const contactType = targetContact.type;

    // Verifica se já existe outro contato do mesmo tipo marcado como primário
    const hasOtherPrimaryOfSameType = formData.some(
      (contact, i) =>
        i !== index && contact.type === contactType && contact.isPrimary
    );

    return !hasOtherPrimaryOfSameType;
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(data || []);
    onCancel();
  };

  const getContactTypeLabel = (type: string) => {
    switch (type) {
      case "Email":
        return "E-mail";
      case "Phone":
        return "Telefone";
      case "WhatsApp":
        return "WhatsApp";
      default:
        return type;
    }
  };

  const getContactTypeIcon = (type: string) => {
    switch (type) {
      case "Email":
        return "📧";
      case "Phone":
        return "📞";
      case "WhatsApp":
        return "💬";
      default:
        return "📱";
    }
  };

  const getCategoryLabel = (category: string) => {
    switch (category) {
      case "Personal":
        return "Pessoal";
      case "Emergency":
        return "Emergência";
      case "Work":
        return "Trabalho";
      default:
        return category;
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
          <Typography variant="h6" sx={{ fontWeight: "bold" }}>
            Contatos
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
            Alterações em contatos primários requerem confirmação. Certifique-se
            de que pelo menos um contato esteja verificado.
          </Alert>
        )}

        {formData.length === 0 && !isEditing ? (
          <Box sx={{ textAlign: "center", py: 4 }}>
            <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
              Nenhum contato cadastrado
            </Typography>
            <Button variant="outlined" startIcon={<AddIcon />} onClick={onEdit}>
              Adicionar Contato
            </Button>
          </Box>
        ) : (
          <List sx={{ p: 0 }}>
            {formData.map((contact, index) => (
              <ListItem
                key={contact.id}
                sx={{
                  border: 1,
                  borderColor: "divider",
                  borderRadius: 1,
                  mb: 3,
                  bgcolor: "background.paper",
                  p: { xs: 3, sm: 4 },
                  flexDirection: "column",
                  alignItems: "stretch",
                }}
              >
                <Box
                  sx={{
                    flex: 1,
                    width: "100%",
                    minWidth: 0, // Permite que o conteúdo seja cortado se necessário
                  }}
                >
                  {/* Linha 1: Ícone, Tipo de Contato e Categoria */}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: 1,
                      mb: 1,
                      flexWrap: "wrap",
                    }}
                  >
                    <Typography variant="body2" sx={{ fontSize: "1.2rem" }}>
                      {getContactTypeIcon(contact.type)}
                    </Typography>
                    <Typography variant="subtitle2" sx={{ fontWeight: "bold" }}>
                      {getContactTypeLabel(contact.type)}
                    </Typography>

                    <Chip
                      label={getCategoryLabel(contact.category)}
                      color="info"
                      size="small"
                      variant="outlined"
                      sx={{
                        fontSize: { xs: "0.7rem", sm: "0.75rem" },
                        height: { xs: 24, sm: 28 },
                      }}
                    />
                  </Box>

                  {/* Linha 2: Chips de Status (Primário e Verificação) */}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: { xs: 0.5, sm: 1 },
                      mb: { xs: 2, sm: 1 },
                      flexWrap: "wrap",
                    }}
                  >
                    {contact.isPrimary && (
                      <Chip
                        label="Primário"
                        color="primary"
                        size="small"
                        icon={<PrimaryIcon />}
                        sx={{
                          fontSize: { xs: "0.7rem", sm: "0.75rem" },
                          height: { xs: 24, sm: 28 },
                        }}
                      />
                    )}
                    {!canBePrimary(index) && !contact.isPrimary && (
                      <Chip
                        label="Não pode ser primário"
                        color="warning"
                        size="small"
                        variant="outlined"
                        sx={{
                          fontSize: { xs: "0.6rem", sm: "0.7rem" },
                          height: { xs: 20, sm: 24 },
                        }}
                      />
                    )}

                    {contact.isVerified ? (
                      <Chip
                        label="Verificado"
                        color="success"
                        size="small"
                        icon={<VerifiedIcon />}
                        sx={{
                          fontSize: { xs: "0.7rem", sm: "0.75rem" },
                          height: { xs: 24, sm: 28 },
                        }}
                      />
                    ) : (
                      <Chip
                        label="Não verificado"
                        color="warning"
                        size="small"
                        icon={<UnverifiedIcon />}
                        sx={{
                          fontSize: { xs: "0.7rem", sm: "0.75rem" },
                          height: { xs: 24, sm: 28 },
                        }}
                      />
                    )}
                  </Box>

                  {isEditing ? (
                    <Box sx={{ width: "100%" }}>
                      <Box
                        sx={{
                          width: "100%",
                          minWidth: { xs: "100%", sm: 300 },
                          mb: 3,
                        }}
                      >
                        {contact.type === "Phone" ||
                        contact.type === "WhatsApp" ? (
                          <PhoneInputWithDDI
                            fullWidth
                            label="Valor"
                            value={contact.value}
                            onChange={(value) =>
                              handleContactChange(index, "value", value)
                            }
                            error={!!errors[`contact_${index}_value`]}
                            helperText={errors[`contact_${index}_value`]}
                          />
                        ) : (
                          <ResponsiveTextField
                            fullWidth
                            label="Valor"
                            value={contact.value}
                            onChange={(e) =>
                              handleContactChange(
                                index,
                                "value",
                                e.target.value
                              )
                            }
                            type="email"
                            placeholder="exemplo@email.com"
                            error={!!errors[`contact_${index}_value`]}
                            helperText={errors[`contact_${index}_value`]}
                            mobileOptimized={true}
                          />
                        )}
                      </Box>

                      <Box
                        sx={{
                          display: "flex",
                          gap: { xs: 2, sm: 2 },
                          alignItems: "center",
                          flexDirection: { xs: "column", sm: "row" },
                          width: "100%",
                          mt: { xs: 2, sm: 1 },
                        }}
                      >
                        <ResponsiveButton
                          variant={contact.isPrimary ? "contained" : "outlined"}
                          startIcon={
                            contact.isPrimary ? (
                              <PrimaryIcon />
                            ) : (
                              <NotPrimaryIcon />
                            )
                          }
                          onClick={() => handleSetPrimary(index)}
                          disabled={
                            contact.isVerified === false || !canBePrimary(index)
                          }
                          title={
                            !canBePrimary(index)
                              ? `Já existe um ${contact.type} marcado como primário`
                              : contact.isPrimary
                              ? "Este contato é primário"
                              : "Tornar este contato primário"
                          }
                          fullWidthOnMobile={true}
                          mobileOptimized={true}
                        >
                          {contact.isPrimary ? "Primário" : "Tornar Primário"}
                        </ResponsiveButton>

                        <ResponsiveButton
                          variant="outlined"
                          color="error"
                          startIcon={<DeleteIcon />}
                          onClick={() => handleRemoveContact(index)}
                          disabled={contact.isPrimary}
                          fullWidthOnMobile={true}
                          mobileOptimized={true}
                        >
                          Remover
                        </ResponsiveButton>
                      </Box>
                    </Box>
                  ) : (
                    <Box>
                      <SensitiveDataField
                        value={contact.value}
                        fieldName={`contact_${index}`}
                        isRevealed={
                          sensitiveFields[`contact_${index}`] || false
                        }
                        onReveal={() =>
                          handleRevealSensitive(`contact_${index}`)
                        }
                        onHide={() => handleHideSensitive(`contact_${index}`)}
                        timeRemaining={
                          sensitiveFields[`contact_${index}`] ? 10 : 0
                        }
                        isEditable={false}
                        type={contact.type === "Email" ? "email" : "tel"}
                        placeholder="••••••••••"
                      />
                    </Box>
                  )}
                </Box>
              </ListItem>
            ))}
          </List>
        )}

        {isEditing && (
          <Box sx={{ mt: 2 }}>
            <ResponsiveButton
              variant="outlined"
              startIcon={<AddIcon />}
              onClick={handleAddContact}
              fullWidth
              mobileOptimized={true}
              sx={{
                border: "2px dashed",
                borderColor: "primary.main",
                color: "primary.main",
                "&:hover": {
                  borderColor: "primary.dark",
                  backgroundColor: "primary.light",
                },
              }}
            >
              Adicionar Novo Contato
            </ResponsiveButton>
          </Box>
        )}

        {/* Botões de ação */}
        {isEditing && (
          <>
            <Divider sx={{ my: 3 }} />
            <Stack
              direction={isMobile ? "column" : "row"}
              spacing={2}
              justifyContent="flex-end"
              sx={{ width: "100%" }}
            >
              <ResponsiveButton
                variant="outlined"
                startIcon={<CancelIcon />}
                onClick={handleCancel}
                disabled={isLoading}
                fullWidthOnMobile={true}
                mobileOptimized={true}
              >
                Cancelar
              </ResponsiveButton>
              <ResponsiveButton
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={isLoading || formData.length === 0}
                fullWidthOnMobile={true}
                mobileOptimized={true}
              >
                {isLoading ? "Salvando..." : "Salvar"}
              </ResponsiveButton>
            </Stack>
          </>
        )}
      </CardContent>
    </Card>
  );
};
