namespace APICatalogo.Services;

public interface ITokenRevocationService
{
    void RevokeToken(string token);
    bool IsTokenRevoked(string token);
}