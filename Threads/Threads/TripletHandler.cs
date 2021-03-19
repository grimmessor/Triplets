using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class TripletHandler
    {
        public List<List<string>> cutList = new List<List<string>>();
        public Dictionary<string, int> triplets = new Dictionary<string, int>();
        static object locker = new object();
        public List<string> ReadFile(string path)
        {
            char[] charsToTrip = { '.', ',', ':', ';', '?', '!', '-', ')', '(', '\"', ']', '[' };
            string[] parts;
            List<string> words = new List<string>();
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    parts = line.Split();
                    foreach (var part in parts)
                    {
                        if (part.Length > 2)
                        {
                            words.Add(part.Trim(charsToTrip));
                        }
                    }
                }
            }
            return words;
        }

        public void CutText(List<string> words, int numOfCuts)
        {
            int i = words.Count / numOfCuts;
            if (i == 1)
            {
                numOfCuts = numOfCuts - (numOfCuts - words.Count / 2);
                i = words.Count / numOfCuts;
            }
            int j = 0;
            while (true)
            {
                if (words.Count - j > i)
                {
                    cutList.Add(words.GetRange(j, i));
                    j += i;
                }
                else
                {
                    cutList.Add(words.GetRange(j, words.Count - j));
                    break;
                }
            }
        }

        public void GetTriplets(List<string> words)
        {
            string triplet;
            int i = 0;
            foreach (var item in words)
            {
                while (i + 2 < item.Length)
                {
                    triplet = $"{item[i]}{item[i + 1]}{item[i + 2]}";
                    lock (locker)
                    {
                        if (triplets.ContainsKey(triplet))
                        {
                            triplets[triplet] += 1;
                        }
                        else
                        {
                            triplets.Add(triplet, 1);
                        }
                        i += 1;
                    }
                }
                i = 0;
            }
        }

        public void ShowDict(TripletHandler tripletHandler)
        {
            int i = 0;
            Dictionary<string, int> dict = tripletHandler.triplets.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key}, {item.Value}");
                i += 1;
                if (i == 10)
                {
                    break;
                }
            }
        }
    }
}
