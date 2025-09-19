/**
 * Componente de filtros para lista de membros
 */

import React, { memo, useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
  Button,
  Collapse,
  Grid,
  Autocomplete,
  Slider,
  IconButton,
  Tooltip,
} from "@mui/material";
import { Search, FilterList, Clear } from "@mui/icons-material";
import type { MemberFilters, UserLevel } from "../../types/members";
import { MemberStatus, MemberGender } from "../../types/members";

interface MemberFiltersProps {
  filters: MemberFilters;
  onFiltersChange: (filters: Partial<MemberFilters>) => void;
  onClearFilters: () => void;
  userLevel: UserLevel;
  hierarchyOptions?: {
    divisions: Array<{ id: string; name: string; memberCount: number }>;
    unions: Array<{ id: string; name: string; memberCount: number }>;
    regions: Array<{ id: string; name: string; memberCount: number }>;
    associations: Array<{ id: string; name: string; memberCount: number }>;
    districts: Array<{ id: string; name: string; memberCount: number }>;
    clubs: Array<{ id: string; name: string; memberCount: number }>;
    units: Array<{ id: string; name: string; memberCount: number }>;
  };
  loading?: boolean;
}

const MemberFiltersComponentImpl: React.FC<MemberFiltersProps> = ({
  filters,
  onFiltersChange,
  onClearFilters,
  userLevel: _userLevel,
  hierarchyOptions,
  loading = false,
}) => {
  const [expanded, setExpanded] = useState(false);
  const [ageRange, setAgeRange] = useState<[number, number]>([0, 100]);

  const statusOptions = [
    { value: MemberStatus.Active, label: "Ativo" },
    { value: MemberStatus.Pending, label: "Pendente" },
    { value: MemberStatus.Inactive, label: "Inativo" },
    { value: MemberStatus.Suspended, label: "Suspenso" },
  ];

  const genderOptions = [
    { value: MemberGender.Male, label: "Masculino" },
    { value: MemberGender.Female, label: "Feminino" },
    { value: MemberGender.Other, label: "Outro" },
  ];

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onFiltersChange({ search: event.target.value });
  };

  const handleStatusChange = (event: any) => {
    onFiltersChange({ status: event.target.value });
  };

  const handleGenderChange = (event: any) => {
    onFiltersChange({ gender: event.target.value });
  };

  const handleAgeRangeChange = (_event: Event, newValue: number | number[]) => {
    const [min, max] = newValue as number[];
    setAgeRange([min, max]);
    onFiltersChange({ ageRange: { min, max } });
  };

  const handleHierarchyChange = (level: string, value: string[]) => {
    onFiltersChange({ [`${level}Ids`]: value });
  };

  const getActiveFiltersCount = () => {
    let count = 0;
    if (filters.search) count++;
    if (filters.status && filters.status.length > 0) count++;
    if (filters.gender && filters.gender.length > 0) count++;
    if (filters.ageRange) count++;
    if (filters.clubIds && filters.clubIds.length > 0) count++;
    if (filters.unitIds && filters.unitIds.length > 0) count++;
    return count;
  };

  const activeFiltersCount = getActiveFiltersCount();

  return (
    <Card sx={{ mb: 2 }}>
      <CardContent sx={{ p: 2 }}>
        {/* Header com busca básica */}
        <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 2 }}>
          <TextField
            fullWidth
            placeholder="Buscar por nome, CPF, email..."
            value={filters.search || ""}
            onChange={handleSearchChange}
            InputProps={{
              startAdornment: (
                <Search sx={{ mr: 1, color: "text.secondary" }} />
              ),
            }}
            disabled={loading}
          />

          <Tooltip title="Filtros avançados">
            <IconButton
              onClick={() => setExpanded(!expanded)}
              color={activeFiltersCount > 0 ? "primary" : "default"}
            >
              <FilterList />
              {activeFiltersCount > 0 && (
                <Chip
                  label={activeFiltersCount}
                  size="small"
                  color="primary"
                  sx={{ ml: 1, minWidth: 20, height: 20 }}
                />
              )}
            </IconButton>
          </Tooltip>

          {activeFiltersCount > 0 && (
            <Button
              startIcon={<Clear />}
              onClick={onClearFilters}
              size="small"
              disabled={loading}
            >
              Limpar
            </Button>
          )}
        </Box>

        {/* Filtros avançados */}
        <Collapse in={expanded}>
          <Box sx={{ pt: 2, borderTop: 1, borderColor: "divider" }}>
            <Grid container spacing={2}>
              {/* Status */}
              <Box
                sx={{ gridColumn: { xs: "1 / -1", sm: "auto", md: "auto" } }}
              >
                <FormControl fullWidth size="small">
                  <InputLabel>Status</InputLabel>
                  <Select
                    multiple
                    value={filters.status || []}
                    onChange={handleStatusChange}
                    label="Status"
                    disabled={loading}
                    renderValue={(selected) => (
                      <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                        {selected.map((value) => (
                          <Chip
                            key={value}
                            label={
                              statusOptions.find((opt) => opt.value === value)
                                ?.label
                            }
                            size="small"
                          />
                        ))}
                      </Box>
                    )}
                  >
                    {statusOptions.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Box>

              {/* Gênero */}
              <Box
                sx={{ gridColumn: { xs: "1 / -1", sm: "auto", md: "auto" } }}
              >
                <FormControl fullWidth size="small">
                  <InputLabel>Gênero</InputLabel>
                  <Select
                    multiple
                    value={filters.gender || []}
                    onChange={handleGenderChange}
                    label="Gênero"
                    disabled={loading}
                    renderValue={(selected) => (
                      <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                        {selected.map((value) => (
                          <Chip
                            key={value}
                            label={
                              genderOptions.find((opt) => opt.value === value)
                                ?.label
                            }
                            size="small"
                          />
                        ))}
                      </Box>
                    )}
                  >
                    {genderOptions.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Box>

              {/* Faixa etária */}
              <Box
                sx={{ gridColumn: { xs: "1 / -1", sm: "auto", md: "auto" } }}
              >
                <Box>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    gutterBottom
                  >
                    Faixa etária: {ageRange[0]} - {ageRange[1]} anos
                  </Typography>
                  <Slider
                    value={ageRange}
                    onChange={handleAgeRangeChange}
                    valueLabelDisplay="auto"
                    min={0}
                    max={100}
                    step={1}
                    disabled={loading}
                  />
                </Box>
              </Box>

              {/* Clubes */}
              {hierarchyOptions?.clubs && (
                <Box
                  sx={{ gridColumn: { xs: "1 / -1", sm: "auto", md: "auto" } }}
                >
                  <Autocomplete
                    multiple
                    options={hierarchyOptions.clubs}
                    getOptionLabel={(option) =>
                      `${option.name} (${option.memberCount})`
                    }
                    value={hierarchyOptions.clubs.filter((club) =>
                      filters.clubIds?.includes(club.id)
                    )}
                    onChange={(_, value) =>
                      handleHierarchyChange(
                        "club",
                        value.map((v) => v.id)
                      )
                    }
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        label="Clubes"
                        size="small"
                        disabled={loading}
                      />
                    )}
                    renderTags={(value, getTagProps) =>
                      value.map((option, index) => (
                        <Chip
                          {...getTagProps({ index })}
                          key={option.id}
                          label={option.name}
                          size="small"
                        />
                      ))
                    }
                  />
                </Box>
              )}

              {/* Unidades */}
              {hierarchyOptions?.units && (
                <Box
                  sx={{ gridColumn: { xs: "1 / -1", sm: "auto", md: "auto" } }}
                >
                  <Autocomplete
                    multiple
                    options={hierarchyOptions.units}
                    getOptionLabel={(option) =>
                      `${option.name} (${option.memberCount})`
                    }
                    value={hierarchyOptions.units.filter((unit) =>
                      filters.unitIds?.includes(unit.id)
                    )}
                    onChange={(_, value) =>
                      handleHierarchyChange(
                        "unit",
                        value.map((v) => v.id)
                      )
                    }
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        label="Unidades"
                        size="small"
                        disabled={loading}
                      />
                    )}
                    renderTags={(value, getTagProps) =>
                      value.map((option, index) => (
                        <Chip
                          {...getTagProps({ index })}
                          key={option.id}
                          label={option.name}
                          size="small"
                        />
                      ))
                    }
                  />
                </Box>
              )}
            </Grid>
          </Box>
        </Collapse>
      </CardContent>
    </Card>
  );
};

export const MemberFiltersComponent = memo(MemberFiltersComponentImpl);
