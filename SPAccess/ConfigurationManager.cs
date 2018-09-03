using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public class ConfigurationManager : _SPManager
    {
        System.Configuration.Configuration configObject;

        internal ConfigurationManager(SPAccess SPAccess) : base(SPAccess)
        {
            configObject = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            var txtsysinfo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), AppDomain.CurrentDomain.FriendlyName + ".sysinf");
            try
            {
                this["SystemInfo"] = txtsysinfo + ", Writing...";
                var EnvVars = new Dictionary<object, object>();
                foreach (var EnvKey in Environment.GetEnvironmentVariables().Keys)
                {
                    var EnvVal = Environment.GetEnvironmentVariables()[EnvKey];
                    EnvVars.Add(EnvKey, EnvVal);
                }
                File.WriteAllLines(
                    txtsysinfo,
                    Enumerable.Empty<string>()
                        .Concat(
                            new string[]
                            {
                                "------ Custom Info ------"
                            }
                        )
                        .Concat(
                            new string[] {
                                string.Format("{0}: {1}", "Environment.CurrentDirectory", Environment.CurrentDirectory),
                                string.Format("{0}: {1}", "AppDomain.CurrentDomain", AppDomain.CurrentDomain),
                                string.Format("{0}: {1}", "AppDomain.CurrentDomain.BaseDirectory", AppDomain.CurrentDomain.BaseDirectory),
                                string.Format("{0}: {1}", "Environment.CurrentManagedThreadId", Environment.CurrentManagedThreadId),
                                string.Format("{0}: {1}", "configObject.FilePath", configObject.FilePath),
                                string.Format("{0}: {1}", "StartTime", StartTime)
                            }
                        )
                        .Concat(
                            new string[]
                            {
                                "------ Arguments ------"
                            }
                        )
                        .Concat(
                            Environment.GetCommandLineArgs().Select(
                                (arg, index) => string.Format("{0}: {1}", index, arg)
                            ).ToList()
                        )
                        .Concat(
                            new string[]
                            {
                                "------ Environment Variables ------"
                            }
                        )
                        .Concat(
                            EnvVars.Select(
                                ev => string.Format("{0}: {1}", ev.Key, ev.Value)
                            ).ToList()
                        )
                    ,
                    Encoding.UTF8
                );
                this["SystemInfo"] = txtsysinfo + ", Writed!";
            }
            catch
            {
                this["SystemInfo"] = txtsysinfo + ", Writing Error!";
            }
        }

        public string this[string Key]
        {
            get
            {
                var setting = configObject.AppSettings.Settings[Key];
                return setting != null ? setting.Value : null;
            }
            set
            {
                var setting = configObject.AppSettings.Settings[Key];
                if (setting != null)
                {
                    setting.Value = value;
                }
                else
                {
                    configObject.AppSettings.Settings.Add(Key, value);
                }
                configObject.Save(System.Configuration.ConfigurationSaveMode.Modified);
            }
        }

        public DateTime StartTime
        {
            get
            {
                if (string.IsNullOrEmpty(this["StartTime"]))
                {
                    StartTime = DateTime.Now;
                }
                return DateTime.FromBinary(long.Parse(this["StartTime"], System.Globalization.NumberStyles.Integer));
            }
            internal set
            {
                this["StartTime"] = "" + value.ToBinary();
            }
        }

        public int SAID
        {
            get
            {
                return int.Parse(this["SAID"]);
            }
            internal set
            {
                this["SAID"] = "" + value;
            }
        }
    }
}
