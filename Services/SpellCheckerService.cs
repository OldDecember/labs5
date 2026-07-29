// Services/SpellCheckerService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SpellCheckerProject.Models;
using SpellCheckerProject.Data;

namespace SpellCheckerProject.Services
{
    public class SpellCheckerService
    {
        private Dictionary<string, string> _spellingDictionary;

        public SpellCheckerService()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _spellingDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var misspelledWords = DictionaryData.GetMisspelledWords();

            foreach (var item in misspelledWords)
            {
                foreach (var misspelling in item.Misspellings)
                {
                    _spellingDictionary[misspelling.ToLower()] = item.CorrectWord;
                }
            }
        }

        public string CorrectText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string[] words = text.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?', ';', ':' },
                                        StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                string cleanWord = word.Trim(new[] { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')' });

                if (_spellingDictionary.TryGetValue(cleanWord.ToLower(), out string correctWord))
                {
                    // Заменяем слово в тексте с сохранением регистра
                    string pattern = $@"\b{Regex.Escape(cleanWord)}\b";
                    string replacement = correctWord;

                    // Если слово было с заглавной буквы
                    if (char.IsUpper(cleanWord[0]))
                    {
                        replacement = char.ToUpper(correctWord[0]) + correctWord.Substring(1);
                    }

                    text = Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
                }
            }

            return text;
        }

        public CorrectionResult CorrectFile(string filePath)
        {
            var result = new CorrectionResult(filePath);
            string content = System.IO.File.ReadAllText(filePath);
            string originalContent = content;

            // Исправляем орфографические ошибки
            string correctedContent = CorrectText(content);

            int spellingErrors = 0;
            // Подсчитываем количество исправлений
            var words = content.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?', ';', ':' },
                                      StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                string cleanWord = word.Trim(new[] { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')' });
                if (_spellingDictionary.ContainsKey(cleanWord.ToLower()))
                {
                    spellingErrors++;
                    result.AddChange($"Исправлено слово: '{cleanWord}' → '{_spellingDictionary[cleanWord.ToLower()]}'");
                }
            }

            result.SpellingErrorsFixed = spellingErrors;

            // Сохраняем изменения
            System.IO.File.WriteAllText(filePath, correctedContent);

            return result;
        }
    }
}