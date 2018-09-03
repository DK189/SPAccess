using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ViewModel
{
    public abstract class SPModel
    {
        public SPAccess SPAccess { get; internal set; }

        internal SPModel (SPAccess SPAccess)
        {
            this.SPAccess = SPAccess;
        }
    }
}
