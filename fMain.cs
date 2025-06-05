using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using BrightIdeasSoftware;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using BatchMKV.Domain;
using EventLogger;

namespace BatchMKV
{
    public partial class fMain : Form
    {
        private MKVTools.LPCMContainer getLPCMContainer(string identifier)
        {
            switch(identifier)
            {
                case "wavex":
                    return MKVTools.LPCMContainer.Wavex;
                default:
                    return MKVTools.LPCMContainer.Raw;
            }
        }

        private MKVTools.FLACCompression getFLACCompression(string identifier)
        {
            switch(identifier)
            {
                case "fast":
                    return MKVTools.FLACCompression.Fast;
                case "best":
                    return MKVTools.FLACCompression.Best;
                default:
                    return MKVTools.FLACCompression.Good;
            }
        }

        public static MKVTools.AudioOutputFormat getAudioOutputFormat(string identifier)
        {
            switch(identifier)
            {
                case "flac":
                    return MKVTools.AudioOutputFormat.FLAC;
                case "lpcm":
                    return MKVTools.AudioOutputFormat.LPCM;
                default:
                    return MKVTools.AudioOutputFormat.DirectCopy;
            }
        }

        private object activityLock = new object();
        private ActivityType processInProgress, scanInProgress;
        private MKVTools.Source analyzeInProgressSource = null;
        private MKVTools.Title conversionInProgressTitle = null;

        /*private object progressLock = new object();
        private ulong analysisDone, analysisCurrent, analysisRemaining, conversionDone, conversionCurrent, conversionRemaining;
        // Done + Remaining = Total
        // Done + Current = Actual (current)
        private readonly ulong ConversionProgressFactor = 20;

        delegate void UpdateTotalProgressCallback();
        private void UpdateTotalProgress()
        {
            if (this.ssStatus.InvokeRequired)
            {
                UpdateTotalProgressCallback d = new UpdateTotalProgressCallback(UpdateTotalProgress);
                this.Invoke(d);
            }
            else
            {
                lock (progressLock)
                {
                    double total = (double)(analysisDone + analysisRemaining + (conversionDone + conversionRemaining) * ConversionProgressFactor);
                    if (total > 0)
                    {
                        double progress = (double)(analysisDone + analysisCurrent + (conversionDone + conversionCurrent) * ConversionProgressFactor) / total;
                        int prg = (int)Math.Round(progress * 100);
                        if (prg < 100)
                        {
                            if (prg != tsProgressTotal.Value)
                            {
                                tsProgressTotal.Value = (progress < 0
                                    ? 0
                                    : prg);
                                tsProgressTotal.Visible = true;
                            }
                        }
                        else if (tsProgressTotal.Visible) 
                            tsProgressTotal.Visible = false;
                    }
                    else if (tsProgressTotal.Visible) 
                        tsProgressTotal.Visible = false;
                }
            }
        }

        delegate void updateProgressTotalRemainingCallback();
        private void updateProgressTotalRemaining()
        {
            if (olvSources.InvokeRequired)
            {
                updateProgressTotalRemainingCallback d = new updateProgressTotalRemainingCallback(updateProgressTotalRemaining);
                this.Invoke(d);
            }
            else
            {
                ulong ra = 0, rc = 0;

                Debug.WriteLine("UPDATE REMAINING");

                foreach (ScanFolder.MediaSource source in olvSources.Objects)
                {
                    if (source.CurrentStatus == ScanFolder.MediaSource.Status.Initialized)
                        ra += source.Size;
                }

                foreach (ScanFolder.MediaSource source in olvSources.CheckedObjects)
                {
                    if (!sources.ContainsKey(source)) continue;
                    foreach (MKVTools.Title title in sources[source].Titles)
                    {
                        if (title.Include && title.Result == MKVTools.Title.ConversionResult.NotAvailable)
                            rc += (ulong)title.Size;
                    }
                }

                lock (progressLock)
                {
                    analysisRemaining = ra;
                    if (ra == 0) analysisDone = 0;

                    conversionRemaining = rc;
                    if (rc == 0) conversionDone = 0;
                }

                UpdateTotalProgress();
            }
        }

        private void updateProgressTotalDone(MKVTools.Source Source)
        {
            updateProgressTotalCurrent(Source, 1);
            updateProgressTotalDone(true, false);
        }

        private void updateProgressTotalDone(MKVTools.Title Title)
        {
            updateProgressTotalCurrent(Title, 1);
            updateProgressTotalDone(false, true);
        }

        private void updateProgressTotalDone(bool Analysis = false, bool Conversion = false)
        {
            if (Analysis || Conversion)
            {
                lock (progressLock)
                {
                    if (Analysis)
                    {
                        analysisDone += analysisCurrent;
                        analysisCurrent = 0;
                    }

                    if (Conversion)
                    {
                        conversionDone += conversionCurrent;
                        conversionCurrent = 0;
                    }
                }

                updateProgressTotalRemaining();
                UpdateTotalProgress();
            }
        }

        private void updateProgressTotalCurrent(MKVTools.Source Source, double Progress)
        {
            ScanFolder.MediaSource src = (ScanFolder.MediaSource)Source.Tag;
            updateProgressTotalCurrent((long)Math.Round(src.Size * Progress), -1);
        }

        private void updateProgressTotalCurrent(MKVTools.Title Title, double Progress)
        {
            updateProgressTotalCurrent(-1, (long)Math.Round(Title.Size * Progress));
        }

        private void updateProgressTotalCurrent(long Analysis = -1, long Conversion = -1)
        {
            if (Analysis >= 0 || Conversion >= 0)
            {
                lock (progressLock)
                {
                    if (Analysis >= 0)
                        analysisCurrent = (ulong)Analysis;

                    if (Conversion >= 0)
                        conversionCurrent = (ulong)Conversion;
                }

                UpdateTotalProgress();
            }
        }*/

        private Queue<string> scanQueue = new Queue<string>();
        private System.Windows.Forms.Timer scanDriveTimer;

        delegate void SetScanInProgressCallback(ActivityType Activity);
        private void SetScanInProgress(ActivityType Activity)
        {
            if (Activity == ActivityType.AnalyzeSource || Activity == ActivityType.ConvertTitle || Activity == ActivityType.FileOperation) return;

            if (this.InvokeRequired)
            {
                SetScanInProgressCallback d = new SetScanInProgressCallback(SetScanInProgress);
                this.Invoke(d, new object[] { Activity });
            }
            else
            {
                scanInProgress = Activity;

                /*switch(Activity)
                {
                    case ActivityType.None:
                        SetScanFolderEnabled(true);
                        break;
                    default:
                        SetScanFolderEnabled(false);
                        break;
                }*/
            }
        }

        delegate void SetProcessInProgressCallback(ActivityType Activity);
        private void SetProcessInProgress(ActivityType Activity)
        {
            if (Activity == ActivityType.ScanDrive || Activity == ActivityType.ScanFolder) return;

            if (this.InvokeRequired)
            {
                SetProcessInProgressCallback d = new SetProcessInProgressCallback(SetProcessInProgress);
                this.Invoke(d, new object[] { Activity });
            }
            else
            {
                processInProgress = Activity;
                
                /*switch(Activity)
                {
                    case ActivityType.None:
                        SetScanFolderEnabled(true);
                        break;
                    default:
                        SetScanFolderEnabled(false);
                        break;
                }*/
            }
        }

        private enum ActivityType
        {
            None = 0,
            ScanFolder = 1,
            ScanDrive = 2,
            AnalyzeSource = 3,
            ConvertTitle = 4,
            FileOperation = 5
        }

        private ISourceStateRepository sourceStateRepository;

        

        public fMain()
        {
            InitializeComponent();

            SetScanInProgress(ActivityType.None);
            SetProcessInProgress(ActivityType.None);

            sourceStateRepository = new Repositories.SourceStateRepository();

            cbxProperties.SelectedIndex = 0;

            defaultOutputSettings = new OutputSettings();

            this.olvSources.PrimarySortColumn = olvTitle;

            this.olvSize.AspectToStringConverter = delegate(object x)
            {
                return ((ulong)x > 0 ? String.Format(new BatchMKV.FileSizeFormatProvider(), "{0:fs}", x) : "?");
            };

            this.olvStatus.AspectToStringConverter = delegate(object x)
            {
                return ((SourceState.Status)x).GetDescription() + ".";
            };

            this.tlvTitles.TriStateCheckBoxes = false;
            this.tlvTitles.CheckedAspectName = "Include";

            this.tlvTitles.CanExpandGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Title))
                    return true;
                else if (x.GetType() == typeof(MKVTools.Track))
                {
                    MKVTools.Track t = (MKVTools.Track)x;
                    return (t.Child != null && !t.Child.IsEmpty);
                }
                else
                    return false;
            };

            this.tlvTitles.ChildrenGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Title))
                    return ((MKVTools.Title)x).Tracks;
                else if (x.GetType() == typeof(MKVTools.Track))
                {
                    MKVTools.Track t = (MKVTools.Track)x;
                    return (t.Child != null && !t.Child.IsEmpty ? new MKVTools.Track[1] { t.Child } : new MKVTools.Track[0] { });
                }
                else
                    return new List<MKVTools.Track>();
            };

            this.tlvTitles.CheckBoxDisabledGetter = delegate(object x)
            {
                MKVTools.Title title = (x.GetType() == typeof(MKVTools.Title))
                    ? (MKVTools.Title)x
                    : ((MKVTools.Track)x).Title;
                
                return (title != null && (title.Result == MKVTools.Title.ConversionResult.ConversionInProgress || title.Result == MKVTools.Title.ConversionResult.Success));
            };

            this.tlvTitles.CheckBoxHiddenGetter = delegate(object x)
            {
                if (x.GetType() != typeof(MKVTools.Track)) return false;
                MKVTools.Track t = (MKVTools.Track)x;
                return (!t.StreamFlags.HasFlag(MKVTools.StreamFlag.DerivedStream) && t.TrackType == MKVTools.TrackType.Video);
            };

            this.otvType.AspectGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Title))
                {
                    return "Title";
                }
                else if (x.GetType() == typeof(MKVTools.Track))
                {
                    return ((MKVTools.Track)x).TrackType.ToString();
                }
                else
                {
                    return "Unknown";
                }
            };

            this.otvDescription.AspectGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Title))
                {
                    MKVTools.Title title = (MKVTools.Title)x;
                    return String.Format(new BatchMKV.FileSizeFormatProvider(), "{0}{1} chapter(s), {2:fs}", 
                        (title.Name != null && title.Name.Length > 0 ? title.Name + " - " : ""), 
                        title.Chapters, 
                        title.Size);
                }
                else if (x.GetType() == typeof(MKVTools.Track))
                {
                    MKVTools.Track track = (MKVTools.Track)x;
                    switch(track.TrackType)
                    {
                        case MKVTools.TrackType.Video:
                            return String.Format("{0}{4} {1}x{2} {3} fps", 
                                track.VideoCodec.GetDescription(), 
                                track.VideoResolution.Width, 
                                track.VideoResolution.Height, 
                                track.VideoFramerate,
                                (track.Name != null && track.Name.Length > 0) ? " " + track.Name : "");
                        case MKVTools.TrackType.Audio:
                            MKVTools.AudioOutputFormat audioOutputFormat = track.AudioOutputFormat(this.makeMKV);
                            return String.Format("{0}{3}{1} {2}", 
                                track.AudioCodec.GetDescription(), 
                                (track.Name != null && track.Name.Length > 0) ? " " + track.Name : track.AudioChannelLayout.GetDescription(), 
                                track.Language.GetDescription(),
                                (audioOutputFormat != MKVTools.AudioOutputFormat.DirectCopy) ? " \u2192 " + audioOutputFormat.GetDescription() : "");
                        case MKVTools.TrackType.Subtitle:
                            return String.Format("{0}{3} {1}{2}", 
                                track.SubtitleCodec.GetDescription(), 
                                track.Language.ToString(), 
                                track.SubtitleForced ? " (forced only)" : "",
                                (track.Name != null && track.Name.Length > 0) ? " " + track.Name : "");
                        default:
                            return "Unknown";
                    }
                }
                else
                {
                    return "Unknown";
                }
            };

            this.otvDefault.AspectGetter = delegate(object x)
            {
                return (x.GetType() == typeof(MKVTools.Track) && ((MKVTools.Track)x).Default);
            };

            this.otvOrder.AspectGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Track))
                {
                    MKVTools.Track track = (MKVTools.Track)x;
                    byte order = track.TrackOrder;
                    return (order > 0 ? order.ToString() : "");
                }
                else
                    return "";
            };

            this.otvOrder.ImageGetter = delegate(object x)
            {
                if (x.GetType() == typeof(MKVTools.Track))
                {
                    MKVTools.Track track = (MKVTools.Track)x;
                    if (track.TrackOrder == 0)
                        return -1;
                    else
                        return (track.TrackType == MKVTools.TrackType.Audio ? 0 : (track.TrackType == MKVTools.TrackType.Subtitle ? 1 : -1));
                }
                else
                    return -1;
            };

            this.otvDefault.CheckBoxHiddenGetter = delegate(object x)
            {
                return !(x.GetType() == typeof(MKVTools.Track) && ((MKVTools.Track)x).TrackType != MKVTools.TrackType.Video);
            };

            this.olvPriority.AspectGetter = delegate (object x)
            {
                if (x.GetType() != typeof(SourceState)) return "";

                SourceState src = (SourceState)x;
                return (src.CurrentStatus == SourceState.Status.Finished ? "" : src.Priority.ToString());
            };

            eventLogger = new WindowsEventLogger();
            eventLogger.SourcePrefix = "BatchMKV";
            eventLogger.LogName = "BATCHMKV";
            eventLogger.Enabled = Properties.Settings.Default.ErrorLogging;

            recentFolders = new List<string>();
            makeMKV = new MKVTools.MakeMKV(); makeMKV.EventLogger = eventLogger;
            mkvToolNix = new MKVTools.MKVToolNix(); mkvToolNix.EventLogger = eventLogger;
            streamAgent = new StreamAgent();
            titleAgent = new TitleAgent();

            drivesAvailable = new Dictionary<DriveInfo, SourceState>();
            scanDriveTimer = new System.Windows.Forms.Timer();
            scanDriveTimer.Tick += scanDriveTimer_Tick;

            purgeDataState();
            restoreDataStatePreload();

            makeMKV.ModifyTrackSettingsAfterConversion = delegate(List<MKVTools.Track> tracks)
            {
                // If a default subtitle track was set, and that subtitle track turned out to be empty, reset default subtitle track.
                foreach(MKVTools.Track track in tracks)
                {
                    if (track.TrackType == MKVTools.TrackType.Subtitle &&
                        track.Default &&
                        track.IsEmpty)
                    {
                        streamAgent.SetDefaultTracks(tracks, false, true);
                        break;
                    }
                }
            };
        }

        private Dictionary<DriveInfo, SourceState> drivesAvailable;

        private delegate bool scanDrivesDelegate();
        private bool scanDrives()
        {
            return (menuMain.InvokeRequired
                ? (bool)this.Invoke(new scanDrivesDelegate(scanDrives))
                : scanDrivesToolStripMenuItem.Checked);
        }

        private delegate void scanDrivesTimerEnabledDelegate(bool Enabled);
        private void scanDrivesTimerEnabled(bool Enabled)
        {
            if (menuMain.InvokeRequired)
                this.Invoke(new scanDrivesTimerEnabledDelegate(scanDrivesTimerEnabled), new object[] { Enabled });
            else if (Enabled)
                scanDriveTimer.Start();
            else
                scanDriveTimer.Stop();
        }

        private async void scanDriveTimer_Tick(object sender, EventArgs e)
        {
            scanDrivesTimerEnabled(false);
            Task<int> t = scanDrivesAsync();
            int n = await t;
            if (n > 0) processNext();

            if (scanDrives())
                scanDrivesTimerEnabled(true);
        }

        private async Task<int> scanDrivesAsync()
        {
            int n = 0;
            await Task.Run(() =>
            {
                // Check for drives which are currently listed, but not ready.
                List<SourceState> sourcesNotReady = new List<SourceState>();
                List<DriveInfo> drivesNotReady = new List<DriveInfo>();
                foreach (KeyValuePair<DriveInfo, SourceState> drive in drivesAvailable)
                {
                    if (!drive.Key.IsReady)
                    {
                        drivesNotReady.Add(drive.Key);
                        RemoveSourceObject(drive.Value);
                    }
                }

                foreach (DriveInfo drive in drivesNotReady)
                    drivesAvailable.Remove(drive);

                // If we're not to scan for new drives, we're done here.
                if (!scanDrives()) return;

                // Check for new drives ready.
                SourceState source;
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.DriveType == DriveType.CDRom &&
                        drive.IsReady &&
                        !drivesAvailable.ContainsKey(drive))
                    {
                        source = new SourceState(LocationType.Drive, drive.Name);
                        drivesAvailable.Add(drive, source);
                        AddSourceObject(source);
                        n++;
                    }
                }
            });

            return n;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            showSourceDetails(null);

            if (Properties.Settings.Default.WindowStartWidth > 0)
            {
                this.Width = (Properties.Settings.Default.WindowStartWidth < Screen.PrimaryScreen.WorkingArea.Width 
                    ? Properties.Settings.Default.WindowStartWidth 
                    : Screen.PrimaryScreen.WorkingArea.Width);
                this.Height = (Properties.Settings.Default.WindowStartHeight < Screen.PrimaryScreen.WorkingArea.Height 
                    ? Properties.Settings.Default.WindowStartHeight
                    : Screen.PrimaryScreen.WorkingArea.Height);
                this.Location = new Point(
                    (Properties.Settings.Default.WindowStartX + this.Width < Screen.PrimaryScreen.WorkingArea.Width
                    ? (Properties.Settings.Default.WindowStartX < 0 ? 0 : Properties.Settings.Default.WindowStartX)
                    : Screen.PrimaryScreen.WorkingArea.Width - this.Width),
                    (Properties.Settings.Default.WindowStartY + this.Height < Screen.PrimaryScreen.WorkingArea.Height
                    ? (Properties.Settings.Default.WindowStartY < 0 ? 0 : Properties.Settings.Default.WindowStartY)
                    : Screen.PrimaryScreen.WorkingArea.Height - this.Height)
                    );
            }
            else
            {
                if (this.Width > Screen.PrimaryScreen.WorkingArea.Width)
                    this.Width = this.DefaultSize.Width;
                if (this.Height > Screen.PrimaryScreen.WorkingArea.Height)
                    this.Height = this.DefaultSize.Height;
                this.CenterToScreen();
            }

            this.WindowState = (Properties.Settings.Default.WindowStartMaximized
                ? FormWindowState.Maximized
                : FormWindowState.Normal);

        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            readSettings();
            updateRecentFolders();

            // Load data state, observing if any missing sources should not be restored
            restoreDataStateApply();

            if (Properties.Settings.Default.ScanOnStart)
                scanFolder(Properties.Settings.Default.ScanOnStartPath);

            if (Properties.Settings.Default.ScanDrives)
                scanDriveTimer.Start();
        }

        private List<SourceState> sourcesRestored;

        private readonly short purgeIntervalDays = 1;
        private void purgeDataState()
        {
            // Do not purge if restore/persist is disabled (any previously data state will not be touched at this point)
            if (!Properties.Settings.Default.RestoreSources) return;

            byte purgeSourceState = Properties.Settings.Default.PurgeSourceState;
            if (purgeSourceState == 0) return;

            DateTime lastPurged = Properties.Settings.Default.PurgeSourceStateLastExecuted;
            if (lastPurged.AddDays(purgeIntervalDays) < DateTime.Now)
            {
                DateTime lastUpdatedBefore;
                switch(purgeSourceState)
                {
                    case 1:
                        lastUpdatedBefore = DateTime.Now.AddDays(-1);
                        break;
                    case 2:
                        lastUpdatedBefore = DateTime.Now.AddDays(-3);
                        break;
                    case 3:
                        lastUpdatedBefore = DateTime.Now.AddDays(-7);
                        break;
                    case 5:
                        lastUpdatedBefore = DateTime.Now.AddMonths(-1);
                        break;
                    case 6:
                        lastUpdatedBefore = DateTime.Now.AddMonths(-3);
                        break;
                    default:
                        lastUpdatedBefore = DateTime.Now.AddDays(-14);
                        break;
                }
                sourceStateRepository.Purge(lastUpdatedBefore);
                Properties.Settings.Default.PurgeSourceStateLastExecuted = DateTime.Now;
                Properties.Settings.Default.Save();
            }
        }

        private void restoreDataStatePreload()
        {
            // Restores source data state to correspond with sources in the repository where RestoreOnStart == true

            sourcesRestored = new List<SourceState>();
            if (!Properties.Settings.Default.RestoreSources) return;

            bool clearMissing = Properties.Settings.Default.ClearMissingSourcesOnStart;
            foreach (SourceState source in sourceStateRepository.GetRestoreOnStart())
            {
                // Do not restore drives
                if (source.LocationType == LocationType.Drive) continue;

                // Do not restore sources that are not available - if configured, clear missing sources so they will not be restored even if they should appear again later.
                if (!source.IsAvailable)
                {
                    if (clearMissing)
                    {
                        source.RestoreOnStart = false;
                        sourceStateRepository.Update(source);
                    }

                    continue;
                }

                // Initialize source
                if (source.Initialize())
                {
                    source.InitializeSource(makeMKV, mkvToolNix);
                    sourcesRestored.Add(source);

                    // Reset source/title status if analysis/conversion/archiving/deleting was in progress when program was exited.
                    switch(source.CurrentStatus)
                    {
                        case SourceState.Status.Analyzing:
                            source.CurrentStatus = SourceState.Status.Initialized;
                            break;
                        case SourceState.Status.Converting:
                            source.CurrentStatus = SourceState.Status.Analyzed;
                            foreach (TitleState t in source.Titles)
                                if (t.Result == MKVTools.Title.ConversionResult.ConversionInProgress)
                                    t.Result = MKVTools.Title.ConversionResult.CancelledByUser;
                            break;
                        case SourceState.Status.DeletingSource:
                            source.CurrentStatus = SourceState.Status.DeleteSourceFailed;
                            break;
                        case SourceState.Status.ArchivingSource:
                            source.CurrentStatus = SourceState.Status.ArchiveSourceFailed;
                            break;
                    }
                }
            }
        }

        private void restoreDataStateApply()
        {
            // Restores source data state to correspond with sources in the repository where RestoreOnStart == true

            olvSources.ClearObjects();
            olvSources.AddObjects(sourcesRestored);

            long lastSelectedID = Properties.Settings.Default.LastSourceSelectedID;
            if (lastSelectedID >= 0 && olvSources.GetItemCount() > 0)
            {
                foreach(SourceState s in olvSources.Objects)
                {
                    if (s.ID == lastSelectedID)
                    {
                        olvSources.DeselectAll();
                        olvSources.SelectObject(s);
                        break;
                    }
                }
            }
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock(activityLock)
            {
                switch(processInProgress)
                {
                    case ActivityType.AnalyzeSource:
                        // Abort analysis.
                        if (analyzeInProgressSource != null)
                            analyzeInProgressSource.AbortScan();
                        break;
                    case ActivityType.ConvertTitle:
                        // Ask for confirmation
                        if (MessageBox.Show("A title is currently being converted, are you sure you want to quit?", "Conversion in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        { e.Cancel = true; return; }
                        else
                            if (conversionInProgressTitle != null)
                                conversionInProgressTitle.AbortConversion();
                        break;
                }
            }

            scanDriveTimer.Stop();
            saveOutputSettings();
            writeSettings();

            long sourceSelectedID;
            if (currentSource != null && currentSource.Tag != null)
            {
                SourceState src = (SourceState)currentSource.Tag;
                storeTreeViewState(src);
                sourceStateRepository.Update(src);

                sourceSelectedID = src.ID;
            }
            else
                sourceSelectedID = -1;

            Properties.Settings.Default.LastSourceSelectedID = sourceSelectedID;
            Properties.Settings.Default.Save();

            // Save data state, observing if any finished sources should be cleared (RestoreOnStart = false)
            updateDataState();
        }

        private void updateDataState()
        {
            // Update RestoreOnStart for data state.

            if (olvSources.Objects == null) return;

            bool clearFinished = Properties.Settings.Default.ClearFinishedSourcesOnExit;
            bool restoreOnStart = Properties.Settings.Default.RestoreSources;
            bool restoreUse;

            foreach (SourceState source in olvSources.Objects)
            {
                restoreUse = (restoreOnStart && (!clearFinished || source.CurrentStatus != SourceState.Status.Finished));
                
                if (source.RestoreOnStart != restoreUse)
                {
                    source.RestoreOnStart = restoreUse;
                    sourceStateRepository.Update(source);
                }
            }
        }

        private void readSettings()
        {
            scanDriveTimer.Interval = (Properties.Settings.Default.ScanDrivePollInterval > 500 ? Properties.Settings.Default.ScanDrivePollInterval : 500);
            scanDrivesToolStripMenuItem.Checked = Properties.Settings.Default.ScanDrives;
            analyzeSourcesAutomaticallyToolStripMenuItem.Checked = Properties.Settings.Default.AnalyzeSourcesAutomatically;
            convertSelectedSourcesToolStripMenuItem.Checked = Properties.Settings.Default.ConvertSourcesAutomatically;
            if (Properties.Settings.Default.ScanRecentFolders.Length > 0)
                recentFolders = Properties.Settings.Default.ScanRecentFolders.Split('|').ToList();
            else
                recentFolders.Clear();

            makeMKV.MakeMKVPath = Properties.Settings.Default.MakeMKVPath;
            mkvToolNix.MKVToolNixPath = Properties.Settings.Default.MKVToolNixPath;
            
            if (!makeMKV.Available || !mkvToolNix.Available)
            {
                if (MessageBox.Show(
                    (!makeMKV.Available && mkvToolNix.Available 
                        ? "The currently specified MakeMKV path does not contain the MakeMKV application files. BatchMKV will not be able to analyze or convert files until this is corrected.\r\n\r\nDo you want to open Settings to specify the MakeMKV path?"
                        : (makeMKV.Available && !mkvToolNix.Available
                            ? "The currently specified MKVToolNix path does not contain the MKVToolNix application files (mkvinfo.exe and mkvpropedit.exe). BatchMKV will not be able to convert files until this is corrected.\r\n\r\nDo you want to open Settings to specify the MKVToolNix path?"
                            : "The currently specified MakeMKV and MKVToolNix paths does not contain the MakeMKV and MKVToolNix application files. BatchMKV will not be able to analyze or convert files until this is corrected.\r\n\r\nDo you want to open Settings to specify the MakeMKV and MKVToolNix paths?")),
                    (!makeMKV.Available && mkvToolNix.Available 
                        ? "Invalid MakeMKV path"
                        : (makeMKV.Available && !mkvToolNix.Available 
                            ? "Invalid MKVToolNix path" 
                            : "Invalid MakeMKV and MKVToolNix paths")), 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning) 
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    settingsToolStripMenuItem_Click(this, null);
                    return;
                }
            }

            // Read audio transcoding settings
            makeMKV.SubtitleCompression = (Properties.Settings.Default.SubtitleCompression ? MKVTools.SubtitleCompression.zlib : MKVTools.SubtitleCompression.None);
            makeMKV.FLACCompression = this.getFLACCompression(Properties.Settings.Default.AudioFLACCompression);
            makeMKV.LPCMContainer = this.getLPCMContainer(Properties.Settings.Default.AudioLPCMContainer);
            makeMKV.AudioOutputFormatDefault = fMain.getAudioOutputFormat(Properties.Settings.Default.AudioTranscodeDefault);
            makeMKV.AudioOutputFormatCustom.Clear();
            string audioTranscode = Properties.Settings.Default.AudioTranscodeCodecs;
            if (audioTranscode.Length > 0)
            {
                string[] entrySetting;
                MKVTools.AudioCodec codec;
                MKVTools.AudioOutputFormat output;
                foreach(string entry in audioTranscode.Split(';'))
                {
                    if (!entry.Contains('=')) continue;
                    entrySetting = entry.Split('=');
                    if (entrySetting.GetLength(0) != 2) continue;
                    codec = MKVTools.MakeMKV.GetAudioCodecByIdentifier(entrySetting[0]);
                    output = fMain.getAudioOutputFormat(entrySetting[1]);
                    makeMKV.AudioOutputFormatCustom.Add(codec, output);
                }
            }

            // Read output settings
            makeMKV.AllowOutputFileOverwrite = Properties.Settings.Default.OutputOverwrite;
            string outputPath = Properties.Settings.Default.DefaultOutputFolder;
            if (outputPath.Length > 0)
            {
                defaultOutputSettings.OutputAtSource = false;
                defaultOutputSettings.OutputPath = outputPath;      // Path can include template tags
            }
            else
            {
                defaultOutputSettings.OutputAtSource = true;
                defaultOutputSettings.OutputPath = String.Empty;
            }
            switch (Properties.Settings.Default.DefaultSourceAction)
            {
                case "archive":
                    defaultOutputSettings.SourceAction = OutputSettings.SourceTarget.Archive; break;
                case "delete":
                    defaultOutputSettings.SourceAction = OutputSettings.SourceTarget.Delete; break;
                default:
                    defaultOutputSettings.SourceAction = OutputSettings.SourceTarget.Keep; break;
            }

            // Titles
            makeMKV.MinimumTitleLength = Properties.Settings.Default.TitlesIgnoreLength;

            // Agents
            streamAgent.UpdateSettings();
            titleAgent.UpdateSettings();

            // Error logging
            eventLogger.Enabled = Properties.Settings.Default.ErrorLogging;
        }

        private void writeSettings()
        {
            Properties.Settings.Default.ScanDrives = scanDrivesToolStripMenuItem.Checked;
            Properties.Settings.Default.AnalyzeSourcesAutomatically = analyzeSourcesAutomaticallyToolStripMenuItem.Checked;
            Properties.Settings.Default.ConvertSourcesAutomatically = convertSelectedSourcesToolStripMenuItem.Checked;

            string folders = "";
            foreach(string folder in recentFolders)
            {
                if (folders.Length > 0) folders += '|';
                folders += folder;
            }
            Properties.Settings.Default.ScanRecentFolders = folders;

            Properties.Settings.Default.Save();
        }

        List<string> recentFolders;
        IEventLogger eventLogger;
        MKVTools.MakeMKV makeMKV;
        MKVTools.MKVToolNix mkvToolNix;
        StreamAgent streamAgent;
        TitleAgent titleAgent;

        delegate void SetStatusMessageCallback(string Message);
        private void SetStatusMessage(string Message)
        {
            if (this.tsMessage.GetCurrentParent().InvokeRequired)
            {
                SetStatusMessageCallback d = new SetStatusMessageCallback(SetStatusMessage);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                this.tsMessage.Text = Message;
            }
        }

        delegate void SetStatusProgressCallback(int Progress);
        private void SetStatusProgress(int Progress)
        {
            if (this.ssStatus.InvokeRequired)
            {
                SetStatusProgressCallback d = new SetStatusProgressCallback(SetStatusProgress);
                this.Invoke(d, new object[] { Progress });
            }
            else
            {
                if (Progress >= 0 && Progress <= 100)
                { 
                    tsProgress.Visible = true; tsProgress.Value = Progress;
                }
                else
                    tsProgress.Visible = false;
            }
        }

        delegate void SetScanFolderEnabledCallback(bool Enabled);
        private void SetScanFolderEnabled(bool Enabled)
        {
            if (this.scanFolderToolStripMenuItem.GetCurrentParent().InvokeRequired)
            {
                SetScanFolderEnabledCallback d = new SetScanFolderEnabledCallback(SetScanFolderEnabled);
                this.Invoke(d, new object[] { Enabled });
            }
            else
            {
                this.scanFolderToolStripMenuItem.Enabled = Enabled;
                foreach (ToolStripMenuItem recentFolder in this.toolStripMenuItemScanRecent.DropDownItems)
                    recentFolder.Enabled = Enabled;
            }
        }

        delegate void AddSourceObjectCallback(SourceState Source);
        private void AddSourceObject(SourceState Source)
        {
            if (this.olvSources.InvokeRequired)
            {
                AddSourceObjectCallback d = new AddSourceObjectCallback(AddSourceObject);
                this.Invoke(d, new object[] { Source });
            }
            else
            {
                // Check if source is already in list
                if (olvSources.Objects != null)
                {
                    foreach (SourceState source in olvSources.Objects)
                        if (source.LocationType == Source.LocationType && source.Location.Equals(Source.Location, StringComparison.InvariantCultureIgnoreCase))
                            return;
                }

                // Add to repository.
                sourceStateRepository.Add(Source);

                // Initialize Source.
                Source.Initialize();

                // Add source to list view
                olvSources.AddObject(Source);

                // Initialize MKVTools Source.
                Source.InitializeSource(makeMKV, mkvToolNix);

                // Update scanning progress status bar.
                updateScanning();
            }
        }

        delegate void RemoveSourceObjectCallback(SourceState Source);
        private void RemoveSourceObject(SourceState Source)
        {
            if (this.olvSources.InvokeRequired)
            {
                RemoveSourceObjectCallback d = new RemoveSourceObjectCallback(RemoveSourceObject);
                this.Invoke(d, new object[] { Source });
            }
            else
            {
                if (olvSources.Objects == null)
                    return;

                bool found = false;
                foreach(SourceState src in olvSources.Objects)
                    if (src.Equals(Source))
                    { found = true; break; }

                if (found)
                    olvSources.RemoveObject(Source);
                
                updateScanning();
            }
        }
        delegate void SetSourceStatusCallback(SourceState Source, SourceState.Status Status);
        private void SetSourceStatus(SourceState Source, SourceState.Status Status)
        {
            if (this.olvSources.InvokeRequired)
            {
                SetSourceStatusCallback d = new SetSourceStatusCallback(SetSourceStatus);
                try { this.Invoke(d, new object[] { Source, Status }); }
                catch { }
            }
            else
            {
                Source.CurrentStatus = Status;
                olvSources.RefreshObject(Source);
            }
        }
        private void scanFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgBrowseFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                queueFolderScan(dlgBrowseFolder.SelectedPath);
                scanNextItem();
            }
        }

        private bool queueFolderScan(string path)
        {
            // Returns true if path was queued, false if it was already queued.

            lock(activityLock)
            {
                foreach(string s in scanQueue)
                {
                    if (s.Equals(path, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                scanQueue.Enqueue(path);
                return true;
            }
        }

        delegate bool scanNextItemCallback();
        private bool scanNextItem()
        {
            if (this.olvSources.InvokeRequired)
            {
                scanNextItemCallback d = new scanNextItemCallback(scanNextItem);
                return (bool)this.Invoke(d);
            }
            else
            {
                string path;

                lock(activityLock)
                {
                    if (scanInProgress != ActivityType.None ||
                        (processInProgress != ActivityType.None && !Properties.Settings.Default.ScanSimultaneously) ||
                        scanQueue.Count == 0) 
                        return false;

                    path = scanQueue.Dequeue();
                }

                scanFolder(path);
                return true;
            }
        }

        private async void scanFolder(string path)
        {
            lock(activityLock)
            {
                if (scanInProgress != ActivityType.None || (processInProgress != ActivityType.None && !Properties.Settings.Default.ScanSimultaneously)) return;
                SetScanInProgress(ActivityType.ScanFolder);
            }

            ScanFolder scan = new ScanFolder();
            scan.ScanStarted += scanStarted;
            scan.ScanFoundSource += scanFoundSource;
            scan.ScanFailed += scanFailed;
            scan.ScanCompleted += scanCompleted;

            // Update Recent Folders
            int i = 0;
            bool found = false;
            foreach(string folder in recentFolders)
            {
                if (folder.Equals(path, StringComparison.OrdinalIgnoreCase)) { found = true; break; }
                i++;
            }
            if (found) recentFolders.RemoveAt(i);
            recentFolders.Insert(0, path);
            byte maxFolders = (Properties.Settings.Default.RecentFoldersMax > 0 && Properties.Settings.Default.RecentFoldersMax <= 20 ? Properties.Settings.Default.RecentFoldersMax : (byte)5);
            if (recentFolders.Count > maxFolders) recentFolders.RemoveRange(maxFolders, recentFolders.Count - maxFolders);
            updateRecentFolders();

            // Initiate scan
            await Task.Run(() => scan.Scan(path, true));
        }

        delegate void analyzeNextSourceCallback(SourceState Source = null);
        private void analyzeNextSource(SourceState Source = null)
        {
            if (this.olvSources.InvokeRequired)
            {
                analyzeNextSourceCallback d = new analyzeNextSourceCallback(analyzeNextSource);
                this.Invoke(d, new object[] { Source });
            }
            else
            {
                // Get next source for scanning.
                SourceState source = (Source != null ? Source : getNextSourceForAnalysis());
                if (source == null || source.Source == null)
                {
                    if (Source == null)
                    {
                        // There are no further sources to analyze at this time,
                        // so if conversion is enabled...
                        if (convertAutomatically())
                        {
                            // ... convert the next title.
                            convertNextTitle();
                        }
                    }

                    return;
                }

                // Check that MakeMKV is available.
                if (!makeMKV.Available) return;

                // Get MKV source and check that it is not scanning/scanned.
                MKVTools.Source mkvSource = source.Source;
                if (mkvSource.Result == MKVTools.Source.ScanResult.ScanInProgress || mkvSource.Result == MKVTools.Source.ScanResult.Success) return;

                lock(activityLock)
                {
                    if (processInProgress != ActivityType.None) return;
                    analyzeInProgressSource = mkvSource;
                    SetProcessInProgress(ActivityType.AnalyzeSource);
                }

                // Hookup events.
                mkvSource.ScanStarted += mkvScanStarted;
                mkvSource.ScanFailed += mkvScanFailed;
                mkvSource.ScanCompleted += mkvScanCompleted;
                mkvSource.ScanProgress += mkvScanProgress;

                // Start scan.
                mkvSource.Scan();
            }
        }

        delegate bool isSourceSelectedCallback(SourceState Source);
        private bool isSourceSelected(SourceState Source)
        {
            if (this.olvSources.InvokeRequired)
                return (bool)this.Invoke(new isSourceSelectedCallback(isSourceSelected), new object[] { Source });
            else
                return (this.olvSources.SelectedObject == Source);
        }

        delegate void refreshTreeViewCallback();
        private void refreshTreeView()
        {
            if (this.tlvTitles.InvokeRequired)
                this.Invoke(new refreshTreeViewCallback(refreshTreeView));
            else
            {
                tlvTitles.Refresh();
            }
        }

        delegate void refreshTreeViewTitleCallback(MKVTools.Title Title);
        private void refreshTreeView(MKVTools.Title Title)
        {
            if (this.tlvTitles.InvokeRequired)
                this.Invoke(new refreshTreeViewTitleCallback(refreshTreeView), new object[] { Title });
            else
            {
                foreach (MKVTools.Track t in Title.Tracks)
                {
                    try { tlvTitles.RefreshObject(t); }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Exception: " + ex.ToString());
                    }
                }
            }
        }

        delegate bool analyzeAutomaticallyCallback();
        private bool analyzeAutomatically()
        {
            if (holdProcessing) return false;

            if (this.menuMain.InvokeRequired)
            {
                analyzeAutomaticallyCallback d = new analyzeAutomaticallyCallback(analyzeAutomatically);
                return (bool)this.Invoke(d);
            }
            else
            {
                return (this.analyzeSourcesAutomaticallyToolStripMenuItem.Checked);
            }
        }

        delegate bool convertAutomaticallyCallback();
        private bool convertAutomatically()
        {
            if (holdProcessing) return false;

            if (this.menuMain.InvokeRequired)
            {
                return (bool)this.Invoke(new convertAutomaticallyCallback(convertAutomatically));
            }
            else
            {
                return (this.convertSelectedSourcesToolStripMenuItem.Checked);
            }
        }

        delegate void convertNextTitleCallback(MKVTools.Title Title);
        private async void convertNextTitle(MKVTools.Title Title = null)
        {
            if (this.olvSources.InvokeRequired)
            {
                convertNextTitleCallback d = new convertNextTitleCallback(convertNextTitle);
                this.Invoke(d, new object[] { Title });
            }
            else
            {
                // Check that MakeMKV and MKVToolNix utilities are available.
                if (!makeMKV.Available || !mkvToolNix.Available) return;

                // Get next title for conversion.
                MKVTools.Title title = (Title != null ? Title : getNextTitleForConversion());
                if (title == null) return;

                // Check that it is not converting/converted.
                if (title.Result == MKVTools.Title.ConversionResult.ConversionInProgress || title.Result == MKVTools.Title.ConversionResult.Success) return;

                lock (activityLock)
                {
                    if (processInProgress != ActivityType.None) return;
                    conversionInProgressTitle = title;
                    SetProcessInProgress(ActivityType.ConvertTitle);
                }

                SourceState src = (SourceState)title.Source.Tag;

                // Determine output folder
                OutputSettings output = src.OutputSettings != null ? src.OutputSettings : defaultOutputSettings;
                title.OutputFolder = output.GetOutputPath(title.Source);

                /*if (output.OutputAtSource && src.LocationType == ScanFolder.LocationType.File)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(title.Source.Location);
                        title.OutputFolder = fi.DirectoryName;
                    }
                    catch
                    { 
                        title.OutputFolder = title.Source.Location; 
                    }
                }
                else
                    title.OutputFolderTemplate = output.OutputPath;     // Set to template instead of fixed because default output settings would need to be a template*/

                //Debug.WriteLine("Convert, Output @ Source: {0}, Output Folder: {1}, Source Action: {2}", output.OutputSource, title.OutputFolder, output.Source);

                // Check that,
                //      if source is a file: output file does not overwrite source file
                //      if source is a folder: output file does not overwrite file in source folder
                await Task.Run(() =>
                {
                    if (src.SourceDeleted || src.SourceMoved)
                    {
                        SetSourceStatus(src, SourceState.Status.ConversionFailed);
                        SetStatusMessage("Conversion failed: Source file(s) not available.");
                        title.ResetResult(MKVTools.Title.ConversionResult.SourceNotFound);
                        return;
                    }

                    if (src.LocationType == LocationType.File)
                    {
                        if (title.OutputFullName.ToLower().Equals(src.Location.ToLower()))
                        {
                            SetSourceStatus(src, SourceState.Status.ConversionFailed);
                            SetStatusMessage(String.Format("Conversion failed: Output file '{0}' would overwrite source file.", title.OutputFilename));
                            title.ResetResult(MKVTools.Title.ConversionResult.OutputFileAlreadyExists);
                            return;
                        }
                    }
                    else
                    {
                        bool outputLocationInvalid;
                        try 
                        {
                            outputLocationInvalid = (title.OutputFullName.Length > src.Location.Length &&
                                title.OutputFullName.Substring(0, src.Location.Length).ToLower().Equals(src.Location.ToLower()) &&
                                File.Exists(title.OutputFullName));
                        }
                        catch
                        { outputLocationInvalid = true; }

                        if (outputLocationInvalid)
                        {
                            SetSourceStatus(src, SourceState.Status.ConversionFailed);
                            SetStatusMessage(String.Format("Conversion failed: Output file '{0}' would overwrite file in source folder.", title.OutputFilename));
                            title.ResetResult(MKVTools.Title.ConversionResult.OutputFileAlreadyExists);
                            return;
                        }

                    }
                });

                //SetSourceStatus(src, ScanFolder.MediaSource.Status.ConversionFailed);
                //SetStatusMessage("Conversion failed: Just debuggin'.");
                //title.ResetResult(MKVTools.Title.ConversionResult.CancelledByUser);
                //return;

                // Hookup events.
                title.ConversionStarted += mkvConversionStarted;
                title.ConversionProgress += mkvConversionProgress;
                title.ConversionFailed += mkvConversionFailed;
                title.ConversionCompleted += mkvConversionCompleted;

                // Start conversion.
                title.Convert();
            }
        }

        private SourceState fileOpSource;
        private OutputSettings.SourceTarget fileOpAction;

        private void mkvConversionCompleted(object sender, MKVTools.ConversionEventArgs e)
        {
            MKVTools.Title title = (MKVTools.Title)sender;
            SourceState source = (SourceState)title.Source.Tag;
            SetSourceStatus(source, SourceState.Status.Waiting);
            SetStatusMessage(String.Format("Conversion completed: {0} ({1}).", title.OutputFilename, source.Title));
            SetStatusProgress(-1);
            //updateProgressTotalDone(title);
            updateRemainingTitles();

            if (title.Source == currentSource)
            {
                refreshTreeView(title);
                refreshTreeView();
            }

            if (title == currentTitle || (currentTrack != null && title == currentTrack.Title))
                updateConversionStatus(title);

            lock (activityLock)
                if (processInProgress == ActivityType.ConvertTitle)
                {
                    SetProcessInProgress(ActivityType.None);
                    conversionInProgressTitle = null;
                }

            processNext(title.Source);
        }

        private void fileOperationCompleted(MKVTools.FileOperation sender, MKVTools.FileOperation.OperationEventArgs e)
        {
            lock (activityLock)
            {
                SetSourceStatus(fileOpSource, SourceState.Status.Finished);
                SetStatusMessage(String.Format("{1} source completed: {0}.", fileOpSource.Title, (fileOpAction == OutputSettings.SourceTarget.Archive ? "Archive" : "Delete")));
                SetStatusProgress(-1);
                
                switch(fileOpAction)
                {
                    case OutputSettings.SourceTarget.Archive:
                        fileOpSource.SourceMoved = true;
                        // fileOpSource.Location = update location
                        break;
                    case OutputSettings.SourceTarget.Delete:
                        fileOpSource.SourceDeleted = true;
                        break;
                }

                if (processInProgress == ActivityType.FileOperation) 
                    SetProcessInProgress(ActivityType.None);
            }

            processNext();
        }

        private void fileOperationFailed(MKVTools.FileOperation sender, MKVTools.FileOperation.OperationEventArgs e)
        {
            lock (activityLock)
            {
                SetSourceStatus(fileOpSource, (fileOpAction == OutputSettings.SourceTarget.Archive ? SourceState.Status.ArchiveSourceFailed : SourceState.Status.DeleteSourceFailed));
                SetStatusMessage(String.Format("{1} source failed: {0}.", fileOpSource.Title, (fileOpAction == OutputSettings.SourceTarget.Archive ? "Archive" : "Delete")));
                SetStatusProgress(-1);

                if (processInProgress == ActivityType.FileOperation) 
                    SetProcessInProgress(ActivityType.None);
            }

            processNext();
        }

        private void fileOperationProgress(MKVTools.FileOperation sender, MKVTools.FileOperation.OperationProgressEventArgs e)
        {
            SetStatusProgress((int)Math.Round(e.Progress * 100));
        }

        private void fileOperationStarted(MKVTools.FileOperation sender, MKVTools.FileOperation.OperationEventArgs e)
        {
            lock (activityLock)
            {
                SetSourceStatus(fileOpSource, (fileOpAction == OutputSettings.SourceTarget.Archive ? SourceState.Status.ArchivingSource : SourceState.Status.DeletingSource));
                SetStatusMessage(String.Format("{1} source: {0}...", fileOpSource.Title, (fileOpAction == OutputSettings.SourceTarget.Archive ? "Archiving" : "Deleting")));
                SetStatusProgress(0);
            }
        }

        private void mkvConversionFailed(object sender, MKVTools.ConversionEventArgs e)
        {
            MKVTools.Title title = (MKVTools.Title)sender;
            SourceState source = (SourceState)title.Source.Tag;
            SetSourceStatus(source, SourceState.Status.ConversionFailed);
            SetStatusMessage(String.Format("Conversion failed: {0} ({1}).", title.OutputFilename, source.Title));
            SetStatusProgress(-1);
            //updateProgressTotalDone(title);

            if (title.Source == currentSource) refreshTreeView();
            if (title == currentTitle || (currentTrack != null && title == currentTrack.Title))
                updateConversionStatus(title);

            lock (activityLock)
            {
                if (processInProgress == ActivityType.ConvertTitle)
                {
                    SetProcessInProgress(ActivityType.None);
                    conversionInProgressTitle = null;
                }
            }

            processNext();
        }

        private void mkvConversionProgress(object sender, MKVTools.ProgressEventArgs e)
        {
            SetStatusProgress((int)Math.Round(e.TotalProgress * 100));
            //updateProgressTotalCurrent((MKVTools.Title)sender, e.TotalProgress);
        }

        private void mkvConversionStarted(object sender, MKVTools.ConversionEventArgs e)
        {
            MKVTools.Title title = (MKVTools.Title)sender;
            SourceState source = (SourceState)title.Source.Tag;
            SetSourceStatus(source, SourceState.Status.Converting);
            SetStatusMessage(String.Format(new FileSizeFormatProvider(), "Converting: {0} (~{1:fs}, {2})...", title.OutputFilename, title.Size, source.Title));
            SetStatusProgress(0);
            //updateProgressTotalCurrent(title, 0);

            if (title.Source == currentSource) refreshTreeView();
            if (title == currentTitle || (currentTrack != null && title == currentTrack.Title))
                updateConversionStatus(title);
        }

        private void mkvScanCompleted(object sender, MKVTools.ScanEventArgs e)
        {
            MKVTools.Source src = (MKVTools.Source)sender;
            SourceState source = (SourceState)src.Tag;
            SetSourceStatus(source, SourceState.Status.Analyzed);
            SetStatusMessage("Analysis completed: " + source.Title + ".");
            SetStatusProgress(-1);
            //updateProgressTotalDone((MKVTools.Source)sender);

            // Update SourceState - instantiate and link TitleState and TrackState objects
            source.PrepareScanned();

            SourceState sourceSettings = null;
            // Only restore from data store if option is enabled to persist data state
            if (Properties.Settings.Default.RestoreSources)
                sourceSettings = sourceStateRepository.GetMatch(src.Hash, source.LocationType, source.Location, source.ID);

            if (sourceSettings != null && sourceSettings != source)
            {
                // Apply selections and settings from data store.
                sourceSettings.CopySettings(source);
            }
            else
            {
                // Apply default title selections and settings
                titleAgent.ApplyDefaultSelections(src.Titles);
                titleAgent.ApplyDefaultSettings(src.Titles);

                // Expand selected titles
                TitleState ts;
                foreach (MKVTools.Title title in src.Titles.Where(t => t.Include))
                {
                    if ((ts = source.Titles.Single(t => t.Title == title)) != null)
                        ts.Expanded = true;
                }

                // Apply default track selections, ordering and default tracks.
                foreach (MKVTools.Title title in src.Titles)
                    streamAgent.ApplyDefaults(title.Tracks);
            }

            // Update source in repository.
            sourceStateRepository.Update(source);

            lock (activityLock)
            {
                if (processInProgress == ActivityType.AnalyzeSource)
                {
                    SetProcessInProgress(ActivityType.None);
                    analyzeInProgressSource = null;
                }
            }

            if (isSourceSelected(source))
                showSourceDetails(source.Source);

            updateRemainingTitles();
            processNext();
        }

        private void mkvScanFailed(object sender, MKVTools.ScanEventArgs e)
        {
            SourceState source = (SourceState)((MKVTools.Source)sender).Tag;
            SetSourceStatus(source, SourceState.Status.AnalysisFailed);
            SetStatusMessage("Analysis failed: " + source.Title + ".");
            SetStatusProgress(-1);
            //updateProgressTotalDone((MKVTools.Source)sender);

            lock (activityLock)
            {
                if (processInProgress == ActivityType.AnalyzeSource)
                {
                    SetProcessInProgress(ActivityType.None);
                    analyzeInProgressSource = null;
                }
            }

            processNext();
        }

        private void mkvScanProgress(object sender, MKVTools.ProgressEventArgs e)
        {
            SetStatusProgress((int)Math.Round(e.TotalProgress * 100));
            //updateProgressTotalCurrent((MKVTools.Source)sender, e.TotalProgress);
        }

        private void mkvScanStarted(object sender, MKVTools.ScanEventArgs e)
        {
            SourceState source = (SourceState)((MKVTools.Source)sender).Tag;
            SetSourceStatus(source, SourceState.Status.Analyzing);
            SetStatusMessage("Analyzing: " + source.Title + "...");
            SetStatusProgress(0);
            //updateProgressTotalCurrent((MKVTools.Source)sender, 0);
        }

        private SourceState getNextSourceForAnalysis()
        {
            // Priority:
            //      1/ Checked items
            //      2/ Selected items
            //      3/ The rest

            SourceState result = null;
            int resultRank = -1, rank;

            foreach(SourceState source in olvSources.Objects)
            {
                // Only pick for scan if scan has not previously been initiated (TODO: should include some type of forced flag).
                if (source.Source == null || source.Source.Result != MKVTools.Source.ScanResult.NotAvailable) continue;

                // Rank by:
                //      1) Priority (low/medium/high)
                //      2) Checked for conversion
                //      3) Selected

                rank = (int)source.Priority * 100 + (source.Convert ? 10 : 0) + (olvSources.IsSelected(source) ? 1 : 0);
                if (rank > resultRank)
                {
                    result = source;
                    resultRank = rank;
                    if (rank == 211) return result; // Highest possible rank - no need to iterate over remaining sources
                }
            }

            return result;
        }

        private MKVTools.Title getNextTitleForConversion()
        {
            SourceState target = null;
            int targetRank = -1, rank;

            foreach (SourceState source in olvSources.CheckedObjects)
            {
                if (source.Source == null || source.Source.Titles == null ||
                    (source.CurrentStatus != SourceState.Status.Analyzed && source.CurrentStatus != SourceState.Status.Waiting)) continue;

                rank = (int)source.Priority;
                if (rank > targetRank)
                {
                    target = source;
                    targetRank = rank;
                    if (rank == 2) break; // Highest possible rank - no need to iterate over remaining sources
                }
            }

            if (target != null)
            {
                foreach (MKVTools.Title title in target.Source.Titles)
                {
                    if (title.Include && title.Result == MKVTools.Title.ConversionResult.NotAvailable)
                        return title;
                }
            }

            return null;
        }

        delegate void updateScanningDelegate();
        private void updateScanning()
        {
            if (ssStatus.InvokeRequired)
                this.Invoke(new updateScanningDelegate(updateScanning));
            else
            {
                bool inProgress;
                lock (activityLock)
                { inProgress = scanInProgress != ActivityType.None; }

                if (inProgress)
                    tsSources.Text = "Scanning...";
                else
                    tsSources.Text = String.Format("{0} source(s)", olvSources.Items.Count);
            }
        }

        delegate void updateRemainingTitlesDelegate();
        private void updateRemainingTitles()
        {
            if (ssStatus.InvokeRequired)
                this.Invoke(new updateRemainingTitlesDelegate(updateRemainingTitles));
            else
            {
                int titles = 0;
                ulong size = 0;
                foreach (SourceState source in olvSources.CheckedObjects)
                {
                    if (source.Source == null || source.Source.Titles == null) continue;
                    foreach (MKVTools.Title title in source.Source.Titles)
                    {
                        if (title.Include && title.Result != MKVTools.Title.ConversionResult.Success)
                        {
                            titles++;
                            size += (ulong)title.Size;
                        }
                    }
                }

                if (titles > 0)
                {
                    tsRemaining.Text = String.Format(new FileSizeFormatProvider(), "To convert: {0} title(s), ~{1:fs}", titles, size);
                    tsRemaining.Visible = true;
                }
                else
                    tsRemaining.Visible = false;
            }
        }

        private void scanFoundSource(object sender, ScanFolder.ScanEventArgs e)
        {
            AddSourceObject(e.Source);
            //updateProgressTotalRemaining();
        }

        private void scanFailed(object sender, ScanFolder.ScanEventArgs e)
        {
            // SetScanFolderEnabled(true);
            // if (!Properties.Settings.Default.ScanSimultaneously) SetStatusMessage(e.Message);

            lock (activityLock)
            { SetScanInProgress(ActivityType.None); }

            updateScanning();
            processNext();
        }

        private void scanCompleted(object sender, ScanFolder.ScanEventArgs e)
        {
            // SetScanFolderEnabled(true);
            // if (!Properties.Settings.Default.ScanSimultaneously) SetStatusMessage(e.Message);

            lock (activityLock)
            { SetScanInProgress(ActivityType.None); }

            updateScanning();
            processNext();
        }

        private void processNext(MKVTools.Source Source = null)
        {
            if (Source != null)
            {
                // Check if there are no unconverted titles left for this source.
                // If there is not, archive/delete/keep source to finish this source now.
                bool sourceCompleted = true;
                foreach (MKVTools.Title t in Source.Titles)
                {
                    if (t.Include && t.Result != MKVTools.Title.ConversionResult.Success)
                    { sourceCompleted = false; break; }
                }

                if (sourceCompleted)
                {
                    SourceState src = (SourceState)Source.Tag;
                    OutputSettings output = src.OutputSettings != null ? src.OutputSettings : defaultOutputSettings;
                    if (output.SourceAction != OutputSettings.SourceTarget.Keep && src.LocationType != LocationType.Drive)
                    {
                        // Check that a file operation is not currently in progress, before starting file operation on this source.
                        bool startFileOp = false;

                        lock (activityLock)
                        {
                            if (processInProgress == ActivityType.None)
                            {
                                fileOpSource = src;
                                fileOpAction = output.SourceAction;
                                SetProcessInProgress(ActivityType.FileOperation);
                                startFileOp = true;
                            }
                        }

                        if (startFileOp)
                        {
                            // Initialize file operation and hookup events.
                            MKVTools.FileOperation fo = new MKVTools.FileOperation();
                            fo.OperationStarted += fileOperationStarted;
                            fo.OperationProgress += fileOperationProgress;
                            fo.OperationFailed += fileOperationFailed;
                            fo.OperationCompleted += fileOperationCompleted;

                            // Start file operation on source
                            switch (output.SourceAction)
                            {
                                case OutputSettings.SourceTarget.Archive:
                                    string archivePath = Properties.Settings.Default.ArchiveFolder;
                                    if (src.LocationType != LocationType.File)
                                    {
                                        if (!archivePath.EndsWith("\\")) archivePath += "\\";
                                        archivePath += src.Location.Substring(src.Location.LastIndexOf('\\') + 1);
                                    }
                                    else
                                    {
                                        if (archivePath.EndsWith("\\")) archivePath = archivePath.Substring(0, archivePath.Length - 1);
                                    }
                                    Debug.WriteLine("Archive, move '{0}' to '{1}'.", src.Location, archivePath);
                                    fo.MoveAsync(src.Location, archivePath);
                                    break;
                                case OutputSettings.SourceTarget.Delete:
                                    Debug.WriteLine("Delete '{0}'.", new object[] { src.Location });
                                    fo.DeleteAsync(src.Location);
                                    break;
                            }

                            // Don't proceed to analyze/convert, as file operation is now in progress.
                            return;
                        }
                    }
                    else
                    {
                        SetSourceStatus(src, SourceState.Status.Finished);
                        processNext();
                    }
                }
            }

            // Scan next item. If a scan was started, and scan/analyze/convert is not running simultaneously, don't check for analyze/convert.
            if (scanNextItem() && !Properties.Settings.Default.ScanSimultaneously)
                return;

            if (analyzeAutomatically())
                analyzeNextSource();
            else if (convertAutomatically())
                convertNextTitle();
        }

        private void scanStarted(object sender, ScanFolder.ScanEventArgs e)
        {
            // SetScanFolderEnabled(false);
            // if (!Properties.Settings.Default.ScanSimultaneously) SetStatusMessage(e.Message);
            updateScanning();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAbout f = new fAbout();
            f.ShowDialog();
            f.Dispose();
        }

        private void olvSources_CellToolTipShowing(object sender, BrightIdeasSoftware.ToolTipShowingEventArgs e)
        {
            e.Text = ((SourceState)e.Model).Location;
        }

        private void analyzeSourcesAutomaticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analyzeSourcesAutomaticallyToolStripMenuItem.Checked = !analyzeSourcesAutomaticallyToolStripMenuItem.Checked;

            if (analyzeAutomatically())
                analyzeNextSource();
        }

        private void olvSources_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
        }

        delegate void showSourceDetailsDelegate(MKVTools.Source Source);
        private void showSourceDetails(MKVTools.Source Source)
        {
            if (tlvTitles.InvokeRequired)
            {
                showSourceDetailsDelegate d = new showSourceDetailsDelegate(showSourceDetails);
                this.Invoke(d, new object[] { Source });
            }
            else
            {
                SourceState src;

                // Save output settings + data state for currently selected and showing source, before showing another source or hiding this source
                if (currentSource != null && currentSource != Source)
                {
                    saveOutputSettings();
                    src = (SourceState)currentSource.Tag;
                    storeTreeViewState(src);
                    sourceStateRepository.Update(src);
                }

                if (Source != null && Source.Result == MKVTools.Source.ScanResult.Success)
                {
                    src = (SourceState)Source.Tag;

                    // Update treeview
                    tlvTitles.Roots = Source.Titles;
                    updateTreeViewState(src);

                    // Show appropriate output settings
                    OutputSettings output = (src.OutputSettings != null ? src.OutputSettings : defaultOutputSettings);

                    // Update output folder view
                    if (output.GetOutputAtSource(Source))
                        radOutputSource.Checked = true;
                    else
                        radOutputSpecified.Checked = true;
                    txtOutputSpecified.Text = output.GetOutputPath(Source);

                    updateOutputSettingsUI();

                    if (Source.Type == MKVTools.SourceType.Disc || Source.Type == MKVTools.SourceType.Device)
                    {
                        radOutputSource.Enabled = false;
                        cbxSourceAction.SelectedIndex = 0;
                        cbxSourceAction.Enabled = false;
                    }
                    else
                    {
                        radOutputSource.Enabled = true;
                        switch (output.SourceAction)
                        {
                            case OutputSettings.SourceTarget.Archive:
                                cbxSourceAction.SelectedIndex = 1; break;
                            case OutputSettings.SourceTarget.Delete:
                                cbxSourceAction.SelectedIndex = 2; break;
                            default:
                                cbxSourceAction.SelectedIndex = 0; break;
                        }
                        cbxSourceAction.Enabled = true;
                    }

                    // Enable details view
                    splitContainer2.Panel2.Enabled = true;
                }
                else
                {
                    // Clear treeview
                    tlvTitles.ClearObjects();

                    // Disable details view
                    splitContainer2.Panel2.Enabled = false;
                }

                // Update current source to new source
                currentSource = Source;
            }
        }

        private void updateTreeViewState(SourceState source)
        {
            if (source == null || source.Titles == null) return;

            tlvTitles.Freeze();
            tlvTitles.ExpandAll();

            foreach(TitleState title in source.Titles)
            {
                foreach (TrackState track in title.Tracks)
                    if (!track.Expanded && track.Track != null && track.Track.Child != null)
                        tlvTitles.Collapse(track.Track);

                if (!title.Expanded && title.Title != null)
                    tlvTitles.Collapse(title.Title);
            }

            tlvTitles.Unfreeze();
        }

        private void storeTreeViewState(SourceState source)
        {
            if (source == null || source.Titles == null) return;

            MKVTools.Title t;
            MKVTools.Track tr;

            foreach (TitleState title in source.Titles)
            {
                if ((t = title.Title) != null)
                    title.Expanded = tlvTitles.IsExpanded(t);

                foreach (TrackState track in title.Tracks)
                    if ((tr = track.Track) != null)
                        track.Expanded = tlvTitles.IsExpanded(tr);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void convertSelectedSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            convertSelectedSourcesToolStripMenuItem.Checked = !convertSelectedSourcesToolStripMenuItem.Checked;

            if (convertAutomatically())
                convertNextTitle();
        }

        private void olvSources_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            updateRemainingTitles();
            //updateProgressTotalRemaining();

            // If item was unchecked, no need to check further.
            if (!e.Item.Checked) return;

            // If conversion is enabled convert the next title.
            if (convertAutomatically())
                convertNextTitle();
        }

        private void tlvTitles_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            updateRemainingTitles();
            //updateProgressTotalRemaining();
            tlvTitles.Refresh();

            if (currentTitle != null)
            {
                updateTitleInfo(currentTitle);
                readPropertyValue(currentTitle);
            }

            // If item was unchecked, no need to check further.
            if (!e.Item.Checked) return;

            // If conversion is enabled convert the next title.
            if (convertAutomatically())
                convertNextTitle();

            
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSettings f = new fSettings();
            f.ShowTroubleshooting = (sender.ToString() == "&Troubleshooting");
            f.StartPosition = FormStartPosition.CenterParent;
            
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                readSettings();
                if (currentSource != null)
                    tlvTitles.Refresh();
                    //showSourceDetails(currentSource);
                processNext();
            }
        }

        private void radOutputSource_CheckedChanged(object sender, EventArgs e)
        {
            updateOutputSettingsUI();
            saveOutputSettings();
        }

        private void radOutputSpecified_CheckedChanged(object sender, EventArgs e)
        {
            updateOutputSettingsUI();
            saveOutputSettings();
        }

        private void saveOutputSettings()
        {
            if (currentSource == null || !splitContainer2.Panel2.Enabled) 
                return;

            if (!radOutputSource.Checked && 
                txtOutputSpecified.Enabled && 
                txtOutputSpecified.Text.Trim().Length > 0 &&
                (!Regex.Match(txtOutputSpecified.Text.Trim(), validFolderRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline).Success || File.Exists(txtOutputSpecified.Text.Trim())))
            {
                MessageBox.Show("The specified output folder is not valid.", "Invalid output folder", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtOutputSpecified.Focus();
                return;
            }

            OutputSettings output = new OutputSettings();
            output.OutputAtSource = radOutputSource.Checked;
            output.OutputPath = output.OutputAtSource
                ? String.Empty
                : txtOutputSpecified.Text.Trim();
            output.SourceAction = (OutputSettings.SourceTarget)(cbxSourceAction.SelectedIndex);

            SourceState src = (SourceState)currentSource.Tag;

            if (output.Equals(defaultOutputSettings))
                // Don't save, use defaults. Revert to defaults if saved source-specific previously.
                src.OutputSettings = null;
            else 
                src.OutputSettings = output;

            // debugPrintOutputSettingsAll();
        }

        private void debugPrintOutputSettingsAll()
        {
            if (outputSettings.Count == 0)
            { Debug.WriteLine("All sources uses default output settings."); return; }

            foreach(KeyValuePair<MKVTools.Source, OutputSettings> outputSetting in outputSettings)
                Debug.WriteLine("Source: {0}, Output: {1}, SourceAction: {2}", outputSetting.Key.Location, outputSetting.Value.OutputAtSource ? "Same as source" : outputSetting.Value.OutputPath, outputSetting.Value.SourceAction);
        }

        private void updateOutputSettingsUI()
        {
            if (radOutputSource.Checked)
            {
                txtOutputSpecified.Enabled = false;
                btnBrowseOutput.Enabled = false;
            }
            else
            {
                txtOutputSpecified.Enabled = true;
                btnBrowseOutput.Enabled = true;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            dlgOutputPath.SelectedPath = txtOutputSpecified.Text;
            if (dlgOutputPath.ShowDialog() == DialogResult.OK)
            {
                txtOutputSpecified.Text = dlgOutputPath.SelectedPath;
                saveOutputSettings();
            }
        }

        // Use these variables to track the currently selected source/title/track
        private MKVTools.Source currentSource;
        private MKVTools.Title currentTitle;
        private MKVTools.Track currentTrack;

        // Use this dict. to keep track of output settings
        private OutputSettings defaultOutputSettings;
        private Dictionary<MKVTools.Source, OutputSettings> outputSettings;

        private void olvSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            MKVTools.Source newSource = (olvSources.SelectedObject != null
                ? ((SourceState)olvSources.SelectedObject).Source
                : null);

            showSourceDetails(newSource);

            // TODO: Check if it is necessary to invoke this manually
            tlvTitles_SelectedIndexChanged(this, null);
        }

        private void tlvTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tlvTitles.SelectedObject != null && tlvTitles.SelectedObject.GetType() == typeof(MKVTools.Title))
                showTitleDetails((MKVTools.Title)tlvTitles.SelectedObject);
            else if (tlvTitles.SelectedObject != null && tlvTitles.SelectedObject.GetType() == typeof(MKVTools.Track))
                showTrackDetails((MKVTools.Track)tlvTitles.SelectedObject);
            else
                showTitleDetails(null);
        }

        private readonly string[][] itemProperties = new string[4][]
        { 
            new string[] { "Name", "Metadata language", "File name" },
            new string[] { "Name", "Metadata language", "Language", "MKV flags" },
            new string[] { "Name", "Metadata language", "Language", "MKV flags", "Order weight" },
            new string[] { "Name", "Metadata language", "Language", "MKV flags", "Order weight" }
        };

        private void showTitleDetails(MKVTools.Title Title)
        {
            if (Title != null)
            {
                grpInformation.Text = "Title information";
                updateTitleInfo(Title);
                updatePropertiesUI(0);
                readPropertyValue(Title);
                updateConversionStatus(Title);
            }
            else
            {
                txtInformation.Text = "";
                txtPropertyValue.Text = "";
                updateConversionStatus();
            }

            currentTitle = Title;
            currentTrack = null;
        }

        private void showTrackDetails(MKVTools.Track Track)
        {
            if (Track != null)
            {
                grpInformation.Text = "Track information";
                updateTrackInfo(Track);
                updatePropertiesUI((byte)Track.TrackType);
                readPropertyValue(Track);
                updateConversionStatus(Track.Title);
            }
            else
            {
                txtInformation.Text = "";
                txtPropertyValue.Text = "";
                updateConversionStatus();
            }

            currentTrack = Track;
            currentTitle = null;
        }

        delegate void updateConversionStatusDelegate(MKVTools.Title Title);
        private void updateConversionStatus(MKVTools.Title Title = null)
        {
            if (lblConversionStatus.InvokeRequired)
            {
                updateConversionStatusDelegate d = new updateConversionStatusDelegate(updateConversionStatus);
                this.Invoke(d, new object[] { Title });
            }
            else
            {
                if (Title != null)
                {
                    switch (Title.Result)
                    {
                        case MKVTools.Title.ConversionResult.Success:
                            lblConversionStatus.Text = "Converted, size: " + String.Format(new FileSizeFormatProvider(), "{0:fs}", Title.Size);
                            break;
                        default:
                            lblConversionStatus.Text = Title.Result.GetDescription() + ".";
                            break;
                    }

                    txtPropertyValue.ReadOnly = (Title.Result == MKVTools.Title.ConversionResult.ConversionInProgress || Title.Result == MKVTools.Title.ConversionResult.Success);

                    btnRetry.Enabled = (Title.Result != MKVTools.Title.ConversionResult.NotAvailable && Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress);
                    btnAbort.Enabled = (Title.Result == MKVTools.Title.ConversionResult.ConversionInProgress);
                }
                else
                {
                    txtPropertyValue.ReadOnly = true;

                    lblConversionStatus.Text = "Not available.";
                    btnAbort.Enabled = false;
                    btnRetry.Enabled = false;
                }
            }
        }

        private void updateTitleInfo(MKVTools.Title Title)
        {
            string segmentMap = "";
            foreach (int segment in Title.Segments)
            {
                if (segmentMap.Length > 0) segmentMap += ", ";
                segmentMap += segment.ToString();
            }

            string textInfo = "";

            if (Title.Name != null && Title.Name.Length > 0)
                textInfo += String.Format("Name: {0}{1}\r\n",
                    Title.Name,
                    (Title.MetadataLanguage != MKVTools.Language.Undetermined ? " (" + Title.MetadataLanguage.GetDescription() + ")" : ""));

            if (Title.SourceFilename != null && Title.SourceFilename.Length > 0)
                textInfo += String.Format("Source file name: {0}\r\n",
                    Title.SourceFilename);

            textInfo += String.Format("Duration: {0}\r\nChapters count: {1}\r\nSize: {2}\r\nSegment count: {3}\r\nSegment map: {4}\r\nFile name: {5}\r\n",
                Title.Duration.ToString(),
                Title.Chapters,
                String.Format(new FileSizeFormatProvider(), "{0:fs}", Title.Size),
                Title.Segments.Length,
                segmentMap,
                Title.OutputFilename);

            if (!String.IsNullOrWhiteSpace(Title.Comment))
                textInfo += String.Format("Comment: {0}\r\n", Title.Comment);

            txtInformation.Text = textInfo;
        }

        private void updateTrackInfo(MKVTools.Track Track)
        {
            string flags = "";
            foreach (MKVTools.StreamFlag flag in Enum.GetValues(typeof(MKVTools.StreamFlag)))
            {
                if (Track.StreamFlags.HasFlag(flag))
                {
                    if (flags.Length > 0)
                        flags += ", " + flag.GetDescription().ToLower();
                    else
                        flags = flag.GetDescription();
                }
            }
            if (flags.Length > 0)
                flags = "\r\nFlags: " + flags;

            string mkvFlags = Track.MKVFlags.Length > 0
                ? String.Format("\r\nMKV flags: {0}", Track.MKVFlags)
                : "";

            switch (Track.TrackType)
            {
                case MKVTools.TrackType.Video:
                    txtInformation.Text = String.Format("Type: {0}{1}\r\nCodec: {2}\r\nResolution: {3}x{4}\r\nAspect ratio: 1:{5}\r\nFrame rate: {6}{7}",
                        Track.TrackType,
                        flags,
                        Track.VideoCodec.GetDescription(),
                        Track.VideoResolution.Width,
                        Track.VideoResolution.Height,
                        Track.VideoAspectRatio,
                        Track.VideoFramerate,
                        mkvFlags);
                    break;
                case MKVTools.TrackType.Audio:
                    txtInformation.Text = String.Format("Type: {0}{1}\r\nName: {2}{3}\r\n{4}Codec: {5}{6}\r\nChannels: {7}\r\nChannel layout: {8}\r\nSample rate: {9}{10}{11}",
                        Track.TrackType,
                        flags,
                        Track.Name,
                        (Track.MetadataLanguage != MKVTools.Language.Undetermined ? " (" + Track.MetadataLanguage.GetDescription() + ")" : ""),
                        (Track.Language != MKVTools.Language.Undetermined ? String.Format("Language: {0}\r\n", Track.Language.GetDescription()) : ""),
                        Track.AudioCodec.GetDescription(),
                        "",
                        Track.AudioChannels,
                        Track.AudioChannelLayout.GetDescription() + (Track.AudioHasObjectAudio ? " " + Track.AudioObjectAudioDescription : ""),
                        Track.AudioSampleRate,
                        Track.AudioBitdepth > 0 ? String.Format("\r\nBits per sample: {0}", Track.AudioBitdepth) : "",
                        mkvFlags);
                    break;
                case MKVTools.TrackType.Subtitle:
                    txtInformation.Text = String.Format("Type: {0}{1}{2}\r\n{3}Codec: {4}{5}",
                        Track.TrackType,
                        flags,
                        (Track.Name != null && Track.Name.Length > 0 ? String.Format("\r\nName: {0} ({1})", Track.Name, Track.MetadataLanguage.GetDescription()) : ""),
                        (Track.Language != MKVTools.Language.Undetermined ? String.Format("Language: {0}\r\n", Track.Language.GetDescription()) : ""),
                        Track.SubtitleCodec.GetDescription(),
                        mkvFlags);
                    break;
                default:
                    txtInformation.Text = "";
                    break;
            }
        }

        private void updatePropertiesUI(byte index)
        {
            if (index < 0 || index > itemProperties.GetUpperBound(0)) return;

            string value = cbxProperties.SelectedItem.ToString();

            // Check if there are properties not currently in list
            foreach(string propName in itemProperties[index])
            {
                if (!cbxProperties.Items.Contains(propName))
                    cbxProperties.Items.Add(propName);
            }

            // Remove properties not applicable
            for (int i = cbxProperties.Items.Count - 1; i >= 0; i--)
            {
                if (!itemProperties[index].Contains(cbxProperties.Items[i].ToString()))
                    cbxProperties.Items.RemoveAt(i);
            }

            // Set selected index
            if (cbxProperties.SelectedItem == null || cbxProperties.SelectedItem.ToString() != value)
            {
                if (cbxProperties.Items.Contains(value))
                    cbxProperties.SelectedItem = value;
                else
                    cbxProperties.SelectedIndex = 0;
            }
        }

        private void readPropertyValue()
        {
            if (currentTitle != null)
                readPropertyValue(currentTitle);
            else if (currentTrack != null)
                readPropertyValue(currentTrack);
        }

        private void readPropertyValue(MKVTools.Title Title)
        {
            switch (cbxProperties.SelectedItem.ToString())
            {
                case "Name":
                    txtPropertyValue.Text = Title.Name; break;
                case "Metadata language":
                    txtPropertyValue.Text = MKVTools.MakeMKV.GetLanguageISOCode(Title.MetadataLanguage); break;
                case "File name":
                    txtPropertyValue.Text = Title.OutputFilename; break;
                default:
                    txtPropertyValue.Text = ""; break;
            }
        }

        private void readPropertyValue(MKVTools.Track Track)
        {
            switch (cbxProperties.SelectedItem.ToString())
            {
                case "Name":
                    txtPropertyValue.Text = Track.Name; break;
                case "Metadata language":
                    txtPropertyValue.Text = MKVTools.MakeMKV.GetLanguageISOCode(Track.MetadataLanguage); break;
                case "Language":
                    txtPropertyValue.Text = MKVTools.MakeMKV.GetLanguageISOCode(Track.Language); break;
                case "MKV flags":
                    txtPropertyValue.Text = Track.MKVFlags; break;
                case "Order weight":
                    txtPropertyValue.Text = Track.OrderWeight.ToString(); break;
                default:
                    txtPropertyValue.Text = ""; break;
            }
        }

        private void savePropertyValue()
        {
            if (currentTitle != null)
                savePropertyValue(currentTitle);
            else if (currentTrack != null)
                savePropertyValue(currentTrack);
        }

        private readonly string validNameRegex = "^[\\w.,:;&%'!?#()\\[\\]\\-_\" ]{0,100}$";
        private readonly string validFilenameRegex = "^[^\\\\/:*?\"<>|]{1,100}\\.mkv$";
        private readonly string validFolderRegex = "^(?:[a-z]:|\\\\\\\\[a-z0-9_.$\\●-]+\\\\[a-z0-9_.$\\●\\-() ,]+)(?:\\\\[^\\\\/:*?\"<>|\\r\\n]+)*(?:\\\\)?$";

        private void savePropertyValue(MKVTools.Title Title)
        {
            bool modified = false;

            switch (cbxProperties.SelectedItem.ToString())
            {
                case "Name":
                    if (!Regex.Match(txtPropertyValue.Text.Trim(), validNameRegex).Success)
                    { MessageBox.Show("The specified title name contains invalid characters.", "Invalid title name", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); return; }
                    else
                    {
                        if (Title.Name == null || !Title.Name.Equals(txtPropertyValue.Text.Trim(), StringComparison.CurrentCulture))
                        { Title.Name = txtPropertyValue.Text.Trim(); modified = true; }
                    }
                    break;
                case "Metadata language":
                    MKVTools.Language lang = MKVTools.MakeMKV.GetLanguage(txtPropertyValue.Text);
                    if (lang == MKVTools.Language.Undetermined)
                    { MessageBox.Show("The specified language ISO code could not be recognized.", "Invalid metadata language ISO code", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); return; }
                    else
                    {
                        if (!Title.MetadataLanguage.Equals(lang))
                        { Title.MetadataLanguage = lang; modified = true; }
                    }
                    break;
                case "File name":
                    if (!Regex.Match(txtPropertyValue.Text.Trim(), validFilenameRegex).Success)
                    { MessageBox.Show("The specified filename is not a valid MKV file name.", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); return; }
                    else
                    {
                        if (Title.OutputFilename == null || !Title.OutputFilename.Equals(txtPropertyValue.Text.Trim(), StringComparison.CurrentCulture))
                        { Title.OutputFilename = txtPropertyValue.Text.Trim(); modified = true; }
                    }
                    break;
            }

            if (modified)
            {
                updateTitleInfo(Title);
                tlvTitles.RefreshObject(Title);
            }
        }

        private void savePropertyValue(MKVTools.Track Track)
        {
            switch (cbxProperties.SelectedItem.ToString())
            {
                case "Name":
                    if (!Regex.Match(txtPropertyValue.Text.Trim(), validNameRegex).Success)
                    { MessageBox.Show("The specified track name contains invalid characters.", "Invalid track name", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); }
                    else
                        Track.Name = txtPropertyValue.Text.Trim();
                    break;
                case "Metadata language":
                    MKVTools.Language metaLang = MKVTools.MakeMKV.GetLanguage(txtPropertyValue.Text);
                    if (metaLang == MKVTools.Language.Undetermined)
                    { MessageBox.Show("The specified language ISO code could not be recognized.", "Invalid metadata language ISO code", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); }
                    else
                        Track.MetadataLanguage = metaLang;
                    break;
                case "Language":
                    MKVTools.Language lang = MKVTools.MakeMKV.GetLanguage(txtPropertyValue.Text);
                    if (lang == MKVTools.Language.Undetermined)
                    { MessageBox.Show("The specified language ISO code could not be recognized.", "Invalid language ISO code", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); }
                    else
                        Track.Language = lang;
                    break;
                case "MKV flags":
                    Track.MKVFlags = txtPropertyValue.Text.Trim().ToLower();
                    break;
                case "Order weight":
                    int weight;
                    if (int.TryParse(txtPropertyValue.Text.Trim(), out weight))
                        if (weight != Track.OrderWeight)
                        {
                            Track.OrderWeight = weight;
                            tlvTitles.Refresh();
                        }
                    else
                    { MessageBox.Show("The specified order weight is not a valid integer value.", "Invalid order weight", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPropertyValue.Focus(); }
                    break;
            }

            updateTrackInfo(Track);
            tlvTitles.RefreshObject(Track);
        }

        private void txtPropertyValue_Leave(object sender, EventArgs e)
        {
            savePropertyValue();
        }

        private void cbxProperties_SelectedValueChanged(object sender, EventArgs e)
        {
            readPropertyValue();
        }

        private void cbxSourceAction_Leave(object sender, EventArgs e)
        {
            saveOutputSettings();
        }

        private void radOutputSpecified_Leave(object sender, EventArgs e)
        {
            saveOutputSettings();
        }

        private void radOutputSource_Leave(object sender, EventArgs e)
        {
            saveOutputSettings();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            MKVTools.Title title = currentTitle != null
                ? currentTitle
                : currentTrack.Title;
            if (title == null)
            { updateConversionStatus(); return; }
            else
            { title.ResetResult(); updateConversionStatus(title); }

            tlvTitles.Refresh();

            if (analyzeAutomatically())
                analyzeNextSource();
            else if (convertAutomatically())
                convertNextTitle();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            MKVTools.Title title = currentTitle != null
                ? currentTitle
                : currentTrack.Title;
            btnAbort.Enabled = false;
            title.AbortConversion();
        }

        private void fMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.WindowStartMaximized = (this.WindowState == FormWindowState.Maximized);

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowStartX = this.Location.X;
                Properties.Settings.Default.WindowStartY = this.Location.Y;
                Properties.Settings.Default.WindowStartWidth = this.Width;
                Properties.Settings.Default.WindowStartHeight = this.Height;
            }

            // Save settings
            Properties.Settings.Default.Save();

        }

        private void tlvTitles_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            if (e.RowObject == null) return;

            // Only tracks can be set Default.
            if (e.RowObject.GetType() != typeof(MKVTools.Track))
            { e.Canceled = true; return; }

            // Save new value to track
            MKVTools.Track t = (MKVTools.Track)e.RowObject;
            if (t.TrackType == MKVTools.TrackType.Video) 
            { e.Canceled = true; return; }
            t.Default = e.NewValue == CheckState.Checked;
            this.tlvTitles.Refresh();
        }

        private void updateRecentFolders()
        {
            // Updates the recent folders menu items to correspond to List<string> recentFolders.
            foreach (ToolStripMenuItem menuItem in toolStripMenuItemScanRecent.DropDownItems)
                menuItem.Click -= recentFolder_Click;
            toolStripMenuItemScanRecent.DropDownItems.Clear();

            if (recentFolders.Count == 0)
                toolStripMenuItemScanRecent.Visible = false;
            else
            {
                ToolStripMenuItem menuItem;
                foreach(string folder in recentFolders)
                {
                    menuItem = new ToolStripMenuItem(folder);
                    menuItem.Click += recentFolder_Click;
                    toolStripMenuItemScanRecent.DropDownItems.Add(menuItem);
                }

                toolStripMenuItemScanRecent.Visible = true;
            }

        }

        private void recentFolder_Click(object sender, EventArgs e)
        {
            scanFolder(sender.ToString());
        }

        private void scanDrivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scanDrivesToolStripMenuItem.Checked = !scanDrivesToolStripMenuItem.Checked;

            if (scanDrivesToolStripMenuItem.Checked)
                scanDriveTimer.Start();
            else
            {
                // Only stop timer if there are currently no drives in source list (otherwise it needs to continue running to monitor if they are ejected).
                if (drivesAvailable.Count == 0)
                    scanDriveTimer.Stop();
            }
        }

        private void txtOutputSpecified_Leave(object sender, EventArgs e)
        {
            saveOutputSettings();
        }

        private void olvSources_CellRightClick(object sender, CellRightClickEventArgs e)
        {
            if (olvSources.Items.Count == 0) return;

            ctmSource_ToggleItem.Visible = (e.Model != null);
            ctmSource_ToggleItemSeparator.Visible = (e.Model != null);

            if (e.Model != null)
            {
                SourceState src = (SourceState)e.Model;

                // Update ctmSource_Priority
                ctmSource_PrioritySeparator.Visible = true;
                ctmSource_Priority.Visible = true;
                ctmSource_Priority.Enabled = (src.CurrentStatus != SourceState.Status.Finished);
                ctmSource_PriorityLow.Checked = (src.Priority == Priority.Low);
                ctmSource_PriorityMedium.Checked = (src.Priority == Priority.Medium);
                ctmSource_PriorityHigh.Checked = (src.Priority == Priority.High);

                // Update ctmSource_Analyze
                if (analyzeAutomatically() && src.CurrentStatus == SourceState.Status.AnalysisFailed)
                {
                    ctmSource_AnalyzeSeparator.Visible = true;
                    ctmSource_Analyze.Visible = true;
                    ctmSource_Analyze.Enabled = makeMKV.Available;
                    ctmSource_Analyze.Text = "Retry analysis";
                }
                else if (!analyzeAutomatically() && (src.CurrentStatus == SourceState.Status.Initialized || src.CurrentStatus == SourceState.Status.AnalysisFailed))
                {
                    ctmSource_AnalyzeSeparator.Visible = true;
                    ctmSource_Analyze.Visible = true;
                    bool inProgress;
                    lock (activityLock)
                    { inProgress = processInProgress != ActivityType.None; }
                    ctmSource_Analyze.Enabled = (!inProgress && makeMKV.Available);
                    ctmSource_Analyze.Text = (src.CurrentStatus == SourceState.Status.Initialized ? "Analyze" : "Retry analysis");
                }
                else
                {
                    ctmSource_AnalyzeSeparator.Visible = false;
                    ctmSource_Analyze.Visible = false;
                }

                // Update ctmSource_Clear
                ctmSource_ClearSeparator.Visible = true;
                ctmSource_Clear.Visible = true;
                ctmSource_Clear.Enabled = (src.CurrentStatus != SourceState.Status.Analyzing && src.CurrentStatus != SourceState.Status.Converting && src.CurrentStatus != SourceState.Status.ArchivingSource && src.CurrentStatus != SourceState.Status.DeletingSource);
                ctmSource_ClearFinished.Visible = false;
                ctmSource_ClearMissing.Visible = false;
                ctmSource_ClearAll.Visible = false;
            }
            else
            {
                ctmSource_PrioritySeparator.Visible = false;
                ctmSource_Priority.Visible = false;

                ctmSource_AnalyzeSeparator.Visible = false;
                ctmSource_Analyze.Visible = false;

                ctmSource_ClearSeparator.Visible = true;
                ctmSource_ClearFinished.Visible = true;
                ctmSource_ClearMissing.Visible = true;
                ctmSource_ClearAll.Visible = true;
                ctmSource_Clear.Visible = false;
            }

            ctmSource_SelectAll.Enabled = (olvSources.CheckedItems.Count < olvSources.Items.Count);
            ctmSource_UnselectAll.Enabled = (olvSources.CheckedItems.Count > 0);

            ctmSource.Show(olvSources, e.Location);
        }

        private void ctmSource_ToggleItem_Click(object sender, EventArgs e)
        {
            if (olvSources.SelectedObjects != null)
            {
                foreach(object o in olvSources.SelectedObjects)
                    olvSources.ToggleCheckObject(o);
            }
        }

        private void ctmSource_SelectAll_Click(object sender, EventArgs e)
        {
            olvSources.CheckAll();
        }

        private void ctmSource_UnselectAll_Click(object sender, EventArgs e)
        {
            olvSources.UncheckAll();
        }

        private void ctmSource_Analyze_Click(object sender, EventArgs e)
        {
            if (olvSources.SelectedObjects != null)
            {
                foreach (SourceState src in olvSources.SelectedObjects)
                {
                    if (analyzeAutomatically() && src.CurrentStatus == SourceState.Status.AnalysisFailed)
                        src.CurrentStatus = SourceState.Status.Initialized;
                    else if (!analyzeAutomatically() && 
                        (src.CurrentStatus == SourceState.Status.Initialized || src.CurrentStatus == SourceState.Status.AnalysisFailed))
                    {
                        analyzeNextSource(src);
                        break;
                    }
                }
            }
        }

        private void tlvTitles_CellRightClick(object sender, CellRightClickEventArgs e)
        {
            if (tlvTitles.Items.Count == 0)
                return;

            MKVTools.Title title;

            if (tlvTitles.SelectedObject == null ||
                tlvTitles.SelectedObject.GetType() == typeof(MKVTools.Title))
            {
                // Show Title context menu.

                // Show toggle item?
                ctmTitle_ToggleItem.Visible = (tlvTitles.SelectedObject != null);
                ctmTitle_ToggleItemSeparator.Visible = (tlvTitles.SelectedObject != null);

                // Is Select/Unselect all enabled?
                int nsel = 0, ntot = 0, ncm = 0;
                foreach(MKVTools.Title t in tlvTitles.Roots)
                {
                    ntot++;
                    if (tlvTitles.IsChecked(t))
                        nsel++;

                    if (t.Result != MKVTools.Title.ConversionResult.ConversionInProgress && t.Result != MKVTools.Title.ConversionResult.Success)
                        ncm++;
                }
                ctmTitle_SelectAll.Enabled = (nsel < ntot);
                ctmTitle_UnselectAll.Enabled = (nsel > 0);
                ctmTitle_ApplyDefaultSelections.Enabled = (ncm > 0);
                ctmTitle_ApplyDefaultSettings.Enabled = (nsel > 0);

                // Is Convert visible? enabled?
                if (tlvTitles.SelectedObject != null)
                {
                    title = (MKVTools.Title)tlvTitles.SelectedObject;

                    ctmTitle_ToggleItem.Enabled = (title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && title.Result != MKVTools.Title.ConversionResult.Success);

                    if (title.Result == MKVTools.Title.ConversionResult.ConversionInProgress)
                    {
                        ctmTitle_ConvertSeparator.Visible = true;
                        ctmTitle_Convert.Visible = false;
                        ctmTitle_AbortConversion.Visible = true;
                    }
                    else if (convertAutomatically() && title.Result != MKVTools.Title.ConversionResult.NotAvailable && title.Result != MKVTools.Title.ConversionResult.ConversionInProgress)
                    {
                        ctmTitle_ConvertSeparator.Visible = true;
                        ctmTitle_Convert.Visible = true;
                        ctmTitle_Convert.Enabled = (makeMKV.Available && mkvToolNix.Available);
                        ctmTitle_Convert.Text = "Retry conversion";
                        ctmTitle_AbortConversion.Visible = false;
                    }
                    else if (!convertAutomatically())
                    {
                        ctmTitle_ConvertSeparator.Visible = true;
                        ctmTitle_Convert.Visible = true;
                        bool inProgress;
                        lock (activityLock)
                        { inProgress = processInProgress != ActivityType.None; }
                        ctmTitle_Convert.Enabled = (!inProgress && title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && makeMKV.Available && mkvToolNix.Available);
                        ctmTitle_Convert.Text = ((title.Result == MKVTools.Title.ConversionResult.NotAvailable || title.Result == MKVTools.Title.ConversionResult.ConversionInProgress) ? "Convert" : "Retry conversion");
                        ctmTitle_AbortConversion.Visible = false;
                    }
                    else
                    {
                        ctmTitle_ConvertSeparator.Visible = false;
                        ctmTitle_Convert.Visible = false;
                        ctmTitle_AbortConversion.Visible = false;
                    }
                }
                else
                {
                    ctmTitle_ConvertSeparator.Visible = false;
                    ctmTitle_Convert.Visible = false;
                    ctmTitle_AbortConversion.Visible = false;
                }

                ctmTitle.Show(tlvTitles, e.Location);
            }
            else if (tlvTitles.SelectedObject != null &&
                tlvTitles.SelectedObject.GetType() == typeof(MKVTools.Track))
            {
                // Show Track context menu.

                // Enable/disable Select/Unselect all based on track availability
                int nac = 0, nat = 0, nsc = 0, nst = 0;
                title = ((MKVTools.Track)tlvTitles.SelectedObject).Title;
                foreach(MKVTools.Track tm in title.Tracks)
                {
                    foreach (MKVTools.Track t in new MKVTools.Track[] { tm, tm.Child })
                    {
                        if (t == null) continue;

                        switch (t.TrackType)
                        {
                            case MKVTools.TrackType.Audio:
                                nat++;
                                if (t.Include) nac++;
                                break;
                            case MKVTools.TrackType.Subtitle:
                                nst++;
                                if (t.Include) nsc++;
                                break;
                        }
                    }
                }

                ctmTrack_SelectAll.Enabled = (nac + nsc < nat + nst);
                ctmTrack_SelectAllAudio.Enabled = (nac < nat);
                ctmTrack_SelectAllSubtitle.Enabled = (nsc < nst);
                ctmTrack_UnselectAll.Enabled = (nac + nsc > 0);
                ctmTrack_UnselectAllAudio.Enabled = (nac > 0);
                ctmTrack_UnselectAllSubtitle.Enabled = (nsc > 0);

                ctmTrack_UseTrackDefaults.Enabled = (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success);

                ctmTrack.Show(tlvTitles, e.Location);
            } 
        }

        private void ctmTitle_ToggleItem_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            tlvTitles.ToggleCheckObject(tlvTitles.SelectedObject);
            tlvTitles.Unfreeze();
        }

        private void ctmTitle_SelectAll_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            foreach(MKVTools.Title title in tlvTitles.Roots)
                tlvTitles.CheckObject(title);
            tlvTitles.Unfreeze();
        }

        private void ctmTitle_UnselectAll_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            foreach (MKVTools.Title title in tlvTitles.Roots)
                tlvTitles.UncheckObject(title);
            tlvTitles.Unfreeze();
        }

        private void ctmTitle_AbortConversion_Click(object sender, EventArgs e)
        {
            if (currentTitle != null)
                currentTitle.AbortConversion();
        }

        private void ctmTitle_Convert_Click(object sender, EventArgs e)
        {
            if (tlvTitles.SelectedObjects != null)
            {
                foreach (MKVTools.Title title in tlvTitles.SelectedObjects)
                {
                    if (convertAutomatically() && title.Result != MKVTools.Title.ConversionResult.NotAvailable && title.Result != MKVTools.Title.ConversionResult.ConversionInProgress)
                    {
                        title.ResetResult();
                        if (title == currentTitle)
                            updateConversionStatus(title);
                        processNext();
                    }
                    else if (!convertAutomatically() &&
                        title.Result != MKVTools.Title.ConversionResult.ConversionInProgress)
                    {
                        convertNextTitle(title);
                        if (title == currentTitle)
                            updateConversionStatus(title);
                        break;
                    }
                }
            }

        }

        private void ctmTrack_UseTrackDefaults_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                streamAgent.ApplyDefaults(currentTrack.Title.Tracks);
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_UnselectAllSubtitle_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    if (track.TrackType == MKVTools.TrackType.Subtitle)
                    {
                        track.Include = false;
                        if (track.Child != null) track.Child.Include = false;
                    }
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_UnselectAllAudio_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    if (track.TrackType == MKVTools.TrackType.Audio)
                    {
                        track.Include = false;
                        if (track.Child != null) track.Child.Include = false;
                    }
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_UnselectAll_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    track.Include = false;
                    if (track.Child != null) track.Child.Include = false;
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_SelectAllSubtitle_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    if (track.TrackType == MKVTools.TrackType.Subtitle)
                    {
                        track.Include = true;
                        if (track.Child != null) track.Child.Include = true;
                    }
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_SelectAllAudio_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    if (track.TrackType == MKVTools.TrackType.Audio)
                    {
                        track.Include = true;
                        if (track.Child != null) track.Child.Include = true;
                    }
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTrack_SelectAll_Click(object sender, EventArgs e)
        {
            if (currentTrack != null && currentTrack.Title.Result != MKVTools.Title.ConversionResult.ConversionInProgress && currentTrack.Title.Result != MKVTools.Title.ConversionResult.Success)
            {
                tlvTitles.Freeze();
                foreach (MKVTools.Track track in currentTrack.Title.Tracks)
                {
                    track.Include = true;
                    if (track.Child != null) track.Child.Include = true;
                }
                tlvTitles.Unfreeze();
                tlvTitles.Refresh();
            }
        }

        private void ctmTitle_ApplyDefaultSettings_Click(object sender, EventArgs e)
        {
            if (currentSource != null)
            {
                if (currentTitle != null)
                {
                    titleAgent.ApplyDefaultSettings(currentTitle);
                    updateTitleInfo(currentTitle);
                    readPropertyValue(currentTitle);
                }
                else
                    titleAgent.ApplyDefaultSettings(currentSource.Titles);

                tlvTitles.Refresh();
            }
        }

        private void ctmTitle_ExpandAll_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            tlvTitles.ExpandAll();
            tlvTitles.Unfreeze();
        }

        private void ctmTitle_CollapseAll_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            tlvTitles.ExpandAll();
            foreach (MKVTools.Title title in tlvTitles.Roots) tlvTitles.Collapse(title);
            tlvTitles.Unfreeze();
        }

        private void ctmTitle_ExpandSelected_Click(object sender, EventArgs e)
        {
            tlvTitles.Freeze();
            foreach (MKVTools.Title title in tlvTitles.Roots)
            {
                if (tlvTitles.IsChecked(title))
                    tlvTitles.Expand(title);
            }
            tlvTitles.Unfreeze();
        }

        private void clearFinishedSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearFinishedSources();
        }

        private void clearMissingSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearMissingSources();
        }

        private void clearAllSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearAllSources();
        }

        private void ctmTitle_UseTitleDefaults_Click(object sender, EventArgs e)
        {
            // if (currentSource != null && currentSource.Result != MKVTools.Source.ScanResult.)

            if (currentSource != null)
            {
                titleAgent.ApplyDefaultSelections(currentSource.Titles);
                if (currentTitle != null) updateTitleInfo(currentTitle);
                showSourceDetails(currentSource);
            }

        }

        private bool holdProcessing = false;

        private void clearSource(SourceState source)
        {
            holdProcessing = true;
            clearSources(new List<SourceState>() { source });
            holdProcessing = false;
        }

        private void clearMissingSources()
        {
            holdProcessing = true;

            List<SourceState> clear = new List<SourceState>();
            foreach (SourceState source in olvSources.Objects)
                if (!source.IsAvailable)
                    clear.Add(source);

            clearSources(clear);

            holdProcessing = false;
        }

        private void clearFinishedSources()
        {
            holdProcessing = true;

            // Clears all sources with status 'Finished'.
            List<SourceState> clear = new List<SourceState>();
            foreach (SourceState source in olvSources.Objects)
                if (source.CurrentStatus == SourceState.Status.Finished)
                    clear.Add(source);

            clearSources(clear);

            holdProcessing = false;
        }

        private void clearAllSources()
        {
            holdProcessing = true;

            // Clears all sources where an operation is not currently in progress.
            List<SourceState> clear = new List<SourceState>();
            foreach (SourceState source in olvSources.Objects)
                if (source.CurrentStatus != SourceState.Status.Analyzing &&
                    source.CurrentStatus != SourceState.Status.ArchivingSource &&
                    source.CurrentStatus != SourceState.Status.Converting &&
                    source.CurrentStatus != SourceState.Status.DeletingSource)
                    clear.Add(source);

            clearSources(clear);

            holdProcessing = false;
        }

        private void ctmSource_PriorityLow_Click(object sender, EventArgs e)
        {
            setSourcePriority(Priority.Low);
        }

        private void ctmSource_PriorityMedium_Click(object sender, EventArgs e)
        {
            setSourcePriority(Priority.Medium);
        }

        private void ctmSource_PriorityHigh_Click(object sender, EventArgs e)
        {
            setSourcePriority(Priority.High);
        }

        private void setSourcePriority(Priority priority)
        {
            if (olvSources.SelectedObjects != null)
                foreach (SourceState src in olvSources.SelectedObjects)
                    src.Priority = priority;

            olvSources.RefreshSelectedObjects();
        }

        private void ctmSource_Clear_Click(object sender, EventArgs e)
        {
            if (olvSources.SelectedItem == null) return;

            SourceState src = (SourceState)olvSources.SelectedItem.RowObject;
            clearSource(src);
        }

        private void ctmSource_ClearFinished_Click(object sender, EventArgs e)
        {
            clearFinishedSources();
        }

        private void ctmSource_ClearMissing_Click(object sender, EventArgs e)
        {
            clearMissingSources();
        }

        private void ctmSource_ClearAll_Click(object sender, EventArgs e)
        {
            clearAllSources();
        }

        private void troubleshootingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsToolStripMenuItem_Click(sender, e);
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=Z84KBB8HS496U");
        }

        private void olvSources_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None);

            /*DragDropEffects effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                bool isFolders = true;
                foreach (string target in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (!Directory.Exists(target))
                    {
                        isFolders = false;
                        break;
                    }
                }

                if (isFolders) effect = DragDropEffects.Link;
            }

            e.Effect = effect;*/
        }

        private void olvSources_DragDrop(object sender, DragEventArgs e)
        {
            foreach(string target in (string[])e.Data.GetData(DataFormats.FileDrop))
                queueFolderScan(target);
            scanNextItem();
        }

        private void clearSources(List<SourceState> sources)
        {
            if (sources == null || sources.Count == 0) return;

            // Store treeview state if a source is currently showing
            if (currentSource != null && currentSource.Tag != null)
            {
                SourceState src = (SourceState)currentSource.Tag;
                if (sources.Contains(src))
                {
                    storeTreeViewState(src);
                    sourceStateRepository.Update(src);
                }
            }

            // Do not restore cleared sources on start
            foreach (SourceState sst in sources)
            {
                sst.RestoreOnStart = false;
                sourceStateRepository.Update(sst);
            }

            try { olvSources.RemoveObjects(sources); }
            catch { }
        }

    }

    public class OutputSettings
    {
        // The properties can be set as desired and are not limited.
        // Use the Get methods to get the applicable values for a specific source.

        public bool OutputAtSource { get; set; }        // True if output should be placed in same folder as source.

        public string OutputPath { get; set; }          // Output folder to use if !OutputSource.

        public SourceTarget SourceAction { get; set; }  // What to do with the source after successful conversion.

        public OutputSettings()
        {
            OutputAtSource = true;
            OutputPath = String.Empty;
            SourceAction = SourceTarget.Archive;
        }

        public enum SourceTarget : byte
        {
            Keep = 0,
            Archive = 1,
            Delete = 2
        }

        public bool GetOutputAtSource(MKVTools.Source Source)
        {
            return (OutputAtSource && (Source.Type == MKVTools.SourceType.File || Source.Type == MKVTools.SourceType.Folder));
        }

        public string GetOutputPath(MKVTools.Source Source)
        {
            // If OutputPath is null, String.Empty or .Length == 0, OutputPath should be same as for OutputAtSource == true.
            if (OutputAtSource || String.IsNullOrEmpty(OutputPath))
            {
                // OutputAtSource
                switch(Source.Type)
                {
                    case MKVTools.SourceType.File:
                    case MKVTools.SourceType.Folder:
                        int n = Source.Location.LastIndexOf('\\');
                        return (n > 0 && n < Source.Location.Length
                            ? Source.Location.Substring(0, n)
                            : "");
                    default:
                        return "";
                }
            }
            else
                return MKVTools.Title.GetOutputFolderByTemplate(Source, OutputPath);
        }

        public override bool Equals(object obj)
        {
            OutputSettings comp = obj as OutputSettings;
            return (comp == null 
                ? false 
                : (this.OutputAtSource.Equals(comp.OutputAtSource) &&
                    this.OutputPath.Equals(comp.OutputPath) &&
                    this.SourceAction.Equals(comp.SourceAction)));
        }
    }


    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }

        private const string fileSizeFormat = "fs";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat))
            {
                return defaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            Decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = " GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = " MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = " kB";
            }
            else
            {
                suffix = " B";
            }

            string precision = format.Substring(2);
            if (String.IsNullOrEmpty(precision)) precision = "2";
            return String.Format("{0:N" + precision + "}{1}", size, suffix);

        }

        private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) return "";

            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return arg.ToString();
        }

    }

}
