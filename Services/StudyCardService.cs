using StudyCardsGenerator.Models;

namespace StudyCardsGenerator.Services
{
    public class StudyCardService : IStudyCardService
    {
        private readonly HttpClient _httpClient;

        public StudyCardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StudyCard>> GenerateStudyCardsAsync(byte[] pdfContent, string model)
        {
            await Task.Delay(2000);
            return GetSampleStudyCards();
        }

        public List<StudyCard> GetSampleStudyCards()
        {
            return new List<StudyCard>
            {
                new() { Question = "What is the capital of France?", Answer = "Paris" },
                new() { Question = "Who wrote 'Romeo and Juliet'?", Answer = "William Shakespeare" },
                new() { Question = "What is the chemical symbol for gold?", Answer = "Au" },
                new() { Question = "What is the largest planet in our solar system?", Answer = "Jupiter" },
                new() { Question = "Who painted the Mona Lisa?", Answer = "Leonardo da Vinci" },
                new() { Question = "What is the square root of 144?", Answer = "12" },
                new() { Question = "What is the main ingredient in guacamole?", Answer = "Avocado" },
                new() { Question = "Who was the first president of the United States?", Answer = "George Washington" },
                new() { Question = "What is the capital of Japan?", Answer = "Tokyo" },
                new() { Question = "What is the chemical formula for water?", Answer = "H2O" },
                new() { Question = "Who wrote '1984'?", Answer = "George Orwell" },
                new() { Question = "What is the largest ocean on Earth?", Answer = "Pacific Ocean" }
            };
        }
    }
}
