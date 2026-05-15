using AutoMapper;
using Lamie.Application.Settings.Attributes.Categories;
using Lamie.Application.Settings.Attributes.Collections;
using Lamie.Application.Settings.Attributes.Colors;
using Lamie.Application.Settings.Attributes.Languages;
using Lamie.Application.Settings.Attributes.Occasions;
using Lamie.Application.Settings.Attributes.Styles;
using Lamie.Application.Settings.Attributes.Tags;
using Lamie.Domain.Entities;

namespace Lamie.Application;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Language, LanguageDto>();

        CreateMap<TagTranslation, TagTranslationDto>();
        CreateMap<Tag, TagDto>();

        CreateMap<ColorTranslation, ColorTranslationDto>();
        CreateMap<Color, ColorDto>();

        CreateMap<CategoryTranslation, CategoryTranslationDto>();
        CreateMap<Category, CategoryDto>();

        CreateMap<CollectionTranslation, CollectionTranslationDto>();
        CreateMap<Collection, CollectionDto>();

        CreateMap<OccasionTranslation, OccasionTranslationDto>();
        CreateMap<Occasion, OccasionDto>();

        CreateMap<StyleTranslation, StyleTranslationDto>();
        CreateMap<Style, StyleDto>();
    }
}
