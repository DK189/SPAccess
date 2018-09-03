using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    partial class SPAccessDB
    {
        public SPAccessDB(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public SPAccessDB Reload ()
        {
            Reload(this.UserIdentity);
            Reload(this.GroupIdentity);
            Reload(this.PermissionIdentity);
            Reload(this.Session);
            return this;
        }

        public DbSet<TEntity> Reload<TEntity>(DbSet<TEntity> EntityLocal) where TEntity : class
        {
            Reload(EntityLocal.Local);
            return EntityLocal;
        }

        public ObservableCollection<TEntity> Reload<TEntity>(ObservableCollection<TEntity> EntityLocal) where TEntity : class
        {
            foreach (var entity in EntityLocal)
            {
                Reload(entity);
            }
            return EntityLocal;
        }

        public TEntity Reload<TEntity>(TEntity Entity) where TEntity : class
        {
            this.Entry(Entity).Reload();
            return Entity;
        }
    }
}