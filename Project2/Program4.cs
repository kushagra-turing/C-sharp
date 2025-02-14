namespace Project2;

using System;

public class CoinsChange
{
    private readonly int[] coins = { 1, 5, 10, 12, 25 };
    private Dictionary<int, int> memo = new Dictionary<int, int>();

    public int MinCoins(int amount)
    {
        if (amount == 0)
        {
            return 0;
        }
        else if (amount < 0)
        {
            return -1;
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

    public static void Main(String[] args)
    {
        CoinsChange coinsChange = new CoinsChange();
        Console.WriteLine("Step 4, Input: 45, Expected: 3, Output: " + coinsChange.MinCoins(45));
    }
}