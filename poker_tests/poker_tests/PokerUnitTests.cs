using Poker;
using FluentAssertions;
using static Poker.PokerHands;

namespace poker_tests
{
    public class PokerUnitTests
    {
        [Fact]
        public void TestDeck()
        {
            var deck = PokerHands.CreateDeck();
            deck.Should().HaveCount(52);
            deck.Should().StartWith("2C");
            deck.Should().EndWith("AS");
            deck.Should().OnlyHaveUniqueItems();
            deck.Should().HaveElementAt(12, "AC");
        }

        [Fact]
        public void TestHand()
        {
            var deck = CreateDeck();
            var hand1 = Deal(deck);
            var hand2 = Deal(deck);

            // deal hand for player 1
            hand1.Should().HaveCount(5);

            // deal hand for player 2
            hand2.Should().HaveCount(5);
            
            // deal cards without replacement
            deck.Should().HaveCount(42);
        }

        [Fact]
        public void TestCardRankStringValue()
        {
            string card1 = "10D";
            var card1ValueString = GetCardRankStringValue(card1);
            string expectedCard1Value = "10";

            string card2 = "JH";
            var card2ValueString = GetCardRankStringValue(card2);
            string expectedCard2Value = "J";

            string card3 = "5D";
            var card3ValueString = GetCardRankStringValue(card3);
            string expectedCard3Value = "5";

            card1ValueString.Should().Be(expectedCard1Value);
            card2ValueString.Should().Be(expectedCard2Value);
            card3ValueString.Should().Be(expectedCard3Value);
        }

       [Fact]
        public void TestCardRankIntValue()
        {
            string card1 = "5";
            var card1ValueInt = GetCardRankIntValue(card1);
            int expectedCard1Value = 5;

            string card2 = "10";
            var card2ValueInt = GetCardRankIntValue(card2);
            int expectedCard2Value = 10;

            string card3 = "Q";
            var card3ValueInt = GetCardRankIntValue(card3);
            int expectedCard3Value = 12;

            card1ValueInt.Should().Be(expectedCard1Value);
            card2ValueInt.Should().Be(expectedCard2Value);
            card3ValueInt.Should().Be(expectedCard3Value);
        }

        [Fact]
        public void TestCardRankConvertIntToString()
        {
            int card1 = 5;
            var card1ValueString = GetCardRankConvertIntToString(card1);
            string expectedCard1Value = "5";

            int card2 = 10;
            var card2ValueString = GetCardRankConvertIntToString(card2);
            string expectedCard2Value = "10";

            int card3 = 11;
            var card3ValueString = GetCardRankConvertIntToString(card3);
            string expectedCard3Value = "Jack";

            card1ValueString.Should().Be(expectedCard1Value);
            card2ValueString.Should().Be(expectedCard2Value);
            card3ValueString.Should().Be(expectedCard3Value);
        }

        [Fact]
        public void TestCardRankSuit()
        {
            string card1 = "10H";
            var card1Suit = GetCardRankSuit(card1);
            string expectedCard1Suit = "H";

            string card2 = "2D";
            var card2Suit = GetCardRankSuit(card2);
            string expectedCard2Suit = "D";

            string card3 = "AC";
            var card3Suit = GetCardRankSuit(card3);
            string expectedCard3Suit = "C";

            card1Suit.Should().Be(expectedCard1Suit);
            card2Suit.Should().Be(expectedCard2Suit);
            card3Suit.Should().Be(expectedCard3Suit);
        }

        [Fact]
        public void TestHandOrdered()
        {
            var hand = new List<string>() { "5H", "2D", "AC", "QS", "10S" };

            var orderedHand = OrderHand(hand);

            int bestCardValue = orderedHand.ElementAt(0).Value;
            int expectedBestCardValue = 14;

            var bestCardSuit = orderedHand.ElementAt(0).Suit;
            var expectedBestCardSuit = "C";

            int worstCardValue = orderedHand.ElementAt(4).Value;
            int expectedWorstCardValue = 2;

            var worstCardSuit = orderedHand.ElementAt(4).Suit;
            var expectedWorstCardSuit = "D";

            bestCardValue.Should().Be(expectedBestCardValue);
            bestCardSuit.Should().Be(expectedBestCardSuit);
            worstCardValue.Should().Be(expectedWorstCardValue);
            worstCardSuit.Should().Be(expectedWorstCardSuit);
        }

        [Fact]
        public void TestFlush()
        {
            var hand1 = new List<string>() { "5H", "2D", "AC", "QS", "10S" };
            var hand2 = new List<string>() { "5H", "2H", "AH", "QH", "10H" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "5H", "2H", "AH", "QH", "10S" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);

            var isHand1Flush = IsFlush(orderedHand1);
            var isHand2Flush = IsFlush(orderedHand2);
            var isHand3Flush = IsFlush(orderedHand3);
            var isHand4Flush = IsFlush(orderedHand4);

            isHand1Flush.Should().BeFalse();
            isHand2Flush.Should().BeTrue();
            isHand3Flush.Should().BeFalse();
            isHand4Flush.Should().BeFalse();
        }

        [Fact]
        public void TestAnyFlush()
        {
            var hand1 = new List<string>() { "5H", "2D", "AC", "QS", "10S" };
            var hand2 = new List<string>() { "5H", "2H", "AH", "QH", "10H" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "5H", "2H", "AH", "QH", "10S" };
            var hand5 = new List<string>() { "10C", "AC", "3C", "4C", "JC" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);
            var orderedHand5 = OrderHand(hand5);

            var isDeal1Flush = IsAnyFlush(orderedHand1, orderedHand2);
            var isDeal2Flush = IsAnyFlush(orderedHand1, orderedHand4);
            var isDeal3Flush = IsAnyFlush(orderedHand2, orderedHand5);
            var isDeal4Flush = IsAnyFlush(orderedHand3, orderedHand4);

            isDeal1Flush.Should().BeTrue();
            isDeal2Flush.Should().BeFalse();
            isDeal3Flush.Should().BeTrue();
            isDeal4Flush.Should().BeFalse();
        }

        [Fact]
        public void TestFlushResultBlackWinsWhiteNoFlush()
        {
            var handBlack = new List<string>() { "5H", "2H", "AH", "QH", "10H" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "QH", "10C" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetFlushResult(orderedHandBlack, orderedHandWhite);
            
            var expectedResultOutput = "BLACK WINS... flush";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestFlushResultWhiteWinsBlackNoFlush()
        {
            var handBlack = new List<string>() { "5H", "2H", "AH", "QH", "10C" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "QH", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = PokerHands.GetFlushResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... flush";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestFlushResultTie()
        {
            var handBlack = new List<string>() { "5C", "2C", "AC", "QC", "10C" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "QH", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetFlushResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "TIE";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestFlushResultBlackHigherFlush()
        {
            var handBlack = new List<string>() { "5C", "2C", "4C", "10C", "QC" };
            var handWhite = new List<string>() { "5H", "2H", "4H", "10H", "JH" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetFlushResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... higher flush (Queen > Jack)";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestFlushResultWhiteHigherFlush()
        {
            var handBlack = new List<string>() { "5C", "2C", "4C", "10C", "QC" };
            var handWhite = new List<string>() { "5H", "2H", "4H", "10H", "AH" };

            var handBlack2 = new List<string>() { "5C", "2C", "4C", "8C", "9C" };
            var handWhite2 = new List<string>() { "5H", "2H", "4H", "8H", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);
            var orderedHandBlack2 = OrderHand(handBlack2);
            var orderedHandWhite2 = OrderHand(handWhite2);

            var result = GetFlushResult(orderedHandBlack, orderedHandWhite);
            var result2 = GetFlushResult(orderedHandBlack2, orderedHandWhite2);

            var expectedResultOutput = "WHITE WINS... higher flush (Ace > Queen)";
            var expectedResultOutput2 = "WHITE WINS... higher flush (10 > 9)";

            result.Should().Be(expectedResultOutput);
            result2.Should().Be(expectedResultOutput2);
        }

        [Fact]
        public void TestThreeOfAKind()
        {
            var hand1 = new List<string>() { "5H", "5D", "5C", "QS", "10S" };
            var hand2 = new List<string>() { "5H", "2H", "AH", "AC", "AS" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "10H", "10C", "5H", "QH", "10S" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);

            var isHand1ThreeOfAKind = IsThreeOfAKind(orderedHand1);
            var isHand2ThreeOfAKind = IsThreeOfAKind(orderedHand2);
            var isHand3ThreeOfAKind = IsThreeOfAKind(orderedHand3);
            var isHand4ThreeOfAKind = IsThreeOfAKind(orderedHand4);

            isHand1ThreeOfAKind.Should().BeTrue();
            isHand2ThreeOfAKind.Should().BeTrue();
            isHand3ThreeOfAKind.Should().BeFalse();
            isHand4ThreeOfAKind.Should().BeTrue();
        }

        [Fact]
        public void TestAnyThreeOfAKind()
        {
            var hand1 = new List<string>() { "AH", "AD", "AC", "2S", "10S" };
            var hand2 = new List<string>() { "AH", "2H", "2C", "QH", "2D" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "5H", "2H", "AH", "QH", "10S" };
            var hand5 = new List<string>() { "10C", "10H", "10S", "10D", "JC" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);
            var orderedHand5 = OrderHand(hand5);

            var isDeal1ThreeOfAKind = IsAnyThreeOfAKind(orderedHand1, orderedHand2);
            var isDeal2ThreeOfAKind = IsAnyThreeOfAKind(orderedHand1, orderedHand4);
            var isDeal3ThreeOfAKind = IsAnyThreeOfAKind(orderedHand2, orderedHand5);
            var isDeal4FThreeOfAKind = IsAnyThreeOfAKind(orderedHand3, orderedHand4);

            isDeal1ThreeOfAKind.Should().BeTrue();
            isDeal2ThreeOfAKind.Should().BeTrue();
            isDeal3ThreeOfAKind.Should().BeTrue();
            isDeal4FThreeOfAKind.Should().BeFalse();
        }

        [Fact]
        public void TestThreeOfAKindResultBlackWinsWhiteNoThreeOfAKind()
        {
            var handBlack = new List<string>() { "5H", "5C", "AH", "QH", "5S" };
            var handWhite = new List<string>() { "5D", "2H", "AC", "QC", "10C" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetThreeOfAKindResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... three of a kind";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestThreeOfAKindResultWhiteWinsBlackNoThreeOfAKind()
        {
            var handBlack = new List<string>() { "5H", "8C", "AH", "QH", "5S" };
            var handWhite = new List<string>() { "JD", "JH", "AC", "QC", "JS" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetThreeOfAKindResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... three of a kind";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestThreeOfAKindResultBlackHigherThreeOfAKind()
        {
            var handBlack = new List<string>() { "5C", "5H", "5S", "10C", "QC" };
            var handWhite = new List<string>() { "2H", "2S", "2C", "9H", "JH" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = PokerHands.GetThreeOfAKindResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... higher three of a kind (5 > 2)";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestThreeOfAKindResultWhiteHigherThreeOfAKind()
        {
            var handBlack = new List<string>() { "KC", "KS", "4C", "10C", "KD" };
            var handWhite = new List<string>() { "AH", "AD", "KH", "AS", "5H" };

            var handBlack2 = new List<string>() { "3C", "3D", "4C", "8C", "3S" };
            var handWhite2 = new List<string>() { "4H", "4S", "4D", "8H", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);
            var orderedHandBlack2 = OrderHand(handBlack2);
            var orderedHandWhite2 = OrderHand(handWhite2);

            var result = PokerHands.GetThreeOfAKindResult(orderedHandBlack, orderedHandWhite);
            var result2 = PokerHands.GetThreeOfAKindResult(orderedHandBlack2, orderedHandWhite2);

            var expectedResultOutput = "WHITE WINS... higher three of a kind (Ace > King)";
            var expectedResultOutput2 = "WHITE WINS... higher three of a kind (4 > 3)";

            result.Should().Be(expectedResultOutput);
            result2.Should().Be(expectedResultOutput2);
        }

        [Fact]
        public void TestReOrderTwoPairHand()
        {
            List<Card> orderedHand =
            [
                new Card{Suit= "C", Value=14},
                new Card{Suit= "D", Value=5},
                new Card{Suit= "S", Value=5},
                new Card{Suit= "H", Value=2},
                new Card{Suit= "C", Value=2}
            ];

            var reOrderedHand = OrderTwoPairsHand(orderedHand);

            var lastCard = reOrderedHand.ElementAt(4).Value;
            var expectedResultOutput = 14;

            lastCard.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestTwoPairs()
        {
            var hand1 = new List<string>() { "AH", "AD", "10C", "10S", "5S" };
            var hand2 = new List<string>() { "AH", "AD", "10H", "5H", "5D" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "AH", "5H", "5C", "2H", "2S" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);

            var isHand1TwoPairs = IsTwoPairs(orderedHand1);
            var isHand2TwoPairs = IsTwoPairs(orderedHand2);
            var isHand3TwoPairs = IsTwoPairs(orderedHand3);
            var isHand4TwoPairs = IsTwoPairs(orderedHand4);

            isHand1TwoPairs.Should().BeTrue();
            isHand2TwoPairs.Should().BeTrue();
            isHand3TwoPairs.Should().BeFalse();
            isHand4TwoPairs.Should().BeTrue();
        }

        [Fact]
        public void TestAnyTwoPairs()
        {
            var hand1 = new List<string>() { "AH", "AD", "10C", "10S", "5S" };
            var hand2 = new List<string>() { "AH", "AD", "10H", "5H", "5D" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "AH", "5H", "5C", "2H", "2S" };
            var hand5 = new List<string>() { "9C", "7H", "10H", "JH", "8S" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);
            var orderedHand5 = OrderHand(hand5);

            var isDeal1TwoPairs = IsAnyTwoPairs(orderedHand1, orderedHand3);
            var isDeal2TwoPairs = IsAnyTwoPairs(orderedHand3, orderedHand4);
            var isDeal3TwoPairs = IsAnyTwoPairs(orderedHand2, orderedHand5);
            var isDeal4TwoPairs = IsAnyTwoPairs(orderedHand3, orderedHand5);

            isDeal1TwoPairs.Should().BeTrue();
            isDeal2TwoPairs.Should().BeTrue();
            isDeal3TwoPairs.Should().BeTrue();
            isDeal4TwoPairs.Should().BeFalse();
        }

        [Fact]
        public void TestTwoPairsResultBlackWinsWhiteNoTwoPairs()
        {
            var handBlack = new List<string>() { "5H", "2H", "AH", "2D", "5S" };
            var handWhite = new List<string>() { "5C", "4H", "AC", "QH", "10C" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetTwoPairsResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... two pairs";

            result.Should().Be(expectedResultOutput);
        }
        
        [Fact]
        public void TestTwoPairsResultWhiteWinsBlackNoTwoPairs()
        {
            var handBlack = new List<string>() { "6H", "2H", "AH", "2D", "5S" };
            var handWhite = new List<string>() { "5C", "5H", "10C", "QH", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetTwoPairsResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... two pairs";

            result.Should().Be(expectedResultOutput);
        }

        
        [Fact]
        public void TestTwoPairsResultTie()
        {
            var handBlack = new List<string>() { "5C", "2C", "AC", "2S", "5D" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "2D", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetTwoPairsResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "TIE";

            result.Should().Be(expectedResultOutput);
        }
        
        [Fact]
        public void TestTwoPairsResultBlackHigherTwoPairs()
        {
            var handBlack = new List<string>() { "5C", "10C", "AC", "10S", "5D" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "2D", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetTwoPairsResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... higher two pairs (10 > 5)";

            result.Should().Be(expectedResultOutput);
        }
        
        [Fact]
        public void TestTwoPairsResultWhiteHigherTwoPairs()
        {
            var handBlack = new List<string>() { "5C", "10C", "AC", "10S", "5D" };
            var handWhite = new List<string>() { "5H", "AH", "AD", "2D", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetTwoPairsResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... higher two pairs (Ace > 10)";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestReOrderPairHand()
        {
            List<Card> orderedHand =
            [
                new Card{Suit= "C", Value=14},
                new Card{Suit= "D", Value=5},
                new Card{Suit= "S", Value=5},
                new Card{Suit= "H", Value=3},
                new Card{Suit= "C", Value=2}
            ];

            var reOrderedHand = OrderPairHand(orderedHand);

            var firstCard = reOrderedHand.ElementAt(0).Value;
            var secondCard = reOrderedHand.ElementAt(1).Value;
            var thirdCard = reOrderedHand.ElementAt(2).Value;
            var fourthCard = reOrderedHand.ElementAt(3).Value;
            var fifthCard = reOrderedHand.ElementAt(4).Value;
            var expectedResult1Output = 5;
            var expectedResult2Output = 5;
            var expectedResult3Output = 14;
            var expectedResult4Output = 3;
            var expectedResult5Output = 2;

            firstCard.Should().Be(expectedResult1Output);
            secondCard.Should().Be(expectedResult2Output);
            thirdCard.Should().Be(expectedResult3Output);
            fourthCard.Should().Be(expectedResult4Output);
            fifthCard.Should().Be(expectedResult5Output);
        }

        [Fact]
        public void TestAnyPair()
        {
            var hand1 = new List<string>() { "AH", "AD", "6C", "10S", "5S" };
            var hand2 = new List<string>() { "7H", "AD", "10H", "5H", "5D" };
            var hand3 = new List<string>() { "5C", "2H", "AH", "QH", "10H" };
            var hand4 = new List<string>() { "AH", "JH", "5C", "2H", "2S" };
            var hand5 = new List<string>() { "9C", "7H", "10H", "JH", "8S" };

            var orderedHand1 = OrderHand(hand1);
            var orderedHand2 = OrderHand(hand2);
            var orderedHand3 = OrderHand(hand3);
            var orderedHand4 = OrderHand(hand4);
            var orderedHand5 = OrderHand(hand5);

            var isDeal1Pair = IsAnyPair(orderedHand1, orderedHand3);
            var isDeal2Pair = IsAnyPair(orderedHand3, orderedHand4);
            var isDeal3Pair = IsAnyPair(orderedHand2, orderedHand5);
            var isDeal4Pair = IsAnyPair(orderedHand3, orderedHand5);

            isDeal1Pair.Should().BeTrue();
            isDeal2Pair.Should().BeTrue();
            isDeal3Pair.Should().BeTrue();
            isDeal4Pair.Should().BeFalse();
        }

        [Fact]
        public void TestPairResultBlackWinsWhiteNoPair()
        {
            var handBlack = new List<string>() { "8H", "2H", "AH", "2D", "5S" };
            var handWhite = new List<string>() { "5C", "4H", "AC", "QH", "10C" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetPairResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... pair";

            result.Should().Be(expectedResultOutput);
        }

        
        [Fact]
        public void TestPairResultWhiteWinsBlackNoPair()
        {
            var handBlack = new List<string>() { "6H", "2H", "AH", "JD", "5S" };
            var handWhite = new List<string>() { "5C", "6D", "10C", "QH", "10H" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetPairResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... pair";

            result.Should().Be(expectedResultOutput);
        }

        
        [Fact]
        public void TestPairResultTie()
        {
            var handBlack = new List<string>() { "5C", "2C", "AC", "2S", "7D" };
            var handWhite = new List<string>() { "5H", "2H", "AH", "2D", "7S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetPairResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "TIE";

            result.Should().Be(expectedResultOutput);
        }
        
        [Fact]
        public void TestPairResultBlackHigherPair()
        {
            var handBlack = new List<string>() { "6C", "10C", "AC", "10S", "5D" };
            var handWhite = new List<string>() { "5H", "9H", "AH", "2D", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetPairResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... higher pair (10 > 5)";

            result.Should().Be(expectedResultOutput);
        }
        
        [Fact]
        public void TestPairResultWhiteHigherPair()
        {
            var handBlack = new List<string>() { "JC", "10C", "KC", "JS", "5D" };
            var handWhite = new List<string>() { "JH", "AH", "7D", "JD", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetPairResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "WHITE WINS... higher pair (Ace > King)";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestHighCardResultBlackHigherCard()
        {
            var handBlack = new List<string>() { "JC", "10C", "KC", "JS", "AD" };
            var handWhite = new List<string>() { "JH", "KH", "7D", "2D", "5S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var result = GetHighCardResult(orderedHandBlack, orderedHandWhite);

            var expectedResultOutput = "BLACK WINS... high card: Ace";

            result.Should().Be(expectedResultOutput);
        }

        [Fact]
        public void TestHighCardResultWhiteHigherCard()
        {
            var handBlack = new List<string>() { "2C", "8C", "9C", "6S", "5D" };
            var handWhite = new List<string>() { "10H", "9H", "2D", "6D", "5S" };

            var handBlack2 = new List<string>() { "10C", "8C", "9C", "6S", "5D" };
            var handWhite2 = new List<string>() { "10H", "9H", "8D", "2D", "7S" };

            var orderedHandBlack = OrderHand(handBlack);
            var orderedHandWhite = OrderHand(handWhite);

            var orderedHandBlack2 = OrderHand(handBlack2);
            var orderedHandWhite2 = OrderHand(handWhite2);

            var result = GetHighCardResult(orderedHandBlack, orderedHandWhite);
            var result2 = GetHighCardResult(orderedHandBlack2, orderedHandWhite2);

            var expectedResultOutput = "WHITE WINS... high card: 10";
            var expectedResultOutput2 = "WHITE WINS... high card: 7";

            result.Should().Be(expectedResultOutput);
            result2.Should().Be(expectedResultOutput2);
        }
    }
}