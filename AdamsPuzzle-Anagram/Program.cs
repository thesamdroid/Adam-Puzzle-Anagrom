using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace AdamsPuzzle_Anagram
{
    internal class Program
    {
        private static readonly IList<string> TotallyAllWordsInTheEnglishDictionary = new List<string> {"nags", "sink", "kins", "sang" };

        private static void Main()
        {
            Dictionary<string, List<string>> workingDictionaryOfAllWords = TotallyAllWordsInTheEnglishDictionary.ToDictionary(word => word, word => word.Select(c => c.ToString()).ToList());

            List<List<string>> listOfAnagrams = new List<List<string>>();

            while (workingDictionaryOfAllWords.Any())
            {
                List<string> wordToAnalyze = workingDictionaryOfAllWords.Keys.FirstOrDefault()?.Select(c => c.ToString()).ToList();

                List<string> anagrams = FindAnagramsOfWord(workingDictionaryOfAllWords, wordToAnalyze);

                listOfAnagrams.Add(anagrams);
                foreach (string anagram in anagrams)
                {
                    KeyValuePair<string, List<string>> foundword = workingDictionaryOfAllWords.FirstOrDefault(kvp => kvp.Key == anagram);
                    workingDictionaryOfAllWords.Remove(foundword.Key);
                }

            }

            foreach (List<string> anagram in listOfAnagrams)
            {
                Console.WriteLine(string.Join("\t", anagram.Cast<string>().ToArray()));
            }
        }

        private static List<string> FindAnagramsOfWord(Dictionary<string, List<string>> workingDictionaryOfAllWords, List<string> lettersToAnalyze)
        {
            List<string> remainingLetters = lettersToAnalyze;
            Dictionary<string, List<string>> remainingWords = workingDictionaryOfAllWords;

            while (remainingLetters.Any())
            {
                string characterToLookFor = remainingLetters.First();

                remainingWords = remainingWords
                    .Where(kvp => kvp.Value.Contains(characterToLookFor))
                    .ToDictionary(x => x.Key, x => x.Value.Except(new List<string> { characterToLookFor }).ToList());

                remainingLetters = remainingLetters.Except(new List<string> { characterToLookFor }).ToList();
            }


            return remainingWords.Keys.ToList();
        }
    }
}
