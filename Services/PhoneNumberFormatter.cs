// Services/PhoneNumberFormatter.cs
using SpellCheckerProject.Models;
using System;
using System.Text.RegularExpressions;

namespace SpellCheckerProject.Services
{
    public class PhoneNumberFormatter
    {
        private readonly Regex _phonePattern;
        private readonly string _replacementFormat;

        public PhoneNumberFormatter()
        {
            // Паттерн для поиска номеров вида (012) 345-67-89
            _phonePattern = new Regex(@"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})", RegexOptions.Compiled);
            // Новый формат: +380 12 345 67 89
            _replacementFormat = "+380 $1 $2 $3 $4";
        }

        public string FormatPhoneNumbers(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return _phonePattern.Replace(text, _replacementFormat);
        }

        public CorrectionResult FormatPhoneNumbersInFile(string filePath)
        {
            var result = new CorrectionResult(filePath);
            string content = System.IO.File.ReadAllText(filePath);

            // Находим все номера до замены
            var matches = _phonePattern.Matches(content);
            int phoneCount = matches.Count;

            if (phoneCount > 0)
            {
                string formattedContent = _phonePattern.Replace(content, _replacementFormat);

                foreach (Match match in matches)
                {
                    result.AddChange($"Телефон: '{match.Value}' → '{_phonePattern.Replace(match.Value, _replacementFormat)}'");
                }

                System.IO.File.WriteAllText(filePath, formattedContent);
                result.PhoneNumbersFixed = phoneCount;
            }

            return result;
        }

        public string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            var match = _phonePattern.Match(phone);
            return match.Success ? _phonePattern.Replace(phone, _replacementFormat) : phone;
        }
    }
}