using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchMKV.Domain
{
    public class TitleState
    {
        public TitleState()
        {
            UseStaticValues = true;
            Result = MKVTools.Title.ConversionResult.NotAvailable;
        }

        public virtual long ID { get; set; }                                    // Database ID

        private int index;
        public virtual int TitleIndex
        {
            get { return index; }
            set
            {
                if (value == index) return;

                index = value;
                updateTitle();
            }
        }

        private SourceState source;
        public virtual SourceState Source
        {
            get { return source; }
            set
            {
                if (value == source) return;

                source = value;
                updateTitle();
            }
        }

        public virtual ICollection<TrackState> Tracks { get; set; }

        public virtual bool Expanded { get; set; }                              // NH + UI only (not applicable on MKVTools.Title)

        private bool useStaticValues;
        public virtual bool UseStaticValues                                     // If True, properties will reflect database values.
        {                                                                       // If False, properties will reflect the actual MKVTools.Source object if it is available
            get { return useStaticValues; }
            set {
                if (value == useStaticValues) return;

                useStaticValues = value;
                updateTitle();

                if (!value && Title != null)
                {
                    // Copy properties to MKVTools.Title
                    Title.Include = include;
                    Title.ResetResult(result);

                    if (name != null)
                    {
                        Title.Name = name;
                        Title.MetadataLanguage = metadataLanguage;
                        Title.OutputFilename = filename;
                    }
                }
            }
        }

        public virtual bool CopySettings(TitleState Target)
        {
            if (Source.Hash != Target.Source.Hash || Tracks.Count != Target.Tracks.Count) return false;

            // Copy title settings...
            Target.Include = Include;
            Target.Expanded = Expanded;
            Target.Result = Result;
            Target.Name = Name;
            Target.MetadataLanguage = MetadataLanguage;
            Target.Filename = Filename;

            // Copy track settings
            for (int i = 0; i < Tracks.Count; i++)
                if (!Tracks.ElementAt(i).CopySettings(Target.Tracks.ElementAt(i)))
                    return false;

            return true;
        }

        private void updateTitle()
        {
            if (!useStaticValues && Source.Source != null && Source.Source.Titles != null && TitleIndex >= 0 && Source.Source.Titles.GetLength(0) > TitleIndex)
                title = Source.Source.Titles[TitleIndex];
            else
                title = null;
        }

        private MKVTools.Title title;
        public virtual MKVTools.Title Title
        { get { return title; } }

        private bool include;
        public virtual bool Include
        {
            get
            {
                return (Title != null
                    ? Title.Include
                    : include);
            }
            set
            {
                if (Title != null)
                    Title.Include = value;
                else
                    include = value;
            }
        }

        private string scanInfo;
        public virtual string ScanInfo
        {
            get
            {
                return (Title != null
                    ? Title.ScanInfo
                    : scanInfo);
            }
            set
            {
                scanInfo = value;
            }
        }

        private MKVTools.Title.ConversionResult result;
        public virtual MKVTools.Title.ConversionResult Result
        {
            get
            {
                return (Title != null
                    ? Title.Result
                    : result);
            }
            set
            {
                if (Title != null)
                    Title.ResetResult(value);
                else
                    result = value;
            }
        }

        private string name;
        public virtual string Name
        {
            get
            {
                return (Title != null
                    ? Title.Name
                    : name);
            }
            set
            {
                if (Title != null)
                    Title.Name = value;
                else
                    name = value;
            }
        }

        private MKVTools.Language metadataLanguage;
        public virtual MKVTools.Language MetadataLanguage
        {
            get
            {
                return (Title != null
                    ? Title.MetadataLanguage
                    : metadataLanguage);
            }
            set
            {
                if (Title != null)
                    Title.MetadataLanguage = value;
                else
                    metadataLanguage = value;
            }
        }

        private string filename;
        public virtual string Filename
        {
            get
            {
                return (Title != null
                    ? Title.OutputFilename
                    : filename);
            }
            set
            {
                if (Title != null)
                    Title.OutputFilename = value;
                else
                    filename = value;
            }
        }

    }
}
