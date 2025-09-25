/**
 * Componente de lista de membros com agrupamento hierárquico
 */

import React, { memo, useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  List,
  ListItem,
  ListItemSecondaryAction,
  Checkbox,
  Avatar,
  Chip,
  IconButton,
  Menu,
  MenuItem,
  Divider,
  Collapse,
  Badge,
  LinearProgress,
  Alert,
} from "@mui/material";
import {
  ExpandMore,
  ExpandLess,
  MoreVert,
  Person,
  Edit,
  Delete,
  Visibility,
  TransferWithinAStation,
  Email,
} from "@mui/icons-material";
import type {
  MemberSummary,
  MemberGroup,
  UserLevel,
} from "../../types/members";
import { MemberStatus, MemberGender } from "../../types/members";

interface MembersListProps {
  members: MemberSummary[];
  groups: MemberGroup[];
  selectedMembers: string[];
  onSelectMember: (memberId: string) => void;
  onSelectAll: () => void;
  onMemberAction: (action: string, memberId: string) => void;
  userLevel: UserLevel;
  groupingStrategy: "hierarchical" | "flat" | "by_club" | "by_unit";
  loading?: boolean;
  error?: string | null;
}

interface GroupedMembers {
  [key: string]: {
    group: MemberGroup;
    members: MemberSummary[];
    expanded: boolean;
  };
}

const MembersListComponent: React.FC<MembersListProps> = ({
  members,
  groups,
  selectedMembers,
  onSelectMember,
  onSelectAll,
  onMemberAction,
  userLevel: _userLevel,
  groupingStrategy,
  loading = false,
  error = null,
}) => {
  const [expandedGroups, setExpandedGroups] = useState<Set<string>>(
    new Set(["all"])
  ); // Expandir por padrão
  const [anchorEl, setAnchorEl] = useState<{
    [key: string]: HTMLElement | null;
  }>({});

  // Agrupar membros por estratégia
  const groupedMembers = React.useMemo(() => {
    const grouped: GroupedMembers = {};

    if (groupingStrategy === "flat") {
      grouped["all"] = {
        group: {
          id: "all",
          name: "Todos os Membros",
          type: "Unit" as any,
          code: "ALL",
          codePath: "ALL",
          memberCount: members.length,
          subGroups: [],
          directMembers: members,
          isExpanded: true,
        },
        members: members,
        expanded: true,
      };
    } else if (groups.length > 0) {
      // Agrupar por hierarquia quando há grupos
      groups.forEach((group) => {
        const groupMembers = members.filter((member) => {
          switch (groupingStrategy) {
            case "by_club":
              return member.clubName === group.name;
            case "by_unit":
              return member.unitName === group.name;
            case "hierarchical":
              return group.name === "All Members"; // Simplificado para MVP
            default:
              return false;
          }
        });

        grouped[group.id] = {
          group,
          members: groupMembers,
          expanded: expandedGroups.has(group.id),
        };
      });
    } else {
      // Fallback: quando não há grupos, criar grupos baseados nos membros
      const uniqueGroups = new Map<
        string,
        { name: string; members: MemberSummary[] }
      >();

      members.forEach((member) => {
        let groupKey: string;
        let groupName: string;

        // Verificar se é administrador (sem clube/unidade)
        const isAdmin =
          !((member as any).ClubName || member.clubName) &&
          !((member as any).UnitName || member.unitName);

        switch (groupingStrategy) {
          case "by_club":
            if (isAdmin) {
              groupKey = "administradores";
              groupName = "Administradores";
            } else {
              groupKey =
                (member as any).ClubName || member.clubName || "Sem Clube";
              groupName =
                (member as any).ClubName || member.clubName || "Sem Clube";
            }
            break;
          case "by_unit":
            if (isAdmin) {
              groupKey = "administradores";
              groupName = "Administradores";
            } else {
              groupKey =
                (member as any).UnitName || member.unitName || "Sem Unidade";
              groupName =
                (member as any).UnitName || member.unitName || "Sem Unidade";
            }
            break;
          case "hierarchical":
            if (isAdmin) {
              groupKey = "administradores";
              groupName = "Administradores";
            } else {
              groupKey = `${
                (member as any).ClubName || member.clubName || "Sem Clube"
              } > ${
                (member as any).UnitName || member.unitName || "Sem Unidade"
              }`;
              groupName = `${
                (member as any).ClubName || member.clubName || "Sem Clube"
              } > ${
                (member as any).UnitName || member.unitName || "Sem Unidade"
              }`;
            }
            break;
          default:
            groupKey = "all";
            groupName = "Todos os Membros";
        }

        if (!uniqueGroups.has(groupKey)) {
          uniqueGroups.set(groupKey, { name: groupName, members: [] });
        }
        uniqueGroups.get(groupKey)!.members.push(member);
      });

      // Converter para formato esperado
      Array.from(uniqueGroups.entries()).forEach(([key, groupData]) => {
        grouped[key] = {
          group: {
            id: key,
            name: groupData.name,
            type: "Unit" as any,
            code: key,
            codePath: key,
            memberCount: groupData.members.length,
            subGroups: [],
            directMembers: groupData.members,
            isExpanded: true,
          },
          members: groupData.members,
          expanded: expandedGroups.has(key) || true, // Sempre expandido por padrão
        };
      });
    }

    return grouped;
  }, [members, groups, groupingStrategy, expandedGroups]);

  const handleGroupToggle = (groupId: string) => {
    setExpandedGroups((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(groupId)) {
        newSet.delete(groupId);
      } else {
        newSet.add(groupId);
      }
      return newSet;
    });
  };

  const handleMenuOpen = (
    event: React.MouseEvent<HTMLElement>,
    memberId: string
  ) => {
    setAnchorEl((prev) => ({ ...prev, [memberId]: event.currentTarget }));
  };

  const handleMenuClose = (memberId: string) => {
    setAnchorEl((prev) => ({ ...prev, [memberId]: null }));
  };

  const getStatusColor = (status: MemberStatus) => {
    switch (status) {
      case MemberStatus.Active:
        return "success";
      case MemberStatus.Pending:
        return "warning";
      case MemberStatus.Inactive:
        return "default";
      case MemberStatus.Suspended:
        return "error";
      default:
        return "default";
    }
  };

  const getStatusLabel = (status: MemberStatus) => {
    switch (status) {
      case MemberStatus.Active:
        return "Ativo";
      case MemberStatus.Pending:
        return "Pendente";
      case MemberStatus.Inactive:
        return "Inativo";
      case MemberStatus.Suspended:
        return "Suspenso";
      default:
        return status;
    }
  };

  const getGenderIcon = (gender: MemberGender) => {
    switch (gender) {
      case MemberGender.Male:
        return "👨";
      case MemberGender.Female:
        return "👩";
      case MemberGender.Other:
        return "🧑";
      default:
        return "👤";
    }
  };

  const formatDate = (dateString: string) => {
    if (!dateString) return "Data não disponível";

    try {
      const date = new Date(dateString);
      if (isNaN(date.getTime())) {
        return "Data inválida";
      }
      return date.toLocaleDateString("pt-BR");
    } catch (error) {
      console.error("Erro ao formatar data:", error, dateString);
      return "Data inválida";
    }
  };

  if (error) {
    return (
      <Alert severity="error" sx={{ mb: 2 }}>
        Erro ao carregar membros: {error}
      </Alert>
    );
  }

  if (loading) {
    return (
      <Box sx={{ mb: 2 }}>
        <LinearProgress />
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ mt: 1, textAlign: "center" }}
        >
          Carregando membros...
        </Typography>
      </Box>
    );
  }

  if (members.length === 0) {
    return (
      <Card>
        <CardContent sx={{ textAlign: "center", py: 4 }}>
          <Person sx={{ fontSize: 64, color: "text.secondary", mb: 2 }} />
          <Typography variant="h6" color="text.secondary" gutterBottom>
            Nenhum membro encontrado
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Ajuste os filtros ou adicione novos membros
          </Typography>
        </CardContent>
      </Card>
    );
  }

  // Debug: Log dos dados agrupados
  console.log("🔍 MembersList: Dados agrupados", {
    groupedMembers,
    members,
    groups,
    groupingStrategy,
  });

  return (
    <Box>
      {Object.entries(groupedMembers).map(([groupId, groupData]) => (
        <Card key={groupId} sx={{ mb: 2 }}>
          <CardContent sx={{ p: 0 }}>
            {/* Header do grupo */}
            <ListItem
              component="div"
              onClick={() => handleGroupToggle(groupId)}
              sx={{
                bgcolor: "grey.50",
                borderBottom: 1,
                borderColor: "divider",
                cursor: "pointer",
              }}
            >
              <Checkbox
                checked={groupData.members.every((member) =>
                  selectedMembers.includes(member.id)
                )}
                indeterminate={
                  groupData.members.some((member) =>
                    selectedMembers.includes(member.id)
                  ) &&
                  !groupData.members.every((member) =>
                    selectedMembers.includes(member.id)
                  )
                }
                onChange={onSelectAll}
                onClick={(e) => e.stopPropagation()}
              />

              <Box sx={{ flex: 1, minWidth: 0 }}>
                <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                  <Typography variant="h6" component="span">
                    {groupData.group.name}
                  </Typography>
                  <Badge
                    badgeContent={groupData.members.length}
                    color="primary"
                    sx={{ ml: 1 }}
                  />
                </Box>
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ mt: 0.5 }}
                >
                  {groupData.group.type} • {groupData.members.length} membro(s)
                </Typography>
              </Box>

              <IconButton>
                {groupData.expanded ? <ExpandLess /> : <ExpandMore />}
              </IconButton>
            </ListItem>

            {/* Lista de membros do grupo */}
            <Collapse in={groupData.expanded}>
              <List sx={{ p: 0 }}>
                {groupData.members.map((member, index) => {
                  // Debug: Log dos dados do membro
                  console.log("🔍 MembersList: Renderizando membro", {
                    member,
                    fullName: (member as any).FullName || member.fullName,
                    age: (member as any).Age || member.age,
                    createdAt: (member as any).CreatedAt || member.createdAt,
                    clubName: (member as any).ClubName || member.clubName,
                    unitName: (member as any).UnitName || member.unitName,
                    status: (member as any).Status || member.status,
                    gender: (member as any).Gender || member.gender,
                  });

                  return (
                    <React.Fragment key={member.id}>
                      <ListItem
                        sx={{
                          bgcolor: selectedMembers.includes(member.id)
                            ? "action.selected"
                            : "transparent",
                          "&:hover": {
                            bgcolor: "action.hover",
                          },
                        }}
                      >
                        <Checkbox
                          checked={selectedMembers.includes(member.id)}
                          onChange={() => onSelectMember(member.id)}
                        />

                        <Avatar
                          sx={{
                            bgcolor: "primary.light",
                            color: "primary.contrastText",
                            mr: 2,
                          }}
                        >
                          {getGenderIcon(
                            (member as any).Gender || (member.gender as any)
                          )}
                        </Avatar>

                        <Box sx={{ flex: 1, minWidth: 0 }}>
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                              gap: 1,
                              mb: 0.5,
                            }}
                          >
                            <Typography variant="subtitle1" component="span">
                              {(member as any).FullName ||
                                (member as any).DisplayName ||
                                member.fullName ||
                                member.displayName ||
                                "Nome não disponível"}
                            </Typography>
                            <Chip
                              label={getStatusLabel(
                                (member as any).Status || (member.status as any)
                              )}
                              size="small"
                              color={
                                getStatusColor(
                                  (member as any).Status ||
                                    (member.status as any)
                                ) as any
                              }
                            />
                          </Box>
                          <Box
                            sx={{
                              display: "flex",
                              flexDirection: "column",
                              gap: 0.5,
                            }}
                          >
                            <Box
                              sx={{
                                display: "flex",
                                alignItems: "center",
                                gap: 1,
                              }}
                            >
                              <Typography
                                variant="body2"
                                sx={{
                                  fontSize: "0.875rem",
                                  color: "text.secondary",
                                }}
                              >
                                {(() => {
                                  const isAdmin =
                                    !(
                                      (member as any).ClubName ||
                                      member.clubName
                                    ) &&
                                    !(
                                      (member as any).UnitName ||
                                      member.unitName
                                    );

                                  if (isAdmin) {
                                    // Para administradores, mostrar as roles se disponíveis
                                    const allRoles =
                                      (member as any).AllRoles ||
                                      member.allRoles;
                                    if (allRoles) {
                                      return allRoles;
                                    }
                                    return "Administrador";
                                  }

                                  return `${
                                    (member as any).ClubName ||
                                    member.clubName ||
                                    "Sem Clube"
                                  } • ${
                                    (member as any).UnitName ||
                                    member.unitName ||
                                    "Sem Unidade"
                                  }`;
                                })()}
                              </Typography>
                            </Box>

                            <Box
                              sx={{
                                display: "flex",
                                alignItems: "center",
                                gap: 2,
                              }}
                            >
                              <Typography
                                variant="caption"
                                sx={{
                                  fontSize: "0.75rem",
                                  color: "text.secondary",
                                }}
                              >
                                {(member as any).Age || member.age || 0} anos
                              </Typography>

                              {((member as any).PrimaryEmail ||
                                member.primaryEmail) && (
                                <Box
                                  sx={{
                                    display: "flex",
                                    alignItems: "center",
                                    gap: 0.5,
                                  }}
                                >
                                  <Email fontSize="small" color="action" />
                                  <Typography
                                    variant="caption"
                                    sx={{
                                      fontSize: "0.75rem",
                                      color: "text.secondary",
                                    }}
                                  >
                                    {(member as any).PrimaryEmail ||
                                      member.primaryEmail}
                                  </Typography>
                                </Box>
                              )}

                              <Typography
                                variant="caption"
                                sx={{
                                  fontSize: "0.75rem",
                                  color: "text.secondary",
                                }}
                              >
                                Cadastrado em{" "}
                                {formatDate(
                                  (member as any).CreatedAt || member.createdAt
                                )}
                              </Typography>
                            </Box>
                          </Box>
                        </Box>

                        <ListItemSecondaryAction>
                          <IconButton
                            onClick={(e) => handleMenuOpen(e, member.id)}
                            size="small"
                          >
                            <MoreVert />
                          </IconButton>

                          <Menu
                            anchorEl={anchorEl[member.id]}
                            open={Boolean(anchorEl[member.id])}
                            onClose={() => handleMenuClose(member.id)}
                          >
                            <MenuItem
                              onClick={() => onMemberAction("view", member.id)}
                            >
                              <Visibility sx={{ mr: 1 }} />
                              Visualizar
                            </MenuItem>
                            <MenuItem
                              onClick={() => onMemberAction("edit", member.id)}
                            >
                              <Edit sx={{ mr: 1 }} />
                              Editar
                            </MenuItem>
                            <MenuItem
                              onClick={() =>
                                onMemberAction("transfer", member.id)
                              }
                            >
                              <TransferWithinAStation sx={{ mr: 1 }} />
                              Transferir
                            </MenuItem>
                            <Divider />
                            <MenuItem
                              onClick={() =>
                                onMemberAction("delete", member.id)
                              }
                              sx={{ color: "error.main" }}
                            >
                              <Delete sx={{ mr: 1 }} />
                              Excluir
                            </MenuItem>
                          </Menu>
                        </ListItemSecondaryAction>
                      </ListItem>

                      {index < groupData.members.length - 1 && <Divider />}
                    </React.Fragment>
                  );
                })}
              </List>
            </Collapse>
          </CardContent>
        </Card>
      ))}
    </Box>
  );
};

export const MembersList = memo(MembersListComponent);
