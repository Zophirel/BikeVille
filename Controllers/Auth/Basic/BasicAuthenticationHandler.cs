using BikeVille.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace BikeVille.Controllers.Auth.Basic
{
    public class BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder url, ISystemClock clock, AuthService authService) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, url, clock)
    {
        private readonly AuthService _authService = authService;
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine("BASIC AUTH REQUEST HEADER");
            Console.WriteLine(Request.Headers.Authorization);
            Response.Headers.Append("WWW-Authenticate", "Basic");
            
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Autorizzazione mancante"));
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegEx = new Regex("Basic (.*)");

            if (!authHeaderRegEx.IsMatch(authHeader))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Autorizzazione non valida"));
            }

            string auth64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader[6..]));

            var authArr = auth64.Split(':');
            var authUser = authArr[0];
            var authPwd = authArr.Length > 1 ?
                authArr[1] : throw new Exception("Password ASSENTE !!!");

            if (string.IsNullOrEmpty(authUser.Trim()) || string.IsNullOrEmpty(authPwd.Trim()))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Username e/o Password, NON presenti"));
            }
            else
            {
                try
                {
                    await _authService.LoginAsync(Credential.Create(() => new Credential(authUser, authPwd)));
                    Console.WriteLine("Login effettuato con successo");   
                }
                catch (System.Exception)
                {
                    return await Task.FromResult(AuthenticateResult.Fail("Autorizzazione non valida"));
                    throw;
                }
            }

            var authenticatedUse = new AuthenticatedUser("BasicAuthentication", true, authUser);
            var claimMain = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUse));
            return await Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(claimMain, Scheme.Name)));
        }
    }
}
