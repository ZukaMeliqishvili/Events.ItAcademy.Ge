namespace Events.It.Academy.Ge.Api.Infrastructure.Auth.JWT
{
    public class JWTConfiguration
    {
        public string Secret { get; set; }

        public int ExpirationInMinutes { get; set; }
    }
}
