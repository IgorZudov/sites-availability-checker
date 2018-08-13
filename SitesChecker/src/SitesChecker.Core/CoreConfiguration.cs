using System.IO;
using Newtonsoft.Json;

namespace SitesChecker.Core
{
    public  class CoreConfiguration
    {
        [JsonProperty("Время паузы между запросами на доступность сайтов")]
        public int UpdateSitesDelay { get; set; } = 5;
        
        private const string ConfigName = "config.conf";
        private static readonly object SyncObj = new object();
        private static CoreConfiguration instance;

        public static CoreConfiguration Default
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                lock (SyncObj)
                {
                    if (File.Exists(ConfigName))
                    {
                        instance = JsonConvert.DeserializeObject<CoreConfiguration>(File.ReadAllText(ConfigName));
                        return instance;
                    }
                    instance = new CoreConfiguration();
                    instance.Save();
                    return instance;
                }
            }
        }

        public void Save()
        {
            File.WriteAllText(ConfigName, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}