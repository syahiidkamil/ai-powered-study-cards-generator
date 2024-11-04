namespace StudyCardsGenerator.Services.Interfaces;

public interface IAuthService
{
    Task<bool> IsUserAuthenticated();
    Task<string?> GetToken();
    Task SetToken(string token);
    Task RemoveToken();
}