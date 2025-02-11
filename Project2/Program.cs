namespace Project2;

using System;

public class CoinExchange
{
    public int MinCoins(int amount)
    {
        if (amount <= 0)
        {
            return 0;
        }

        int[] coins = { 25, 12, 10, 5, 1 };
        int numCoins = 0;

        for (int i = 0; i < coins.Length; i++)
        {
            while (amount >= coins[i])
            {
                amount -= coins[i];
                numCoins++;
            }
        }

        return numCoins;
    }

    public static void Main(String[] args){
        CoinExchange cex = new CoinExchange();
        Console.WriteLine("Amount: 45, Expected: 3, Output: " + cex.MinCoins(45));
    }
}
