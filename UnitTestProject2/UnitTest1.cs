namespace UnitTestProject2;
using Project2;
using NUnit.Framework;

[TestFixture]
public class CoinExchangeTests
{
    [Test]
    public void TestZeroAmount()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(0, exchange.MinCoins(0));
    }

    [Test]
    public void TestSmallAmount()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(1, exchange.MinCoins(1));
    }

    [Test]
    public void TestAmountEqualToCoinDenomination()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(1, exchange.MinCoins(5));
        Assert.AreEqual(1, exchange.MinCoins(10));
        Assert.AreEqual(1, exchange.MinCoins(12));
        Assert.AreEqual(1, exchange.MinCoins(25));
    }

    [Test]
    public void TestSimpleAmount()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(2, exchange.MinCoins(2));
        Assert.AreEqual(2, exchange.MinCoins(6));
        Assert.AreEqual(2, exchange.MinCoins(11));
    }

    [Test]
    public void TestCombinationAmount()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(2, exchange.MinCoins(24)); //12 + 12
        Assert.AreEqual(2, exchange.MinCoins(26)); // 25 + 1
        Assert.AreEqual(3, exchange.MinCoins(27)); // 25 + 1 + 1
    }

    [Test]
    public void TestLargerAmount()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(6, exchange.MinCoins(64)); // 25 + 25 + 12 + 1 + 1
        Assert.AreEqual(4, exchange.MinCoins(49)); // 25 + 12 + 10 + 1 + 1
    }

    [Test]
    public void TestExampleProvided()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(4, exchange.MinCoins(42)); //25 + 12 + 5
    }

     [Test]
    public void TestAnotherExample()
    {
        CoinExchange exchange = new CoinExchange();
        Assert.AreEqual(3, exchange.MinCoins(37)); //25 + 12
    }
}