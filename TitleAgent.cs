using System.Collections.Generic;
using System.Text.RegularExpressions;
using MKVTools;
using System.Diagnostics;

namespace BatchMKV
{
    public class TitleAgent
    {
        public TitleAgent()
        {
            UpdateSettings();
        }

        public void UpdateSettings()
        {
            // Read settings from Properties.
            includeAll = Properties.Settings.Default.DefaultTitlesIncludeAll;
            includeWithinLength = Properties.Settings.Default.DefaultTitlesIncludeWithinLength;
            ignoreMuted = Properties.Settings.Default.DefaultTitlesIgnoreMuted;
            titleNameTemplate = Properties.Settings.Default.DefaultTitleName;
            titleFilenameTemplate = Properties.Settings.Default.DefaultTitleFilename;
        }

        private bool ignoreMuted, includeAll;
        private ushort includeWithinLength;
        private string titleNameTemplate, titleFilenameTemplate;

        public void ApplyDefaultSelections(List<Title> Titles)
        {
            ApplyDefaultSelections(Titles.ToArray());
        }

        public void ApplyDefaultSelections(Title[] Titles)
        {
            double durationLongest = 0, durationInclude;
            int naudio = 0;
            Title mainFeature = null;

            foreach (Title title in Titles)
            {
                if (title.Comment != null && title.Comment.Equals("FPL_MainFeature"))
                    mainFeature = title;

                // Check if title can be modified (not if it is currently being converted, or has already been converted.
                if (title.Result == Title.ConversionResult.ConversionInProgress || title.Result == Title.ConversionResult.Success)
                    continue;

                // Set title include

                if (ignoreMuted)
                {
                    naudio = 0;
                    foreach (Track track in title.Tracks)
                        if (track.TrackType == TrackType.Audio)
                        { naudio++; break; }
                }

                if (ignoreMuted && naudio == 0)
                    title.Include = false;
                else
                {
                    title.Include = includeAll;
                    if (!includeAll && title.Duration.TotalMilliseconds > durationLongest)
                        durationLongest = title.Duration.TotalMilliseconds;
                }
            }

            if (!includeAll)
            {
                if (mainFeature != null)
                {
                    mainFeature.Include = true;
                    return;
                    
                }

                durationInclude = durationLongest - includeWithinLength * 1000;
                foreach (Title title in Titles)
                {
                    if (title.Duration.TotalMilliseconds >= durationInclude)
                    {
                        title.Include = true;
                        if (includeWithinLength == 0) break;
                    }
                }
            }
        }

        public void ApplyDefaultSettings(Title Title)
        {
            ApplyDefaultSettings(new Title[] { Title });
        }

        public void ApplyDefaultSettings(List<Title> Titles)
        {
            ApplyDefaultSettings(Titles.ToArray());
        }

        public void ApplyDefaultSettings(Title[] Titles)
        {
            foreach (Title title in Titles)
            {
                title.NameTemplate = titleNameTemplate;

                if (titleFilenameTemplate.Length == 0)
                    title.OutputFilenameTemplate = titleNameTemplate + ".mkv";
                else
                    title.OutputFilenameTemplate = titleFilenameTemplate;
            }
        }
    }
}
