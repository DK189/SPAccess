using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        public object this[string Name]
        {
            get
            {
                string rawValue = GetMeta(Name);

                if (rawValue == null)
                {
                    byte[] buffer = null;

                    try
                    {
                        buffer = Convert.FromBase64String(rawValue);
                    }
                    catch
                    {

                    }

                    if (buffer != null)
                    {
                        var ms = new MemoryStream();
                        var bf = new BinaryFormatter();
                        ms.Write(buffer, 0, buffer.Length);
                        ms.Position = 0;
                        return bf.Deserialize(ms);
                    }
                }
                return null;
            }

            set
            {
                var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                byte[] buffer = ms.GetBuffer();

                string rawValue = Convert.ToBase64String(buffer);

                SetMeta(Name, rawValue);
            }
        }

        internal abstract string GetMeta(string Name);
        internal abstract void SetMeta(string Name, string rawValue);
    }
}
