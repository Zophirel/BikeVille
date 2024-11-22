using BikeVille.Models.Customers;
using BikeVille.SqlDbContext;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using BikeVille.Controllers.Auth;


namespace BikeVille.Services
{
    public class AuthService(BikeVilleCustomersContext context)
    {
        private readonly BikeVilleCustomersContext _context = context;

        public enum LoginResult
        {
            Success,
            InvalidCredentials,
            MigratedCustomer
        }

        // Handle Login logic
        public async Task<LoginResult> LoginAsync(Credential credential)
        {
            try
            {
                Customer? customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.EmailAddress == credential.Email)
                    ?? throw new Exception("Customer not found");

                // Compare the provided password with stored hash
                string recreatedHash = BC.HashPassword(credential.Password, customer.PasswordSalt);
                bool isVerified = customer.PasswordHash == recreatedHash;

                if (isVerified)
                {
                    if (customer.MigratedCustomer)
                    {
                        customer.MigratedCustomer = false;
                        customer.ModifiedDate = DateTime.Now;
                        _context.Customers.Update(customer);
                    }
                    await _context.SaveChangesAsync();
                    return LoginResult.Success;
                }
                else if (customer.MigratedCustomer)
                {
                    // Send email for password reset if customer migrated
                    return LoginResult.MigratedCustomer;
                }
                else
                {
                    return LoginResult.InvalidCredentials;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        // Handle SignUp logic
        public async Task<string> SignUpAsync(SignUpCredential credential)
        {
            try
            {
                // Validate the credentials
                Credential.Create(() => credential);

                // Generate salt and hash the password
                string salt = BC.GenerateSalt();
                string hash = BC.HashPassword(credential.Password, salt);

                // Create the new customer
                Customer newCustomer = new Customer
                {
                    Title = credential.Title,
                    EmailAddress = credential.Email,
                    CompanyName = credential.CompanyName,
                    SalesPerson = credential.SalesPerson,
                    FirstName = credential.FirstName,
                    MiddleName = credential.MiddleName,
                    LastName = credential.LastName,
                    Suffix = credential.Suffix,
                    Phone = credential.Phone,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };

                // Add the new customer to the database
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
                
                return "Account created successfully!";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
