using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MKVTools
{
    public class TemplateProcessor
    {
        private string template;
        public string Template
        {
            get { return template; }
            set { template = value; }
        }

        public string GetProcessedName(Title Title, Source Source)
        {
            return GetProcessedName(Title, Source, Template);
        }

        public static string GetProcessedName(Title Title, Source Source, string template)
        {
            if (string.IsNullOrWhiteSpace(template)) return null;

            MatchCollection matches;
            string key, value = null;
            Regex regex = new Regex("{(?:(?<pre>[^|{}]*)\\|)?(?<key>[a-z0-9-]+)(?:\\|(?<post>[^|{}]*))?(?:\\|(?<ifempty>[^|{}]*))?}");

            // Process template
            switch (template)
            {
                case "{name-default}":
                    template = (Title != null ? Title.NameDefault : null);
                    break;
                case "{filename-default}":
                    template = (Title != null ? Title.OutputFilenameDefault.Replace(".mkv", "") : null);
                    break;
                default:
                    int n;
                    List<Track> videoTracks = (Title != null ? Title.GetTracksByType(TrackType.Video) : null);
                    Title[] titles;
                    matches = regex.Matches(template);
                    foreach (Match match in matches)
                    {
                        // Find, process and replace.
                        key = match.Groups["key"].Value.ToLower();
                        switch (key)
                        {
                            // Source file/name
                            case "source-file":
                            case "source-file-clean":
                                switch (Source.Type)
                                {
                                    case SourceType.Disc:
                                        value = Source.Disc.Name; break;
                                    case SourceType.File:
                                    case SourceType.Folder:
                                        // Trim path.
                                        value = Source.Location.Trim();

                                        // If the path ends with VIDEO_TS (DVD folder) remove that part.
                                        if (Source.Type == SourceType.Folder && value.EndsWith("\\VIDEO_TS", StringComparison.InvariantCultureIgnoreCase))
                                            value = value.Substring(0, value.Length - 9);

                                        n = value.LastIndexOf('\\');
                                        if (n > 0 && n + 1 < value.Length)
                                        {
                                            value = value.Substring(n + 1);
                                            // Remove extension if file.
                                            if (Source.Type == SourceType.File)
                                            {
                                                n = value.LastIndexOf('.');
                                                if (n > 0 && n < value.Length) value = value.Substring(0, n);
                                            }
                                        }
                                        else
                                            value = null;

                                        break;
                                    default:
                                        value = null;
                                        break;
                                }
                                if (key == "source-file-clean")
                                    value = value.Replace('.', ' ').Replace('_', ' ').Trim();
                                break;
                            case "source-file-ext":
                                if (Source.Type != SourceType.File)
                                    value = null;
                                else
                                {
                                    n = Source.Location.LastIndexOf('.');
                                    value = (n > 0 && n + 1 < Source.Location.Length ? Source.Location.Substring(n + 1) : null);
                                }
                                break;
                            case "source-name":
                                value = Source.Disc.Name;
                                break;

                            // Source type
                            case "source-isfile":
                                value = (Source.Type == SourceType.File ? " " : null);
                                break;
                            case "source-isfolder":
                                value = (Source.Type == SourceType.Folder ? " " : null);
                                break;
                            case "source-isdrive":
                                value = (Source.Type == SourceType.Disc ? " " : null);
                                break;
                            case "source-isbluray":
                                value = (Source.Disc.DiscType == DiscType.Bluray ? " " : null);
                                break;
                            case "source-isdvd":
                                value = (Source.Disc.DiscType == DiscType.DVD ? " " : null);
                                break;

                            // Title index/count
                            case "titleindex-all":
                                value = null;
                                if (Title != null && Source.Titles.GetLength(0) > 1)
                                {
                                    n = 0;
                                    foreach (Title t in Source.Titles)
                                    {
                                        n++;
                                        if (t == Title) { value = n.ToString(); break; }
                                    }
                                }
                                break;
                            case "titleindex-included":
                                value = null;
                                if (Title != null && Title.Include)
                                {
                                    if ((titles = Source.Titles.Where(t => t.Include).ToArray()).GetLength(0) > 1)
                                    {
                                        n = 0;
                                        foreach (Title t in titles)
                                        {
                                            n++;
                                            if (t == Title) { value = n.ToString(); break; }
                                        }
                                    }
                                }
                                break;
                            case "titlecount-all":
                                n = Source.Titles.GetLength(0);
                                value = (n > 1 ? n.ToString() : null);
                                break;
                            case "titlecount-included":
                                n = Source.Titles.Count(t => t.Include);
                                value = (n > 1 ? n.ToString() : null);
                                break;

                            // Video track info
                            case "video-codec":
                                value = (Title != null && videoTracks != null && videoTracks.Count > 0 ? videoTracks[0].VideoCodec.ToString() : null);
                                break;
                            case "video-resolution":
                                value = (Title != null && videoTracks != null && videoTracks.Count > 0 ? String.Format("{0}x{1}", videoTracks[0].VideoResolution.Width, videoTracks[0].VideoResolution.Height) : null);
                                break;
                            case "video-framerate":
                                value = (Title != null && videoTracks != null && videoTracks.Count > 0 ? Math.Round(videoTracks[0].VideoFramerate, 0).ToString() : null);
                                break;
                            case "video-3d":
                                value = (Title != null && Title.VideoInclude3D ? " " : null);
                                break;

                            // Misc.
                            case "segment-map":
                                value = (Title != null && Title.Segments != null && Title.Segments.Length > 0 ? string.Join<int>(",", Title.Segments) : null);
                                break;

                            // Defaults
                            case "filename-default":
                                value = (Title != null ? Title.OutputFilenameDefault.Replace(".mkv", "") : null);
                                break;
                            case "name-default":
                                value = (Title != null ? Title.NameDefault : null);
                                break;

                            default:
                                // Unrecognized, replace with empty
                                value = null;
                                break;
                        }

                        if (value != null && value.Length > 0)
                            template = template.Replace(match.Value, match.Groups["pre"].Value + value.Trim() + match.Groups["post"].Value);
                        else
                            template = template.Replace(match.Value, match.Groups["ifempty"].Value);
                    }
                    break;
            }

            return (template != null ? template.Trim() : null);
        }

    }
}
