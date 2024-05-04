namespace trsaints_frontend_api.Entities;

public class UserToken
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
}
