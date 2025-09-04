using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Pms.Backend.Api.Infrastructure;

/// <summary>
/// Application model convention that applies kebab-case transformation to controller routes.
/// This ensures that [controller] tokens in routes are automatically converted to kebab-case.
/// </summary>
public class KebabCaseRoutingConvention : IControllerModelConvention
{
    /// <summary>
    /// Applies the kebab-case transformation to controller routes.
    /// </summary>
    /// <param name="controller">The controller model to modify</param>
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel?.Template != null)
            {
                // Replace [controller] with kebab-case version
                selector.AttributeRouteModel.Template = TransformControllerName(selector.AttributeRouteModel.Template, controller.ControllerName);
            }
        }
    }

    /// <summary>
    /// Transforms the route template by replacing [controller] with kebab-case version.
    /// </summary>
    /// <param name="template">The original route template</param>
    /// <param name="controllerName">The controller name</param>
    /// <returns>The transformed route template</returns>
    private static string TransformControllerName(string template, string controllerName)
    {
        if (string.IsNullOrEmpty(template) || string.IsNullOrEmpty(controllerName))
        {
            return template;
        }

        // Remove "Controller" suffix if present
        var cleanControllerName = controllerName;
        if (cleanControllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
        {
            cleanControllerName = cleanControllerName.Substring(0, cleanControllerName.Length - "Controller".Length);
        }

        // Convert to kebab-case
        var kebabCaseName = ConvertToKebabCase(cleanControllerName);

        // Replace [controller] with kebab-case version
        return template.Replace("[controller]", kebabCaseName, StringComparison.OrdinalIgnoreCase);
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
