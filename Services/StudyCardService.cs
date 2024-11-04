using System.Net.Http.Json;
using StudyCardsGenerator.Models;
using StudyCardsGenerator.Services.Interfaces;

namespace StudyCardsGenerator.Services
{
    public class StudyCardService : IStudyCardService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public StudyCardService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<List<StudyCard>> GenerateStudyCardsAsync(Stream pdfStream, string title, int targetCount = 10, string? model = null)
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No authentication token available");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(pdfStream), "file", "document.pdf");
            content.Add(new StringContent(title), "title");
            content.Add(new StringContent(targetCount.ToString()), "targetCount");
            if (!string.IsNullOrEmpty(model))
                content.Add(new StringContent(model), "model");

            var response = await _httpClient.PostAsync("api/flashcards/sets", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<StudyCardSet>();
            return result?.StudyCards ?? new List<StudyCard>();
        }

        public async Task<List<StudyCard>> GetStudyCardsAsync(int setId)
        {
            var set = await GetSetAsync(setId);
            return set?.StudyCards ?? new List<StudyCard>();
        }

        public async Task<List<StudyCardSet>> GetAllSetsAsync()
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No authentication token available");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<List<StudyCardSet>>("api/flashcards/sets");
            return response ?? new List<StudyCardSet>();
        }

        public async Task<StudyCardSet> GetSetAsync(int setId)
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No authentication token available");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.GetFromJsonAsync<StudyCardSet>($"api/flashcards/sets/{setId}")
                ?? throw new Exception("Failed to retrieve flashcard set");
        }

        public async Task<bool> DeleteSetAsync(int setId)
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No authentication token available");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"api/flashcards/sets/{setId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<StudyCardSet> UpdateSetTitleAsync(int setId, string newTitle)
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("No authentication token available");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"api/flashcards/sets/{setId}", newTitle);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<StudyCardSet>()
                ?? throw new Exception("Failed to update flashcard set");
        }
    }
}