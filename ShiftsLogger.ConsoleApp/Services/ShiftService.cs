using ShiftsLogger.ConsoleApp.Models;
using System.Net.Http.Json;

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

    public async Task<ShiftDto> CreateShiftAsync(ShiftDto newShift)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7150");
        var response = await httpClient.PostAsJsonAsync("/api/Shifts", newShift);
        return newShift;
    }
}