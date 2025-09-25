import React, { useState, useEffect } from "react";
import { TextField } from "@mui/material";
import type { TextFieldProps } from "@mui/material";
import {
  formatPhoneNumber,
  isValidPhoneNumber,
} from "../../utils/phoneFormatter";

interface PhoneInputProps extends Omit<TextFieldProps, "onChange" | "value"> {
  value: string;
  onChange: (value: string) => void;
  onValidationChange?: (isValid: boolean) => void;
}

/**
 * Componente de input com formatação automática de telefone
 * Permite digitação normal e formata visualmente
 */
export const PhoneInput: React.FC<PhoneInputProps> = ({
  value,
  onChange,
  onValidationChange,
  error,
  helperText,
  ...props
}) => {
  const [isValid, setIsValid] = useState(true);

  // Valida o telefone sempre que o valor muda
  useEffect(() => {
    const valid = !value || isValidPhoneNumber(value);
    setIsValid(valid);
    onValidationChange?.(valid);
  }, [value, onValidationChange]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const inputValue = event.target.value;

    // Remove caracteres não numéricos para processamento
    const numbersOnly = inputValue.replace(/\D/g, "");

    // Limita a 11 dígitos (DDD + 9 dígitos)
    const limitedNumbers = numbersOnly.slice(0, 11);

    // Chama onChange com apenas os números (sem formatação)
    onChange(limitedNumbers);
  };

  const handleBlur = () => {
    // Aplica formatação apenas no blur para não interferir na digitação
    const formatted = formatPhoneNumber(value);
    // Força uma re-renderização com o valor formatado
    if (formatted !== value) {
      onChange(value); // Mantém o valor original
    }
  };

  // Formata o valor apenas para exibição
  const displayValue = formatPhoneNumber(value);

  return (
    <TextField
      {...props}
      value={displayValue}
      onChange={handleChange}
      onBlur={handleBlur}
      error={error || (!isValid && value.length > 0)}
      helperText={
        helperText ||
        (!isValid && value.length > 0 ? "Telefone inválido" : undefined)
      }
      placeholder="(11) 99999-9999"
    />
  );
};
