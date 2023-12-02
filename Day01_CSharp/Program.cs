namespace Day01
{
    class Trebuchet
    {
        private static int CalculatePartOne(string[] lines)
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

        private static int CalculatePartTwo(string[] lines)
        {
            Dictionary<string, int> mapping = new()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };
            for (int i = 1; i < 10; i++)
            {
                mapping.Add(i.ToString(), i);
            }

            int sum = 0;
            foreach (var line in lines)
            {
                var firstIndex = line.Length;
                var firstValue = 0;
                var secondIndex = -1;
                var secondValue = 0;
                foreach (var pair in mapping)
                {
                    var index = line.IndexOf(pair.Key);
                    if (index != -1 && index < firstIndex)
                    {
                        firstIndex = index;
                        firstValue = pair.Value;
                    }
                    index = line.LastIndexOf(pair.Key);
                    if (index != -1 && index > secondIndex)
                    {
                        secondIndex = index;
                        secondValue = pair.Value;
                    }
                }
                sum += firstValue * 10 + secondValue;
            }
            return sum;
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 1: Trebuchet?! (https://adventofcode.com/2023/day/1)");

            string[] calibrations = File.ReadAllLines("input.txt");

            int resultOne = CalculatePartOne(calibrations);
            Console.WriteLine($"Part 1: The sum of all calibration values is: {resultOne}");

            int resultTwo = CalculatePartTwo(calibrations);
            Console.WriteLine($"Part 2: The sum of all calibration values is: {resultTwo}");
        }
    }
}