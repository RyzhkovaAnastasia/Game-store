using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OnlineGameStore.BLL.CustomValidator
{
    public class CardExpireValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string expireDate = (string)value;

            if (Regex.Match(expireDate, "[0-9]{2}/[0-9]{4}").Success)
            {
                int month = int.Parse(expireDate.Substring(0, 2));
                int year = int.Parse(expireDate.Substring(3, 4));

                if (year > DateTime.UtcNow.Year || (year == DateTime.UtcNow.Year && month >= DateTime.UtcNow.Month))
                {
                    return true;
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString);
        }
    }
}
