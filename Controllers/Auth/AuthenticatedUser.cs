using System.Security.Principal;

namespace BikeVille.Controllers.Auth
{
    public class AuthenticatedUser(string authType, bool isAuth, string name) : IIdentity
    {
        public string? AuthenticationType { get; set; } = authType;

        public bool IsAuthenticated { get; set; } = isAuth;

        public string? Name { get; set; } = name;
    }
}
