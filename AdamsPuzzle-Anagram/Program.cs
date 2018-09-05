using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace AdamsPuzzle_Anagram
{
    internal class Program
    {
        private static void Main()
        {
            var totallyAllWordsInTheEnglishDictionary = new List<string>();
            int counter = 0;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Sam\Desktop\wordlist.txt");
            while ((line = file.ReadLine()) != null)
            {
                totallyAllWordsInTheEnglishDictionary.Add(line);
                counter++;
            }


            //Dictionary<string, List<string>> workingDictionaryOfAllWords = totallyAllWordsInTheEnglishDictionary
            //    .ToDictionary(word => word, word => word.Select(c => c.ToString().ToLower()).ToList());

            IList<List<string>> listOfAnagrams = new List<List<string>>();

            while (totallyAllWordsInTheEnglishDictionary.Any())
            {
                List<string> wordToAnalyze = totallyAllWordsInTheEnglishDictionary.FirstOrDefault()?.Select(c => c.ToString().ToLower()).ToList();

                List<string> anagrams = FindAnagramsOfWord(totallyAllWordsInTheEnglishDictionary, wordToAnalyze);

                if (anagrams.Count > 1)
                    listOfAnagrams.Add(anagrams);
                foreach (string anagram in anagrams)
                {
                    totallyAllWordsInTheEnglishDictionary.Remove(anagram);
                }

                if (listOfAnagrams.Count % 1000 == 0)
                    Console.WriteLine(listOfAnagrams.Count);
            }

            Console.WriteLine(listOfAnagrams.Count);
        }

        private static List<string> FindAnagramsOfWord(IEnumerable<string> workingListOfWords, ICollection<string> wordToAnalyze)
        {
            IEnumerable<string> remainingWords = workingListOfWords.Where(word => word.Length == wordToAnalyze.Count);
            Dictionary<string, int> wordToAnalyzeCharToUsageCountDictionary = wordToAnalyze.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

            foreach (string letter in wordToAnalyze)
            {
                remainingWords = remainingWords.Where(word => word.ToLower().Contains(letter));
            }



            //remainingWords = remainingWords.Where(word => !word.Select(c => c.ToString().ToLower()).ToList().Except(wordToAnalyze).Any());

            if (remainingWords.Count() > 1)
            {
                remainingWords = remainingWords.Where(word =>
                {
                    Dictionary<string, int> keyWordCharToUsageCountDictionary = word.Select(c => c.ToString().ToLower()).ToList().GroupBy(c => c)
                        .ToDictionary(g => g.Key, g => g.Count());

                    foreach (KeyValuePair<string, int> character in wordToAnalyzeCharToUsageCountDictionary)
                    {
                        if (!keyWordCharToUsageCountDictionary.ContainsKey(character.Key))
                            return false;
                        if (keyWordCharToUsageCountDictionary[character.Key] != wordToAnalyzeCharToUsageCountDictionary[character.Key])
                            return false;
                    }

                    return true;
                });
            }

            return remainingWords.ToList();
        }
    }
}
