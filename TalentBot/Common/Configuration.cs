using Newtonsoft.Json;
using System;
using System.IO;

namespace TalentBot
{
    /// <summary> 
    /// A file that contains information you either don't want public
    /// or will want to change without having to compile another bot.
    /// </summary>
    public class Configuration
    {
        [JsonIgnore]
        /// <summary> The location and name of your bot's configuration file. </summary>
        public static string FileName { get; private set; } = "config/configuration.json";
        /// <summary> Ids of users who will have owner access to the bot. </summary>
        public ulong[] Owners { get; set; }
        /// <summary> Your bot's command prefix. </summary>
        public string Prefix { get; set; } = "!";
        /// <summary> Your bot's login token. </summary>
        public string Token { get; set; } = Hidden.token;

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))                                 // Check if the configuration file exists.
            {
                string path = Path.GetDirectoryName(file);          // Create config directory if doesn't exist.
                Console.WriteLine(path);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();                   // Create a new configuration object.

                config.Token = Hidden.token;
                config.SaveJson();                                  // Save the new configuration object to file.
            }
            Console.WriteLine("Configuration Loaded");
        }

        /// <summary> Save the configuration to the path specified in FileName. </summary>
        public void SaveJson()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllText(file, ToJson());
        }

        /// <summary> Load the configuration from the path specified in FileName. </summary>
        public static Configuration Load()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }

        /// <summary> Convert the configuration to a json string. </summary>
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}