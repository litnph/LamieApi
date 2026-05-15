using System.Security.Cryptography;

namespace Lamie.Shared.Guids;

/// <summary>
/// Generator for UUID v7 (RFC 9562). Time-ordered prefix gives good locality
/// on B-tree indexes (Postgres) compared to fully random Guids.
/// </summary>
public static class GuidV7
{
    public static Guid NewGuid() => NewGuid(DateTimeOffset.UtcNow);

    public static Guid NewGuid(DateTimeOffset timestamp)
    {
        Span<byte> bytes = stackalloc byte[16];
        RandomNumberGenerator.Fill(bytes);

        var unixMs = timestamp.ToUnixTimeMilliseconds();
        bytes[0] = (byte)(unixMs >> 40);
        bytes[1] = (byte)(unixMs >> 32);
        bytes[2] = (byte)(unixMs >> 24);
        bytes[3] = (byte)(unixMs >> 16);
        bytes[4] = (byte)(unixMs >> 8);
        bytes[5] = (byte)unixMs;

        bytes[6] = (byte)((bytes[6] & 0x0F) | 0x70);
        bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);

        return new Guid(bytes, bigEndian: true);
    }
}
