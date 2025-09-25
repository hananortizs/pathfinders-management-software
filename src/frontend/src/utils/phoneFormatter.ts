/**
 * Utilitários para formatação de telefone
 */

/**
 * Formata um número de telefone brasileiro
 * @param value - Valor a ser formatado
 * @returns Valor formatado
 */
export function formatPhoneNumber(value: string): string {
  // Remove todos os caracteres não numéricos
  const numbers = value.replace(/\D/g, "");

  // Limita a 11 dígitos (DDD + 9 dígitos)
  const limitedNumbers = numbers.slice(0, 11);

  // Aplica a formatação baseada no tamanho
  if (limitedNumbers.length <= 2) {
    return limitedNumbers;
  } else if (limitedNumbers.length <= 6) {
    return `(${limitedNumbers.slice(0, 2)}) ${limitedNumbers.slice(2)}`;
  } else if (limitedNumbers.length <= 10) {
    return `(${limitedNumbers.slice(0, 2)}) ${limitedNumbers.slice(
      2,
      6
    )}-${limitedNumbers.slice(6)}`;
  } else {
    return `(${limitedNumbers.slice(0, 2)}) ${limitedNumbers.slice(
      2,
      7
    )}-${limitedNumbers.slice(7)}`;
  }
}

/**
 * Remove a formatação do telefone, retornando apenas números
 * @param value - Valor formatado
 * @returns Apenas números
 */
export function unformatPhoneNumber(value: string): string {
  return value.replace(/\D/g, "");
}

/**
 * Valida se um telefone brasileiro é válido
 * @param value - Valor a ser validado
 * @returns true se válido, false caso contrário
 */
export function isValidPhoneNumber(value: string): boolean {
  const numbers = unformatPhoneNumber(value);

  // Telefone fixo: 10 dígitos (DDD + 8 dígitos)
  // Celular: 11 dígitos (DDD + 9 dígitos)
  return numbers.length === 10 || numbers.length === 11;
}

/**
 * Obtém a máscara de entrada baseada no tipo de contato
 * @param type - Tipo do contato
 * @returns Máscara apropriada
 */
export function getPhoneMask(type: string): string {
  if (type === "Phone" || type === "WhatsApp") {
    return "(99) 99999-9999";
  }
  return "";
}
