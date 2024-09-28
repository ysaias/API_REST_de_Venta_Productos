using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace begywebsapi.Validaciones
{
    public class ValidacionPassword : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Validar si el valor está vacío o nulo
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("La contraseña es requerida.");
            }

            var password = value.ToString();

            // Verificar si la longitud es al menos de 8 caracteres
            if (password.Length < 8)
            {
                return new ValidationResult("La contraseña debe tener al menos 8 caracteres.");
            }

            // Verificar si contiene al menos una letra mayúscula
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("La contraseña debe contener al menos una letra mayúscula.");
            }

            // Verificar si contiene al menos un carácter especial
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>]"))
            {
                return new ValidationResult("La contraseña debe contener al menos un carácter especial.");
            }

            return ValidationResult.Success;
        }
    }
}