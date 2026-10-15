using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.JSInterop;

namespace Dets.Services;

public class SaveOnDevice : ISavingService
{
    private const string Prefix = "dets:";
    private readonly IJSInProcessRuntime js;

    public SaveOnDevice(IJSRuntime jsRuntime)
    {
        js = (IJSInProcessRuntime)jsRuntime;
    }

    private List<string> AllKeys()
    {
        int count = js.Invoke<int>("eval", "localStorage.length");
        var keys = new List<string>();
        for (int i = 0; i < count; i++)
        {
            string? k = js.Invoke<string?>("localStorage.key", i);
            if (k != null && k.StartsWith(Prefix))
                keys.Add(k.Substring(Prefix.Length));
        }
        return keys;
    }

    public Dictionary<string, JsonNode?> Data
    {
        get
        {
            var data = new Dictionary<string, JsonNode?>();
            foreach (string key in AllKeys())
            {
                string json = js.Invoke<string?>("localStorage.getItem", Prefix + key) ?? "";
                data[key] = JsonSerializer.Deserialize<JsonNode>(json);
            }
            return data;
        }
        set
        {
            foreach (string key in AllKeys())
                js.InvokeVoid("localStorage.removeItem", Prefix + key);

            foreach (var (name, node) in value)
                js.InvokeVoid("localStorage.setItem", Prefix + name, JsonSerializer.Serialize(node));
        }
    }

    public Task SaveAsync<T>(string key, T data)
    {
        js.InvokeVoid("localStorage.setItem", Prefix + key, JsonSerializer.Serialize(data));
        return Task.CompletedTask;
    }

    public Task<T?> LoadAsync<T>(string key)
    {
        string? json = js.Invoke<string?>("localStorage.getItem", Prefix + key);
        if (json == null)
            return Task.FromResult<T?>(default);
        return Task.FromResult(JsonSerializer.Deserialize<T>(json));
    }

    public Task DeleteAsync(string key)
    {
        js.InvokeVoid("localStorage.removeItem", Prefix + key);
        return Task.CompletedTask;
    }
}