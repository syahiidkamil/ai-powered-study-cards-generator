namespace StudyCardsGenerator.Models.Auth;

public class AuthResponse
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
}