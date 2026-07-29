// Services/FileProcessorService.cs
using System;
using System.IO;
using System.Collections.Generic;
using SpellCheckerProject.Models;

namespace SpellCheckerProject.Services
{
    public class FileProcessorService
    {
        private SpellCheckerService _spellChecker;
        private PhoneNumberFormatter _phoneFormatter;

        public FileProcessorService()
        {
            _spellChecker = new SpellCheckerService();
            _phoneFormatter = new PhoneNumberFormatter();
        }

        public CorrectionResult ProcessFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден");

            // Сначала исправляем орфографию
            var spellingResult = _spellChecker.CorrectFile(filePath);

            // Затем форматируем телефонные номера
            var phoneResult = _phoneFormatter.FormatPhoneNumbersInFile(filePath);

            // Объединяем результаты
            var combinedResult = new CorrectionResult(filePath);
            combinedResult.SpellingErrorsFixed = spellingResult.SpellingErrorsFixed;
            combinedResult.PhoneNumbersFixed = phoneResult.PhoneNumbersFixed;
            combinedResult.Changes.AddRange(spellingResult.Changes);
            combinedResult.Changes.AddRange(phoneResult.Changes);

            return combinedResult;
        }

        public List<CorrectionResult> ProcessDirectory(string directory, bool recursive = true)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"Директория {directory} не найдена");

            var results = new List<CorrectionResult>();
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(directory, "*.txt", searchOption);

            foreach (var file in files)
            {
                try
                {
                    Console.WriteLine($"Обработка файла: {file}");
                    var result = ProcessFile(file);
                    results.Add(result);
                    Console.WriteLine($"✓ Обработан: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Ошибка при обработке {file}: {ex.Message}");
                }
            }

            return results;
        }

        public void PrintResults(List<CorrectionResult> results)
        {
            Console.WriteLine("\n═══════════════════════════════════════");
            Console.WriteLine("          РЕЗУЛЬТАТЫ ОБРАБОТКИ");
            Console.WriteLine("═══════════════════════════════════════\n");

            int totalFiles = results.Count;
            int totalSpellingErrors = 0;
            int totalPhoneErrors = 0;

            foreach (var result in results)
            {
                Console.WriteLine($"Файл: {Path.GetFileName(result.FilePath)}");
                Console.WriteLine($"  Орфографических ошибок: {result.SpellingErrorsFixed}");
                Console.WriteLine($"  Телефонных номеров: {result.PhoneNumbersFixed}");

                if (result.Changes.Count > 0 && result.Changes.Count <= 5)
                {
                    Console.WriteLine("  Изменения:");
                    foreach (var change in result.Changes)
                    {
                        Console.WriteLine($"    - {change}");
                    }
                }
                else if (result.Changes.Count > 5)
                {
                    Console.WriteLine($"  Изменений: {result.Changes.Count} (показаны первые 5)");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine($"    - {result.Changes[i]}");
                    }
                }
                Console.WriteLine();

                totalSpellingErrors += result.SpellingErrorsFixed;
                totalPhoneErrors += result.PhoneNumbersFixed;
            }

            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine($"Всего обработано файлов: {totalFiles}");
            Console.WriteLine($"Всего исправлено орфографических ошибок: {totalSpellingErrors}");
            Console.WriteLine($"Всего исправлено телефонных номеров: {totalPhoneErrors}");
            Console.WriteLine("═══════════════════════════════════════");
        }
    }
}