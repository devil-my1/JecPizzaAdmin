using System.Globalization;
using System.Windows.Controls;

namespace JecPizza.Infostucture.Validation
{
    public class NotEmptyValidationcs : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string messCont;

            switch (CultureInfo.CurrentCulture.Name)
            {
                case "en-US":
                    messCont = "Fill the blank!";
                    break;
                case "ru-RU":
                    messCont = "Заполните поле!";
                    break;
                case "ja-JP":
                    messCont = "入力してください！";
                    break;
                default:
                    messCont = "Fill the blank!";
                    break;
            }

            return string.IsNullOrEmpty((value ?? "").ToString()) ? new ValidationResult(false, messCont) : ValidationResult.ValidResult;
        }
    }
}
