using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Dets.Services;

public class SaveOnDevice : ISavingService
{
    private readonly string basePath;

    public Dictionary<string, JsonNode?> Data
    {
        get
        {
            Dictionary<string, JsonNode?> data = new Dictionary<string, JsonNode?>();
            List<string> files = Directory.GetFiles(basePath, "*.json").ToList();
            foreach (string file in files)
            {
                string json = File.ReadAllText(file) ?? "";
                data[Path.GetFileNameWithoutExtension(file)] = JsonSerializer.Deserialize<JsonNode>(json);
            }
            return data;
        }
        set
        {
            foreach (string file in Directory.GetFiles(basePath, "*.json"))
            {
                File.Delete(file);
            }

            foreach (var (name, node) in value)
            {
                string file = Path.Combine(basePath, $"{name}.json");
                File.WriteAllText(file, JsonSerializer.Serialize(node));
            }
        }
    }

    //private void SaveData()
    //{
    //    foreach(var item in Data)
    //    {
    //        SaveAsync(item.Key, item.Value).Wait();
    //    }
    //}

    public SaveOnDevice()
    {
        basePath = FileSystem.AppDataDirectory;
        //App.OnAppStopped += (sender, e) => SaveData();
    }

    public async Task SaveAsync<T>(string key, T data)
    {
        string filePath = GetFilePath(key);
        string json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<T?> LoadAsync<T>(string key)
    {
        string filePath = GetFilePath(key);

        if (!File.Exists(filePath))
            return default;

        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }

    public Task DeleteAsync(string key)
    {
        string filePath = GetFilePath(key);

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }

    private string GetFilePath(string key)
    {
        return Path.Combine(basePath, $"{key}.json");
    }
}
