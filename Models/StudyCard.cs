namespace StudyCardsGenerator.Models
{
    public class StudyCard
    {
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public bool IsAnswerVisible { get; set; } = true;
    }
}