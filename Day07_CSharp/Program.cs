namespace Day07
{
    class CamelCards
    {
        private static Tuple<List<int[]>,List<int>,List<int>> ParseInput(string[] lines, bool useJoker)
        {
            var hands = new List<int[]>();
            var types = new List<int>();
            var bids  = new List<int>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var hand = parts[0].Select(c => c == 'A' ? 14 : c == 'K' ? 13 : c == 'Q' ? 12 : c == 'J' ? (useJoker ? 1 : 11) : c == 'T' ? 10 : int.Parse(c.ToString())).ToArray();
                
                hands.Add(hand);
                types.Add(GetType(hand, useJoker));
                bids.Add(int.Parse(parts[1]));
            }

            return Tuple.Create(hands, types, bids);
        }

        // translate rules for hand strength to integer values for comparison
        private static int GetType(int[] hand, bool useJoker)
        {
            var e = EvaluateHand(hand);
            switch (e.Count) {
                case 5:
                    if (useJoker && e[4].Key == 1) {
                        return 2;
                    }
                    return 1;
                case 4:
                    if (useJoker && (e[0].Key == 1 || e[3].Key == 1)) {
                        return 4;
                    }
                    return 2;
                case 3:
                    if (e[0].Value == 2)
                    {
                        if (useJoker) {
                            if (e[1].Key == 1) {
                                return 6;
                            }
                            if (e[2].Key == 1) {
                                return 5;
                            }
                        }
                        return 3;
                    }
                    else
                    {
                        if (useJoker && (e[0].Key == 1 || e[1].Key == 1 || e[2].Key == 1)) {
                            return 6;
                        }
                        return 4;
                    }
                case 2:
                    if (e[0].Value == 3)
                    {
                        if (useJoker && (e[0].Key == 1 || e[1].Key == 1)) {
                            return 7;
                        }
                        return 5;
                    }
                    else
                    {
                        if (useJoker && (e[0].Key == 1 || e[1].Key == 1)) {
                            return 7;
                        }
                        return 6;
                    }
                case 1:
                    return 7;
                default:
                    return 0;
            }
        }

        // group cards in hand by value and count them, sort them by type and then value
        private static List<KeyValuePair<int,int>> EvaluateHand(int[] hand)
        {
            return [.. hand.GroupBy(x => x).Select(group => new KeyValuePair<int,int>(group.Key, group.Count())).OrderByDescending(x => x.Value).ThenByDescending(x => x.Key)];
        }

        // compare two hands card by card until a sort is possible
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
            var (hands, types, bids) = ParseInput(lines, useJoker);

            var sequence = hands.Zip(types, (hand, type) => (Hand: hand, Type: type))
                    .Zip(bids, (ht, bid) => (ht.Hand, ht.Type, Bid: bid))
                    .ToList();
            sequence.Sort((x, y) => {
                // compare hand types first, then compare hands
                var typeComparation = x.Type.CompareTo(y.Type);
                return typeComparation == 0 ? CompareHands(x.Hand, y.Hand) : typeComparation;
            });

            var result = 0;
            for (var i = 0; i < sequence.Count; i++)
            {
                var bid = sequence[i].Bid;
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