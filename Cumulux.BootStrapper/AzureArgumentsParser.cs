namespace Cumulux.BootStrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class AzureArgumentsParser : IArgumentsParser
    {
        internal static readonly string[] prefixes = new string[] { "$lr", "$sc", "$port", "$ip", "$config" };
        internal static readonly Regex argsRegex = new Regex(@"(?<key>\$\S+?)(?:\()((?<value>\S+?))(?:\))", RegexOptions.Compiled);  //c|rdm
        
        public AzureArgumentsParser()
        {
        }

        public string ParseArguments(string arguments)
        {
            if (String.IsNullOrEmpty(arguments))
                return string.Empty;

            //Regex regex = new Regex(@"(?<key>\$\w+)(?<value>\(\w+\))");
            //x|Regex regex = new Regex(@"(?<key>\$\S+)(?<value>\(\S+\))"); //c|rdm

            var expanded = argsRegex.Replace(arguments, new MatchEvaluator(delegate(Match m)  //c|rdm
            {
                var prefix = m.Groups["key"].Value; // $lr
                var v = m.Groups["value"].Value; // temp  //c|rdm

                // Unknown prefix, return unchanged
                if (!prefixes.Contains(prefix))
                    return m.Value;

                string replaced = null;

                switch (prefix)
                {
                    case "$lr":
                        replaced = SettingManager.GetResourcePath(v);
                        break;
                    case "$config":
                        replaced = SettingManager.GetConfigurationSetting(v);
                        break;
                    case "$sc":
                        replaced = SettingManager.GetConfigurationSetting(v);
                        break;
                    case "$ip":
                        replaced = SettingManager.GetEndpointIpAddress(v);
                        break;
                    case "$port":
                        replaced = SettingManager.GetEndpointPort(v);
                        break;
                }
                return replaced;
            }));
            
            return Environment.ExpandEnvironmentVariables(expanded);
        }
    }
}
