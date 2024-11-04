using StudyCardsGenerator.Models;

namespace StudyCardsGenerator.Services.Interfaces
{
    public interface IStudyCardService
    {
        Task<List<StudyCard>> GenerateStudyCardsAsync(Stream pdfStream, string title, int targetCount = 10, string? model = null);
        Task<List<StudyCard>> GetStudyCardsAsync(int setId);
        Task<List<StudyCardSet>> GetAllSetsAsync();
        Task<StudyCardSet> GetSetAsync(int setId);
        Task<bool> DeleteSetAsync(int setId);
        Task<StudyCardSet> UpdateSetTitleAsync(int setId, string newTitle);
    }
}