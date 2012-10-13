/* 
ServerChecker v4 operates and manages various kinds of software on server systems.
Copyright (C) 2010 Stijn Devriendt

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Security
{
    class License : SC.Interfaces.ILicense
    {
        private Guid id;
        private string name;

        public License(Guid guid, string licenseName)
        {
            this.id = guid;
            this.name = licenseName;
        }

        #region ILicense Members

        public void AssertValid()
        {
            LicenseManager.Instance.AssertValid(this);
        }

        public bool IsValid()
        {
            return LicenseManager.Instance.IsValid(this);
        }

        public void Invalidate()
        {
            LicenseManager.Instance.Invalidate(this);
        }

        #endregion

        internal Guid ID
        {
            get
            {
                return id;
            }
        }
        internal string LicenseName
        {
            get
            {
                return name;
            }
        }
        ~License()
        {
            System.Diagnostics.Trace.Assert(!LicenseManager.Instance.IsValid(this));
            if (LicenseManager.Instance.IsValid(this))
            {
                LicenseManager.Logger.Error("License " + name + " not cleaned up.");
                LicenseManager.Instance.Invalidate(this);
            }
        }
    }
    class LicenseInfo
    {
        public LicenseInfo(uint count, string licenseName)
        {
            this.count = count;
            this.licenseName = licenseName;
            this.startTime = DateTime.Now - new TimeSpan(1, 0, 0);
            this.validTimeSpan = new TimeSpan(5, 0, 0);
        }
        private uint count;
        private DateTime startTime;
        private TimeSpan validTimeSpan;
        private string licenseName;
        public uint Count
        {
            get
            {
                return count;
            }
        }
        public bool IsValid
        {
            get
            {
                DateTime now = DateTime.Now;
                return startTime < now && now < startTime + validTimeSpan;
            }
        }
    }

    [System.Security.SecurityCritical()]
    public class LicenseManager
    {
        private System.Collections.Generic.Dictionary<Guid, License> licenses = new Dictionary<Guid, License>();
        private Dictionary<string, LicenseInfo> licenseinfos = new Dictionary<string, LicenseInfo>();
        internal static log4net.ILog Logger = null;

        private static LicenseManager instance;
        public static void Initialize()
        {
            if (instance != null)
                throw new InvalidOperationException("Already initialised the License Manager");

            Logger = log4net.LogManager.GetLogger("License Manager");
            instance = new LicenseManager();
        }
        public static LicenseManager Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException("License Manager not instantiated");
                return instance;
            }
        }

        private static SC.Interfaces.LicenseRequest GetLicenseRequest(Type t)
        {
            SC.Interfaces.LicenseAttribute[] attrs = (SC.Interfaces.LicenseAttribute[])t.GetCustomAttributes(typeof(SC.Interfaces.LicenseAttribute), false);
            if (attrs.Length == 1)
                return attrs[0].GetLicenseRequest();
            else
                return null;
        }
        private static bool RequiresLicense(Type t)
        {
            IList<System.Reflection.CustomAttributeData> attrs = System.Reflection.CustomAttributeData.GetCustomAttributes(t);
            foreach (System.Reflection.CustomAttributeData attr in attrs)
            {
                if (attr.Constructor.DeclaringType.FullName == typeof(SC.Interfaces.LicenseAttribute).FullName)
                    return true;
            }
            return false;
        }
        private static bool RequiresLicense(object o)
        {
            return RequiresLicense(o.GetType());
        }
        private LicenseManager()
        {
            licenseinfos.Add("DefaultPlugins", new LicenseInfo(10, "DefaultPlugins"));
        }
        public void License(SC.Interfaces.ILicensee licensee)
        {
            if (RequiresLicense(licensee))
            {
                Guid guid = Guid.NewGuid();
                while (licenses.ContainsKey(guid) || guid == Guid.Empty)
                    guid = Guid.NewGuid();

                SC.Interfaces.LicenseRequest req = GetLicenseRequest(licensee.GetType());
                string licName = req.LicenseName;

                bool valid = false;
                while (req != null)
                {
                    if (licenseinfos.ContainsKey(req.LicenseName))
                    {
                        LicenseInfo info = licenseinfos[req.LicenseName];
                        if (!(valid = info.Count > GetUsedLicenseCount(req.LicenseName)))
                        {
                            valid = false;
                            break;
                        }
                        else if (info.IsValid)
                        {
                            req = null;
                        }
                    }
                    else
                    {
                        valid = false;
                        req = null;
                    }
                }

                if (valid)
                {
                        Logger.Info("Granting license " + licName + " to " + licensee.ToString());
                        License lic = new License(guid, licName);
                        licensee.AssignLicense(lic);
                        licenses.Add(guid, lic);
                }
                else
                {
                        Logger.Info("No license " + licName + " available for " + licensee.ToString());
                        licensee.AssignLicense(new License(Guid.Empty, licName));
                }
            }
        }
        internal void AssertValid(License lic)
        {
            if (!IsValid(lic))
                throw new SC.Interfaces.LicenseException("Invalid license");
        }
        internal void Invalidate(License lic)
        {
            Guid id = lic.ID;

            AssertValid(lic);
            licenses.Remove(id);
        }
        internal bool IsValid(License lic)
        {
            Guid id = lic.ID;
            return licenses.ContainsKey(id) && licenses[id].Equals(lic);
        }
        private int GetUsedLicenseCount(string licenseName)
        {
            int count = 0;
            foreach (License lic in licenses.Values)
            {
                if (lic.LicenseName == licenseName)
                    ++count;
            }
            return count;
        }
    }
}
