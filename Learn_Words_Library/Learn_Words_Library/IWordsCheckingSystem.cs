namespace Learn_Words_Library
{
    public interface IWordsCheckingSystem
    {
        Dictionaries_List Dictionaries { get; set; }
        Mode ModeOfGenerating { get; set; }
        void LoadDictionary(string dictionaryName);
        bool CheckAnswer(string UsersAnswer);
        int GetCountOfCorrectAnswers();
        int GetCountOfAttempts();
        int GetCountOfWrongAnswers();
        string GetRightAnswer();
    }
}