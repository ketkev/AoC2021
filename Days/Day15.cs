using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoC2021.utils.graph;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day15 : BaseDay
    {
        private readonly List<string> _stringInput;
        private List<List<int>> _input;
        private Graph _graph;

        public Day15()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
            _input = new List<List<int>>();

            foreach (var line in _stringInput)
            {
                var lineList = new List<int>();

                foreach (var c in line)
                {
                    lineList.Add((int)char.GetNumericValue(c));
                }

                _input.Add(lineList);
            }

            CreateGraph();
        }

        private void CreateGraph()
        {
            _graph = new Graph();

            for (int x = 0; x < _input.First().Count; x++)
            {
                for (int y = 0; y < _input.Count; y++)
                {
                    var surrounding = GetSurrounding(x, y);

                    foreach (var vector2Int in surrounding)
                    {
                        _graph.AddEdge($"{x}, {y}", $"{vector2Int.x}, {vector2Int.y}",
                            _input[vector2Int.y][vector2Int.x]);
                    }
                }
            }
        }

        private List<Vector2Int> GetSurrounding(int x, int y)
        {
            var list = new List<Vector2Int>();

            if (y - 1 >= 0)
            {
                list.Add(new Vector2Int(x, y - 1));
            }

            if (x + 1 < _input[y].Count)
            {
                list.Add(new Vector2Int(x + 1, y));
            }

            if (y + 1 < _input.Count)
            {
                list.Add(new Vector2Int(x, y + 1));
            }

            if (x - 1 >= 0)
            {
                list.Add(new Vector2Int(x - 1, y));
            }

            return list;
        }

        private void CreateNewMap()
        {
            _input = new List<List<int>>();

            for (var y = 0; y < _stringInput.Count * 5; y++)
            {
                var lineList = new List<int>();

                for (var x = 0; x < _stringInput[y % _stringInput.Count].Length * 5; x++)
                {
                    // :(
                    // Blame 6 uur 's ochtends voor deze code
                    var c = _stringInput[y % _stringInput.Count][x % _stringInput[y % _stringInput.Count].Length];
                    var value = (int)char.GetNumericValue(c) + x / _stringInput[y % _stringInput.Count].Length +
                                y / _stringInput.Count;

                    if (value > 9)
                    {
                        if (value % 10 == 0)
                        {
                            value = 1;
                        }
                        else
                        {
                            value = value % 10 + 1;
                        }
                    }

                    lineList.Add(value);
                }

                _input.Add(lineList);
            }
        }

        public override ValueTask<string> Solve_1()
        {
            _graph.Dijkstra("0, 0");

            var exitCoordinates = $"{_input.First().Count - 1}, {_input.Count - 1}";
            var exit = _graph.GetVertex(exitCoordinates);

            return new ValueTask<string>(exit.distance.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            CreateNewMap();
            CreateGraph();

            _graph.Dijkstra("0, 0");

            var exitCoordinates = $"{_input.First().Count - 1}, {_input.Count - 1}";
            var exit = _graph.GetVertex(exitCoordinates);

            return new ValueTask<string>(exit.distance.ToString());
        }
    }
}