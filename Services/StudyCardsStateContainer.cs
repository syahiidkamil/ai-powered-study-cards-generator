using StudyCardsGenerator.Models;
using StudyCardsGenerator.Services.Interfaces;
using System.Text.Json;

namespace StudyCardsGenerator.Services
{
    public class StudyCardsStateContainer
    {
        private List<StudyCard>? _currentCards;
        private string? _currentSetTitle;
        private readonly ILocalStorageService _localStorage;
        private const string CARDS_STORAGE_KEY = "current_study_cards";
        private const string TITLE_STORAGE_KEY = "current_study_title";
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public StudyCardsStateContainer(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<StudyCard>> GetCurrentCardsAsync()
        {
            await EnsureInitializedAsync();
            return _currentCards ?? new List<StudyCard>();
        }

        public async Task SetCurrentCardsAsync(List<StudyCard> value)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentCards = value;
                await PersistCardsAsync();
                NotifyStateChanged();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> GetCurrentSetTitleAsync()
        {
            await EnsureInitializedAsync();
            return _currentSetTitle ?? "My Study Set";
        }

        public async Task SetCurrentSetTitleAsync(string value)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentSetTitle = value;
                await PersistTitleAsync();
                NotifyStateChanged();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task EnsureInitializedAsync()
        {
            if (_isInitialized) return;

            await _semaphore.WaitAsync();
            try
            {
                if (!_isInitialized)
                {
                    _currentCards = await _localStorage.GetItemAsync<List<StudyCard>>(CARDS_STORAGE_KEY);
                    _currentSetTitle = await _localStorage.GetItemAsync<string>(TITLE_STORAGE_KEY);
                    _isInitialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task PersistCardsAsync()
        {
            if (_currentCards != null)
            {
                await _localStorage.SetItemAsync(CARDS_STORAGE_KEY, _currentCards);
            }
            else
            {
                await _localStorage.RemoveItemAsync(CARDS_STORAGE_KEY);
            }
        }

        private async Task PersistTitleAsync()
        {
            if (_currentSetTitle != null)
            {
                await _localStorage.SetItemAsync(TITLE_STORAGE_KEY, _currentSetTitle);
            }
            else
            {
                await _localStorage.RemoveItemAsync(TITLE_STORAGE_KEY);
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task ClearCurrentSetAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentCards = null;
                _currentSetTitle = null;
                await _localStorage.RemoveItemAsync(CARDS_STORAGE_KEY);
                await _localStorage.RemoveItemAsync(TITLE_STORAGE_KEY);
                NotifyStateChanged();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}