using System.Text.RegularExpressions;

namespace Day04
{
    class Scratchcards
    {
        private static int CalculatePartOne(string[] lines)
        {   
            string[][] splitLines = lines.Select(line => {
                var parts = line.Split(':');
                var numbers = parts[1].Split('|');
                var winning = numbers[0];
                var given = numbers[1];
                return new[] { winning, given };
            }).ToArray();

            var worth = 0;

            foreach (string[] splitLine in splitLines) {
                var winning = Regex.Replace(splitLine[0].Trim(), @"\s+", "|");
                var matches = Regex.Matches(splitLine[1], $@"(?:^|\s)({winning})(?=\s|$)");
                var exponent = matches.Count - 1;
                if (exponent > -1) {
                    worth += (int)Math.Pow(2, exponent);
                }
            }

            return worth;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 4: Scratchcards (https://adventofcode.com/2023/day/4)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Points worth total: {resultOne}");
        }
    }
}   