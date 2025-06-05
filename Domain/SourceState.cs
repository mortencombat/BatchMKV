using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKVTools;
using System.IO;
using System.ComponentModel;

namespace BatchMKV.Domain
{
    public enum Priority : byte
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class SourceState
    {
        // Database ID
        public virtual long ID { get; set; }

        // Source unique hash identifier
        private string hash;
        public virtual string Hash
        {
            get
            {
                return (Source != null && Source.Hash != null)
                    ? Source.Hash
                    : hash;
            }
            set
            {
                if (Source == null) hash = value;
            }
        }

        // If true this source will be loaded/restored on application start
        public virtual bool RestoreOnStart { get; set; }

        public virtual bool Convert { get; set; }

        public virtual DateTime LastUpdated { get; set; }

        // MKVTools.Source
        public virtual Source Source { get; set; }

        #region Output settings

        public virtual OutputSettings OutputSettings { get; set; }

        public virtual bool OutputSettingsUseDefault
        {
            get { return (OutputSettings == null); }
            set
            {
                if (!value && OutputSettings == null)
                    OutputSettings = new OutputSettings();
                else if (value && OutputSettings != null)
                    OutputSettings = null;
            }
        }

        public virtual bool OutputSettingsOutputAtSource
        {
            get
            {
                return (OutputSettings != null
                    ? OutputSettings.OutputAtSource
                    : true);
            }
            set
            {
                if (OutputSettings == null) OutputSettings = new OutputSettings();
                OutputSettings.OutputAtSource = value;
            }
        }

        public virtual string OutputSettingsOutputPath
        {
            get
            {
                return (OutputSettings != null
                    ? OutputSettings.OutputPath
                    : null);
            }
            set
            {
                if (OutputSettings == null) OutputSettings = new OutputSettings();
                OutputSettings.OutputPath = value;
            }
        }

        public virtual OutputSettings.SourceTarget OutputSettingsSourceAction
        {
            get
            {
                return (OutputSettings != null
                    ? OutputSettings.SourceAction
                    : OutputSettings.SourceTarget.Keep);
            }
            set
            {
                if (OutputSettings == null) OutputSettings = new OutputSettings();
                OutputSettings.SourceAction = value;
            }
        }

        #endregion

        #region Data for re-initializing Source

        public virtual DiscType DiscType { get; set; }

        private string discName = null;

        public virtual string DiscNameActual
        {
            get
            {
                return (string.IsNullOrWhiteSpace(discName) ? null : discName);
            }
        }

        public virtual string DiscName
        {
            get
            {
                // Combine priority and discName
                string priorityChar;
                switch(Priority)
                {
                    case Priority.Low:
                        priorityChar = "¹";
                        break;
                    case Priority.High:
                        priorityChar = "³";
                        break;
                    default:
                        priorityChar = "²";
                        break;
                }

                return priorityChar + (string.IsNullOrWhiteSpace(discName) ? "" : discName);
            }
            set
            {
                // Get priority
                // ¹ = low priority
                // ² = medium priority
                // ³ = high priority

                if (string.IsNullOrWhiteSpace(value))
                {
                    discName = null;
                }
                else if (value.StartsWith("¹"))
                {
                    Priority = Priority.Low;
                    discName = (value.Length > 1 ? value.Substring(1).Trim() : null);
                }
                else if (value.StartsWith("²"))
                {
                    Priority = Priority.Medium;
                    discName = (value.Length > 1 ? value.Substring(1).Trim() : null);
                }
                else if (value.StartsWith("³"))
                {
                    Priority = Priority.High;
                    discName = (value.Length > 1 ? value.Substring(1).Trim() : null);
                }
                else
                {
                    discName = value.Trim();
                }
            }
        }

        public virtual Language DiscMetadataLanguage { get; set; }

        public virtual Priority Priority { get; set; } = Priority.Medium;

        public virtual ICollection<TitleState> Titles { get; set; }

        #endregion

        #region Source location type and location

        private LocationType locationType;
        public virtual LocationType LocationType
        {
            get { return locationType; }
            set { locationType = value; }
        }

        public virtual SourceType SourceType
        {
            get
            {
                switch (locationType)
                {
                    case LocationType.File:
                        return SourceType.File;
                    case LocationType.Drive:
                        return SourceType.Disc;
                    default:
                        return SourceType.Folder;
                }
            }
        }

        private string location;
        private DriveInfo drive = null;

        public virtual string Location
        {
            get { return (locationType != LocationType.Drive ? location : drive.Name); }
            set {
                if (locationType == LocationType.Drive)
                {
                    try { drive = new DriveInfo(value.Trim().Substring(0, 1)); }
                    catch { throw new Exception("Location not valid for Drive type."); }
                }
                else
                    location = value;

                Initialize();
            }
        }

        public virtual DriveInfo Drive {
            get { return drive; }
        }

        public virtual bool IsAvailable
        {
            get
            {
                // Returns true if source file/folder exists (does not check the full folder structure, only key file(s))
                switch(SourceType)
                {
                    case SourceType.File:
                        return File.Exists(Location);
                    case SourceType.Disc:
                        return drive.IsReady;
                    case SourceType.Device:
                        return false;
                    default:
                        string keyFile = location;
                        if (keyFile.EndsWith("\\")) keyFile = keyFile.TrimEnd('\u005c');
                        switch(LocationType)
                        {
                            case LocationType.FolderBluray:
                                keyFile += "\\BDMV\\index.bdmv"; break;
                            case LocationType.FolderDVD:
                                keyFile += "\\VIDEO_TS\\VIDEO_TS.ifo"; break;
                            default:
                                return false;
                        }
                        return File.Exists(keyFile);
                }
            }
        }

        #endregion

        #region Source basic info (title, file extension, size, type description) - for source list view etc.

        private string fileExtension;
        public virtual string FileExtension { get { return fileExtension; } }

        private ulong size = 0;
        public virtual ulong Size
        {
            get
            {
                if (locationType == LocationType.Drive)
                {
                    try { size = (ulong)drive.TotalSize; }
                    catch { size = 0; }
                }

                return size;
            }
        }

        private string title = null;
        public virtual string Title
        {
            get
            {
                if (locationType == LocationType.Drive)
                {
                    try
                    {
                        title = drive.VolumeLabel.Trim();
                        if (title.Length == 0) title = drive.Name;
                    }
                    catch
                    {
                        title = drive.Name;
                    }
                }

                return title;
            }
        }

        private string type = null;
        public virtual string Type { get { return type; } }

        #endregion

        #region Source status

        public virtual Status CurrentStatus { get; set; }

        private bool sourceMoved;
        public virtual bool SourceMoved { get { return sourceMoved; } set { sourceMoved = value; } }

        private bool sourceDeleted;
        public virtual bool SourceDeleted { get { return sourceDeleted; } set { sourceDeleted = value; } }

        public enum Status : short
        {
            [Description("Analysis failed")]
            AnalysisFailed = -10,
            [Description("Conversion failed")]
            ConversionFailed = -9,
            [Description("Archive source failed")]
            ArchiveSourceFailed = -8,
            [Description("Delete source failed")]
            DeleteSourceFailed = -7,

            [Description("Discovered")]
            Initialized = 0,

            [Description("Analyzing..")]
            Analyzing = 1,
            Analyzed = 2,
            [Description("Converting..")]
            Converting = 3,
            [Description("Waiting..")]
            Waiting = 4,
            [Description("Archiving source..")]
            ArchivingSource = 5,
            [Description("Deleting source..")]
            DeletingSource = 6,
            Finished = 7
        }

        #endregion

        #region Instantiation and initialization

        public SourceState()
        {
            this.CurrentStatus = Status.Initialized;
        }

        public SourceState(LocationType Type, string Location)
        {
            this.CurrentStatus = Status.Initialized;
            this.LocationType = Type;
            this.Location = Location;

            Initialize();
        }

        public virtual SourceMatch Matches(string Hash, LocationType LocationType, string Location)
        {
            if (this.Hash == null || Hash == null || !this.Hash.Equals(Hash))
                return SourceMatch.NotMatch;
            else
                return (this.LocationType.Equals(LocationType) && this.Location.Equals(Location, StringComparison.InvariantCultureIgnoreCase))
                    ? SourceMatch.ExactMatch
                    : SourceMatch.HashMatch;
        }

        public enum SourceMatch : byte
        {
            NotMatch = 0,       // Hash does not match
            HashMatch = 1,      // Hash matches, location does not
            ExactMatch = 2      // Both hash and location matches
        }

        public virtual void InitializeSource(MakeMKV MakeMKV, MKVToolNix MKVToolNix = null, bool ForceRescan = false)
        {
            string loc;
            switch (LocationType)
            {
                case LocationType.FolderDVD:
                    loc = Location;
                    if (loc.EndsWith("\\")) loc = loc.TrimEnd('\u005c');
                    loc += "\\VIDEO_TS";
                    break;
                default:
                    loc = Location;
                    break;
            }

            if (!ForceRescan && Titles != null && Titles.Count > 0)
            {
                // Initialize restored.

                // Instantiate Disc class/parameter
                Disc disc = new Disc(DiscType, DiscNameActual, DiscMetadataLanguage, null);

                // Assemble ScanInfo
                List<string> scanInfo = new List<string>();
                foreach (TitleState title in Titles)
                    scanInfo.Add(title.ScanInfo);

                Source = new Source(MakeMKV, SourceType, loc, disc, scanInfo, MKVToolNix);

                // Switch TitleStates and TrackStates to use live values (from MKVTools.Title/Track objects. 
                // This will copy properties TitleState -> MKVTools.Title, TrackState -> MKVTools.Track.
                // Afterwards, TitleState and TrackState will get live values from MKVTools.Title/Track.
                foreach(TitleState title in Titles)
                {
                    title.UseStaticValues = false;
                    foreach (TrackState track in title.Tracks)
                        track.UseStaticValues = false;
                }
            }
            else
            {
                // Initialize unscanned source.
                Source = new Source(MakeMKV, SourceType, loc, MKVToolNix);
            }

            Source.Tag = this;
        }

        public virtual void PrepareScanned()
        {
            // Use when Source has just been scanned, will initialize and link TitleState and TrackState objects.

            DiscName = Source.Disc.Name;
            DiscType = Source.Disc.DiscType;
            DiscMetadataLanguage = Source.Disc.MetadataLanguage;

            Titles = new List<TitleState>();
            TitleState titleState;
            TrackState trackState;
            foreach(Title title in Source.Titles)
            {
                titleState = new TitleState();
                Titles.Add(titleState);

                // Link TitleState to Title object
                titleState.TitleIndex = title.Index;
                titleState.Source = this;
                titleState.UseStaticValues = false;
                titleState.Expanded = title.Include;

                titleState.Tracks = new List<TrackState>();
                foreach(Track track in title.TracksAll)
                {
                    trackState = new TrackState();
                    titleState.Tracks.Add(trackState);

                    // Link TrackState to Track object (via Title object)
                    trackState.TrackIndex = track.Index;
                    trackState.Title = titleState;
                    trackState.UseStaticValues = false;
                    trackState.Expanded = true;
                }
            }

            RestoreOnStart = Properties.Settings.Default.RestoreSources;
        }

        public virtual bool Initialize()
        {
            FileInfo fi;
            switch (locationType)
            {
                case LocationType.File:
                    // Get FileInfo
                    try { fi = new FileInfo(location); }
                    catch { return false; }
                    if (!fi.Exists) return false;

                    location = fi.FullName;
                    fileExtension = fi.Extension.ToUpper();
                    title = fi.Name.Substring(0, fi.Name.Length - fileExtension.Length);
                    if (fileExtension.StartsWith(".")) fileExtension = fileExtension.Substring(1);
                    size = (ulong)fi.Length;
                    type = fileExtension + " file";

                    break;
                case LocationType.Drive:
                    // Get DriveInfo
                    type = "Optical drive";

                    if (location != null && drive == null)
                    {
                        try { drive = new DriveInfo(location.Trim().Substring(0, 1)); }
                        catch { return false; }
                    }
                    
                    break;
                default:
                    int idx = location.LastIndexOf('\\');
                    title = (idx >= 0 ? location.Substring(idx + 1) : location);
                    switch (LocationType)
                    {
                        case LocationType.FolderBluray: type = "Blu-ray folder"; break;
                        case LocationType.FolderDVD: type = "DVD folder"; break;
                        case LocationType.FolderHDDVD: type = "HD-DVD folder"; break;
                    }

                    size = 0;
                    string[] files;
                    try { files = Directory.GetFiles(location, "*.*", SearchOption.AllDirectories); }
                    catch { return false; }
                    if (files == null || files.GetLength(0) == 0) return false;

                    foreach (string f in files)
                    {
                        fi = new FileInfo(f);
                        size += (ulong)fi.Length;
                    }

                    break;
            }

            return true;
        }

        public virtual bool CopySettings(SourceState Target)
        {
            // Copies source output settings, title and track selections and properties to Target.
            // (matching hashes should be enough to be sure that titles and tracks match, but we check collection/array sizes just to be sure)
            if (Hash != Target.Hash || Titles.Count != Target.Titles.Count) return false;

            // Output settings
            Target.OutputSettings = this.OutputSettings;

            // Title settings (includes track settings)
            for(int i = 0; i < Titles.Count; i++)
                if (!Titles.ElementAt(i).CopySettings(Target.Titles.ElementAt(i)))
                    return false;

            return true;
        }

        #endregion

    }

    public enum LocationType : byte
    {
        File = 1,
        FolderBluray = 2,
        FolderDVD = 3,
        FolderHDDVD = 4,
        Drive = 5
    }

    public class SourceStateComparer : IEqualityComparer<SourceState>
    {
        // Compares SourceState.Hash only (Location is not taken into account)

        public bool Equals(SourceState obj1, SourceState obj2)
        {
            return obj1.Hash.Equals(obj2.Hash);
        }

        public int GetHashCode(SourceState obj)
        {
            return obj.Hash.GetHashCode();
        }
    }

    public class SourceStateExactComparer : IEqualityComparer<SourceState>
    {
        // Compares SourceState.Hash and Location.

        public bool Equals(SourceState obj1, SourceState obj2)
        {
            return (obj1.Hash.Equals(obj2.Hash) && 
                obj1.LocationType.Equals(obj2.LocationType) &&
                obj1.Location.Equals(obj2.Location, StringComparison.InvariantCultureIgnoreCase));
        }

        public int GetHashCode(SourceState obj)
        {
            return String.Format("{0};{1};{2}", obj.Hash, obj.LocationType.ToString(), obj.Location).GetHashCode();
        }
    }

}
