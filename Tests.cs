using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = System.Tuple<DroPoker.Suit, DroPoker.Dignity>;

namespace DroPoker
{
    [TestFixture]
    class Tests
    {
        Table t = new Table();
        [Test]
        public void HighCard()
        {
            Assert.AreEqual(Combination.HighCard, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Four),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Six),
                                                                                     Tuple.Create(Suit.Club, Dignity.Eight),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Ten),}));
        }

        [Test]
        public void Pair()
        {
            Assert.AreEqual(Combination.Pair, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Two),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Six),
                                                                                     Tuple.Create(Suit.Club, Dignity.Eight),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Ten),}));
        }

        [Test]
        public void TwoPairs()
        {
            Assert.AreEqual(Combination.TwoPairs, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Two),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Six),
                                                                                     Tuple.Create(Suit.Club, Dignity.Six),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Ten),}));
        }

        [Test]
        public void Set()
        {
            Assert.AreEqual(Combination.Set, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Two),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Two),
                                                                                     Tuple.Create(Suit.Club, Dignity.Eight),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Ten),}));
        }

        [Test]
        public void Straight()
        {
            Assert.AreEqual(Combination.Straight, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Four),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Three),
                                                                                     Tuple.Create(Suit.Club, Dignity.Six),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Five),}));
        }

        [Test]
        public void Flush()
        {
            Assert.AreEqual(Combination.Flush, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Club, Dignity.Four),
                                                                                     Tuple.Create(Suit.Club, Dignity.Three),
                                                                                     Tuple.Create(Suit.Club, Dignity.Eight),
                                                                                     Tuple.Create(Suit.Club, Dignity.Ten),}));
        }

        [Test]
        public void FullHause()
        {
            Assert.AreEqual(Combination.FullHause, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Jack),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Two),
                                                                                     Tuple.Create(Suit.Club, Dignity.Jack),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Jack),}));
        }

        [Test]
        public void Square()
        {
            Assert.AreEqual(Combination.Square, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Ace),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Ace),
                                                                                     Tuple.Create(Suit.Heart, Dignity.Ace),
                                                                                     Tuple.Create(Suit.Club, Dignity.Eight),
                                                                                     Tuple.Create(Suit.Spade, Dignity.Ace),}));
        }

        [Test]
        public void StraightFlush()
        {
            Assert.AreEqual(Combination.StraightFlush, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                                                                                     Tuple.Create(Suit.Club, Dignity.Six),
                                                                                     Tuple.Create(Suit.Club, Dignity.Three),
                                                                                     Tuple.Create(Suit.Club, Dignity.Five),
                                                                                     Tuple.Create(Suit.Club, Dignity.Four),}));
        }

        [Test]
        public void RoyalFlush()
        {
            Assert.AreEqual(Combination.RoyalFlush, t.AnanlizeCombination(new Card[] { Tuple.Create(Suit.Diamond, Dignity.Jack),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Ten),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Ace),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.King),
                                                                                     Tuple.Create(Suit.Diamond, Dignity.Queen),}));
        }

        [Test]
        public void LevensteinTest()
        {
            var computerCard = new Card[] { Tuple.Create(Suit.Club, Dignity.Two),
                          Tuple.Create(Suit.Club, Dignity.Jack),
                          Tuple.Create(Suit.Club, Dignity.Queen),
                          Tuple.Create(Suit.Club, Dignity.King),
                          Tuple.Create(Suit.Club, Dignity.Ace)};
            var comp = new Computer();
            Assert.AreEqual(1, comp.LevensteinDistance(computerCard, CombinationArrayClass.RoyalFlush).Item2);
            Assert.AreEqual(CombinationArrayClass.RoyalFlush[0], comp.LevensteinDistance(computerCard, CombinationArrayClass.RoyalFlush).Item1);
        }
    }
}
