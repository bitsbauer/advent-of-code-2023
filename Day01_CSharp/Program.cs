using System.Text.RegularExpressions;

namespace Day01
{
    class Trebuchet
    {
        private static readonly Dictionary<string, string> NumberMapping = new()
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };

        private static int CalculateCalibration(string[] lines)
        {
            int sum = 0;
            foreach (var line in lines)
            {
                var digits = line.Where(char.IsDigit).ToArray();
                if (digits.Length > 0)
                {
                    int value = int.Parse(digits.First().ToString() + digits.Last().ToString());
                    sum += value;
                }
            }
            return sum;
        }

        private static string[] Sanatize(string[] lines)
        {
            return lines.Select(line => {
                var matches = Regex.Matches(line, @"(one|two|three|four|five|six|seven|eight|nine)");
                if (matches.Count > 0)
                {
                    line = line.Remove(matches[0].Index, matches[0].Length).Insert(matches[0].Index, NumberMapping[matches[0].Value]);
                }
                matches = Regex.Matches(line, @"(one|two|three|four|five|six|seven|eight|nine)");
                if (matches.Count > 0)
                {
                    line = line.Remove(matches.Last().Index, matches.Last().Length).Insert(matches.Last().Index, NumberMapping[matches.Last().Value]);
                }
                return line;
            }).ToArray();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 1: Trebuchet?! (https://adventofcode.com/2023/day/1)");

            string[] calibrations = File.ReadAllLines("input.txt");

            int resultOne = CalculateCalibration(calibrations);
            Console.WriteLine($"Part 1: The sum of all calibration values is: {resultOne}");

            int resultTwo = CalculateCalibration(Sanatize(calibrations));
            Console.WriteLine($"Part 2: The sum of all calibration values is: {resultTwo}");
        }
    }
}