using System.Globalization;
using System.Windows.Controls;

namespace Utilidades.Rules;

public class PasswordMatchValidationRule : ValidationRule
{
    public PasswordBox PasswordOriginal { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var confirmPassword = value as string ?? "";

        if (PasswordOriginal == null)
            return new ValidationResult(false, "Referencia de contraseña original no asignada.");

        if (confirmPassword != PasswordOriginal.Password)
            return new ValidationResult(false, "Las contraseñas no coinciden.");

        return ValidationResult.ValidResult;
    }
}