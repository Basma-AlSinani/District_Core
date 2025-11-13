using System.Net.Http;
using CrimeManagment.DTOs;
namespace ResidentsDatabase.Services
{
    public class ResidentsApiClient
    {
        private readonly HttpClient _httpClient;

        public ResidentsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ResidentDto>> GetAllResidentsAsync()
        {
            var response = await _httpClient.GetAsync("api/Resident/list");
            response.EnsureSuccessStatusCode();
            var residents = await response.Content.ReadFromJsonAsync<IEnumerable<ResidentDto>>();
            return residents ?? Enumerable.Empty<ResidentDto>();
        }
    }
}
