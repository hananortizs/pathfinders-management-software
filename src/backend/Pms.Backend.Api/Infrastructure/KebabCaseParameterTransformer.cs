using Microsoft.AspNetCore.Routing;

namespace Pms.Backend.Api.Infrastructure;

/// <summary>
/// Parameter transformer that converts controller names to kebab-case format.
/// This allows [controller] tokens in routes to be automatically converted to kebab-case.
/// For example: MemberController becomes "member", HierarchyController becomes "hierarchy".
/// </summary>
public class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    /// <summary>
    /// Transforms the parameter value to kebab-case format for outbound routes.
    /// </summary>
    /// <param name="value">The parameter value to transform (typically a controller name)</param>
    /// <returns>The transformed value in kebab-case format, or null if the input is null</returns>
    public string? TransformOutbound(object? value)
    {
        if (value == null)
        {
            return null;
        }

        var stringValue = value.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            return stringValue;
        }

        // Remove "Controller" suffix if present
        if (stringValue.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
        {
            stringValue = stringValue.Substring(0, stringValue.Length - "Controller".Length);
        }

        // Convert to kebab-case
        return ConvertToKebabCase(stringValue);
    }

    /// <summary>
    /// Converts a PascalCase string to kebab-case.
    /// </summary>
    /// <param name="input">The input string in PascalCase</param>
    /// <returns>The string converted to kebab-case</returns>
    private static string ConvertToKebabCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var result = new System.Text.StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];

            // Add hyphen before uppercase letters (except for the first character)
            if (char.IsUpper(currentChar) && i > 0)
            {
                result.Append('-');
            }

            // Convert to lowercase
            result.Append(char.ToLower(currentChar));
        }

        return result.ToString();
    }
}
