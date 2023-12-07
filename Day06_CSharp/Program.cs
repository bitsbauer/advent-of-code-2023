using System.Text.RegularExpressions;

namespace Day06
{
    class WaitForIt
    {
        private static Tuple<List<int>,List<int>> ParseInput(string[] lines)
        {
            var times = Regex.Matches(lines[0], @"(\d+)").Select(m => int.Parse(m.Value)).ToList();
            var distances = Regex.Matches(lines[1], @"(\d+)").Select(m => int.Parse(m.Value)).ToList();
            return new Tuple<List<int>,List<int>>(times, distances);
        }

        private static int CalculatePartOne(string[] lines)
        {
            var (times, distances) = ParseInput(lines);

            var results = new List<int>();
            for (int i = 0; i < times.Count; i++)
            {
                var time = times[i];
                var distance = distances[i];

                // Formula for calculating the distance like x = (distance - time) * time leads to a quadratic equation with two solutions. 
                // The difference between the solutions is the number of ways to beat the record. -x^2 + tx - d = 0 with t = time and d = distance

                double sqh = Math.Sqrt(time * time - 4 * distance) / 2f;
                double th = time / 2f;

                var x1 = th - sqh;
                var x2 = th + sqh;

                if (x1 < x2) {
                    var ix2 = (int)Math.Ceiling(x2);
                    var ix1 = (int)Math.Floor(x1);

                    results.Add(ix2 - ix1 - 1);
                } else {
                    var ix1 = (int)Math.Ceiling(x1);
                    var ix2 = (int)Math.Floor(x2);

                    results.Add(ix1 - ix2 - 1);
                }

            }

            return results.Aggregate(1, (acc, val) => acc * val);
        }

        private static long CalculatePartTwo(string[] lines)
        {
            var (times, distances) = ParseInput(lines);

            var time = long.Parse(times.Select(t => t.ToString()).ToArray().Aggregate((acc, val) => acc + val));
            var distance = long.Parse(distances.Select(d => d.ToString()).ToArray().Aggregate((acc, val) => acc + val));

            var sqh = Math.Sqrt(time * time - 4 * distance) / 2d;
            var th = time / 2d;

            var x1 = th - sqh;
            var x2 = th + sqh;

            if (x1 < x2) {
                var ix2 = (long)Math.Ceiling(x2);
                var ix1 = (long)Math.Floor(x1);

                return ix2 - ix1 - 1;
            } else {
                var ix1 = (long)Math.Ceiling(x1);
                var ix2 = (long)Math.Floor(x2);

                return ix1 - ix2 - 1;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 6: Wait For It (https://adventofcode.com/2023/day/6)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Muliplied ways to beat the races: {resultOne}");

            var resultTwo = CalculatePartTwo(input);
            Console.WriteLine($"Part 2: Ways to beat record in much longer race: {resultTwo}");
        }
    }
}