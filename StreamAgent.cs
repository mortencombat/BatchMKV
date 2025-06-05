using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKVTools;

namespace BatchMKV
{
    public class StreamAgent
    {
        public StreamAgent()
        {
            favouriteLanguages = new List<Language>();
            this.UpdateSettings();
        }

        public void UpdateSettings()
        {
            // Read settings from Properties.

            // Languages
            favouriteLanguages.Clear();
            if (Properties.Settings.Default.FavouriteLanguages.Length > 0)
            {
                foreach (string language in Properties.Settings.Default.FavouriteLanguages.Split('|'))
                    favouriteLanguages.Add(MakeMKV.GetLanguage(language));
            }

            // Audio
            audioIncludeAll = Properties.Settings.Default.DefaultAudioIncludeAll;
            audioLimit = Properties.Settings.Default.DefaultAudioLimit;
            audioMainCore = (AudioMainCore)Properties.Settings.Default.DefaultAudioMainCore;
            audioIncludeQuality = Properties.Settings.Default.DefaultAudioIncludeQuailty;
            audioIncludeNonFavourites = Properties.Settings.Default.DefaultAudioIncludeNonFavourite;
            audioIncludeFirst = Properties.Settings.Default.DefaultAudioIncludeFirst;
            audioOrder = (TrackSortOrder)Properties.Settings.Default.DefaultAudioOrder;
            audioIncludeCommentaryTracks = Properties.Settings.Default.DefaultAudioIncludeCommentaryTracks;
            audioCommentaryTrackName = (string.IsNullOrWhiteSpace(Properties.Settings.Default.DefaultAudioCommentaryTrackName) ? "Commentary" : Properties.Settings.Default.DefaultAudioCommentaryTrackName);

            // Subtitle
            subtitleIncludeAll = Properties.Settings.Default.DefaultSubtitleIncludeAll;
            subtitleLimit = Properties.Settings.Default.DefaultSubtitleLimit;
            subtitleOrder = (TrackSortOrder)Properties.Settings.Default.DefaultSubtitleOrder;
            subtitleDefault = (SubtitleDefault)Properties.Settings.Default.DefaultSubtitleTrack;
        }

        public void ApplyDefaults(List<MKVTools.Track> Tracks)
        {
            SelectTracks(Tracks);
            OrderTracks(Tracks);
            SetDefaultTracks(Tracks);
        }

        private bool audioIncludeAll, subtitleIncludeAll;
        private byte audioLimit, subtitleLimit;
        private TrackSortOrder audioOrder, subtitleOrder;
        private AudioMainCore audioMainCore;
        private SubtitleDefault subtitleDefault;
        private List<Language> favouriteLanguages;
        private bool audioIncludeQuality, audioIncludeNonFavourites, audioIncludeFirst, audioIncludeCommentaryTracks;
        private string audioCommentaryTrackName;

        public void SelectTracks(List<MKVTools.Track> Tracks)
        {
            // Sets include true/false for audio and subtitle tracks.
            Dictionary<Language, byte> languageCountAudio = new Dictionary<Language, byte>();
            Dictionary<Language, byte> languageCountSubtitle = new Dictionary<Language, byte>();
            byte[] n = new byte[2] { 0, 0 };

            // Check if there is at least one track in favourite language
            bool audioIncludeAllEffective = audioIncludeAll;
            bool audioIncludeQualityEffective = false;
            if (!audioIncludeAll && (audioIncludeNonFavourites || audioIncludeQuality))
            {
                n[0] = 0; n[1] = 0;
                if (favouriteLanguages.Count > 0)
                {
                    foreach (Track tm in Tracks)
                    {
                        foreach (Track t in new Track[2] { tm, tm.Child })
                        {
                            if (t == null || t.TrackType != TrackType.Audio) continue;
                            
                            if (favouriteLanguages.Contains(t.Language))
                                n[0]++;

                            if (MakeMKV.GetAudioCodecIsLossless(t.AudioCodec))
                                n[1]++;
                        }

                        if ((!audioIncludeNonFavourites || n[0] > 0) &&
                            (!audioIncludeQuality || n[1] > 0))
                            break;
                    }
                }

                if (n[0] == 0 && audioIncludeNonFavourites) 
                    audioIncludeAllEffective = true;

                if (n[1] == 0 && audioIncludeQuality)
                    audioIncludeQualityEffective = true;
            }

            n[0] = 0;
            foreach(Track tm in Tracks)
            {
                switch(tm.TrackType)
                {
                    case TrackType.Audio:
                        foreach (Track t in new Track[2] { tm, tm.Child })
                        {
                            if (t != null) 
                                t.Include = false;

                            // Check language (unless all included), quality and language limit
                            if (t != null &&
                                (audioIncludeAllEffective ||                                                            // If include all
                                favouriteLanguages.Contains(t.Language) ||                                              // If favourite language
                                (audioIncludeFirst && n[0] == 0) ||                                                     // If first track
                                (audioIncludeQualityEffective && MakeMKV.GetAudioCodecIsLossless(t.AudioCodec))) &&     // If lossless track and no favourite tracks in lossless quality
                                (audioLimit == 0 || !languageCountAudio.ContainsKey(t.Language) ||languageCountAudio[t.Language] < audioLimit))
                                                                                                                        // If track limit not reached
                            {
                                // Check main/core
                                if ((audioMainCore == AudioMainCore.Both) ||
                                    (audioMainCore == AudioMainCore.MainOnly && !t.StreamFlags.HasFlag(StreamFlag.CoreAudio)) ||
                                    (audioMainCore == AudioMainCore.CoreOnly && !t.StreamFlags.HasFlag(StreamFlag.HasCoreAudio)))
                                {
                                    t.Include = true;
                                    n[0]++;
                                    if (audioLimit == 0) continue;
                                    if (languageCountAudio.ContainsKey(t.Language))
                                        languageCountAudio[t.Language]++;
                                    else
                                        languageCountAudio.Add(t.Language, 1);
                                }
                            }
                        }
                        break;
                    case TrackType.Subtitle:
                        tm.Include = false;

                        if ((subtitleIncludeAll ||
                            favouriteLanguages.Contains(tm.Language)) &&
                            (subtitleLimit == 0 || !languageCountSubtitle.ContainsKey(tm.Language) || languageCountSubtitle[tm.Language] < subtitleLimit))
                        {
                            tm.Include = true;
                            if (subtitleLimit == 0) break;
                            if (languageCountSubtitle.ContainsKey(tm.Language))
                                languageCountSubtitle[tm.Language]++;
                            else
                                languageCountSubtitle.Add(tm.Language, 1);
                        }
                        break;
                }
            }

            if (!audioIncludeAll && audioIncludeCommentaryTracks)
            {
                // Check if there are any tracks that look like commentary tracks
                Track tc;
                for (int i = Tracks.Count - 1; i >= 0; i--)
                {
                    tc = Tracks[i];

                    if (tc.TrackType != TrackType.Audio) continue;

                    // 1) Check if track is already included
                    if (tc.Include) break;

                    // 2) Check that track is lossy stereo without any core tracks
                    if (tc.Child != null || tc.AudioChannelLayout != AudioChannelLayout.Stereo || MakeMKV.GetAudioCodecIsLossless(tc.AudioCodec)) break;

                    // 3) Check that track is in one of the favourite languages
                    if (!favouriteLanguages.Contains(tc.Language)) break;

                    // Track looks like a commentary track, include and rename
                    tc.Include = true;
                    tc.Name = audioCommentaryTrackName;
                }
            }
        }

        private enum AudioMainCore
        {
            MainOnly = 0,
            CoreOnly = 1,
            Both = 2
        }

        private enum TrackSortOrder
        {
            Default = 0,
            Language = 1,
            Quality = 2
        }

        private enum SubtitleDefault
        {
            No = 0,
            First = 1,
            FirstForced = 2
        }

        public void OrderTracks(List<MKVTools.Track> Tracks)
        {
            // Set order weights for audio and subtitle tracks.

            SortedList<double, Track> trks = new SortedList<double, Track>();
            double d = 0, dq, dl;
            double[] dt = new double[3] { 0, 1000, 2000 };

            foreach (Track t in Tracks)
            {
                foreach (Track track in new Track[] { t, t.Child })
                {
                    if (track != null && track.Include && !track.IsEmpty)
                    {
                        d += 0.000001;
                        
                        // Lossy audio should "sink".
                        dq = (track.TrackType == TrackType.Audio && !MakeMKV.GetAudioCodecIsLossless(track.AudioCodec) ? 100 : 0)
                            * ((track.TrackType == TrackType.Audio && audioOrder == TrackSortOrder.Language) ? 0.001 : 1);

                        // The less favourite, the heavier language.
                        dl = (track.Language != Language.Undetermined && favouriteLanguages.Contains(track.Language)
                            ? 1 + favouriteLanguages.IndexOf(track.Language)
                            : 200)
                            * ((track.TrackType == TrackType.Audio && audioOrder == TrackSortOrder.Quality) ? 0.001 : 1);

                        trks.Add(d + dt[(int)track.TrackType - 1] + dq + dl, track);
                    }
                }
            }

            int w = 0;
            foreach(Track t in trks.Values)
            {
                t.OrderWeight = w;
                w++;
            }
        }

        public void SetDefaultTracks(List<MKVTools.Track> Tracks, bool Audio = true, bool Subtitle = true)
        {
            // Set default flag for first audio track in order
            // If specified, sets default flag for first subtitle track or first forced subtitle track, ignoring any subtitle tracks which turned out to be empty (if conversion has taken place)
            
            SortedList<double, Track> trks = new SortedList<double, Track>();
            double d = 0;
            foreach (Track t in Tracks)
            {
                foreach (Track track in new Track[] { t, t.Child })
                {
                    if (track != null && track.Include && !track.IsEmpty)
                        trks.Add((double)track.OrderWeight + (d += 0.001), track);
                }
            }

            bool audioSet = false, subtitleSet = false;
            foreach(Track t in trks.Values)
            {
                if (t.TrackType == TrackType.Audio && Audio && !audioSet)
                { t.Default = true; audioSet = true; }
                else if (t.TrackType == TrackType.Subtitle && Subtitle && !subtitleSet)
                {
                    if (subtitleDefault == SubtitleDefault.No)
                        t.Default = false;
                    else if ((subtitleDefault == SubtitleDefault.First) || (subtitleDefault == SubtitleDefault.FirstForced && t.SubtitleForced))
                    { t.Default = true; subtitleSet = true; }
                }

                if ((!Audio || audioSet) && (!Subtitle || subtitleSet))
                    break;
            }
        }

    }
}
