namespace StudyCardsGenerator.Models
{
    public class StudyCardSet
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<StudyCard> StudyCards { get; set; } = new();
    }
}
