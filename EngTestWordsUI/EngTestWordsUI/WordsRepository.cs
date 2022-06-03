using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EngTestWordsUI
{
    class WordsRepository
    {
        public static String[] chars = {   "a", "b", "c", "d",
                                           "e", "f", "g", "h",
                                           "i", "j","k", "l",
                                           "m", "n", "o", "p",
                                           "q","r", "s", "t",
                                           "u", "v", "w", "x",
                                           "y", "z"
                                        };

        //Необходимое кол-во верных ответов, 
        //для исключения слова из коллекции.
        public static int countForIgnore = 3;

        //Содержит буквы, которые есть в текстовом файле
        private static List<String> currentChars = new List<string>();
        public static List<String> CurrentChars
        {
            get
            {
                if (currentChars.Count == 0 || currentChars == null)
                {
                    //Засунуть ф-цию
                    if (words.Count > 0)
                        currentChars = GetDictionaryChars(words);
                    else
                        return null;
                }

                return currentChars;
            }
        }

        //Выбранная буква
        private static String engChar = "All";
        public static String EngUserChar
        {
            get { return engChar; }
            set { engChar = value; }
        }

        //Здесь содержаться слова по алфавиту
        private static Dictionary<String, Dictionary<String, String>> words = new Dictionary<String, Dictionary<String, String>>();
        public static Dictionary<String, Dictionary<String, String>> Words
        {
            get
            {
                return words;
            }

            set
            {
                if (words["a"] == null)
                {
                    for (int i = 0; i < chars.Length; i++)
                    {
                        words[chars[i]] = new Dictionary<string, string>();
                    }

                }
            }
        }

        //Содержатся данные о кол-ве верных ответов.
        //Если ответ был верный n-ое кол-во раз, то в дальнейшем это слово не учитывается.
        private static Dictionary<String, List<int>> limitWords = new Dictionary<String, List<int>>();
        public static Dictionary<String, List<int>> LimitWords
        {
            get
            {
                return limitWords;
            }

            set
            {
                if (limitWords["a"] == null)
                {
                    for (int i = 0; i < chars.Length; i++)
                    {
                        limitWords[chars[i]] = new List<int>();
                    }
                }
            }
        }

        public static List<String> ReadWordsFromTxtFile(String fileName)
        {
            List<String> lines = new List<string>();
            IniDictionary();

            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("windows-1251")))
                {
                    while (sr.Peek() > -1)
                    {
                        String line = sr.ReadLine();
                        lines.Add(line);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                //Вывести всплывающее сообщение, что файл отсутствует
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return lines;
        }

        public static void ParseLines(List<String> lines)
        {
            String engLine = null;
            String ruLine = null;

            foreach (String line in lines)
            {
                try
                {
                    int index = line.IndexOf("-", 0);

                    if (index > 0 && line.Length > 3)
                    {
                        engLine = line.Substring(0, index).Trim().ToLower();
                        ruLine = line.Substring(index + 1, line.Length - index - 1).Trim().ToLower();
                        AddWordsToDictionary(engLine, ruLine);
                        IniLimitWords(engLine);
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void IniLimitWords(String engLine)
        {
            String firstChar = engLine.Substring(0, 1).ToLower();

            for (int i = 0; i < chars.Length; i++)
            {
                if (firstChar.Equals(chars[i]))
                {
                    LimitWords[chars[i]].Add(countForIgnore);
                    return;
                }
            }
        }

        private static void AddWordsToDictionary(String engLine, String ruLine)
        {
            String firstChar = engLine.Substring(0, 1).ToLower();

            for (int i = 0; i < chars.Length; i++)
            {
                if (firstChar.Equals(chars[i]))
                {
                    Words[chars[i]].Add(engLine, ruLine);
                    return;
                }
            }
        }

        private static void IniDictionary()
        {
            for (int i = 0; i < chars.Length; i++)
            {
                words.Add(chars[i], new Dictionary<string, string>());
                limitWords.Add(chars[i], new List<int>());
            }
        }

        private static List<String> GetDictionaryChars(Dictionary<String, Dictionary<String, String>> wrds)
        {
            List<String> chars = new List<string>();

            foreach (String w in wrds.Keys)
            {

                if (wrds[w].Count != 0)
                    chars.Add(w);
            }

            return chars;
        }
    }
}
