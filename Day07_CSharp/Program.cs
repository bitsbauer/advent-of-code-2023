namespace CamelCards
{
    class Unknown
    {
        private static Tuple<List<string>,List<int[]>,List<int>> ParseInput(string[] lines, bool useJoker)
        {
            var originalHands = new List<string>();
            var hands = new List<int[]>();
            var bids  = new List<int>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                originalHands.Add(parts[0]);
                hands.Add(parts[0].Select(c => c == 'A' ? 14 : c == 'K' ? 13 : c == 'Q' ? 12 : c == 'J' ? (useJoker ? 1 : 11) : c == 'T' ? 10 : int.Parse(c.ToString())).ToArray());
                bids.Add(int.Parse(parts[1]));
            }

            return Tuple.Create(originalHands, hands, bids);
        }

        private static List<KeyValuePair<int,int>> EvaluateHand(int[] hand)
        {
            return [.. hand.GroupBy(x => x).Select(group => new KeyValuePair<int,int>(group.Key, group.Count())).OrderByDescending(x => x.Value).ThenByDescending(x => x.Key)];
        }

        // translate rules for hand strength to integer values for comparison
        private static int GetType(int[] hand, bool useJoker)
        {
            var evaluated = EvaluateHand(hand);
            switch (evaluated.Count) {
                case 5:
                    if (useJoker && evaluated[4].Key == 1) {
                        return 2;
                    }
                    return 1;
                case 4:
                    if (useJoker && (evaluated[0].Key == 1 || evaluated[3].Key == 1)) {
                        return 4;
                    }
                    return 2;
                case 3:
                    if (evaluated[0].Value == 2)
                    {
                        if (useJoker) {
                            if (evaluated[1].Key == 1) {
                                return 6;
                            }
                            if (evaluated[2].Key == 1) {
                                return 5;
                            }
                        }
                        return 3;
                    }
                    else
                    {
                        if (useJoker) {
                            if (evaluated[0].Key == 1) {
                                return 6;
                            }
                            if (evaluated[1].Key == 1) {
                                return 6;
                            }
                            if (evaluated[2].Key == 1) {
                                return 6;
                            }
                        }
                        return 4;
                    }
                case 2:
                    if (evaluated[0].Value == 3)
                    {
                        if (useJoker) {
                            if (evaluated[0].Key == 1) {
                                return 7;
                            }
                            if (evaluated[1].Key == 1) {
                                return 7;
                            }
                        }
                        return 5;
                    }
                    else
                    {
                        if (useJoker) {
                            if (evaluated[0].Key == 1) {
                                return 7;
                            }
                            if (evaluated[1].Key == 1) {
                                return 7;
                            }
                        }
                        return 6;
                    }
                case 1:
                    return 7;
                default:
                    return 0;
            }
        }

        private static int CompareHands(int[] hand1, int[] hand2)
        {
            foreach (var (card1, card2) in hand1.Zip(hand2))
            {
                if (card1 > card2)
                {
                    return 1;
                }
                else if (card1 < card2)
                {
                    return -1;
                }
            }

            return 0;
        }

        private static int Calculate(string[] lines, bool useJoker)
        {
            var (originalHands, hands, bids) = ParseInput(lines, useJoker);
            var types = hands.Select(hands => GetType(hands, useJoker)).ToList();

            var sortedHands = new List<Tuple<int[],int,int>>();
            foreach (var (hand, type, bid) in hands.Zip(types, bids))
            {
                sortedHands.Add(Tuple.Create(hand, type, bid));
            }
            sortedHands.Sort((x, y) => {
                var typeCompare = x.Item2.CompareTo(y.Item2);
                return typeCompare == 0 ? CompareHands(x.Item1, y.Item1) : typeCompare;
            });

            var result = 0;
            for (var i = 0; i < sortedHands.Count; i++)
            {
                var bid = sortedHands[i].Item3;
                result += bid * (i + 1);
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2023 - Day 7: Camel Cards (https://adventofcode.com/2023/day/7)");

            string[] input = File.ReadAllLines("input.txt");

            var resultOne = Calculate(input, false);
            Console.WriteLine($"Part 1: What are the total winnings: {resultOne}");

            var resultTwo = Calculate(input, true);
            Console.WriteLine($"Part 2: What are the new total winnings: {resultTwo}");
        }
    }
}