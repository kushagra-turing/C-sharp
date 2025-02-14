namespace UnitTestProject2;
using Project2;
using NUnit.Framework;

[TestFixture]
public class CoinsChangeTests
{
    [Test]
    public void TestExample1()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(3, coinsChange.MinCoins(40));
    }

    [Test]
    public void TestExample2()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(3, coinsChange.MinCoins(32));
    }

    [Test]
    public void TestExample3()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(4, coinsChange.MinCoins(52));
    }

    [Test]
    public void TestZeroAmount()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(0, coinsChange.MinCoins(0));
    }

    [Test]
    public void TestSmallAmount()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(1, coinsChange.MinCoins(1));
        Assert.AreEqual(1, coinsChange.MinCoins(5));
        Assert.AreEqual(1, coinsChange.MinCoins(10));
        Assert.AreEqual(1, coinsChange.MinCoins(12));
        Assert.AreEqual(1, coinsChange.MinCoins(25));
    }

    [Test]
    public void TestAmount45()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(3, coinsChange.MinCoins(45));
    }

    [Test]
    public void TestLargerAmount()
    {
         CoinsChange coinsChange = new CoinsChange();
         Assert.AreEqual(5, coinsChange.MinCoins(99));
    }

    [Test]
    public void TestNegativeAmount()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(-1, coinsChange.MinCoins(-5));
    }
    
    [Test]
    public void TestVeryLargeAmount()
    {
        CoinsChange coinsChange = new CoinsChange();
        Assert.AreEqual(402, coinsChange.MinCoins(10015));
    }
}