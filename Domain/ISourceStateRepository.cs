using System;
using System.Collections.Generic;

namespace BatchMKV.Domain
{
    public interface ISourceStateRepository
    {
        void Add(SourceState source);
        void Update(SourceState source);
        void Remove(SourceState source);
        ICollection<SourceState> GetRestoreOnStart();
        SourceState GetMatch(string Hash, LocationType LocationType, string Location, long ID = 0);
        void Purge(DateTime LastUpdatedBefore);
    }
}
