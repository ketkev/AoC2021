using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day10 : BaseDay
    {
        private readonly List<string> _input;

        public Day10()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var corruptedCharacters = new List<char>();

            var validValues = new Dictionary<char, char>
            {
                { '{', '}' },
                { '[', ']' },
                { '(', ')' },
                { '<', '>' }
            };

            var openingCharacters = new HashSet<char> { '{', '[', '(', '<' };
            var closingCharacters = new HashSet<char> { '}', ']', ')', '>' };

            var stack = new Stack<char>();

            foreach (var line in _input)
            {
                foreach (var character in line)
                {
                    if (openingCharacters.Contains(character))
                    {
                        stack.Push(character);
                    }
                    else if (closingCharacters.Contains(character))
                    {
                        if (stack.Count == 0)
                        {
                            return new ValueTask<string>($"Invalid input: {line}");
                        }

                        var openingCharacter = stack.Pop();
                        if (validValues[openingCharacter] != character)
                        {
                            corruptedCharacters.Add(character);
                            break;
                        }
                    }
                }
            }

            var corruptedCharacterScores = new Dictionary<char, int>
            {
                { '}', 1197 },
                { ']', 57 },
                { ')', 3 },
                { '>', 25137 }
            };

            var score = corruptedCharacters.Sum(c => corruptedCharacterScores[c]);

            return new ValueTask<string>(score.ToString());
        }


        public override ValueTask<string> Solve_2()
        {
            var corrections = new List<string>();

            var validValues = new Dictionary<char, char>
            {
                { '{', '}' },
                { '[', ']' },
                { '(', ')' },
                { '<', '>' }
            };

            var openingCharacters = new HashSet<char> { '{', '[', '(', '<' };
            var closingCharacters = new HashSet<char> { '}', ']', ')', '>' };

            var stack = new Stack<char>();

            foreach (var line in _input)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (openingCharacters.Contains(line[i]))
                    {
                        stack.Push(line[i]);
                    }
                    else if (closingCharacters.Contains(line[i]))
                    {
                        if (stack.Count == 0)
                        {
                            return new ValueTask<string>($"Invalid input: {line}");
                        }

                        var openingCharacter = stack.Pop();
                        if (validValues[openingCharacter] != line[i])
                        {
                            // Corrupted line
                            break;
                        }
                    }

                    if (i == line.Length - 1 && stack.Count > 0)
                    {
                        var correction = "";

                        while (stack.Count != 0)
                        {
                            var character = stack.Pop();
                            correction += validValues[character];
                        }

                        corrections.Add(correction);
                        break;
                    }
                }

                stack.Clear();
            }

            var correctedCharacterScores = new Dictionary<char, int>
            {
                { '}', 3 },
                { ']', 2 },
                { ')', 1 },
                { '>', 4 }
            };

            var scores = new List<long>();

            foreach (var correction in corrections)
            {
                var score = 0;
                foreach (var character in correction)
                {
                    score *= 5;
                    score += correctedCharacterScores[character];
                }

                scores.Add(score);
            }

            scores.Sort();
            
            return new ValueTask<string>(scores[scores.Count / 2].ToString());
        }
    }
}