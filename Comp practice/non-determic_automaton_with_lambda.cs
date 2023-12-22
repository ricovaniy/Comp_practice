using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Comp_practice
{
    public class non_determic_automaton_with_lambda
    {
        class Automaton
        {
            private readonly int n;
            private readonly int alphPower;
            private readonly int[] starts;
            private readonly int[] ends;
            private readonly Dictionary<int, Dictionary<char, List<int>>> matrix = new();
            private readonly Dictionary<int, HashSet<int>> Cache = new ();
            public Automaton()
            {
                var pair = Console.ReadLine().Split();
                n = int.Parse(pair[0]);
                alphPower = int.Parse(pair[1]);
                var _ = Console.ReadLine();
                starts = Console.ReadLine().Split().Select(int.Parse).ToArray();
                _ = Console.ReadLine();
                ends = Console.ReadLine().Split().Select(int.Parse).ToArray();
                ReadMatrixData();
                PrecalculateLambdas();
            }

            private void PrecalculateLambdas()
            {
                for (var i = 0; i < n; i++)
                {
                    Cache[i] = GetAllNextLambdaStates(new HashSet<int>{i});
                }
            }

            public void ReadMatrixData()
            {
                for (var i = 0; i < n; i++)
                {
                    matrix[i] = new Dictionary<char, List<int>>();
                    for (var _ = 0; _ < alphPower; _++)
                    {
                        var condition = Console.ReadLine().Split()[0][0];
                        matrix[i][condition] = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse).ToList();
                    }
                }
            }

            public bool ReadWord(string word)
            {
                var states = starts.ToHashSet();
                foreach (var letter in word)
                {
                    states = GetNewStates(states, letter);
                }
                states.UnionWith(GetAllLambdasFromCache(states));
                return states.Overlaps(ends);
            }
            
            public  HashSet<int> GetNewStates(HashSet<int> currentStates, char letter)
            {
                var nextStates = new HashSet<int>();
                var lambda = GetAllLambdasFromCache(currentStates);
                foreach (var currentState in lambda)
                    nextStates.UnionWith(matrix[currentState][letter]);
                return nextStates;
            }

            private HashSet<int> GetAllLambdasFromCache(HashSet<int> states)
            {
                var res = new HashSet<int>();
                foreach (var state in states)
                {
                    res.UnionWith(Cache[state]);
                }

                return res;
            }
            private HashSet<int> GetAllNextLambdaStates(HashSet<int> current)
            {
                var nexts = current;
                while (true)
                {
                    var next = new HashSet<int>();
                    foreach (var nextState in nexts)
                    {
                        next.UnionWith(matrix[nextState]['_']);
                    }
                    if (next.Except(nexts).Any())
                    {
                        nexts.UnionWith(next);
                    }
                    else break;
                }
                return nexts;
            }
        }
        public static void Main()
        {
            var auto = new Automaton();
            var words = new List<string>();
            var wordsCount = int.Parse(Console.ReadLine());
            for (var __ = 0; __ < wordsCount; __++)
            {
                words.Add(Console.ReadLine());
            }
            foreach (var word in words)
            {
                Console.WriteLine(auto.ReadWord(word));
            }
        }
    }
}