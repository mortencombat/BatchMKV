using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using EventLogger;

namespace MKVTools
{
    public class MKVToolNix
    {

        public MKVToolNix()
        {
            
        }

        private Version version;
        public Version Version
        {
            get
            {
                if (version == null) version = getVersion();
                return version;
            }
        }

        private bool version64Bit;
        public bool Version64Bit
        {
            get
            {
                if (version == null) version = getVersion();
                return version64Bit;
            }
        }

        private Version getVersion()
        {
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = this.MKVToolNixPath + "mkvinfo.exe";
                process.StartInfo.Arguments = "--version";

                if (process.Start())
                {
                    string ver = process.StandardOutput.ReadToEnd();
                    Match match = Regex.Match(ver, "mkvinfo v([0-9]+)\\.([0-9]+)\\.([0-9]+) .+ ((?:64)|(?:32))-bit");
                    version = new Version(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
                    version64Bit = (match.Groups[4].Value == "64");
                    return version;
                }
                else
                {
                    return new Version(19, 0, 0);
                }
            }
        }

        private object lockProperties = new object();

        private IEventLogger eventLogger = null;
        public IEventLogger EventLogger
        {
            get { return eventLogger; }
            set { eventLogger = value; }
        }

        private string mkvToolNixPath = @"C:\Program Files\MKVToolNix";
        public string MKVToolNixPath
        {
            get { lock (lockProperties) return mkvToolNixPath; }
            set
            {
                if (value == null) return;

                string path = value.Trim();
                if (!path.EndsWith("\\")) path += "\\";

                lock (lockProperties)
                {
                    mkvToolNixPath = path;
                    version = null;
                }
            }
        }

        public bool Available
        {
            get
            {
                lock (lockProperties)
                {
                    return (File.Exists(mkvToolNixPath + "mkvinfo.exe") && File.Exists(mkvToolNixPath + "mkvpropedit.exe"));
                }
            }
        }

    }

    public class MKVFile
    {

        private MKVToolNix mkvToolNix;
        public MKVFile(MKVToolNix MKVToolNix)
        {
            this.mkvToolNix = MKVToolNix;
        }

        private string path;
        public OperationResult Open(string Path)
        {
            // Read title info and tracks from MKV file using mkvinfo.
            using (Process process = new Process())
            {
                // Redirect the output stream of the child process.
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = mkvToolNix.MKVToolNixPath + "mkvinfo.exe";
                process.StartInfo.Arguments = "--ui-language en " + String.Format("\"{0}\"", Path);

                if (!process.Start())
                {
                    if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvinfo failed to execute.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Warning);
                    return OperationResult.CouldNotStartMKVInfo;
                }

                // Read all output from mkvinfo
                string info = process.StandardOutput.ReadToEnd();

                // Wait for process to exit, attempt to kill it if necessary.
                Thread.Sleep(500);
                byte b = 0;
                while (!process.HasExited && b < 6)
                {
                    try { process.Kill(); }
                    catch { }
                    Thread.Sleep(500);
                    b++;
                }
                process.Close();

                string[] lines;
                lines = info.Split(new[] { "\r\n", "\r", "\n", "|" }, StringSplitOptions.RemoveEmptyEntries);

                mkvInfoSection currentSection = mkvInfoSection.Ignore;
                bool trackInfoStarted = false;
                uint trackIndex = 0;
                ulong trackUID = 0;
                TrackType trackType = TrackType.Video;
                Track.MKVFlags trackFlags = 0;
                string trackCodecID = "", trackName = "";
                Language trackLanguage = Language.Undetermined;

                Match match;

                this.title = "";
                List<Track> tr = new List<Track>();

                string lineWork;

                foreach (string line in lines)
                {
                    lineWork = line.Trim(); // (mkvToolNix.Version.Major < 20 ? line.TrimStart(new[] { '|', ' ' }) : line.Trim());
                    if (lineWork.Contains("EmblVoid")) lineWork = "+ EmblVoid";
                    Debug.WriteLine(lineWork);

                    // Check if this line is section switching:
                    switch (lineWork)
                    {
                        case "+ Segment information":
                            if (currentSection == mkvInfoSection.SegmentTracks && trackInfoStarted)
                            { tr.Add(new Track(this, trackUID, trackIndex, trackType, trackLanguage, trackName, trackFlags, trackCodecID)); trackInfoStarted = false; }
                            currentSection = mkvInfoSection.SegmentInfo;
                            break;
                        case "+ Segment tracks":
                        case "+ Tracks":
                            currentSection = mkvInfoSection.SegmentTracks;
                            break;
                        case "+ Chapters":
                            if (currentSection == mkvInfoSection.SegmentTracks && trackInfoStarted)
                            { tr.Add(new Track(this, trackUID, trackIndex, trackType, trackLanguage, trackName, trackFlags, trackCodecID)); trackInfoStarted = false; }
                            currentSection = mkvInfoSection.Chapters;
                            break;
                        case "+ EbmlVoid":
                            if (currentSection == mkvInfoSection.SegmentTracks && trackInfoStarted)
                            { tr.Add(new Track(this, trackUID, trackIndex, trackType, trackLanguage, trackName, trackFlags, trackCodecID)); trackInfoStarted = false; }
                            break;
                        case "+ Cluster":
                            if (currentSection == mkvInfoSection.SegmentTracks && trackInfoStarted)
                            { tr.Add(new Track(this, trackUID, trackIndex, trackType, trackLanguage, trackName, trackFlags, trackCodecID)); trackInfoStarted = false; }
                            currentSection = mkvInfoSection.Ignore;
                            break;
                        default:
                            // Not section switching
                            switch(currentSection)
                            {
                                case mkvInfoSection.SegmentInfo:
                                    // Look for: title info
                                    if (this.title == "" && (match = Regex.Match(lineWork, "^\\+ Title: ([\\S ]{1,100})$")).Success)
                                    {
                                        this.titleCurrent = match.Groups[1].Value;
                                        this.title = this.titleCurrent;
                                    }
                                    else if ((match = Regex.Match(lineWork, "^\\+ Duration: ([0-9]{1,10}).[0-9]{1,5}s")).Success)
                                    {
                                        this.duration = new TimeSpan(0, 0, int.Parse(match.Groups[1].Value));
                                    }
                                    break;
                                case mkvInfoSection.SegmentTracks:
                                    // Look for: track info

                                    if (mkvToolNix.Version.Major < 20 && lineWork.Equals("+ A track") || lineWork.Equals("+ Track"))
                                    {
                                        if (trackInfoStarted)
                                            tr.Add(new Track(this, trackUID, trackIndex, trackType, trackLanguage, trackName, trackFlags, trackCodecID));
                                        else
                                            trackInfoStarted = true;

                                        trackUID = 0;
                                        trackName = null;
                                        trackCodecID = null;
                                        trackFlags = Track.MKVFlags.Enabled;
                                        trackLanguage = Language.Undetermined;
                                        trackType = TrackType.Video;
                                    }
                                    else if (trackInfoStarted)
                                    {
                                        // Track info does neither start nor end, look for: track info
                                        if ((match = Regex.Match(lineWork, "^\\+ Track number: ([1-9][0-9]{0,9})")).Success)
                                        {
                                            trackIndex = (uint)(int.Parse(match.Groups[1].Value) - 1);
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Track UID: ([1-9][0-9]{0,9})$")).Success)
                                        {
                                            trackUID = (ulong)long.Parse(match.Groups[1].Value);
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Name: ([\\S ]{1,100})$")).Success)
                                        {
                                            trackName = match.Groups[1].Value;
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Track type: (video|audio|subtitles)$")).Success)
                                        {
                                            trackType = (match.Groups[1].Value == "audio"
                                                ? TrackType.Audio
                                                : (match.Groups[1].Value == "subtitles"
                                                    ? TrackType.Subtitle
                                                    : TrackType.Video));
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Codec ID: ([\\S ]{1,100})$")).Success)
                                        {
                                            trackCodecID = match.Groups[1].Value;
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Language: ([a-z]{2,3})$")).Success)
                                        {
                                            trackLanguage = MakeMKV.GetLanguage(match.Groups[1].Value);
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Default flag: 1$")).Success || (match = Regex.Match(lineWork, "^\\+ Default track flag: 1$")).Success)
                                        {
                                            trackFlags = trackFlags | Track.MKVFlags.Default;
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Enabled: 0$")).Success)
                                        {
                                            trackFlags &= ~Track.MKVFlags.Enabled;
                                        }
                                        else if ((match = Regex.Match(lineWork, "^\\+ Forced flag: 1$")).Success || (match = Regex.Match(lineWork, "^\\+ Forced track flag: 1$")).Success)
                                        {
                                            trackFlags = trackFlags | Track.MKVFlags.Forced;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                }

                if (tr.Count > 0)
                {
                    if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvinfo succeeded, {2} track(s) identified.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments, tr.Count), EventLogEntryType.Information);
                    this.tracks = tr.ToArray();
                }
                else
                {
                    if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvinfo failed, no tracks identified.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Error);
                    this.tracks = null;
                    return OperationResult.MKVInfoError;
                }
            }

            this.path = Path;
            return OperationResult.FileOpened;
        }

        private enum mkvInfoSection
        {
            Ignore = 0,
            SegmentInfo = 3,
            SegmentTracks = 4,
            Chapters = 6,
        }

        public OperationResult Save()
        {
            // Save changes to MKV file using mkvpropedit (compare value to valueCurrent to see if values have been changed and need to be committed to file).

            // Build arguments for mkvpropedit
            StringBuilder args = new StringBuilder();

            // Set title header (in segment info)
            if (title != null && title != titleCurrent)
                args.Append(String.Format(" -e info -s \"title={0}\"", title));

            // Set track headers
            foreach (Track track in tracks)
                args.Append(track.MKVPropEditArguments);

            if (args.Length > 0)
            {
                args.Insert(0, "--ui-language en " + String.Format("\"{0}\"", path));

                // Execute mkvpropedit with arguments
                using (Process process = new Process())
                {
                    // Redirect the output stream of the child process.
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.FileName = mkvToolNix.MKVToolNixPath + "mkvpropedit.exe";
                    process.StartInfo.Arguments = args.ToString();

                    if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvpropedit: Preparing to save metadata changes to mkv file.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Information);

                    bool processStarted;
                    try
                    {
                        processStarted = process.Start();
                    }
                    catch
                    {
                        processStarted = false;
                    }

                    if (!processStarted)
                    {
                        if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvpropedit failed to execute.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Error);
                        return OperationResult.CouldNotStartMKVPropEdit;
                    }

                    string line;
                    byte saveOK = 0;

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        Console.WriteLine(line);

                        if ((saveOK == 0 && line.Equals("The file is being analyzed.")) ||
                            (saveOK == 1 && line.Equals("The changes are written to the file.")) ||
                            (saveOK == 2 && line.Equals("Done.")))
                            saveOK++;

                        if (saveOK == 3) break;
                    }

                    // Wait for process to exit, attempt to kill it if necessary.
                    Thread.Sleep(500);
                    byte b = 0;
                    while (!process.HasExited && b < 6)
                    {
                        try { process.Kill(); }
                        catch { }
                        Thread.Sleep(500);
                        b++;
                    }
                    process.Close();

                    if (saveOK < 3)
                    {
                        if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvpropedit failed, changes do not appear to have been saved.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Warning);
                        return OperationResult.MKVPropEditError;
                    }

                    if (mkvToolNix.EventLogger != null) mkvToolNix.EventLogger.LogEntry("MKVToolNix", string.Format("mkvpropedit succeeded, changes saved to file.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Information);
                }
                return OperationResult.FileSaved;
            }
            else
                return OperationResult.NoChanges;
        }

        private TimeSpan duration;
        public TimeSpan Duration { get { return duration; } }

        private string title, titleCurrent;
        public string Title { get { return title; } set { title = value; } }

        private Track[] tracks;
        public Track[] Tracks { get { return tracks; } }

        public class Track
        {
            public Track(MKVFile File, ulong UID, uint Index, TrackType Type, Language Language, string Name, MKVFlags Flags, string CodecID)
            {
                this.file = File;
                this.uid = UID;
                this.index = Index;
                this.type = Type;
                this.language = Language; this.languageCurrent = Language;
                this.name = Name; this.nameCurrent = Name;
                this.flags = Flags; this.flagsCurrent = Flags;
                this.codecId = CodecID;
            }

            private MKVFile file;
            private ulong uid;

            private uint index;
            public uint Index { get { return index; } }

            private TrackType type;
            public TrackType Type { get { return type; } }

            private Language language, languageCurrent;
            public Language Language { get { return language; } set { language = value; } }

            private string name, nameCurrent;
            public string Name { get { return name; } set { name = value; } }

            private MKVFlags flags, flagsCurrent;
            public MKVFlags Flags { 
                get { return flags; } 
                set {
                    // TODO: Whenever the default flag is set on one track, it should be unset on all other tracks of the same type
                    //       If the default flag is unset, the default track should be automatically set on the first track of the same type.
                    //
                    // This applies only to Audio and Subtitle tracks.


                    flags = value; 
                } 
            }

            public bool Enabled
            {
                get { return this.flags.HasFlag(MKVFlags.Enabled); }
                set
                {
                    if (value && !this.flags.HasFlag(MKVFlags.Enabled))
                        this.flags |= MKVFlags.Enabled;
                    if (!value && this.flags.HasFlag(MKVFlags.Enabled))
                        this.flags &= ~MKVFlags.Enabled;
                }
            }

            public bool Default
            {
                get { return this.flags.HasFlag(MKVFlags.Default); }
                set
                {
                    if (value && !this.flags.HasFlag(MKVFlags.Default))
                        this.flags |= MKVFlags.Default;
                    if (!value && this.flags.HasFlag(MKVFlags.Default))
                        this.flags &= ~MKVFlags.Default;
                }
            }

            public bool Forced
            {
                get { return this.flags.HasFlag(MKVFlags.Forced); }
                set
                {
                    if (value && !this.flags.HasFlag(MKVFlags.Forced))
                        this.flags |= MKVFlags.Forced;
                    if (!value && this.flags.HasFlag(MKVFlags.Forced))
                        this.flags &= ~MKVFlags.Forced;
                }
            }

            private string codecId;
            public string CodecID { get { return codecId; } }

            [Flags]
            public enum MKVFlags
            {
                None = 0,
                Default = 1,
                Enabled = 2,
                Forced = 4,
            }

            public string MKVPropEditArguments
            {
                get
                {
                    StringBuilder args = new StringBuilder();
                    byte n = 0;

                    // Select track
                    args.Append(String.Format(" -e track:={0}", uid));

                    // Set language header
                    if (language != Language.Undetermined && language != languageCurrent)
                    { args.Append(String.Format(" -s language={0}", MakeMKV.GetLanguageISOCode(language))); n++; }

                    // Set name header
                    if (name != nameCurrent)
                    { args.Append(String.Format(" -s \"name={0}\"", name ?? "")); n++; }

                    // Set flag headers
                    string[] flagAttr = new string[] { "flag-default", "flag-enabled", null, "flag-forced" };
                    foreach (MKVFlags flag in Enum.GetValues(typeof(MKVFlags)))
                    {
                        if (flags.HasFlag(flag) != flagsCurrent.HasFlag(flag))
                        { args.Append(String.Format(" -s {0}={1}", flagAttr[(int)flag - 1], (flags.HasFlag(flag) ? '1' : '0'))); n++; }
                    }
                    
                    return (n > 0 ? args.ToString() : "");
                }
            }
        }

        public enum OperationResult
        {
            CouldNotStartMKVPropEdit = -1,
            CouldNotStartMKVInfo = -2,
            MKVInfoError = -3,
            MKVPropEditError = -4,

            FileOpened = 100,
            NoChanges = 199,
            FileSaved = 200
        }

    }

}
