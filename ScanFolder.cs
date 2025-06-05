using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using BatchMKV.Domain;

namespace BatchMKV
{
    public class ScanFolder
    {
        private readonly string[] supportedExtensions = new string[] { ".bdmv", ".ifo", ".dat", ".iso", ".avi", ".m2v", ".m4v", ".mkv", ".mpg", ".mp4", ".wmv" };

        public event ScanEventArgs.ScanEventHandler ScanStarted;
        public event ScanEventArgs.ScanEventHandler ScanFailed;
        public event ScanEventArgs.ScanEventHandler ScanCompleted;
        public event ScanEventArgs.ScanEventHandler ScanFoundSource;

        private void OnScanStartedEvent(object sender, ScanEventArgs e)
        { if (ScanStarted != null) ScanStarted(sender, e); }

        private void OnScanFailedEvent(object sender, ScanEventArgs e)
        { if (ScanFailed != null) ScanFailed(sender, e); }

        private void OnScanCompletedEvent(object sender, ScanEventArgs e)
        { if (ScanCompleted != null) ScanCompleted(sender, e); }

        private void OnScanFoundSourceEvent(object sender, ScanEventArgs e)
        { if (ScanFoundSource != null) ScanFoundSource(sender, e); }

        public class ScanEventArgs : EventArgs
        {
            public delegate void ScanEventHandler(object sender, ScanEventArgs e);

            public ScanEventArgs(string Message = "", SourceState Source = null)
            {
                this.source = Source;
                this.message = Message;
            }

            private SourceState source;
            public SourceState Source {get{return source;}}

            private string message;
            public string Message { get { return message; } }
        }

        public bool ScanFile(string File, ref SourceState Source, ref string Location, List<string> IgnoreLocations = null)
        {
            Source = null;
            Location = null;

            try
            {
                FileInfo fi = new FileInfo(File);
                string folder, fileExt = fi.Extension.ToLower();
                if (!supportedExtensions.Contains(fileExt)) return false;

                switch (fileExt)
                {
                    case ".bdmv":
                        /* Verify that this is a valid Bluray folder:
                         * 1/ This file must be index.bdmv
                         * 2/ This file must be in a folder named BDMV
                         * 3/ This folder must contain a STREAM sub folder
                         * 4/ The parent folder is the Bluray folder.
                         */

                        if (!fi.Name.Equals("index.bdmv", StringComparison.OrdinalIgnoreCase) ||
                            !fi.Directory.Name.Equals("BDMV", StringComparison.OrdinalIgnoreCase))
                            return false;

                        folder = fi.Directory.Parent.FullName;
                        if (IgnoreLocations != null && IgnoreLocations.Contains(folder)) return false;
                        if (fi.Directory.GetDirectories("STREAM", SearchOption.TopDirectoryOnly).Length != 1) return false;

                        Source = new SourceState(LocationType.FolderBluray, folder);
                        Location = folder;
                        break;
                    case ".ifo":
                        /* Verify that this is a valid DVD folder:
                         * 1/ This file must be VIDEO_TS.IFO
                         * 2/ This file must be in a folder named VIDEO_TS
                         * 3/ The parent folder is the DVD folder.
                         */

                        if (!fi.Name.Equals("VIDEO_TS.IFO", StringComparison.OrdinalIgnoreCase) ||
                            !fi.Directory.Name.Equals("VIDEO_TS", StringComparison.OrdinalIgnoreCase))
                            return false;

                        folder = fi.Directory.Parent.FullName;
                        if (IgnoreLocations != null && IgnoreLocations.Contains(folder)) return false;

                        Source = new SourceState(LocationType.FolderDVD, folder);
                        Location = folder;
                        break;
                    case ".dat":
                        // TODO: Verify that this is a valid HD-DVD folder.

                        break;
                    default:
                        // This is a type of media file (not a folder).
                        if (IgnoreLocations != null && IgnoreLocations.Contains(fi.FullName)) return false;
                        Source = new SourceState(LocationType.File, fi.FullName);
                        Location = fi.FullName;
                        break;
                }

                if (Source != null)
                {
                    Source.Initialize();
                    return true;
                }
            }
            catch
            {
                // Failed - do nothing
            }

            return false;
        }

        public void Scan(string Target, bool Recursive = true)
        {
            string location = null;
            SourceState src = null;

            // Check if target is a folder or a file.
            bool dirExists;
            dirExists = Directory.Exists(Target);
            if (!dirExists)
            {
                if (File.Exists(Target))
                {
                    OnScanStartedEvent(this, new ScanEventArgs(String.Format("Scanning for sources...", Target)));
                    if (ScanFile(Target, ref src, ref location))
                    {
                        OnScanFoundSourceEvent(this, new ScanEventArgs(String.Format("Source found: {0}", location), src));
                        OnScanCompletedEvent(this, new ScanEventArgs("Scan completed, source found."));
                    }
                    else
                    {
                        OnScanCompletedEvent(this, new ScanEventArgs("Scan completed, source not found."));
                    }
                    return;
                }
            }

            OnScanStartedEvent(this, new ScanEventArgs(String.Format("Scanning '{0}' for sources...", Target)));

            List<SourceState> locations = new List<SourceState>();
            List<string> locationKeys = new List<string>();

            Stack<string> dirs = new Stack<string>(20);

            if (!dirExists)
            {
                OnScanFailedEvent(this, new ScanEventArgs(String.Format("Path '{0}' not found.", Target)));
                return;
            }
            dirs.Push(Target);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch
                {
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch
                {
                    continue;
                }

                foreach (string file in files)
                {
                    if (ScanFile(file, ref src, ref location, locationKeys))
                    {
                        locationKeys.Add(location);
                        OnScanFoundSourceEvent(this, new ScanEventArgs(String.Format("Source found: {0}", location), src));
                    }
                }

                if (Recursive)
                {
                    // Push the subdirectories onto the stack for traversal. 
                    foreach (string str in subDirs)
                        dirs.Push(str);
                }
            }

            OnScanCompletedEvent(this, new ScanEventArgs(String.Format("Scan completed, {0} source(s) found.", locationKeys.Count)));
        }

    }
}
