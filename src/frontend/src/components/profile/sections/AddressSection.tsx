import React, { useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Chip,
  Divider,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  useMediaQuery,
  useTheme,
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
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  
  const [formData, setFormData] = useState<AddressInfo[]>(data || []);
  const [editingAddressIndex, setEditingAddressIndex] = useState<number | null>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [addressToDelete, setAddressToDelete] = useState<number | null>(null);
  const [swipeStartX, setSwipeStartX] = useState<number | null>(null);
  const [swipeOffset, setSwipeOffset] = useState<number>(0);
  const [swipingAddressIndex, setSwipingAddressIndex] = useState<number | null>(null);

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
    setEditingAddressIndex(null);
    onCancel();
  };

  // Funções para edição individual de endereços
  const handleEditAddress = (index: number) => {
    setEditingAddressIndex(index);
  };

  const handleSaveAddress = () => {
    setEditingAddressIndex(null);
    onSave(formData);
  };

  const handleCancelAddress = () => {
    setFormData(data || []);
    setEditingAddressIndex(null);
  };

  // Funções para swipe e delete
  const handleTouchStart = (e: React.TouchEvent, index: number) => {
    if (!isMobile) return;
    setSwipeStartX(e.touches[0].clientX);
    setSwipingAddressIndex(index);
  };

  const handleTouchMove = (e: React.TouchEvent) => {
    if (!isMobile || swipeStartX === null || swipingAddressIndex === null) return;
    
    const currentX = e.touches[0].clientX;
    const diff = swipeStartX - currentX;
    
    if (diff > 0) {
      setSwipeOffset(Math.min(diff, 100)); // Máximo 100px de swipe
    }
  };

  const handleTouchEnd = () => {
    if (!isMobile || swipingAddressIndex === null) return;
    
    if (swipeOffset > 50) {
      // Swipe suficiente para mostrar opção de delete
      setAddressToDelete(swipingAddressIndex);
      setDeleteDialogOpen(true);
    }
    
    // Reset swipe
    setSwipeOffset(0);
    setSwipeStartX(null);
    setSwipingAddressIndex(null);
  };

  const handleConfirmDelete = () => {
    if (addressToDelete !== null) {
      const newData = formData.filter((_, index) => index !== addressToDelete);
      setFormData(newData);
      onSave(newData);
    }
    setDeleteDialogOpen(false);
    setAddressToDelete(null);
  };

  const handleCancelDelete = () => {
    setDeleteDialogOpen(false);
    setAddressToDelete(null);
    setSwipeOffset(0);
  };

  const formatZipCode = (value: string) => {
    // Remove tudo que não é dígito
    const numbers = value.replace(/\D/g, "");

    // Aplica a máscara do CEP
    if (numbers.length <= 5) {
      return numbers;
    } else {
      return `${numbers.slice(0, 5)}-${numbers.slice(5, 8)}`;
    }
  };

  const handleZipCodeChange = (index: number, value: string) => {
    const formattedValue = formatZipCode(value);
    handleFieldChange(index, "zipCode", formattedValue);
  };

  const getAddressTypeLabel = (type: string) => {
    const typeMap: Record<string, string> = {
      Residential: "Residencial",
      Commercial: "Comercial",
      Other: "Outro",
    };
    return typeMap[type] || "Residencial";
  };

  const getAddressTypeColor = (type: string) => {
    const colorMap: Record<string, string> = {
      Residential: "primary",
      Commercial: "secondary",
      Other: "default",
    };
    return colorMap[type] || "primary";
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

          {!isMobile && !isEditing && (
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
            {isMobile ? (
              // Layout Mobile: Cards individuais com swipe
              formData.map((address, index) => (
                <Box
                  key={address.id || index}
                  sx={{
                    position: "relative",
                    mb: 2,
                    transform: swipingAddressIndex === index ? `translateX(-${swipeOffset}px)` : "translateX(0)",
                    transition: swipingAddressIndex === index ? "none" : "transform 0.3s ease",
                  }}
                  onTouchStart={(e) => handleTouchStart(e, index)}
                  onTouchMove={handleTouchMove}
                  onTouchEnd={handleTouchEnd}
                >
                  {/* Card do Endereço */}
                  <Card
                    sx={{
                      position: "relative",
                      zIndex: 2,
                      backgroundColor: "background.paper",
                      boxShadow: 2,
                    }}
                  >
                    <CardContent>
                      {/* Cabeçalho do card: Ícone + Nome + Botão Editar */}
                      <Box
                        sx={{
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                          mb: 1,
                        }}
                      >
                        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                          <LocationIcon color="primary" />
                          <Typography variant="subtitle1" sx={{ fontWeight: "bold" }}>
                            Endereço {index + 1}
                          </Typography>
                        </Box>
                        
                        {editingAddressIndex !== index && (
                          <IconButton
                            onClick={() => handleEditAddress(index)}
                            size="small"
                            sx={{ color: "primary.main" }}
                          >
                            <EditIcon />
                          </IconButton>
                        )}
                      </Box>

                      {/* Tipo de endereço */}
                      <Box sx={{ mb: 2 }}>
                        <Chip
                          label={getAddressTypeLabel(address.type || "Residential")}
                          size="small"
                          color={getAddressTypeColor(address.type || "Residential") as any}
                          variant="outlined"
                        />
                      </Box>

                      {editingAddressIndex === index ? (
                        // Modo de edição
                        <Box>
                          <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
                            <Box sx={{ display: "flex", gap: 2 }}>
                              <TextField
                                fullWidth
                                label="CEP"
                                value={address.zipCode}
                                onChange={(e) =>
                                  handleZipCodeChange(index, e.target.value)
                                }
                                placeholder="00000-000"
                                inputProps={{ maxLength: 9 }}
                              />
                              <TextField
                                fullWidth
                                label="Número"
                                value={address.number}
                                onChange={(e) =>
                                  handleFieldChange(index, "number", e.target.value)
                                }
                              />
                            </Box>
                            <TextField
                              fullWidth
                              label="Rua"
                              value={address.street}
                              onChange={(e) =>
                                handleFieldChange(index, "street", e.target.value)
                              }
                            />
                            <Box sx={{ display: "flex", gap: 2 }}>
                              <TextField
                                fullWidth
                                label="Complemento"
                                value={address.complement || ""}
                                onChange={(e) =>
                                  handleFieldChange(index, "complement", e.target.value)
                                }
                              />
                              <TextField
                                fullWidth
                                label="Bairro"
                                value={address.neighborhood}
                                onChange={(e) =>
                                  handleFieldChange(index, "neighborhood", e.target.value)
                                }
                              />
                            </Box>
                            <Box sx={{ display: "flex", gap: 2 }}>
                              <TextField
                                fullWidth
                                label="Cidade"
                                value={address.city}
                                onChange={(e) =>
                                  handleFieldChange(index, "city", e.target.value)
                                }
                                sx={{ flex: 2 }}
                              />
                              <TextField
                                fullWidth
                                label="Estado"
                                value={address.state}
                                onChange={(e) =>
                                  handleFieldChange(index, "state", e.target.value)
                                }
                                inputProps={{ maxLength: 2 }}
                                sx={{ flex: 1 }}
                              />
                            </Box>
                          </Box>
                          
                          <Box sx={{ display: "flex", gap: 1, mt: 2 }}>
                            <Button
                              variant="contained"
                              startIcon={<SaveIcon />}
                              onClick={handleSaveAddress}
                              size="small"
                            >
                              Salvar
                            </Button>
                            <Button
                              variant="outlined"
                              startIcon={<CancelIcon />}
                              onClick={handleCancelAddress}
                              size="small"
                            >
                              Cancelar
                            </Button>
                          </Box>
                        </Box>
                      ) : (
                        // Modo de visualização
                        <Box>
                          {/* Cidade, Estado */}
                          <Typography variant="body2" color="text.secondary" sx={{ mb: 1, fontWeight: "medium" }}>
                            {address.city}, {address.state}
                          </Typography>
                          
                          {/* CEP */}
                          <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                            CEP: {address.zipCode}
                          </Typography>
                          
                          {/* Nome e número da rua */}
                          <Typography variant="body2" color="text.secondary">
                            {address.street}, {address.number}
                            {address.complement && `, ${address.complement}`}
                          </Typography>
                        </Box>
                      )}
                    </CardContent>
                  </Card>

                  {/* Área de delete (aparece quando faz swipe) */}
                  {swipeOffset > 20 && (
                    <Box
                      sx={{
                        position: "absolute",
                        top: 0,
                        right: -100,
                        width: 100,
                        height: "100%",
                        backgroundColor: "error.main",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        zIndex: 1,
                      }}
                    >
                      <DeleteIcon sx={{ color: "white", fontSize: 24 }} />
                    </Box>
                  )}
                </Box>
              ))
            ) : (
              // Layout Desktop: Formulário tradicional
              formData.map((address, index) => (
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

                  <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
                    {/* Primeira linha: CEP, Rua, Número */}
                    <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap" }}>
                    <TextField
                      label="CEP"
                      value={address.zipCode}
                      onChange={(e) =>
                        handleZipCodeChange(index, e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="00000-000"
                      error={!!errors.zipCode}
                      helperText={errors.zipCode || "Digite apenas números"}
                        inputProps={{ maxLength: 9 }}
                        sx={{ minWidth: 120, flex: "0 0 120px" }}
                    />
                    <TextField
                      label="Rua"
                      value={address.street}
                      onChange={(e) =>
                        handleFieldChange(index, "street", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome da rua"
                      error={!!errors.street}
                      helperText={errors.street}
                        sx={{ flex: 1, minWidth: 200 }}
                    />
                    <TextField
                      label="Número"
                      value={address.number}
                      onChange={(e) =>
                        handleFieldChange(index, "number", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="123"
                      error={!!errors.number}
                      helperText={errors.number}
                        sx={{ minWidth: 100, flex: "0 0 100px" }}
                    />
                    </Box>

                    {/* Segunda linha: Complemento, Bairro */}
                    <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap" }}>
                    <TextField
                      label="Complemento"
                      value={address.complement || ""}
                      onChange={(e) =>
                        handleFieldChange(index, "complement", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Apartamento, bloco, etc."
                      error={!!errors.complement}
                      helperText={errors.complement}
                        sx={{ flex: 1, minWidth: 200 }}
                    />
                    <TextField
                      label="Bairro"
                      value={address.neighborhood}
                      onChange={(e) =>
                        handleFieldChange(index, "neighborhood", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome do bairro"
                      error={!!errors.neighborhood}
                      helperText={errors.neighborhood}
                        sx={{ flex: 1, minWidth: 200 }}
                    />
                    </Box>

                    {/* Terceira linha: Cidade, Estado, País */}
                    <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap" }}>
                    <TextField
                      label="Cidade"
                      value={address.city}
                      onChange={(e) =>
                        handleFieldChange(index, "city", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Nome da cidade"
                      error={!!errors.city}
                      helperText={errors.city}
                        sx={{ flex: 2, minWidth: 200 }}
                    />
                    <TextField
                      label="Estado"
                      value={address.state}
                      onChange={(e) =>
                        handleFieldChange(index, "state", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="SP"
                      error={!!errors.state}
                      helperText={errors.state}
                        inputProps={{ maxLength: 2 }}
                        sx={{ minWidth: 80, flex: "0 0 80px" }}
                    />
                    <TextField
                      label="País"
                      value={address.country}
                      onChange={(e) =>
                        handleFieldChange(index, "country", e.target.value)
                      }
                      disabled={!isEditing}
                      placeholder="Brasil"
                      error={!!errors.country}
                      helperText={errors.country}
                        sx={{ minWidth: 120, flex: "0 0 120px" }}
                      />
                    </Box>
                  </Box>

                  {/* Tipo de endereço */}
                  {address.type && (
                    <Box sx={{ mt: 2 }}>
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
              ))
            )}

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

        {/* Botão Adicionar Endereço (Mobile) */}
        {isMobile && (
          <Box sx={{ mt: 2, textAlign: "center" }}>
            <Button
              variant="outlined"
              startIcon={<AddIcon />}
              onClick={handleAddAddress}
              fullWidth
              sx={{ py: 1.5 }}
            >
              Adicionar Novo Endereço
            </Button>
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

        {/* Diálogo de Confirmação de Delete */}
        <Dialog
          open={deleteDialogOpen}
          onClose={handleCancelDelete}
          maxWidth="sm"
          fullWidth
        >
          <DialogTitle>
            <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
              <DeleteIcon color="error" />
              Confirmar Exclusão
            </Box>
          </DialogTitle>
          <DialogContent>
            <Typography variant="body1">
              Tem certeza que deseja excluir este endereço? Esta ação não pode ser desfeita.
            </Typography>
            {addressToDelete !== null && formData[addressToDelete] && (
              <Box sx={{ mt: 2, p: 2, backgroundColor: "grey.100", borderRadius: 1 }}>
                <Typography variant="body2" color="text.secondary">
                  <strong>Endereço a ser excluído:</strong>
                </Typography>
                <Typography variant="body2">
                  {formData[addressToDelete].street}, {formData[addressToDelete].number}
                  {formData[addressToDelete].complement && `, ${formData[addressToDelete].complement}`}
                </Typography>
                <Typography variant="body2">
                  {formData[addressToDelete].neighborhood} - {formData[addressToDelete].city}/{formData[addressToDelete].state}
                </Typography>
              </Box>
            )}
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCancelDelete} color="primary">
              Cancelar
            </Button>
            <Button onClick={handleConfirmDelete} color="error" variant="contained">
              Excluir
            </Button>
          </DialogActions>
        </Dialog>
      </CardContent>
    </Card>
  );
};