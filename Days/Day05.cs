using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public class Line
    {
        public Vector2Long From;
        public Vector2Long To;

        public Line(Vector2Long from, Vector2Long to)
        {
            From = from;
            To = to;
        }
    }

    public sealed class Day05 : BaseDay
    {
        private readonly List<string> _input;
        private List<Line> _linesPart1;
        private List<Line> _linesPart2;

        public Day05()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
            _linesPart1 = new List<Line>();
            _linesPart2 = new List<Line>();

            foreach (var s in _input)
            {
                var (one, two, rest) = s.Split("->");

                var (x1, y1, rest2) = one.Split(",");
                var (x2, y2, rest3) = two.Split(",");

                var x1l = long.Parse(x1);
                var y1l = long.Parse(y1);
                var x2l = long.Parse(x2);
                var y2l = long.Parse(y2);

                if (x1l == x2l || y1l == y2l)
                {
                    _linesPart1.Add(new Line(new Vector2Long(x1l, y1l),
                        new Vector2Long(x2l, y2l)));
                    _linesPart2.Add(new Line(new Vector2Long(x1l, y1l),
                        new Vector2Long(x2l, y2l)));
                }
                else
                {
                    _linesPart2.Add(new Line(new Vector2Long(x1l, y1l),
                        new Vector2Long(x2l, y2l)));
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var intersections = new Dictionary<Vector2Long, int>();

            foreach (var line in _linesPart1)
            {
                if (line.From.x == line.To.x) // vertical
                {
                    var min = Math.Min(line.From.y, line.To.y);
                    var max = Math.Max(line.From.y, line.To.y);

                    for (var i = min; i <= max; i++)
                    {
                        var point = new Vector2Long(line.From.x, i);
                        if (intersections.ContainsKey(point))
                        {
                            intersections[point]++;
                        }
                        else
                        {
                            intersections.Add(point, 1);
                        }
                    }
                }
                else // horizontal
                {
                    var min = Math.Min(line.From.x, line.To.x);
                    var max = Math.Max(line.From.x, line.To.x);

                    for (var i = min; i <= max; i++)
                    {
                        var point = new Vector2Long(i, line.From.y);
                        if (intersections.ContainsKey(point))
                        {
                            intersections[point]++;
                        }
                        else
                        {
                            intersections.Add(point, 1);
                        }
                    }
                }
            }


            var count = intersections.LongCount(keyValuePair => keyValuePair.Value > 1);

            return new ValueTask<string>(count.ToString());
        }

        public static IEnumerable<Vector2Long> GetPointsOnLine(Vector2Long from, Vector2Long to)
        {
            int x0 = (int)from.x;
            int y0 = (int)from.y;
            int x1 = (int)to.x;
            int y1 = (int)to.y;

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }

            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Vector2Long((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }

            yield break;
        }


        public override ValueTask<string> Solve_2()
        {
            var intersections = new Dictionary<Vector2Long, int>();

            foreach (var line in _linesPart2)
            {
                if (line.From.x == line.To.x) // vertical
                {
                    var min = Math.Min(line.From.y, line.To.y);
                    var max = Math.Max(line.From.y, line.To.y);

                    for (var i = min; i <= max; i++)
                    {
                        var point = new Vector2Long(line.From.x, i);

                        if (intersections.ContainsKey(point))
                        {
                            intersections[point]++;
                        }
                        else
                        {
                            intersections.Add(point, 1);
                        }
                    }
                }
                else if (line.From.y == line.To.y) // horizontal
                {
                    var min = Math.Min(line.From.x, line.To.x);
                    var max = Math.Max(line.From.x, line.To.x);

                    for (var i = min; i <= max; i++)
                    {
                        var point = new Vector2Long(i, line.From.y);

                        if (intersections.ContainsKey(point))
                        {
                            intersections[point]++;
                        }
                        else
                        {
                            intersections.Add(point, 1);
                        }
                    }
                }
                else
                {
                    var enumerator = GetPointsOnLine(line.From, line.To).ToList();

                    foreach (var point in enumerator)
                    {
                        if (intersections.ContainsKey(point))
                        {
                            intersections[point]++;
                        }
                        else
                        {
                            intersections.Add(point, 1);
                        }
                    }
                }
            }

            var count = intersections.LongCount(keyValuePair => keyValuePair.Value > 1);
            
            return new ValueTask<string>(count.ToString());
        }
    }
}