using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = System.Tuple<DroPoker.Suit, DroPoker.Dignity>;

namespace DroPoker
{
    class Table
    {
        public int PlayerMoney = 1000;
        public int ComputerMoney = 1000;
        public Deck deck;
        public Card[] PlayerCard;
        public Card[] ComputerCard;
        public Computer Comp;
        public int[] PlayerBid;
        public int ComputerBid { get; set; }
        public int TradingCircle { get; set; }

        public Table()
        {
            deck = new Deck();
            PlayerCard = new Card[5];
            ComputerCard = new Card[5];
            PlayerBid = new int[3];
            Comp = new Computer();
            Comp.ComputerDiscard += () =>
            {
                PlayerMoney += (PlayerBid[0] + PlayerBid[1] + PlayerBid[2] + ComputerBid);
                GameFinish("Player(Comp discard)", ComputerBid);
            };
        }

        public void NewRound()
        {
            deck = new Deck();
            PlayerCard = new Card[5];
            ComputerCard = new Card[5];
            PlayerBid = new int[3];
            ComputerBid = 0;
            TradingCircle = 0;
        }

        public Combination AnanlizeCombination(Card[] hand)
        {
            if (Deck.IsRoyalFlush(hand)) return Combination.RoyalFlush;
            else if (Deck.IsStraightFlush(hand)) return Combination.StraightFlush;
            else if (Deck.IsSquare(hand)) return Combination.Square;
            else if (Deck.IsFullHause(hand)) return Combination.FullHause;
            else if (Deck.IsFlush(hand)) return Combination.Flush;
            else if (Deck.IsStreight(hand)) return Combination.Straight;
            else if (Deck.IsSet(hand)) return Combination.Set;
            else if (Deck.IsTwoPairs(hand)) return Combination.TwoPairs;
            else if (Deck.IsPair(hand)) return Combination.Pair;
            else return Combination.HighCard;
        }

        public void MovePlayer(int bid, List<int> cards)
        {
            PlayerBid[TradingCircle] = bid;
            PlayerMoney -= bid;
            if (Comp.GetReplyDiscard(this, bid)) return;
            TradingCircle++;
            if (TradingCircle == 1)
                for (var i = 0; i < 5; i++)
                {
                    PlayerCard[i] = deck.GetRandomCard();
                    ComputerCard[i] = deck.GetRandomCard();
                }
            if (TradingCircle == 2) ReplaceCard(cards);
            if (TradingCircle == 3)
            {
                if (AnanlizeCombination(PlayerCard) > AnanlizeCombination(ComputerCard))
                {
                    PlayerMoney += (PlayerBid[0] + PlayerBid[1] + PlayerBid[2] + ComputerBid);
                    GameFinish("Player", ComputerBid);
                }
                else if (AnanlizeCombination(PlayerCard) < AnanlizeCombination(ComputerCard))
                {
                    ComputerMoney += (PlayerBid[0] + PlayerBid[1] + PlayerBid[2] + ComputerBid);
                    GameFinish("Computer", PlayerBid[0] + PlayerBid[1] + PlayerBid[2]);
                }
                else
                {
                    PlayerMoney += ((PlayerBid[0] + PlayerBid[1] + PlayerBid[2] + ComputerBid)/2);
                    ComputerMoney += ((PlayerBid[0] + PlayerBid[1] + PlayerBid[2] + ComputerBid)/2);
                    GameFinish("Draw", 0);
                }
            }
        }

        public void ReplaceCard(List<int> cards)
        {
            foreach (var i in cards)
                PlayerCard[i] = deck.GetRandomCard();
            Comp.ReplaceCards(this);
        }

        public event Action<string, int> GameFinish;
    }
}
