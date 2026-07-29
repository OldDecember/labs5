// Program.cs (финальная версия)
using System;
using System.IO;
using SpellCheckerProject.Services;
using SpellCheckerProject.Utils;

namespace SpellCheckerProject
{
    class Program
    {
        private static FileProcessorService _processor;

        static void Main(string[] args)
        {
            Console.Title = "Корректор текста";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║        КОРРЕКТОР ТЕКСТА                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();

            _processor = new FileProcessorService();

            bool exit = false;
            while (!exit)
            {
                try
                {
                    ShowMainMenu();
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ProcessSingleFile();
                            break;
                        case "2":
                            ProcessDirectory();
                            break;
                        case "3":
                            ShowDictionary();
                            break;
                        case "4":
                            CreateTestFiles();
                            break;
                        case "5":
                            exit = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nДо свидания! Спасибо за использование программы.");
                            Console.ResetColor();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Неверный выбор! Пожалуйста, введите число от 1 до 5.");
                            Console.ResetColor();
                            break;
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Критическая ошибка: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ShowMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine("│              ГЛАВНОЕ МЕНЮ              │");
            Console.WriteLine("└────────────────────────────────────────┘");
            Console.ResetColor();
            Console.WriteLine("1. Обработать один файл");
            Console.WriteLine("2. Обработать директорию");
            Console.WriteLine("3. Показать словарь ошибочных слов");
            Console.WriteLine("4. Создать тестовые файлы");
            Console.WriteLine("5. Выход");
            Console.Write("\nВаш выбор: ");
        }

        static void ProcessSingleFile()
        {
            Console.Write("Введите путь к файлу: ");
            string filePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Путь не может быть пустым!");
                return;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            // Показываем содержимое до обработки
            Console.WriteLine("\nСодержимое файла ДО обработки:");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine(File.ReadAllText(filePath));
            Console.WriteLine("═══════════════════════════════════════\n");

            Console.WriteLine("Обработка файла...");
            var result = _processor.ProcessFile(filePath);

            // Показываем содержимое после обработки
            Console.WriteLine("\nСодержимое файла ПОСЛЕ обработки:");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine(File.ReadAllText(filePath));
            Console.WriteLine("═══════════════════════════════════════\n");

            Console.WriteLine("РЕЗУЛЬТАТ:");
            Console.WriteLine($"  Орфографических ошибок: {result.SpellingErrorsFixed}");
            Console.WriteLine($"  Телефонных номеров: {result.PhoneNumbersFixed}");

            if (result.Changes.Count > 0)
            {
                Console.WriteLine("  Изменения:");
                foreach (var change in result.Changes)
                {
                    Console.WriteLine($"    - {change}");
                }
            }
        }

        static void ProcessDirectory()
        {
            Console.Write("Введите путь к директории: ");
            string directory = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(directory))
            {
                Console.WriteLine("Путь не может быть пустым!");
                return;
            }

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Директория не найдена!");
                return;
            }

            Console.Write("Обрабатывать поддиректории? (да/нет): ");
            bool recursive = Console.ReadLine()?.ToLower() == "да";

            Console.WriteLine("\nНачинается обработка файлов...");
            var results = _processor.ProcessDirectory(directory, recursive);
            _processor.PrintResults(results);
        }

        static void ShowDictionary()
        {
            Console.WriteLine("\n=== СЛОВАРЬ ОШИБОЧНЫХ СЛОВ ===\n");

            var words = Data.DictionaryData.GetMisspelledWords();

            foreach (var item in words)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"✓ {item.CorrectWord}");
                Console.ResetColor();
                Console.Write(" → ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Join(", ", item.Misspellings));
                Console.ResetColor();
            }

            Console.WriteLine($"\n📚 Всего слов в словаре: {words.Count}");
            Console.WriteLine($"📝 Всего вариантов ошибок: {words.Sum(w => w.Misspellings.Count)}");
        }

        static void CreateTestFiles()
        {
            Console.Write("Введите путь к директории для тестовых файлов: ");
            string directory = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(directory))
            {
                Console.WriteLine("Путь не может быть пустым!");
                return;
            }

            try
            {
                FileHelper.CreateTestFiles(directory);

                Console.WriteLine("\nХотите обработать созданные файлы? (да/нет): ");
                if (Console.ReadLine()?.ToLower() == "да")
                {
                    var results = _processor.ProcessDirectory(directory, false);
                    _processor.PrintResults(results);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании тестовых файлов: {ex.Message}");
            }
        }
    }
}