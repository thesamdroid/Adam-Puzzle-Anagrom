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


            Dictionary<string, List<string>> workingDictionaryOfAllWords = totallyAllWordsInTheEnglishDictionary
                .ToDictionary(word => word, word => word.Select(c => c.ToString().ToLower()).ToList());

            List<List<string>> listOfAnagrams = new List<List<string>>();

            while (workingDictionaryOfAllWords.Any())
            {
                List<string> wordToAnalyze = workingDictionaryOfAllWords.Keys.FirstOrDefault()?.Select(c => c.ToString().ToLower()).ToList();

                List<string> anagrams = FindAnagramsOfWord(workingDictionaryOfAllWords, wordToAnalyze);

                listOfAnagrams.Add(anagrams);
                foreach (string anagram in anagrams)
                {
                    KeyValuePair<string, List<string>> foundword = workingDictionaryOfAllWords.FirstOrDefault(kvp => kvp.Key == anagram);
                    workingDictionaryOfAllWords.Remove(foundword.Key);
                }
                Console.WriteLine(string.Join("\t", anagrams.Cast<string>().ToArray()));

            }

            foreach (List<string> anagram in listOfAnagrams)
            {
                Console.WriteLine(string.Join("\t", anagram.Cast<string>().ToArray()));
            }
        }

        private static List<string> FindAnagramsOfWord(Dictionary<string, List<string>> workingDictionaryOfAllWords, List<string> wordToAnalyze)
        {
            List<string> remainingLetters = wordToAnalyze;
            Dictionary<string, List<string>> remainingWords = workingDictionaryOfAllWords.Where(kvp => kvp.Key.Length == wordToAnalyze.Count).ToDictionary(x => x.Key, x => x.Value);

            while (remainingLetters.Any())
            {
                string characterToLookFor = remainingLetters.First();

                remainingWords = remainingWords
                    .Where(kvp => kvp.Value.Contains(characterToLookFor.ToLower()) || kvp.Value.Contains(characterToLookFor.ToUpper()))
                    .ToDictionary(x => x.Key, x =>
                    {
                        var index = x.Value.IndexOf(characterToLookFor);
                        if (index > -1)
                            x.Value.RemoveAt(index);
                        index = x.Value.IndexOf(characterToLookFor.ToUpper());
                        if (index > -1)
                            x.Value.RemoveAt(index);
                        return x.Value;
                    });

                remainingLetters.RemoveAt(0);
            }


            return remainingWords.Keys.ToList();
        }
    }
}
