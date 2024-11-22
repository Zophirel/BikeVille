using System.ComponentModel.DataAnnotations;
namespace BikeVille.Controllers.Auth
{
    public class PasswordValidator
    {
        public static List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }
            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one number.");
            }
            if (!password.Any(c => "!@#$%^&*".Contains(c)))
            {
                errors.Add("Password must contain at least one special character.");
            }
            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }

            return errors;
        }
    }  

    public class PasswordStrengthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string password){
                var errors = PasswordValidator.ValidatePassword(password);
                if (errors.Count > 0)
                {
                    return new ValidationResult(string.Join(" ", errors));
                }
                return ValidationResult.Success;
            } else {
                return new ValidationResult("Password must be a string.");
            }
        }
    }  
}
