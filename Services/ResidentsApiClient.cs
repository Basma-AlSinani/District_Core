using CrimeManagment.DTOs;

public class ResidentsApiClient
{
    private readonly HttpClient _httpClient;

    public ResidentsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7165/"); 
    }

    public async Task<IEnumerable<ResidentDto>> GetAllResidentsAsync()
    {
        var response = await _httpClient.GetAsync("api/Resident/list");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ResidentDto>>();
    }
}
