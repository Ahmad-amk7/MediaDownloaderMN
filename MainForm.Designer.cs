namespace MediaDownloaderM;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private System.Windows.Forms.Panel headerPanel = null!;
    private System.Windows.Forms.Label lblTitle = null!;
    private System.Windows.Forms.Label lblSubtitle = null!;
    private System.Windows.Forms.ComboBox cmbLanguage = null!;

    private System.Windows.Forms.Label lblUrl = null!;
    private System.Windows.Forms.TextBox txtUrl = null!;
    private System.Windows.Forms.Button btnFetch = null!;

    private System.Windows.Forms.Label lblMode = null!;
    private System.Windows.Forms.ComboBox cmbMode = null!;
    private System.Windows.Forms.CheckBox chkMergeAudio = null!;
    private System.Windows.Forms.CheckBox chkConvertMp3 = null!;
    private System.Windows.Forms.CheckBox chkPlaylist = null!;

    private System.Windows.Forms.Label lblFolder = null!;
    private System.Windows.Forms.TextBox txtFolder = null!;
    private System.Windows.Forms.Button btnBrowseFolder = null!;
    private System.Windows.Forms.Button btnOpenFolder = null!;

    private System.Windows.Forms.Label lblCookies = null!;
    private System.Windows.Forms.TextBox txtCookies = null!;
    private System.Windows.Forms.Button btnBrowseCookies = null!;
    private System.Windows.Forms.Button btnClearCookies = null!;

    private System.Windows.Forms.Label lblFormats = null!;
    private System.Windows.Forms.ComboBox cmbFilter = null!;
    private System.Windows.Forms.DataGridView dgvFormats = null!;

    private System.Windows.Forms.Button btnDownload = null!;
    private System.Windows.Forms.Button btnCancel = null!;
    private System.Windows.Forms.Button btnFfmpeg = null!;
    private System.Windows.Forms.Label lblHint = null!;

    private System.Windows.Forms.ProgressBar progressBar = null!;
    private System.Windows.Forms.Label lblStatus = null!;
    private System.Windows.Forms.RichTextBox txtLog = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();

        this.headerPanel = new System.Windows.Forms.Panel();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblSubtitle = new System.Windows.Forms.Label();
        this.cmbLanguage = new System.Windows.Forms.ComboBox();
        this.lblUrl = new System.Windows.Forms.Label();
        this.txtUrl = new System.Windows.Forms.TextBox();
        this.btnFetch = new System.Windows.Forms.Button();
        this.lblMode = new System.Windows.Forms.Label();
        this.cmbMode = new System.Windows.Forms.ComboBox();
        this.chkMergeAudio = new System.Windows.Forms.CheckBox();
        this.chkConvertMp3 = new System.Windows.Forms.CheckBox();
        this.chkPlaylist = new System.Windows.Forms.CheckBox();
        this.lblFolder = new System.Windows.Forms.Label();
        this.txtFolder = new System.Windows.Forms.TextBox();
        this.btnBrowseFolder = new System.Windows.Forms.Button();
        this.btnOpenFolder = new System.Windows.Forms.Button();
        this.lblCookies = new System.Windows.Forms.Label();
        this.txtCookies = new System.Windows.Forms.TextBox();
        this.btnBrowseCookies = new System.Windows.Forms.Button();
        this.btnClearCookies = new System.Windows.Forms.Button();
        this.lblFormats = new System.Windows.Forms.Label();
        this.cmbFilter = new System.Windows.Forms.ComboBox();
        this.dgvFormats = new System.Windows.Forms.DataGridView();
        this.btnDownload = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnFfmpeg = new System.Windows.Forms.Button();
        this.lblHint = new System.Windows.Forms.Label();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.lblStatus = new System.Windows.Forms.Label();
        this.txtLog = new System.Windows.Forms.RichTextBox();

        var colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colExt = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colRes = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colFps = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colVideo = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colAudio = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colBitrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        var colNote = new System.Windows.Forms.DataGridViewTextBoxColumn();

        this.headerPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvFormats)).BeginInit();
        this.SuspendLayout();

        // headerPanel
        this.headerPanel.BackColor = System.Drawing.Color.FromArgb(24, 44, 74);
        this.headerPanel.Controls.Add(this.lblTitle);
        this.headerPanel.Controls.Add(this.lblSubtitle);
        this.headerPanel.Controls.Add(this.cmbLanguage);
        // إظهار قائمة اللغة فوق الترويسة بدل أن تُغطّى بعنوان النموذج.
        this.cmbLanguage.BringToFront();
        this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
        this.headerPanel.Location = new System.Drawing.Point(0, 0);
        this.headerPanel.Name = "headerPanel";
        this.headerPanel.Size = new System.Drawing.Size(980, 74);
        this.headerPanel.TabIndex = 0;

        // lblTitle
        this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.ForeColor = System.Drawing.Color.White;
        this.lblTitle.Location = new System.Drawing.Point(0, 8);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(980, 36);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // lblSubtitle
        this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(200, 218, 240);
        this.lblSubtitle.Location = new System.Drawing.Point(0, 44);
        this.lblSubtitle.Name = "lblSubtitle";
        this.lblSubtitle.Size = new System.Drawing.Size(980, 24);
        this.lblSubtitle.TabIndex = 1;
        this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // cmbLanguage
        this.cmbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                  System.Windows.Forms.AnchorStyles.Right;
        this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.cmbLanguage.Items.AddRange(new object[] { "العربية", "English", "Deutsch" });
        this.cmbLanguage.Location = new System.Drawing.Point(850, 22);
        this.cmbLanguage.Name = "cmbLanguage";
        this.cmbLanguage.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.cmbLanguage.Size = new System.Drawing.Size(110, 30);
        this.cmbLanguage.TabIndex = 2;

        // lblUrl
        this.lblUrl.AutoSize = true;
        this.lblUrl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblUrl.Location = new System.Drawing.Point(24, 88);
        this.lblUrl.Name = "lblUrl";
        this.lblUrl.Size = new System.Drawing.Size(160, 19);
        this.lblUrl.TabIndex = 1;

        // txtUrl
        this.txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top |
                             System.Windows.Forms.AnchorStyles.Left |
                             System.Windows.Forms.AnchorStyles.Right;
        this.txtUrl.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtUrl.Location = new System.Drawing.Point(24, 114);
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.txtUrl.Size = new System.Drawing.Size(760, 34);
        this.txtUrl.TabIndex = 2;

        // btnFetch
        this.btnFetch.Anchor = System.Windows.Forms.AnchorStyles.Top |
                               System.Windows.Forms.AnchorStyles.Right;
        this.btnFetch.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
        this.btnFetch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnFetch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnFetch.ForeColor = System.Drawing.Color.White;
        this.btnFetch.Location = new System.Drawing.Point(794, 114);
        this.btnFetch.Name = "btnFetch";
        this.btnFetch.Size = new System.Drawing.Size(162, 34);
        this.btnFetch.TabIndex = 3;
        this.btnFetch.UseVisualStyleBackColor = false;

        // lblMode
        this.lblMode.AutoSize = true;
        this.lblMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblMode.Location = new System.Drawing.Point(24, 158);
        this.lblMode.Name = "lblMode";
        this.lblMode.Size = new System.Drawing.Size(80, 19);
        this.lblMode.TabIndex = 4;

        // cmbMode
        this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbMode.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.cmbMode.Location = new System.Drawing.Point(24, 184);
        this.cmbMode.Name = "cmbMode";
        this.cmbMode.Size = new System.Drawing.Size(420, 32);
        this.cmbMode.TabIndex = 5;

        // chkMergeAudio
        this.chkMergeAudio.AutoSize = true;
        this.chkMergeAudio.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.chkMergeAudio.Location = new System.Drawing.Point(460, 190);
        this.chkMergeAudio.Name = "chkMergeAudio";
        this.chkMergeAudio.Size = new System.Drawing.Size(180, 24);
        this.chkMergeAudio.TabIndex = 6;
        this.chkMergeAudio.UseVisualStyleBackColor = true;

        // chkConvertMp3
        this.chkConvertMp3.AutoSize = true;
        this.chkConvertMp3.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.chkConvertMp3.Location = new System.Drawing.Point(650, 190);
        this.chkConvertMp3.Name = "chkConvertMp3";
        this.chkConvertMp3.Size = new System.Drawing.Size(150, 24);
        this.chkConvertMp3.TabIndex = 7;
        this.chkConvertMp3.UseVisualStyleBackColor = true;

        // chkPlaylist
        this.chkPlaylist.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                  System.Windows.Forms.AnchorStyles.Right;
        this.chkPlaylist.AutoSize = true;
        this.chkPlaylist.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.chkPlaylist.Location = new System.Drawing.Point(810, 190);
        this.chkPlaylist.Name = "chkPlaylist";
        this.chkPlaylist.Size = new System.Drawing.Size(150, 24);
        this.chkPlaylist.TabIndex = 8;
        this.chkPlaylist.UseVisualStyleBackColor = true;

        // lblFolder
        this.lblFolder.AutoSize = true;
        this.lblFolder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblFolder.Location = new System.Drawing.Point(24, 224);
        this.lblFolder.Name = "lblFolder";
        this.lblFolder.Size = new System.Drawing.Size(70, 19);
        this.lblFolder.TabIndex = 9;

        // txtFolder
        this.txtFolder.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                System.Windows.Forms.AnchorStyles.Left |
                                System.Windows.Forms.AnchorStyles.Right;
        this.txtFolder.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.txtFolder.Location = new System.Drawing.Point(24, 250);
        this.txtFolder.Name = "txtFolder";
        this.txtFolder.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.txtFolder.ReadOnly = true;
        this.txtFolder.Size = new System.Drawing.Size(700, 32);
        this.txtFolder.TabIndex = 10;

        // btnBrowseFolder
        this.btnBrowseFolder.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                      System.Windows.Forms.AnchorStyles.Right;
        this.btnBrowseFolder.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
        this.btnBrowseFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnBrowseFolder.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.btnBrowseFolder.Location = new System.Drawing.Point(734, 250);
        this.btnBrowseFolder.Name = "btnBrowseFolder";
        this.btnBrowseFolder.Size = new System.Drawing.Size(110, 32);
        this.btnBrowseFolder.TabIndex = 11;
        this.btnBrowseFolder.UseVisualStyleBackColor = false;

        // btnOpenFolder
        this.btnOpenFolder.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                    System.Windows.Forms.AnchorStyles.Right;
        this.btnOpenFolder.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
        this.btnOpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnOpenFolder.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.btnOpenFolder.Location = new System.Drawing.Point(854, 250);
        this.btnOpenFolder.Name = "btnOpenFolder";
        this.btnOpenFolder.Size = new System.Drawing.Size(102, 32);
        this.btnOpenFolder.TabIndex = 12;
        this.btnOpenFolder.UseVisualStyleBackColor = false;

        // lblCookies
        this.lblCookies.AutoSize = true;
        this.lblCookies.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.lblCookies.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
        this.lblCookies.Location = new System.Drawing.Point(24, 292);
        this.lblCookies.Name = "lblCookies";
        this.lblCookies.Size = new System.Drawing.Size(300, 19);
        this.lblCookies.TabIndex = 13;

        // txtCookies
        this.txtCookies.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                 System.Windows.Forms.AnchorStyles.Left |
                                 System.Windows.Forms.AnchorStyles.Right;
        this.txtCookies.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.txtCookies.Location = new System.Drawing.Point(24, 318);
        this.txtCookies.Name = "txtCookies";
        this.txtCookies.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.txtCookies.ReadOnly = true;
        this.txtCookies.Size = new System.Drawing.Size(700, 32);
        this.txtCookies.TabIndex = 14;

        // btnBrowseCookies
        this.btnBrowseCookies.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                       System.Windows.Forms.AnchorStyles.Right;
        this.btnBrowseCookies.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
        this.btnBrowseCookies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnBrowseCookies.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.btnBrowseCookies.Location = new System.Drawing.Point(734, 318);
        this.btnBrowseCookies.Name = "btnBrowseCookies";
        this.btnBrowseCookies.Size = new System.Drawing.Size(110, 32);
        this.btnBrowseCookies.TabIndex = 15;
        this.btnBrowseCookies.UseVisualStyleBackColor = false;

        // btnClearCookies
        this.btnClearCookies.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                      System.Windows.Forms.AnchorStyles.Right;
        this.btnClearCookies.BackColor = System.Drawing.Color.FromArgb(236, 240, 245);
        this.btnClearCookies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnClearCookies.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.btnClearCookies.Location = new System.Drawing.Point(854, 318);
        this.btnClearCookies.Name = "btnClearCookies";
        this.btnClearCookies.Size = new System.Drawing.Size(102, 32);
        this.btnClearCookies.TabIndex = 16;
        this.btnClearCookies.UseVisualStyleBackColor = false;

        // lblFormats
        this.lblFormats.AutoSize = true;
        this.lblFormats.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblFormats.Location = new System.Drawing.Point(24, 362);
        this.lblFormats.Name = "lblFormats";
        this.lblFormats.Size = new System.Drawing.Size(260, 19);
        this.lblFormats.TabIndex = 17;

        // cmbFilter
        this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.cmbFilter.Location = new System.Drawing.Point(740, 358);
        this.cmbFilter.Name = "cmbFilter";
        this.cmbFilter.Size = new System.Drawing.Size(216, 30);
        this.cmbFilter.TabIndex = 18;

        // dgvFormats
        this.dgvFormats.AllowUserToAddRows = false;
        this.dgvFormats.AllowUserToDeleteRows = false;
        this.dgvFormats.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                 System.Windows.Forms.AnchorStyles.Bottom |
                                 System.Windows.Forms.AnchorStyles.Left |
                                 System.Windows.Forms.AnchorStyles.Right;
        this.dgvFormats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
        this.dgvFormats.BackgroundColor = System.Drawing.Color.White;
        this.dgvFormats.ColumnHeadersHeightSizeMode =
            System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvFormats.Location = new System.Drawing.Point(24, 424);
        this.dgvFormats.MultiSelect = true;
        this.dgvFormats.Name = "dgvFormats";
        this.dgvFormats.ReadOnly = true;
        this.dgvFormats.RowHeadersWidth = 30;
        this.dgvFormats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvFormats.Size = new System.Drawing.Size(932, 220);
        this.dgvFormats.TabIndex = 19;

        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.Width = 60;
        colKind.HeaderText = "Type";
        colKind.Name = "colKind";
        colKind.Width = 120;
        colExt.HeaderText = "Ext";
        colExt.Name = "colExt";
        colExt.Width = 60;
        colRes.HeaderText = "Resolution";
        colRes.Name = "colRes";
        colRes.Width = 90;
        colFps.HeaderText = "FPS";
        colFps.Name = "colFps";
        colFps.Width = 60;
        colVideo.HeaderText = "Video";
        colVideo.Name = "colVideo";
        colVideo.Width = 110;
        colAudio.HeaderText = "Audio";
        colAudio.Name = "colAudio";
        colAudio.Width = 120;
        colSize.HeaderText = "Size";
        colSize.Name = "colSize";
        colSize.Width = 90;
        colBitrate.HeaderText = "Bitrate";
        colBitrate.Name = "colBitrate";
        colBitrate.Width = 85;
        colNote.HeaderText = "Note";
        colNote.Name = "colNote";
        colNote.Width = 170;
        this.dgvFormats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
        {
            colId, colKind, colExt, colRes, colFps, colVideo, colAudio, colSize, colBitrate, colNote
        });

        // btnDownload
        this.btnDownload.BackColor = System.Drawing.Color.FromArgb(38, 125, 64);
        this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        this.btnDownload.ForeColor = System.Drawing.Color.White;
        this.btnDownload.Location = new System.Drawing.Point(24, 654);
        this.btnDownload.Name = "btnDownload";
        this.btnDownload.Size = new System.Drawing.Size(200, 44);
        this.btnDownload.TabIndex = 20;
        this.btnDownload.UseVisualStyleBackColor = false;

        // btnCancel
        this.btnCancel.BackColor = System.Drawing.Color.FromArgb(200, 70, 70);
        this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        this.btnCancel.ForeColor = System.Drawing.Color.White;
        this.btnCancel.Location = new System.Drawing.Point(234, 654);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(140, 44);
        this.btnCancel.TabIndex = 21;
        this.btnCancel.UseVisualStyleBackColor = false;

        // btnFfmpeg
        this.btnFfmpeg.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
        this.btnFfmpeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnFfmpeg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnFfmpeg.ForeColor = System.Drawing.Color.White;
        this.btnFfmpeg.Location = new System.Drawing.Point(384, 654);
        this.btnFfmpeg.Name = "btnFfmpeg";
        this.btnFfmpeg.Size = new System.Drawing.Size(220, 44);
        this.btnFfmpeg.TabIndex = 22;
        this.btnFfmpeg.UseVisualStyleBackColor = false;

        // lblHint
        this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8F);
        this.lblHint.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
        this.lblHint.Location = new System.Drawing.Point(620, 654);
        this.lblHint.Name = "lblHint";
        this.lblHint.Size = new System.Drawing.Size(336, 44);
        this.lblHint.TabIndex = 23;

        // progressBar
        this.progressBar.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                  System.Windows.Forms.AnchorStyles.Left |
                                  System.Windows.Forms.AnchorStyles.Right;
        this.progressBar.Location = new System.Drawing.Point(24, 708);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(932, 18);
        this.progressBar.TabIndex = 24;

        // lblStatus
        this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                System.Windows.Forms.AnchorStyles.Left |
                                System.Windows.Forms.AnchorStyles.Right;
        this.lblStatus.AutoEllipsis = true;
        this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(40, 60, 85);
        this.lblStatus.Location = new System.Drawing.Point(24, 730);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(932, 24);
        this.lblStatus.TabIndex = 25;

        // txtLog
        this.txtLog.Anchor = System.Windows.Forms.AnchorStyles.Top |
                             System.Windows.Forms.AnchorStyles.Bottom |
                             System.Windows.Forms.AnchorStyles.Left |
                             System.Windows.Forms.AnchorStyles.Right;
        this.txtLog.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
        this.txtLog.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.txtLog.Location = new System.Drawing.Point(24, 758);
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        this.txtLog.Size = new System.Drawing.Size(932, 170);
        this.txtLog.TabIndex = 26;

        // MainForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(980, 950);
        this.Controls.Add(this.headerPanel);
        this.Controls.Add(this.lblUrl);
        this.Controls.Add(this.txtUrl);
        this.Controls.Add(this.btnFetch);
        this.Controls.Add(this.lblMode);
        this.Controls.Add(this.cmbMode);
        this.Controls.Add(this.chkMergeAudio);
        this.Controls.Add(this.chkConvertMp3);
        this.Controls.Add(this.chkPlaylist);
        this.Controls.Add(this.lblFolder);
        this.Controls.Add(this.txtFolder);
        this.Controls.Add(this.btnBrowseFolder);
        this.Controls.Add(this.btnOpenFolder);
        this.Controls.Add(this.lblCookies);
        this.Controls.Add(this.txtCookies);
        this.Controls.Add(this.btnBrowseCookies);
        this.Controls.Add(this.btnClearCookies);
        this.Controls.Add(this.lblFormats);
        this.Controls.Add(this.cmbFilter);
        this.Controls.Add(this.dgvFormats);
        this.Controls.Add(this.btnDownload);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnFfmpeg);
        this.Controls.Add(this.lblHint);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.txtLog);
        this.Font = new System.Drawing.Font("Segoe UI", 9F);
        this.Name = "MainForm";
        this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.RightToLeftLayout = true;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Media Downloader M";

        this.headerPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvFormats)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
