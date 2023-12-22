using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn_Words_Library
{
    public class WordAndAnswers
    {
        public string[] Answers;
        public string Word;

        public WordAndAnswers(string word, string[] answers)
        {
            Answers = answers;
            Word = word;
        }
    }
}
