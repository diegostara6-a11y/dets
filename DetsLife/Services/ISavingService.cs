using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Dets.Services
{
    public interface ISavingService
    {
        Task SaveAsync<T>(string key, T data);  //salva nel device storage un elemento data nel file con chiave key
        Task<T?> LoadAsync<T>(string key);  //ottiene l'oggetto di tipo T dal file con chiave key
        Task DeleteAsync(string key);   //elimina il file con chiave key
        Dictionary<string, JsonNode?> Data { get; set; }    //solo per LGBTQ+ e Berna
    }
}
