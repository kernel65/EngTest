using System;
using System.Collections.Generic;
using System.Linq;

namespace EngTestWordsUI
{
    class EngTest
    {
        private Random rand;
        private Form2 form2;
        private String userCharChoose;

        //Кол-во верных ответов
        private int correctCount = 0;
        public int CorrectCount
        {
            get { return this.correctCount; }
            set { }
        }

        //Кол-во неверных ответов
        private int incorrectCount = 0;
        public int IncorrectCount
        {
            get { return this.incorrectCount; }
            set { }
        }

        //Содержит текущую рандомную букву
        private String randomCurrentChar = WordsRepository.EngUserChar;
        public String RandomCurrentChar
        {
            get { return randomCurrentChar; }
        }

        //Рандомный индекс слова
        private int randomWordIndex = 0;
        public int RandomWordIndex
        {
            get { return randomWordIndex; }
        }

        public EngTest(Form2 form2, Random rnd)
        {
            rand = rnd;
            this.form2 = form2;
        }

        public bool EqualsWords(String engWord, String wordLine, String ruWord)
        {
            List<String> wordsList = ParseWords(wordLine);
            this.form2.correctWordsLabel.Text = "";


            if (wordLine.Equals(ruWord))
            {
                this.correctCount++;
                this.form2.resultTextLabel.Text = "Верно!";
                this.form2.label5.Text = correctCount.ToString();
                DccRightWord();

                return true;
            }
            if (wordsList.Count > 1)
            {
                int tempCount = 0;
                List<String> answersList = ParseWords(ruWord);

                foreach (String str in wordsList)
                {
                    foreach (String answer in answersList)
                        if (str.Equals(answer))
                            tempCount++;
                }

                this.correctCount += tempCount;
                this.form2.label5.Text = correctCount.ToString();

                if (tempCount >= wordsList.Count)
                {
                    this.form2.resultTextLabel.Text = "Верно!";
                    WordsRepository.LimitWords[randomCurrentChar][randomWordIndex]--;
                    DccRightWord();
                    return true;
                }
                if (tempCount < wordsList.Count && tempCount > 0)
                {
                    this.form2.resultTextLabel.Text = "Верно!Но есть другие значения";
                    this.form2.correctWordsLabel.Text = "(" + wordLine + ")";
                    DccRightWord();
                    return true;
                }
            }

            incorrectCount++;
            this.form2.label6.Text = incorrectCount.ToString();
            this.form2.resultTextLabel.Text = "Не верно!Перевод слова " + engWord.ToUpper() + ":";
            this.form2.correctWordsLabel.Text = "(" + wordLine + ")";
            IncWrondWord();
            return false;
        }

        public Dictionary<String, String> GetRandomWord()
        {
            String eWord = null;
            String rWord = null;
            Dictionary<String, String> tempDict = new Dictionary<String, String>();


            if (CheckAvailableChars(WordsRepository.EngUserChar, WordsRepository.CurrentChars))
                userCharChoose = WordsRepository.EngUserChar;
            else
                userCharChoose = "All";

            if (userCharChoose != "All")
            {
                Dictionary<String, String> dict = WordsRepository.Words[userCharChoose];
                int randIndex = GetRandWordIndex(userCharChoose, dict.Count);

                eWord = dict.ElementAt(randIndex).Key;
                rWord = dict.ElementAt(randIndex).Value;

                //Сохраняем, чтобы в дальнейшем отвергнуть повторяющиеся слова
                randomCurrentChar = userCharChoose;
                randomWordIndex = randIndex;

                tempDict.Add(eWord, rWord);
                return tempDict;
            }

            //Выбираем рандомно букву
            int randChar = GetRandCharIndex(WordsRepository.CurrentChars);
            Dictionary<String, String> dct = WordsRepository.Words[WordsRepository.CurrentChars[randChar]];


            //Выбираем рандомно слово
            int randWord = GetRandWordIndex(WordsRepository.CurrentChars[randChar], dct.Count);
            String engWord = dct.ElementAt(randWord).Key;
            String ruWord = dct.ElementAt(randWord).Value;

            //Сохраняем, чтобы в дальнейшем отвергнуть повторяющиеся слова
            randomCurrentChar = WordsRepository.CurrentChars[randChar];
            randomWordIndex = randWord;

            tempDict.Add(engWord, ruWord);
            return tempDict;
        }

        public String GetEngWord(Dictionary<String, String> line)
        {
            return line.ElementAt(0).Key;
        }

        public String GetRuWord(Dictionary<String, String> line)
        {
            return line.ElementAt(0).Value;
        }

        private void DccRightWord()
        {
            WordsRepository.LimitWords[randomCurrentChar][randomWordIndex]--;
        }

        private void IncWrondWord()
        {
            if (WordsRepository.LimitWords[randomCurrentChar][randomWordIndex] < WordsRepository.countForIgnore)
                WordsRepository.LimitWords[randomCurrentChar][randomWordIndex]++;
        }

        private int GetRandCharIndex(List<String> currChars)
        {
            int rndChar = rand.Next(currChars.Count);
            List<int> tempList = WordsRepository.LimitWords[currChars[rndChar]];

            if (tempList[tempList.Count - 1] <= 0)
            {
                for (int i = 0; i < currChars.Count; i++)
                {
                    List<int> currLimits = WordsRepository.LimitWords[currChars[i]];

                    for (int j = 0; j < currLimits.Count; j++)
                    {
                        if (currLimits[j] > 0)
                            return i;

                        //Если все лимиты исчерпаны, инициализируем их заново
                        if (i >= (currChars.Count - 1) && j >= (currLimits.Count - 1))
                        {
                            foreach (String ch in WordsRepository.LimitWords.Keys)
                            {
                                for (int y = 0; y < WordsRepository.LimitWords[ch].Count; y++)
                                {
                                    WordsRepository.LimitWords[ch][y] = WordsRepository.countForIgnore;
                                }
                            }
                        }
                    }
                }
            }

            return rndChar;
        }

        private int GetRandWordIndex(String currentChar, int wordsCount)
        {
            int rndIndex = rand.Next(wordsCount);
            int limWordIndex = WordsRepository.LimitWords[currentChar][rndIndex];

            if (limWordIndex <= 0)
                for (int i = 0; i < wordsCount; i++)
                {

                    if (WordsRepository.LimitWords[currentChar][i] > 0)
                        return i;

                    if (i >= (wordsCount - 1))
                    {
                        //Обновляем лимиты слов
                        for (int j = 0; j < WordsRepository.LimitWords[currentChar].Count; j++)
                        {
                            WordsRepository.LimitWords[currentChar][j] = WordsRepository.countForIgnore;
                        }

                        return rndIndex;
                    }
                }

            return rndIndex;
        }

        private List<String> ParseWords(String line)
        {
            List<String> list = new List<string>();
            int index = 0;
            int firstIndex = 0;

            if (line.IndexOf(',', 0) < 0)
            {
                list.Add(line);
                return list;
            }

            while ((index = line.IndexOf(',', index + 1)) > -1)
            {
                String temp = line.Substring(firstIndex, index - firstIndex).Trim();
                firstIndex = index + 1;
                list.Add(temp);
            }

            list.Add(line.Substring(firstIndex, line.Length - firstIndex).Trim());

            return list;
        }

        private bool CheckAvailableChars(String userCharAvailable, List<String> currentCharsAvailable)
        {

            foreach (String chr in currentCharsAvailable)
            {
                if (userCharAvailable.Equals(chr))
                    return true;
            }

            return false;
        }
    }
}
