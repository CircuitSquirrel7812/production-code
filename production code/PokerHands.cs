namespace Poker;

public class PokerHands
{

    private readonly static List<string> _suits = ["C", "D", "H", "S"];
    private readonly static List<string> _cardValues = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"];

    static void Main()
    {
        var deck = CreateDeck();

        // deal cards to players white and black
        var black = Deal(deck);
        var white = Deal(deck);

        // order hands for both players
        var blackOrdered = OrderHand(black);
        var whiteOrdered = OrderHand(white);

        if (IsAnyFlush(blackOrdered, whiteOrdered)) 
        { 
            string flushResult = GetFlushResult(blackOrdered, whiteOrdered);
            Console.WriteLine(flushResult);
        }
        else if (IsAnyThreeOfAKind(blackOrdered, whiteOrdered))
        {
            string threeOfAKindResult = GetThreeOfAKindResult(blackOrdered, whiteOrdered);
            Console.WriteLine(threeOfAKindResult);
        }
        else if (IsAnyTwoPairs(blackOrdered, whiteOrdered))
        {
            string twoPairsResult = GetTwoPairsResult(blackOrdered, whiteOrdered);
            Console.WriteLine(twoPairsResult);
        }
        else if (IsAnyPair(blackOrdered, whiteOrdered))
        {
            string pairResult = GetPairResult(blackOrdered, whiteOrdered);
            Console.WriteLine(pairResult);
        }
        else 
        {
            string highCardResult = GetHighCardResult(blackOrdered, whiteOrdered);
            Console.WriteLine(highCardResult);
        }
    }

    public static List<string> CreateDeck()
    {
        var deck = new List<string>();
        for (int i = 0; i < _suits.Count; i++)
        {
            for (int j = 0; j < _cardValues.Count; j++)
            {
                deck.Add(_cardValues[j] + _suits[i]);
            }
        }
        return deck;
    }

    public static List<string> Deal(List<string> deck)
    {
        var hand = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            int randCardIndex = new Random().Next(0, deck.Count);
            hand.Add(deck[randCardIndex]);
            deck.RemoveAt(randCardIndex);
        }

        return hand;
    }

    public static List<Card> OrderHand(List<string> hand)
    {
        List<Card> handOrdered = [];
        for (int i = 0; i < hand.Count; i++)
        {
            var card = new Card
            {
                Suit = GetCardRankSuit(hand[i]),
                Value = GetCardRankIntValue(GetCardRankStringValue(hand[i]))
            };
            handOrdered.Add(card);
        }
        handOrdered = [.. handOrdered.OrderByDescending(c => c.Value)];
        return handOrdered;
    }

    // hand results flush

    public static bool IsFlush(List<Card> hand)
    {
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].Suit != hand[i + 1].Suit)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsAnyFlush(List<Card> handBlack, List<Card> handWhite)
    {
        return (IsFlush(handBlack) || IsFlush(handWhite));
    }

    public static string GetFlushResult(List<Card> handBlack, List<Card> handWhite)
    {
        if (IsFlush(handBlack) && !IsFlush(handWhite)){ return "BLACK WINS... flush"; }
        else if (!IsFlush(handBlack) && IsFlush(handWhite)){ return "WHITE WINS... flush"; }
        else
        {
            for (int i = 0; i < handBlack.Count; i++)
            {
                if (handBlack[i].Value > handWhite[i].Value) 
                {
                    return $"BLACK WINS... higher flush ({GetCardRankConvertIntToString(handBlack[i].Value)} > {GetCardRankConvertIntToString(handWhite[i].Value)})";
                }
                else if (handBlack[i].Value < handWhite[i].Value)
                {
                    return $"WHITE WINS... higher flush ({GetCardRankConvertIntToString(handWhite[i].Value)} > {GetCardRankConvertIntToString(handBlack[i].Value)})";
                }
            }
            return "TIE";
        }
    }

    // hand results 3 of a kind
    public static bool IsThreeOfAKind(List<Card> hand)
    {
        for (int i = 0; i < hand.Count - 2; i++)
        {
            if (hand[i].Value == hand[i + 1].Value && hand[i].Value == hand[i + 2].Value)
            {
                return true;
            }
        }
        return false;
    }

    public static int GetThreeOfAKindValue(List<Card> hand)
    {
        // used to get the value which makes three of a kind
        for (int i = 0; i < hand.Count - 2; i++)
        {
            if (hand[i].Value == hand[i + 1].Value && hand[i].Value == hand[i + 2].Value)
            {
                return hand[i].Value;
            }
        }

        return -1;
    }

    public static bool IsAnyThreeOfAKind(List<Card> handBlack, List<Card> handWhite)
    {
        return (IsThreeOfAKind(handBlack) || IsThreeOfAKind(handWhite));
    }

    public static string GetThreeOfAKindResult(List<Card> handBlack, List<Card> handWhite)
    {
        if (IsThreeOfAKind(handBlack) && !IsThreeOfAKind(handWhite)) { return "BLACK WINS... three of a kind"; }
        else if (!IsThreeOfAKind(handBlack) && IsThreeOfAKind(handWhite)) { return "WHITE WINS... three of a kind"; }
        else
        {
            var threeOfAKindValueBlack = GetThreeOfAKindValue(handBlack);
            var threeOfAKindValueWhite = GetThreeOfAKindValue(handWhite);
            if (threeOfAKindValueBlack > threeOfAKindValueWhite)
            {
                return $"BLACK WINS... higher three of a kind ({GetCardRankConvertIntToString(threeOfAKindValueBlack)} > {GetCardRankConvertIntToString(threeOfAKindValueWhite)})";
            }
            else { return $"WHITE WINS... higher three of a kind ({GetCardRankConvertIntToString(threeOfAKindValueWhite)} > {GetCardRankConvertIntToString(threeOfAKindValueBlack)})"; }
        }
    }

    // hand results two pairs
    public static bool IsTwoPairs(List<Card> hand)
    {
        int pairCount = 0;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].Value == hand[i + 1].Value)
            {
                pairCount++;
            }
        }
        return pairCount == 2;
    }

    public static List<Card> OrderTwoPairsHand(List<Card> hand)
    {
        // order two pairs hand so that highest pair at beginning followed by next highest pair then non pair card

        // add only paired cards to list
        var reOrderedHand = new List<Card>();
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].Value == hand[i + 1].Value)
            {
                reOrderedHand.Add(hand[i]);
                reOrderedHand.Add(hand[i + 1]);
            }
        }

        // add non pair card to list
        for (int i = 0; i < hand.Count; i++)
        {
            var nonPairCard = reOrderedHand.FirstOrDefault(c => c.Value == hand[i].Value);
            if (nonPairCard == null)
            {
                reOrderedHand.Add(hand[i]);
                return reOrderedHand;
            }
        }
        return reOrderedHand;
    }

    public static bool IsAnyTwoPairs(List<Card> handBlack, List<Card> handWhite)
    {
        return (IsTwoPairs(handBlack) || IsTwoPairs(handWhite));
    }

    public static string GetTwoPairsResult(List<Card> handBlack, List<Card> handWhite)
    {
        if (IsTwoPairs(handBlack) && !IsTwoPairs(handWhite)) { return "BLACK WINS... two pairs"; }
        else if (!IsTwoPairs(handBlack) && IsTwoPairs(handWhite)) { return "WHITE WINS... two pairs"; }
        else
        {
            var reOrderedBlackHand = OrderTwoPairsHand(handBlack);
            var reOrderedWhiteHand = OrderTwoPairsHand(handWhite);
            for (int i = 0; i < handBlack.Count; i++)
            {
                if (reOrderedBlackHand[i].Value > reOrderedWhiteHand[i].Value)
                {
                    return $"BLACK WINS... higher two pairs ({GetCardRankConvertIntToString(reOrderedBlackHand[i].Value)} > {GetCardRankConvertIntToString(reOrderedWhiteHand[i].Value)})";
                }
                else if (handBlack[i].Value < handWhite[i].Value)
                {
                    return $"WHITE WINS... higher two pairs ({GetCardRankConvertIntToString(reOrderedWhiteHand[i].Value)} > {GetCardRankConvertIntToString(reOrderedBlackHand[i].Value)})";
                }
            }
            return "TIE";
        }
    }

    // hand results pair
    public static bool IsPair(List<Card> hand)
    {
        int pairCount = 0;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].Value == hand[i + 1].Value)
            {
                pairCount++;
            }
        }
        return pairCount == 1;
    }

    public static List<Card> OrderPairHand(List<Card> hand)
    {
        // order pairs hand so that pair at beginning followed by highest to lowest non pair cards

        // add only paired cards to list
        var reOrderedHand = new List<Card>();
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].Value == hand[i + 1].Value)
            {
                reOrderedHand.Add(hand[i]);
                reOrderedHand.Add(hand[i + 1]);
            }
        }

        // add non pair card to list
        for (int i = 0; i < hand.Count; i++)
        {
            var nonPairCard = reOrderedHand.FirstOrDefault(c => c.Value == hand[i].Value);
            if (nonPairCard == null)
            {
                reOrderedHand.Add(hand[i]);
            }
        }
        return reOrderedHand;
    }

    public static bool IsAnyPair(List<Card> handBlack, List<Card> handWhite)
    {
        return (IsPair(handBlack) || IsPair(handWhite));
    }

    public static string GetPairResult(List<Card> handBlack, List<Card> handWhite)
    {
        if (IsPair(handBlack) && !IsPair(handWhite)) { return "BLACK WINS... pair"; }
        else if (!IsPair(handBlack) && IsPair(handWhite)) { return "WHITE WINS... pair"; }
        else
        {
            var reOrderedBlackHand = OrderPairHand(handBlack);
            var reOrderedWhiteHand = OrderPairHand(handWhite);
            for (int i = 0; i < handBlack.Count; i++)
            {
                if (reOrderedBlackHand[i].Value > reOrderedWhiteHand[i].Value)
                {
                    return $"BLACK WINS... higher pair ({GetCardRankConvertIntToString(reOrderedBlackHand[i].Value)} > {GetCardRankConvertIntToString(reOrderedWhiteHand[i].Value)})";
                }
                else if (reOrderedBlackHand[i].Value < reOrderedWhiteHand[i].Value)
                {
                    return $"WHITE WINS... higher pair ({GetCardRankConvertIntToString(reOrderedWhiteHand[i].Value)} > {GetCardRankConvertIntToString(reOrderedBlackHand[i].Value)})";
                }
            }
            return "TIE";
        }
    }

    public static string GetHighCardResult(List<Card> handBlack, List<Card> handWhite)
    {
        for (int i = 0; i < handBlack.Count; i++)
        {
            if (handBlack[i].Value > handWhite[i].Value)
            {
                return $"BLACK WINS... high card: {GetCardRankConvertIntToString(handBlack[i].Value)}";
            }
            else if (handBlack[i].Value < handWhite[i].Value)
            {
                return $"WHITE WINS... high card: {GetCardRankConvertIntToString(handWhite[i].Value)}";
            }
        }
        return "TIE";
    }


    // helper methods for converting extracting string and int values from cards
    public static string GetCardRankStringValue(string card)
    {
        // gets the value of the card as string such as A, 10, K etc.
        return card[..^1];
    }

    public static int GetCardRankIntValue(string cardValue)
    {
        // gets the value of the card as int such as 'A' => 14 etc.
        return cardValue switch
        {
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => 14,
            _ => int.Parse(cardValue),
        };
    }

    public static string GetCardRankSuit(string card)
    {
        // gets the suit of the card
        return card[^1..];
    }

    public static string GetCardRankConvertIntToString(int cardValue)
    {
        // gets the value of the card as string such as 14 => 'Ace' etc.
        return cardValue switch
        {
            11 => "Jack",
            12 => "Queen",
            13 => "King",
            14 => "Ace",
            _ => cardValue.ToString(),
        };
    }



    public class Card
    {
        public int Value { get; set; }
        public string? Suit { get; set; }
    }
}