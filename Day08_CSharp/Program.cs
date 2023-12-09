using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Day08
{
    internal class Node(string name)
    {
        public string Name { get; set; } = name;
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public bool IsEndNode { get; set; } = name.EndsWith('Z');
    }

    class HauntedWasteland
    {
        private static Tuple<char[],Dictionary<string, Node>> ParseInput(string[] lines) {
            
            var instructions = lines[0].Select(c => c).ToArray();
            var nodes = new Dictionary<string, Node>();
        
            var name = new List<string>();
            var left = new List<string>();
            var right = new List<string>();

            for(int i = 2; i < lines.Length; i++) {
                var matches = Regex.Matches(lines[i], @"[A-Z]{3}");
                name.Add(matches[0].Value);
                left.Add(matches[1].Value);
                right.Add(matches[2].Value);
            }

            foreach(var n in name) {
                nodes.Add(n, new Node(n));
            }
            foreach(var (First, Second, Third) in name.Zip(left, right)) {
               nodes[First].Left = nodes[Second];
               nodes[First].Right = nodes[Third];
            }

            return new Tuple<char[],Dictionary<string, Node>>(instructions, nodes);
        }

        private static int Traverse(Node start, char[] instructions, string endsWith)
        {
            Node current = start;
            var instructionRuns = 0;
            var steps = 0;

            while (true) {
                instructionRuns++;
                foreach (char instruction in instructions) {
                    current = instruction == 'L' ? current.Left! : current.Right!;
                    steps++;
                    if (current.Name.EndsWith(endsWith)) {
                        return steps;
                    }
                }
                if (instructionRuns > 1000) {
                    return -1;
                }
            }
        }

        // https://stackoverflow.com/questions/239865/best-way-to-find-all-factors-of-a-given-number
        private static List<int> Factor(int number)
        {
            var factors = new List<int>();
            int max = (int)Math.Sqrt(number);  // Round down

            for (int factor = 1; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
            {
                if (number % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != number / factor) // Don't add the square root twice!  Thanks Jon
                        factors.Add(number / factor);
                }
            }
            return factors;
        }

        private static int CalculatePartOne(string[] lines)
        {
            var (instructions, nodes) = ParseInput(lines);
            return Traverse(nodes["AAA"], instructions, "ZZZ");
        }

        private static long CalculatePartTwo(string[] lines)
        {
            var (instructions, nodes) = ParseInput(lines);
            return nodes.Where(x => x.Key.EndsWith('A'))
                .Select(x => Traverse(x.Value,instructions, "Z"))
                .SelectMany(x => Factor(x).Where(f => x != f) // remove the traverse result itself
                .Select(x => (long)x)) // cast to long to not overflow
                .Distinct() // remove duplicates
                .Aggregate((x, y) => x * y); // multiply all
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 8: Haunted Wasteland (https://adventofcode.com/2023/day/8)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = CalculatePartOne(input);
            Console.WriteLine($"Part 1: : How many steps are required to reach ZZZ? {resultOne}");

            var resultTwo = CalculatePartTwo(input);
            Console.WriteLine($"Part 2: : How many steps does it take before you're only on nodes that end with Z? {resultTwo}");
        }
    }
}