using System.Globalization;
using System.Text.Json;

namespace MediaDownloaderM.Models;

/// <summary>صيغة واحدة كما يبلغ عنها yt-dlp.</summary>
public sealed class FormatInfo
{
    public string Id { get; init; } = "";
    public string Ext { get; init; } = "";
    public string Protocol { get; init; } = "";
    public string FormatNote { get; init; } = "";
    public int? Width { get; init; }
    public int? Height { get; init; }
    public double? Fps { get; init; }
    public string VideoCodec { get; init; } = "";
    public string AudioCodec { get; init; } = "";
    public long? FileSize { get; init; }
    public long? ApproxSize { get; init; }
    public double? Bitrate { get; init; }
    public string AudioSampleRate { get; init; } = "";

    public bool HasVideo => !string.IsNullOrEmpty(VideoCodec) &&
                            !string.Equals(VideoCodec, "none", StringComparison.OrdinalIgnoreCase);

    public bool HasAudio => !string.IsNullOrEmpty(AudioCodec) &&
                            !string.Equals(AudioCodec, "none", StringComparison.OrdinalIgnoreCase);

    public string KindKey => HasVideo && HasAudio ? "fmt.both" :
                             HasVideo ? "fmt.videoOnly" :
                             HasAudio ? "fmt.audioOnly" : "fmt.other";

    public string Resolution => Width.HasValue && Height.HasValue
        ? $"{Width}x{Height}"
        : Height.HasValue ? $"{Height}p" : "—";

    public string SizeText => FormatBytes(FileSize ?? ApproxSize);

    public string BitrateText => Bitrate.HasValue
        ? $"{Bitrate.Value:0} kbps" : "—";

    public string Codec(string which) =>
        string.IsNullOrEmpty(which) || which == "none" ? "—" : which;

    public static FormatInfo FromJson(JsonElement element)
    {
        double? bitrate = GetDouble(element, "tbr");
        if (bitrate is null)
        {
            double vbr = GetDouble(element, "vbr") ?? 0;
            double abr = GetDouble(element, "abr") ?? 0;
            if (vbr + abr > 0)
                bitrate = vbr + abr;
        }

        return new FormatInfo
        {
            Id = GetString(element, "format_id"),
            Ext = GetString(element, "ext"),
            Protocol = GetString(element, "protocol"),
            FormatNote = FirstNonEmpty(GetString(element, "format_note"), GetString(element, "format")),
            Width = GetInt(element, "width"),
            Height = GetInt(element, "height"),
            Fps = GetDouble(element, "fps"),
            VideoCodec = GetString(element, "vcodec"),
            AudioCodec = GetString(element, "acodec"),
            FileSize = GetLong(element, "filesize"),
            ApproxSize = GetLong(element, "filesize_approx"),
            Bitrate = bitrate,
            AudioSampleRate = GetInt(element, "asr")?.ToString(CultureInfo.InvariantCulture) ?? ""
        };
    }

    public static string FormatBytes(long? bytes)
    {
        if (bytes is null || bytes <= 0)
            return "—";

        double value = bytes.Value;
        string[] units = { "B", "KiB", "MiB", "GiB" };
        int index = 0;
        while (value >= 1024 && index < units.Length - 1)
        {
            value /= 1024;
            index++;
        }
        return $"{value:0.#} {units[index]}";
    }

    private static string FirstNonEmpty(string first, string second) =>
        !string.IsNullOrWhiteSpace(first) ? first : second;

    private static string GetString(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String
            ? v.GetString() ?? ""
            : "";

    private static int? GetInt(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out int i)
            ? i
            : null;

    private static long? GetLong(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out long l)
            ? l
            : null;

    private static double? GetDouble(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out double d)
            ? d
            : null;
}
