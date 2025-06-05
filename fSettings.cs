using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace BatchMKV
{
    public partial class fSettings : Form
    {
        [DllImport("user32")]
        public static extern UInt32 SendMessage(IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);
        internal const int BCM_FIRST = 0x1600; //Normal button
        internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button

        static internal void AddShieldToButton(Button b)
        {
            b.FlatStyle = FlatStyle.System;
            SendMessage(b.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
        }

        public bool ShowTroubleshooting { get; set; } = false;

        public fSettings()
        {
            InitializeComponent();

            olvLossless.AspectToStringConverter = delegate(object x)
            {
                return ((bool)x ? "Yes" : "No");
            };

            olvTranscoding.ClearObjects();
            foreach (MKVTools.AudioCodec codec in Enum.GetValues(typeof(MKVTools.AudioCodec)))
            {
                if (codec == MKVTools.AudioCodec.Unknown) continue;
                olvTranscoding.AddObject(new audioTranscodeCodec(codec, true, MKVTools.AudioOutputFormat.DirectCopy));
            }
            olvTranscoding.Sort(olvCodec, SortOrder.Ascending);

            olvcLanguagesAll.AspectGetter = delegate (object x)
            {
                return ((languageInfo)x).language.GetDescription();
            };

            olvcLanguagesFavourites.AspectGetter = delegate (object x)
            {
                return ((languageInfo)x).language.GetDescription();
            };

            olvLanguagesAll.ClearObjects();

            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Albanian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Arabic));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Armenian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Basque));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Belarusian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Bosnian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Burmese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Catalan));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Chinese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Croatian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Czech));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Danish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Dutch));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.English));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Estonian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Faroese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Finnish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.French));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Georgian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.German));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Hawaiian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Hebrew));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Hindi));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Hungarian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Icelandic));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Indonesian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Irish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Italian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Japanese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Korean));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Kurdish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Latvian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Lithuanian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Macedonian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Maltese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Greek));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Norwegian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Polish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Portuguese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Romanian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Russian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Serbian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Slovenian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Spanish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Swedish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Thai));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Turkish));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Ukrainian));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Vietnamese));
            olvLanguagesAll.AddObject(new languageInfo(MKVTools.Language.Welsh));

            /*foreach (MKVTools.Language language in Enum.GetValues(typeof(MKVTools.Language)))
            {
                if (language == MKVTools.Language.Undetermined || 
                    language == MKVTools.Language.Nolinguisticcontent || 
                    language == MKVTools.Language.Uncodedlanguages || 
                    language == MKVTools.Language.Multiplelanguages) continue;
                olvLanguagesAll.AddObject(new languageInfo(language));
            }*/

            olvLanguagesAll.Sort(olvcLanguagesAll, SortOrder.Ascending);

            olvLanguagesFavourites.ClearObjects();

            AddShieldToButton(this.btnErrorLoggingEnable);
        }

        private class languageInfo
        {
            public languageInfo(MKVTools.Language language)
            {
                this.language = language;
                this.iso = MKVTools.MakeMKV.GetLanguageISOCode(language);
            }

            public MKVTools.Language language { get; set; }
            public string iso { get; internal set; }
        }

        private class audioTranscodeCodec
        {
            private MKVTools.AudioCodec _codec;
            public MKVTools.AudioCodec codec 
            {
                get { return _codec; }
                set
                {
                    _codec = value;
                    _lossless = MKVTools.MakeMKV.GetAudioCodecIsLossless(value);
                    _description = _codec.GetDescription();
                    if (_codec.ToString().Contains("Multi")) _description += " Surround";
                    if (_codec.ToString().Contains("Core")) _description += " (Core)";
                }
            }

            private string _description;
            public string description { get { return _description; } }

            private bool _lossless = false;
            public bool lossless { get { return _lossless; } }

            public bool outputDefault { get; set; }
            public MKVTools.AudioOutputFormat outputCustom { get; set; }

            public string output
            {
                get 
                {
                    return (outputDefault
                        ? "Use default"
                        : outputCustom.GetDescription());
                }
            }

            public audioTranscodeCodec(string entry)
            {
                if (!entry.Contains('='))
                    throw new Exception("Invalid audio transcoding setting.");
                string[] p = entry.Split('=');
                if (p.GetLength(0) != 2)
                    throw new Exception("Invalid audio transcoding setting.");

                this.codec = MKVTools.MakeMKV.GetAudioCodecByIdentifier(p[0]);
                this.outputDefault= false;
                this.outputCustom = fMain.getAudioOutputFormat(p[1]);
            }

            public audioTranscodeCodec(MKVTools.AudioCodec codec, bool outputDefault, MKVTools.AudioOutputFormat outputCustom)
            {
                this.codec = codec;
                this.outputDefault = outputDefault;
                this.outputCustom = outputCustom;
            }
        }

        private void btnBrowseMakeMKV_Click(object sender, EventArgs e)
        {
            dlgMakeMKVPath.SelectedPath = txtMakeMKVPath.Text;
            if (dlgMakeMKVPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtMakeMKVPath.Text = dlgMakeMKVPath.SelectedPath;
        }

        private void fSettings_Load(object sender, EventArgs e)
        {
            // General / Dependencies (MakeMKV and MKVToolNix paths)
            txtMakeMKVPath.Text = Properties.Settings.Default.MakeMKVPath;
            txtMKVToolNixPath.Text = Properties.Settings.Default.MKVToolNixPath;

            // General / Output
            string outputPath = Properties.Settings.Default.DefaultOutputFolder;
            if (outputPath.Length > 0)
            {
                radOutputSpecified.Checked = true;
                txtOutputSpecified.Text = outputPath;
            }
            else
            {
                radOutputSource.Checked = true;
                txtOutputSpecified.Text = "";
            }
            updateOutputSettingsUI();
            chkOutputOverwrite.Checked = Properties.Settings.Default.OutputOverwrite;

            // General / Source files
            switch(Properties.Settings.Default.DefaultSourceAction)
            {
                case "archive":
                    cbxSourceAction.SelectedIndex = 1; break;
                case "delete":
                    cbxSourceAction.SelectedIndex = 2; break;
                default:
                    cbxSourceAction.SelectedIndex = 0; break;
            }

            txtArchiveFolder.Text = Properties.Settings.Default.ArchiveFolder;

            // General / Scan on start
            chkScanOnStart.Checked = Properties.Settings.Default.ScanOnStart;
            txtScanOnStart.Text = Properties.Settings.Default.ScanOnStartPath;

            // Transcoding / Subtitles
            chkZLibCompression.Checked = Properties.Settings.Default.SubtitleCompression;

            // Transcoding / Audio streams
            cbxLPCMContainer.SelectedIndex = (Properties.Settings.Default.AudioLPCMContainer == "wavex" ? 1 : 0);
            cbxFLACCompression.SelectedIndex = (Properties.Settings.Default.AudioFLACCompression == "fast" ? 0 : (Properties.Settings.Default.AudioFLACCompression == "best" ? 2 : 1));
            cbxTranscodingDefault.SelectedIndex = (Properties.Settings.Default.AudioTranscodeDefault == "flac" ? 1 : (Properties.Settings.Default.AudioTranscodeDefault == "lpcm" ? 2 : 0));

            string audioTranscode = Properties.Settings.Default.AudioTranscodeCodecs;
            if (audioTranscode.Length > 0)
            {
                string[] entrySetting;
                MKVTools.AudioCodec codec;
                foreach (string entry in audioTranscode.Split(';'))
                {
                    if (!entry.Contains('=')) continue;
                    entrySetting = entry.Split('=');
                    if (entrySetting.GetLength(0) != 2) continue;
                    codec = MKVTools.MakeMKV.GetAudioCodecByIdentifier(entrySetting[0]);
                    foreach(audioTranscodeCodec c in olvTranscoding.Objects)
                    {
                        if (c.codec == codec)
                        {
                            c.outputDefault = false;
                            c.outputCustom = fMain.getAudioOutputFormat(entrySetting[1]);
                            olvTranscoding.RefreshObject(c);
                            break;
                        }
                    }
                }
            }
            cbxTranscoding.Enabled = (olvTranscoding.SelectedItems.Count > 0);
            btnTranscodingReset.Enabled = !transcodingIsDefault();

            // Streams / Languages
            olvLanguagesFavourites.ClearObjects();
            if (Properties.Settings.Default.FavouriteLanguages.Length > 0)
            {
                foreach(string language in Properties.Settings.Default.FavouriteLanguages.Split('|'))
                {
                    foreach(languageInfo lng in olvLanguagesAll.Objects)
                    {
                        if (lng.iso.Equals(language))
                            olvLanguagesFavourites.AddObject(lng);
                    }
                }
            }
            updateLanguagesUI();

            // Streams / Audio defaults
            radAudioIncludeAll.Checked = Properties.Settings.Default.DefaultAudioIncludeAll;
            radAudioIncludeFavourites.Checked = !Properties.Settings.Default.DefaultAudioIncludeAll;
            chkAudioIncludeQuality.Checked = Properties.Settings.Default.DefaultAudioIncludeQuailty;
            chkAudioIncludeNonFavourite.Checked = Properties.Settings.Default.DefaultAudioIncludeNonFavourite;
            chkAudioIncludeFirst.Checked = Properties.Settings.Default.DefaultAudioIncludeFirst;
            chkAudioIncludeCommentaryTracks.Checked = Properties.Settings.Default.DefaultAudioIncludeCommentaryTracks;
            txtAudioCommentaryTrackName.Text = (string.IsNullOrWhiteSpace(Properties.Settings.Default.DefaultAudioCommentaryTrackName) ? "Commentary" : Properties.Settings.Default.DefaultAudioCommentaryTrackName);
            updateIncludeAudioUI();
            chkAudioLimit.Checked = (Properties.Settings.Default.DefaultAudioLimit > 0);
            updAudioLimit.Enabled = chkAudioLimit.Checked;
            updAudioLimit.Value = (Properties.Settings.Default.DefaultAudioLimit > 0 ? Properties.Settings.Default.DefaultAudioLimit : 3);
            cbxAudioMainCore.SelectedIndex = Properties.Settings.Default.DefaultAudioMainCore;
            cbxAudioOrder.SelectedIndex = Properties.Settings.Default.DefaultAudioOrder;

            // Streams / Subtitle defaults
            radSubtitleIncludeAll.Checked = Properties.Settings.Default.DefaultSubtitleIncludeAll;
            radSubtitleIncludeFavourites.Checked = !Properties.Settings.Default.DefaultSubtitleIncludeAll;
            chkSubtitleLimit.Checked = (Properties.Settings.Default.DefaultSubtitleLimit > 0);
            updSubtitleLimit.Enabled = chkSubtitleLimit.Checked;
            updSubtitleLimit.Value = (Properties.Settings.Default.DefaultSubtitleLimit > 0 ? Properties.Settings.Default.DefaultSubtitleLimit : 3);
            cbxSubtitleOrder.SelectedIndex = Properties.Settings.Default.DefaultSubtitleOrder;
            cbxSubtitleDefault.SelectedIndex = Properties.Settings.Default.DefaultSubtitleTrack;

            // Titles
            updIgnoreTitlesLength.Value = (Properties.Settings.Default.TitlesIgnoreLength > 0 ? Properties.Settings.Default.TitlesIgnoreLength : 300);
            chkIgnoreTitlesLength.Checked = (Properties.Settings.Default.TitlesIgnoreLength > 0);
            updateTitleLengthUI();

            radTitlesIncludeAll.Checked = Properties.Settings.Default.DefaultTitlesIncludeAll;
            radTitlesIncludeLongest.Checked = !Properties.Settings.Default.DefaultTitlesIncludeAll;
            chkTitlesIncludeWithin.Checked = (Properties.Settings.Default.DefaultTitlesIncludeWithinLength > 0);
            updTitlesIncludeWithin.Value = (Properties.Settings.Default.DefaultTitlesIncludeWithinLength > 0 ? Properties.Settings.Default.DefaultTitlesIncludeWithinLength : 300);
            chkTitlesIgnoreMuted.Checked = Properties.Settings.Default.DefaultTitlesIgnoreMuted;

            txtTitleName.Text = (Properties.Settings.Default.DefaultTitleName.Length > 0 ? Properties.Settings.Default.DefaultTitleName : "{name-default}");
            txtTitleFilename.Text = "{filename-default}.mkv";
            if (Properties.Settings.Default.DefaultTitleFilename.Length > 0)
            {
                radTitlesFilenameCustom.Checked = true;
                txtTitleFilename.Text = Properties.Settings.Default.DefaultTitleFilename;
            }
            else
                radTitlesFilenameBasedOnName.Checked = true;
            updateTitleSettingUI();

            // Restore sources
            chkRestoreSourcesOnRestart.Checked = Properties.Settings.Default.RestoreSources;
            chkClearFinishedSourcesOnExit.Checked = Properties.Settings.Default.ClearFinishedSourcesOnExit;
            chkClearMissingSourcesOnStart.Checked = Properties.Settings.Default.ClearMissingSourcesOnStart;
            chkPurgeSourceState.Checked = (Properties.Settings.Default.PurgeSourceState > 0);
            cbxPurgeSourceState.SelectedIndex = (chkPurgeSourceState.Checked ? Properties.Settings.Default.PurgeSourceState - 1 : 3);
            updateSourceStateSettingUI();

            // Troubleshooting (error logging)
            if (Properties.Settings.Default.ErrorLogging)
            {
                lblErrorLoggingStatus.Text = "Error logging is enabled.";
                btnErrorLoggingEnable.Enabled = false;
                btnErrorLoggingDisable.Enabled = true;
            }
            else
            {
                lblErrorLoggingStatus.Text = "Error logging is disabled.";
                btnErrorLoggingEnable.Enabled = true;
                btnErrorLoggingDisable.Enabled = false;
            }

            tabControl1.SelectedTab = (ShowTroubleshooting ? tabTroubleshooting : tabGeneral);
        }

        private void fSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!settingsValid) e.Cancel = true;
        }

        private void radOutputSpecified_CheckedChanged(object sender, EventArgs e)
        {
            updateOutputSettingsUI();
        }

        private void updateOutputSettingsUI()
        {
            txtOutputSpecified.Enabled = !radOutputSource.Checked;
            btnBrowseOutput.Enabled = !radOutputSource.Checked;
        }

        private readonly string[] lpcmContainer = new string[2] { "raw", "wavex" };
        private readonly string[] flacCompression = new string[3] { "fast", "good", "best" };
        private readonly string[] audioTranscode = new string[3] { "copy", "flac", "lpcm" };

        private bool settingsValid = false;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!makeMKVPathIsValid())
            {
                MessageBox.Show("The specified path to the MakeMKV executable is not valid.", "MakeMKV path not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (!mkvToolNixPathIsValid())
            {
                MessageBox.Show("The specified path to the MKVToolNix executables is not valid.", "MKVToolNix path not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (!outputFolderIsValid())
            {
                MessageBox.Show("The specified output path is not valid.", "Output path not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (cbxSourceAction.SelectedItem.Equals("Move to archive folder") && !archiveFolderIsValid())
            {
                MessageBox.Show("The specified archive path is not valid.", "Archive path not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (!scanOnStartFolderIsValid())
            {
                MessageBox.Show("The specified path to scan on start is not valid.", "Scan path not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (!titleNameIsValid())
            {
                MessageBox.Show("The specified title name is not valid.", "Title name not valid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            if (!titleFilenameIsValid())
            {
                MessageBox.Show("The specified title filename is not valid.", "Title filename not valid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                settingsValid = false;
                return;
            }

            settingsValid = true;

            // General / Dependencies
            Properties.Settings.Default.MakeMKVPath = txtMakeMKVPath.Text;
            Properties.Settings.Default.MKVToolNixPath = txtMKVToolNixPath.Text;

            // General / Output
            Properties.Settings.Default.DefaultOutputFolder = radOutputSource.Checked
                ? ""
                : txtOutputSpecified.Text;
            Properties.Settings.Default.OutputOverwrite = chkOutputOverwrite.Checked;

            // General / Source files
            switch(cbxSourceAction.SelectedIndex)
            {
                case 0:
                    Properties.Settings.Default.DefaultSourceAction = "keep"; break;
                case 1:
                    Properties.Settings.Default.DefaultSourceAction = "archive"; break;
                case 2:
                    Properties.Settings.Default.DefaultSourceAction = "delete"; break;
            }

            Properties.Settings.Default.ArchiveFolder = txtArchiveFolder.Text;

            // General / Scan on start
            Properties.Settings.Default.ScanOnStart = chkScanOnStart.Checked;
            Properties.Settings.Default.ScanOnStartPath = txtScanOnStart.Text.Trim();

            // Transcoding / Subtitles
            Properties.Settings.Default.SubtitleCompression = chkZLibCompression.Checked;

            // Transcoding / Audio streams
            Properties.Settings.Default.AudioLPCMContainer = lpcmContainer[cbxLPCMContainer.SelectedIndex];
            Properties.Settings.Default.AudioFLACCompression = flacCompression[cbxFLACCompression.SelectedIndex];
            Properties.Settings.Default.AudioTranscodeDefault = audioTranscode[cbxTranscodingDefault.SelectedIndex];

            string transcodeCodecs = "";
            foreach(audioTranscodeCodec c in olvTranscoding.Objects)
            {
                if (!c.outputDefault)
                {
                    if (transcodeCodecs.Length > 0) transcodeCodecs += ";";
                    transcodeCodecs += String.Format("{0}={1}", MKVTools.MakeMKV.GetAudioCodecIdentifier(c.codec), 
                        (c.outputCustom == MKVTools.AudioOutputFormat.LPCM ? "lpcm" :
                        (c.outputCustom == MKVTools.AudioOutputFormat.FLAC ? "flac" : "copy"))
                        );
                }
            }
            Properties.Settings.Default.AudioTranscodeCodecs = transcodeCodecs;

            // Streams / Languages
            string languages = "";
            foreach(languageInfo lng in olvLanguagesFavourites.Objects)
            {
                if (languages.Length > 0) languages += "|";
                languages += lng.iso;
            }
            Properties.Settings.Default.FavouriteLanguages = languages;

            // Streams / Audio defaults
            Properties.Settings.Default.DefaultAudioIncludeAll = radAudioIncludeAll.Checked;
            Properties.Settings.Default.DefaultAudioIncludeQuailty = chkAudioIncludeQuality.Checked;
            Properties.Settings.Default.DefaultAudioIncludeNonFavourite = chkAudioIncludeNonFavourite.Checked;
            Properties.Settings.Default.DefaultAudioIncludeFirst = chkAudioIncludeFirst.Checked;
            Properties.Settings.Default.DefaultAudioLimit = chkAudioLimit.Checked ? (byte)updAudioLimit.Value : (byte)0;
            Properties.Settings.Default.DefaultAudioMainCore = (byte)cbxAudioMainCore.SelectedIndex;
            Properties.Settings.Default.DefaultAudioOrder = (byte)cbxAudioOrder.SelectedIndex;
            Properties.Settings.Default.DefaultAudioIncludeCommentaryTracks = chkAudioIncludeCommentaryTracks.Checked;
            Properties.Settings.Default.DefaultAudioCommentaryTrackName = txtAudioCommentaryTrackName.Text.Trim();

            // Streams / Subtitle defaults
            Properties.Settings.Default.DefaultSubtitleIncludeAll = radSubtitleIncludeAll.Checked;
            Properties.Settings.Default.DefaultSubtitleLimit = chkSubtitleLimit.Checked ? (byte)updSubtitleLimit.Value : (byte)0;
            Properties.Settings.Default.DefaultSubtitleOrder = (byte)cbxSubtitleOrder.SelectedIndex;
            Properties.Settings.Default.DefaultSubtitleTrack = (byte)cbxSubtitleDefault.SelectedIndex;

            // Titles
            Properties.Settings.Default.TitlesIgnoreLength = (chkIgnoreTitlesLength.Checked ? (ushort)updIgnoreTitlesLength.Value : (ushort)0);
            Properties.Settings.Default.DefaultTitlesIgnoreMuted = chkTitlesIgnoreMuted.Checked;
            Properties.Settings.Default.DefaultTitlesIncludeAll = radTitlesIncludeAll.Checked;
            Properties.Settings.Default.DefaultTitlesIncludeWithinLength = (chkTitlesIncludeWithin.Checked ? (ushort)updTitlesIncludeWithin.Value : (ushort)0);
            Properties.Settings.Default.DefaultTitleName = txtTitleName.Text.Trim();
            Properties.Settings.Default.DefaultTitleFilename = (radTitlesFilenameBasedOnName.Checked
                ? ""
                : (radTitlesFilenameCustom.Checked
                ? txtTitleFilename.Text.Trim()
                : "{filename-default}"));

            // Restore sources
            Properties.Settings.Default.RestoreSources = chkRestoreSourcesOnRestart.Checked;
            Properties.Settings.Default.ClearFinishedSourcesOnExit = chkClearFinishedSourcesOnExit.Checked;
            Properties.Settings.Default.ClearMissingSourcesOnStart = chkClearMissingSourcesOnStart.Checked;
            Properties.Settings.Default.PurgeSourceState = (byte)(chkPurgeSourceState.Checked ? cbxPurgeSourceState.SelectedIndex + 1 : 0);

            // Error logging
            Properties.Settings.Default.ErrorLogging = !btnErrorLoggingEnable.Enabled;

            // Save settings
            Properties.Settings.Default.Save();
        }

        private bool makeMKVPathIsValid()
        {
            try { return (Directory.Exists(txtMakeMKVPath.Text)); }
            catch { return false; }
        }

        private bool mkvToolNixPathIsValid()
        {
            try { return (Directory.Exists(txtMKVToolNixPath.Text)); }
            catch { return false; }
        }

        private bool archiveFolderIsValid()
        {
            try { return (txtArchiveFolder.Text.Trim().Length > 0 && Directory.Exists(txtArchiveFolder.Text)); }
            catch { return false; }
        }

        private bool outputFolderIsValid()
        {
            return (radOutputSource.Checked 
                ? true 
                : txtOutputSpecified.Text.Trim().Length > 0);
        }

        private bool scanOnStartFolderIsValid()
        {
            if (!chkScanOnStart.Checked) return true;

            try { return (Directory.Exists(txtScanOnStart.Text)); }
            catch { return false; }
        }

        private bool titleNameIsValid()
        {
            return (txtTitleName.Text.Trim().Length > 0);
        }

        private bool titleFilenameIsValid()
        {
            if (radTitlesFilenameBasedOnName.Checked) return true;

            string filename = txtTitleFilename.Text.Trim().ToLower();
            return (filename.Length > 4 && filename.EndsWith(".mkv"));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            settingsValid = true;
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            dlgOutputPath.SelectedPath = txtOutputSpecified.Text;
            if (dlgOutputPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtOutputSpecified.Text = dlgOutputPath.SelectedPath;
        }

        private void btnBrowseArchive_Click(object sender, EventArgs e)
        {
            dlgArchivePath.SelectedPath = txtArchiveFolder.Text;
            if (dlgArchivePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtArchiveFolder.Text = dlgArchivePath.SelectedPath;
        }

        private void btnBrowseScanOnStart_Click(object sender, EventArgs e)
        {
            dlgScanOnStart.SelectedPath = txtScanOnStart.Text;
            if (dlgScanOnStart.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtScanOnStart.Text = dlgScanOnStart.SelectedPath;
        }

        private void chkScanOnStart_CheckedChanged(object sender, EventArgs e)
        {
            txtScanOnStart.Enabled = chkScanOnStart.Checked;
            btnBrowseScanOnStart.Enabled = chkScanOnStart.Checked;
        }

        private bool transcodingIsDefault()
        {
            if (cbxTranscodingDefault.SelectedIndex > 0) return false;
            if (olvTranscoding.Objects != null)
            {
                foreach (audioTranscodeCodec codec in olvTranscoding.Objects)
                    if (!codec.outputDefault) 
                        return false;
            }
            return true;
        }

        private void btnTranscodingReset_Click(object sender, EventArgs e)
        {
            foreach (audioTranscodeCodec codec in olvTranscoding.Objects)
            {
                codec.outputDefault = true;
                olvTranscoding.RefreshObject(codec);
            }
            cbxTranscodingDefault.SelectedIndex = 0;

            olvTranscoding.Refresh();
            btnTranscodingReset.Enabled = false;
        }

        private void cbxTranscodingDefault_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTranscodingReset.Enabled = !transcodingIsDefault();
        }

        private void olvTranscoding_SelectionChanged(object sender, EventArgs e)
        {
            bool outputDefault = true, isIdentical = true;
            MKVTools.AudioOutputFormat outputCustom = MKVTools.AudioOutputFormat.DirectCopy;
            int n = 0;

            if (olvTranscoding.SelectedObjects != null)
            {
                foreach (audioTranscodeCodec codec in olvTranscoding.SelectedObjects)
                {
                    if (n == 0)
                    { outputDefault = codec.outputDefault; outputCustom = codec.outputCustom; }
                    else
                    {
                        if (codec.outputDefault != outputDefault || (!outputDefault && codec.outputCustom != outputCustom))
                        { isIdentical = false; break; }
                    }
                    n++;
                }
            }

            if (n > 0)
            {
                if (isIdentical)
                {
                    cbxTranscoding.SelectedIndex = (outputDefault 
                        ? 0 
                        : 1 + (int)outputCustom);
                }
                else
                    cbxTranscoding.SelectedIndex = -1;
                cbxTranscoding.Enabled = true;
            }
            else
            {
                cbxTranscoding.SelectedIndex = -1;
                cbxTranscoding.Enabled = false;
            }
        }

        private void cbxTranscoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTranscoding.SelectedIndex < 0) return;

            bool outputDefault = (cbxTranscoding.SelectedIndex == 0);
            MKVTools.AudioOutputFormat outputCustom = (cbxTranscoding.SelectedIndex == 0 
                ? MKVTools.AudioOutputFormat.DirectCopy
                : (MKVTools.AudioOutputFormat)(cbxTranscoding.SelectedIndex - 1));

            if (olvTranscoding.SelectedObjects != null)
            {
                foreach (audioTranscodeCodec codec in olvTranscoding.SelectedObjects)
                {
                    codec.outputDefault = outputDefault;
                    codec.outputCustom = outputCustom;
                }
                olvTranscoding.RefreshSelectedObjects();
            }

            btnTranscodingReset.Enabled = !transcodingIsDefault();
        }

        private void btnBrowseMKVToolNix_Click(object sender, EventArgs e)
        {
            dlgMKVToolNixPath.SelectedPath = txtMKVToolNixPath.Text;
            if (dlgMKVToolNixPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtMKVToolNixPath.Text = dlgMKVToolNixPath.SelectedPath;
        }

        private void chkSubtitleLimit_CheckedChanged(object sender, EventArgs e)
        {
            updSubtitleLimit.Enabled = chkSubtitleLimit.Checked;
        }

        private void chkAudioLimit_CheckedChanged(object sender, EventArgs e)
        {
            updAudioLimit.Enabled = chkAudioLimit.Checked;
        }

        private void radAudioIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            updateIncludeAudioUI();
        }

        private void radAudioIncludeFavourites_CheckedChanged(object sender, EventArgs e)
        {
            updateIncludeAudioUI();
        }

        private void updateIncludeAudioUI()
        {
            chkAudioIncludeQuality.Enabled = radAudioIncludeFavourites.Checked;
            chkAudioIncludeNonFavourite.Enabled = radAudioIncludeFavourites.Checked;
            chkAudioIncludeFirst.Enabled = radAudioIncludeFavourites.Checked;
            chkAudioIncludeCommentaryTracks.Enabled = radAudioIncludeFavourites.Checked;
            txtAudioCommentaryTrackName.Enabled = (radAudioIncludeFavourites.Checked && chkAudioIncludeCommentaryTracks.Checked);
        }

        private void olvLanguagesFavourites_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLanguagesUI();
        }

        private void olvLanguagesAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLanguagesUI();
        }

        private void updateLanguagesUI()
        {
            btnLanguageAdd.Enabled = (olvLanguagesAll.SelectedItems.Count > 0);
            btnLanguageRemove.Enabled = (olvLanguagesFavourites.SelectedItems.Count > 0);

            if (olvLanguagesFavourites.SelectedItems.Count > 0)
            {
                btnLanguageUp.Enabled = (olvLanguagesFavourites.SelectedIndices[0] > 0);
                btnLanguageDown.Enabled = (olvLanguagesFavourites.SelectedIndices[olvLanguagesFavourites.SelectedItems.Count - 1] < olvLanguagesFavourites.Items.Count - 1);
            }
            else
            {
                btnLanguageDown.Enabled = false;
                btnLanguageUp.Enabled = false;
            }

        }

        private void btnLanguageAdd_Click(object sender, EventArgs e)
        {
            foreach(languageInfo lng in olvLanguagesAll.SelectedObjects)
                olvLanguagesFavourites.UpdateObject(lng);

            olvLanguagesAll.SelectedIndex = -1;
        }

        private void btnLanguageRemove_Click(object sender, EventArgs e)
        {
            olvLanguagesFavourites.RemoveObjects(olvLanguagesFavourites.SelectedObjects);
        }

        private void btnLanguageDown_Click(object sender, EventArgs e)
        {
            List<object> selectedObjects = new List<object>();
            List<int> selectedIndicesAfter = new List<int>();
            for (int i = 0; i < olvLanguagesFavourites.SelectedObjects.Count; i++)
            {
                selectedIndicesAfter.Add(olvLanguagesFavourites.SelectedIndices[i] + 2);
                selectedObjects.Add(olvLanguagesFavourites.SelectedObjects[i]);
            }

            for (int i = selectedIndicesAfter.Count - 1; i >= 0; i--)
                olvLanguagesFavourites.MoveObjects(selectedIndicesAfter[i], new object[] { selectedObjects[i] });
            olvLanguagesFavourites.SelectedObjects = selectedObjects;
        }

        private void btnLanguageUp_Click(object sender, EventArgs e)
        {
            List<object> selectedObjects = new List<object>();
            List<int> selectedIndicesAfter = new List<int>();
            for (int i = 0; i < olvLanguagesFavourites.SelectedObjects.Count; i++)
            {
                selectedIndicesAfter.Add(olvLanguagesFavourites.SelectedIndices[i] - 1);
                selectedObjects.Add(olvLanguagesFavourites.SelectedObjects[i]);
            }

            for (int i = 0; i < selectedIndicesAfter.Count; i++)
                olvLanguagesFavourites.MoveObjects(selectedIndicesAfter[i], new object[] { selectedObjects[i] });
            olvLanguagesFavourites.SelectedObjects = selectedObjects;
        }

        private void chkIgnoreTitlesLength_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleLengthUI();
        }

        private void updateTitleLengthUI()
        {
            updIgnoreTitlesLength.Enabled = chkIgnoreTitlesLength.Checked;
        }

        private void chkTitlesIncludeWithin_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSelectionUI();
        }

        private void radTitlesIncludeLongest_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSelectionUI();
        }

        private void radTitlesIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSelectionUI();
        }

        private void updateTitleSelectionUI()
        {
            chkTitlesIncludeWithin.Enabled = (radTitlesIncludeLongest.Checked);
            updTitlesIncludeWithin.Enabled = (radTitlesIncludeLongest.Checked && chkTitlesIncludeWithin.Checked);
        }

        private void radTitlesFilenameCustom_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSettingUI();
        }

        private void radTitlesFilenameBasedOnName_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSettingUI();
        }

        private void radTitlesFilenameDefault_CheckedChanged(object sender, EventArgs e)
        {
            updateTitleSettingUI();
        }

        private void updateTitleSettingUI()
        {
            txtTitleFilename.Enabled = radTitlesFilenameCustom.Checked;
        }

        private void updateSourceStateSettingUI()
        {
            chkClearFinishedSourcesOnExit.Enabled = chkRestoreSourcesOnRestart.Checked;
            chkClearMissingSourcesOnStart.Enabled = chkRestoreSourcesOnRestart.Checked;
            chkPurgeSourceState.Enabled = chkRestoreSourcesOnRestart.Checked;
            cbxPurgeSourceState.Enabled = (chkRestoreSourcesOnRestart.Checked && chkPurgeSourceState.Checked);
        }

        private void lnkTitleNameTags_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showTemplateGuide();
        }

        private void lnkTitleFilenameTags_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showTemplateGuide();
        }

        private void lnkOutputFolderTags_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showTemplateGuide();
        }

        private void showTemplateGuide()
        {
            System.Diagnostics.Process.Start("http://www.adeptweb.dk/BatchMKV/docs/TemplateGuide_v1.0.1.pdf");
        }

        private void chkRestoreSourcesOnRestart_CheckedChanged(object sender, EventArgs e)
        {
            updateSourceStateSettingUI();
        }

        private void chkPurgeSourceState_CheckedChanged(object sender, EventArgs e)
        {
            updateSourceStateSettingUI();
        }

        private void chkAudioIncludeCommentaryTracks_CheckedChanged(object sender, EventArgs e)
        {
            updateIncludeAudioUI();
        }

        private void lnkForumThread_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.makemkv.com/forum2/viewtopic.php?f=10&t=9345");
        }

        private void btnErrorLoggingDisable_Click(object sender, EventArgs e)
        {
            lblErrorLoggingStatus.Text = "Error logging is disabled.";
            btnErrorLoggingEnable.Enabled = true;
            btnErrorLoggingDisable.Enabled = false;
        }

        private void btnErrorLoggingEnable_Click(object sender, EventArgs e)
        {
            // Create event sources.

            var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

            // The following properties run the new process as administrator
            processInfo.UseShellExecute = true;
            processInfo.Verb = "runas";
            processInfo.Arguments = "/createeventsources";

            // Start the new process
            try
            {
                Process process = Process.Start(processInfo);
                process.WaitForExit();
                if (!process.HasExited || process.ExitCode != 1)
                {
                    MessageBox.Show("Could not create logs for error logging.\r\n\r\nError logging could not be enabled.");
                    return;
                }
            }
            catch (Exception)
            {
                // The user did not allow the application to run as administrator
                MessageBox.Show("You must allow administrator rights for BatchMKV to create the logs necessary to enable error logging.\r\n\r\nError logging could not be enabled.");
                return;
            }

            lblErrorLoggingStatus.Text = "Error logging is enabled.";
            btnErrorLoggingEnable.Enabled = false;
            btnErrorLoggingDisable.Enabled = true;

        }
    }
}
