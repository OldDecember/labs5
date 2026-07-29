// Utils/FileHelper.cs
using System;
using System.IO;

namespace SpellCheckerProject.Utils
{
    public static class FileHelper
    {
        public static void CreateDemoFile(string path)
        {
            if (File.Exists(path))
                return;

            string content = @"Здраствуйте! Меня зовут Петр.
Я программист и хочу поделится своим опытом.
Мой мобильний телефон: (012) 345-67-89.
Также вы можете связатся со мной по електронной почте.

Благодарю за внемание!
До свидания!";

            File.WriteAllText(path, content);
            Console.WriteLine($"Создан демонстрационный файл: {path}");
        }

        public static void CreateTestFiles(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var testFiles = new[]
            {
                ("test1.txt", @"Првиет! Это тестовый фаил.
Мой номер телефона: (012) 345-67-89.
Спасибо за внемание!"),

                ("test2.txt", @"Здраствуйте! Я хочу извинится.
Мой компютер сломался.
Позвоните мне: (012) 345-67-89 или (099) 123-45-67."),

                ("test3.txt", @"Пожалуста, проверте этот текст.
В нем много ошыбок.
Мой телефон: (012) 345-67-89."),

                ("test4.txt", @"Добрый день! 
Это тестовый фаил для проверки корректора.
Телефон для связи: (012) 345-67-89.
До свидания!")
            };

            foreach (var (fileName, content) in testFiles)
            {
                string fullPath = Path.Combine(directory, fileName);
                File.WriteAllText(fullPath, content);
            }

            Console.WriteLine($"Создано {testFiles.Length} тестовых файлов в {directory}");
        }
    }
}