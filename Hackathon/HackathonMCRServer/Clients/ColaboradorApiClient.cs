using System.Net.Http.Json;
using System.Text.Json;
using HackathonMCRServer.DTOs;

namespace HackathonMCRServer.Clients
{
    public class ColaboradorApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ColaboradorApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<ColaboradorResponse>> ObterColaboradoresAsync(string? nome = null)
        {
            var url = string.IsNullOrWhiteSpace(nome) ? "colaboradores" : $"colaboradores?nome={Uri.EscapeDataString(nome)}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return new List<ColaboradorResponse>();

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ColaboradorResponse>>(_jsonOptions);
        }

        public async Task<ColaboradorResponse?> ObterColaboradorPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"colaboradores/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ColaboradorResponse>(_jsonOptions);
        }

        public async Task<int?> CriarColaboradorAsync(ColaboradorRequest colaborador)
        {
            var response = await _httpClient.PostAsJsonAsync("colaboradores", colaborador);

            if (!response.IsSuccessStatusCode)
                return null;

            var id = await response.Content.ReadFromJsonAsync<int>(_jsonOptions);
            return id;
        }

        public async Task<bool> AtualizarColaboradorAsync(int id, ColaboradorRequest colaborador)
        {
            var response = await _httpClient.PutAsJsonAsync($"colaboradores/{id}", colaborador);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExcluirColaboradorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"colaboradores/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
    }

}
