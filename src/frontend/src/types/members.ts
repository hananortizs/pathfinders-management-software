/**
 * Tipos e interfaces para o sistema de membros
 */

export interface Member {
  id: string;
  firstName: string;
  lastName: string;
  middleNames?: string;
  socialName?: string;
  dateOfBirth: string;
  cpf: string;
  rg?: string;
  status: MemberStatus;
  gender: MemberGender;
  clubId: string;
  clubName: string;
  unitId: string;
  unitName: string;
  hierarchyPath: HierarchyPath;
  contacts: MemberContact[];
  addressInfo?: MemberAddress;
  medicalInfo?: MemberMedicalInfo;
  baptismInfo?: MemberBaptismInfo;
  scarfInfo?: MemberScarfInfo;
  createdAt: string;
  updatedAt: string;
  lastLoginAt?: string;
}

// Interface para lista de membros (resposta da API)
export interface MemberSummary {
  id: string;
  fullName: string;
  displayName: string;
  age: number;
  gender: string;
  status: string;
  clubName: string;
  unitName: string;
  currentRole: string;
  allRoles?: string; // Todas as roles ativas, ordenadas por nível hierárquico
  primaryEmail: string;
  primaryPhone: string;
  createdAt: string;
  hasScarfInvestiture: boolean;
  hasValidBaptism: boolean;
}

export interface MemberGroup {
  id: string;
  name: string;
  type: string;
  code: string;
  codePath: string;
  memberCount: number;
  subGroups: MemberGroup[];
  directMembers: MemberSummary[];
  isExpanded: boolean;
}

export interface MemberListStats {
  totalMembers: number;
  activeMembers: number;
  pendingMembers: number;
  inactiveMembers: number;
  suspendedMembers: number;
  byGender: Record<string, number>;
  byStatus: Record<string, number>;
  byClub: Record<string, number>;
  byUnit: Record<string, number>;
}

export interface MemberContact {
  id: string;
  type: ContactType;
  value: string;
  isVerified: boolean;
  isPrimary: boolean;
}

export interface MemberAddress {
  street: string;
  number: string;
  complement?: string;
  neighborhood: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
}

export interface MemberMedicalInfo {
  allergies?: string;
  medications?: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelationship?: string;
  medicalNotes?: string;
}

export interface MemberBaptismInfo {
  date?: string;
  church?: string;
  pastor?: string;
}

export interface MemberScarfInfo {
  date?: string;
  church?: string;
  pastor?: string;
}

export interface HierarchyPath {
  divisionId: string;
  divisionName: string;
  unionId: string;
  unionName: string;
  regionId: string;
  regionName: string;
  associationId: string;
  associationName: string;
  districtId: string;
  districtName: string;
  clubId: string;
  clubName: string;
  unitId: string;
  unitName: string;
}

export interface MemberFilters {
  search?: string;
  status?: MemberStatus[];
  gender?: MemberGender[];
  clubIds?: string[];
  unitIds?: string[];
  divisionIds?: string[];
  unionIds?: string[];
  regionIds?: string[];
  associationIds?: string[];
  districtIds?: string[];
  ageRange?: {
    min: number;
    max: number;
  };
  dateRange?: {
    start: string;
    end: string;
  };
}

export interface MemberListResponse {
  members: MemberSummary[];
  groups: MemberGroup[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  stats: MemberListStats;
}

export interface MemberBulkAction {
  action: "activate" | "deactivate" | "delete" | "export" | "transfer";
  memberIds: string[];
  targetClubId?: string;
  targetUnitId?: string;
}

export const MemberStatus = {
  Pending: "Pending",
  Active: "Active",
  Inactive: "Inactive",
  Suspended: "Suspended",
} as const;

export type MemberStatus = (typeof MemberStatus)[keyof typeof MemberStatus];

export const MemberGender = {
  Male: "Male",
  Female: "Female",
  Other: "Other",
} as const;

export type MemberGender = (typeof MemberGender)[keyof typeof MemberGender];

export const ContactType = {
  Email: "Email",
  Phone: "Phone",
  Mobile: "Mobile",
  WhatsApp: "WhatsApp",
} as const;

export type ContactType = (typeof ContactType)[keyof typeof ContactType];

export const HierarchyLevel = {
  Division: "Division",
  Union: "Union",
  Region: "Region",
  Association: "Association",
  District: "District",
  Club: "Club",
  Unit: "Unit",
} as const;

export type HierarchyLevel =
  (typeof HierarchyLevel)[keyof typeof HierarchyLevel];

export const UserLevel = {
  Admin: "Admin",
  Division: "Division",
  Union: "Union",
  Region: "Region",
  Association: "Association",
  District: "District",
  Club: "Club",
  Unit: "Unit",
} as const;

export type UserLevel = (typeof UserLevel)[keyof typeof UserLevel];

// DTOs para criação e edição
export interface CreateMemberDto {
  firstName: string;
  lastName: string;
  middleNames?: string;
  socialName?: string;
  dateOfBirth: string;
  cpf: string;
  rg?: string;
  gender: MemberGender;
  clubId: string;
  unitId: string;
  contacts: Omit<MemberContact, "id">[];
  addressInfo?: Omit<MemberAddress, "id">;
  medicalInfo?: Omit<MemberMedicalInfo, "id">;
  baptismInfo?: Omit<MemberBaptismInfo, "id">;
  scarfInfo?: Omit<MemberScarfInfo, "id">;
}

export interface UpdateMemberDto {
  firstName?: string;
  lastName?: string;
  middleNames?: string;
  socialName?: string;
  dateOfBirth?: string;
  cpf?: string;
  rg?: string;
  gender?: MemberGender;
  status?: MemberStatus;
  clubId?: string;
  unitId?: string;
  contacts?: Omit<MemberContact, "id">[];
  addressInfo?: Omit<MemberAddress, "id">;
  medicalInfo?: Omit<MemberMedicalInfo, "id">;
  baptismInfo?: Omit<MemberBaptismInfo, "id">;
  scarfInfo?: Omit<MemberScarfInfo, "id">;
}
