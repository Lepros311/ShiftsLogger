using ShiftsLogger.ConsoleApp.Models;
using System.Text.Json;

namespace ShiftsLogger.ConsoleApp.Services;

public class WorkerService
{
    private readonly HttpClient _httpClient;

    public WorkerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WorkerDto>> GetWorkersAsync()
    {
        var response = await _httpClient.GetStringAsync("/api/Workers");
        return JsonSerializer.Deserialize<List<WorkerDto>>(response);
    }

    public async Task InsertWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("/api/Workers", content);
    }

    public async Task UpdateWorker(WorkerDto worker)
    {
        var json = JsonSerializer.Serialize(worker);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        await _httpClient.PutAsync($"/api/Workers/{worker.WorkerId}", content);
    }

    public async Task DeleteWorker(int workerId)
    {
        await _httpClient.DeleteAsync($"/api/Workers/{workerId}");
    }
}

