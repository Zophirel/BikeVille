using System;
using System.ComponentModel.DataAnnotations;

namespace BikeVille.Controllers.Auth
{
    public class Credential(string email, string password)
    {
        [MaxLength(50)]
        [EmailAddress]
        [Required]
        public string Email { get; set; } = email ?? throw new ArgumentNullException(nameof(email));

        [PasswordStrength]
        public string Password { get; set; } = password ?? throw new ArgumentNullException(nameof(password));

        public static T Create<T>(Func<T> creator)
        {
            var instance = creator();
            Validate(instance);
            return instance;
        }

        private static void Validate<T>(T cred)
        {
            if(cred != null){
                var context = new ValidationContext(cred, null, null);
                Validator.ValidateObject(cred, context, validateAllProperties: true);
            }
        }
    }

    public class SignUpCredential(string? title, string firstName, string? middleName, string lastName, string? suffix,
                            string? companyName, string? salesPerson, string email, string password, string? phone) : Credential(email, password)
    {
        [MaxLength(8)]
        public string? Title { get; private set; } = title;

        [MaxLength(50)]
        [MinLength(1, ErrorMessage = "Frist Name cannot be empty.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "First name cannot be empty or whitespace.")]
        public string FirstName { get; private set; } = firstName ?? throw new ArgumentNullException(nameof(firstName));

        [MaxLength(50)]
        public string? MiddleName { get; private set; } = middleName;


        [MaxLength(50)]
        [MinLength(1, ErrorMessage = "Last Name cannot be empty.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Name cannot be empty or whitespace.")]
        public string LastName { get; private set; } = lastName ?? throw new ArgumentNullException(nameof(lastName));

        [MaxLength(10)]
        public string? Suffix { get; private set; } = suffix;

        [MaxLength(128)]
        public string? CompanyName { get; private set; } = companyName;

        [MaxLength(256)]
        public string? SalesPerson { get; private set; } = salesPerson;

        [MaxLength(25)]
        
        public string? Phone { get; private set; } = phone;
    }
}
