using System.Collections.Generic;

namespace SpellCheckerProject.Models
{
    public class MisspelledWord
    {
        public string CorrectWord { get; set; }
        public List<string> Misspellings { get; set; }

        public MisspelledWord(string correctWord, params string[] misspellings)
        {
            CorrectWord = correctWord;
            Misspellings = new List<string>(misspellings);
        }

        public bool ContainsMisspelling(string word)
        {
            return Misspellings.Contains(word.ToLower());
        }

        public string GetCorrectWord(string misspelled)
        {
            return Misspellings.Contains(misspelled.ToLower()) ? CorrectWord : misspelled;
        }
    }
}