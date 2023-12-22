using System.Collections.Generic;

namespace Learn_Words_Library
{
    public class ChooseWriteAnswerWordsCheckingSystem:WordsCheckingSystem
    {
        int countOfWrongOptions = 9;
        List<string> allWordsFromRightPart;
        List<string> allWordsFromLeftPart;

        public ChooseWriteAnswerWordsCheckingSystem(IDataReaderWriter dataReaderWriter):base(dataReaderWriter)
        {
        }

        public ChooseWriteAnswerWordsCheckingSystem(Dictionaries_List dictionaries_List):base(dictionaries_List)
        { 
        }

        public override void LoadDictionary(string dictionaryName)
        {
            base.LoadDictionary(dictionaryName);
            LoadAllWordsFromLeftPart();
            LoadAllWordsFromRightPart();
        }

        private void LoadAllWordsFromLeftPart()
        {
            allWordsFromLeftPart = new List<string>();
            for (int i = 0; i < allWordsAndTheirTranslationsFromActiveDictionary.Count; i++)
            {
                string[] wordsInLine = allWordsAndTheirTranslationsFromActiveDictionary[i].Word;
                for (int j = 0; j < wordsInLine.Length; j++)
                {
                    allWordsFromLeftPart.Add(wordsInLine[j]);
                }
            }

            RemoveWordsThatAreRepeated(allWordsFromLeftPart);
        }

        private void LoadAllWordsFromRightPart()
        {
            allWordsFromRightPart = new List<string>();
            for (int i = 0; i < allWordsAndTheirTranslationsFromActiveDictionary.Count; i++)
            {
                string[] wordsInLine = allWordsAndTheirTranslationsFromActiveDictionary[i].Translations;
                for (int j = 0; j < wordsInLine.Length; j++)
                {
                    allWordsFromRightPart.Add(wordsInLine[j]);
                }
            }

            RemoveWordsThatAreRepeated(allWordsFromRightPart);
        }

        private void RemoveWordsThatAreRepeated(List<string> allOptions)
        {
            for (int i = 0; i < allOptions.Count; i++)
            {
                for (int j = i + 1; j < allOptions.Count; j++)
                {
                    if (allOptions[j] == allOptions[i])
                    {
                        allOptions.RemoveAt(j);
                    }
                }
            }
        }

        public string[] GenerateAnswers()
        {
            string[] options;
            if (ModeOfGenerating == Mode.FromFirstLanguage)
            {
                options = GenerateAnswersFromSecondLanguage();
            }
            else if(ModeOfGenerating == Mode.FromSecondLanguage)
            {
                options = GenerateAnswersFromFirstLanguage();
            }
            else
            {
                if (FromWhichModeWasGeneratedLastWord == Mode.FromSecondLanguage)
                {
                    options = GenerateAnswersFromFirstLanguage();
                }
                else
                {
                    options = GenerateAnswersFromSecondLanguage();
                }
            }

            ShuffleArray(options);

            return options;
        }

        private List<string> GetCopyOf(List<string> list)
        {
            List<string> copy = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                copy.Add(list[i]);
            }

            return copy;
        }

        private string[] GenerateAnswersFromSecondLanguage()
        {
            return GenerateOptionsFrom(GetCopyOf(allWordsFromRightPart));
        }

        private string[] GenerateAnswersFromFirstLanguage()
        {
            return GenerateOptionsFrom(GetCopyOf(allWordsFromLeftPart));
        }

        private void RemoveAllRightAnswers(List<string> list, string[] answers)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < answers.Length; j++)
                {
                    if (list[i] == answers[j])
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private string[] GenerateOptionsFrom(List<string> allOptions)
        {
            ShuffleList(allOptions);

            string[] answers = TranslationsOfCurrentGenaratedWord;

            RemoveAllRightAnswers(allOptions, answers);

            string[] optionsToReturn = new string[countOfWrongOptions + 1];
            for (int i = 0; i < countOfWrongOptions; i++)
            {
                optionsToReturn[i] = allOptions[i];
            }
            string answer = answers[random.Next(answers.Length)];
            optionsToReturn[optionsToReturn.Length - 1] = answer;

            //int randomIndex = random.Next(optionsToReturn.Length);
            //string temp = optionsToReturn[randomIndex];
            //optionsToReturn[randomIndex] = answer;
            //optionsToReturn[optionsToReturn.Length - 1] = temp;

            return optionsToReturn;
        }

        private void ShuffleList(List<string> words)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    string temp = words[i];
                    int index = random.Next(words.Count);
                    words[i] = words[index];
                    words[index] = temp;
                }
            }
        }

        private void ShuffleArray(string[] words)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    string temp = words[i];
                    int index = random.Next(words.Length);
                    words[i] = words[index];
                    words[index] = temp;
                }
            }
        }

        private void ShuffleArray(WordAndTranslations[] copyOfWords)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < copyOfWords.Length; i++)
                {
                    WordAndTranslations temp = copyOfWords[i];
                    int index = random.Next(copyOfWords.Length);
                    copyOfWords[i] = copyOfWords[index];
                    copyOfWords[index] = temp;
                }
            }
        }

        public override bool CheckAnswer(string UsersAnswer)
        {
            if (UsersAnswer.Contains(","))
            {
                UsersAnswer = FixWord(UsersAnswer);
            }
            return base.CheckAnswer(UsersAnswer);
        }

        private string FixWord(string word)
        {
            string NewWord = word;
            while (NewWord.Contains("("))
            {
                int indexOfStart = NewWord.IndexOf('(');
                int indexOfEnd = NewWord.IndexOf(')');
                NewWord = NewWord.Remove(indexOfStart, indexOfEnd - indexOfStart + 1);
            }
            NewWord = NewWord.Split(',')[0];

            return NewWord;
        }
    }
}