using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Learn_Words_Library
{
    public class WordsCheckingSystem:IWordsCheckingSystem
    {
        public Dictionaries_List Dictionaries { get; set; }
        private DictionaryToLearn activeDictionary;
        public DictionaryToLearn ActiveDictionary
        {
            get
            {
                return activeDictionary;
            }
            set
            {
                activeDictionary = value;
                allWordsAndTheirTranslationsFromActiveDictionary = activeDictionary.GetCopyOfWordsAndTranslations();
                wrongAnswers = new List<WordAndTranslations>();
                countOfCorrectAnswers = 0;
                countOfAttempts = 0;
            }
        }

        private Mode modeOfGenerating;
        public Mode ModeOfGenerating
        {
            get
            {
                return modeOfGenerating;
            }
            set
            {
                modeOfGenerating = value;
            }
        }

        protected Mode FromWhichModeWasGeneratedLastWord;

        protected List<WordAndTranslations> allWordsAndTheirTranslationsFromActiveDictionary;

        private int indexOfWordInWords;

        protected Random random;

        protected string[] TranslationsOfCurrentGenaratedWord;

        string[] generatedWord;

        private List<WordAndTranslations> wrongAnswers;

        private int countOfCorrectAnswers;

        private int countOfAttempts;

        public WordsCheckingSystem(IDataReaderWriter dataReaderWriter)
        {
            Dictionaries = new Dictionaries_List(dataReaderWriter);
            random = new Random();
        }
        public WordsCheckingSystem(Dictionaries_List dictionaries_List)
        {
            Dictionaries = dictionaries_List;
            random = new Random();
        }

        public virtual void LoadDictionary(string dictionaryName)
        {
            ActiveDictionary = Dictionaries.LoadDictionary(dictionaryName);
        }

        public string GetAdressOfDictionaryFileWithName(string dictionaryName)
        {
            return Dictionaries.GetAdressOfDictionaryFileWithName(dictionaryName);
        }

        public int GetCountOfCorrectAnswers()
        {
            return countOfCorrectAnswers;
        }

        public int GetCountOfAttempts()
        {
            return countOfAttempts;
        }

        public int GetCountOfWrongAnswers()
        {
            return countOfAttempts - countOfCorrectAnswers;
        }

        public virtual string GenerateWord()
        {
            if (allWordsAndTheirTranslationsFromActiveDictionary.Count > 0)
            {
                string word;

                if (ModeOfGenerating == Mode.FromFirstLanguage)
                {
                    word = GenerateWordFromFirstLanguage();
                }
                else if (ModeOfGenerating == Mode.FromSecondLanguage)
                {
                    word = GenerateWordFromSecondLanguage();
                }
                else
                {
                    int indexOfLanguage = random.Next(2);
                    if (indexOfLanguage == 0)
                    {
                        word = GenerateWordFromFirstLanguage();

                        FromWhichModeWasGeneratedLastWord = Mode.FromFirstLanguage;
                    }
                    else
                    {
                        word = GenerateWordFromSecondLanguage();

                        FromWhichModeWasGeneratedLastWord = Mode.FromSecondLanguage;
                    }
                }

                //generatedWord = word;

                return word;
            }
            else if (wrongAnswers.Count > 0)
            {
                for (int i = 0; i < wrongAnswers.Count; i++)
                {
                    allWordsAndTheirTranslationsFromActiveDictionary.Add(wrongAnswers[i]);
                }

                wrongAnswers = new List<WordAndTranslations>();
                return GenerateWord();
            }
            else
            {
                return "There are no words";
            }
        }

        private string GenerateWordFromSecondLanguage()
        {
            int indexOfWord = random.Next(allWordsAndTheirTranslationsFromActiveDictionary.Count);
            indexOfWordInWords = indexOfWord;
            WordAndTranslations wordAndTranslations = allWordsAndTheirTranslationsFromActiveDictionary[indexOfWord];
            int index = random.Next(wordAndTranslations.Translations.Length);
            string word = wordAndTranslations.Translations[index];
            TranslationsOfCurrentGenaratedWord = wordAndTranslations.Word;
            generatedWord = wordAndTranslations.Translations;

            return word;
        }

        private string GenerateWordFromFirstLanguage()
        {
            int indexOfWord = random.Next(allWordsAndTheirTranslationsFromActiveDictionary.Count);
            indexOfWordInWords = indexOfWord;
            WordAndTranslations wordAndTranslations = allWordsAndTheirTranslationsFromActiveDictionary[indexOfWord];
            int randomWordFromLeftPart = random.Next(wordAndTranslations.Word.Length);
            string word = wordAndTranslations.Word[randomWordFromLeftPart];
            generatedWord = wordAndTranslations.Word;
            TranslationsOfCurrentGenaratedWord = wordAndTranslations.Translations;

            return word;
        }

        public virtual bool CheckAnswer(string UsersAnswer)
        {
            countOfAttempts++;

            UsersAnswer = RemoveAllUnnecessary(UsersAnswer);

            for (int i = 0; i < TranslationsOfCurrentGenaratedWord.Length; i++)
            {
                string translation = RemoveAllUnnecessary(TranslationsOfCurrentGenaratedWord[i]);

                if (UsersAnswer == translation)
                {
                    countOfCorrectAnswers++;
                    allWordsAndTheirTranslationsFromActiveDictionary.RemoveAt(indexOfWordInWords);
                    return true;
                }
            }

            allWordsAndTheirTranslationsFromActiveDictionary.RemoveAt(indexOfWordInWords);
            if (FromWhichModeWasGeneratedLastWord == Mode.FromSecondLanguage)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < TranslationsOfCurrentGenaratedWord.Length - 1; i++)
                {
                    stringBuilder.Append(TranslationsOfCurrentGenaratedWord[i] + "\\");
                }
                stringBuilder.Append(TranslationsOfCurrentGenaratedWord[TranslationsOfCurrentGenaratedWord.Length - 1]);

                wrongAnswers.Add(new WordAndTranslations(stringBuilder.ToString(), generatedWord));
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < generatedWord.Length - 1; i++)
                {
                    stringBuilder.Append(generatedWord[i] + "\\");
                }
                stringBuilder.Append(generatedWord[generatedWord.Length - 1]);

                wrongAnswers.Add(new WordAndTranslations(stringBuilder.ToString(), TranslationsOfCurrentGenaratedWord));
            }

            return false;
        }

        protected string RemoveAllUnnecessary(string word)
        {
            string NewWord = word;

            NewWord = NewWord.ToLower();

            if (NewWord.Contains('('))
            {
                int indexOfStart = NewWord.IndexOf('(');
                int indexOfEnd = NewWord.IndexOf(')');
                NewWord = NewWord.Remove(indexOfStart, indexOfEnd - indexOfStart + 1);
            }
            if (NewWord.Contains('?'))
            {
                NewWord = NewWord.Replace("?", "");
            }
            if (NewWord.Contains('.'))
            {
                NewWord = NewWord.Replace(".", "");
            }
            if (NewWord.Contains('!'))
            {
                NewWord = NewWord.Replace("!", "");
            }
            if (NewWord.Contains(','))
            {
                NewWord = NewWord.Replace(",", "");
            }
            if (NewWord.Contains('\r'))
            {
                NewWord = NewWord.Replace("\r", "");
            }
            if(NewWord.Contains('-'))
            {
                NewWord = NewWord.Replace("-", "");
            }

            NewWord = RemoveExtraSpaces(NewWord);

            return NewWord;
        }

        protected static string RemoveExtraSpaces(string usersInput)
        {
            if (usersInput.Contains("  "))
            {
                for (int i = 0; i < usersInput.Length - 1; i++)
                {
                    if (usersInput[i] == ' ')
                    {
                        if (usersInput[i + 1] == ' ')
                        {
                            usersInput = usersInput.Remove(i, 1);
                            i--;
                        }
                    }
                }
            }

            if (usersInput[0] == ' ')
            {
                usersInput = usersInput.Remove(0, 1);
            }
            if (usersInput[usersInput.Length - 1] == ' ')
            {
                usersInput = usersInput.Remove(usersInput.Length - 1, 1);
            }

            return usersInput;
        }

        public string GetRightAnswer()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < TranslationsOfCurrentGenaratedWord.Length - 1; i++)
            {
                stringBuilder.Append(TranslationsOfCurrentGenaratedWord[i] + ", ");
            }
            stringBuilder.Append(TranslationsOfCurrentGenaratedWord[TranslationsOfCurrentGenaratedWord.Length - 1]);

            return stringBuilder.ToString();
        }
    }
}