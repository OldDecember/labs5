using System;
using System.Collections.Generic;

namespace SpellCheckerProject.Models
{
    public class CorrectionResult
    {
        public string FilePath { get; set; }
        public int SpellingErrorsFixed { get; set; }
        public int PhoneNumbersFixed { get; set; }
        public List<string> Changes { get; set; }
        public DateTime ProcessedAt { get; set; }

        public CorrectionResult(string filePath)
        {
            FilePath = filePath;
            SpellingErrorsFixed = 0;
            PhoneNumbersFixed = 0;
            Changes = new List<string>();
            ProcessedAt = DateTime.Now;
        }

        public void AddChange(string description)
        {
            Changes.Add(description);
        }

        public override string ToString()
        {
            return $"Файл: {FilePath}\n" +
                   $"Исправлено орфографических ошибок: {SpellingErrorsFixed}\n" +
                   $"Исправлено номеров телефонов: {PhoneNumbersFixed}\n" +
                   $"Изменений: {Changes.Count}";
        }
    }
}