using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day03 : BaseDay
    {
        private string _epsilon = "";
        private string _gamma = "";

        private readonly List<string> _input;

        public Day03()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            for (var i = 0; i < _input.First().Length; i++)
            {
                var one_count = 0;
                var zero_count = 0;

                foreach (var row in _input)
                {
                    if (row[i] == '1')
                    {
                        one_count++;
                    }
                    else
                    {
                        zero_count++;
                    }
                }

                if (one_count > zero_count)
                {
                    _gamma += "1";
                    _epsilon += "0";
                }
                else
                {
                    _gamma += "0";
                    _epsilon += "1";
                }
            }

            var gamma_int = Convert.ToInt32(_gamma, 2);
            var epsilon_int = Convert.ToInt32(_epsilon, 2);

            var result = gamma_int * epsilon_int;

            return new ValueTask<string>(result.ToString());
        }

        private (int one_count, int zero_count) CountCommonBits(List<string> input, List<bool> validRows, int column)
        {
            var oneCount = 0;
            var zeroCount = 0;

            foreach (var row in _input)
            {
                if (validRows[input.IndexOf(row)])
                {
                    if (row[column] == '1')
                    {
                        oneCount++;
                    }
                    else
                    {
                        zeroCount++;
                    }
                }
            }

            return (oneCount, zeroCount);
        }

        private void InvalidateRows(List<string> input, List<bool> validRows, int column, char toInvalidate)
        {
            foreach (var row in _input)
            {
                if (row[column] == toInvalidate)
                {
                    validRows[input.IndexOf(row)] = false;
                }
            }
        }

        private int CountValidRows(List<bool> validRows)
        {
            var count = 0;

            foreach (var row in validRows)
            {
                if (row)
                {
                    count++;
                }
            }

            return count;
        }

        private string GetValidRow(List<string> input, List<bool> validRows)
        {
            for (var i = 0; i < input.Count; i++)
            {
                if (validRows[i])
                {
                    return input[i];
                }
            }

            return "";
        }

        public override ValueTask<string> Solve_2()
        {
            var validRows = new List<bool>(_input.Count);

            for (var i = 0; i < _input.Count; i++)
            {
                validRows.Add(true);
            }

            for (int i = 0; true; i++)
            {
                if (CountValidRows(validRows) == 1)
                {
                    break;
                }

                var (one_count, zero_count) = CountCommonBits(_input, validRows, i);

                if (one_count >= zero_count)
                {
                    InvalidateRows(_input, validRows, i, '0');
                }
                else
                {
                    InvalidateRows(_input, validRows, i, '1');
                }
            }

            var oxygen_rating = GetValidRow(_input, validRows);

            validRows.Clear();

            for (var i = 0; i < _input.Count; i++)
            {
                validRows.Add(true);
            }

            for (int i = 0; true; i++)
            {
                if (CountValidRows(validRows) == 1)
                {
                    break;
                }

                var (one_count, zero_count) = CountCommonBits(_input, validRows, i);

                if (one_count >= zero_count)
                {
                    InvalidateRows(_input, validRows, i, '1');
                }
                else
                {
                    InvalidateRows(_input, validRows, i, '0');
                }
            }

            var co2_rating = GetValidRow(_input, validRows);

            var oxygen_rating_int = Convert.ToInt32(oxygen_rating, 2);
            var co2_rating_int = Convert.ToInt32(co2_rating, 2);

            var result = oxygen_rating_int * co2_rating_int;

            return new ValueTask<string>(result.ToString());
        }
    }
}