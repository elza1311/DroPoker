using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = System.Tuple<DroPoker.Suit, DroPoker.Dignity>;

namespace DroPoker
{
    class Deck
    {
        private LinkedList<Card> DeckOfCard;

        public Deck() => DeckOfCard = CreateDeck();

        private LinkedList<Card> CreateDeck()
        {
            var deck = new LinkedList<Card>();
            var rdm = new Random();
            var nxtSuit = rdm.Next(0, 4);
            var nxtDignity = rdm.Next(0, 13);
            for (var i = 0; i < 52; i++)
            {
                var card = Tuple.Create((Suit)nxtSuit, (Dignity)nxtDignity);
                if (deck.Contains(card))
                {
                    i--;
                    nxtSuit = rdm.Next(0, 4);
                    nxtDignity = rdm.Next(0, 13);
                    continue;
                }
                deck.AddLast(card);
                nxtSuit = rdm.Next(0, 4);
                nxtDignity = rdm.Next(0, 13);
            }
            return deck;
        }

        public Card GetRandomCard()
        {
            var random = new Random();
            var numberOfCard = random.Next(0, DeckOfCard.Count);
            var card = DeckOfCard.ElementAt(numberOfCard);
            DeckOfCard.Remove(card);
            return card;
        }

        static public bool IsPair(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            return (sortHand[0].Item2 == sortHand[1].Item2)
                || (sortHand[1].Item2 == sortHand[2].Item2)
                || (sortHand[2].Item2 == sortHand[3].Item2)
                || (sortHand[3].Item2 == sortHand[4].Item2);
        }

        static public bool IsTwoPairs(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            return (sortHand[0].Item2 == sortHand[1].Item2 && sortHand[2].Item2 == sortHand[3].Item2)
                || (sortHand[0].Item2 == sortHand[1].Item2 && sortHand[3].Item2 == sortHand[4].Item2)
                || (sortHand[1].Item2 == sortHand[2].Item2 && sortHand[3].Item2 == sortHand[4].Item2);
        }

        static public bool IsSet(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            return (sortHand[0].Item2 == sortHand[1].Item2 && sortHand[1].Item2 == sortHand[2].Item2) 
                || (sortHand[1].Item2 == sortHand[2].Item2 && sortHand[2].Item2 == sortHand[3].Item2) 
                || (sortHand[2].Item2 == sortHand[3].Item2 && sortHand[3].Item2 == sortHand[4].Item2);
        }

        static public bool IsStreight(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            var firstCard = (int)sortHand[0].Item2;
            for (var i = 1; i < 5; i++)
                if (sortHand[i].Item2 != (Dignity)(firstCard + i)) return false;
            return true;
        }

        static public bool IsFlush(Card[] hand)
        {
            var suit = hand[0].Item1;
            for (var i = 1; i < 5; i++)
                if (hand[i].Item1 != suit) return false;
            return true;
        }

        static public bool IsFullHause(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            return (sortHand[0].Item2 == sortHand[1].Item2 && sortHand[2].Item2 == sortHand[3].Item2 && sortHand[3].Item2 == sortHand[4].Item2)
                || (sortHand[0].Item2 == sortHand[1].Item2 && sortHand[1].Item2 == sortHand[2].Item2 && sortHand[3].Item2 == sortHand[4].Item2);
        }

        static public bool IsSquare(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            var firstCard = sortHand[0].Item2;
            var secondCard = sortHand[1].Item2;
            for (var i = 2; i < 5; i++)
                if (sortHand[i].Item2 != firstCard && sortHand[i].Item2 != secondCard) return false;
            return true;
        }

        static public bool IsStraightFlush(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            var suit = sortHand[0].Item1;
            var firstCard = (int)sortHand[0].Item2;
            for (var i = 0; i < 5; i++)
                if (sortHand[i].Item2 != (Dignity)(firstCard + i) || sortHand[i].Item1 != suit) return false;
            return true;
        }

        static public bool IsRoyalFlush(Card[] hand)
        {
            var sortHand = hand.OrderBy(x => x.Item2).ToArray();
            var suit = sortHand[0].Item1;
            for (var i = 0; i < 5; i++)
                if (sortHand[i].Item2 != (Dignity)(8 + i) || sortHand[i].Item1 != suit) return false;
            return true;
        }
    }

    enum Suit
    {
        Heart,
        Diamond,
        Club,
        Spade
    }
    
    enum Dignity
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
    }
}
