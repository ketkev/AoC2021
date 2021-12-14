using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day14 : BaseDay
    {
        private readonly List<string> _stringInput;

        private LinkedList<char> _polymer;

        private Dictionary<string, char> _rules;

        public Day14()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();

            ParseInput();
        }

        private void ParseInput()
        {
            var blankIndex = _stringInput.FindIndex(str => str == "");

            _polymer = new LinkedList<char>();

            foreach (var c in _stringInput[0])
            {
                _polymer.AddLast(c);
            }

            _rules = new Dictionary<string, char>();

            for (int i = blankIndex + 1; i < _stringInput.Count; i++)
            {
                var (pair, element, _) = _stringInput[i].Split("->");

                _rules.Add(pair.Trim(), element.Trim()[0]);
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var steps = 10;

            for (int i = 0; i < steps; i++)
            {
                var current = _polymer.First;
                while (current != null)
                {
                    var next = current.Next;

                    if (next == null)
                    {
                        break;
                    }

                    string currentPair = current.Value.ToString() + next.Value.ToString();

                    if (_rules.ContainsKey(currentPair))
                    {
                        _polymer.AddAfter(current, _rules[currentPair]);
                    }

                    current = next;
                }
            }

            var counts = new Dictionary<char, int>();

            foreach (var c in _polymer)
            {
                if (!counts.ContainsKey(c))
                {
                    counts.Add(c, 0);
                }

                counts[c]++;
            }

            var mostCommon = counts.OrderByDescending(kvp => kvp.Value).First().Key;
            var leastCommon = counts.OrderBy(kvp => kvp.Value).First().Key;

            var mostCommonCount = counts[mostCommon];
            var leastCommonCount = counts[leastCommon];

            return new ValueTask<string>((mostCommonCount - leastCommonCount).ToString());
        }

        private Dictionary<string, long> CopyDictionary(Dictionary<string, long> original)
        {
            var copy = new Dictionary<string, long>();

            foreach (var kvp in original)
            {
                copy.Add(kvp.Key, kvp.Value);
            }

            return copy;
        }

        public override ValueTask<string> Solve_2()
        {
            ParseInput();

            var steps = 40;

            var state = new Dictionary<string, long>();

            foreach (var (key, _) in _rules)
            {
                state.Add(key, 0);
            }

            var current = _polymer.First;
            while (current != null)
            {
                var next = current.Next;

                if (next == null)
                {
                    break;
                }

                string currentPair = current.Value.ToString() + next.Value.ToString();

                if (!state.ContainsKey(currentPair))
                {
                    Console.WriteLine($"{currentPair} not found!");
                    state.Add(currentPair, 0);
                }

                state[currentPair]++;

                current = next;
            }

            var counts = new Dictionary<char, long>();

            foreach (var c in _polymer)
            {
                if (!counts.ContainsKey(c))
                {
                    counts.Add(c, 0);
                }

                counts[c]++;
            }

            for (int i = 0; i < steps; i++)
            {
                var newState = CopyDictionary(state);

                foreach (var (key, _) in state)
                {
                    string firstNewPair = key[0].ToString() + _rules[key].ToString();
                    string secondNewPair = _rules[key].ToString() + key[1].ToString();

                    newState[firstNewPair] += state[key];
                    newState[secondNewPair] += state[key];

                    if (!counts.ContainsKey(_rules[key]))
                    {
                        counts.Add(_rules[key], 0);
                    }

                    counts[_rules[key]] += state[key];

                    newState[key] -= state[key];
                }

                state = newState;
            }

            var mostCommon = counts.OrderByDescending(kvp => kvp.Value).First().Key;
            var leastCommon = counts.OrderBy(kvp => kvp.Value).First().Key;

            var mostCommonCount = counts[mostCommon];
            var leastCommonCount = counts[leastCommon];

            return new ValueTask<string>((mostCommonCount - leastCommonCount).ToString());
        }
    }
}