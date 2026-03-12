using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lamie.API.Models.MasterData;

public sealed class TagTranslationRequest
{
    [Required]
    public string LanguageCode { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}

public sealed class CreateTagRequest
{
    public bool IsActive { get; set; } = true;

    [Required]
    [MinLength(1)]
    public List<TagTranslationRequest> Translations { get; set; } = [];
}

public sealed class UpdateTagRequest
{
    public bool IsActive { get; set; } = true;

    [Required]
    [MinLength(1)]
    public List<TagTranslationRequest> Translations { get; set; } = [];
}

