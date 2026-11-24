using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MediaDownloaderM.Models;

namespace MediaDownloaderM.Services;

public sealed record DownloadProgressSnapshot(double Percent, string Speed, string Eta);

public sealed record DownloadResult(
    bool Success,
    int ExitCode,
    IReadOnlyList<string> Files,
    string? Error);

/// <summary>يشغّل yt-dlp ويتابع تقدّمه.</summary>
public sealed class DownloadEngine
{
    private static readonly Regex PercentRegex = new(
        @"\[download\]\s+(?<p>\d+(?:[.,]\d+)?)%", RegexOptions.Compiled);
    private static readonly Regex SpeedRegex = new(
        @"at\s+(?<s>\d+(?:[.,]\d+)?\s*[KMGTPE]?i?B/s)", RegexOptions.Compiled);
    private static readonly Regex EtaRegex = new(
        @"ETA\s+(?<e>\d+:\d+(?::\d+)?)", RegexOptions.Compiled);

    public async Task<DownloadResult> RunAsync(
        DownloadOptions options,
        IProgress<DownloadProgressSnapshot> progress,
        Action<string> log,
        CancellationToken cancellationToken)
    {
        var start = new ProcessStartInfo
        {
            FileName = options.YtDlpPath,
            WorkingDirectory = options.OutputDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        start.ArgumentList.Add("--no-colors");
        start.ArgumentList.Add("--newline");
        start.ArgumentList.Add("--add-metadata");
        start.ArgumentList.Add("--socket-timeout");
        start.ArgumentList.Add("20");
        start.ArgumentList.Add("--retries");
        start.ArgumentList.Add("5");
        start.ArgumentList.Add("--ffmpeg-location");
        start.ArgumentList.Add(options.ToolsDirectory);

        if (!options.Playlist)
            start.ArgumentList.Add("--no-playlist");

        if (!string.IsNullOrWhiteSpace(options.CookiesFile))
        {
            start.ArgumentList.Add("--cookies");
            start.ArgumentList.Add(options.CookiesFile);
        }

        foreach (string argument in FormatArguments(options))
            start.ArgumentList.Add(argument);

        start.ArgumentList.Add("-o");
        start.ArgumentList.Add(Path.Combine(
            options.OutputDirectory, "%(title)s [%(id)s].%(ext)s"));
        start.ArgumentList.Add(options.Url);

        var files = new List<string>();
        string? error = null;

        void HandleLine(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            log(line);

            const string marker = "Destination: ";
            int index = line.IndexOf(marker, StringComparison.Ordinal);
            if (index >= 0)
            {
                string path = line[(index + marker.Length)..].Trim();
                if (path.Length > 0)
                    files.Add(path);
            }

            Match percent = PercentRegex.Match(line);
            if (percent.Success &&
                double.TryParse(percent.Groups["p"].Value.Replace(',', '.'),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                string speed = SpeedRegex.Match(line).Groups["s"].Value;
                string eta = EtaRegex.Match(line).Groups["e"].Value;
                progress.Report(new DownloadProgressSnapshot(
                    value,
                    speed.Length > 0 ? speed : "—",
                    eta.Length > 0 ? eta : "—"));
            }

            if (line.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
                error = line;
        }

        using var process = new Process { StartInfo = start };
        process.OutputDataReceived += (_, e) => HandleLine(e.Data);
        process.ErrorDataReceived += (_, e) => HandleLine(e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var killOnCancel = cancellationToken.Register(() =>
        {
            try
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
            }
            catch { /* العملية انتهت */ }
        });

        await process.WaitForExitAsync().ConfigureAwait(false);

        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException(cancellationToken);

        string[] finalFiles = files
            .Where(File.Exists)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new DownloadResult(
            process.ExitCode == 0,
            process.ExitCode,
            finalFiles,
            error);
    }

    private static string[] FormatArguments(DownloadOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.FormatSelector))
        {
            var arguments = new List<string> { "-f", options.FormatSelector };
            if (options.MergeOutputMp4)
                arguments.AddRange(new[] { "--merge-output-format", "mp4" });
            if (options.ExtractMp3)
                arguments.AddRange(new[] { "-x", "--audio-format", "mp3", "--audio-quality", "0" });
            return arguments.ToArray();
        }

        return options.QuickMode switch
        {
            0 => new[] { "-f", "bv*+ba/b", "--merge-output-format", "mp4" },
            1 => new[] { "-f", "bv*[height<=720]+ba/b[height<=720]/b", "--merge-output-format", "mp4" },
            2 => new[] { "-f", "bv*[height<=1080]+ba/b[height<=1080]/b", "--merge-output-format", "mp4" },
            3 => new[] { "-f", "bestaudio/best", "-x", "--audio-format", "mp3", "--audio-quality", "0" },
            4 => new[] { "-f", "bestaudio/best" },
            _ => new[] { "-f", "bv*+ba/b", "--merge-output-format", "mp4" }
        };
    }
}
