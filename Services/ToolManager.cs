using System.IO.Compression;

namespace MediaDownloaderM.Services;

/// <summary>يجھّز yt-dlp و ffmpeg في %LOCALAPPDATA%\MediaDownloader\tools.</summary>
public sealed class ToolManager
{
    private const string YtDlpUrl =
        "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

    private static readonly string[] FfmpegUrls =
    {
        "https://github.com/FFmpeg-Builds/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-lgpl-shared.zip",
        "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip"
    };

    public string ToolsDirectory { get; }
    public string YtDlpPath { get; }
    public string FfmpegPath { get; }

    public ToolManager()
    {
        ToolsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MediaDownloader", "tools");
        YtDlpPath = Path.Combine(ToolsDirectory, "yt-dlp.exe");
        FfmpegPath = Path.Combine(ToolsDirectory, "ffmpeg.exe");
    }

    public bool HasYtDlp => IsUsable(YtDlpPath, 50_000);
    public bool HasFfmpeg => IsUsable(FfmpegPath, 100_000);

    private static bool IsUsable(string path, long minBytes) =>
        File.Exists(path) && new FileInfo(path).Length > minBytes;

    public async Task<string> EnsureYtDlpAsync(
        Action<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(ToolsDirectory);
        if (HasYtDlp)
            return YtDlpPath;

        string temp = YtDlpPath + ".tmp";
        await DownloadAsync(new[] { YtDlpUrl }, temp, progress, cancellationToken)
            .ConfigureAwait(false);
        MoveIntoPlace(temp, YtDlpPath);
        return YtDlpPath;
    }

    public async Task<string> EnsureFfmpegAsync(
        bool force = false,
        Action<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(ToolsDirectory);
        if (!force && HasFfmpeg)
            return FfmpegPath;

        string zipPath = Path.Combine(ToolsDirectory, "_ffmpeg_download.zip");
        await DownloadAsync(FfmpegUrls, zipPath, progress, cancellationToken)
            .ConfigureAwait(false);

        await Task.Run(() => ExtractFfmpeg(zipPath, ToolsDirectory), cancellationToken)
            .ConfigureAwait(false);

        try { File.Delete(zipPath); } catch { /* تجاهل */ }

        if (!HasFfmpeg)
            throw new InvalidOperationException("ffmpeg.exe was not found in the downloaded archive.");

        return FfmpegPath;
    }

    private static void MoveIntoPlace(string temp, string destination)
    {
        if (File.Exists(destination))
            File.Delete(destination);
        File.Move(temp, destination);
    }

    private static void ExtractFfmpeg(string zipPath, string toolsDirectory)
    {
        using var archive = ZipFile.OpenRead(zipPath);
        foreach (var entry in archive.Entries)
        {
            string name = Path.GetFileName(entry.FullName.Replace('\\', '/'));
            if (string.IsNullOrEmpty(name))
                continue;

            string folder = Path.GetDirectoryName(entry.FullName.Replace('\\', '/')) ?? string.Empty;
            string folderName = new DirectoryInfo(folder).Name;
            bool isBin = string.Equals(folderName, "bin", StringComparison.OrdinalIgnoreCase);
            bool wanted =
                string.Equals(name, "ffmpeg.exe", StringComparison.OrdinalIgnoreCase) ||
                (isBin && (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                           name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)));

            if (wanted)
                entry.ExtractToFile(Path.Combine(toolsDirectory, name), overwrite: true);
        }
    }

    private static async Task DownloadAsync(
        IReadOnlyList<string> urls,
        string destination,
        Action<double>? progress,
        CancellationToken cancellationToken)
    {
        string? directory = Path.GetDirectoryName(destination);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        Exception? lastError = null;

        foreach (string url in urls)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(20) };
                using var response = await client.GetAsync(
                    url, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                long? total = response.Content.Headers.ContentLength;
                await using var source = await response.Content
                    .ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
                await using var target = new FileStream(
                    destination, FileMode.Create, FileAccess.Write, FileShare.None,
                    81920, useAsync: true);

                var buffer = new byte[81920];
                long downloaded = 0;
                int count;

                while ((count = await source.ReadAsync(buffer.AsMemory(), cancellationToken)
                           .ConfigureAwait(false)) > 0)
                {
                    await target.WriteAsync(buffer.AsMemory(0, count), cancellationToken)
                        .ConfigureAwait(false);
                    downloaded += count;
                    if (total is > 0)
                        progress?.Invoke(Math.Min(100.0, 100.0 * downloaded / total.Value));
                }

                progress?.Invoke(100.0);
                return;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                lastError = ex;
                progress?.Invoke(0);
            }
        }

        throw new InvalidOperationException(
            "Could not download the file from any available source. Check your internet connection.",
            lastError);
    }
}
