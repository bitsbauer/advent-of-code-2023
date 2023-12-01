namespace Day01
{
    class Trebuchet
    {
        public static int CalculateCalibration(string[] lines)
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

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 1: Trebuchet?! (https://adventofcode.com/2023/day/1)");

            string[] calibrations = File.ReadAllLines("input.txt");

            int partOneResult = CalculateCalibration(calibrations);
            
            Console.WriteLine($"Part 1: The sum of all calibration values is: {partOneResult}");
        }
    }
}