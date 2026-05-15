namespace Lamie.Application.Settings.Attributes.Channels;

public sealed record ChannelDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? IconUrl { get; init; }
    public bool IsActive { get; init; }
    public int SortOrder { get; init; }
}
