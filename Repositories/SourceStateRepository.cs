using System.Collections.Generic;
using NHibernate;
using BatchMKV.Domain;
using NHibernate.Criterion;
using System.Linq;
using System;

namespace BatchMKV.Repositories
{
    public class SourceStateRepository : ISourceStateRepository
    {
        private void initialize()
        {
            sources = new List<SourceState>();
        }

        public void Add(SourceState source)
        {
            if (sources == null) initialize();
            if (!sources.Contains(source)) sources.Add(source);

            // TODO: This should not add duplicate sources if it is already in the repository (database-wise).

            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                source.LastUpdated = DateTime.Now;
                session.Save(source);
                transaction.Commit();
            }

            clearMatches(source);
        }

        public void Update(SourceState source)
        {
            if (sources == null) initialize();
            if (!sources.Contains(source)) sources.Add(source);

            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                source.LastUpdated = DateTime.Now;
                session.Update(source);
                transaction.Commit();
            }

            clearMatches(source);
        }

        private void clearMatches(SourceState source, bool ExactMatch = true)
        {
            // Remove other sources which are an exact match for this new source.

            // This can only be done if the source has been saved and assigned a database ID.
            if (source.ID <= 0) return;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                // Get exact matches which are not this source.
                var sourcesRemove = session.CreateCriteria<SourceState>()
                    .Add(Restrictions.Eq("Hash", source.Hash))
                    .Add(Restrictions.Eq("LocationType", (byte)source.LocationType))
                    .Add(Restrictions.Eq("Location", source.Location).IgnoreCase())
                    .Add(Restrictions.Not(Restrictions.Eq("ID", source.ID)))
                    .List<SourceState>();

                // Delete those matches
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (SourceState s in sourcesRemove)
                    {
                        session.Delete(s);
                        if (sources != null && sources.Contains(s)) sources.Remove(s);
                    }
                    transaction.Commit();
                }
            }
        }

        public void Remove(SourceState source)
        {
            if (sources == null) initialize();
            if (sources.Contains(source)) sources.Remove(source);

            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(source);
                transaction.Commit();
            }
        }

        public void Purge(DateTime LastUpdatedBefore)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                // Get sources which were last updated before LastUpdatedBefore
                var sourcesRemove = session.CreateCriteria<SourceState>()
                    .Add(Restrictions.Or(Restrictions.Lt("LastUpdated", LastUpdatedBefore), Restrictions.IsNull("Hash")))
                    .List<SourceState>();
                if (sourcesRemove.Count == 0) return;

                // Delete those sources
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (SourceState s in sourcesRemove)
                    {
                        session.Delete(s);
                        if (sources != null && sources.Contains(s)) sources.Remove(s);
                    }
                    transaction.Commit();
                }

                // Compact database
                session.CreateSQLQuery("VACUUM;").ExecuteUpdate();
            }

            if (sources != null) sources.Clear();
        }

        private ICollection<SourceState> sources;

        public ICollection<SourceState> GetRestoreOnStart()
        {
            if (sources == null) initialize();

            // Add any SourceStates with RestoreOnStart to sources.
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var sourcesRestoreOnStart = session.CreateCriteria<SourceState>()
                    .Add(Restrictions.Eq("RestoreOnStart", true))
                    .List<SourceState>();

                foreach(SourceState source in sourcesRestoreOnStart)
                {
                    if (!sources.Contains(source))
                    {
                        // Load Titles and Tracks (prevent lazy loading)
                        NHibernateUtil.Initialize(source.Titles);
                        foreach(TitleState title in source.Titles)
                            NHibernateUtil.Initialize(title.Tracks);

                        sources.Add(source);
                    }
                }
            }

            // Return only those SourceStates where RestoreOnStart == true.
            return sources.Where(x => x.RestoreOnStart).ToList();
        }

        public SourceState GetMatch(string Hash, LocationType LocationType, string Location, long ID = 0)
        {
            // Check if there is a source state in the repository which matches the given information.
            // It must match the Hash. If there are multiple matches, LocationType+Location is the tiebreaker.
            // If multiple matches and no match for LocationType+Location, the SourceState which was last updated/saved is returned.

            // Check local cache for exact match.
            // If exact match is found, return that.
            foreach (SourceState s in sources)
                if ((ID == 0 || s.ID != ID) && s.Matches(Hash, LocationType, Location) == SourceState.SourceMatch.ExactMatch)
                    return s;

            // If exact match is not found in local cache, check for exact or hash match in full repository.
            SourceState source = null;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                // Check for exact match:
                source = session.CreateCriteria<SourceState>()
                    .Add(Restrictions.Eq("Hash", Hash))
                    .Add(Restrictions.Eq("LocationType", (byte)LocationType))
                    .Add(Restrictions.Eq("Location", Location).IgnoreCase())
                    .List<SourceState>()
                    .FirstOrDefault();

                if (source == null)
                    // Exact match was not found, get the first hash match.
                    source = session.CreateCriteria<SourceState>()
                        .Add(Restrictions.Eq("Hash", Hash))
                        .AddOrder(Order.Desc("LastUpdated"))
                        .List<SourceState>()
                        .FirstOrDefault();

                if (source != null)
                {
                    NHibernateUtil.Initialize(source.Titles);
                    foreach (TitleState title in source.Titles)
                        NHibernateUtil.Initialize(title.Tracks);
                }
            }

            return source;
        }

    }
}
