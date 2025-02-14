namespace UnitTestProject2;
using Project2;
using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class CoinChangeTests
{
    [Test]
    public void TestExample1()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(3, coinChange.MinCoins(40));
    }

    [Test]
    public void TestExample2()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(3, coinChange.MinCoins(32));
    }

    [Test]
    public void TestExample3()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(4, coinChange.MinCoins(52));
    }

    [Test]
    public void TestZeroAmount()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(0, coinChange.MinCoins(0));
    }

    [Test]
    public void TestSmallAmount()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(1, coinChange.MinCoins(1));
        Assert.AreEqual(1, coinChange.MinCoins(5));
        Assert.AreEqual(1, coinChange.MinCoins(10));
        Assert.AreEqual(1, coinChange.MinCoins(12));
        Assert.AreEqual(1, coinChange.MinCoins(25));
    }

    [Test]
    public void TestAmount24()
    {
        CoinChange coinChange = new CoinChange();
        Assert.AreEqual(2, coinChange.MinCoins(24));
    }

    [Test]
    public void TestLargerAmount()
    {
         CoinChange coinChange = new CoinChange();
         Assert.AreEqual(6, coinChange.MinCoins(99));
    }

    /*[Test]
    public void TestImpossibleAmount()
    {
        CoinChange coinChange = new CoinChange();
        coinChange.memo.Clear();
        Assert.AreEqual(int.MaxValue, coinChange.MinCoins(13)); //Example where amount can't be reached using the given coins (if 1 coin is not available).
    }*/
}