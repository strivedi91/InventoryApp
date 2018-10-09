using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

using System.Reflection;
using InventoryApp_DL.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using InventoryApp_DL.Infrastructure;

namespace InventoryApp_DL.DataContext
{
    public class DataContext : DbContext, IDataContext, IDataContextAsync
    {
        private readonly Guid _instanceId;

        public DataContext()
            : base("DefaultConnection")
        {
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public DbContext DbContext { get { return this; } }

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public override Task<int> SaveChangesAsync()
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync();
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).ObjectState);
        }
        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            
            //modelBuilder.Entity<AspNetRoles>().HasKey(t => t.Id);
            //modelBuilder.Entity<AspNetUserClaims>().HasKey(t => t.Id);
            //modelBuilder.Entity<AspNetUserLogins>().HasKey(t => t.UserId);
            //modelBuilder.Entity<AspNetUsers>().HasKey(t => t.Id);
            //modelBuilder.Entity<Frequency>().HasKey(t => t.ID);
            //modelBuilder.Entity<Product>().HasKey(t => t.ID);
            //modelBuilder.Entity<Route>().HasKey(t => t.ID);
            //modelBuilder.Entity<RouteWorksheet>().HasKey(t => t.ID);
            //modelBuilder.Entity<RouteWorksheetItem>().HasKey(t => t.ID);
            //modelBuilder.Entity<Stop>().HasKey(t => t.ID);
            
        }
    }
}
