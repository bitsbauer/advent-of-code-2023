using System.Text.RegularExpressions;

namespace Day03
{
    class GearRatios
    {
        private static int CalculatePartOne(string[] lines)
        {
            // verify holds the positions which define part-numbers
            var lineLength = lines[0].Length;
            bool[,] verify = new bool[lineLength, lines.Length + 2];

            var lineIndex = 0;
            foreach (var line in lines)
            {
                lineIndex++;
                var matches = Regex.Matches(line, @"([^\.0-9])");
                foreach (Match match in matches)
                {
                    var index = match.Index;
                    for (var x = -1; x < 2; x++)
                    {
                        for (var y = -1; y < 2; y++)
                        {
                            var xPos = index + x;
                            var yPos = lineIndex + y;
                            if ( xPos < 0 || xPos >= lineLength) {
                                continue;
                            }
                            verify[xPos, yPos] = true;
                        }
                    }
                }
            }

            var partNumberSum = 0;  

            lineIndex = 0;
            foreach (var line in lines)
            {
                lineIndex++;
                var matches = Regex.Matches(line, @"([0-9]+)");
                foreach (Match match in matches)
                {
                    for (var x = match.Index; x < match.Index + match.Length; x++)
                    {
                        if (verify[x, lineIndex])
                        {
                            partNumberSum += int.Parse(match.Value);
                            break;
                        }
                    }
                }
            }

            return partNumberSum;
        }

        private static int CalculatePartTwo(string[] lines)
        {
            // verify holds the positions which define part-numbers
            var lineLength = lines[0].Length;
            int[,] verify = new int[lineLength, lines.Length + 2];

            var lineIndex = 0;
            var gearIndex = 1;
            foreach (var line in lines)
            {
                lineIndex++;
                var matches = Regex.Matches(line, @"([*])");
                foreach (Match match in matches)
                {
                    var index = match.Index;
                    for (var x = -1; x < 2; x++)
                    {
                        for (var y = -1; y < 2; y++)
                        {
                            var xPos = index + x;
                            var yPos = lineIndex + y;
                            if ( xPos < 0 || xPos >= lineLength) {
                                continue;
                            }
                            verify[xPos, yPos] = gearIndex;
                        }
                    }
                    gearIndex++;
                }
            }

            int[,] gearTrack= new int[gearIndex, 2];

            lineIndex = 0;
            foreach (var line in lines)
            {
                lineIndex++;
                var matches = Regex.Matches(line, @"([0-9]+)");
                foreach (Match match in matches)
                {
                    for (var x = match.Index; x < match.Index + match.Length; x++)
                    {
                        if (verify[x, lineIndex] > 0)
                        {
                            var gear = verify[x, lineIndex];
                            gearTrack[gear, 0]++;
                            gearTrack[gear, 1] = gearTrack[gear, 1] > 0 ? gearTrack[gear, 1] * int.Parse(match.Value) : int.Parse(match.Value);
                            break;
                        }
                    }
                }
            }

            var sumGearRatio = 0;
            for (var i = 1; i < gearIndex; i++)
            {
                if (gearTrack[i, 0] == 2)
                {
                    sumGearRatio += gearTrack[i, 1];
                }
            }

            return sumGearRatio;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 3: Gear Ratios (https://adventofcode.com/2023/day/3)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Sum of all the part numbers: {resultOne}");

            var resultTwo = CalculatePartTwo(input);
            Console.WriteLine($"Part 1: Sum of all the part numbers: {resultTwo}");
        }
    }
}