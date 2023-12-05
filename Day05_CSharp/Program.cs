namespace Day05
{
    class IfYouGiveASeedAFertilizer
    {
        private static Tuple<List<string>, List<long>, Dictionary<string, List<string>>> ParseInput(string[] lines)
        {
            var pipeline = new List<string>();
            var seeds = new List<long>();
            var maps = new Dictionary<string, List<string>>();
            
            string currentKey = "";
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) {
                    continue;
                }
                if (line.StartsWith("seeds: ")) { 
                    seeds = line.Replace("seeds: ", "").Split(" ").Select(long.Parse).ToList();
                }
                else if (!char.IsDigit(line[0]))
                {
                    currentKey = line.Replace(" map:", "");
                    pipeline.Add(currentKey);
                    maps[currentKey] = new List<string>();
                }
                else if (!string.IsNullOrEmpty(currentKey))
                {
                    maps[currentKey].Add(line);
                }
            }
            return new Tuple<List<string>, List<long>, Dictionary<string, List<string>>> (pipeline, [.. seeds], maps);
        }

        private static List<long[]> SortMap(List<string> map) 
        {
            var parsedMap = map.Select(line => line.Split(" ").Select(long.Parse).ToArray()).ToList();
            parsedMap.Sort((a, b) => a[1].CompareTo(b[1]));

            var result = new List<long[]>();
            long nextRangeStart = 0;
            foreach (var item in parsedMap)
            {
                if (item[1] != nextRangeStart) {
                   result.Add([nextRangeStart, 0]);
                }
                nextRangeStart = item[1]+item[2];
                result.Add([item[1], item[0]-item[1]]);
            }
            result.Add([nextRangeStart, 0]);
            return result;
        }

        private static long CalculatePartOne(string[] lines)
        {
            var parsedInput = ParseInput(lines);
            var pipeline = parsedInput.Item1;
            var seeds = parsedInput.Item2;
            var maps = parsedInput.Item3;

            var sorted =  pipeline.Select(step => SortMap(maps[step])).ToList();

            var lowestLocation = Int64.MaxValue;
            foreach(var seed in seeds) {
                var currentSeed = seed;
                foreach(var step in sorted) {
                    long lastDelta = 0;
                    foreach(var range in step) {   
                        if (currentSeed < range[0]) {
                            break;
                        }
                        lastDelta = range[1];
                    }
                    currentSeed += lastDelta;
                }
                if (currentSeed < lowestLocation) {
                    lowestLocation = currentSeed;
                }
            }
            return lowestLocation;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 5: If You Give A Seed A Fertilizer (https://adventofcode.com/2023/day/5)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Lowest location number: {resultOne}");
        }
    }
}