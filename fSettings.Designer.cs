namespace BatchMKV
{
    partial class fSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fSettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.cbxPurgeSourceState = new System.Windows.Forms.ComboBox();
            this.chkPurgeSourceState = new System.Windows.Forms.CheckBox();
            this.chkClearMissingSourcesOnStart = new System.Windows.Forms.CheckBox();
            this.chkClearFinishedSourcesOnExit = new System.Windows.Forms.CheckBox();
            this.chkRestoreSourcesOnRestart = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkScanOnStart = new System.Windows.Forms.CheckBox();
            this.btnBrowseScanOnStart = new System.Windows.Forms.Button();
            this.txtScanOnStart = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowseArchive = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtArchiveFolder = new System.Windows.Forms.TextBox();
            this.cbxSourceAction = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnBrowseMKVToolNix = new System.Windows.Forms.Button();
            this.txtMKVToolNixPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowseMakeMKV = new System.Windows.Forms.Button();
            this.txtMakeMKVPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkOutputFolderTags = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOutputOverwrite = new System.Windows.Forms.CheckBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputSpecified = new System.Windows.Forms.TextBox();
            this.radOutputSpecified = new System.Windows.Forms.RadioButton();
            this.radOutputSource = new System.Windows.Forms.RadioButton();
            this.tabTranscoding = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnTranscodingReset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxTranscodingDefault = new System.Windows.Forms.ComboBox();
            this.cbxTranscoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.olvTranscoding = new BrightIdeasSoftware.ObjectListView();
            this.olvCodec = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvLossless = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvTranscode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label3 = new System.Windows.Forms.Label();
            this.cbxFLACCompression = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxLPCMContainer = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkZLibCompression = new System.Windows.Forms.CheckBox();
            this.tabTitles = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lnkTitleFilenameTags = new System.Windows.Forms.LinkLabel();
            this.lnkTitleNameTags = new System.Windows.Forms.LinkLabel();
            this.txtTitleFilename = new System.Windows.Forms.TextBox();
            this.radTitlesFilenameCustom = new System.Windows.Forms.RadioButton();
            this.radTitlesFilenameBasedOnName = new System.Windows.Forms.RadioButton();
            this.label19 = new System.Windows.Forms.Label();
            this.txtTitleName = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radTitlesIncludeAll = new System.Windows.Forms.RadioButton();
            this.radTitlesIncludeLongest = new System.Windows.Forms.RadioButton();
            this.label18 = new System.Windows.Forms.Label();
            this.updTitlesIncludeWithin = new System.Windows.Forms.NumericUpDown();
            this.chkTitlesIncludeWithin = new System.Windows.Forms.CheckBox();
            this.chkTitlesIgnoreMuted = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.updIgnoreTitlesLength = new System.Windows.Forms.NumericUpDown();
            this.chkIgnoreTitlesLength = new System.Windows.Forms.CheckBox();
            this.tabTracks = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.cbxSubtitleDefault = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbxSubtitleOrder = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.updSubtitleLimit = new System.Windows.Forms.NumericUpDown();
            this.chkSubtitleLimit = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.radSubtitleIncludeFavourites = new System.Windows.Forms.RadioButton();
            this.radSubtitleIncludeAll = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.olvLanguagesAll = new BrightIdeasSoftware.ObjectListView();
            this.olvcLanguagesAll = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label12 = new System.Windows.Forms.Label();
            this.olvLanguagesFavourites = new BrightIdeasSoftware.ObjectListView();
            this.olvcLanguagesFavourites = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label13 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLanguageRemove = new System.Windows.Forms.Button();
            this.btnLanguageDown = new System.Windows.Forms.Button();
            this.btnLanguageAdd = new System.Windows.Forms.Button();
            this.btnLanguageUp = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtAudioCommentaryTrackName = new System.Windows.Forms.TextBox();
            this.chkAudioIncludeCommentaryTracks = new System.Windows.Forms.CheckBox();
            this.cbxAudioMainCore = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.updAudioLimit = new System.Windows.Forms.NumericUpDown();
            this.chkAudioLimit = new System.Windows.Forms.CheckBox();
            this.cbxAudioOrder = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkAudioIncludeFirst = new System.Windows.Forms.CheckBox();
            this.chkAudioIncludeNonFavourite = new System.Windows.Forms.CheckBox();
            this.chkAudioIncludeQuality = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.radAudioIncludeFavourites = new System.Windows.Forms.RadioButton();
            this.radAudioIncludeAll = new System.Windows.Forms.RadioButton();
            this.tabTroubleshooting = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.lnkForumThread = new System.Windows.Forms.LinkLabel();
            this.btnErrorLoggingDisable = new System.Windows.Forms.Button();
            this.btnErrorLoggingEnable = new System.Windows.Forms.Button();
            this.lblErrorLoggingStatus = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.dlgMakeMKVPath = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgOutputPath = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgArchivePath = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgScanOnStart = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgMKVToolNixPath = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabTranscoding.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvTranscoding)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tabTitles.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updTitlesIncludeWithin)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updIgnoreTitlesLength)).BeginInit();
            this.tabTracks.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSubtitleLimit)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvLanguagesAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvLanguagesFavourites)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updAudioLimit)).BeginInit();
            this.tabTroubleshooting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabTranscoding);
            this.tabControl1.Controls.Add(this.tabTitles);
            this.tabControl1.Controls.Add(this.tabTracks);
            this.tabControl1.Controls.Add(this.tabTroubleshooting);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 703);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupBox13);
            this.tabGeneral.Controls.Add(this.groupBox4);
            this.tabGeneral.Controls.Add(this.groupBox3);
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(552, 677);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.cbxPurgeSourceState);
            this.groupBox13.Controls.Add(this.chkPurgeSourceState);
            this.groupBox13.Controls.Add(this.chkClearMissingSourcesOnStart);
            this.groupBox13.Controls.Add(this.chkClearFinishedSourcesOnExit);
            this.groupBox13.Controls.Add(this.chkRestoreSourcesOnRestart);
            this.groupBox13.Location = new System.Drawing.Point(15, 369);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(523, 100);
            this.groupBox13.TabIndex = 5;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "                                                                                 " +
    "                                           ";
            // 
            // cbxPurgeSourceState
            // 
            this.cbxPurgeSourceState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPurgeSourceState.FormattingEnabled = true;
            this.cbxPurgeSourceState.Items.AddRange(new object[] {
            "1 day",
            "3 days",
            "1 week",
            "2 weeks",
            "1 month",
            "3 months"});
            this.cbxPurgeSourceState.Location = new System.Drawing.Point(168, 67);
            this.cbxPurgeSourceState.Name = "cbxPurgeSourceState";
            this.cbxPurgeSourceState.Size = new System.Drawing.Size(121, 21);
            this.cbxPurgeSourceState.TabIndex = 17;
            // 
            // chkPurgeSourceState
            // 
            this.chkPurgeSourceState.AutoSize = true;
            this.chkPurgeSourceState.Location = new System.Drawing.Point(30, 69);
            this.chkPurgeSourceState.Name = "chkPurgeSourceState";
            this.chkPurgeSourceState.Size = new System.Drawing.Size(139, 17);
            this.chkPurgeSourceState.TabIndex = 16;
            this.chkPurgeSourceState.Text = "Purge data older than";
            this.chkPurgeSourceState.UseVisualStyleBackColor = true;
            this.chkPurgeSourceState.CheckedChanged += new System.EventHandler(this.chkPurgeSourceState_CheckedChanged);
            // 
            // chkClearMissingSourcesOnStart
            // 
            this.chkClearMissingSourcesOnStart.AutoSize = true;
            this.chkClearMissingSourcesOnStart.Location = new System.Drawing.Point(30, 46);
            this.chkClearMissingSourcesOnStart.Name = "chkClearMissingSourcesOnStart";
            this.chkClearMissingSourcesOnStart.Size = new System.Drawing.Size(179, 17);
            this.chkClearMissingSourcesOnStart.TabIndex = 15;
            this.chkClearMissingSourcesOnStart.Text = "Clear missing sources on start";
            this.chkClearMissingSourcesOnStart.UseVisualStyleBackColor = true;
            // 
            // chkClearFinishedSourcesOnExit
            // 
            this.chkClearFinishedSourcesOnExit.AutoSize = true;
            this.chkClearFinishedSourcesOnExit.Location = new System.Drawing.Point(30, 23);
            this.chkClearFinishedSourcesOnExit.Name = "chkClearFinishedSourcesOnExit";
            this.chkClearFinishedSourcesOnExit.Size = new System.Drawing.Size(177, 17);
            this.chkClearFinishedSourcesOnExit.TabIndex = 14;
            this.chkClearFinishedSourcesOnExit.Text = "Clear finished sources on exit";
            this.chkClearFinishedSourcesOnExit.UseVisualStyleBackColor = true;
            // 
            // chkRestoreSourcesOnRestart
            // 
            this.chkRestoreSourcesOnRestart.AutoSize = true;
            this.chkRestoreSourcesOnRestart.Location = new System.Drawing.Point(10, 0);
            this.chkRestoreSourcesOnRestart.Name = "chkRestoreSourcesOnRestart";
            this.chkRestoreSourcesOnRestart.Size = new System.Drawing.Size(376, 17);
            this.chkRestoreSourcesOnRestart.TabIndex = 13;
            this.chkRestoreSourcesOnRestart.Text = "Keep analysis information and source settings on application restart";
            this.chkRestoreSourcesOnRestart.UseVisualStyleBackColor = true;
            this.chkRestoreSourcesOnRestart.CheckedChanged += new System.EventHandler(this.chkRestoreSourcesOnRestart_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkScanOnStart);
            this.groupBox4.Controls.Add(this.btnBrowseScanOnStart);
            this.groupBox4.Controls.Add(this.txtScanOnStart);
            this.groupBox4.Location = new System.Drawing.Point(15, 300);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(523, 58);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "                                        ";
            // 
            // chkScanOnStart
            // 
            this.chkScanOnStart.AutoSize = true;
            this.chkScanOnStart.Location = new System.Drawing.Point(10, 0);
            this.chkScanOnStart.Name = "chkScanOnStart";
            this.chkScanOnStart.Size = new System.Drawing.Size(127, 17);
            this.chkScanOnStart.TabIndex = 13;
            this.chkScanOnStart.Text = "Scan folder on start";
            this.chkScanOnStart.UseVisualStyleBackColor = true;
            this.chkScanOnStart.CheckedChanged += new System.EventHandler(this.chkScanOnStart_CheckedChanged);
            // 
            // btnBrowseScanOnStart
            // 
            this.btnBrowseScanOnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseScanOnStart.Enabled = false;
            this.btnBrowseScanOnStart.Location = new System.Drawing.Point(484, 24);
            this.btnBrowseScanOnStart.Name = "btnBrowseScanOnStart";
            this.btnBrowseScanOnStart.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseScanOnStart.TabIndex = 15;
            this.btnBrowseScanOnStart.Text = "...";
            this.btnBrowseScanOnStart.Click += new System.EventHandler(this.btnBrowseScanOnStart_Click);
            // 
            // txtScanOnStart
            // 
            this.txtScanOnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScanOnStart.Enabled = false;
            this.txtScanOnStart.Location = new System.Drawing.Point(10, 25);
            this.txtScanOnStart.Name = "txtScanOnStart";
            this.txtScanOnStart.ReadOnly = true;
            this.txtScanOnStart.Size = new System.Drawing.Size(468, 22);
            this.txtScanOnStart.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btnBrowseArchive);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtArchiveFolder);
            this.groupBox3.Controls.Add(this.cbxSourceAction);
            this.groupBox3.Location = new System.Drawing.Point(15, 209);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(523, 80);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source files";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Archive path:";
            // 
            // btnBrowseArchive
            // 
            this.btnBrowseArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseArchive.Location = new System.Drawing.Point(484, 46);
            this.btnBrowseArchive.Name = "btnBrowseArchive";
            this.btnBrowseArchive.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseArchive.TabIndex = 12;
            this.btnBrowseArchive.Text = "...";
            this.btnBrowseArchive.Click += new System.EventHandler(this.btnBrowseArchive_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "After conversion:";
            // 
            // txtArchiveFolder
            // 
            this.txtArchiveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArchiveFolder.Location = new System.Drawing.Point(113, 47);
            this.txtArchiveFolder.MaxLength = 250;
            this.txtArchiveFolder.Name = "txtArchiveFolder";
            this.txtArchiveFolder.Size = new System.Drawing.Size(365, 22);
            this.txtArchiveFolder.TabIndex = 11;
            // 
            // cbxSourceAction
            // 
            this.cbxSourceAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSourceAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSourceAction.FormattingEnabled = true;
            this.cbxSourceAction.Items.AddRange(new object[] {
            "Keep at current location",
            "Move to archive folder",
            "Delete"});
            this.cbxSourceAction.Location = new System.Drawing.Point(113, 20);
            this.cbxSourceAction.Name = "cbxSourceAction";
            this.cbxSourceAction.Size = new System.Drawing.Size(400, 21);
            this.cbxSourceAction.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.btnBrowseMKVToolNix);
            this.groupBox2.Controls.Add(this.txtMKVToolNixPath);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnBrowseMakeMKV);
            this.groupBox2.Controls.Add(this.txtMakeMKVPath);
            this.groupBox2.Location = new System.Drawing.Point(15, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 79);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dependencies";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "MKVToolNix path:";
            // 
            // btnBrowseMKVToolNix
            // 
            this.btnBrowseMKVToolNix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMKVToolNix.Location = new System.Drawing.Point(484, 45);
            this.btnBrowseMKVToolNix.Name = "btnBrowseMKVToolNix";
            this.btnBrowseMKVToolNix.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseMKVToolNix.TabIndex = 4;
            this.btnBrowseMKVToolNix.Text = "...";
            this.btnBrowseMKVToolNix.Click += new System.EventHandler(this.btnBrowseMKVToolNix_Click);
            // 
            // txtMKVToolNixPath
            // 
            this.txtMKVToolNixPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMKVToolNixPath.Location = new System.Drawing.Point(113, 46);
            this.txtMKVToolNixPath.Name = "txtMKVToolNixPath";
            this.txtMKVToolNixPath.ReadOnly = true;
            this.txtMKVToolNixPath.Size = new System.Drawing.Size(365, 22);
            this.txtMKVToolNixPath.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "MakeMKV path:";
            // 
            // btnBrowseMakeMKV
            // 
            this.btnBrowseMakeMKV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMakeMKV.Location = new System.Drawing.Point(484, 19);
            this.btnBrowseMakeMKV.Name = "btnBrowseMakeMKV";
            this.btnBrowseMakeMKV.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseMakeMKV.TabIndex = 2;
            this.btnBrowseMakeMKV.Text = "...";
            this.btnBrowseMakeMKV.Click += new System.EventHandler(this.btnBrowseMakeMKV_Click);
            // 
            // txtMakeMKVPath
            // 
            this.txtMakeMKVPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMakeMKVPath.Location = new System.Drawing.Point(113, 20);
            this.txtMakeMKVPath.Name = "txtMakeMKVPath";
            this.txtMakeMKVPath.ReadOnly = true;
            this.txtMakeMKVPath.Size = new System.Drawing.Size(365, 22);
            this.txtMakeMKVPath.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lnkOutputFolderTags);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chkOutputOverwrite);
            this.groupBox1.Controls.Add(this.btnBrowseOutput);
            this.groupBox1.Controls.Add(this.txtOutputSpecified);
            this.groupBox1.Controls.Add(this.radOutputSpecified);
            this.groupBox1.Controls.Add(this.radOutputSource);
            this.groupBox1.Location = new System.Drawing.Point(15, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 93);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // lnkOutputFolderTags
            // 
            this.lnkOutputFolderTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkOutputFolderTags.AutoSize = true;
            this.lnkOutputFolderTags.Location = new System.Drawing.Point(438, 68);
            this.lnkOutputFolderTags.Name = "lnkOutputFolderTags";
            this.lnkOutputFolderTags.Size = new System.Drawing.Size(78, 13);
            this.lnkOutputFolderTags.TabIndex = 10;
            this.lnkOutputFolderTags.TabStop = true;
            this.lnkOutputFolderTags.Text = "Available tags";
            this.lnkOutputFolderTags.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkOutputFolderTags.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOutputFolderTags_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Default location:";
            // 
            // chkOutputOverwrite
            // 
            this.chkOutputOverwrite.AutoSize = true;
            this.chkOutputOverwrite.Location = new System.Drawing.Point(113, 67);
            this.chkOutputOverwrite.Name = "chkOutputOverwrite";
            this.chkOutputOverwrite.Size = new System.Drawing.Size(240, 17);
            this.chkOutputOverwrite.TabIndex = 9;
            this.chkOutputOverwrite.Text = "Overwrite output files if they already exist";
            this.chkOutputOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutput.Enabled = false;
            this.btnBrowseOutput.Location = new System.Drawing.Point(484, 41);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseOutput.TabIndex = 8;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputSpecified
            // 
            this.txtOutputSpecified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputSpecified.Enabled = false;
            this.txtOutputSpecified.Location = new System.Drawing.Point(224, 41);
            this.txtOutputSpecified.MaxLength = 1000;
            this.txtOutputSpecified.Name = "txtOutputSpecified";
            this.txtOutputSpecified.Size = new System.Drawing.Size(254, 22);
            this.txtOutputSpecified.TabIndex = 7;
            // 
            // radOutputSpecified
            // 
            this.radOutputSpecified.AutoSize = true;
            this.radOutputSpecified.Location = new System.Drawing.Point(113, 42);
            this.radOutputSpecified.Name = "radOutputSpecified";
            this.radOutputSpecified.Size = new System.Drawing.Size(109, 17);
            this.radOutputSpecified.TabIndex = 6;
            this.radOutputSpecified.TabStop = true;
            this.radOutputSpecified.Text = "Specified folder:";
            this.radOutputSpecified.UseVisualStyleBackColor = true;
            this.radOutputSpecified.CheckedChanged += new System.EventHandler(this.radOutputSpecified_CheckedChanged);
            // 
            // radOutputSource
            // 
            this.radOutputSource.AutoSize = true;
            this.radOutputSource.Checked = true;
            this.radOutputSource.Location = new System.Drawing.Point(113, 19);
            this.radOutputSource.Name = "radOutputSource";
            this.radOutputSource.Size = new System.Drawing.Size(103, 17);
            this.radOutputSource.TabIndex = 5;
            this.radOutputSource.TabStop = true;
            this.radOutputSource.Text = "Same as source";
            this.radOutputSource.UseVisualStyleBackColor = true;
            // 
            // tabTranscoding
            // 
            this.tabTranscoding.Controls.Add(this.groupBox6);
            this.tabTranscoding.Controls.Add(this.groupBox5);
            this.tabTranscoding.Location = new System.Drawing.Point(4, 22);
            this.tabTranscoding.Name = "tabTranscoding";
            this.tabTranscoding.Padding = new System.Windows.Forms.Padding(3);
            this.tabTranscoding.Size = new System.Drawing.Size(552, 677);
            this.tabTranscoding.TabIndex = 0;
            this.tabTranscoding.Text = "Transcoding";
            this.tabTranscoding.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.btnTranscodingReset);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.cbxTranscodingDefault);
            this.groupBox6.Controls.Add(this.cbxTranscoding);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.olvTranscoding);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.cbxFLACCompression);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.cbxLPCMContainer);
            this.groupBox6.Location = new System.Drawing.Point(15, 72);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(523, 558);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Audio streams";
            // 
            // btnTranscodingReset
            // 
            this.btnTranscodingReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTranscodingReset.Location = new System.Drawing.Point(385, 465);
            this.btnTranscodingReset.Name = "btnTranscodingReset";
            this.btnTranscodingReset.Size = new System.Drawing.Size(127, 23);
            this.btnTranscodingReset.TabIndex = 20;
            this.btnTranscodingReset.Text = "Reset to Default";
            this.btnTranscodingReset.Click += new System.EventHandler(this.btnTranscodingReset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Codec-specific:";
            // 
            // cbxTranscodingDefault
            // 
            this.cbxTranscodingDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTranscodingDefault.FormattingEnabled = true;
            this.cbxTranscodingDefault.Items.AddRange(new object[] {
            "Direct copy",
            "Transcode to FLAC",
            "Transcode to LPCM"});
            this.cbxTranscodingDefault.Location = new System.Drawing.Point(113, 20);
            this.cbxTranscodingDefault.Name = "cbxTranscodingDefault";
            this.cbxTranscodingDefault.Size = new System.Drawing.Size(200, 21);
            this.cbxTranscodingDefault.TabIndex = 17;
            this.cbxTranscodingDefault.SelectedIndexChanged += new System.EventHandler(this.cbxTranscodingDefault_SelectedIndexChanged);
            // 
            // cbxTranscoding
            // 
            this.cbxTranscoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxTranscoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTranscoding.FormattingEnabled = true;
            this.cbxTranscoding.Items.AddRange(new object[] {
            "Use default",
            "Direct copy",
            "Transcode to FLAC",
            "Transcode to LPCM"});
            this.cbxTranscoding.Location = new System.Drawing.Point(113, 466);
            this.cbxTranscoding.Name = "cbxTranscoding";
            this.cbxTranscoding.Size = new System.Drawing.Size(200, 21);
            this.cbxTranscoding.TabIndex = 19;
            this.cbxTranscoding.SelectedIndexChanged += new System.EventHandler(this.cbxTranscoding_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Default:";
            // 
            // olvTranscoding
            // 
            this.olvTranscoding.AllColumns.Add(this.olvCodec);
            this.olvTranscoding.AllColumns.Add(this.olvLossless);
            this.olvTranscoding.AllColumns.Add(this.olvTranscode);
            this.olvTranscoding.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvTranscoding.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvCodec,
            this.olvLossless,
            this.olvTranscode});
            this.olvTranscoding.FullRowSelect = true;
            this.olvTranscoding.Location = new System.Drawing.Point(113, 47);
            this.olvTranscoding.Name = "olvTranscoding";
            this.olvTranscoding.ShowGroups = false;
            this.olvTranscoding.Size = new System.Drawing.Size(398, 413);
            this.olvTranscoding.TabIndex = 18;
            this.olvTranscoding.UseCompatibleStateImageBehavior = false;
            this.olvTranscoding.UseTranslucentSelection = true;
            this.olvTranscoding.View = System.Windows.Forms.View.Details;
            this.olvTranscoding.SelectionChanged += new System.EventHandler(this.olvTranscoding_SelectionChanged);
            // 
            // olvCodec
            // 
            this.olvCodec.AspectName = "description";
            this.olvCodec.FillsFreeSpace = true;
            this.olvCodec.Text = "Codec";
            // 
            // olvLossless
            // 
            this.olvLossless.AspectName = "lossless";
            this.olvLossless.Text = "Lossless";
            // 
            // olvTranscode
            // 
            this.olvTranscode.AspectName = "output";
            this.olvTranscode.Text = "Transcoding";
            this.olvTranscode.Width = 100;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 530);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "FLAC compression:";
            // 
            // cbxFLACCompression
            // 
            this.cbxFLACCompression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxFLACCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFLACCompression.FormattingEnabled = true;
            this.cbxFLACCompression.Items.AddRange(new object[] {
            "Fast",
            "Good",
            "Best"});
            this.cbxFLACCompression.Location = new System.Drawing.Point(113, 527);
            this.cbxFLACCompression.Name = "cbxFLACCompression";
            this.cbxFLACCompression.Size = new System.Drawing.Size(100, 21);
            this.cbxFLACCompression.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 503);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "LPCM container:";
            // 
            // cbxLPCMContainer
            // 
            this.cbxLPCMContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxLPCMContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLPCMContainer.FormattingEnabled = true;
            this.cbxLPCMContainer.Items.AddRange(new object[] {
            "Raw",
            "Wavex"});
            this.cbxLPCMContainer.Location = new System.Drawing.Point(113, 500);
            this.cbxLPCMContainer.Name = "cbxLPCMContainer";
            this.cbxLPCMContainer.Size = new System.Drawing.Size(100, 21);
            this.cbxLPCMContainer.TabIndex = 21;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.chkZLibCompression);
            this.groupBox5.Location = new System.Drawing.Point(15, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(523, 46);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Subtitles";
            // 
            // chkZLibCompression
            // 
            this.chkZLibCompression.AutoSize = true;
            this.chkZLibCompression.Location = new System.Drawing.Point(10, 20);
            this.chkZLibCompression.Name = "chkZLibCompression";
            this.chkZLibCompression.Size = new System.Drawing.Size(134, 17);
            this.chkZLibCompression.TabIndex = 16;
            this.chkZLibCompression.Text = "Use zlib compression";
            this.chkZLibCompression.UseVisualStyleBackColor = true;
            // 
            // tabTitles
            // 
            this.tabTitles.Controls.Add(this.groupBox9);
            this.tabTitles.Controls.Add(this.groupBox8);
            this.tabTitles.Controls.Add(this.groupBox7);
            this.tabTitles.Location = new System.Drawing.Point(4, 22);
            this.tabTitles.Name = "tabTitles";
            this.tabTitles.Size = new System.Drawing.Size(552, 677);
            this.tabTitles.TabIndex = 3;
            this.tabTitles.Text = "Titles";
            this.tabTitles.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.lnkTitleFilenameTags);
            this.groupBox9.Controls.Add(this.lnkTitleNameTags);
            this.groupBox9.Controls.Add(this.txtTitleFilename);
            this.groupBox9.Controls.Add(this.radTitlesFilenameCustom);
            this.groupBox9.Controls.Add(this.radTitlesFilenameBasedOnName);
            this.groupBox9.Controls.Add(this.label19);
            this.groupBox9.Controls.Add(this.txtTitleName);
            this.groupBox9.Controls.Add(this.label21);
            this.groupBox9.Location = new System.Drawing.Point(15, 225);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(523, 152);
            this.groupBox9.TabIndex = 30;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Default title settings";
            // 
            // lnkTitleFilenameTags
            // 
            this.lnkTitleFilenameTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkTitleFilenameTags.AutoSize = true;
            this.lnkTitleFilenameTags.Location = new System.Drawing.Point(438, 126);
            this.lnkTitleFilenameTags.Name = "lnkTitleFilenameTags";
            this.lnkTitleFilenameTags.Size = new System.Drawing.Size(78, 13);
            this.lnkTitleFilenameTags.TabIndex = 38;
            this.lnkTitleFilenameTags.TabStop = true;
            this.lnkTitleFilenameTags.Text = "Available tags";
            this.lnkTitleFilenameTags.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkTitleFilenameTags.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTitleFilenameTags_LinkClicked);
            // 
            // lnkTitleNameTags
            // 
            this.lnkTitleNameTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkTitleNameTags.AutoSize = true;
            this.lnkTitleNameTags.Location = new System.Drawing.Point(438, 45);
            this.lnkTitleNameTags.Name = "lnkTitleNameTags";
            this.lnkTitleNameTags.Size = new System.Drawing.Size(78, 13);
            this.lnkTitleNameTags.TabIndex = 37;
            this.lnkTitleNameTags.TabStop = true;
            this.lnkTitleNameTags.Text = "Available tags";
            this.lnkTitleNameTags.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkTitleNameTags.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTitleNameTags_LinkClicked);
            // 
            // txtTitleFilename
            // 
            this.txtTitleFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleFilename.Location = new System.Drawing.Point(113, 101);
            this.txtTitleFilename.MaxLength = 1000;
            this.txtTitleFilename.Name = "txtTitleFilename";
            this.txtTitleFilename.Size = new System.Drawing.Size(400, 22);
            this.txtTitleFilename.TabIndex = 36;
            // 
            // radTitlesFilenameCustom
            // 
            this.radTitlesFilenameCustom.AutoSize = true;
            this.radTitlesFilenameCustom.Location = new System.Drawing.Point(94, 78);
            this.radTitlesFilenameCustom.Name = "radTitlesFilenameCustom";
            this.radTitlesFilenameCustom.Size = new System.Drawing.Size(67, 17);
            this.radTitlesFilenameCustom.TabIndex = 35;
            this.radTitlesFilenameCustom.TabStop = true;
            this.radTitlesFilenameCustom.Text = "Custom:";
            this.radTitlesFilenameCustom.UseVisualStyleBackColor = true;
            this.radTitlesFilenameCustom.CheckedChanged += new System.EventHandler(this.radTitlesFilenameCustom_CheckedChanged);
            // 
            // radTitlesFilenameBasedOnName
            // 
            this.radTitlesFilenameBasedOnName.AutoSize = true;
            this.radTitlesFilenameBasedOnName.Location = new System.Drawing.Point(94, 55);
            this.radTitlesFilenameBasedOnName.Name = "radTitlesFilenameBasedOnName";
            this.radTitlesFilenameBasedOnName.Size = new System.Drawing.Size(127, 17);
            this.radTitlesFilenameBasedOnName.TabIndex = 34;
            this.radTitlesFilenameBasedOnName.TabStop = true;
            this.radTitlesFilenameBasedOnName.Text = "Based on title name";
            this.radTitlesFilenameBasedOnName.UseVisualStyleBackColor = true;
            this.radTitlesFilenameBasedOnName.CheckedChanged += new System.EventHandler(this.radTitlesFilenameBasedOnName_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 13);
            this.label19.TabIndex = 32;
            this.label19.Text = "Filename:";
            // 
            // txtTitleName
            // 
            this.txtTitleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleName.Location = new System.Drawing.Point(113, 20);
            this.txtTitleName.MaxLength = 1000;
            this.txtTitleName.Name = "txtTitleName";
            this.txtTitleName.Size = new System.Drawing.Size(400, 22);
            this.txtTitleName.TabIndex = 31;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 23);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(39, 13);
            this.label21.TabIndex = 30;
            this.label21.Text = "Name:";
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.radTitlesIncludeAll);
            this.groupBox8.Controls.Add(this.radTitlesIncludeLongest);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Controls.Add(this.updTitlesIncludeWithin);
            this.groupBox8.Controls.Add(this.chkTitlesIncludeWithin);
            this.groupBox8.Controls.Add(this.chkTitlesIgnoreMuted);
            this.groupBox8.Location = new System.Drawing.Point(15, 81);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(523, 128);
            this.groupBox8.TabIndex = 19;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Default title selection";
            // 
            // radTitlesIncludeAll
            // 
            this.radTitlesIncludeAll.AutoSize = true;
            this.radTitlesIncludeAll.Location = new System.Drawing.Point(8, 21);
            this.radTitlesIncludeAll.Name = "radTitlesIncludeAll";
            this.radTitlesIncludeAll.Size = new System.Drawing.Size(106, 17);
            this.radTitlesIncludeAll.TabIndex = 25;
            this.radTitlesIncludeAll.TabStop = true;
            this.radTitlesIncludeAll.Text = "Include all titles";
            this.radTitlesIncludeAll.UseVisualStyleBackColor = true;
            this.radTitlesIncludeAll.CheckedChanged += new System.EventHandler(this.radTitlesIncludeAll_CheckedChanged);
            // 
            // radTitlesIncludeLongest
            // 
            this.radTitlesIncludeLongest.AutoSize = true;
            this.radTitlesIncludeLongest.Location = new System.Drawing.Point(8, 44);
            this.radTitlesIncludeLongest.Name = "radTitlesIncludeLongest";
            this.radTitlesIncludeLongest.Size = new System.Drawing.Size(202, 17);
            this.radTitlesIncludeLongest.TabIndex = 26;
            this.radTitlesIncludeLongest.TabStop = true;
            this.radTitlesIncludeLongest.Text = "Include title with longest duration";
            this.radTitlesIncludeLongest.UseVisualStyleBackColor = true;
            this.radTitlesIncludeLongest.CheckedChanged += new System.EventHandler(this.radTitlesIncludeLongest_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(199, 68);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(153, 13);
            this.label18.TabIndex = 22;
            this.label18.Text = "seconds of longest duration";
            // 
            // updTitlesIncludeWithin
            // 
            this.updTitlesIncludeWithin.Increment = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.updTitlesIncludeWithin.Location = new System.Drawing.Point(154, 66);
            this.updTitlesIncludeWithin.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.updTitlesIncludeWithin.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.updTitlesIncludeWithin.Name = "updTitlesIncludeWithin";
            this.updTitlesIncludeWithin.Size = new System.Drawing.Size(42, 22);
            this.updTitlesIncludeWithin.TabIndex = 28;
            this.updTitlesIncludeWithin.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // chkTitlesIncludeWithin
            // 
            this.chkTitlesIncludeWithin.AutoSize = true;
            this.chkTitlesIncludeWithin.Location = new System.Drawing.Point(28, 67);
            this.chkTitlesIncludeWithin.Name = "chkTitlesIncludeWithin";
            this.chkTitlesIncludeWithin.Size = new System.Drawing.Size(128, 17);
            this.chkTitlesIncludeWithin.TabIndex = 27;
            this.chkTitlesIncludeWithin.Text = "Include titles within";
            this.chkTitlesIncludeWithin.UseVisualStyleBackColor = true;
            this.chkTitlesIncludeWithin.CheckedChanged += new System.EventHandler(this.chkTitlesIncludeWithin_CheckedChanged);
            // 
            // chkTitlesIgnoreMuted
            // 
            this.chkTitlesIgnoreMuted.AutoSize = true;
            this.chkTitlesIgnoreMuted.Location = new System.Drawing.Point(8, 103);
            this.chkTitlesIgnoreMuted.Name = "chkTitlesIgnoreMuted";
            this.chkTitlesIgnoreMuted.Size = new System.Drawing.Size(165, 17);
            this.chkTitlesIgnoreMuted.TabIndex = 29;
            this.chkTitlesIgnoreMuted.Text = "Ignore titles without audio";
            this.chkTitlesIgnoreMuted.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.updIgnoreTitlesLength);
            this.groupBox7.Controls.Add(this.chkIgnoreTitlesLength);
            this.groupBox7.Location = new System.Drawing.Point(15, 15);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(523, 55);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "General";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(215, 22);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(49, 13);
            this.label20.TabIndex = 17;
            this.label20.Text = "seconds";
            // 
            // updIgnoreTitlesLength
            // 
            this.updIgnoreTitlesLength.Increment = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.updIgnoreTitlesLength.Location = new System.Drawing.Point(162, 20);
            this.updIgnoreTitlesLength.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.updIgnoreTitlesLength.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.updIgnoreTitlesLength.Name = "updIgnoreTitlesLength";
            this.updIgnoreTitlesLength.Size = new System.Drawing.Size(50, 22);
            this.updIgnoreTitlesLength.TabIndex = 24;
            this.updIgnoreTitlesLength.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // chkIgnoreTitlesLength
            // 
            this.chkIgnoreTitlesLength.AutoSize = true;
            this.chkIgnoreTitlesLength.Location = new System.Drawing.Point(8, 21);
            this.chkIgnoreTitlesLength.Name = "chkIgnoreTitlesLength";
            this.chkIgnoreTitlesLength.Size = new System.Drawing.Size(155, 17);
            this.chkIgnoreTitlesLength.TabIndex = 23;
            this.chkIgnoreTitlesLength.Text = "Ignore titles shorter than";
            this.chkIgnoreTitlesLength.UseVisualStyleBackColor = true;
            this.chkIgnoreTitlesLength.CheckedChanged += new System.EventHandler(this.chkIgnoreTitlesLength_CheckedChanged);
            // 
            // tabTracks
            // 
            this.tabTracks.Controls.Add(this.groupBox12);
            this.tabTracks.Controls.Add(this.groupBox10);
            this.tabTracks.Controls.Add(this.groupBox11);
            this.tabTracks.Location = new System.Drawing.Point(4, 22);
            this.tabTracks.Name = "tabTracks";
            this.tabTracks.Padding = new System.Windows.Forms.Padding(3);
            this.tabTracks.Size = new System.Drawing.Size(552, 677);
            this.tabTracks.TabIndex = 2;
            this.tabTracks.Text = "Tracks";
            this.tabTracks.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.cbxSubtitleDefault);
            this.groupBox12.Controls.Add(this.label17);
            this.groupBox12.Controls.Add(this.cbxSubtitleOrder);
            this.groupBox12.Controls.Add(this.label11);
            this.groupBox12.Controls.Add(this.updSubtitleLimit);
            this.groupBox12.Controls.Add(this.chkSubtitleLimit);
            this.groupBox12.Controls.Add(this.label10);
            this.groupBox12.Controls.Add(this.radSubtitleIncludeFavourites);
            this.groupBox12.Controls.Add(this.radSubtitleIncludeAll);
            this.groupBox12.Location = new System.Drawing.Point(15, 507);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(523, 155);
            this.groupBox12.TabIndex = 6;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Subtitle defaults";
            // 
            // cbxSubtitleDefault
            // 
            this.cbxSubtitleDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSubtitleDefault.FormattingEnabled = true;
            this.cbxSubtitleDefault.Items.AddRange(new object[] {
            "No",
            "First track",
            "First forced track"});
            this.cbxSubtitleDefault.Location = new System.Drawing.Point(112, 121);
            this.cbxSubtitleDefault.Name = "cbxSubtitleDefault";
            this.cbxSubtitleDefault.Size = new System.Drawing.Size(140, 21);
            this.cbxSubtitleDefault.TabIndex = 51;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 124);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Default track:";
            // 
            // cbxSubtitleOrder
            // 
            this.cbxSubtitleOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSubtitleOrder.FormattingEnabled = true;
            this.cbxSubtitleOrder.Items.AddRange(new object[] {
            "Default",
            "By favourite language"});
            this.cbxSubtitleOrder.Location = new System.Drawing.Point(112, 94);
            this.cbxSubtitleOrder.Name = "cbxSubtitleOrder";
            this.cbxSubtitleOrder.Size = new System.Drawing.Size(140, 21);
            this.cbxSubtitleOrder.TabIndex = 49;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 97);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Order:";
            // 
            // updSubtitleLimit
            // 
            this.updSubtitleLimit.Location = new System.Drawing.Point(360, 64);
            this.updSubtitleLimit.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.updSubtitleLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSubtitleLimit.Name = "updSubtitleLimit";
            this.updSubtitleLimit.Size = new System.Drawing.Size(35, 22);
            this.updSubtitleLimit.TabIndex = 48;
            this.updSubtitleLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkSubtitleLimit
            // 
            this.chkSubtitleLimit.AutoSize = true;
            this.chkSubtitleLimit.Location = new System.Drawing.Point(112, 65);
            this.chkSubtitleLimit.Name = "chkSubtitleLimit";
            this.chkSubtitleLimit.Size = new System.Drawing.Size(248, 17);
            this.chkSubtitleLimit.TabIndex = 47;
            this.chkSubtitleLimit.Text = "Limit number of tracks in same language to";
            this.chkSubtitleLimit.UseVisualStyleBackColor = true;
            this.chkSubtitleLimit.CheckedChanged += new System.EventHandler(this.chkSubtitleLimit_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Include:";
            // 
            // radSubtitleIncludeFavourites
            // 
            this.radSubtitleIncludeFavourites.AutoSize = true;
            this.radSubtitleIncludeFavourites.Location = new System.Drawing.Point(112, 42);
            this.radSubtitleIncludeFavourites.Name = "radSubtitleIncludeFavourites";
            this.radSubtitleIncludeFavourites.Size = new System.Drawing.Size(155, 17);
            this.radSubtitleIncludeFavourites.TabIndex = 46;
            this.radSubtitleIncludeFavourites.TabStop = true;
            this.radSubtitleIncludeFavourites.Text = "Only favourite languages";
            this.radSubtitleIncludeFavourites.UseVisualStyleBackColor = true;
            // 
            // radSubtitleIncludeAll
            // 
            this.radSubtitleIncludeAll.AutoSize = true;
            this.radSubtitleIncludeAll.Checked = true;
            this.radSubtitleIncludeAll.Location = new System.Drawing.Point(112, 19);
            this.radSubtitleIncludeAll.Name = "radSubtitleIncludeAll";
            this.radSubtitleIncludeAll.Size = new System.Drawing.Size(38, 17);
            this.radSubtitleIncludeAll.TabIndex = 45;
            this.radSubtitleIncludeAll.TabStop = true;
            this.radSubtitleIncludeAll.Text = "All";
            this.radSubtitleIncludeAll.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.tableLayoutPanel1);
            this.groupBox10.Location = new System.Drawing.Point(15, 15);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(523, 196);
            this.groupBox10.TabIndex = 4;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Languages";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.olvLanguagesAll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.olvLanguagesFavourites, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label13, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 19);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 169);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // olvLanguagesAll
            // 
            this.olvLanguagesAll.AllColumns.Add(this.olvcLanguagesAll);
            this.olvLanguagesAll.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvcLanguagesAll});
            this.olvLanguagesAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvLanguagesAll.FullRowSelect = true;
            this.olvLanguagesAll.HideSelection = false;
            this.olvLanguagesAll.Location = new System.Drawing.Point(2, 20);
            this.olvLanguagesAll.Margin = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.olvLanguagesAll.Name = "olvLanguagesAll";
            this.olvLanguagesAll.ShowGroups = false;
            this.olvLanguagesAll.Size = new System.Drawing.Size(232, 147);
            this.olvLanguagesAll.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvLanguagesAll.TabIndex = 30;
            this.olvLanguagesAll.UseCompatibleStateImageBehavior = false;
            this.olvLanguagesAll.View = System.Windows.Forms.View.Details;
            this.olvLanguagesAll.SelectedIndexChanged += new System.EventHandler(this.olvLanguagesAll_SelectedIndexChanged);
            // 
            // olvcLanguagesAll
            // 
            this.olvcLanguagesAll.AspectName = "";
            this.olvcLanguagesAll.AspectToStringFormat = "";
            this.olvcLanguagesAll.FillsFreeSpace = true;
            this.olvcLanguagesAll.Text = "Language";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 1);
            this.label12.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "All:";
            // 
            // olvLanguagesFavourites
            // 
            this.olvLanguagesFavourites.AllColumns.Add(this.olvcLanguagesFavourites);
            this.olvLanguagesFavourites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvcLanguagesFavourites});
            this.olvLanguagesFavourites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvLanguagesFavourites.FullRowSelect = true;
            this.olvLanguagesFavourites.HideSelection = false;
            this.olvLanguagesFavourites.Location = new System.Drawing.Point(274, 20);
            this.olvLanguagesFavourites.Margin = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.olvLanguagesFavourites.Name = "olvLanguagesFavourites";
            this.olvLanguagesFavourites.ShowGroups = false;
            this.olvLanguagesFavourites.Size = new System.Drawing.Size(232, 147);
            this.olvLanguagesFavourites.TabIndex = 35;
            this.olvLanguagesFavourites.UseCompatibleStateImageBehavior = false;
            this.olvLanguagesFavourites.View = System.Windows.Forms.View.Details;
            this.olvLanguagesFavourites.SelectedIndexChanged += new System.EventHandler(this.olvLanguagesFavourites_SelectedIndexChanged);
            // 
            // olvcLanguagesFavourites
            // 
            this.olvcLanguagesFavourites.AspectName = "";
            this.olvcLanguagesFavourites.FillsFreeSpace = true;
            this.olvcLanguagesFavourites.Text = "Language";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(274, 1);
            this.label13.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Favourites:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnLanguageRemove, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnLanguageDown, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.btnLanguageAdd, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnLanguageUp, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(239, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(30, 147);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // btnLanguageRemove
            // 
            this.btnLanguageRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLanguageRemove.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnLanguageRemove.Location = new System.Drawing.Point(0, 75);
            this.btnLanguageRemove.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.btnLanguageRemove.Name = "btnLanguageRemove";
            this.btnLanguageRemove.Size = new System.Drawing.Size(30, 25);
            this.btnLanguageRemove.TabIndex = 33;
            this.btnLanguageRemove.Text = "¬";
            this.btnLanguageRemove.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLanguageRemove.UseVisualStyleBackColor = true;
            this.btnLanguageRemove.Click += new System.EventHandler(this.btnLanguageRemove_Click);
            // 
            // btnLanguageDown
            // 
            this.btnLanguageDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLanguageDown.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnLanguageDown.Location = new System.Drawing.Point(0, 121);
            this.btnLanguageDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnLanguageDown.Name = "btnLanguageDown";
            this.btnLanguageDown.Size = new System.Drawing.Size(30, 26);
            this.btnLanguageDown.TabIndex = 34;
            this.btnLanguageDown.Text = "¯";
            this.btnLanguageDown.UseVisualStyleBackColor = true;
            this.btnLanguageDown.Click += new System.EventHandler(this.btnLanguageDown_Click);
            // 
            // btnLanguageAdd
            // 
            this.btnLanguageAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLanguageAdd.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnLanguageAdd.Location = new System.Drawing.Point(0, 46);
            this.btnLanguageAdd.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnLanguageAdd.Name = "btnLanguageAdd";
            this.btnLanguageAdd.Size = new System.Drawing.Size(30, 25);
            this.btnLanguageAdd.TabIndex = 32;
            this.btnLanguageAdd.Text = "®";
            this.btnLanguageAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLanguageAdd.UseVisualStyleBackColor = true;
            this.btnLanguageAdd.Click += new System.EventHandler(this.btnLanguageAdd_Click);
            // 
            // btnLanguageUp
            // 
            this.btnLanguageUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLanguageUp.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnLanguageUp.Location = new System.Drawing.Point(0, 0);
            this.btnLanguageUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnLanguageUp.Name = "btnLanguageUp";
            this.btnLanguageUp.Size = new System.Drawing.Size(30, 25);
            this.btnLanguageUp.TabIndex = 31;
            this.btnLanguageUp.Text = "­";
            this.btnLanguageUp.UseVisualStyleBackColor = true;
            this.btnLanguageUp.Click += new System.EventHandler(this.btnLanguageUp_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.Controls.Add(this.txtAudioCommentaryTrackName);
            this.groupBox11.Controls.Add(this.chkAudioIncludeCommentaryTracks);
            this.groupBox11.Controls.Add(this.cbxAudioMainCore);
            this.groupBox11.Controls.Add(this.label16);
            this.groupBox11.Controls.Add(this.updAudioLimit);
            this.groupBox11.Controls.Add(this.chkAudioLimit);
            this.groupBox11.Controls.Add(this.cbxAudioOrder);
            this.groupBox11.Controls.Add(this.label15);
            this.groupBox11.Controls.Add(this.chkAudioIncludeFirst);
            this.groupBox11.Controls.Add(this.chkAudioIncludeNonFavourite);
            this.groupBox11.Controls.Add(this.chkAudioIncludeQuality);
            this.groupBox11.Controls.Add(this.label14);
            this.groupBox11.Controls.Add(this.radAudioIncludeFavourites);
            this.groupBox11.Controls.Add(this.radAudioIncludeAll);
            this.groupBox11.Location = new System.Drawing.Point(15, 221);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(523, 276);
            this.groupBox11.TabIndex = 5;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Audio defaults";
            // 
            // txtAudioCommentaryTrackName
            // 
            this.txtAudioCommentaryTrackName.Location = new System.Drawing.Point(149, 157);
            this.txtAudioCommentaryTrackName.Name = "txtAudioCommentaryTrackName";
            this.txtAudioCommentaryTrackName.Size = new System.Drawing.Size(138, 22);
            this.txtAudioCommentaryTrackName.TabIndex = 46;
            this.txtAudioCommentaryTrackName.Text = "Commentary";
            // 
            // chkAudioIncludeCommentaryTracks
            // 
            this.chkAudioIncludeCommentaryTracks.AutoSize = true;
            this.chkAudioIncludeCommentaryTracks.Location = new System.Drawing.Point(130, 134);
            this.chkAudioIncludeCommentaryTracks.Name = "chkAudioIncludeCommentaryTracks";
            this.chkAudioIncludeCommentaryTracks.Size = new System.Drawing.Size(277, 17);
            this.chkAudioIncludeCommentaryTracks.TabIndex = 45;
            this.chkAudioIncludeCommentaryTracks.Text = "Include and rename assumed commentary tracks:";
            this.chkAudioIncludeCommentaryTracks.UseVisualStyleBackColor = true;
            this.chkAudioIncludeCommentaryTracks.CheckedChanged += new System.EventHandler(this.chkAudioIncludeCommentaryTracks_CheckedChanged);
            // 
            // cbxAudioMainCore
            // 
            this.cbxAudioMainCore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAudioMainCore.FormattingEnabled = true;
            this.cbxAudioMainCore.Items.AddRange(new object[] {
            "Include main (lossless) track only",
            "Include core (lossy) track only",
            "Include both main and core tracks"});
            this.cbxAudioMainCore.Location = new System.Drawing.Point(210, 214);
            this.cbxAudioMainCore.Name = "cbxAudioMainCore";
            this.cbxAudioMainCore.Size = new System.Drawing.Size(199, 21);
            this.cbxAudioMainCore.TabIndex = 43;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(109, 217);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 13);
            this.label16.TabIndex = 24;
            this.label16.Text = "Main/core tracks:";
            // 
            // updAudioLimit
            // 
            this.updAudioLimit.Location = new System.Drawing.Point(360, 185);
            this.updAudioLimit.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.updAudioLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updAudioLimit.Name = "updAudioLimit";
            this.updAudioLimit.Size = new System.Drawing.Size(35, 22);
            this.updAudioLimit.TabIndex = 42;
            this.updAudioLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAudioLimit
            // 
            this.chkAudioLimit.AutoSize = true;
            this.chkAudioLimit.Location = new System.Drawing.Point(112, 186);
            this.chkAudioLimit.Name = "chkAudioLimit";
            this.chkAudioLimit.Size = new System.Drawing.Size(248, 17);
            this.chkAudioLimit.TabIndex = 41;
            this.chkAudioLimit.Text = "Limit number of tracks in same language to";
            this.chkAudioLimit.UseVisualStyleBackColor = true;
            this.chkAudioLimit.CheckedChanged += new System.EventHandler(this.chkAudioLimit_CheckedChanged);
            // 
            // cbxAudioOrder
            // 
            this.cbxAudioOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAudioOrder.FormattingEnabled = true;
            this.cbxAudioOrder.Items.AddRange(new object[] {
            "Default",
            "By favourite language",
            "By quality"});
            this.cbxAudioOrder.Location = new System.Drawing.Point(112, 243);
            this.cbxAudioOrder.Name = "cbxAudioOrder";
            this.cbxAudioOrder.Size = new System.Drawing.Size(140, 21);
            this.cbxAudioOrder.TabIndex = 44;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 246);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "Order:";
            // 
            // chkAudioIncludeFirst
            // 
            this.chkAudioIncludeFirst.AutoSize = true;
            this.chkAudioIncludeFirst.Location = new System.Drawing.Point(130, 111);
            this.chkAudioIncludeFirst.Name = "chkAudioIncludeFirst";
            this.chkAudioIncludeFirst.Size = new System.Drawing.Size(240, 17);
            this.chkAudioIncludeFirst.TabIndex = 40;
            this.chkAudioIncludeFirst.Text = "Include first audio track (by default order)";
            this.chkAudioIncludeFirst.UseVisualStyleBackColor = true;
            // 
            // chkAudioIncludeNonFavourite
            // 
            this.chkAudioIncludeNonFavourite.AutoSize = true;
            this.chkAudioIncludeNonFavourite.Location = new System.Drawing.Point(130, 88);
            this.chkAudioIncludeNonFavourite.Name = "chkAudioIncludeNonFavourite";
            this.chkAudioIncludeNonFavourite.Size = new System.Drawing.Size(328, 17);
            this.chkAudioIncludeNonFavourite.TabIndex = 39;
            this.chkAudioIncludeNonFavourite.Text = "Include non-favourite languages if no favourite languages";
            this.chkAudioIncludeNonFavourite.UseVisualStyleBackColor = true;
            // 
            // chkAudioIncludeQuality
            // 
            this.chkAudioIncludeQuality.AutoSize = true;
            this.chkAudioIncludeQuality.Location = new System.Drawing.Point(130, 65);
            this.chkAudioIncludeQuality.Name = "chkAudioIncludeQuality";
            this.chkAudioIncludeQuality.Size = new System.Drawing.Size(288, 17);
            this.chkAudioIncludeQuality.TabIndex = 38;
            this.chkAudioIncludeQuality.Text = "Include non-favourite languages if quality is better";
            this.chkAudioIncludeQuality.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "Include:";
            // 
            // radAudioIncludeFavourites
            // 
            this.radAudioIncludeFavourites.AutoSize = true;
            this.radAudioIncludeFavourites.Location = new System.Drawing.Point(112, 42);
            this.radAudioIncludeFavourites.Name = "radAudioIncludeFavourites";
            this.radAudioIncludeFavourites.Size = new System.Drawing.Size(155, 17);
            this.radAudioIncludeFavourites.TabIndex = 37;
            this.radAudioIncludeFavourites.TabStop = true;
            this.radAudioIncludeFavourites.Text = "Only favourite languages";
            this.radAudioIncludeFavourites.UseVisualStyleBackColor = true;
            this.radAudioIncludeFavourites.CheckedChanged += new System.EventHandler(this.radAudioIncludeFavourites_CheckedChanged);
            // 
            // radAudioIncludeAll
            // 
            this.radAudioIncludeAll.AutoSize = true;
            this.radAudioIncludeAll.Checked = true;
            this.radAudioIncludeAll.Location = new System.Drawing.Point(112, 19);
            this.radAudioIncludeAll.Name = "radAudioIncludeAll";
            this.radAudioIncludeAll.Size = new System.Drawing.Size(38, 17);
            this.radAudioIncludeAll.TabIndex = 36;
            this.radAudioIncludeAll.TabStop = true;
            this.radAudioIncludeAll.Text = "All";
            this.radAudioIncludeAll.UseVisualStyleBackColor = true;
            this.radAudioIncludeAll.CheckedChanged += new System.EventHandler(this.radAudioIncludeAll_CheckedChanged);
            // 
            // tabTroubleshooting
            // 
            this.tabTroubleshooting.Controls.Add(this.label24);
            this.tabTroubleshooting.Controls.Add(this.lnkForumThread);
            this.tabTroubleshooting.Controls.Add(this.btnErrorLoggingDisable);
            this.tabTroubleshooting.Controls.Add(this.btnErrorLoggingEnable);
            this.tabTroubleshooting.Controls.Add(this.lblErrorLoggingStatus);
            this.tabTroubleshooting.Controls.Add(this.label23);
            this.tabTroubleshooting.Controls.Add(this.label22);
            this.tabTroubleshooting.Location = new System.Drawing.Point(4, 22);
            this.tabTroubleshooting.Name = "tabTroubleshooting";
            this.tabTroubleshooting.Size = new System.Drawing.Size(552, 677);
            this.tabTroubleshooting.TabIndex = 4;
            this.tabTroubleshooting.Text = "Troubleshooting";
            this.tabTroubleshooting.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(15, 73);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(522, 128);
            this.label24.TabIndex = 14;
            this.label24.Text = resources.GetString("label24.Text");
            // 
            // lnkForumThread
            // 
            this.lnkForumThread.AutoSize = true;
            this.lnkForumThread.Location = new System.Drawing.Point(46, 49);
            this.lnkForumThread.Name = "lnkForumThread";
            this.lnkForumThread.Size = new System.Drawing.Size(169, 13);
            this.lnkForumThread.TabIndex = 13;
            this.lnkForumThread.TabStop = true;
            this.lnkForumThread.Text = "BatchMKV on MakeMKV forums";
            this.lnkForumThread.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkForumThread_LinkClicked);
            // 
            // btnErrorLoggingDisable
            // 
            this.btnErrorLoggingDisable.Location = new System.Drawing.Point(271, 238);
            this.btnErrorLoggingDisable.Name = "btnErrorLoggingDisable";
            this.btnErrorLoggingDisable.Size = new System.Drawing.Size(150, 26);
            this.btnErrorLoggingDisable.TabIndex = 201;
            this.btnErrorLoggingDisable.Text = "Disable Error Logging";
            this.btnErrorLoggingDisable.UseVisualStyleBackColor = true;
            this.btnErrorLoggingDisable.Click += new System.EventHandler(this.btnErrorLoggingDisable_Click);
            // 
            // btnErrorLoggingEnable
            // 
            this.btnErrorLoggingEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnErrorLoggingEnable.Location = new System.Drawing.Point(116, 238);
            this.btnErrorLoggingEnable.Name = "btnErrorLoggingEnable";
            this.btnErrorLoggingEnable.Size = new System.Drawing.Size(150, 26);
            this.btnErrorLoggingEnable.TabIndex = 200;
            this.btnErrorLoggingEnable.Text = "Enable Error Logging";
            this.btnErrorLoggingEnable.UseVisualStyleBackColor = true;
            this.btnErrorLoggingEnable.Click += new System.EventHandler(this.btnErrorLoggingEnable_Click);
            // 
            // lblErrorLoggingStatus
            // 
            this.lblErrorLoggingStatus.AutoSize = true;
            this.lblErrorLoggingStatus.Location = new System.Drawing.Point(113, 212);
            this.lblErrorLoggingStatus.Name = "lblErrorLoggingStatus";
            this.lblErrorLoggingStatus.Size = new System.Drawing.Size(137, 13);
            this.lblErrorLoggingStatus.TabIndex = 10;
            this.lblErrorLoggingStatus.Text = "Error logging is disabled.";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(15, 212);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(42, 13);
            this.label23.TabIndex = 9;
            this.label23.Text = "Status:";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(15, 15);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(522, 34);
            this.label22.TabIndex = 8;
            this.label22.Text = "If you experience any bugs or other issues using BatchMKV, or have suggestions fo" +
    "r improvements, new features, etc. please go to the BatchMKV thread on the MakeM" +
    "KV forums:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 724);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(416, 724);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 100;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dlgMakeMKVPath
            // 
            this.dlgMakeMKVPath.Description = "Select folder containing MakeMKV executable:";
            this.dlgMakeMKVPath.ShowNewFolderButton = false;
            // 
            // dlgOutputPath
            // 
            this.dlgOutputPath.Description = "Select the default output folder:";
            // 
            // dlgScanOnStart
            // 
            this.dlgScanOnStart.Description = "Select folder to automatically scan on application start:";
            // 
            // dlgMKVToolNixPath
            // 
            this.dlgMKVToolNixPath.Description = "Select folder containing MKVToolNix executables:";
            this.dlgMKVToolNixPath.ShowNewFolderButton = false;
            // 
            // fSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 762);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 900);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 700);
            this.Name = "fSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fSettings_FormClosing);
            this.Load += new System.EventHandler(this.fSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabTranscoding.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvTranscoding)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabTitles.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updTitlesIncludeWithin)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updIgnoreTitlesLength)).EndInit();
            this.tabTracks.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSubtitleLimit)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvLanguagesAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvLanguagesFavourites)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updAudioLimit)).EndInit();
            this.tabTroubleshooting.ResumeLayout(false);
            this.tabTroubleshooting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTranscoding;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxSourceAction;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtOutputSpecified;
        private System.Windows.Forms.RadioButton radOutputSpecified;
        private System.Windows.Forms.RadioButton radOutputSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseMakeMKV;
        private System.Windows.Forms.TextBox txtMakeMKVPath;
        private System.Windows.Forms.FolderBrowserDialog dlgMakeMKVPath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnBrowseArchive;
        private System.Windows.Forms.TextBox txtArchiveFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgOutputPath;
        private System.Windows.Forms.FolderBrowserDialog dlgArchivePath;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkScanOnStart;
        private System.Windows.Forms.Button btnBrowseScanOnStart;
        private System.Windows.Forms.TextBox txtScanOnStart;
        private System.Windows.Forms.FolderBrowserDialog dlgScanOnStart;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxLPCMContainer;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkZLibCompression;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxFLACCompression;
        private System.Windows.Forms.ComboBox cbxTranscoding;
        private System.Windows.Forms.Label label4;
        private BrightIdeasSoftware.ObjectListView olvTranscoding;
        private BrightIdeasSoftware.OLVColumn olvCodec;
        private BrightIdeasSoftware.OLVColumn olvLossless;
        private BrightIdeasSoftware.OLVColumn olvTranscode;
        private System.Windows.Forms.Button btnTranscodingReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbxTranscodingDefault;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkOutputOverwrite;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabTracks;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnBrowseMKVToolNix;
        private System.Windows.Forms.TextBox txtMKVToolNixPath;
        private System.Windows.Forms.FolderBrowserDialog dlgMKVToolNixPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown updSubtitleLimit;
        private System.Windows.Forms.CheckBox chkSubtitleLimit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radSubtitleIncludeFavourites;
        private System.Windows.Forms.RadioButton radSubtitleIncludeAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BrightIdeasSoftware.ObjectListView olvLanguagesAll;
        private BrightIdeasSoftware.OLVColumn olvcLanguagesAll;
        private System.Windows.Forms.Label label12;
        private BrightIdeasSoftware.ObjectListView olvLanguagesFavourites;
        private BrightIdeasSoftware.OLVColumn olvcLanguagesFavourites;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnLanguageUp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnLanguageDown;
        private System.Windows.Forms.Button btnLanguageAdd;
        private System.Windows.Forms.Button btnLanguageRemove;
        private System.Windows.Forms.ComboBox cbxSubtitleOrder;
        private System.Windows.Forms.ComboBox cbxAudioMainCore;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown updAudioLimit;
        private System.Windows.Forms.CheckBox chkAudioLimit;
        private System.Windows.Forms.ComboBox cbxAudioOrder;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkAudioIncludeFirst;
        private System.Windows.Forms.CheckBox chkAudioIncludeNonFavourite;
        private System.Windows.Forms.CheckBox chkAudioIncludeQuality;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton radAudioIncludeFavourites;
        private System.Windows.Forms.RadioButton radAudioIncludeAll;
        private System.Windows.Forms.ComboBox cbxSubtitleDefault;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage tabTitles;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown updIgnoreTitlesLength;
        private System.Windows.Forms.CheckBox chkIgnoreTitlesLength;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radTitlesIncludeAll;
        private System.Windows.Forms.RadioButton radTitlesIncludeLongest;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown updTitlesIncludeWithin;
        private System.Windows.Forms.CheckBox chkTitlesIncludeWithin;
        private System.Windows.Forms.CheckBox chkTitlesIgnoreMuted;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox txtTitleFilename;
        private System.Windows.Forms.RadioButton radTitlesFilenameCustom;
        private System.Windows.Forms.RadioButton radTitlesFilenameBasedOnName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtTitleName;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.LinkLabel lnkOutputFolderTags;
        private System.Windows.Forms.LinkLabel lnkTitleFilenameTags;
        private System.Windows.Forms.LinkLabel lnkTitleNameTags;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.CheckBox chkClearMissingSourcesOnStart;
        private System.Windows.Forms.CheckBox chkClearFinishedSourcesOnExit;
        private System.Windows.Forms.CheckBox chkRestoreSourcesOnRestart;
        private System.Windows.Forms.ComboBox cbxPurgeSourceState;
        private System.Windows.Forms.CheckBox chkPurgeSourceState;
        private System.Windows.Forms.TextBox txtAudioCommentaryTrackName;
        private System.Windows.Forms.CheckBox chkAudioIncludeCommentaryTracks;
        private System.Windows.Forms.TabPage tabTroubleshooting;
        private System.Windows.Forms.Label lblErrorLoggingStatus;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnErrorLoggingDisable;
        private System.Windows.Forms.Button btnErrorLoggingEnable;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.LinkLabel lnkForumThread;
    }
}