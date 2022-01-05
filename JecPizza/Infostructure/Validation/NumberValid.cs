using System.Globalization;
using System.Windows.Controls;

namespace JecPizza.Infostructure.Validation
{
    public class NumberValid : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string errMess = string.Empty;
            string corMess = string.Empty;
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "en-US":
                    errMess = "Enter only numbers without hyphens!";
                    corMess = "Please, input the 11 digit number!";
                    break;
                case "ja-JP":
                    errMess = "ハイフンなしで数字だけ入力してください！";
                    corMess = "１１桁の数字を入力してください！";
                    break;
                case "ru-RU":
                    errMess = "Введите только цифры без дефизов!";
                    corMess = "Пожалуйста, введите 11-значный номер телефона!";
                    break;
                default:
                    break;
            }

            if (long.TryParse((value ?? "").ToString(), out long res))
            {
                if (value.ToString().Length == 11)
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, corMess);
            }
            else
                return new ValidationResult(false, errMess);
        }
    }
}
