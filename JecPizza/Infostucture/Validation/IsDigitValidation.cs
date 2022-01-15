using System.Globalization;
using System.Windows.Controls;

namespace JecPizza.Infostucture.Validation
{
    public class IsDigitValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) => !int.TryParse(value?.ToString() ?? "0", out int res) ? new ValidationResult(false, "Not Correct Format") : ValidationResult.ValidResult;
    }
}