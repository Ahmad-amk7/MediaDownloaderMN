using System.Diagnostics;
using System.Text;
using System.Text.Json;
using MediaDownloaderM.Models;

namespace MediaDownloaderM.Services;

public sealed record ProbeResult(string Title, IReadOnlyList<FormatInfo> Formats);

/// <summary>يجلب قائمة الصيغ المتاحة من yt-dlp دون تنزيل الوسائط.</summary>
public sealed class FormatProbe
{
    public async Task<ProbeResult> ProbeAsync(
        string url,
        string ytDlpPath,
        string? cookiesFile,
        Action<string> log,
        CancellationToken cancellationToken)
    {
        var start = new ProcessStartInfo
        {
            FileName = ytDlpPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        start.ArgumentList.Add("--dump-single-json");
        start.ArgumentList.Add("--no-playlist");
        start.ArgumentList.Add("--no-colors");
        start.ArgumentList.Add("--socket-timeout");
        start.ArgumentList.Add("20");
        start.ArgumentList.Add("--retries");
        start.ArgumentList.Add("3");

        if (!string.IsNullOrWhiteSpace(cookiesFile))
        {
            start.ArgumentList.Add("--cookies");
            start.ArgumentList.Add(cookiesFile);
        }

        start.ArgumentList.Add(url);

        using var process = new Process { StartInfo = start };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                log(e.Data);
        };

        process.Start();
        process.BeginErrorReadLine();

        using var killOnCancel = cancellationToken.Register(() =>
        {
            try { if (!process.HasExited) process.Kill(entireProcessTree: true); }
            catch { /* العملية انتهت */ }
        });

        string json = await process.StandardOutput.ReadToEndAsync(cancellationToken)
            .ConfigureAwait(false);
        await process.WaitForExitAsync().ConfigureAwait(false);

        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException(cancellationToken);

        if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(json))
            throw new InvalidOperationException($"yt-dlp exited with code {process.ExitCode}");

        using var document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;

        string title = root.TryGetProperty("title", out var titleElement) &&
                       titleElement.ValueKind == JsonValueKind.String
            ? titleElement.GetString() ?? ""
            : "";

        var formats = new List<FormatInfo>();
        if (root.TryGetProperty("formats", out var array) &&
            array.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in array.EnumerateArray())
            {
                FormatInfo format = FormatInfo.FromJson(item);
                // استبعاد صور الفيديو (storyboards) التي لا قيمة لها بالتحميل.
                if (!format.HasVideo && !format.HasAudio)
                    continue;
                if (format.FormatNote.Contains("storyboard", StringComparison.OrdinalIgnoreCase))
                    continue;
                formats.Add(format);
            }
        }

        return new ProbeResult(title, formats);
    }
}
