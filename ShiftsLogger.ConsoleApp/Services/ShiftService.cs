using ShiftsLogger.ConsoleApp.Models;
using System.Text.Json;

namespace ShiftsLogger.ConsoleApp.Services;

public class ShiftService
{
    private readonly HttpClient _httpClient;

    public ShiftService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ShiftDto>> GetShiftsAsync()
    {
        var response = await _httpClient.GetStringAsync("/api/Shifts");
        return JsonSerializer.Deserialize<List<ShiftDto>>(response);
    }
}