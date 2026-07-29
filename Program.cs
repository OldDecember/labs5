// Program.cs
using System;

namespace SpellCheckerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Корректор текста";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║        КОРРЕКТОР ТЕКСТА                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("Программа для исправления ошибок в текстовых файлах");
        }
    }
}