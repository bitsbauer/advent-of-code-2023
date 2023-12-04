using System.Text.RegularExpressions;

namespace Day04
{
    class Scratchcards
    {
        private static string[][] SplitLines(string[] lines)
        {
            return lines.Select(SplitLine).ToArray();
        }

        private static string[] SplitLine(string line)
        {
            var parts = line.Split(':');
            var numbers = parts[1].Split('|');
            var winning = Regex.Replace(numbers[0].Trim(), @"\s+", "|");
            var given = numbers[1];
            return [winning, given];
        }

        private static MatchCollection GetMatches(string[] splitLine)
        {
            var winning = splitLine[0];
            var given = splitLine[1];
            return Regex.Matches(given, $@"(?:^|\s)({winning})(?=\s|$)");
        }

        private static int RecursiveScratchcardCount(string[] lines, int[] idxs, int count)
        {   
            foreach (int idx in idxs) {
                var splitLine = SplitLine(lines[idx]);
                var matchesCount = GetMatches(splitLine).Count;
                if (matchesCount > 0) {
                    int[] newIdxs = Enumerable.Range(idx + 1, matchesCount).ToArray();
                    count += RecursiveScratchcardCount(lines, newIdxs, 1);
                } else {
                    count += 1;
                }
            }
            return count;
        }

        private static int CalculatePartOne(string[] lines)
        {   
            var worth = 0;
            foreach (string[] splitLine in SplitLines(lines)) {
                var exponent = GetMatches(splitLine).Count - 1;
                if (exponent > -1) {
                    worth += (int)Math.Pow(2, exponent);
                }
            }
            return worth;
        }

        private static int CalculatePartTwo(string[] lines)
        {   
            
            Queue<int> queue = new Queue<int>(Enumerable.Range(0, lines.Length));
            int[] matchCounts = lines.Select(line => GetMatches(SplitLine(line)).Count).ToArray();

            var total = 0;
            while (queue.Count > 0) {
                int idx = queue.Dequeue();
                total += 1;
                if (matchCounts[idx] > 0) {
                    int[] newIdxs = Enumerable.Range(idx + 1, matchCounts[idx]).ToArray();
                    foreach (int newIdx in newIdxs) {
                        queue.Enqueue(newIdx);
                    }
                }
            }
            return total;

            // LEGACY SOLUTION (RECURSIVE) - TOO SLOW
            // return RecursiveScratchcardCount(lines, Enumerable.Range(0, lines.Length).ToArray(), 0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 4: Scratchcards (https://adventofcode.com/2023/day/4)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Points worth total: {resultOne}");

            var resultTwo = CalculatePartTwo(input);
            Console.WriteLine($"Part 1: Total count of scratchcards: {resultTwo}");
        }
    }
}   