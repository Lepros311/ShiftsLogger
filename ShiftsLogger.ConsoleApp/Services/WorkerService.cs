using ShiftsLogger.ConsoleApp.Models;
using System.Text;
using System.Text.Json;

namespace ShiftsLogger.ConsoleApp.Services;

public class WorkerService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public WorkerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7150/api/");

    }

    public async Task<List<WorkerDto>> GetWorkersAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("Workers/workers");
            return JsonSerializer.Deserialize<List<WorkerDto>>(response, jsonOptions) ?? new List<WorkerDto>();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error retrieving workers: {e.Message}");
            return new List<WorkerDto>(); // Return an empty list to prevent crashes
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error parsing worker data: {e.Message}");
            return new List<WorkerDto>(); // Handle JSON deserialization errors
        }
    }


    public async Task<WorkerDto> GetWorkerAsync(int workerId)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"Workers/{workerId}");
            return JsonSerializer.Deserialize<WorkerDto>(response, jsonOptions);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error retrieving worker: {e.Message}");
            return null;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error parsing worker data: {e.Message}");
            return null;
        }
    }

    public async Task<bool> InsertWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("Workers", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error inserting worker: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PutAsync($"Workers/{worker.WorkerId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error updating worker: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteWorker(int workerId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Workers/{workerId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorMessage}");
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting worker: {ex.Message}");
            return false;
        }
    }

}

