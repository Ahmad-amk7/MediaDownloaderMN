using System.Diagnostics;
using System.Windows.Forms;
using MediaDownloaderM.Models;
using MediaDownloaderM.Services;

namespace MediaDownloaderM;

public partial class MainForm : Form
{
    private readonly ToolManager _tools = new();
    private readonly FormatProbe _probe = new();
    private readonly Localizer _loc = new();

    private List<FormatInfo> _formats = new();
    private CancellationTokenSource? _cts;
    private bool _busy;
    private bool _preparing = true;
    private int _savedFilterIndex;
    private int _savedModeIndex;

    public MainForm() : this(AppLanguage.Arabic)
    {
    }

    public MainForm(AppLanguage initialLanguage)
    {
        InitializeComponent();

        // يُضبط قبل WireEvents حتى لا يُطلق حدث تغيير اللغة أثناء التهيئة.
        cmbLanguage.SelectedIndex = initialLanguage switch
        {
            AppLanguage.German => 2,
            AppLanguage.English => 1,
            _ => 0
        };
        // cmbMode و cmbFilter يعبّأان داخل ApplyLanguage أدناه.

        try
        {
            txtFolder.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads", "MediaDownloader");
            Directory.CreateDirectory(txtFolder.Text);
        }
        catch
        {
            txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        WireEvents();
        ApplyLanguage(initialLanguage);
        UpdateUi();
        _ = PrepareToolsAsync();
    }

    /// <summary>تغيير لغة الواجهة برمجياً (يُستخدم أيضاً للاختبار).</summary>
    public void SetLanguage(AppLanguage language) => ApplyLanguage(language);

    private string L(string key) => _loc.Get(key);

    /// <summary>
    /// تغيير اتجاه التخطيط (RightToLeftLayout) بعد إنشاء مقابض Windows قد يسبب
    /// AccessViolationException؛ لذلك ننشئ نموذجاً جديداً باللغة المطلوبة وننقل حالة الواجهة إليه.
    /// </summary>
    private void SwitchLanguage(AppLanguage language)
    {
        if (IsDisposed || _loc.Language == language)
            return;

        if (cmbLanguage.DroppedDown)
            cmbLanguage.DroppedDown = false;

        UiState state = CaptureState();
        var next = new MainForm(language);
        next.RestoreState(state);
        next.FormClosed += (_, _) =>
        {
            if (!IsDisposed)
                Close();
        };

        next.Show();
        Hide();
    }

    private UiState CaptureState() => new()
    {
        Url = txtUrl.Text,
        Folder = txtFolder.Text,
        Cookies = txtCookies.Text,
        ModeIndex = Math.Clamp(cmbMode.SelectedIndex, 0, 4),
        FilterIndex = Math.Clamp(cmbFilter.SelectedIndex, 0, 3),
        MergeAudio = chkMergeAudio.Checked,
        ConvertMp3 = chkConvertMp3.Checked,
        Playlist = chkPlaylist.Checked,
        Formats = new List<FormatInfo>(_formats),
        SelectedIds = SelectedFormats().Select(f => f.Id).ToHashSet(StringComparer.OrdinalIgnoreCase),
        Log = txtLog.Text
    };

    private void RestoreState(UiState state)
    {
        txtUrl.Text = state.Url;
        txtFolder.Text = state.Folder;
        txtCookies.Text = state.Cookies;

        _savedModeIndex = state.ModeIndex;
        _savedFilterIndex = state.FilterIndex;
        cmbMode.SelectedIndex = _savedModeIndex;
        cmbFilter.SelectedIndex = _savedFilterIndex;

        chkMergeAudio.Checked = state.MergeAudio;
        chkConvertMp3.Checked = state.ConvertMp3;
        chkPlaylist.Checked = state.Playlist;

        _formats = new List<FormatInfo>(state.Formats);
        ApplyFilter();

        foreach (DataGridViewRow row in dgvFormats.Rows)
        {
            string id = (row.Tag as FormatInfo)?.Id ?? "";
            if (state.SelectedIds.Contains(id))
                row.Selected = true;
        }

        txtLog.Text = state.Log;
    }

    private sealed class UiState
    {
        public string Url { get; init; } = "";
        public string Folder { get; init; } = "";
        public string Cookies { get; init; } = "";
        public int ModeIndex { get; init; }
        public int FilterIndex { get; init; }
        public bool MergeAudio { get; init; }
        public bool ConvertMp3 { get; init; }
        public bool Playlist { get; init; }
        public List<FormatInfo> Formats { get; init; } = new();
        public HashSet<string> SelectedIds { get; init; } = new(StringComparer.OrdinalIgnoreCase);
        public string Log { get; init; } = "";
    }

    private void WireEvents()
    {
        cmbLanguage.SelectedIndexChanged += (_, _) =>
        {
            AppLanguage language = cmbLanguage.SelectedIndex switch
            {
                0 => AppLanguage.Arabic,
                2 => AppLanguage.German,
                _ => AppLanguage.English
            };

            // تأجيل التبديل حتى ينتهي Windows من إغلاق القائمة المنسدلة؛
            // إنشاء نموذج آخر أثناء القائمة المفتوحة يسبب AccessViolationException.
            if (!IsDisposed)
                BeginInvoke(() => SwitchLanguage(language));
        };

        cmbFilter.SelectedIndexChanged += (_, _) =>
        {
            _savedFilterIndex = Math.Clamp(cmbFilter.SelectedIndex, 0, 3);
            ApplyFilter();
        };

        txtUrl.TextChanged += (_, _) => UpdateUi();

        btnFetch.Click += async (_, _) => await FetchFormatsAsync();
        btnDownload.Click += async (_, _) => await DownloadAsync();
        btnCancel.Click += (_, _) =>
        {
            if (_busy)
            {
                _cts?.Cancel();
                SetStatus(L("status.cancel"));
            }
        };
        btnFfmpeg.Click += async (_, _) => await InstallFfmpegAsync();

        btnBrowseFolder.Click += (_, _) =>
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = L("folder.label"),
                SelectedPath = txtFolder.Text
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
                txtFolder.Text = dialog.SelectedPath;
        };

        btnOpenFolder.Click += (_, _) =>
        {
            if (!Directory.Exists(txtFolder.Text))
                Directory.CreateDirectory(txtFolder.Text);
            Process.Start(new ProcessStartInfo("explorer.exe", txtFolder.Text)
            {
                UseShellExecute = true
            });
        };

        btnBrowseCookies.Click += (_, _) =>
        {
            using var dialog = new OpenFileDialog
            {
                Title = L("cookies.label"),
                Filter = "cookies.txt|*.txt|*.*|*.*",
                CheckFileExists = true
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
                txtCookies.Text = dialog.FileName;
        };

        btnClearCookies.Click += (_, _) => txtCookies.Clear();
    }

    private void ApplyLanguage(AppLanguage language)
    {
        _loc.SetLanguage(language);
        bool arabic = language == AppLanguage.Arabic;

        Text = L("app.title");
        lblTitle.Text = L("app.title");
        lblSubtitle.Text = L("app.subtitle");
        lblUrl.Text = L("url.label");
        btnFetch.Text = L("fetch.button");
        lblMode.Text = L("mode.label");
        chkMergeAudio.Text = L("merge.check");
        chkConvertMp3.Text = L("mp3.check");
        chkPlaylist.Text = L("playlist.check");
        lblFolder.Text = L("folder.label");
        btnBrowseFolder.Text = L("browse.button");
        btnOpenFolder.Text = L("openFolder.button");
        lblCookies.Text = L("cookies.label");
        btnBrowseCookies.Text = L("choose.button");
        btnClearCookies.Text = L("clear.button");
        lblFormats.Text = L("formats.label");
        btnDownload.Text = L("download.button");
        btnCancel.Text = L("cancel.button");
        btnFfmpeg.Text = L("ffmpeg.button");
        lblHint.Text = L("hint");

        _savedModeIndex = cmbMode.SelectedIndex >= 0 ? cmbMode.SelectedIndex : _savedModeIndex;
        cmbMode.Items.Clear();
        for (int i = 0; i < 5; i++)
            cmbMode.Items.Add(L($"mode.{i}"));
        cmbMode.SelectedIndex = Math.Clamp(_savedModeIndex, 0, 4);

        cmbFilter.Items.Clear();
        for (int i = 0; i < 4; i++)
            cmbFilter.Items.Add(L($"filter.{i}"));
        cmbFilter.SelectedIndex = Math.Clamp(_savedFilterIndex, 0, 3);

        dgvFormats.Columns["colId"]!.HeaderText = L("col.id");
        dgvFormats.Columns["colKind"]!.HeaderText = L("col.kind");
        dgvFormats.Columns["colExt"]!.HeaderText = L("col.ext");
        dgvFormats.Columns["colRes"]!.HeaderText = L("col.res");
        dgvFormats.Columns["colFps"]!.HeaderText = L("col.fps");
        dgvFormats.Columns["colVideo"]!.HeaderText = L("col.video");
        dgvFormats.Columns["colAudio"]!.HeaderText = L("col.audio");
        dgvFormats.Columns["colSize"]!.HeaderText = L("col.size");
        dgvFormats.Columns["colBitrate"]!.HeaderText = L("col.bitrate");
        dgvFormats.Columns["colNote"]!.HeaderText = L("col.note");

        SuspendLayout();
        RightToLeft = arabic
            ? System.Windows.Forms.RightToLeft.Yes
            : System.Windows.Forms.RightToLeft.No;
        // لا تُغيّر بعد إنشاء المقبض؛ النموذج الجديد يُنشأ بالاتجاه الصحيح منذ البداية.
        if (!IsHandleCreated)
            RightToLeftLayout = arabic;
        ResumeLayout(true);

        ApplyFilter();
        SetStatus(L("status.ready"));
    }

    private async Task PrepareToolsAsync()
    {
        _preparing = true;
        UpdateUi();
        SetStatus(L("status.tools"));

        try
        {
            await _tools.EnsureYtDlpAsync(
                p => SetStatus($"{L("status.tools")} — {p:0}%"));
            SetStatus(L("status.ready"));
            AppendLog(string.Format(L("log.note"), "yt-dlp: " + _tools.YtDlpPath));
        }
        catch (Exception ex)
        {
            SetStatus(L("status.failed").Replace("{0}", "tools"));
            AppendLog(string.Format(L("log.error"), ex.Message));
        }
        finally
        {
            _preparing = false;
            UpdateUi();
        }
    }

    private async Task FetchFormatsAsync()
    {
        string url = txtUrl.Text.Trim();
        if (!IsValidUrl(url))
        {
            MessageBox.Show(this, L("msg.invalidUrl"), L("app.title"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _busy = true;
        _cts = new CancellationTokenSource();
        UpdateUi();
        SetStatus(L("status.fetching"));
        AppendLog(L("log.separator"));
        AppendLog(string.Format(L("log.url"), url));

        try
        {
            string ytdlp = await _tools.EnsureYtDlpAsync(
                p => SetStatus($"{L("status.tools")} — {p:0}%"), _cts.Token);

            ProbeResult result = await _probe.ProbeAsync(
                url, ytdlp, Cookies, line => AppendLog(line), _cts.Token);

            _formats = result.Formats.ToList();
            ApplyFilter();
            SetStatus(string.Format(L("status.formats"), _formats.Count));
            AppendLog(string.Format(L("log.fetch"), result.Title));
        }
        catch (OperationCanceledException)
        {
            SetStatus(L("status.cancel"));
        }
        catch (Exception ex)
        {
            SetStatus(L("status.failed").Replace("{0}", "probe"));
            AppendLog(string.Format(L("log.error"), ex.Message));
            MessageBox.Show(this, string.Format(L("msg.fetchFailed"), ex.Message),
                L("app.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
            _cts?.Dispose();
            _cts = null;
            UpdateUi();
        }
    }

    private async Task DownloadAsync()
    {
        string url = txtUrl.Text.Trim();
        if (!IsValidUrl(url))
        {
            MessageBox.Show(this, L("msg.invalidUrl"), L("app.title"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Directory.CreateDirectory(txtFolder.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, string.Format(L("msg.folder"), ex.Message),
                L("app.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<FormatInfo> selection = SelectedFormats();
        int quickMode = Math.Clamp(cmbMode.SelectedIndex, 0, 4);

        if (selection.Count == 0 &&
            new Uri(url).Host.Contains("soundcloud", StringComparison.OrdinalIgnoreCase) &&
            quickMode is 0 or 1 or 2)
        {
            quickMode = 3;
            cmbMode.SelectedIndex = 3;
            AppendLog(L("log.soundcloud"));
        }

        string? formatSelector = null;
        bool merge = false;
        bool extractMp3 = false;
        string description;

        if (selection.Count > 0)
        {
            if (selection.Count == 1)
            {
                FormatInfo format = selection[0];
                formatSelector = format.Id;
                description = string.Format(L("log.format"), $"{format.Id} ({L(format.KindKey)})");

                if (format.HasVideo && !format.HasAudio && chkMergeAudio.Checked)
                {
                    formatSelector = $"{format.Id}+bestaudio/best";
                    merge = true;
                }
                else if (!format.HasVideo && format.HasAudio && chkConvertMp3.Checked)
                {
                    extractMp3 = true;
                }
                else if (format.HasVideo && format.HasAudio)
                {
                    merge = true;
                }
            }
            else if (selection.Count == 2)
            {
                FormatInfo? video = selection.FirstOrDefault(f => f.HasVideo && !f.HasAudio);
                FormatInfo? audio = selection.FirstOrDefault(f => !f.HasVideo && f.HasAudio);
                if (video is null || audio is null)
                {
                    MessageBox.Show(this, L("msg.badSelection"), L("app.title"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                formatSelector = $"{video.Id}+{audio.Id}";
                merge = true;
                description = string.Format(L("log.format"), formatSelector);
            }
            else
            {
                MessageBox.Show(this, L("msg.select"), L("app.title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        else
        {
            description = string.Format(L("log.mode"), cmbMode.GetItemText(cmbMode.SelectedItem));
        }

        _busy = true;
        _cts = new CancellationTokenSource();
        UpdateUi();
        progressBar.Value = 0;

        AppendLog(L("log.separator"));
        AppendLog(string.Format(L("log.url"), url));
        AppendLog(description);
        SetStatus(L("status.downloading"));

        try
        {
            string ytdlp = await _tools.EnsureYtDlpAsync(
                p => SetStatus($"{L("status.tools")} — {p:0}%"), _cts.Token);

            bool needsFfmpeg = merge || extractMp3;
            if (needsFfmpeg && !_tools.HasFfmpeg)
            {
                DialogResult answer = MessageBox.Show(this, L("msg.ffmpeg"), L("ffmpeg.button"),
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (answer == DialogResult.Cancel)
                {
                    SetStatus(L("status.cancel"));
                    return;
                }

                if (answer == DialogResult.Yes)
                {
                    SetStatus(L("status.installing"));
                    await _tools.EnsureFfmpegAsync(
                        progress: p => SetStatus($"{L("status.installing")} — {p:0}%"),
                        cancellationToken: _cts.Token);
                    AppendLog(string.Format(L("log.note"), L("note.ffmpegOk")));
                }
                else
                {
                    if (merge && formatSelector is not null)
                        formatSelector = formatSelector.Split('+')[0];
                    merge = false;
                    extractMp3 = false;
                    AppendLog(string.Format(L("log.note"), L("note.fallback")));
                }
            }

            var options = new DownloadOptions
            {
                Url = url,
                OutputDirectory = txtFolder.Text,
                YtDlpPath = ytdlp,
                ToolsDirectory = _tools.ToolsDirectory,
                QuickMode = quickMode,
                FormatSelector = formatSelector,
                MergeOutputMp4 = merge,
                ExtractMp3 = extractMp3,
                Playlist = chkPlaylist.Checked,
                CookiesFile = Cookies
            };

            DownloadResult result = await new DownloadEngine().RunAsync(
                options,
                new Progress<DownloadProgressSnapshot>(p => UiInvoke(() =>
                {
                    progressBar.Value = (int)Math.Clamp(p.Percent, 0, 100);
                    lblStatus.Text = $"{p.Percent:0}%   •   {p.Speed}   •   {p.Eta}";
                })),
                line => AppendLog(line),
                _cts.Token);

            if (result.Success)
            {
                progressBar.Value = 100;
                SetStatus(L("status.success"));
                foreach (string file in result.Files)
                    AppendLog(string.Format(L("log.file"), file));
            }
            else
            {
                SetStatus(string.Format(L("status.failed"), result.ExitCode));
                if (!string.IsNullOrEmpty(result.Error))
                    AppendLog(string.Format(L("log.error"), result.Error));
            }
        }
        catch (OperationCanceledException)
        {
            SetStatus(L("status.cancel"));
        }
        catch (Exception ex)
        {
            SetStatus(L("status.failed").Replace("{0}", "error"));
            AppendLog(string.Format(L("log.error"), ex.Message));
            MessageBox.Show(this, ex.Message, L("app.title"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
            _cts?.Dispose();
            _cts = null;
            UpdateUi();
        }
    }

    private async Task InstallFfmpegAsync()
    {
        if (_busy)
            return;

        _busy = true;
        _cts = new CancellationTokenSource();
        UpdateUi();

        try
        {
            SetStatus(L("status.installing"));
            await _tools.EnsureFfmpegAsync(
                force: true,
                progress: p => SetStatus($"{L("status.installing")} — {p:0}%"),
                cancellationToken: _cts.Token);
            SetStatus(L("status.success"));
            AppendLog(string.Format(L("log.note"), _tools.FfmpegPath));
        }
        catch (OperationCanceledException)
        {
            SetStatus(L("status.cancel"));
        }
        catch (Exception ex)
        {
            AppendLog(string.Format(L("log.error"), ex.Message));
            MessageBox.Show(this, ex.Message, L("app.title"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
            _cts?.Dispose();
            _cts = null;
            UpdateUi();
        }
    }

    private List<FormatInfo> SelectedFormats() => dgvFormats.SelectedRows
        .Cast<DataGridViewRow>()
        .Select(row => row.Tag as FormatInfo)
        .Where(format => format is not null)
        .Select(format => format!)
        .ToList();

    private void ApplyFilter()
    {
        var selectedIds = SelectedFormats()
            .Select(f => f.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        List<FormatInfo> visible = _formats.Where(f => _savedFilterIndex switch
        {
            1 => f.HasVideo && f.HasAudio,
            2 => f.HasVideo && !f.HasAudio,
            3 => !f.HasVideo && f.HasAudio,
            _ => true
        }).ToList();

        dgvFormats.Rows.Clear();
        foreach (FormatInfo format in visible)
        {
            int index = dgvFormats.Rows.Add(
                format.Id,
                L(format.KindKey),
                format.Ext,
                format.Resolution,
                format.Fps?.ToString("0.#") ?? "—",
                format.Codec(format.VideoCodec),
                format.Codec(format.AudioCodec),
                format.SizeText,
                format.BitrateText,
                format.FormatNote);

            dgvFormats.Rows[index].Tag = format;
            if (selectedIds.Contains(format.Id))
                dgvFormats.Rows[index].Selected = true;
        }
    }

    private string? Cookies =>
        string.IsNullOrWhiteSpace(txtCookies.Text) ? null : txtCookies.Text.Trim();

    private static bool IsValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private void UpdateUi()
    {
        if (IsDisposed)
            return;

        bool canStart = !_busy && !_preparing && IsValidUrl(txtUrl.Text.Trim());
        btnFetch.Enabled = canStart;
        btnDownload.Enabled = canStart;
        btnCancel.Enabled = _busy;
        btnFfmpeg.Enabled = !_busy;
        btnBrowseFolder.Enabled = !_busy;
        btnOpenFolder.Enabled = !_busy;
        btnBrowseCookies.Enabled = !_busy;
        btnClearCookies.Enabled = !_busy;
        cmbLanguage.Enabled = !_busy;
        txtUrl.Enabled = !_busy;
        cmbMode.Enabled = !_busy;
        cmbFilter.Enabled = !_busy;
        chkMergeAudio.Enabled = !_busy;
        chkConvertMp3.Enabled = !_busy;
        chkPlaylist.Enabled = !_busy;
    }

    private void SetStatus(string text) => UiInvoke(() => lblStatus.Text = text);

    private void AppendLog(string line) => UiInvoke(() =>
    {
        if (txtLog.TextLength > 20_000)
            txtLog.Clear();
        txtLog.AppendText(line + Environment.NewLine);
        try
        {
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }
        catch { /* المقبض غير جاهز */ }
    });

    private void UiInvoke(Action action)
    {
        if (IsDisposed)
            return;

        if (InvokeRequired)
        {
            try { BeginInvoke(action); }
            catch { /* النموذج أُغلق */ }
        }
        else
        {
            action();
        }
    }
}
