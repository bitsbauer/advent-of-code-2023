using System.Text.RegularExpressions;

namespace Day02
{
    class CubeConundrum
    {
        private static int CalculatePartOne(string[] lines)
        {
            int possibleGames = 0;

            int gameId = 0;
            foreach (var game in lines)
            {
                gameId++;

                int maxRed = 0;
                int maxGreen = 0;
                int maxBlue = 0;

                var matches = Regex.Matches(game, @"(\d+) (red|green|blue)");

                foreach (Match match in matches)
                {
                    var value = int.Parse(match.Groups[1].Value);
                    var color = match.Groups[2].Value;

                    switch (color)
                    {
                        case "red":
                            if (value > maxRed)
                            {
                                maxRed = value;
                            }
                            break;
                        case "green":
                            if (value > maxGreen)
                            {
                                maxGreen = value;
                            }
                            break;
                        case "blue":
                            if (value > maxBlue)
                            {
                                maxBlue = value;
                            }
                            break;
                    }
                }

                if (maxRed < 13 && maxGreen  < 14 && maxBlue < 15) {
                    possibleGames += gameId;
                }  
            }

            return possibleGames;
        }
        

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 2: Cube Conundrum (https://adventofcode.com/2023/day/2)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Sum of IDs of possible games: {resultOne}");
        }
    }
}