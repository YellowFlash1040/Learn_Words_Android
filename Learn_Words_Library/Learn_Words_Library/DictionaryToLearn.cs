using System.Collections.Generic;
using System.Text;

namespace Learn_Words_Library
{
    public class DictionaryToLearn
    {
        List<WordAndTranslations> WordsAndTranslations;

        IDataReaderWriter dataReaderWriter;

        public DictionaryToLearn(IDataReaderWriter dataReaderWriter)
        {
            WordsAndTranslations = new List<WordAndTranslations>();

            this.dataReaderWriter = dataReaderWriter;

            FillWordsAndTranslationsList();
        }

        public List<WordAndTranslations> GetCopyOfWordsAndTranslations()
        {
            List<WordAndTranslations> wordAndTranslations = new List<WordAndTranslations>();
            for (int i = 0; i < WordsAndTranslations.Count; i++)
            {
                wordAndTranslations.Add(WordsAndTranslations[i]);
            }

            return wordAndTranslations;
        }

        private void FillWordsAndTranslationsList()
        {
            string[] data = ReadDataFromFile();
            for (int i = 0; i < data.Length; i++)
            {
                string[] line = data[i].Split('=');

                if (line[0] == "" || line[1] == "")
                {
                    continue;
                }

                string word = line[0].Remove(line[0].Length - 1);
                string translation = line[1].Remove(0, 1);
                WordsAndTranslations.Add(new WordAndTranslations(word, translation));
            }
        }

        private string[] ReadDataFromFile()
        {
            string[] alldData = dataReaderWriter.ReadAllLines();
            List<string> correctData = new List<string>();
            for (int i = 0; i < alldData.Length; i++)
            {
                //if (alldData[i].Contains(" = "))
                if (alldData[i].Contains("="))
                {
                    correctData.Add(alldData[i]);
                }
            }

            return correctData.ToArray();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < WordsAndTranslations.Count; i++)
            {
                stringBuilder.Append(WordsAndTranslations[i].ToString() + '\n');
            }

            return stringBuilder.ToString();
        }

        public void Add_Word(string leftPart, string rightPart)
        {
            dataReaderWriter.AppendAllText($"{leftPart} = {rightPart}\n");
        }
    }
}