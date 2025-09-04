-- Script para atualizar todas as foreign keys de Guid para string(8)
-- Execute este script após a migração do Entity Framework

-- Atualizar tabelas de hierarquia
ALTER TABLE "Unions" ALTER COLUMN "DivisionId" TYPE VARCHAR(8);
ALTER TABLE "Associations" ALTER COLUMN "UnionId" TYPE VARCHAR(8);
ALTER TABLE "Regions" ALTER COLUMN "AssociationId" TYPE VARCHAR(8);
ALTER TABLE "Districts" ALTER COLUMN "RegionId" TYPE VARCHAR(8);
ALTER TABLE "Churches" ALTER COLUMN "DistrictId" TYPE VARCHAR(8);
ALTER TABLE "Clubs" ALTER COLUMN "DistrictId" TYPE VARCHAR(8);
ALTER TABLE "Clubs" ALTER COLUMN "ChurchId" TYPE VARCHAR(8);
ALTER TABLE "Units" ALTER COLUMN "ClubId" TYPE VARCHAR(8);

-- Atualizar tabelas de membros e assignments
ALTER TABLE "Memberships" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "Memberships" ALTER COLUMN "ClubId" TYPE VARCHAR(8);
ALTER TABLE "Assignments" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "Assignments" ALTER COLUMN "RoleId" TYPE VARCHAR(8);
ALTER TABLE "ApprovalDelegates" ALTER COLUMN "DelegatedToAssignmentId" TYPE VARCHAR(8);

-- Atualizar outras tabelas relacionadas
ALTER TABLE "Investitures" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "InvestitureWitnesses" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "MemberEventParticipations" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "MemberEventParticipations" ALTER COLUMN "OfficialEventId" TYPE VARCHAR(8);
ALTER TABLE "TimelineEntries" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "UserCredentials" ALTER COLUMN "MemberId" TYPE VARCHAR(8);
ALTER TABLE "PasswordHistories" ALTER COLUMN "MemberId" TYPE VARCHAR(8);

-- Atualizar chaves primárias
ALTER TABLE "Divisions" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Unions" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Associations" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Regions" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Districts" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Churches" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Clubs" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Units" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Members" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Memberships" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Assignments" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "ApprovalDelegates" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "RoleCatalogs" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "Investitures" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "InvestitureWitnesses" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "OfficialEvents" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "MemberEventParticipations" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "TaskItems" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "TimelineEntries" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "UserCredentials" ALTER COLUMN "Id" TYPE VARCHAR(8);
ALTER TABLE "PasswordHistories" ALTER COLUMN "Id" TYPE VARCHAR(8);

-- Recriar índices para otimizar performance
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Unions_DivisionId" ON "Unions" ("DivisionId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Associations_UnionId" ON "Associations" ("UnionId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Regions_AssociationId" ON "Regions" ("AssociationId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Districts_RegionId" ON "Districts" ("RegionId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Churches_DistrictId" ON "Churches" ("DistrictId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Clubs_DistrictId" ON "Clubs" ("DistrictId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Clubs_ChurchId" ON "Clubs" ("ChurchId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Units_ClubId" ON "Units" ("ClubId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Memberships_MemberId" ON "Memberships" ("MemberId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Memberships_ClubId" ON "Memberships" ("ClubId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Assignments_MemberId" ON "Assignments" ("MemberId");
CREATE INDEX CONCURRENTLY IF NOT EXISTS "IX_Assignments_RoleId" ON "Assignments" ("RoleId");
