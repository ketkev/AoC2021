using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day01 : BaseDay
    {
        private readonly string _input;

        private readonly List<int> _moduleMass;

        public Day01()
        {
            _input = File.ReadAllText(InputFilePath);
            _moduleMass = _input.Split('\n').Select(int.Parse).ToList();
        }

        private static int CalculateRequiredModuleFuel(int mass)
        {
            return mass / 3 - 2;
        }

        public override ValueTask<string> Solve_1()
        {
            var fuelRequirement = _moduleMass.Select(CalculateRequiredModuleFuel).Sum();
            return new ValueTask<string>($"{fuelRequirement}");
        }

        public override ValueTask<string> Solve_2()
        {
            var initialFuelRequirements = _moduleMass.Select(CalculateRequiredModuleFuel);
            var totalFuelRequirement = 0;

            foreach (var fuelRequirement in initialFuelRequirements)
            {
                var fuelRequirementForFuel = fuelRequirement;
                while (fuelRequirementForFuel > 0)
                {
                    totalFuelRequirement += fuelRequirementForFuel;
                    fuelRequirementForFuel = CalculateRequiredModuleFuel(fuelRequirementForFuel);
                }
            }

            return new ValueTask<string>($"{totalFuelRequirement}");
        }
    }
}