using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public abstract class _SPManager
    {
        public SPAccess SPAccess { get; internal set; }

        internal _SPManager(SPAccess SPAccess)
        {
            this.SPAccess = SPAccess;
        }

        internal SPAccessDB DB
        {
            get
            {
                return SPAccess.DB;
            }
        }
    }
}
