using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Dets.Services;

public class GoogleCalendarService
{
    private const string ClientId = "283366784494-1g04vurrnuct7ni8qskl2h32jpo7348e.apps.googleusercontent.com";
    private const string Scope = "https://www.googleapis.com/auth/calendar.readonly";

    private readonly IJSRuntime _js;
    private readonly HttpClient _http;

    public GoogleCalendarService(IJSRuntime js, HttpClient http)
    {
        _js = js;
        _http = http;
    }

    public async Task<List<Dets.Models.Event>> ImportAsync(DateTime from, DateTime to)
    {
        string token = await _js.InvokeAsync<string>("googleAuth.getToken", ClientId, Scope);

        string url = "https://www.googleapis.com/calendar/v3/calendars/primary/events"
            + $"?timeMin={Uri.EscapeDataString(from.ToUniversalTime().ToString("o"))}"
            + $"&timeMax={Uri.EscapeDataString(to.ToUniversalTime().ToString("o"))}"
            + "&singleEvents=true&orderBy=startTime&maxResults=250";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GoogleEventList>();

        var events = new List<Dets.Models.Event>();
        foreach (var item in result?.Items ?? new())
        {
            // Eventi "tutto il giorno" usano Date, gli altri DateTime
            DateTime start = item.Start?.DateTime
                ?? DateTime.Parse(item.Start?.Date ?? DateTime.Today.ToString("yyyy-MM-dd"));

            events.Add(new Dets.Models.Event
            {
                Name = item.Summary ?? "(senza titolo)",
                Description = item.Description ?? "",
                Day = start.Day,
                Month = start.Month,
                Year = start.Year
            });
        }
        return events;
    }
}

public class GoogleEventList
{
    [JsonPropertyName("items")] public List<GoogleEvent>? Items { get; set; }
}

public class GoogleEvent
{
    [JsonPropertyName("summary")] public string? Summary { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("start")] public GoogleEventTime? Start { get; set; }
}

public class GoogleEventTime
{
    [JsonPropertyName("dateTime")] public DateTime? DateTime { get; set; }
    [JsonPropertyName("date")] public string? Date { get; set; }
}