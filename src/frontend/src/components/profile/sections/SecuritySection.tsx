import React, { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Grid,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton,
  InputAdornment,
  CircularProgress,
} from '@mui/material';
import {
  Security as SecurityIcon,
  Lock as LockIcon,
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  CheckCircle as CheckIcon,
} from '@mui/icons-material';
import type { ProfileSectionProps, ChangePasswordData } from '../../../types/profile';

/**
 * Seção de Segurança da Conta do perfil
 * Inclui alteração de senha e configurações de segurança
 */
export const SecuritySection: React.FC<ProfileSectionProps> = ({
  isLoading,
  errors = {},
}) => {
  const [changePasswordOpen, setChangePasswordOpen] = useState(false);
  const [passwordData, setPasswordData] = useState<ChangePasswordData>({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [showPasswords, setShowPasswords] = useState({
    current: false,
    new: false,
    confirm: false,
  });

  const handlePasswordFieldChange = (field: keyof ChangePasswordData, value: string) => {
    setPasswordData(prev => ({ ...prev, [field]: value }));
  };

  const handleTogglePasswordVisibility = (field: 'current' | 'new' | 'confirm') => {
    setShowPasswords(prev => ({ ...prev, [field]: !prev[field] }));
  };

  const handleChangePassword = () => {
    // Validar senhas
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      return;
    }
    
    // TODO: Implementar salvamento da senha
    setChangePasswordOpen(false);
    setPasswordData({
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    });
  };

  const handleCancelPassword = () => {
    setChangePasswordOpen(false);
    setPasswordData({
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    });
  };

  const getPasswordStrength = (password: string) => {
    let strength = 0;
    let feedback = [];

    if (password.length >= 8) strength += 1;
    else feedback.push('Mínimo 8 caracteres');

    if (/[a-z]/.test(password)) strength += 1;
    else feedback.push('Pelo menos uma letra minúscula');

    if (/[A-Z]/.test(password)) strength += 1;
    else feedback.push('Pelo menos uma letra maiúscula');

    if (/[0-9]/.test(password)) strength += 1;
    else feedback.push('Pelo menos um número');

    if (/[^A-Za-z0-9]/.test(password)) strength += 1;
    else feedback.push('Pelo menos um caractere especial');

    return { strength, feedback };
  };

  const passwordStrength = getPasswordStrength(passwordData.newPassword);
  const isPasswordValid = passwordData.newPassword === passwordData.confirmPassword && passwordStrength.strength >= 3;

  const getStrengthColor = (strength: number) => {
    if (strength < 2) return 'error';
    if (strength < 4) return 'warning';
    return 'success';
  };

  const getStrengthLabel = (strength: number) => {
    if (strength < 2) return 'Muito fraca';
    if (strength < 4) return 'Fraca';
    if (strength < 5) return 'Boa';
    return 'Muito forte';
  };

  return (
    <>
      <Card>
        <CardContent>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <SecurityIcon color="primary" />
              <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                Segurança da Conta
              </Typography>
            </Box>
          </Box>

          <Grid container spacing={3}>
            {/* Alterar Senha */}
            <Grid size={{ xs: 12 }}>
              <Box sx={{ 
                p: 3, 
                border: 1, 
                borderColor: 'divider', 
                borderRadius: 1,
                bgcolor: 'background.paper'
              }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                  <LockIcon color="action" />
                  <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                    Alterar Senha
                  </Typography>
                </Box>
                
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                  Para sua segurança, recomendamos alterar sua senha regularmente.
                </Typography>

                <Button
                  variant="contained"
                  startIcon={<LockIcon />}
                  onClick={() => setChangePasswordOpen(true)}
                  disabled={isLoading}
                >
                  Alterar Senha
                </Button>
              </Box>
            </Grid>

            {/* Informações de Segurança */}
            <Grid size={{ xs: 12 }}>
              <Alert severity="info">
                <Typography variant="body2">
                  <strong>Dicas de Segurança:</strong>
                </Typography>
                <ul style={{ margin: '8px 0', paddingLeft: '20px' }}>
                  <li>Use uma senha forte com pelo menos 8 caracteres</li>
                  <li>Combine letras maiúsculas, minúsculas, números e símbolos</li>
                  <li>Não use informações pessoais na senha</li>
                  <li>Não compartilhe sua senha com outras pessoas</li>
                  <li>Altere sua senha regularmente</li>
                </ul>
              </Alert>
            </Grid>

            {/* MVP1: 2FA e Sessões */}
            <Grid size={{ xs: 12 }}>
              <Box sx={{ 
                p: 3, 
                border: 1, 
                borderColor: 'divider', 
                borderRadius: 1,
                bgcolor: 'grey.50',
                textAlign: 'center'
              }}>
                <Typography variant="h6" sx={{ fontWeight: 'bold', mb: 1 }}>
                  Recursos Avançados
                </Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                  Em breve: Autenticação de dois fatores (2FA) e gerenciamento de sessões ativas
                </Typography>
                <Button variant="outlined" disabled>
                  Em Desenvolvimento
                </Button>
              </Box>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {/* Dialog de Alteração de Senha */}
      <Dialog 
        open={changePasswordOpen} 
        onClose={handleCancelPassword}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <LockIcon color="primary" />
            Alterar Senha
          </Box>
        </DialogTitle>
        
        <DialogContent>
          <Grid container spacing={3} sx={{ mt: 1 }}>
            {/* Senha Atual */}
            <Grid size={{ xs: 12 }}>
              <TextField
                fullWidth
                label="Senha Atual"
                type={showPasswords.current ? 'text' : 'password'}
                value={passwordData.currentPassword}
                onChange={(e) => handlePasswordFieldChange('currentPassword', e.target.value)}
                error={!!errors.currentPassword}
                helperText={errors.currentPassword || 'Digite sua senha atual'}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleTogglePasswordVisibility('current')}
                        edge="end"
                      >
                        {showPasswords.current ? <VisibilityOffIcon /> : <VisibilityIcon />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            </Grid>

            {/* Nova Senha */}
            <Grid size={{ xs: 12 }}>
              <TextField
                fullWidth
                label="Nova Senha"
                type={showPasswords.new ? 'text' : 'password'}
                value={passwordData.newPassword}
                onChange={(e) => handlePasswordFieldChange('newPassword', e.target.value)}
                error={!!errors.newPassword}
                helperText={errors.newPassword || 'Digite sua nova senha'}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleTogglePasswordVisibility('new')}
                        edge="end"
                      >
                        {showPasswords.new ? <VisibilityOffIcon /> : <VisibilityIcon />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              
              {/* Indicador de Força da Senha */}
              {passwordData.newPassword && (
                <Box sx={{ mt: 1 }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
                    <Typography variant="caption" color="text.secondary">
                      Força da senha:
                    </Typography>
                    <Typography 
                      variant="caption" 
                      color={`${getStrengthColor(passwordStrength.strength)}.main`}
                      sx={{ fontWeight: 'bold' }}
                    >
                      {getStrengthLabel(passwordStrength.strength)}
                    </Typography>
                  </Box>
                  
                  <CircularProgress
                    variant="determinate"
                    value={(passwordStrength.strength / 5) * 100}
                    color={getStrengthColor(passwordStrength.strength) as any}
                    sx={{ height: 4, borderRadius: 2 }}
                  />
                  
                  {passwordStrength.feedback.length > 0 && (
                    <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>
                      {passwordStrength.feedback.join(', ')}
                    </Typography>
                  )}
                </Box>
              )}
            </Grid>

            {/* Confirmar Nova Senha */}
            <Grid size={{ xs: 12 }}>
              <TextField
                fullWidth
                label="Confirmar Nova Senha"
                type={showPasswords.confirm ? 'text' : 'password'}
                value={passwordData.confirmPassword}
                onChange={(e) => handlePasswordFieldChange('confirmPassword', e.target.value)}
                error={Boolean(errors.confirmPassword) || Boolean(passwordData.confirmPassword && !isPasswordValid)}
                helperText={
                  errors.confirmPassword || 
                  (passwordData.confirmPassword && !isPasswordValid ? 'As senhas não coincidem' : 'Confirme sua nova senha')
                }
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        onClick={() => handleTogglePasswordVisibility('confirm')}
                        edge="end"
                      >
                        {showPasswords.confirm ? <VisibilityOffIcon /> : <VisibilityIcon />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            </Grid>
          </Grid>
        </DialogContent>
        
        <DialogActions>
          <Button onClick={handleCancelPassword} disabled={isLoading}>
            Cancelar
          </Button>
          <Button
            onClick={handleChangePassword}
            variant="contained"
            disabled={isLoading || !isPasswordValid || !passwordData.currentPassword}
            startIcon={isLoading ? <CircularProgress size={16} /> : <CheckIcon />}
          >
            {isLoading ? 'Alterando...' : 'Alterar Senha'}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};
