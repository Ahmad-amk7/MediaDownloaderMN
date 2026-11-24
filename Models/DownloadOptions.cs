namespace MediaDownloaderM.Models;

/// <summary>إعدادات عملية التحميل.</summary>
public sealed class DownloadOptions
{
    public required string Url { get; init; }
    public required string OutputDirectory { get; init; }
    public required string YtDlpPath { get; init; }
    public required string ToolsDirectory { get; init; }
    public required int QuickMode { get; init; }

    /// <summary>صيغة أو مجموعة صيغ بصيغة yt-dlp مثل "137+140". null يعني الوضع السريع.</summary>
    public string? FormatSelector { get; init; }

    /// <summary>دمج المخرجات في ملف mp4 واحد.</summary>
    public bool MergeOutputMp4 { get; init; }

    /// <summary>استخراج الصوت إلى MP3.</summary>
    public bool ExtractMp3 { get; init; }

    public bool Playlist { get; init; }
    public string? CookiesFile { get; init; }
}
