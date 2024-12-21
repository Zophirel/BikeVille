using Microsoft.AspNetCore.Mvc;
using BikeVille.Services;
using static BikeVille.Services.AuthService;
using BikeVille.Controllers.Auth.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BikeVille.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService context, JwtSettings settings) : ControllerBase
    {
        private readonly AuthService _context = context;
        private readonly JwtSettings jwtSettings = settings;  


        // Post: api/auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(Credential credential)
        {   
            try{
                return await _context.LoginAsync(credential) switch
                {
                    LoginResult.Success => Ok(GenerateJwtToken(credential.Email)),
                    LoginResult.InvalidCredentials => BadRequest("Invalid credentials"),
                    LoginResult.MigratedCustomer => BadRequest("An email to reset your password has been sent"),
                    _ => BadRequest(),
                };
            } catch(Exception ex){
                return BadRequest(ex.Message);
            } 
        }

        private string GenerateJwtToken(string username, bool isAdmin = false)
        {
            var secretKey = jwtSettings.SecretKey ?? throw new Exception("Secret key is not set");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim (ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
                ]),
                Expires = DateTime.Now.AddMinutes(jwtSettings.TokenExpirationMinutes),
                Issuer=jwtSettings.Issuer,
                Audience=jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    

        // Post: api/auth/Login
        [HttpPost("jwt/Login")]
        public async Task<ActionResult> LoginJwt(Credential credential)
        {   
        
            try{
                return await _context.LoginAsync(credential) switch
                {
                    LoginResult.Success => Ok(GenerateJwtToken(credential.Email)),
                    LoginResult.InvalidCredentials => BadRequest("Invalid credentials"),
                    LoginResult.MigratedCustomer => BadRequest("An email to reset your password has been sent"),
                    _ => BadRequest(),
                };
            } catch(Exception ex){
                return BadRequest(ex.Message);
            } 
        }


        /*
            [CustomerID]
            [NameStyle]
            [Title]
            [FirstName]
            [MiddleName]
            [LastName]
            [Suffix]
            [CompanyName]
            [SalesPerson]
            [EmailAddress]
            [Phone]
            [PasswordHash]
            [PasswordSalt]
            [rowguid]
            [ModifiedDate]
            [MigratedCustomer]
        */

    
        // Post: api/auth/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(SignUpCredential credential)
        {
            try{
                return Ok(await _context.SignUpAsync(credential)); 
            } catch(Exception ex){
                return BadRequest(ex.Message);
            }   
        }
    }
}





