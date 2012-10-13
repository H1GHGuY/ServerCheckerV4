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
    [Serializable]
    public class UserSettings
    {
        public UserSettings() { }
        public UserSettings(string username, string password, DateTime lastConnect)
        {
            Username = username;
            Password = password;
            LastConnect = lastConnect;
        }
        public string Username = string.Empty;
        public string Password = string.Empty;
        public DateTime LastConnect;
    }

    [Serializable]
    [System.Xml.Serialization.XmlRoot("Security")]
    public class SecuritySettings
    {
        public List<SecuritySubjectSettings> Subjects = new List<SecuritySubjectSettings>();
        public List<UserSettings> Users = new List<UserSettings>();
        public List<SC.Security.NetworkSettings> Clients = new List<SC.Security.NetworkSettings>();

        public SecuritySettings() { }
        public SecuritySettings(List<SecuritySubjectSettings> subjects, List<UserSettings> users, List<SC.Security.NetworkSettings> clients)
        {
            this.Subjects = subjects;
            this.Users = users;
            this.Clients = clients;
        }
    }

    internal class UserInfo
    {
        private string username;
        private string password;
        private DateTime lastConnect = DateTime.MinValue;
        public UserInfo(UserSettings settings)
        {
            this.username = settings.Username;
            this.password = settings.Password;
            this.lastConnect = settings.LastConnect;
        }
        public UserInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public string Username
        {
            get
            {
                return username;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public void SetUserConnected()
        {
            lastConnect = DateTime.Now;
        }
        public DateTime LastConnect
        {
            get
            {
                return lastConnect;
            }
        }
        public UserSettings Settings
        {
            get
            {
                return new UserSettings(username, password, lastConnect);
            }
        }
    }

    [Serializable]
    [System.Security.SecurityCritical()]
    public class SecurityManager : MarshalByRefObject, SC.Interfaces.ISecurityManager, SC.Interfaces.IAuthenticationManager
    {
        private System.Collections.Generic.Dictionary<Guid, SecuritySubject> acls = new Dictionary<Guid, SecuritySubject>();
        private System.Collections.Generic.Dictionary<string, SecuritySubject> subjects = new Dictionary<string, SecuritySubject>();
        private System.Collections.Generic.Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();
        private System.Collections.Generic.List<SC.Security.Network> clients = new List<SC.Security.Network>();

        private Guid securityGuid = Guid.Empty;
        private System.Threading.ReaderWriterLock secLock = new System.Threading.ReaderWriterLock();
        private SC.Interfaces.ISettingsProvider settingsProviders = new SC.Settings.SettingsProvider("SecurityManager");

        internal readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Security");

        public const string USER_ROLE = "SC.SECURITY.USER_ROLE";
        internal const string SYSTEM_ACCOUNT = "SC.SECURITY.SYSTEM_ACCOUNT";
        internal const string SYSTEM_ROLE = "SC.SECURITY.SYSTEM_ROLE";

        public static readonly SecurityManager Instance = new SecurityManager();

        private SecurityManager()
        {
            try
            {
                Logger.Info("Loading Security Settings...");
                RestoreSettings();

                securityGuid = RegisterSubject("SecurityManager", null, "Administrator");
                FixAdministrator();

                FixNetworks();
            }
            catch (Exception e)
            {
                Logger.Error("An unexpected exception occurred during initialisation of the security manager.", e);
                throw;
            }
            AuthenticateAsSystem();
        }
        
        #region Settings
        private SecuritySettings Settings
        {
            get
            {
                List<SecuritySubjectSettings> subsett = new List<SecuritySubjectSettings>();
                foreach (SecuritySubject subj in subjects.Values)
                    subsett.Add(subj.Settings);

                List<UserSettings> usersett = new List<UserSettings>();
                foreach (UserInfo user in users.Values)
                    usersett.Add(user.Settings);

                List<NetworkSettings> clientsett = new List<NetworkSettings>();
                foreach (Network net in clients)
                    clientsett.Add(net.Settings);

                return new SecuritySettings(subsett, usersett, clientsett);
            }
            set
            {
                System.Collections.Generic.Dictionary<string, SecuritySubject> subs = new Dictionary<string, SecuritySubject>();
                System.Collections.Generic.Dictionary<string, UserInfo> usrs = new Dictionary<string, UserInfo>();
                foreach (SecuritySubjectSettings subsett in value.Subjects)
                {
                    SecuritySubject subj = new SecuritySubject(subsett);
                    try
                    {
                        subs.Add(subj.Name, subj);
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error adding security settings for subject " + subj.Name + ". Settings were ignored.", e);
                    }
                }
                foreach (UserSettings user in value.Users)
                {
                    if (!users.ContainsKey(user.Username))
                        usrs[user.Username] = new UserInfo(user);
                    else
                        Logger.Error("Found duplicate user " + user.Username + ". Ignored this one.");
                        
                }
                using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
                {
                    subjects = subs;
                    Logger.Info("Subjectlist: " + string.Join(", ", new System.Collections.Generic.List<string>(subjects.Keys).ToArray()));
                    users = usrs;
                    Logger.Info("Userlist: " + string.Join(", ", new System.Collections.Generic.List<string>(users.Keys).ToArray()));
                    foreach (NetworkSettings netsett in value.Clients)
                    {
                        try
                        {
                            AddAllowedClientNetwork(new Network(netsett));
                        }
                        catch (Exception)
                        {
                            // Logged in function itself
                        }
                    }
                }
            }
        }
        private void RestoreSettings()
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                SecuritySettings mysettings = settingsProviders.RestoreSettings(typeof(SecuritySettings)) as SecuritySettings;
                if (mysettings != null)
                {
                    Settings = mysettings;
                }
            }
        }
        private void SaveSettings()
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                settingsProviders.SaveSettings(Settings);
            }
        }
        #endregion
        protected void DemandAdministratorPermissions()
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                try
                {
                    acls[securityGuid].DemandPermission();
                }
                catch (KeyNotFoundException)
                {
                    Logger.Fatal("SecurityManager did not register to itself.");
                    throw;
                }
                catch (System.Security.SecurityException e)
                {
                    Logger.Error("User " + GetCurrentUser() + " was denied administrator permissions.", e);
                    throw;
                }
                catch (Exception e)
                {
                    Logger.Error("Unexpected exception occurred while demanding administrator permissions.", e);
                    throw new System.Security.SecurityException("An unexpected exception occurred while demanding administrator permissions. Operation was denied.", e);
                }
            }
        }
        private void FixAdministrator()
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                if (users.Count == 0)
                {
                    Logger.Warn("No users found, adding Administrator account with default password");

                    l.UpgradeToWriterLock();
                    UserInfo adminInfo = new UserInfo("Administrator", "ServerChecker4");
                    users.Add(adminInfo.Username, adminInfo);
                    l.DowngradeToReaderLock();
                }

                SecuritySubject me = acls[securityGuid];
                System.Collections.Specialized.StringCollection permissions = new System.Collections.Specialized.StringCollection();
                permissions.AddRange(me.GetPermissions());

                if (permissions.Count == 0)
                {
                    Logger.Warn("No permissions found for SecurityManager. Adding permission for Administrator account");

                    l.UpgradeToWriterLock();
                    me.AddPermission("Administrator");
                    l.DowngradeToReaderLock();
                }
            }
        }
        public void Authenticate(string user, string password)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                if (!users.ContainsKey(user))
                {
                    Logger.Error("Local login failed for username " + user);
                    throw new System.Security.SecurityException("Invalid authentication. User or password incorrect.");
                }

                if (users[user].Password == password)
                {
                    if (users[user].LastConnect < DateTime.Now - new TimeSpan(0, 10, 0))
                        Logger.Info("User " + user + " authenticated locally");
                    users[user].SetUserConnected();
                    System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(user), new string[] { USER_ROLE });
                }
                else
                {
                    Logger.Error("User " + user + " failed to authenticate locally.");
                    throw new System.Security.SecurityException("Invalid authentication. User or password incorrect.");
                }
            }
        }
        public void Authenticate(string user, string base64HMAC, byte[] nonce, System.IO.MemoryStream stream)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                if (!users.ContainsKey(user))
                {
                    Logger.Error("Someone tried to login with username " + user);
                    throw new System.Security.SecurityException("Invalid authentication. Check your username and password.");
                }
                string password = users[user].Password;
                byte[] key = new System.Security.Cryptography.Rfc2898DeriveBytes(password, nonce, 997).GetBytes(64);

                System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512(key);
                string computedBase64HMAC = Convert.ToBase64String(hmac.ComputeHash(stream.ToArray()));
                
                stream.Seek(0, System.IO.SeekOrigin.Begin);

                if (base64HMAC == computedBase64HMAC)
                {
                    if (users[user].LastConnect < DateTime.Now - new TimeSpan(0, 10, 0))
                        Logger.Info("User " + user + " authenticated.");
                    users[user].SetUserConnected();
                    System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(user), new string[] { USER_ROLE });
                }
                else
                {
                    Logger.Error("User " + user + " failed to authenticate.");
                    throw new System.Security.SecurityException("Invalid authentication. Check your username and password.");
                }
            }
        }
        public void UnAuthenticate()
        {
            System.Threading.Thread.CurrentPrincipal = null;
        }
        private void AuthenticateAsSystem()
        {
            System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(SYSTEM_ACCOUNT), new string[] { SYSTEM_ROLE });
        }
        
        #region User Management
        private string GetCurrentUser()
        {
            System.Security.Principal.IPrincipal principal = System.Threading.Thread.CurrentPrincipal;
            if (principal != null)
                return principal.Identity.Name;
            else
                return "-unauthenticated user-";
        }
        public void AddUser(string username, string password)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                if (users.ContainsKey(username))
                    throw new SC.Interfaces.SCException("A user with name " + username + " already exists.");
                if (username == SYSTEM_ACCOUNT)
                    throw new SC.Interfaces.SCException("Invalid username.");

                UserInfo newUser = new UserInfo(username, password);
                Logger.Info("Adding user " + username);
                users.Add(newUser.Username, newUser);
            }
            SaveSettings();
        }
        public string[] GetUsers()
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return new List<string>(users.Keys).ToArray();
            }
        }
        public void SetPassword(string username, string password)
        {
            bool canSet = false;
            // if (!canSet)
            // {
                try
                {
                    new System.Security.Permissions.PrincipalPermission(username, USER_ROLE).Demand();
                    canSet = true;
                }
                catch (System.Security.SecurityException)
                {
                    // We're not the user
                }
            // }

            if (!canSet)
            {
                try
                {
                    DemandAdministratorPermissions();
                    canSet = true;
                }
                catch (System.Security.SecurityException)
                {
                    // We're not an administrator
                }
            }

            if (!canSet)
            {
                Logger.Error("User " + GetCurrentUser() + " tried settings password for user " + username + ". The operation was denied.");

                throw new System.Security.SecurityException("You cannot set other users' password without administrator privileges. Your action will be reported.");
            }

            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                if (!users.ContainsKey(username))
                    throw new SC.Interfaces.SCException("The given username doesn't exist.");
                Logger.Info("User " + username + " changed password.");
                users[username].Password = password;
            }
        }
        public void RemoveUser(string username)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                Logger.Info("User " + username + " was removed.");
                users.Remove(username);

                foreach (SecuritySubject subject in subjects.Values)
                {
                    subject.RemovePermission(username);
                }
            }
            SaveSettings();
        }
        #endregion

        #region Security Provider Interface
        public Guid RegisterSubject(string name)
        {
            return RegisterSubject(name, null);
        }
        public Guid RegisterSubject(string name, string[] additionalOperations)
        {
            return RegisterSubject(name, additionalOperations, null);
        }
        private Guid RegisterSubject(string name, string[] additionalOperations, string username)
        {
            Logger.Debug("Registered subject " + name);

            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                SecuritySubject subject;
                if (!subjects.ContainsKey(name))
                {
                    if (username == null)
                        subjects[name] = new SecuritySubject(name);
                    else
                        subjects[name] = new SecuritySubject(name, username);
                }

                subject = subjects[name];

                Guid guid = Guid.NewGuid();

                while (acls.ContainsKey(guid))
                    guid = Guid.NewGuid();

                acls.Add(guid, subject);

                if (additionalOperations != null)
                {
                    Logger.Debug("Additional operations " + string.Join(", ", additionalOperations));
                    System.Collections.Specialized.StringCollection operations = new System.Collections.Specialized.StringCollection();
                    operations.AddRange(additionalOperations);

                    foreach (string op in operations)
                    {
                        if (!subject.HaveOperation(op))
                            subject.AddOperation(op);
                    }
                    operations.Add(Operation.DEFAULT_OPERATION);
                    foreach (string op in subject.GetOperations())
                    {
                        if (!operations.Contains(op))
                            subject.RemoveOperation(op);
                    }
                }
                return guid;
            }
        }
        public void DemandPermissions(Guid guid)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                try
                {
                    acls[guid].DemandPermission();
                }
                catch (System.Security.SecurityException)
                {
                    Logger.Error("User " + GetCurrentUser() + " was denied access to subject " + acls[guid].Name);
                    throw;
                }
                catch (KeyNotFoundException e)
                {
                    Logger.Error("Subject was not registered.");
                    throw new System.Security.SecurityException("The subject is not registered. Permission was denied.", e);
                }
                catch (Exception e)
                {
                    Logger.Error("An unexpected error occurred while demanding permissions for subject " + acls[guid].Name + ".", e);
                    throw new System.Security.SecurityException("An unexpected error occurred.", e);
                }
            }
        }
        public void DemandPermissions(Guid guid, string operation)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                try
                {
                    acls[guid].DemandPermission(operation);
                }
                catch (System.Security.SecurityException)
                {
                    Logger.Error("User " + GetCurrentUser() + " was denied access to subject " + acls[guid].Name + " for operation " + operation);
                    throw;
                }
                catch (KeyNotFoundException e)
                {
                    Logger.Error("Subject was not registered.");
                    throw new System.Security.SecurityException("The subject is not registered. Permission was denied.", e);
                }
                catch (Exception e)
                {
                    Logger.Error("An unexpected error occurred while demanding permissions for subject " + acls[guid].Name + " and operation " + operation + ".", e);
                    throw new System.Security.SecurityException("An unexpected error occurred.", e);
                }
            }
        }
        public void UnregisterSubject(Guid guid)
        {
            UnregisterSubject(guid, false);
        }
        public void UnregisterSubject(Guid guid, bool remove)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                try
                {
                    SecuritySubject subject = acls[guid];
                    acls.Remove(guid);
                    if (remove)
                    {
                        l.UpgradeToWriterLock();
                        subjects.Remove(subject.Name);
                        l.DowngradeToReaderLock();
                    }
                }
                catch (KeyNotFoundException)
                {
                    Logger.Error("Tried unregistering subject that never registered.");
                    throw;
                }
                catch (Exception e)
                {
                    Logger.Error("An unexpected error occurred during unregistering of a subject.", e);
                    throw;
                }
            }
            SaveSettings();
        }
        public bool HavePermission(Guid guid)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return acls[guid].HavePermission();
            }
        }
        public bool HavePermission(Guid guid, string operation)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return acls[guid].HavePermission(operation);
            }
        }
#endregion
        internal string[] GetDefaultAccess()
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                if (subjects.ContainsKey("SecurityManager"))
                    return subjects["SecurityManager"].GetPermissions(); // current admin permissions
                else
                {
                    return new string[] { "Administrator" };
                }
            }
        }

        #region IP Management
        public bool IsClientIPAllowed(System.Net.IPAddress address)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                foreach (SC.Security.Network net in clients)
                {
                    if (net.IsHostInNet(address))
                        return true;
                }
                return false;
            }
        }
        public void AddAllowedClientNetwork(System.Net.IPAddress address, System.Net.IPAddress netmask)
        {
            Network network = new Network(address, netmask);
            DemandAdministratorPermissions();
            AddAllowedClientNetwork(network);
        }
        private void AddAllowedClientNetwork(Network network)
        {
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                foreach (SC.Security.Network net in clients)
                {
                    if (net.IsHostInNet(network.Address) || network.IsHostInNet(net.Address))
                    {
                        Logger.Error("Network add failed because network " + network.ToString() + " is contained in " + net.ToString());
                        throw new ArgumentException("Cannot add network because it contains or is contained in another network: " + net.ToString());
                    }
                }
                l.UpgradeToWriterLock();
                Logger.Info("Adding network " + network.ToString() + " to the access list.");
                clients.Add(network);
            }
        }
        public void RemoveAllowedClientNetwork(SC.Interfaces.INetwork network)
        {
            SC.Security.Network net = new Network(network.Address, network.Netmask);

            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                if (!clients.Remove(net))
                {
                    Logger.Error("Removal of network " + net.ToString() + " failed because it is not in the access list.");
                    throw new ArgumentException("Given network is not present in list");
                }
                Logger.Info("Network " + net.ToString() + " was removed from the access list.");
            }
        }
        public SC.Interfaces.INetwork[] GetAllowedClientNetworks()
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return clients.ToArray();
            }
        }

        private void FixNetworks()
        {
            if (clients.Count == 0 || !IsClientIPAllowed(System.Net.IPAddress.Loopback))
            {
                Logger.Warn("Did not find any networks in access list. Adding loopback interface.");
                clients.Add(new SC.Security.Network(System.Net.IPAddress.Loopback, 32));
            }
        }
        #endregion

        #region Security Subject Management
        public string[] GetSubjects()
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return new List<string>(subjects.Keys).ToArray();
            }
        }
        public string[] GetAdditionalOperations(string subject)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return subjects[subject].GetOperations();
            }
        }
        public string[] GetPermissions(string subject)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return subjects[subject].GetPermissions();
            }
        }
        public string[] GetPermissions(string subject, string operation)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForReading))
            {
                return subjects[subject].GetPermissions(operation);
            }
        }
        public void AddPermission(string subject, string username)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                Logger.Info("User " + GetCurrentUser() + " gave user " + username + " access to subject " + subject);
                subjects[subject].AddPermission(username);
            }
            SaveSettings();
        }
        public void AddPermission(string subject, string username, string operation)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                Logger.Info("User " + GetCurrentUser() + " gave user " + username + " access to subject " + subject + " for operation " + operation);
                subjects[subject].AddPermission(username, operation);
            }
            SaveSettings();
        }
        public void RemovePermission(string subject, string username)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                Logger.Info("User " + GetCurrentUser() + " removed permissions for user " + username + " to subject " + subject);
                subjects[subject].RemovePermission(username);
            }
            SaveSettings();
        }
        public void RemovePermission(string subject, string username, string operation)
        {
            DemandAdministratorPermissions();
            using (SC.Utility.Lock l = new SC.Utility.Lock(secLock, SC.Utility.Lock.LockType.ForWriting))
            {
                Logger.Info("User " + GetCurrentUser() + " removed permissions for user " + username + " to subject " + subject + " for operation " + operation);
                subjects[subject].RemovePermission(username, operation);
            }
            SaveSettings();
        }
        #endregion
    }
}
