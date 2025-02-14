namespace UnitTestProject2;
using Project2;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class CoinsExchangeTests
{
    [Test]
    public void TestZeroAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(0, exchange.MinCoins(0));
    }

    [Test]
    public void TestSmallAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(1, exchange.MinCoins(1));
        Assert.AreEqual(1, exchange.MinCoins(5));
        Assert.AreEqual(1, exchange.MinCoins(10));
        Assert.AreEqual(1, exchange.MinCoins(12));
        Assert.AreEqual(1, exchange.MinCoins(25));
        Assert.AreEqual(2, exchange.MinCoins(2));
    }

    [Test]
    public void TestCombinationAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(2, exchange.MinCoins(6)); // 5 + 1
        Assert.AreEqual(2, exchange.MinCoins(11)); // 10 + 1
        Assert.AreEqual(2, exchange.MinCoins(15)); // 10 + 5
        Assert.AreEqual(3, exchange.MinCoins(16)); // 10 + 5 + 1
        Assert.AreEqual(3, exchange.MinCoins(27)); //25 + 1 + 1
    }

    [Test]
    public void TestNonCanonicalAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(2, exchange.MinCoins(24)); // 12 + 12.  Greedy would do 1 coin of 25, then 1 + 1 + 1
    }

    [Test]
    public void TestLargeAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(41, exchange.MinCoins(1000));
    }

    [Test]
    public void TestUnreachableAmount()
    {
       CoinsExchange exchange = new CoinsExchange();
       //If no 1 coin exists.
       //Assert.AreEqual(-1, exchange.MinCoins(3));
    }

    [Test]
    public void TestNegativeAmount()
    {
        CoinsExchange exchange = new CoinsExchange();
        Assert.AreEqual(-1, exchange.MinCoins(-5));
    }
}