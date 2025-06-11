using ShiftsLogger.ConsoleApp.Models;
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

    public async Task<List<WorkerDto>> GetWorkersWithShiftsAsync()
    {
        var response = await _httpClient.GetStringAsync("Workers");
        return JsonSerializer.Deserialize<List<WorkerDto>>(response, jsonOptions);
    }

    public async Task<List<WorkerDto>> GetWorkersAsync()
    {
        var response = await _httpClient.GetStringAsync("Workers/workers");
        return JsonSerializer.Deserialize<List<WorkerDto>>(response, jsonOptions);
    }

    public async Task<WorkerDto> GetWorkerAsync(int workerId)
    {
        var response = await _httpClient.GetStringAsync("Workers/{workerId}");
        return JsonSerializer.Deserialize<WorkerDto>(response, jsonOptions);
    }

    public async Task<bool> InsertWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Workers", content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"Workers/{worker.WorkerId}", content);

        return response.IsSuccessStatusCode;
    }

    public async Task DeleteWorker(int workerId)
    {
        await _httpClient.DeleteAsync($"Workers/{workerId}");
    }
}

