using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Program
{
    public class Automate
    {
        private int n;
        private int alphPower;
        private HashSet<int> starts;
        private HashSet<int> ends;
        static Dictionary<int, Dictionary<char, List<int>>> matrix = new();

        public Automate(int n, int alphPower, IEnumerable<int> starts, IEnumerable<int> ends)
        {
            this.n = n;
            this.alphPower = alphPower;
            this.starts = starts.ToHashSet();
            this.ends = ends.ToHashSet();
        }

        public void GetMatrixData()
        {
            for (var i = 0; i < n; i++)
            {
                matrix[i] = new();
                for (var j = 0; j < alphPower; j++)
                {
                    var condition = Console.ReadLine().Split(' ')[0][0];
                    var states = Console.ReadLine()
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
                    matrix[i][condition] = states;
                }
            }
        }

        public bool ReadWord(string word)
        {
            var currentStates = starts;
            foreach (var letter in word)
            {
                currentStates = GetNewStates(currentStates, letter);
            }

            return currentStates.Overlaps(ends);
        }

        static HashSet<int> GetNewStates(HashSet<int> currentStates, char letter)
        {
            var nextStates = new HashSet<int>();
            foreach (var currentState in currentStates)
                nextStates.UnionWith(matrix[currentState][letter]);
            return nextStates;
        }
    }

    public static void Main42()
    {
        var str = Console.ReadLine().Split(' ');
        var n = int.Parse(str[0]);
        var alphPower = int.Parse(str[1]);
        var countOfStarts = int.Parse(Console.ReadLine());
        var starts = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
        var countOfEnds = int.Parse(Console.ReadLine());
        var ends = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
        var automate = new Automate(n, alphPower, starts, ends);
        automate.GetMatrixData();
        var countOfWords = int.Parse(Console.ReadLine());
        var words = new List<string>();
        for (var i = 0; i < countOfWords; i++)
        {
            if (automate.ReadWord(Console.ReadLine()))
                Console.WriteLine("True");
            else
                Console.WriteLine("False");
        }
    }
}