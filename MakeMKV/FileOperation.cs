using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MKVTools
{
    public class FileOperation
    {
        // Class for asynchronously copying, moving and deleting files and folders.

        public FileOperation()
        {

        }

        private object infoLock = new object();
        
        private OperationType operationInProgress = OperationType.None;
        public OperationType OperationInProgress { get { return operationInProgress; } }

        // If MoveCopyFirst = true:
        //      1/ File is copied
        //      2/ Destination file is verified (from size only)
        //      3/ Source file is deleted (if destination file could be verified)
        //
        // If MoveCopyFirst = false:
        //      1/ File is moved
        //      2/ If destination file cannot be verified, event is raised
        private bool moveCopyFirst = false;
        public bool MoveCopyFirst { get { return moveCopyFirst; } set { moveCopyFirst = value; } }

        public enum OperationType
        {
            None = 0,
            Copy = 1,
            Move = 2,
            Delete = 3
        }

        public enum OperationEventType
        {
            OperationStarted = 1,
            OperationFailed = 2,
            OperationCompleted = 3,
            OperationCancelled = 4,

            FileCopyListingFiles = 50,
            FileCopyCalculatingSize = 51,
            FileCopyStarting = 52,

            FileCopied = 60,
            FileMoved = 61,
            FileCopyError = 62,
            FileMoveError = 63,
            FileMoveCouldNotRemoveSource = 64,

            FolderCopied = 70,
            FolderMoved = 71,

            FileDeleted = 100,
            FileNotFound = 101,
            FileAccessUnauthorized = 102,
            FileIOError = 103,

            FolderDeleted = 110,
            FolderNotFound = 111,
            FolderAccessUnauthorized = 112,
            FolderIOError = 113,

        }

        public async Task<bool> CopyAsync(string Source, string Destination, bool Overwrite = false, string[] Exclude = null)
        {
            Task<bool> t = this.copyAsync(new string[] { Source }, Destination, Overwrite, false, Exclude);
            return await t;
        }

        public async Task<bool> CopyAsync(string[] Source, string Destination, bool Overwrite = false, string[] Exclude = null)
        {
            Task<bool> t = this.copyAsync(Source, Destination, Overwrite, false, Exclude);
            return await t;
        }

        public async Task<bool> MoveAsync(string Source, string Destination, bool Overwrite = false, string[] Exclude = null)
        {
            Task<bool> t = this.copyAsync(new string[] { Source }, Destination, Overwrite, true, Exclude);
            return await t;
        }

        public async Task<bool> MoveAsync(string[] Source, string Destination, bool Overwrite = false, string[] Exclude = null)
        {
            Task<bool> t = this.copyAsync(Source, Destination, Overwrite, true, Exclude);
            return await t;
        }

        private double copyTotal, copyCurrent, copySizeTotal, copySizeCurrent;

        private async Task<bool> copyAsync(string[] Source, string Destination, bool Overwrite = false, bool Move = false, string[] Exclude = null)
        {
            // Source can be both files and folders.
            // Returns true if task was started, false otherwise. This does not indicate if the operation succeeded or not.

            // TODO: Files/folders in Exclude (if != null) must not be deleted!

            // Check that an operation is not currently in progress.
            lock (infoLock)
                if (operationInProgress != OperationType.None)
                    return false;

            // Operation started, raise event.
            OperationType type = Move ? OperationType.Move : OperationType.Copy;
            OperationEventArgs e = new OperationEventArgs(type, OperationEventType.OperationStarted);
            OnOperationStartedEvent(this, e);

            // Check if operation was cancelled by event subscriber(s)
            if (e.Cancel)
            {
                OnOperationFailedEvent(this, new OperationEventArgs(type, OperationEventType.OperationCancelled));
                return true;
            }

            // Build complete list of files and folders to be copied (and deleted, in the case of move).
            OnOperationProgressEvent(this, new OperationProgressEventArgs(type, OperationEventType.FileCopyListingFiles));
            Dictionary<FileInfo, string> files = await this.getFileListAsync(Source, Destination, type);
            
            // Calculate the accumulated size of files to be copied/moved.
            OnOperationProgressEvent(this, new OperationProgressEventArgs(type, OperationEventType.FileCopyCalculatingSize));
            double size = 0;
            foreach (FileInfo fi in files.Keys)
                size += (!fi.Attributes.HasFlag(FileAttributes.Directory) ? fi.Length : 0);

            lock (infoLock)
            {
                copyTotal = files.Count;
                copyCurrent = 0;
                copySizeTotal = size;
                copySizeCurrent = 0;
            }

            // Start copy/move
            OnOperationProgressEvent(this, new OperationProgressEventArgs(type, OperationEventType.FileCopyStarting));
            bool fileVerified, isFile;
            double progress = 0;
            long sourceSize;
            FileInfo target;
            await Task.Run(() =>
            {
                foreach (KeyValuePair<FileInfo, string> file in files)
                {
                    isFile = !file.Key.Attributes.HasFlag(FileAttributes.Directory);
                    fileVerified = false;

                    if (isFile)
                    {
                        try
                        {
                            // Create destination path if necessary
                            Directory.CreateDirectory(getPath(file.Value));

                            // Copy file
                            if (moveCopyFirst || !Move)
                            {
                                target = file.Key.CopyTo(file.Value, Overwrite);

                                // Verify that target file exists and has the same size as source file
                                fileVerified = (target.Exists && target.Length == file.Key.Length);
                            }
                            else
                            {
                                sourceSize = file.Key.Length;
                                //File.Move(file.Key.FullName, file.Value);
                                //target = new FileInfo(file.Value);
                                file.Key.MoveTo(file.Value);
                                file.Key.Refresh();

                                // Verify that target file exists and has the same size as source file
                                fileVerified = (file.Key.Exists && file.Key.Length == sourceSize);
                            }
                        }
                        catch
                        { /* Do nothing: fileVerified = false will ensure proper handling */ }
                    }
                    else
                    {
                        // Create folder
                        Directory.CreateDirectory(file.Value);
                    }
                    
                    // If move and if file verified, delete source file/folder (folder delete must be non-recursive, folder should only be deleted if it is empty).
                    if (Move && (!isFile || fileVerified))
                    {
                        if (isFile && moveCopyFirst)
                        {
                            try { File.Delete(file.Key.FullName); }
                            catch { OnOperationProgressEvent(this, new OperationProgressEventArgs(type, OperationEventType.FileMoveCouldNotRemoveSource, file.Value, progress)); }
                        }
                        else if (!isFile)
                        {
                            try { Directory.Delete(file.Key.FullName, false); }
                            catch { /* Do nothing, this is expected if the directory is not empty. */ }
                        }
                    }

                    // Update progress
                    lock (infoLock)
                    {
                        copyCurrent++;
                        if (isFile) copySizeCurrent += file.Key.Length;
                        progress = (copySizeTotal > 0 ? copySizeCurrent / copySizeTotal : copyCurrent / copyTotal);
                    }

                    // Raise event
                    if (isFile && !fileVerified)
                        OnOperationProgressEvent(this, new OperationProgressEventArgs(type, (Move ? OperationEventType.FileMoveError : OperationEventType.FileCopyError), file.Value, progress));
                    else
                        OnOperationProgressEvent(this, new OperationProgressEventArgs(type,
                            Move
                                ? (isFile ? OperationEventType.FileMoved : OperationEventType.FolderMoved)
                                : (isFile ? OperationEventType.FileCopied : OperationEventType.FolderCopied),
                            file.Value,
                            progress));
                }
            });

            // Operation completed (or failed, if deleteCurrent < deleteTotal)
            bool completed;
            lock (infoLock) completed = copyCurrent == copyTotal;
            if (completed)
                OnOperationCompletedEvent(this, new OperationEventArgs(type, OperationEventType.OperationCompleted));
            else
                OnOperationFailedEvent(this, new OperationEventArgs(type, OperationEventType.OperationFailed));

            return true;
        }

        private string getPath(string FullPath)
        {
            if (FullPath.EndsWith("\\"))
                return FullPath.Substring(0, FullPath.Length - 1);
            else
            {
                int c = FullPath.LastIndexOf('\\');
                return (c >= 0 ? FullPath.Substring(0, c) : FullPath);
            }
        }

        private double deleteTotal, deleteCurrent;

        public async Task<bool> DeleteAsync(string Target, string[] Exclude = null)
        {
            return await this.DeleteAsync(new string[] { Target }, Exclude);
        }

        public async Task<bool> DeleteAsync(string[] Targets, string[] Exclude = null)
        {
            // Files can be both files and folders.
            // Returns true if task was started, false otherwise. This does not indicate if the operation succeeded or not.
            
            // Check that an operation is not currently in progress.
            lock (infoLock)
                if (operationInProgress != OperationType.None)
                    return false;

            // Operation started, raise event.
            OperationEventArgs e = new OperationEventArgs(OperationType.Delete, OperationEventType.OperationStarted);
            OnOperationStartedEvent(this, e);

            // Check if operation was cancelled by event subscriber(s)
            if (e.Cancel)
            {
                OnOperationFailedEvent(this, new OperationEventArgs(OperationType.Delete, OperationEventType.OperationCancelled));
                return true;
            }

            // Start delete
            lock (infoLock)
            {
                deleteTotal = Targets.GetLength(0);
                deleteCurrent = 0;
            }
            await this.deleteFilesAsync(Targets, Exclude);

            // Operation completed (or failed, if deleteCurrent < deleteTotal)
            bool completed;
            lock (infoLock) completed = deleteCurrent == deleteTotal;
            if (completed)
                OnOperationCompletedEvent(this, new OperationEventArgs(OperationType.Delete, OperationEventType.OperationCompleted));
            else
                OnOperationFailedEvent(this, new OperationEventArgs(OperationType.Delete, OperationEventType.OperationFailed));

            return true;
        }

        public event OperationEventArgs.OperationEventHandler OperationStarted;
        private void OnOperationStartedEvent(FileOperation sender, OperationEventArgs e)
        {
            lock (infoLock) operationInProgress = e.Type;
            if (OperationStarted != null) OperationStarted(sender, e); 
        }

        public event OperationEventArgs.OperationEventHandler OperationFailed;
        private void OnOperationFailedEvent(FileOperation sender, OperationEventArgs e)
        {
            lock (infoLock) operationInProgress = OperationType.None;
            if (OperationFailed != null) OperationFailed(sender, e); 
        }

        public event OperationEventArgs.OperationEventHandler OperationCompleted;
        private void OnOperationCompletedEvent(FileOperation sender, OperationEventArgs e)
        {
            lock (infoLock) operationInProgress = OperationType.None;
            if (OperationCompleted != null) OperationCompleted(sender, e); 
        }

        public event OperationProgressEventArgs.OperationProgressEventHandler OperationProgress;
        private void OnOperationProgressEvent(FileOperation sender, OperationProgressEventArgs e)
        { 
            if (OperationProgress != null) OperationProgress(sender, e); 
        }

        public class OperationEventArgs : EventArgs
        {
            public delegate void OperationEventHandler(FileOperation sender, OperationEventArgs e);

            public OperationEventArgs(OperationType Type, OperationEventType EventType, string Message = "")
            {
                this.type = Type;
                this.eventType = EventType;
                this.message = Message;
            }

            private OperationType type;
            public OperationType Type { get { return type; } }

            private OperationEventType eventType;
            public OperationEventType EventType { get { return eventType; } }

            private string message;
            public string Message { get { return message; } }

            // Applies only to OnStarted events.
            private bool cancel = false;
            public bool Cancel { get { return cancel; } set { cancel = value; } }
        }

        public class OperationProgressEventArgs : EventArgs
        {
            public delegate void OperationProgressEventHandler(FileOperation sender, OperationProgressEventArgs e);

            public OperationProgressEventArgs(OperationType Type, OperationEventType EventType, string Target = null, double Progress = 0)
            {
                this.type = Type;
                this.eventType = EventType;
                this.target = Target;
                this.progress = Progress;
            }

            private OperationType type;
            public OperationType Type { get { return type; } }

            private OperationEventType eventType;
            public OperationEventType EventType { get { return eventType; } }

            private string target;
            public string Target { get { return target; } }

            private double progress;
            public double Progress { get { return progress; } }
        }

        private async Task deleteFilesAsync(string[] Files, string[] Exclude = null)
        {
            // TODO: Files/folders in Exclude (if != null) must not be deleted!

            await Task.Run(() =>
            {
                bool isFile;
                FileInfo fi;

                foreach (string file in Files)
                {
                    isFile = true;

                    try
                    {
                        // Determine if it is a file or a folder.
                        fi = new FileInfo(file);
                        isFile = !fi.Attributes.HasFlag(FileAttributes.Directory);

                        if (isFile)
                        {
                            File.Delete(file);

                            // File deleted, raise event.
                            lock (infoLock) deleteCurrent++;
                            OnOperationProgressEvent(this, new OperationProgressEventArgs(OperationType.Delete, OperationEventType.FileDeleted, file, deleteCurrent / deleteTotal));
                        }
                        else
                        {
                            Directory.Delete(file, true);

                            // Folder deleted, raise event.
                            lock (infoLock) deleteCurrent++;
                            OnOperationProgressEvent(this, new OperationProgressEventArgs(OperationType.Delete, OperationEventType.FolderDeleted, file, deleteCurrent / deleteTotal));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Could not delete file/folder.
                        lock (infoLock) deleteCurrent++;

                        // Determine reason delete failed.
                        OperationEventType t;
                        switch(ex.GetType().ToString())
                        {
                            case "System.IO.FileNotFoundException":
                            case "System.IO.DirectoryNotFoundException":
                                t = (isFile ? OperationEventType.FileNotFound : OperationEventType.FolderNotFound); break;
                            case "System.UnauthorizedAccessException":
                                t = (isFile ? OperationEventType.FileAccessUnauthorized : OperationEventType.FolderAccessUnauthorized); break;
                            default:
                                t = (isFile ? OperationEventType.FileIOError : OperationEventType.FolderIOError); break;
                        }

                        // Raise event.
                        OnOperationProgressEvent(this, new OperationProgressEventArgs(OperationType.Delete, t, file, deleteCurrent / deleteTotal));
                    }
                }
            });
        }

        private async Task<Dictionary<FileInfo, string>> getFileListAsync(string[] Files, string Destination, OperationType Type, string Source = null)
        {
            // Files can be both files and folders.
            // Return array is ordered such that files/folders can be deleted in order (containing folders are listed after the respective files).

            Dictionary<FileInfo, string> files = new Dictionary<FileInfo, string>();
            FileInfo f;
            bool isFile;
            string sourcePath, destinationPath, destinationFolder;

            foreach (string file in Files)
            {
                isFile = false;

                try
                {
                    f = new FileInfo(file);
                }
                catch (Exception ex)
                {
                    // Could not get file/folder info.
                    // Determine reason operation failed.
                    OperationEventType t;
                    switch (ex.GetType().ToString())
                    {
                        case "System.IO.FileNotFoundException":
                        case "System.IO.DirectoryNotFoundException":
                            t = (isFile ? OperationEventType.FileNotFound : OperationEventType.FolderNotFound); break;
                        case "System.UnauthorizedAccessException":
                            t = (isFile ? OperationEventType.FileAccessUnauthorized : OperationEventType.FolderAccessUnauthorized); break;
                        default:
                            t = (isFile ? OperationEventType.FileIOError : OperationEventType.FolderIOError); break;
                    }

                    // Raise event.
                    OnOperationProgressEvent(this, new OperationProgressEventArgs(Type, t, file, 0));
                    continue;
                }

                isFile = !f.Attributes.HasFlag(FileAttributes.Directory);
                
                // Check if file exists.
                if ((isFile && !f.Exists) || (int)f.Attributes < 0)
                {
                    // File does not exist, raise event.
                    OnOperationProgressEvent(this, new OperationProgressEventArgs(Type, (isFile ? OperationEventType.FileNotFound : OperationEventType.FolderNotFound), file, 0));
                    continue;
                }

                if (!isFile)
                {
                    sourcePath = (Source != null
                        ? Source
                        : file + (!file.EndsWith("\\") ? "\\" : ""));

                    // Get folders in this directory.
                    foreach (KeyValuePair<FileInfo, string> p in await getFileListAsync(Directory.GetDirectories(f.FullName), Destination, Type, sourcePath))
                        files.Add(p.Key, p.Value);

                    // Get files in this directory.
                    foreach (KeyValuePair<FileInfo, string> p in await getFileListAsync(Directory.GetFiles(f.FullName), Destination, Type, sourcePath))
                        files.Add(p.Key, p.Value);
                }

                // Determine destination path.
                destinationFolder = Destination + (!Destination.EndsWith("\\") ? "\\" : "");
                if (Source == null && !isFile)
                {
                    destinationPath = Destination;
                }
                else if ((Source == null && isFile) || file.Equals(Source, StringComparison.InvariantCultureIgnoreCase))
                {
                    // If file == Source, file should be moved to the root of Destination.
                    destinationPath = destinationFolder + f.Name;
                }
                else
                {
                    // If file != Source, file should be moved so that relative path to Source is maintained relative to Destination.
                    if (Source.Length > file.Length || !file.ToLower().StartsWith(Source.ToLower()))
                        throw new Exception("Could not determine destination path.");
                    destinationPath = destinationFolder + file.Substring(Source.Length);
                }

                // Add file/folder to list
                files.Add(f, destinationPath);
            }

            return files;
        }

    }
}
