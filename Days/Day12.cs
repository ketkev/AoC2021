using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    class cave
    {
        public string Name;
        public bool IsBig;
        public List<cave> Neighbours;

        public cave(string name, bool isBig)
        {
            Name = name;
            IsBig = isBig;
            Neighbours = new List<cave>();
        }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(cave other)
        {
            return Name == other.Name && IsBig == other.IsBig;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((cave)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IsBig);
        }
    }

    public sealed class Day12 : BaseDay
    {
        private readonly List<string> _stringInput;
        private readonly List<cave> _caves;
        private readonly Dictionary<string, cave> _cavesDictionary;

        private List<LinkedList<cave>> _paths;

        public Day12()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();

            _caves = new List<cave>();
            _cavesDictionary = new Dictionary<string, cave>();
            var uniqueCaves = new HashSet<string>();

            foreach (var line in _stringInput)
            {
                var (firstName, secondName, _) = line.Split('-');
                uniqueCaves.Add(firstName);
                uniqueCaves.Add(secondName);
            }

            foreach (var caveName in uniqueCaves)
            {
                var isBig = IsUppercase(caveName);
                var cave = new cave(caveName, isBig);
                _caves.Add(cave);
                _cavesDictionary.Add(caveName, cave);
            }

            foreach (var line in _stringInput)
            {
                var (firstName, secondName, _) = line.Split('-');
                var firstCave = _cavesDictionary[firstName];
                var secondCave = _cavesDictionary[secondName];
                firstCave.Neighbours.Add(secondCave);
                secondCave.Neighbours.Add(firstCave);
            }

            _paths = new List<LinkedList<cave>>();
        }

        private static bool IsUppercase(string s)
        {
            return s.Any(char.IsUpper);
        }

        private void StepPath(cave c, LinkedList<cave> path)
        {
            if (path.Contains(c) && !c.IsBig)
            {
                return;
            }

            path.AddLast(c);

            if (c.Name == "end")
            {
                _paths.Add(path);
                return;
            }

            foreach (var neighbour in c.Neighbours)
            {
                var newPath = CopyPath(path);
                StepPath(neighbour, newPath);
            }
        }

        private void StepPath2(cave c, LinkedList<cave> path)
        {
            var (mostVisitedCount, mostVisitedCave, thisCaveCount) = MostSmallCaveVisits(c, path);

            if (mostVisitedCount == 2 && mostVisitedCave.Equals(c) ||
                !c.IsBig && mostVisitedCount == 2 && !mostVisitedCave.Equals(c) && thisCaveCount == 1)
            {
                return;
            }

            path.AddLast(c);

            if (c.Name == "end")
            {
                _paths.Add(path);
                return;
            }

            foreach (var neighbour in c.Neighbours)
            {
                if (neighbour.Name == "start") continue;

                var newPath = CopyPath(path);
                StepPath2(neighbour, newPath);
            }
        }

        private (int, cave, int) MostSmallCaveVisits(cave c, LinkedList<cave> path)
        {
            var visitDictionary = new Dictionary<cave, int>();

            var thisCaveCount = 0;

            var mostVisitedCount = 0;
            cave mostVisitedCave = null;

            foreach (var cave in path)
            {
                if (cave.IsBig) continue;

                if (!visitDictionary.ContainsKey(cave))
                {
                    visitDictionary.Add(cave, 0);
                }

                visitDictionary[cave]++;

                if (visitDictionary[cave] > mostVisitedCount)
                {
                    mostVisitedCount = visitDictionary[cave];
                    mostVisitedCave = cave;
                }

                if (cave.Equals(c))
                {
                    thisCaveCount = visitDictionary[cave];
                }
            }

            return (mostVisitedCount, mostVisitedCave, thisCaveCount);
        }

        private LinkedList<cave> CopyPath(LinkedList<cave> oldPath)
        {
            var newPath = new LinkedList<cave>();

            foreach (var cave in oldPath)
            {
                newPath.AddLast(cave);
            }

            return newPath;
        }

        public override ValueTask<string> Solve_1()
        {
            var startingCave = _cavesDictionary["start"];
            var path = new LinkedList<cave>();
            StepPath(startingCave, path);
            return new ValueTask<string>(_paths.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            _paths.Clear();

            var startingCave = _cavesDictionary["start"];
            var path = new LinkedList<cave>();
            StepPath2(startingCave, path);

            return new ValueTask<string>(_paths.Count.ToString());
        }
    }
}