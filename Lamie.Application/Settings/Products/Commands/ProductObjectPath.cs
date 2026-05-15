namespace Lamie.Application.Settings.Products.Commands;

internal static class ProductObjectPath
{
    public static string Build(string sku, string? originalFileName, int index)
    {
        var safeSku = string.IsNullOrWhiteSpace(sku) ? "unknown-sku" : SanitizeSegment(sku);
        var fileName = string.IsNullOrWhiteSpace(originalFileName) ? $"image-{index}" : originalFileName;

        var safeName = SanitizeSegment(Path.GetFileName(fileName));
        var ext = Path.GetExtension(safeName);
        var nameNoExt = Path.GetFileNameWithoutExtension(safeName);
        var stamp = Guid.NewGuid().ToString("N");

        var finalName = string.IsNullOrWhiteSpace(ext)
            ? $"{nameNoExt}-{stamp}"
            : $"{nameNoExt}-{stamp}{ext}";

        return $"products/{safeSku}/{finalName}";
    }

    private static string SanitizeSegment(string value)
    {
        var cleaned = value.Trim().Replace(' ', '-').Replace('\\', '-').Replace('/', '-');
        return string.IsNullOrWhiteSpace(cleaned) ? "x" : cleaned;
    }
}
