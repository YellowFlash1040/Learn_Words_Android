using System.Text;

namespace Learn_Words_Library
{
    public class WordAndTranslations
    {
        public string[] Word;
        public string[] Translations;
        public WordAndTranslations(string word, string translations)
        {
            Word = word.Split('\\');
            Translations = translations.Split('\\');
        }

        public WordAndTranslations(string word, string[] translations)
        {
            Word = word.Split('\\');
            Translations = translations;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Word.Length - 1; i++)
            {
                stringBuilder.Append(Word[i] + '\\');
            }
            stringBuilder.Append(Word[Word.Length - 1] + " - ");
            for (int i = 0; i < Translations.Length - 1; i++)
            {
                stringBuilder.Append(Translations[i] + '\\');
            }
            stringBuilder.Append(Translations[Translations.Length - 1]);

            return stringBuilder.ToString();
        }
    }
}