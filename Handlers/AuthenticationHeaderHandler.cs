using System.Net.Http.Headers;
using StudyCardsGenerator.Services.Interfaces;

namespace StudyCardsGenerator.Handlers;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    public AuthenticationHeaderHandler(IAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _authService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}