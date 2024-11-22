using Microsoft.AspNetCore.Mvc;
using BikeVille.Services;
using static BikeVille.Services.AuthService;

namespace BikeVille.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService context) : ControllerBase
    {
        private readonly AuthService _context = context;

        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("Test successful!");
        }

        // Post: api/auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(Credential credential)
        {   
            try{
                return await _context.LoginAsync(credential) switch
                {
                    LoginResult.Success => Ok("Login successful!"),
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





