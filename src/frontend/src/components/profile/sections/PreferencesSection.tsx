import React, { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Stack,
  Grid,
  Switch,
  FormControlLabel,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Divider,
  Alert,
} from '@mui/material';
import {
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Settings as SettingsIcon,
  Language as LanguageIcon,
  Palette as PaletteIcon,
  Notifications as NotificationsIcon,
} from '@mui/icons-material';
import type { ProfileSectionProps, Preferences } from '../../../types/profile';

/**
 * Seção de Preferências do perfil
 * Inclui idioma, tema, notificações e configurações pessoais
 */
export const PreferencesSection: React.FC<ProfileSectionProps> = ({
  data,
  isEditing,
  onEdit,
  onSave,
  onCancel,
  isLoading,
  errors = {},
}) => {
  const [formData, setFormData] = useState<Preferences>(data || {
    language: 'pt-BR',
    theme: 'system',
    notifications: {
      email: true,
      sms: false,
      push: true,
    },
    timezone: 'America/Sao_Paulo',
  });

  const handleFieldChange = (field: keyof Preferences, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleNotificationChange = (field: keyof Preferences['notifications'], value: boolean) => {
    setFormData(prev => ({
      ...prev,
      notifications: {
        ...prev.notifications,
        [field]: value,
      },
    }));
  };

  const handleSave = () => {
    onSave(formData);
  };

  const handleCancel = () => {
    setFormData(data || {
      language: 'pt-BR',
      theme: 'system',
      notifications: {
        email: true,
        sms: false,
        push: true,
      },
      timezone: 'America/Sao_Paulo',
    });
    onCancel();
  };

  const getLanguageLabel = (code: string) => {
    switch (code) {
      case 'pt-BR':
        return 'Português (Brasil)';
      case 'en-US':
        return 'English (United States)';
      case 'es-ES':
        return 'Español (España)';
      default:
        return code;
    }
  };

  const getThemeLabel = (theme: string) => {
    switch (theme) {
      case 'light':
        return 'Claro';
      case 'dark':
        return 'Escuro';
      case 'system':
        return 'Sistema';
      default:
        return theme;
    }
  };

  const getTimezoneLabel = (timezone: string) => {
    switch (timezone) {
      case 'America/Sao_Paulo':
        return 'Brasília (GMT-3)';
      case 'America/New_York':
        return 'Nova York (GMT-5)';
      case 'Europe/London':
        return 'Londres (GMT+0)';
      case 'Europe/Paris':
        return 'Paris (GMT+1)';
      default:
        return timezone;
    }
  };

  return (
    <Card>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <SettingsIcon color="primary" />
            <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
              Preferências
            </Typography>
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

        <Grid container spacing={3}>
          {/* Idioma */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
                <LanguageIcon color="action" />
                <Typography variant="body2" color="text.secondary">
                  Idioma
                </Typography>
              </Box>
              {isEditing ? (
                <FormControl fullWidth>
                  <InputLabel>Idioma</InputLabel>
                  <Select
                    value={formData.language}
                    onChange={(e) => handleFieldChange('language', e.target.value)}
                    label="Idioma"
                    error={!!errors.language}
                  >
                    <MenuItem value="pt-BR">Português (Brasil)</MenuItem>
                    <MenuItem value="en-US">English (United States)</MenuItem>
                    <MenuItem value="es-ES">Español (España)</MenuItem>
                  </Select>
                </FormControl>
              ) : (
                <Typography variant="body1">
                  {getLanguageLabel(formData.language)}
                </Typography>
              )}
              {errors.language && (
                <Typography variant="caption" color="error" sx={{ mt: 1, display: 'block' }}>
                  {errors.language}
                </Typography>
              )}
            </Box>
          </Grid>

          {/* Tema */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
                <PaletteIcon color="action" />
                <Typography variant="body2" color="text.secondary">
                  Tema
                </Typography>
              </Box>
              {isEditing ? (
                <FormControl fullWidth>
                  <InputLabel>Tema</InputLabel>
                  <Select
                    value={formData.theme}
                    onChange={(e) => handleFieldChange('theme', e.target.value)}
                    label="Tema"
                    error={!!errors.theme}
                  >
                    <MenuItem value="light">Claro</MenuItem>
                    <MenuItem value="dark">Escuro</MenuItem>
                    <MenuItem value="system">Sistema</MenuItem>
                  </Select>
                </FormControl>
              ) : (
                <Typography variant="body1">
                  {getThemeLabel(formData.theme)}
                </Typography>
              )}
              {errors.theme && (
                <Typography variant="caption" color="error" sx={{ mt: 1, display: 'block' }}>
                  {errors.theme}
                </Typography>
              )}
            </Box>
          </Grid>

          {/* Fuso Horário */}
          <Grid size={{ xs: 12, md: 6 }}>
            <Box>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                Fuso Horário
              </Typography>
              {isEditing ? (
                <FormControl fullWidth>
                  <InputLabel>Fuso Horário</InputLabel>
                  <Select
                    value={formData.timezone}
                    onChange={(e) => handleFieldChange('timezone', e.target.value)}
                    label="Fuso Horário"
                    error={!!errors.timezone}
                  >
                    <MenuItem value="America/Sao_Paulo">Brasília (GMT-3)</MenuItem>
                    <MenuItem value="America/New_York">Nova York (GMT-5)</MenuItem>
                    <MenuItem value="Europe/London">Londres (GMT+0)</MenuItem>
                    <MenuItem value="Europe/Paris">Paris (GMT+1)</MenuItem>
                  </Select>
                </FormControl>
              ) : (
                <Typography variant="body1">
                  {getTimezoneLabel(formData.timezone)}
                </Typography>
              )}
              {errors.timezone && (
                <Typography variant="caption" color="error" sx={{ mt: 1, display: 'block' }}>
                  {errors.timezone}
                </Typography>
              )}
            </Box>
          </Grid>

          <Divider sx={{ my: 2 }} />

          {/* Notificações */}
          <Grid size={{ xs: 12 }}>
            <Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                <NotificationsIcon color="action" />
                <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                  Notificações
                </Typography>
              </Box>
              
              <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                  <FormControlLabel
                    control={
                      <Switch
                        checked={formData.notifications.email}
                        onChange={(e) => handleNotificationChange('email', e.target.checked)}
                        disabled={!isEditing}
                        color="primary"
                      />
                    }
                    label="E-mail"
                  />
                  <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 0.5 }}>
                    Receber notificações por e-mail
                  </Typography>
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                  <FormControlLabel
                    control={
                      <Switch
                        checked={formData.notifications.sms}
                        onChange={(e) => handleNotificationChange('sms', e.target.checked)}
                        disabled={!isEditing}
                        color="primary"
                      />
                    }
                    label="SMS"
                  />
                  <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 0.5 }}>
                    Receber notificações por SMS
                  </Typography>
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                  <FormControlLabel
                    control={
                      <Switch
                        checked={formData.notifications.push}
                        onChange={(e) => handleNotificationChange('push', e.target.checked)}
                        disabled={!isEditing}
                        color="primary"
                      />
                    }
                    label="Push"
                  />
                  <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 0.5 }}>
                    Receber notificações push no navegador
                  </Typography>
                </Grid>
              </Grid>
            </Box>
          </Grid>

          {/* Informações sobre preferências */}
          <Grid size={{ xs: 12 }}>
            <Alert severity="info" sx={{ mt: 2 }}>
              <Typography variant="body2">
                <strong>Informações sobre preferências:</strong>
              </Typography>
              <ul style={{ margin: '8px 0', paddingLeft: '20px' }}>
                <li>As preferências são salvas automaticamente</li>
                <li>O tema "Sistema" segue as configurações do seu dispositivo</li>
                <li>As notificações podem ser ajustadas a qualquer momento</li>
                <li>Alterações no idioma requerem recarregamento da página</li>
              </ul>
            </Alert>
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
                {isLoading ? 'Salvando...' : 'Salvar'}
              </Button>
            </Stack>
          </>
        )}
      </CardContent>
    </Card>
  );
};
