using System;
using System.Collections.Generic;
using System.Linq;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Tools
{
    public static class Algorithm
    {
        /// <summary>
        ///     Sorts the list based on contextual string similarity.
        /// </summary>
        /// <param name="list">The list to sort in place</param>
        /// <param name="item">The item to compute the distance to</param>
        public static void ContextSort(this List<string> list, string item)
        {
            List<string> tmp = new List<string>();
            List<string> deleted = list.Cast<string>().ToList();

            string line = "";
            for (int i = 0; i < list.Count; i++)
            {
                line = ContextFind(deleted, item);
                tmp.Add(line);
                deleted.Remove(line);
            }

            list.Clear();
            list.AddRange(tmp.Cast<string>().ToArray());
        }

        /// <summary>
        ///     Finds the most similar item using Context Similarity algorithm.
        ///     Works only for lists of strings.
        /// </summary>
        /// <typeparam name="T">Type of list (works with string only)</typeparam>
        /// <param name="list">The list to search for most similar item</param>
        /// <param name="item">The referent item</param>
        /// <returns>Returns the most similar item from the list</returns>
        public static string ContextFind<T>(this List<T> list, string item)
        {
            return list.Cast<string>().ToList()[GetBestMatch(item, list.Cast<string>().ToList())];
        }

        // SimilarityCompare algorithm
        // Returns how similar string1 is to string2
        /// <summary>
        ///     ContextSimilarity algorithm
        /// </summary>
        /// <param name="pattern1"></param>
        /// <param name="pattern2"></param>
        /// <returns>
        ///     ContextualSimilarity score (how similar pattern1 is to pattern2)
        ///     on a scale of 0 to 1
        /// </returns>
        public static decimal SCompare(string pattern1, string pattern2)
        {
            int cardS1 = pattern1.Length;
            int cardS2 = pattern2.Length;

            pattern1 = pattern1.ToLower();
            pattern2 = pattern2.ToLower();

            decimal totalScore = 0.0m;

            for (int i = 0; i <= cardS1; i++)
            {
                for (int j = 0; j <= cardS1 - i; j++)
                {
                    if (pattern2.Contains(pattern1.Substring(i, j)))
                        totalScore += pattern1.Substring(i, j).Length;
                    else break;
                }
            }

            decimal normalization = (cardS1 * (cardS1 + 1) * (cardS1 + 2)) / 6.0m;
            return (totalScore / normalization);
        }

        /// <summary>
        ///     Find the pattern in arrayOfPatterns that best matches searchPattern
        /// </summary>
        /// <param name="searchPattern">The pattern we are searching for</param>
        /// <param name="arrayOfPatterns">All patterns available</param>
        /// <returns>Index of the best matching item</returns>
        public static int GetBestMatch(string searchPattern, List<string> arrayOfPatterns)
        {
            int bestIndex = 0;
            decimal bestScore = SCompare(searchPattern, arrayOfPatterns[bestIndex]);
            decimal tmpScore = 0.0m;

            for (int i = 1; i < arrayOfPatterns.Count; i++)
            {
                tmpScore = SCompare(searchPattern, arrayOfPatterns[i]);

                if (bestScore < tmpScore)
                {
                    bestIndex = i;
                    bestScore = tmpScore;
                }
            }

            return bestIndex;
        }



        /// <summary>
        ///     Implementation of SHA256 hashing algorithm.
        /// </summary>
        /// <param name="text">The text to hash</param>
        /// <returns>Hashed string</returns>
        public static string GetStringSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}