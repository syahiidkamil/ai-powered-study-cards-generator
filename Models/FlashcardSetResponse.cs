public class FlashcardSetResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<FlashcardResponse> Flashcards { get; set; } = new();
}