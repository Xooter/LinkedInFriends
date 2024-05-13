using System.Text.Json;
using LinkedInFriends.Models;

namespace LinkedInFriends.Helpers;

public class SecretManager
{
    private readonly string configPath;

    public SecretManager(string configFileName)
    {
        string appDirectory = AppContext.BaseDirectory;

        configPath = Path.Combine(appDirectory, configFileName);
    }


    public Config? LoadConfig()
    {
        Config config = new Config();
        if (!File.Exists(configPath))
        {
            SaveConfig(config);
        }
        else
        {
            var jsonString = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<Config>(jsonString);
        }
        return config;
    }

    public void SaveConfig(Config config)
    {
        var jsonString = JsonSerializer.Serialize(config);
        File.WriteAllText(configPath, jsonString);
    }
}
