using FluentNHibernate.Mapping;
using BatchMKV.Domain;

namespace Mango.Mappings
{
    public class SourceStateMap : ClassMap<SourceState>
    {
        public SourceStateMap()
        {
            Table("Source");

            Id(x => x.ID);
            Map(x => x.LastUpdated).Not.Nullable();

            Map(x => x.Hash).Nullable();
            Map(x => x.Convert).Not.Nullable();
            Map(x => x.RestoreOnStart).Not.Nullable();

            Map(x => x.OutputSettingsOutputAtSource).Column("OutputAtSource").Not.Nullable();
            Map(x => x.OutputSettingsOutputPath).Column("OutputPath").Nullable();
            Map(x => x.OutputSettingsSourceAction).Column("SourceAction").Not.Nullable();
            Map(x => x.OutputSettingsUseDefault).Column("OutputUseDefault").Not.Nullable();

            Map(x => x.DiscType).Nullable().CustomType<byte>();
            Map(x => x.DiscName).Nullable();
            Map(x => x.DiscMetadataLanguage).Nullable().CustomType<short>();

            HasMany(x => x.Titles).Cascade.All();       // .KeyColumn("ID")

            Map(x => x.LocationType).Not.Nullable().CustomType<byte>();
            Map(x => x.Location).Not.Nullable();

            Map(x => x.CurrentStatus).Not.Nullable().CustomType<short>();
            Map(x => x.SourceDeleted).Not.Nullable();
            Map(x => x.SourceMoved).Not.Nullable();
        }
    }

    public class TitleStateMap : ClassMap<TitleState>
    {
        public TitleStateMap()
        {
            Table("Title");
            Id(x => x.ID);

            Map(x => x.TitleIndex).Not.Nullable();

            References(x => x.Source).Column("SourceID").Cascade.All();

            HasMany(x => x.Tracks).Cascade.All();       // .KeyColumn("ID")

            Map(x => x.ScanInfo).Not.Nullable();
            Map(x => x.Include).Not.Nullable();
            Map(x => x.Expanded).Not.Nullable();

            Map(x => x.Result).Not.Nullable().CustomType<short>();

            Map(x => x.Name).Nullable();
            Map(x => x.MetadataLanguage).Nullable().CustomType<short>();
            Map(x => x.Filename).Not.Nullable();
        }
    }

    public class TrackStateMap : ClassMap<TrackState>
    {
        public TrackStateMap()
        {
            Table("Track");
            Id(x => x.ID);

            Map(x => x.TrackIndex).Not.Nullable();

            References(x => x.Title).Column("TitleID").Cascade.All();

            Map(x => x.Include).Not.Nullable();
            Map(x => x.Expanded).Not.Nullable();

            Map(x => x.Name).Nullable();
            Map(x => x.MetadataLanguage).Nullable().CustomType<short>();
            Map(x => x.Language).Nullable().CustomType<short>();
            Map(x => x.MKVFlags).Not.Nullable();
            Map(x => x.OrderWeight).Not.Nullable();
        }
    }

}
