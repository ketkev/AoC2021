using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day08 : BaseDay
    {
        private readonly List<string> _input;

        public Day08()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            long counter = 0;
            foreach (var line in _input)
            {
                var (_, output, _) = line.Split('|');
                var words = output.Split(' ');
                foreach (var word in words)
                {
                    if (word.Length is 2 or 3 or 4 or 7)
                        counter++;
                }
            }

            return new ValueTask<string>(counter.ToString());
        }

        private int CounterOverlap(string a, string b)
        {
            return a.Intersect(b).Count();
        }

        public override ValueTask<string> Solve_2()
        {
            long counter = 0;

            var inputWords = new List<List<string>>();
            var outputWords = new List<List<string>>();

            foreach (var line in _input)
            {
                var (input, output, _) = line.Split('|');

                inputWords.Add(input.Trim().Split(' ').ToList());
                outputWords.Add(output.Trim().Split(' ').ToList());
            }

            for (int i = 0; i < _input.Count; i++)
            {
                var one = "";
                var four = "";
                var seven = "";
                var eight = "";


                for (var j = 0; j < inputWords[i].Count; j++)
                {
                    if (inputWords[i][j].Length is 2)
                    {
                        one = inputWords[i][j];
                    }
                    else if (inputWords[i][j].Length is 4)
                    {
                        four = inputWords[i][j];
                    }
                    else if (inputWords[i][j].Length is 3)
                    {
                        seven = inputWords[i][j];
                    }
                    else if (inputWords[i][j].Length is 7)
                    {
                        eight = inputWords[i][j];
                    }
                }

                if (one == "" || four == "" || seven == "" || eight == "")
                {
                    for (var j = 0; j < outputWords.Count; j++)
                    {
                        if (outputWords[i][j].Length is 2)
                        {
                            one = outputWords[i][j];
                        }
                        else if (outputWords[i][j].Length is 4)
                        {
                            four = outputWords[i][j];
                        }
                        else if (outputWords[i][j].Length is 3)
                        {
                            seven = outputWords[i][j];
                        }
                        else if (outputWords[i][j].Length is 7)
                        {
                            eight = outputWords[i][j];
                        }
                    }
                }

                var outputNumber = "";

                foreach (var word in outputWords[i])
                {
                    if (word.Length == 2)
                    {
                        outputNumber += "1";
                    }
                    else if (word.Length == 4)
                    {
                        outputNumber += "4";
                    }
                    else if (word.Length == 3)
                    {
                        outputNumber += "7";
                    }
                    else if (word.Length == 7)
                    {
                        outputNumber += "8";
                    }
                    else if (word.Length == 5)
                    {
                        var overlapOne = CounterOverlap(one, word);
                        var overlapFour = CounterOverlap(four, word);
                        var overlapSeven = CounterOverlap(seven, word);

                        if (overlapOne == 1 && overlapFour == 2 && overlapSeven == 2)
                        {
                            outputNumber += "2";
                        }

                        if (overlapOne == 2 && overlapFour == 3 && overlapSeven == 3)
                        {
                            outputNumber += "3";
                        }

                        if (overlapOne == 1 && overlapFour == 3 && overlapSeven == 2)
                        {
                            outputNumber += "5";
                        }
                    }
                    else if (word.Length == 6)
                    {
                        var overlapOne = CounterOverlap(one, word);
                        var overlapFour = CounterOverlap(four, word);
                        var overlapSeven = CounterOverlap(seven, word);

                        if (overlapOne == 2 && overlapFour == 3 && overlapSeven == 3)
                        {
                            outputNumber += "0";
                        }

                        if (overlapOne == 1 && overlapFour == 3 && overlapSeven == 2)
                        {
                            outputNumber += "6";
                        }

                        if (overlapOne == 2 && overlapFour == 4 && overlapSeven == 3)
                        {
                            outputNumber += "9";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                }

                counter += int.Parse(outputNumber);
            }

            return new ValueTask<string>(counter.ToString());
        }
    }
}