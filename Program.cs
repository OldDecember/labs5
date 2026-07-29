// Program.cs
using System;
using System.IO;
using SpellCheckerProject.Services;

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
                            exit = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nДо свидания!");
                            Console.ResetColor();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Неверный выбор!");
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
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
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
            Console.WriteLine("4. Выход");
            Console.Write("\nВаш выбор: ");
        }

        static void ProcessSingleFile()
        {
            Console.Write("Введите путь к файлу: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            Console.WriteLine("\nОбработка файла...");
            var result = _processor.ProcessFile(filePath);

            Console.WriteLine("\nРезультат:");
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

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Директория не найдена!");
                return;
            }

            Console.Write("Обрабатывать поддиректории? (да/нет): ");
            bool recursive = Console.ReadLine().ToLower() == "да";

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
                Console.Write($"Правильно: {item.CorrectWord}");
                Console.ResetColor();
                Console.Write(" → Ошибки: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Join(", ", item.Misspellings));
                Console.ResetColor();
            }

            Console.WriteLine($"\nВсего слов в словаре: {words.Count}");
        }
    }
}