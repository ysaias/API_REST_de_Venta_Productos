using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace begywebsapi.Validaciones
{
    public class ValidacionEmail : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var email = value.ToString();
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (regex.IsMatch(email))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("El email no es válido.");
        }
    }
}