using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMKV.Domain
{
    public class TrackState
    {
        public TrackState()
        {
            UseStaticValues = true;
        }

        public virtual long ID { get; set; }

        private int index;
        public virtual int TrackIndex
        {
            get { return index; }
            set
            {
                if (value == index) return;

                index = value;
                updateTrack();
            }
        }

        private TitleState title;
        public virtual TitleState Title
        {
            get { return title; }
            set
            {
                if (title == value) return;

                title = value;
                updateTrack();
            }
        }

        public virtual bool Expanded { get; set; }

        private bool useStaticValues;
        public virtual bool UseStaticValues                                     // If True, properties will reflect database values.
        {                                                                       // If False, properties will reflect the actual MKVTools.Source object if it is available
            get { return useStaticValues; }
            set
            {
                if (value == useStaticValues) return;

                useStaticValues = value;
                updateTrack();

                if (!value && Track != null)
                {
                    // Copy properties to MKVTools.Track
                    Track.Include = include;

                    if (name != null)
                    {
                        Track.Name = name;
                        Track.MetadataLanguage = metadataLanguage;
                        Track.Language = language;
                        Track.MKVFlags = mkvFlags;
                        Track.OrderWeight = orderWeight;
                    }
                }
            }
        }

        public virtual bool CopySettings(TrackState Target)
        {
            if (Title.Source.Hash != Target.Title.Source.Hash) return false;

            // Track settings
            Target.Include = Include;
            Target.Expanded = Expanded;

            // Properties
            Target.Name = Name;
            Target.MetadataLanguage = MetadataLanguage;
            Target.Language = Language;
            Target.MKVFlags = MKVFlags;
            Target.OrderWeight = OrderWeight;

            return true;
        }

        private void updateTrack()
        {
            if (!useStaticValues &&
                Title != null &&
                Title.TitleIndex >= 0 &&
                TrackIndex >= 0 &&
                Title.Source.Source != null &&
                Title.Source.Source.Titles != null &&
                Title.Source.Source.Titles.GetLength(0) > Title.TitleIndex &&
                Title.Source.Source.Titles[Title.TitleIndex].TracksAll != null &&
                Title.Source.Source.Titles[Title.TitleIndex].TracksAll.Count > TrackIndex)
                track = Title.Source.Source.Titles[Title.TitleIndex].TracksAll[TrackIndex];
            else
                track = null;
        }

        private MKVTools.Track track;
        public virtual MKVTools.Track Track
        { get { return track; } }

        private bool include;
        public virtual bool Include
        {
            get
            {
                return (Track != null
                    ? Track.Include
                    : include);
            }
            set
            {
                if (Track != null)
                    Track.Include = value;
                else
                    include = value;
            }
        }

        private string name;
        public virtual string Name
        {
            get
            {
                return (Track != null
                    ? Track.Name
                    : name);
            }
            set
            {
                if (Track != null)
                    Track.Name = value;
                else
                    name = value;
            }
        }

        private MKVTools.Language metadataLanguage;
        public virtual MKVTools.Language MetadataLanguage
        {
            get
            {
                return (Track != null
                    ? Track.MetadataLanguage
                    : metadataLanguage);
            }
            set
            {
                if (Track != null)
                    Track.MetadataLanguage = value;
                else
                    metadataLanguage = value;
            }
        }

        private MKVTools.Language language;
        public virtual MKVTools.Language Language
        {
            get
            {
                return (Track != null
                    ? Track.Language
                    : language);
            }
            set
            {
                if (Track != null)
                    Track.Language = value;
                else
                    language = value;
            }
        }

        private string mkvFlags;
        public virtual string MKVFlags
        {
            get
            {
                return (Track != null
                    ? Track.MKVFlags
                    : mkvFlags);
            }
            set
            {
                if (Track != null)
                    Track.MKVFlags = value;
                else
                    mkvFlags = value;
            }
        }

        private int orderWeight;
        public virtual int OrderWeight
        {
            get
            {
                return (Track != null
                    ? Track.OrderWeight
                    : orderWeight);
            }
            set
            {
                if (Track != null)
                    Track.OrderWeight = value;
                else
                    orderWeight = value;
            }
        }

    }
}
