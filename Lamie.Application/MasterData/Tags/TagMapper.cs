using Lamie.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Lamie.Application.MasterData.Tags;

[Mapper]
public partial class TagMapper
{
    [MapperIgnoreSource(nameof(Tag.CreatedBy))]
    [MapperIgnoreSource(nameof(Tag.CreatedName))]
    [MapperIgnoreSource(nameof(Tag.CreatedAt))]
    [MapperIgnoreSource(nameof(Tag.UpdatedBy))]
    [MapperIgnoreSource(nameof(Tag.UpdatedName))]
    [MapperIgnoreSource(nameof(Tag.UpdatedAt))]
    public partial TagDto ToDto(Tag tag);

    [MapperIgnoreSource(nameof(TagTranslation.Id))]
    [MapperIgnoreSource(nameof(TagTranslation.TagId))]
    [MapperIgnoreSource(nameof(TagTranslation.CreatedBy))]
    [MapperIgnoreSource(nameof(TagTranslation.CreatedName))]
    [MapperIgnoreSource(nameof(TagTranslation.CreatedAt))]
    [MapperIgnoreSource(nameof(TagTranslation.UpdatedBy))]
    [MapperIgnoreSource(nameof(TagTranslation.UpdatedName))]
    [MapperIgnoreSource(nameof(TagTranslation.UpdatedAt))]
    public partial TagTranslationDto ToDto(TagTranslation translation);
}

