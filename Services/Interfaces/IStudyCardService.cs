using StudyCardsGenerator.Models;

namespace StudyCardsGenerator.Services
{
    public interface IStudyCardService
    {
        Task<List<StudyCard>> GenerateStudyCardsAsync(byte[] pdfContent, string model);
        List<StudyCard> GetSampleStudyCards();
    }
}