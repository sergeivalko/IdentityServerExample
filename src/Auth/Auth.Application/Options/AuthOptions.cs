namespace Auth.Application.Options
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Authority { get; set; }
        public int Expires { get; set; }
        public int RefreshTokenExpires { get; set; }
    }
}