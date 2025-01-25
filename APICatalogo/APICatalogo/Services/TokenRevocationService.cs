using Microsoft.Extensions.Caching.Distributed;

namespace APICatalogo.Services;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly IDistributedCache _cache;

    public TokenRevocationService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public void RevokeToken(string token)
    {
        var expirationTime = DateTime.UtcNow.AddHours(1);
        _cache.SetString(token, "revoked", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expirationTime
        });
    }

    public bool IsTokenRevoked(string token)
    {
        return _cache.GetString(token) != null;
    }
}
