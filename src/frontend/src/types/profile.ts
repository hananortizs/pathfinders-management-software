/**
 * Tipos TypeScript para a página de perfil do usuário
 * Inclui interfaces para dados pessoais, contatos, endereço, saúde, documentos e preferências
 */

export interface ProfileData {
  id: string;
  firstName: string;
  lastName: string;
  middleNames?: string;
  socialName?: string;
  dateOfBirth: string;
  gender: "Male" | "Female" | "Other";
  status: "Active" | "Pending" | "Archived";
  clubName?: string;
  unitName?: string;
  roles: string[];
  createdAt: string;
  updatedAt: string;
}

export interface ContactInfo {
  id: string;
  type: "Email" | "Phone" | "WhatsApp";
  value: string;
  isPrimary: boolean;
  isVerified: boolean;
  category: "Personal" | "Emergency" | "Work";
  notes?: string;
}

export interface AddressInfo {
  id: string;
  zipCode: string;
  street: string;
  number: string;
  complement?: string;
  neighborhood: string;
  city: string;
  state: string;
  country: string;
  type: "Residential" | "Commercial" | "Other";
}

export interface MedicalInfo {
  id: string;
  allergies?: string;
  medications?: string;
  bloodType?: string;
  medicalConditions?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelationship?: string;
  notes?: string;
}

export interface DocumentInfo {
  cpf: string;
  rg?: string;
  rgIssuer?: string;
  rgIssueDate?: string;
}

export interface Preferences {
  language: string;
  theme: "light" | "dark" | "system";
  notifications: {
    email: boolean;
    sms: boolean;
    push: boolean;
  };
  timezone: string;
}

export interface ProfileSections {
  personalData: ProfileData;
  contacts: ContactInfo[];
  address: AddressInfo[];
  medical: MedicalInfo;
  documents: DocumentInfo;
  preferences: Preferences;
}

export interface SensitiveFieldProps {
  value: string;
  fieldName: string;
  isRevealed: boolean;
  onReveal: () => void;
  onHide: () => void;
  timeRemaining?: number;
  isEditable?: boolean;
  onChange?: (value: string) => void;
  placeholder?: string;
  type?: "text" | "email" | "tel" | "date";
  mask?: string;
}

export interface ProfileSectionProps {
  data: any;
  isEditing: boolean;
  onEdit: () => void;
  onSave: (data: any) => void;
  onCancel: () => void;
  isLoading: boolean;
  errors?: Record<string, string>;
}

export interface ProfileHeaderProps {
  user: ProfileData;
  onEditProfile: () => void;
  onChangePassword: () => void;
  onViewAudit: () => void;
}

export interface ProfileTabsProps {
  activeTab: string;
  onTabChange: (tab: string) => void;
  isMobile: boolean;
  pendencies?: {
    personalData: number;
    contacts: number;
    address: number;
    health: number;
    documents: number;
    total: number;
  };
}

export interface ChangePasswordData {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface AuditLogEntry {
  id: string;
  action: string;
  fieldName?: string;
  timestamp: string;
  ipAddress?: string;
  userAgent?: string;
  details?: string;
}

export interface ProfileAuditProps {
  logs: AuditLogEntry[];
  isLoading: boolean;
  onLoadMore: () => void;
  hasMore: boolean;
}
