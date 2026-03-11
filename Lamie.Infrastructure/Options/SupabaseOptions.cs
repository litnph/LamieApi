namespace Lamie.Infrastructure.Options;

public sealed class SupabaseOptions
{
    public const string SectionName = "Supabase";

    public string Url { get; init; } = default!;
    public string ServiceRoleKey { get; init; } = default!;
    public string StorageBucket { get; init; } = default!;
}

