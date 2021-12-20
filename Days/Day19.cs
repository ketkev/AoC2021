using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day19 : BaseDay
    {
        private readonly List<string> _stringInput;

        private List<List<Vector3Int>> _scanners;
        private Dictionary<int, List<Vector3Int>> _scannersIn0;
        private HashSet<Vector3Int> _knownPositions;
        private List<Vector3Int> _scannerPositions;

        public Day19()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
            ParseInput();
        }

        private void ParseInput()
        {
            _scanners = new List<List<Vector3Int>>();
            _knownPositions = new HashSet<Vector3Int>();
            _scannersIn0 = new Dictionary<int, List<Vector3Int>>();
            _scannerPositions = new List<Vector3Int>();

            List<Vector3Int> currentScanner = null;

            foreach (var line in _stringInput)
            {
                if (line.StartsWith("--- scanner "))
                {
                    if (currentScanner != null)
                    {
                        _scanners.Add(currentScanner);
                    }

                    currentScanner = new List<Vector3Int>();
                }
                else if (line.Length != 0)
                {
                    var (x, y, z, _) = line.Split(',');
                    currentScanner.Add(new Vector3Int(int.Parse(x), int.Parse(y), int.Parse(z)));
                }
            }

            _scanners.Add(currentScanner);
        }

        private List<Vector3Int> GetPossibleNegationPermutations(Vector3Int vec)
        {
            var result = new List<Vector3Int>
            {
                new Vector3Int(vec.x, vec.y, vec.z),
                new Vector3Int(-vec.x, vec.y, vec.z),
                new Vector3Int(-vec.x, -vec.y, vec.z),
                new Vector3Int(-vec.x, -vec.y, -vec.z),
                new Vector3Int(vec.x, -vec.y, -vec.z),
                new Vector3Int(vec.x, vec.y, -vec.z),
                new Vector3Int(vec.x, -vec.y, vec.z),
                new Vector3Int(-vec.x, vec.y, -vec.z)
            };

            return result;
        }

        private List<Vector3Int> GetPossibleRotations(Vector3Int vec)
        {
            // 3D math is hard, just try all permutations

            var result = new List<Vector3Int>();

            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.x, vec.y, vec.z)));
            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.x, vec.z, vec.y)));
            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.z, vec.x, vec.y)));
            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.z, vec.y, vec.x)));
            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.y, vec.z, vec.x)));
            result.AddRange(GetPossibleNegationPermutations(new Vector3Int(vec.y, vec.x, vec.z)));

            return result;
        }

        private bool FindOverlapping(List<Vector3Int> probesOne, List<Vector3Int> probesTwo, int probesTwoIndex)
        {
            var orientations = new Dictionary<int, int>();

            foreach (var probesOnePairOne in probesOne)
            {
                foreach (var probesOnePairTwo in probesOne)
                {
                    if (probesOnePairOne == probesOnePairTwo)
                    {
                        continue;
                    }

                    var deltaProbeOne = probesOnePairTwo - probesOnePairOne;

                    foreach (var probesTwoPairOne in probesTwo)
                    {
                        foreach (var probesTwoPairTwo in probesTwo)
                        {
                            if (probesTwoPairOne == probesTwoPairTwo)
                            {
                                continue;
                            }

                            var deltaProbeTwo = probesTwoPairTwo - probesTwoPairOne;

                            var deltaPermutations = GetPossibleRotations(deltaProbeTwo);

                            for (var i = 0; i < deltaPermutations.Count; i++)
                            {
                                if (deltaPermutations[i] == deltaProbeOne)
                                {
                                    if (!orientations.ContainsKey(i))
                                    {
                                        orientations.Add(i, 0);
                                    }

                                    orientations[i]++;
                                }
                            }
                        }
                    }
                }
            }

            var permutations = probesTwo.Select(GetPossibleRotations).ToList();

            foreach (var orientationsKey in orientations.Keys)
            {
                foreach (var probeOne in probesOne)
                {
                    foreach (var probeTwo in permutations)
                    {
                        var delta = probeOne - probeTwo[orientationsKey];

                        var shifted =
                            permutations.Select(item => item[orientationsKey].Shift(delta.x, delta.y, delta.z))
                                .ToList();

                        var probesOneSet = probesOne.ToHashSet();

                        var overlap = 0;

                        foreach (var probeTwoShifted in shifted)
                        {
                            if (probesOneSet.Contains(probeTwoShifted))
                            {
                                overlap++;
                            }
                        }

                        if (overlap >= 12)
                        {
                            foreach (var shiftedPosition in shifted)
                            {
                                _knownPositions.Add(shiftedPosition);
                            }

                            _scannerPositions.Add(delta);
                            _scannersIn0.Add(probesTwoIndex, shifted);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool EverythingTrue(List<bool> list)
        {
            return list.All(item => item);
        }

        public override ValueTask<string> Solve_1()
        {
            _scannersIn0.Add(0, _scanners[0]);
            _scannerPositions.Add(Vector3Int.Zero());

            foreach (var position in _scanners[0])
            {
                _knownPositions.Add(position);
            }

            var attempts = new HashSet<(int, int)>();
            var completed = new List<bool>();

            for (var i = 0; i < _scanners.Count; i++)
            {
                completed.Add(false);
            }

            completed[0] = true;

            for (var i = 1; i < _scanners.Count; i++)
            {
                var combination = (0, i);
                attempts.Add(combination);

                var foundMatch = FindOverlapping(_scanners[0], _scanners[i], i);

                if (foundMatch)
                {
                    completed[i] = true;
                }
            }

            while (!EverythingTrue(completed))
            {
                for (var i = 0; i < _scanners.Count; i++)
                {
                    if (!completed[i])
                        continue;

                    for (var j = 0; j < _scanners.Count; j++)
                    {
                        if (i == j)
                            continue;

                        if (completed[j])
                            continue;

                        if (attempts.Contains((i, j)))
                            continue;

                        attempts.Add((i, j));

                        var foundMatch = FindOverlapping(_scannersIn0[i], _scanners[j], j);

                        if (foundMatch)
                        {
                            completed[j] = true;
                        }
                    }
                }
            }

            return new ValueTask<string>(_knownPositions.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var largest = int.MinValue;

            foreach (var scannerPositionA in _scannerPositions)
            {
                foreach (var scannerPositionB in _scannerPositions)
                {
                    var distance = scannerPositionA.ManhattanDistance(scannerPositionB);

                    if (distance > largest)
                    {
                        largest = distance;
                    }
                }
            }

            return new ValueTask<string>(largest.ToString());
        }
    }
}