namespace Project2;
using System;

public class CoinsExchange
{
    /// <summary>
    /// Calculates the minimum number of coins needed to reach the given amount.
    /// </summary>
    /// <param name="amount">The target amount.</param>
    /// <returns>The minimum number of coins required.</returns>
    public int MinCoins(int amount)
    {
        if (amount < 0)
        {
            return -1; // Indicate invalid input (negative amount)
        }

        if (amount == 0)
        {
            return 0; // No coins needed for a zero amount.
        }

        int[] coins = { 1, 5, 10, 12, 25 };
        int[] dp = new int[amount + 1];

        // Initialize DP array.  Each index represents the minimum coins needed to reach that amount.
        // Initialize all values to 'amount + 1', which acts as infinity, to indicate that the amount cannot be reached yet.

        for (int i = 1; i <= amount; i++)
        {
            dp[i] = amount + 1;  // Initialize with a value greater than any possible solution
        }

        dp[0] = 0; // Base case: 0 coins needed to reach an amount of 0

        // Iterate through all possible amounts from 1 to the target amount
        for (int i = 1; i <= amount; i++)
        {
            // Iterate through all available coin denominations
            foreach (int coin in coins)
            {
                // If the current coin denomination is less than or equal to the current amount
                if (coin <= i)
                {
                    // Update the minimum number of coins needed for the current amount
                    // It's the minimum between the current value and (1 + the number of coins needed for the remaining amount after using the current coin)
                    dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
                }
            }
        }

        // If the minimum number of coins needed is still greater than the amount, it means the amount cannot be reached with the given coins
        return dp[amount] > amount ? -1 : dp[amount];
    }

    public static void Main(String[] args)
    {
        CoinsExchange coinsExchange = new CoinsExchange();
        Console.WriteLine("Step 3, Input: 45, Expected: 3, Output: " + coinsExchange.MinCoins(45));
    }
}