using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    class Zone
    {
        public Vector2Int CornerBL;
        public Vector2Int CornerTR;

        public Zone(Vector2Int cornerBl, Vector2Int cornerTr)
        {
            CornerBL = cornerBl;
            CornerTR = cornerTr;
        }
    }

    class Probe
    {
        public Vector2Int Position;

        public Vector2Int Velocity;

        private Zone _zone;

        public bool HasBeenInZone;

        public Probe(Vector2Int velocity, Zone zone)
        {
            Position = new Vector2Int(0, 0);
            Velocity = velocity;
            HasBeenInZone = false;
            _zone = zone;
        }

        public bool IsInZone()
        {
            var inZone = Position.x >= _zone.CornerBL.x && Position.x <= _zone.CornerTR.x &&
                         Position.y >= _zone.CornerBL.y &&
                         Position.y <= _zone.CornerTR.y;

            HasBeenInZone = HasBeenInZone || inZone;

            return inZone;
        }

        public bool CanReachZone()
        {
            if (Position.y < _zone.CornerBL.y && Velocity.y < 0)
            {
                return false;
            }

            return true;
        }

        public void Step()
        {
            Position.x += Velocity.x;
            Position.y += Velocity.y;

            if (Velocity.x > 0)
            {
                Velocity.x--;
            }
            else if (Velocity.x < 0)
            {
                Velocity.x++;
            }

            Velocity.y--;
        }
    }

    public sealed class Day17 : BaseDay
    {
        private readonly string _stringInput;
        private Zone _targetZone;

        public Day17()
        {
            _stringInput = File.ReadAllText(InputFilePath);
            // _stringInput = "target area: x=20..30, y=-10..-5";

            var (xMin, xMax, yMin, yMax, _) = RegexHelper.GetMatches(_stringInput, @"-?\d+").Select(item => item.Value)
                .Select(int.Parse);

            _targetZone = new Zone(new Vector2Int(xMin, yMin), new Vector2Int(xMax, yMax));
        }

        public override ValueTask<string> Solve_1()
        {
            var probe = new Probe(new Vector2Int(0, 0), _targetZone);

            var highestPoint = int.MinValue;
            var highestPointLaunchAngle = new Vector2Int(0, 0);

            var steps = 500;
            var range = 500;

            for (int x = 0; x < range; x++)
            {
                for (int y = 0; y < range; y++)
                {
                    // var probe = new Probe(new Vector2Int(x, y), _targetZone);

                    probe.Position.x = 0;
                    probe.Position.y = 0;
                    probe.Velocity.x = x;
                    probe.Velocity.y = y;
                    probe.HasBeenInZone = false;

                    var highestPointInLaunch = int.MinValue;

                    for (int i = 0; i < steps; i++)
                    {
                        probe.Step();

                        if (probe.IsInZone())
                            break;

                        if (!probe.CanReachZone())
                            break;

                        if (probe.Position.y > highestPointInLaunch)
                        {
                            highestPointInLaunch = probe.Position.y;
                        }
                    }

                    if (highestPointInLaunch > highestPoint && probe.HasBeenInZone)
                    {
                        highestPoint = highestPointInLaunch;
                        highestPointLaunchAngle = new Vector2Int(x, y);
                    }
                }
            }

            return new ValueTask<string>(highestPoint + " with " + highestPointLaunchAngle.x + "," +
                                         highestPointLaunchAngle.y);
        }

        public override ValueTask<string> Solve_2()
        {
            var probe = new Probe(new Vector2Int(0, 0), _targetZone);

            var possibleVelocities = new HashSet<Vector2Int>();

            var steps = 500;
            var range = 500;

            for (int x = -range; x < range; x++)
            {
                for (int y = -range; y < range; y++)
                {
                    // var probe = new Probe(new Vector2Int(x, y), _targetZone);

                    probe.Position.x = 0;
                    probe.Position.y = 0;
                    probe.Velocity.x = x;
                    probe.Velocity.y = y;
                    probe.HasBeenInZone = false;

                    for (int i = 0; i < steps; i++)
                    {
                        probe.Step();

                        if (probe.IsInZone())
                            break;

                        if (!probe.CanReachZone())
                            break;
                    }

                    if (probe.HasBeenInZone)
                    {
                        possibleVelocities.Add(new Vector2Int(x, y));
                    }
                }
            }

            return new ValueTask<string>(possibleVelocities.Count.ToString());
        }
    }
}