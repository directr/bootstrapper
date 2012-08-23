namespace Cumulux.BootStrapper
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.IO;

    public class SettingManager
    {
        public static string GetConfigurationSetting(string setting)
        {
            if (string.IsNullOrEmpty(setting))
                throw new ArgumentNullException("setting");

            if (RoleEnvironment.IsAvailable)
            {
                return RoleEnvironment.GetConfigurationSettingValue(setting);
            }

            return ConfigurationManager.AppSettings[setting];
        }

        public static string GetResourcePath(string resource)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException("resource");

            if (RoleEnvironment.IsAvailable)
            {
                return RoleEnvironment.GetLocalResource(resource).RootPath;
            }
            var info = new DirectoryInfo(ConfigurationManager.AppSettings[resource]);

            return info.Exists ? info.FullName : ConfigurationManager.AppSettings[resource];
        }

        public static string GetEndpointIpAddress(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("endpoint");

            if (RoleEnvironment.IsAvailable)
            {
                RoleInstanceEndpoint rie = null;
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints.TryGetValue(endpoint, out rie);

                if (rie != null)
                {
                    return rie.IPEndpoint.Address.ToString();
                }
            }

            return ConfigurationManager.AppSettings[endpoint];
        }

        public static string GetEndpointPort(string port)
        {
            if (string.IsNullOrEmpty(port))
                throw new ArgumentNullException("port");

            if (RoleEnvironment.IsAvailable)
            {
                RoleInstanceEndpoint rie = null;
                RoleEnvironment.CurrentRoleInstance.InstanceEndpoints.TryGetValue(port, out rie);

                if (rie != null)
                {
                    return rie.IPEndpoint.Port.ToString();
                }
            }

            return ConfigurationManager.AppSettings[port];
        }        
    }
}
