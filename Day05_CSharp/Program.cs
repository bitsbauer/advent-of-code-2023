namespace Day05
{
    class IfYouGiveASeedAFertilizer
    {
        private static Tuple<List<List<long[]>>, List<long>> ParseInput(string[] lines)
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
            var sorted = pipeline.Select(step => SortMap(maps[step])).ToList();

            return new Tuple<List<List<long[]>>, List<long>> (sorted, [.. seeds]);
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

        private static long FindLowestLocation(List<List<long[]>> sorted, List<long> seeds)
        {
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

        private static long CalculatePartOne(string[] lines)
        {
            var parsed = ParseInput(lines);
            return FindLowestLocation(parsed.Item1, parsed.Item2);
        }

        private static long CalculatePartTwo(string[] lines)
        {
            var parsed = ParseInput(lines);

            var seeds = new List<long>();
            for (int i = 0; i < parsed.Item2.Count; i += 2)
            {
                for(long j = parsed.Item2[i]; j < parsed.Item2[i] + parsed.Item2[i+1]; j++) {
                    seeds.Add(j);
                }
            }

            return FindLowestLocation(parsed.Item1, seeds);
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 5: If You Give A Seed A Fertilizer (https://adventofcode.com/2023/day/5)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: Lowest location number: {resultOne}");
            
            var resultTwo = CalculatePartTwo(input);
            Console.WriteLine($"Part 2: Lowest location number expanded seeds: {resultTwo}");
        }
    }
}