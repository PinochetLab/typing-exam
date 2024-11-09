using System;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class Word : IComparable<Word>
    {
        public Sprite Sprite { get; }

        public string EnglishWord { get; }
        public string RussianWord { get; }

        public Word(Sprite sprite)
        {
            Sprite = sprite;
            var ss = sprite.name.ToLower().Split('.');
            EnglishWord = ss[0];
            RussianWord = ss[1];
        }

        public int CompareTo(Word other)
        {
            var c = CompareToHelp(other);
            if (c == 0)
            {
                return UnityEngine.Random.Range(0, 2) * 2 - 1;
            }
            return c;
        }

        private int CompareToHelp(Word other)
        {
            if (LanguageController.CurrentLanguage == Language.English)
            {
                return EnglishWord.Length.CompareTo(other.EnglishWord.Length);
            }
            return RussianWord.Length.CompareTo(other.RussianWord.Length);
        }
    }
}