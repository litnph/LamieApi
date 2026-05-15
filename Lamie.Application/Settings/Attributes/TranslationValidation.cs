using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;

namespace Lamie.Application.Settings.Attributes;

internal static class TranslationValidation
{
    public static async Task EnsureValidAsync<T>(
        IReadOnlyList<T>? translations,
        Func<T, string> getLanguageCode,
        Func<T, string?> getName,
        ILanguageRepository languageRepository,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (translations is null || translations.Count == 0)
        {
            errors["translations"] = ["At least one translation is required"];
            throw new ValidationException(errors);
        }

        for (var i = 0; i < translations.Count; i++)
        {
            var code = getLanguageCode(translations[i]);
            var name = getName(translations[i]);

            if (string.IsNullOrWhiteSpace(code))
            {
                errors[$"translations[{i}].languageCode"] = ["LanguageCode is required"];
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                errors[$"translations[{i}].name"] = ["Name is required"];
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                var exists = await languageRepository.ExistsAsync(code, cancellationToken);
                if (!exists)
                {
                    errors[$"translations[{i}].languageCode"] = ["LanguageCode is not supported"];
                }
            }
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}
