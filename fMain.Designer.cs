namespace BatchMKV
{
    partial class fMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.olvSources = new BrightIdeasSoftware.ObjectListView();
            this.olvTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvPriority = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tlvTitles = new BrightIdeasSoftware.TreeListView();
            this.otvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.otvDescription = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.otvOrder = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.otvDefault = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imlTracks = new System.Windows.Forms.ImageList(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.lblConversionStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpInformation = new System.Windows.Forms.GroupBox();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPropertyValue = new System.Windows.Forms.TextBox();
            this.cbxProperties = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxSourceAction = new System.Windows.Forms.ComboBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputSpecified = new System.Windows.Forms.TextBox();
            this.radOutputSpecified = new System.Windows.Forms.RadioButton();
            this.radOutputSource = new System.Windows.Forms.RadioButton();
            this.imlSources = new System.Windows.Forms.ImageList(this.components);
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanDrivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemScanRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeSourcesAutomaticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertSelectedSourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFinishedSourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMissingSourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllSourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.troubleshootingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsRemaining = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsSources = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressTotal = new System.Windows.Forms.ToolStripProgressBar();
            this.dlgOutputPath = new System.Windows.Forms.FolderBrowserDialog();
            this.ctmSource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmSource_ToggleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_ToggleItemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmSource_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_UnselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_PrioritySeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmSource_Priority = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_PriorityLow = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_PriorityMedium = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_PriorityHigh = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_ClearSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmSource_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_ClearFinished = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_ClearMissing = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_ClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmSource_AnalyzeSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmSource_Analyze = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmTitle_ToggleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_ToggleItemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTitle_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_UnselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTitle_ExpandSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_ExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_CollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTitle_ApplyDefaultSelections = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_ApplyDefaultSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_ConvertSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTitle_Convert = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTitle_AbortConversion = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmTrack_ToggleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_ToggleItemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTrack_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_SelectAllAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_SelectAllSubtitle = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_UnselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_UnselectAllAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmTrack_UnselectAllSubtitle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ctmTrack_UseTrackDefaults = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvSources)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlvTitles)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.grpInformation.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.ctmSource.SuspendLayout();
            this.ctmTitle.SuspendLayout();
            this.ctmTrack.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.olvSources);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(982, 609);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 0;
            // 
            // olvSources
            // 
            this.olvSources.AllColumns.Add(this.olvTitle);
            this.olvSources.AllColumns.Add(this.olvType);
            this.olvSources.AllColumns.Add(this.olvSize);
            this.olvSources.AllColumns.Add(this.olvStatus);
            this.olvSources.AllColumns.Add(this.olvPriority);
            this.olvSources.AllowDrop = true;
            this.olvSources.CheckBoxes = true;
            this.olvSources.CheckedAspectName = "Convert";
            this.olvSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvTitle,
            this.olvType,
            this.olvSize,
            this.olvStatus,
            this.olvPriority});
            this.olvSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvSources.FullRowSelect = true;
            this.olvSources.HideSelection = false;
            this.olvSources.Location = new System.Drawing.Point(0, 0);
            this.olvSources.MultiSelect = false;
            this.olvSources.Name = "olvSources";
            this.olvSources.ShowGroups = false;
            this.olvSources.ShowItemToolTips = true;
            this.olvSources.Size = new System.Drawing.Size(340, 609);
            this.olvSources.TabIndex = 0;
            this.olvSources.UseCompatibleStateImageBehavior = false;
            this.olvSources.UseTranslucentSelection = true;
            this.olvSources.View = System.Windows.Forms.View.Details;
            this.olvSources.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.olvSources_CellRightClick);
            this.olvSources.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.olvSources_CellToolTipShowing);
            this.olvSources.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.olvSources_ItemChecked);
            this.olvSources.SelectedIndexChanged += new System.EventHandler(this.olvSources_SelectedIndexChanged);
            this.olvSources.DragDrop += new System.Windows.Forms.DragEventHandler(this.olvSources_DragDrop);
            this.olvSources.DragEnter += new System.Windows.Forms.DragEventHandler(this.olvSources_DragEnter);
            // 
            // olvTitle
            // 
            this.olvTitle.AspectName = "Title";
            this.olvTitle.CheckBoxes = true;
            this.olvTitle.FillsFreeSpace = true;
            this.olvTitle.Text = "Title";
            // 
            // olvType
            // 
            this.olvType.AspectName = "Type";
            this.olvType.Text = "Type";
            this.olvType.Width = 100;
            // 
            // olvSize
            // 
            this.olvSize.AspectName = "Size";
            this.olvSize.Text = "Size";
            // 
            // olvStatus
            // 
            this.olvStatus.AspectName = "CurrentStatus";
            this.olvStatus.Text = "Status";
            this.olvStatus.Width = 120;
            // 
            // olvPriority
            // 
            this.olvPriority.IsEditable = false;
            this.olvPriority.Text = "Priority";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tlvTitles);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Panel2.Controls.Add(this.grpInformation);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(638, 609);
            this.splitContainer2.SplitterDistance = 338;
            this.splitContainer2.TabIndex = 0;
            // 
            // tlvTitles
            // 
            this.tlvTitles.AllColumns.Add(this.otvType);
            this.tlvTitles.AllColumns.Add(this.otvDescription);
            this.tlvTitles.AllColumns.Add(this.otvOrder);
            this.tlvTitles.AllColumns.Add(this.otvDefault);
            this.tlvTitles.CheckBoxes = true;
            this.tlvTitles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.otvType,
            this.otvDescription,
            this.otvOrder,
            this.otvDefault});
            this.tlvTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlvTitles.FullRowSelect = true;
            this.tlvTitles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvTitles.HideSelection = false;
            this.tlvTitles.Location = new System.Drawing.Point(0, 0);
            this.tlvTitles.MultiSelect = false;
            this.tlvTitles.Name = "tlvTitles";
            this.tlvTitles.OwnerDraw = true;
            this.tlvTitles.ShowGroups = false;
            this.tlvTitles.ShowImagesOnSubItems = true;
            this.tlvTitles.Size = new System.Drawing.Size(338, 609);
            this.tlvTitles.SmallImageList = this.imlTracks;
            this.tlvTitles.TabIndex = 1;
            this.tlvTitles.UseCompatibleStateImageBehavior = false;
            this.tlvTitles.UseTranslucentSelection = true;
            this.tlvTitles.View = System.Windows.Forms.View.Details;
            this.tlvTitles.VirtualMode = true;
            this.tlvTitles.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.tlvTitles_CellRightClick);
            this.tlvTitles.SubItemChecking += new System.EventHandler<BrightIdeasSoftware.SubItemCheckingEventArgs>(this.tlvTitles_SubItemChecking);
            this.tlvTitles.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.tlvTitles_ItemChecked);
            this.tlvTitles.SelectedIndexChanged += new System.EventHandler(this.tlvTitles_SelectedIndexChanged);
            // 
            // otvType
            // 
            this.otvType.Text = "Type";
            this.otvType.Width = 120;
            // 
            // otvDescription
            // 
            this.otvDescription.FillsFreeSpace = true;
            this.otvDescription.Text = "Description";
            this.otvDescription.Width = 180;
            // 
            // otvOrder
            // 
            this.otvOrder.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otvOrder.Text = "Order";
            this.otvOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otvOrder.Width = 45;
            // 
            // otvDefault
            // 
            this.otvDefault.CheckBoxes = true;
            this.otvDefault.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otvDefault.Text = "Default";
            this.otvDefault.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.otvDefault.Width = 50;
            // 
            // imlTracks
            // 
            this.imlTracks.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlTracks.ImageStream")));
            this.imlTracks.TransparentColor = System.Drawing.Color.Transparent;
            this.imlTracks.Images.SetKeyName(0, "icon_audio.png");
            this.imlTracks.Images.SetKeyName(1, "icon_text.png");
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnRetry);
            this.groupBox3.Controls.Add(this.btnAbort);
            this.groupBox3.Controls.Add(this.lblConversionStatus);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(8, 525);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(278, 75);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Conversion";
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRetry.Enabled = false;
            this.btnRetry.Location = new System.Drawing.Point(136, 42);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(63, 23);
            this.btnRetry.TabIndex = 10;
            this.btnRetry.Text = "Retry";
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(205, 42);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(63, 23);
            this.btnAbort.TabIndex = 11;
            this.btnAbort.Text = "Abort";
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // lblConversionStatus
            // 
            this.lblConversionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConversionStatus.AutoEllipsis = true;
            this.lblConversionStatus.Location = new System.Drawing.Point(53, 20);
            this.lblConversionStatus.Name = "lblConversionStatus";
            this.lblConversionStatus.Size = new System.Drawing.Size(213, 13);
            this.lblConversionStatus.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Status:";
            // 
            // grpInformation
            // 
            this.grpInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInformation.Controls.Add(this.txtInformation);
            this.grpInformation.Location = new System.Drawing.Point(8, 171);
            this.grpInformation.Name = "grpInformation";
            this.grpInformation.Size = new System.Drawing.Size(278, 348);
            this.grpInformation.TabIndex = 2;
            this.grpInformation.TabStop = false;
            this.grpInformation.Text = "Title information";
            // 
            // txtInformation
            // 
            this.txtInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInformation.Location = new System.Drawing.Point(9, 19);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.ReadOnly = true;
            this.txtInformation.Size = new System.Drawing.Size(259, 318);
            this.txtInformation.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtPropertyValue);
            this.groupBox2.Controls.Add(this.cbxProperties);
            this.groupBox2.Location = new System.Drawing.Point(8, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            // 
            // txtPropertyValue
            // 
            this.txtPropertyValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPropertyValue.Location = new System.Drawing.Point(148, 19);
            this.txtPropertyValue.Name = "txtPropertyValue";
            this.txtPropertyValue.Size = new System.Drawing.Size(120, 26);
            this.txtPropertyValue.TabIndex = 8;
            this.txtPropertyValue.Leave += new System.EventHandler(this.txtPropertyValue_Leave);
            // 
            // cbxProperties
            // 
            this.cbxProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProperties.FormattingEnabled = true;
            this.cbxProperties.Items.AddRange(new object[] {
            "Name",
            "Metadata language"});
            this.cbxProperties.Location = new System.Drawing.Point(9, 19);
            this.cbxProperties.Name = "cbxProperties";
            this.cbxProperties.Size = new System.Drawing.Size(133, 27);
            this.cbxProperties.TabIndex = 7;
            this.cbxProperties.SelectedValueChanged += new System.EventHandler(this.cbxProperties_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbxSourceAction);
            this.groupBox1.Controls.Add(this.btnBrowseOutput);
            this.groupBox1.Controls.Add(this.txtOutputSpecified);
            this.groupBox1.Controls.Add(this.radOutputSpecified);
            this.groupBox1.Controls.Add(this.radOutputSource);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Source files:";
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
            this.cbxSourceAction.Location = new System.Drawing.Point(125, 71);
            this.cbxSourceAction.Name = "cbxSourceAction";
            this.cbxSourceAction.Size = new System.Drawing.Size(143, 27);
            this.cbxSourceAction.TabIndex = 6;
            this.cbxSourceAction.Leave += new System.EventHandler(this.cbxSourceAction_Leave);
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutput.Location = new System.Drawing.Point(239, 39);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseOutput.TabIndex = 5;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputSpecified
            // 
            this.txtOutputSpecified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputSpecified.Location = new System.Drawing.Point(125, 40);
            this.txtOutputSpecified.Name = "txtOutputSpecified";
            this.txtOutputSpecified.Size = new System.Drawing.Size(108, 26);
            this.txtOutputSpecified.TabIndex = 4;
            this.txtOutputSpecified.Leave += new System.EventHandler(this.txtOutputSpecified_Leave);
            // 
            // radOutputSpecified
            // 
            this.radOutputSpecified.AutoSize = true;
            this.radOutputSpecified.Location = new System.Drawing.Point(10, 42);
            this.radOutputSpecified.Name = "radOutputSpecified";
            this.radOutputSpecified.Size = new System.Drawing.Size(125, 23);
            this.radOutputSpecified.TabIndex = 3;
            this.radOutputSpecified.TabStop = true;
            this.radOutputSpecified.Text = "Specified folder:";
            this.radOutputSpecified.UseVisualStyleBackColor = true;
            this.radOutputSpecified.CheckedChanged += new System.EventHandler(this.radOutputSpecified_CheckedChanged);
            this.radOutputSpecified.Leave += new System.EventHandler(this.radOutputSpecified_Leave);
            // 
            // radOutputSource
            // 
            this.radOutputSource.AutoSize = true;
            this.radOutputSource.Checked = true;
            this.radOutputSource.Location = new System.Drawing.Point(10, 19);
            this.radOutputSource.Name = "radOutputSource";
            this.radOutputSource.Size = new System.Drawing.Size(124, 23);
            this.radOutputSource.TabIndex = 2;
            this.radOutputSource.TabStop = true;
            this.radOutputSource.Text = "Same as source";
            this.radOutputSource.UseVisualStyleBackColor = true;
            this.radOutputSource.CheckedChanged += new System.EventHandler(this.radOutputSource_CheckedChanged);
            this.radOutputSource.Leave += new System.EventHandler(this.radOutputSource_Leave);
            // 
            // imlSources
            // 
            this.imlSources.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSources.ImageStream")));
            this.imlSources.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSources.Images.SetKeyName(0, "Low Priority-16.png");
            this.imlSources.Images.SetKeyName(1, "High Priority-16.png");
            // 
            // menuMain
            // 
            this.menuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sourcesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(984, 28);
            this.menuMain.TabIndex = 2;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanDrivesToolStripMenuItem,
            this.scanFolderToolStripMenuItem,
            this.toolStripMenuItemScanRecent,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // scanDrivesToolStripMenuItem
            // 
            this.scanDrivesToolStripMenuItem.Name = "scanDrivesToolStripMenuItem";
            this.scanDrivesToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.scanDrivesToolStripMenuItem.Text = "Scan Optical Drives";
            this.scanDrivesToolStripMenuItem.Click += new System.EventHandler(this.scanDrivesToolStripMenuItem_Click);
            // 
            // scanFolderToolStripMenuItem
            // 
            this.scanFolderToolStripMenuItem.Name = "scanFolderToolStripMenuItem";
            this.scanFolderToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.scanFolderToolStripMenuItem.Text = "Scan Folder...";
            this.scanFolderToolStripMenuItem.Click += new System.EventHandler(this.scanFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItemScanRecent
            // 
            this.toolStripMenuItemScanRecent.Name = "toolStripMenuItemScanRecent";
            this.toolStripMenuItemScanRecent.Size = new System.Drawing.Size(220, 26);
            this.toolStripMenuItemScanRecent.Text = "Recent Folders";
            this.toolStripMenuItemScanRecent.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(217, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // sourcesToolStripMenuItem
            // 
            this.sourcesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyzeSourcesAutomaticallyToolStripMenuItem,
            this.convertSelectedSourcesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.clearFinishedSourcesToolStripMenuItem,
            this.clearMissingSourcesToolStripMenuItem,
            this.clearAllSourcesToolStripMenuItem});
            this.sourcesToolStripMenuItem.Name = "sourcesToolStripMenuItem";
            this.sourcesToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.sourcesToolStripMenuItem.Text = "&Sources";
            // 
            // analyzeSourcesAutomaticallyToolStripMenuItem
            // 
            this.analyzeSourcesAutomaticallyToolStripMenuItem.Checked = true;
            this.analyzeSourcesAutomaticallyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.analyzeSourcesAutomaticallyToolStripMenuItem.Name = "analyzeSourcesAutomaticallyToolStripMenuItem";
            this.analyzeSourcesAutomaticallyToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.analyzeSourcesAutomaticallyToolStripMenuItem.Text = "Analyze Automatically";
            this.analyzeSourcesAutomaticallyToolStripMenuItem.Click += new System.EventHandler(this.analyzeSourcesAutomaticallyToolStripMenuItem_Click);
            // 
            // convertSelectedSourcesToolStripMenuItem
            // 
            this.convertSelectedSourcesToolStripMenuItem.Name = "convertSelectedSourcesToolStripMenuItem";
            this.convertSelectedSourcesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.convertSelectedSourcesToolStripMenuItem.Text = "Convert Selected Automatically";
            this.convertSelectedSourcesToolStripMenuItem.Click += new System.EventHandler(this.convertSelectedSourcesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(297, 6);
            // 
            // clearFinishedSourcesToolStripMenuItem
            // 
            this.clearFinishedSourcesToolStripMenuItem.Name = "clearFinishedSourcesToolStripMenuItem";
            this.clearFinishedSourcesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.clearFinishedSourcesToolStripMenuItem.Text = "Clear Finished";
            this.clearFinishedSourcesToolStripMenuItem.Click += new System.EventHandler(this.clearFinishedSourcesToolStripMenuItem_Click);
            // 
            // clearMissingSourcesToolStripMenuItem
            // 
            this.clearMissingSourcesToolStripMenuItem.Name = "clearMissingSourcesToolStripMenuItem";
            this.clearMissingSourcesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.clearMissingSourcesToolStripMenuItem.Text = "Clear Missing";
            this.clearMissingSourcesToolStripMenuItem.Click += new System.EventHandler(this.clearMissingSourcesToolStripMenuItem_Click);
            // 
            // clearAllSourcesToolStripMenuItem
            // 
            this.clearAllSourcesToolStripMenuItem.Name = "clearAllSourcesToolStripMenuItem";
            this.clearAllSourcesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.clearAllSourcesToolStripMenuItem.Text = "Clear All";
            this.clearAllSourcesToolStripMenuItem.Click += new System.EventHandler(this.clearAllSourcesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.troubleshootingToolStripMenuItem,
            this.donateToolStripMenuItem,
            this.toolStripMenuItem3,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // troubleshootingToolStripMenuItem
            // 
            this.troubleshootingToolStripMenuItem.Name = "troubleshootingToolStripMenuItem";
            this.troubleshootingToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.troubleshootingToolStripMenuItem.Text = "&Troubleshooting";
            this.troubleshootingToolStripMenuItem.Click += new System.EventHandler(this.troubleshootingToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.donateToolStripMenuItem.Text = "&Donate";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(202, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.aboutToolStripMenuItem.Text = "&About BatchMKV";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dlgBrowseFolder
            // 
            this.dlgBrowseFolder.Description = "Select folder to scan for sources:";
            this.dlgBrowseFolder.ShowNewFolderButton = false;
            // 
            // ssStatus
            // 
            this.ssStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress,
            this.tsMessage,
            this.tsRemaining,
            this.tsSources,
            this.tsProgressTotal});
            this.ssStatus.Location = new System.Drawing.Point(0, 635);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(984, 26);
            this.ssStatus.TabIndex = 3;
            this.ssStatus.Text = "ssStatus";
            // 
            // tsProgress
            // 
            this.tsProgress.AutoToolTip = true;
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(150, 23);
            this.tsProgress.ToolTipText = "Progress for current activity";
            this.tsProgress.Visible = false;
            // 
            // tsMessage
            // 
            this.tsMessage.Name = "tsMessage";
            this.tsMessage.Size = new System.Drawing.Size(889, 20);
            this.tsMessage.Spring = true;
            this.tsMessage.Text = "Ready.";
            this.tsMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsRemaining
            // 
            this.tsRemaining.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsRemaining.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsRemaining.Name = "tsRemaining";
            this.tsRemaining.Size = new System.Drawing.Size(179, 24);
            this.tsRemaining.Text = "To convert: 7 titles, 54 GB";
            this.tsRemaining.Visible = false;
            // 
            // tsSources
            // 
            this.tsSources.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.tsSources.Name = "tsSources";
            this.tsSources.Size = new System.Drawing.Size(80, 20);
            this.tsSources.Text = "0 source(s)";
            // 
            // tsProgressTotal
            // 
            this.tsProgressTotal.AutoToolTip = true;
            this.tsProgressTotal.Name = "tsProgressTotal";
            this.tsProgressTotal.Size = new System.Drawing.Size(150, 19);
            this.tsProgressTotal.ToolTipText = "Total progress";
            this.tsProgressTotal.Visible = false;
            // 
            // dlgOutputPath
            // 
            this.dlgOutputPath.Description = "Select the default output folder:";
            // 
            // ctmSource
            // 
            this.ctmSource.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctmSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmSource_ToggleItem,
            this.ctmSource_ToggleItemSeparator,
            this.ctmSource_SelectAll,
            this.ctmSource_UnselectAll,
            this.ctmSource_PrioritySeparator,
            this.ctmSource_Priority,
            this.ctmSource_ClearSeparator,
            this.ctmSource_Clear,
            this.ctmSource_ClearFinished,
            this.ctmSource_ClearMissing,
            this.ctmSource_ClearAll,
            this.ctmSource_AnalyzeSeparator,
            this.ctmSource_Analyze});
            this.ctmSource.Name = "ctmSource";
            this.ctmSource.Size = new System.Drawing.Size(171, 244);
            // 
            // ctmSource_ToggleItem
            // 
            this.ctmSource_ToggleItem.Name = "ctmSource_ToggleItem";
            this.ctmSource_ToggleItem.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_ToggleItem.Text = "Toggle Item";
            this.ctmSource_ToggleItem.Click += new System.EventHandler(this.ctmSource_ToggleItem_Click);
            // 
            // ctmSource_ToggleItemSeparator
            // 
            this.ctmSource_ToggleItemSeparator.Name = "ctmSource_ToggleItemSeparator";
            this.ctmSource_ToggleItemSeparator.Size = new System.Drawing.Size(167, 6);
            // 
            // ctmSource_SelectAll
            // 
            this.ctmSource_SelectAll.Name = "ctmSource_SelectAll";
            this.ctmSource_SelectAll.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_SelectAll.Text = "Select All";
            this.ctmSource_SelectAll.Click += new System.EventHandler(this.ctmSource_SelectAll_Click);
            // 
            // ctmSource_UnselectAll
            // 
            this.ctmSource_UnselectAll.Name = "ctmSource_UnselectAll";
            this.ctmSource_UnselectAll.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_UnselectAll.Text = "Unselect All";
            this.ctmSource_UnselectAll.Click += new System.EventHandler(this.ctmSource_UnselectAll_Click);
            // 
            // ctmSource_PrioritySeparator
            // 
            this.ctmSource_PrioritySeparator.Name = "ctmSource_PrioritySeparator";
            this.ctmSource_PrioritySeparator.Size = new System.Drawing.Size(167, 6);
            // 
            // ctmSource_Priority
            // 
            this.ctmSource_Priority.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmSource_PriorityLow,
            this.ctmSource_PriorityMedium,
            this.ctmSource_PriorityHigh});
            this.ctmSource_Priority.Name = "ctmSource_Priority";
            this.ctmSource_Priority.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_Priority.Text = "Priority";
            // 
            // ctmSource_PriorityLow
            // 
            this.ctmSource_PriorityLow.Name = "ctmSource_PriorityLow";
            this.ctmSource_PriorityLow.Size = new System.Drawing.Size(147, 26);
            this.ctmSource_PriorityLow.Text = "Low";
            this.ctmSource_PriorityLow.Click += new System.EventHandler(this.ctmSource_PriorityLow_Click);
            // 
            // ctmSource_PriorityMedium
            // 
            this.ctmSource_PriorityMedium.Name = "ctmSource_PriorityMedium";
            this.ctmSource_PriorityMedium.Size = new System.Drawing.Size(147, 26);
            this.ctmSource_PriorityMedium.Text = "Medium";
            this.ctmSource_PriorityMedium.Click += new System.EventHandler(this.ctmSource_PriorityMedium_Click);
            // 
            // ctmSource_PriorityHigh
            // 
            this.ctmSource_PriorityHigh.Name = "ctmSource_PriorityHigh";
            this.ctmSource_PriorityHigh.Size = new System.Drawing.Size(147, 26);
            this.ctmSource_PriorityHigh.Text = "High";
            this.ctmSource_PriorityHigh.Click += new System.EventHandler(this.ctmSource_PriorityHigh_Click);
            // 
            // ctmSource_ClearSeparator
            // 
            this.ctmSource_ClearSeparator.Name = "ctmSource_ClearSeparator";
            this.ctmSource_ClearSeparator.Size = new System.Drawing.Size(167, 6);
            // 
            // ctmSource_Clear
            // 
            this.ctmSource_Clear.Name = "ctmSource_Clear";
            this.ctmSource_Clear.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_Clear.Text = "Clear";
            this.ctmSource_Clear.Click += new System.EventHandler(this.ctmSource_Clear_Click);
            // 
            // ctmSource_ClearFinished
            // 
            this.ctmSource_ClearFinished.Name = "ctmSource_ClearFinished";
            this.ctmSource_ClearFinished.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_ClearFinished.Text = "Clear Finished";
            this.ctmSource_ClearFinished.Click += new System.EventHandler(this.ctmSource_ClearFinished_Click);
            // 
            // ctmSource_ClearMissing
            // 
            this.ctmSource_ClearMissing.Name = "ctmSource_ClearMissing";
            this.ctmSource_ClearMissing.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_ClearMissing.Text = "Clear Missing";
            this.ctmSource_ClearMissing.Click += new System.EventHandler(this.ctmSource_ClearMissing_Click);
            // 
            // ctmSource_ClearAll
            // 
            this.ctmSource_ClearAll.Name = "ctmSource_ClearAll";
            this.ctmSource_ClearAll.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_ClearAll.Text = "Clear All";
            this.ctmSource_ClearAll.Click += new System.EventHandler(this.ctmSource_ClearAll_Click);
            // 
            // ctmSource_AnalyzeSeparator
            // 
            this.ctmSource_AnalyzeSeparator.Name = "ctmSource_AnalyzeSeparator";
            this.ctmSource_AnalyzeSeparator.Size = new System.Drawing.Size(167, 6);
            // 
            // ctmSource_Analyze
            // 
            this.ctmSource_Analyze.Name = "ctmSource_Analyze";
            this.ctmSource_Analyze.Size = new System.Drawing.Size(170, 24);
            this.ctmSource_Analyze.Text = "Analyze";
            this.ctmSource_Analyze.Click += new System.EventHandler(this.ctmSource_Analyze_Click);
            // 
            // ctmTitle
            // 
            this.ctmTitle.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctmTitle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmTitle_ToggleItem,
            this.ctmTitle_ToggleItemSeparator,
            this.ctmTitle_SelectAll,
            this.ctmTitle_UnselectAll,
            this.toolStripSeparator4,
            this.ctmTitle_ExpandSelected,
            this.ctmTitle_ExpandAll,
            this.ctmTitle_CollapseAll,
            this.toolStripMenuItem1,
            this.ctmTitle_ApplyDefaultSelections,
            this.ctmTitle_ApplyDefaultSettings,
            this.ctmTitle_ConvertSeparator,
            this.ctmTitle_Convert,
            this.ctmTitle_AbortConversion});
            this.ctmTitle.Name = "ctmTitle";
            this.ctmTitle.Size = new System.Drawing.Size(275, 268);
            // 
            // ctmTitle_ToggleItem
            // 
            this.ctmTitle_ToggleItem.Name = "ctmTitle_ToggleItem";
            this.ctmTitle_ToggleItem.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_ToggleItem.Text = "Toggle Item";
            this.ctmTitle_ToggleItem.Click += new System.EventHandler(this.ctmTitle_ToggleItem_Click);
            // 
            // ctmTitle_ToggleItemSeparator
            // 
            this.ctmTitle_ToggleItemSeparator.Name = "ctmTitle_ToggleItemSeparator";
            this.ctmTitle_ToggleItemSeparator.Size = new System.Drawing.Size(271, 6);
            // 
            // ctmTitle_SelectAll
            // 
            this.ctmTitle_SelectAll.Name = "ctmTitle_SelectAll";
            this.ctmTitle_SelectAll.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_SelectAll.Text = "Select All Titles";
            this.ctmTitle_SelectAll.Click += new System.EventHandler(this.ctmTitle_SelectAll_Click);
            // 
            // ctmTitle_UnselectAll
            // 
            this.ctmTitle_UnselectAll.Name = "ctmTitle_UnselectAll";
            this.ctmTitle_UnselectAll.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_UnselectAll.Text = "Unselect All Titles";
            this.ctmTitle_UnselectAll.Click += new System.EventHandler(this.ctmTitle_UnselectAll_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(271, 6);
            // 
            // ctmTitle_ExpandSelected
            // 
            this.ctmTitle_ExpandSelected.Name = "ctmTitle_ExpandSelected";
            this.ctmTitle_ExpandSelected.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_ExpandSelected.Text = "Expand All Selected Titles";
            this.ctmTitle_ExpandSelected.Click += new System.EventHandler(this.ctmTitle_ExpandSelected_Click);
            // 
            // ctmTitle_ExpandAll
            // 
            this.ctmTitle_ExpandAll.Name = "ctmTitle_ExpandAll";
            this.ctmTitle_ExpandAll.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_ExpandAll.Text = "Expand All Titles";
            this.ctmTitle_ExpandAll.Click += new System.EventHandler(this.ctmTitle_ExpandAll_Click);
            // 
            // ctmTitle_CollapseAll
            // 
            this.ctmTitle_CollapseAll.Name = "ctmTitle_CollapseAll";
            this.ctmTitle_CollapseAll.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_CollapseAll.Text = "Collapse All Titles";
            this.ctmTitle_CollapseAll.Click += new System.EventHandler(this.ctmTitle_CollapseAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(271, 6);
            // 
            // ctmTitle_ApplyDefaultSelections
            // 
            this.ctmTitle_ApplyDefaultSelections.Enabled = false;
            this.ctmTitle_ApplyDefaultSelections.Name = "ctmTitle_ApplyDefaultSelections";
            this.ctmTitle_ApplyDefaultSelections.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_ApplyDefaultSelections.Text = "Apply Default Title Selections";
            this.ctmTitle_ApplyDefaultSelections.Click += new System.EventHandler(this.ctmTitle_UseTitleDefaults_Click);
            // 
            // ctmTitle_ApplyDefaultSettings
            // 
            this.ctmTitle_ApplyDefaultSettings.Name = "ctmTitle_ApplyDefaultSettings";
            this.ctmTitle_ApplyDefaultSettings.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_ApplyDefaultSettings.Text = "Apply Default Title Settings";
            this.ctmTitle_ApplyDefaultSettings.Click += new System.EventHandler(this.ctmTitle_ApplyDefaultSettings_Click);
            // 
            // ctmTitle_ConvertSeparator
            // 
            this.ctmTitle_ConvertSeparator.Name = "ctmTitle_ConvertSeparator";
            this.ctmTitle_ConvertSeparator.Size = new System.Drawing.Size(271, 6);
            // 
            // ctmTitle_Convert
            // 
            this.ctmTitle_Convert.Name = "ctmTitle_Convert";
            this.ctmTitle_Convert.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_Convert.Text = "Convert";
            this.ctmTitle_Convert.Click += new System.EventHandler(this.ctmTitle_Convert_Click);
            // 
            // ctmTitle_AbortConversion
            // 
            this.ctmTitle_AbortConversion.Name = "ctmTitle_AbortConversion";
            this.ctmTitle_AbortConversion.Size = new System.Drawing.Size(274, 24);
            this.ctmTitle_AbortConversion.Text = "Abort Conversion";
            this.ctmTitle_AbortConversion.Click += new System.EventHandler(this.ctmTitle_AbortConversion_Click);
            // 
            // ctmTrack
            // 
            this.ctmTrack.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctmTrack.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmTrack_ToggleItem,
            this.ctmTrack_ToggleItemSeparator,
            this.ctmTrack_SelectAll,
            this.ctmTrack_SelectAllAudio,
            this.ctmTrack_SelectAllSubtitle,
            this.ctmTrack_UnselectAll,
            this.ctmTrack_UnselectAllAudio,
            this.ctmTrack_UnselectAllSubtitle,
            this.toolStripSeparator5,
            this.ctmTrack_UseTrackDefaults});
            this.ctmTrack.Name = "ctmTrack";
            this.ctmTrack.Size = new System.Drawing.Size(256, 208);
            // 
            // ctmTrack_ToggleItem
            // 
            this.ctmTrack_ToggleItem.Name = "ctmTrack_ToggleItem";
            this.ctmTrack_ToggleItem.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_ToggleItem.Text = "Toggle Item";
            // 
            // ctmTrack_ToggleItemSeparator
            // 
            this.ctmTrack_ToggleItemSeparator.Name = "ctmTrack_ToggleItemSeparator";
            this.ctmTrack_ToggleItemSeparator.Size = new System.Drawing.Size(252, 6);
            // 
            // ctmTrack_SelectAll
            // 
            this.ctmTrack_SelectAll.Name = "ctmTrack_SelectAll";
            this.ctmTrack_SelectAll.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_SelectAll.Text = "Select All Tracks";
            this.ctmTrack_SelectAll.Click += new System.EventHandler(this.ctmTrack_SelectAll_Click);
            // 
            // ctmTrack_SelectAllAudio
            // 
            this.ctmTrack_SelectAllAudio.Name = "ctmTrack_SelectAllAudio";
            this.ctmTrack_SelectAllAudio.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_SelectAllAudio.Text = "Select All Audio Tracks";
            this.ctmTrack_SelectAllAudio.Click += new System.EventHandler(this.ctmTrack_SelectAllAudio_Click);
            // 
            // ctmTrack_SelectAllSubtitle
            // 
            this.ctmTrack_SelectAllSubtitle.Name = "ctmTrack_SelectAllSubtitle";
            this.ctmTrack_SelectAllSubtitle.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_SelectAllSubtitle.Text = "Select All Subtitle Tracks";
            this.ctmTrack_SelectAllSubtitle.Click += new System.EventHandler(this.ctmTrack_SelectAllSubtitle_Click);
            // 
            // ctmTrack_UnselectAll
            // 
            this.ctmTrack_UnselectAll.Name = "ctmTrack_UnselectAll";
            this.ctmTrack_UnselectAll.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_UnselectAll.Text = "Unselect All Tracks";
            this.ctmTrack_UnselectAll.Click += new System.EventHandler(this.ctmTrack_UnselectAll_Click);
            // 
            // ctmTrack_UnselectAllAudio
            // 
            this.ctmTrack_UnselectAllAudio.Name = "ctmTrack_UnselectAllAudio";
            this.ctmTrack_UnselectAllAudio.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_UnselectAllAudio.Text = "Unselect All Audio Tracks";
            this.ctmTrack_UnselectAllAudio.Click += new System.EventHandler(this.ctmTrack_UnselectAllAudio_Click);
            // 
            // ctmTrack_UnselectAllSubtitle
            // 
            this.ctmTrack_UnselectAllSubtitle.Name = "ctmTrack_UnselectAllSubtitle";
            this.ctmTrack_UnselectAllSubtitle.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_UnselectAllSubtitle.Text = "Unselect All Subtitle Tracks";
            this.ctmTrack_UnselectAllSubtitle.Click += new System.EventHandler(this.ctmTrack_UnselectAllSubtitle_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(252, 6);
            // 
            // ctmTrack_UseTrackDefaults
            // 
            this.ctmTrack_UseTrackDefaults.Name = "ctmTrack_UseTrackDefaults";
            this.ctmTrack_UseTrackDefaults.Size = new System.Drawing.Size(255, 24);
            this.ctmTrack_UseTrackDefaults.Text = "Apply Track Defaults";
            this.ctmTrack_UseTrackDefaults.Click += new System.EventHandler(this.ctmTrack_UseTrackDefaults_Click);
            // 
            // fMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "fMain";
            this.Text = "BatchMKV (beta)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fMain_FormClosed);
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvSources)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlvTitles)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpInformation.ResumeLayout(false);
            this.grpInformation.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.ctmSource.ResumeLayout(false);
            this.ctmTitle.ResumeLayout(false);
            this.ctmTrack.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtOutputSpecified;
        private System.Windows.Forms.RadioButton radOutputSpecified;
        private System.Windows.Forms.RadioButton radOutputSource;
        private System.Windows.Forms.Button btnBrowseOutput;
        private BrightIdeasSoftware.ObjectListView olvSources;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanFolderToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.ToolStripStatusLabel tsMessage;
        private BrightIdeasSoftware.OLVColumn olvTitle;
        private BrightIdeasSoftware.OLVColumn olvType;
        private BrightIdeasSoftware.OLVColumn olvSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BrightIdeasSoftware.OLVColumn olvStatus;
        private BrightIdeasSoftware.TreeListView tlvTitles;
        private BrightIdeasSoftware.OLVColumn otvType;
        private BrightIdeasSoftware.OLVColumn otvDescription;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbxSourceAction;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPropertyValue;
        private System.Windows.Forms.ComboBox cbxProperties;
        private System.Windows.Forms.GroupBox grpInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.FolderBrowserDialog dlgOutputPath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Label lblConversionStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripStatusLabel tsRemaining;
        private BrightIdeasSoftware.OLVColumn otvDefault;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemScanRecent;
        private BrightIdeasSoftware.OLVColumn otvOrder;
        private System.Windows.Forms.ImageList imlTracks;
        private System.Windows.Forms.ToolStripMenuItem scanDrivesToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar tsProgressTotal;
        private System.Windows.Forms.ContextMenuStrip ctmSource;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_ToggleItem;
        private System.Windows.Forms.ToolStripSeparator ctmSource_ToggleItemSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_SelectAll;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_UnselectAll;
        private System.Windows.Forms.ToolStripSeparator ctmSource_AnalyzeSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_Analyze;
        private System.Windows.Forms.ContextMenuStrip ctmTitle;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_ToggleItem;
        private System.Windows.Forms.ToolStripSeparator ctmTitle_ToggleItemSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_SelectAll;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_UnselectAll;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_ApplyDefaultSelections;
        private System.Windows.Forms.ToolStripSeparator ctmTitle_ConvertSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_Convert;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_AbortConversion;
        private System.Windows.Forms.ContextMenuStrip ctmTrack;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_ToggleItem;
        private System.Windows.Forms.ToolStripSeparator ctmTrack_ToggleItemSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_SelectAll;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_SelectAllAudio;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_SelectAllSubtitle;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_UnselectAll;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_UnselectAllAudio;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_UnselectAllSubtitle;
        private System.Windows.Forms.ToolStripMenuItem ctmTrack_UseTrackDefaults;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripStatusLabel tsSources;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_ApplyDefaultSettings;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_ExpandAll;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_CollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ctmTitle_ExpandSelected;
        private System.Windows.Forms.ToolStripMenuItem sourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analyzeSourcesAutomaticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertSelectedSourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearFinishedSourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearMissingSourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllSourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator ctmSource_PrioritySeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_Priority;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_PriorityLow;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_PriorityMedium;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_PriorityHigh;
        private BrightIdeasSoftware.OLVColumn olvPriority;
        private System.Windows.Forms.ImageList imlSources;
        private System.Windows.Forms.ToolStripSeparator ctmSource_ClearSeparator;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_Clear;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_ClearFinished;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_ClearMissing;
        private System.Windows.Forms.ToolStripMenuItem ctmSource_ClearAll;
        private System.Windows.Forms.ToolStripMenuItem troubleshootingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
    }
}

