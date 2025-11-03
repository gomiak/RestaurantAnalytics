using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace RestaurantAnalytics.Web.Auth;

public class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _session;
    private readonly IConfiguration _config;
    private const string KEY = "ra_user";

    public SimpleAuthStateProvider(ProtectedSessionStorage session, IConfiguration config)
    {
        _session = session;
        _config = config;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? username = null;

        try
        {
            var stored = await _session.GetAsync<string>(KEY);
            username = stored.Success ? stored.Value : null;
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        if (string.IsNullOrWhiteSpace(username))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.Name, username)
    }, "SimpleAuth");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }


    public async Task<bool> LoginAsync(string user, string pass)
    {
        if (user == _config["AUTH_USER"] && pass == _config["AUTH_PASS"])
        {
            await _session.SetAsync(KEY, user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _session.DeleteAsync(KEY);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
