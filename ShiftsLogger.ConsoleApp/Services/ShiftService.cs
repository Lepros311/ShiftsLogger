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
        try
        {
            var response = await _httpClient.GetAsync("/api/Shifts");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<List<ShiftDto>>() ?? new List<ShiftDto>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error retrieving shifts: {e.Message}");
            return new List<ShiftDto>(); // Return empty list to prevent crashes
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error parsing shift data: {e.Message}");
            return new List<ShiftDto>(); // Handle JSON deserialization errors
        }
    }


    public async Task<ShiftDto?> GetShiftAsync(int shiftId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/Shifts/{shiftId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<ShiftDto>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error retrieving shift: {e.Message}");
            return null; // Return null instead of an empty object
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error parsing shift data: {e.Message}");
            return null; // Handle JSON deserialization errors
        }
    }


    public async Task<bool> InsertShiftAsync(ShiftDto newShift)
    {
        var json = JsonSerializer.Serialize(newShift);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Shifts", newShift);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error inserting shift: {e.Message}");
            return false;
        }
    }


    public async Task<bool> UpdateShift(ShiftDto shift)
    {
        var json = JsonSerializer.Serialize(shift);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PutAsync($"Shifts/{shift.ShiftId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error updating shift: {e.Message}");
            return false;
        }
    }


    public async Task<bool> DeleteShift(int shiftId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Shifts/{shiftId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error deleting shift: {e.Message}");
            return false;
        }
    }

}