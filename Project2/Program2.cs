namespace Project2;
using System;
using System.Collections.Generic;

public class CoinChange
{
    private readonly int[] coins = { 1, 5, 10, 12, 25 };
    private Dictionary<int, int> memo = new Dictionary<int, int>();

    public int MinCoins(int amount)
    {
        if (amount == 0)
        {
            return 0;
        }

        if (memo.ContainsKey(amount))
        {
            return memo[amount];
        }

        int minCoins = int.MaxValue;

        foreach (int coin in coins)
        {
            if (amount >= coin)
            {
                int numCoins = MinCoins(amount - coin);
                if (numCoins != int.MaxValue) // Avoid overflow
                {
                    minCoins = Math.Min(minCoins, numCoins + 1);
                }
            }
        }

        memo[amount] = minCoins;
        return minCoins == int.MaxValue ? int.MaxValue : minCoins;
    }

    public static void Main(String[] args){
        CoinChange coinChange = new CoinChange();
        Console.WriteLine("Step 2, Input: 45, Expected: 3, Output: " + coinChange.MinCoins(45));
    }
}