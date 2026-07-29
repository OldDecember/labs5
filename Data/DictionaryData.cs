// Data/DictionaryData.cs
using System.Collections.Generic;
using SpellCheckerProject.Models;

namespace SpellCheckerProject.Data
{
    public static class DictionaryData
    {
        public static List<MisspelledWord> GetMisspelledWords()
        {
            return new List<MisspelledWord>
            {
                new MisspelledWord("привет", "првиет", "пирвет", "превет", "привт"),
                new MisspelledWord("здравствуйте", "здраствуйте", "здравствуте", "здрасьте"),
                new MisspelledWord("спасибо", "спасиба", "спсбо", "спсибо"),
                new MisspelledWord("пожалуйста", "пжалуйста", "пожалуста", "пожалста"),
                new MisspelledWord("извините", "извинити", "извените", "извенити"),
                new MisspelledWord("до свидания", "досвидания", "до свиданья", "досвиданья"),
                new MisspelledWord("компьютер", "компютер", "компьютерр", "комптер"),
                new MisspelledWord("программа", "програма", "прогамма", "программма"),
                new MisspelledWord("информация", "информациа", "информацыя", "инфомация"),
                new MisspelledWord("технология", "технологиа", "технология", "технолгия"),
                new MisspelledWord("разработка", "разработка", "разроботка", "разрботка"),
                new MisspelledWord("документация", "документациа", "докуменатция", "документацыя"),
                new MisspelledWord("интернет", "интернетт", "инет", "интэрнет"),
                new MisspelledWord("сайт", "сайтт", "саит", "сойт"),
                new MisspelledWord("электронная", "электроная", "електронная", "электорнная"),
                new MisspelledWord("почта", "пошта", "почьта", "пчта"),
                new MisspelledWord("адрес", "адресс", "адрс", "адрис"),
                new MisspelledWord("номер", "номмер", "нормер", "номр"),
                new MisspelledWord("телефон", "телефонн", "телевон", "телефн"),
                new MisspelledWord("мобильный", "мобильний", "мобилный", "мобильны")
            };
        }
    }
}