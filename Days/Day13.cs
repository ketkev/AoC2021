using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    class Fold
    {
        public bool Vertical;
        public int Axis;

        public Fold(int axis, bool vertical)
        {
            Axis = axis;
            Vertical = vertical;
        }
    }

    public sealed class Day13 : BaseDay
    {
        private readonly List<string> _stringInput;
        private List<Vector2Int> _dots;
        private List<Fold> _folds;

        public Day13()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
            
            ParseInput();
        }

        private void ParseInput()
        {
            _dots = new List<Vector2Int>();
            _folds = new List<Fold>();

            var foldsIndex = _stringInput.FindIndex(str => str == "");

            for (int i = 0; i < foldsIndex; i++)
            {
                var (x, y, _) = _stringInput[i].Split(',').Select(int.Parse);
                _dots.Add(new Vector2Int(x, y));
            }

            for (int i = foldsIndex + 1; i < _stringInput.Count; i++)
            {
                var (unfilteredDirection, axis, _) = _stringInput[i].Split('=');
                var direction = unfilteredDirection.Substring(unfilteredDirection.Length - 1, 1);

                _folds.Add(new Fold(int.Parse(axis), direction == "y"));
            }
        }

        private void FoldPaper(Fold fold)
        {
            if (fold.Vertical)
            {
                var toFold = _dots.Where(dot => dot.y > fold.Axis);

                foreach (var dot in toFold)
                {
                    var diff = dot.y - fold.Axis;
                    dot.y = fold.Axis - diff;
                }
            }
            else
            {
                var toFold = _dots.Where(dot => dot.x > fold.Axis);

                foreach (var dot in toFold)
                {
                    var diff = dot.x - fold.Axis;
                    dot.x = fold.Axis - diff;
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            foreach (var fold in _folds)
            {
                FoldPaper(fold);

                break;
            }

            var uniqueDots = new HashSet<Vector2Int>();

            foreach (var dot in _dots)
            {
                uniqueDots.Add(dot);
            }

            return new ValueTask<string>(uniqueDots.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            ParseInput();

            foreach (var fold in _folds)
            {
                FoldPaper(fold);
            }

            var maxX = _dots.Max(dot => dot.x);
            var maxY = _dots.Max(dot => dot.y);

            var grid = new int[maxX + 1, maxY + 1];

            foreach (var dot in _dots)
            {
                grid[dot.x, dot.y] = 1;
            }

            Console.WriteLine("Day 13 part 2:");
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    Console.Write(grid[x, y] == 1 ? "#" : ".");
                }

                Console.WriteLine();
            }

            return new ValueTask<string>("Read result from console");
        }
    }
}