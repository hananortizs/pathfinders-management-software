import React, { useState, useEffect } from "react";
import {
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Box,
} from "@mui/material";
import type { TextFieldProps } from "@mui/material";
import {
  parsePhoneNumber,
  isValidPhoneNumber as libIsValidPhoneNumber,
} from "react-phone-number-input";
import { getCountryCallingCode } from "libphonenumber-js";
import type { CountryCode } from "libphonenumber-js";

interface PhoneInputWithDDIProps
  extends Omit<TextFieldProps, "onChange" | "value"> {
  value: string;
  onChange: (value: string) => void;
  onValidationChange?: (isValid: boolean) => void;
  defaultCountry?: string;
}

/**
 * Componente de input de telefone com seleção de DDI
 * Permite selecionar o país e digitar apenas o DDD + número
 */
export const PhoneInputWithDDI: React.FC<PhoneInputWithDDIProps> = ({
  value,
  onChange,
  onValidationChange,
  defaultCountry = "BR",
  error,
  helperText,
  ...props
}) => {
  const [selectedCountry, setSelectedCountry] = useState(defaultCountry);
  const [phoneNumber, setPhoneNumber] = useState("");
  const [isValid, setIsValid] = useState(true);

  // Países mais comuns para o Brasil
  const countries = [
    { code: "BR", name: "Brasil", flag: "🇧🇷", callingCode: "+55" },
    { code: "US", name: "Estados Unidos", flag: "🇺🇸", callingCode: "+1" },
    { code: "AR", name: "Argentina", flag: "🇦🇷", callingCode: "+54" },
    { code: "CL", name: "Chile", flag: "🇨🇱", callingCode: "+56" },
    { code: "UY", name: "Uruguai", flag: "🇺🇾", callingCode: "+598" },
    { code: "PY", name: "Paraguai", flag: "🇵🇾", callingCode: "+595" },
    { code: "BO", name: "Bolívia", flag: "🇧🇴", callingCode: "+591" },
    { code: "PE", name: "Peru", flag: "🇵🇪", callingCode: "+51" },
    { code: "CO", name: "Colômbia", flag: "🇨🇴", callingCode: "+57" },
    { code: "VE", name: "Venezuela", flag: "🇻🇪", callingCode: "+58" },
    { code: "EC", name: "Equador", flag: "🇪🇨", callingCode: "+593" },
    { code: "GY", name: "Guiana", flag: "🇬🇾", callingCode: "+592" },
    { code: "SR", name: "Suriname", flag: "🇸🇷", callingCode: "+597" },
    { code: "GF", name: "Guiana Francesa", flag: "🇬🇫", callingCode: "+594" },
    { code: "FR", name: "França", flag: "🇫🇷", callingCode: "+33" },
    { code: "DE", name: "Alemanha", flag: "🇩🇪", callingCode: "+49" },
    { code: "IT", name: "Itália", flag: "🇮🇹", callingCode: "+39" },
    { code: "ES", name: "Espanha", flag: "🇪🇸", callingCode: "+34" },
    { code: "PT", name: "Portugal", flag: "🇵🇹", callingCode: "+351" },
    { code: "GB", name: "Reino Unido", flag: "🇬🇧", callingCode: "+44" },
  ];

  // Inicializa o valor quando o componente monta
  useEffect(() => {
    if (value) {
      try {
        const parsed = parsePhoneNumber(value);
        if (parsed) {
          setSelectedCountry(parsed.country || defaultCountry);
          setPhoneNumber(parsed.nationalNumber || "");
        }
      } catch (error) {
        // Se não conseguir fazer parse, assume que é um número nacional
        setPhoneNumber(value);
      }
    }
  }, [value, defaultCountry]);

  // Valida o telefone sempre que o valor muda
  useEffect(() => {
    const fullNumber =
      selectedCountry === "BR"
        ? `+55${phoneNumber}`
        : `+${getCountryCallingCode(
            selectedCountry as CountryCode
          )}${phoneNumber}`;
    const valid = !phoneNumber || libIsValidPhoneNumber(fullNumber);
    setIsValid(valid);
    onValidationChange?.(valid);
  }, [phoneNumber, selectedCountry, onValidationChange]);

  const handleCountryChange = (event: any) => {
    const newCountry = event.target.value;
    setSelectedCountry(newCountry);

    // Atualiza o valor completo
    const fullNumber =
      newCountry === "BR"
        ? `+55${phoneNumber}`
        : `+${getCountryCallingCode(newCountry as CountryCode)}${phoneNumber}`;
    onChange(fullNumber);
  };

  const handlePhoneChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const inputValue = event.target.value;

    // Remove caracteres não numéricos
    const numbersOnly = inputValue.replace(/\D/g, "");

    // Limita o tamanho baseado no país
    const maxLength = selectedCountry === "BR" ? 11 : 15;
    const limitedNumbers = numbersOnly.slice(0, maxLength);

    setPhoneNumber(limitedNumbers);

    // Atualiza o valor completo
    const fullNumber =
      selectedCountry === "BR"
        ? `+55${limitedNumbers}`
        : `+${getCountryCallingCode(
            selectedCountry as CountryCode
          )}${limitedNumbers}`;
    onChange(fullNumber);
  };

  const formatPhoneDisplay = (number: string, country: string) => {
    if (!number) return "";

    if (country === "BR") {
      // Formatação brasileira: (11) 99999-9999
      if (number.length <= 2) return number;
      if (number.length <= 6)
        return `(${number.slice(0, 2)}) ${number.slice(2)}`;
      if (number.length <= 10)
        return `(${number.slice(0, 2)}) ${number.slice(2, 6)}-${number.slice(
          6
        )}`;
      return `(${number.slice(0, 2)}) ${number.slice(2, 7)}-${number.slice(7)}`;
    }

    // Para outros países, retorna o número sem formatação específica
    return number;
  };

  // Removido variável não utilizada

  return (
    <Box
      sx={{
        display: "flex",
        gap: { xs: 2, sm: 2 },
        alignItems: "flex-start",
        flexDirection: { xs: "column", sm: "row" },
        width: "100%",
      }}
    >
      {/* Seletor de País */}
      <FormControl
        sx={{
          minWidth: { xs: "100%", sm: 140, md: 160 },
          width: { xs: "100%", sm: "auto" },
        }}
      >
        <InputLabel id="country-select-label">País</InputLabel>
        <Select
          labelId="country-select-label"
          value={selectedCountry}
          onChange={handleCountryChange}
          label="País"
          size="small"
          sx={{
            minHeight: { xs: 48, sm: 40 }, // Altura mínima para mobile
            fontSize: { xs: "16px", sm: "14px" }, // Previne zoom no iOS
          }}
        >
          {countries.map((country) => (
            <MenuItem key={country.code} value={country.code}>
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                  gap: 1,
                  minWidth: 0,
                  width: "100%",
                }}
              >
                <span style={{ fontSize: "1.2em" }}>{country.flag}</span>
                <span
                  style={{
                    fontSize: "0.9em",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                    whiteSpace: "nowrap",
                  }}
                >
                  {country.name}
                </span>
                <span
                  style={{
                    color: "#666",
                    fontSize: "0.8em",
                    marginLeft: "auto",
                    flexShrink: 0,
                  }}
                >
                  {country.callingCode}
                </span>
              </Box>
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      {/* Input de Telefone */}
      <TextField
        {...props}
        value={formatPhoneDisplay(phoneNumber, selectedCountry)}
        onChange={handlePhoneChange}
        error={error || (!isValid && phoneNumber.length > 0)}
        helperText={
          helperText ||
          (!isValid && phoneNumber.length > 0 ? "Telefone inválido" : undefined)
        }
        placeholder={
          selectedCountry === "BR" ? "(11) 99999-9999" : "Número do telefone"
        }
        sx={{
          flex: 1,
          minWidth: { xs: "100%", sm: 200 },
          "& .MuiInputBase-input": {
            minHeight: { xs: 48, sm: 40 }, // Altura mínima para mobile
            fontSize: { xs: "16px", sm: "14px" }, // Previne zoom no iOS
            padding: { xs: "12px 14px", sm: "8px 14px" },
          },
        }}
      />
    </Box>
  );
};
