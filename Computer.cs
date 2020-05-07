using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = System.Tuple<DroPoker.Suit, DroPoker.Dignity>;

namespace DroPoker
{
    class Computer
    {
        public bool GetReplyDiscard(Table table, int bid)
        {
            if (table.TradingCircle == 0)
            {
                table.ComputerBid += bid;
                table.ComputerMoney -= bid;
                return false;
            }
            else if (table.TradingCircle == 2)
            {
                if (table.AnanlizeCombination(table.ComputerCard) == Combination.HighCard && bid > table.PlayerBid[1])
                {
                    ComputerDiscard();
                    return true;
                }
                else if (table.AnanlizeCombination(table.ComputerCard) == Combination.RoyalFlush)
                {
                    table.ComputerBid += bid;
                    table.ComputerMoney -= bid;
                    return false;
                }
                else
                {
                    if (table.PlayerBid[1] != 0 && (int)table.AnanlizeCombination(table.ComputerCard) * 1.1 < bid / table.PlayerBid[1])
                    {
                        ComputerDiscard();
                        return true;
                    }
                    else
                    {
                        table.ComputerBid += bid;
                        table.ComputerMoney -= bid;
                        return false;
                    }
                }
            }
            else
            {
                if (table.AnanlizeCombination(table.ComputerCard) == Combination.HighCard && bid > table.PlayerBid[0])
                {
                    ComputerDiscard();
                    return true;
                }
                else if (table.AnanlizeCombination(table.ComputerCard) == Combination.RoyalFlush)
                {
                    table.ComputerBid += bid;
                    table.ComputerMoney -= bid;
                    return false;
                }
                else
                {
                    if (table.PlayerBid[0] != 0 && (int)table.AnanlizeCombination(table.ComputerCard) * 1.1 + 1 < bid / table.PlayerBid[0])
                    {
                        ComputerDiscard();
                        return true;
                    }
                    else
                    {
                        table.ComputerBid += bid;
                        table.ComputerMoney -= bid;
                        return false;
                    }
                }
            }
        }

        public void ReplaceCards(Table table)
        {
            if (table.AnanlizeCombination(table.ComputerCard) < Combination.Set)
            {
                var Levenstein = new List<Tuple<Card[], int>>();
                Levenstein.Add(LevensteinDistance(table.ComputerCard.OrderBy(x => x.Item2).ToArray(), CombinationArrayClass.RoyalFlush));
                Levenstein.Add(LevensteinDistance(table.ComputerCard.OrderBy(x => x.Item2).ToArray(), CombinationArrayClass.StraightFlush));
                if (Levenstein[0].Item2 > Levenstein[1].Item2)
                {
                    for (var i = 0; i < 5; i++)
                        if (table.ComputerCard[i] != Levenstein[1].Item1[i])
                            table.ComputerCard[i] = table.deck.GetRandomCard();
                }
                else
                {
                    for (var i = 0; i < 5; i++)
                        if (table.ComputerCard[i] != Levenstein[0].Item1[i])
                            table.ComputerCard[i] = table.deck.GetRandomCard();
                }
            }
        }
       
        public event Action ComputerDiscard;

        public Tuple<Card[], int> LevensteinDistance(Card[] handOfComp, Card[][] waitedHands)
        {
            var maxLevDist = 6;
            var result = Tuple.Create(new Card[0], 0);
            foreach (var hand in waitedHands)
            {
                var opt = new double[2, 6];
                for (var j = 0; j <= 5; ++j) opt[0, j] = 0;
                for (var i = 1; i <= 5;)
                    for (var j = 1; j <= 5; ++j)
                    {
                        opt[i % 2, 0] = 0;
                        if (i % 2 == 1)
                        {
                            if (handOfComp[i - 1].Equals(hand[j - 1])) opt[1, j] = opt[0, j - 1];
                            else opt[1, j] = 1 + opt[0, j - 1];
                        }
                        else
                        {
                            if (handOfComp[i - 1].Equals(hand[j - 1])) opt[0, j] = opt[1, j - 1];
                            else opt[0, j] = 1 + opt[1, j - 1];
                        }
                        i++;
                    }
                if ((int)opt[1, 5] < maxLevDist)
                {
                    result = Tuple.Create(hand, (int)opt[1, 5]);
                    maxLevDist = (int)opt[1, 5];
                }
            }
            return result;
        }
    }
}
