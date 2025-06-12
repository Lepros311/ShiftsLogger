using ShiftsLogger.ConsoleApp.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShiftsLogger.ConsoleApp.Services;

public class ShiftService
{

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ShiftService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7150/api/");

    }

    public async Task<List<ShiftDto>> GetShiftsAsync()
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7150");
        var response = await httpClient.GetAsync("/api/Shifts");
        var shifts = await response.Content.ReadFromJsonAsync<List<ShiftDto>>();
        return shifts;
    }

    public async Task<ShiftDto> GetShiftAsync(int shiftId)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7150");
        var response = await httpClient.GetAsync($"/api/Shifts/{shiftId}");
        var shift = await response.Content.ReadFromJsonAsync<ShiftDto>();
        return shift;
    }

    public async Task<bool> InsertShiftAsync(ShiftDto newShift)
    {
        var json = JsonSerializer.Serialize(newShift);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsJsonAsync("/api/Shifts", newShift);
        return response.IsSuccessStatusCode;
    }
}