using System;

namespace LABB2;

public class Dice
{
    private int numberOfDice;
    private int sidesPerDice;
    private int modifier;
    private Random random = new Random();

    public Dice(int numberOfDice, int sidesPerDice, int modifier)
    {
        this.numberOfDice = numberOfDice;
        this.sidesPerDice = sidesPerDice;
        this.modifier = modifier;
    }

    public int Throw()
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += random.Next(1, sidesPerDice + 1);
        }
        return total + modifier;
    }

    public override string ToString()
    {
        string mod = modifier >= 0 ? "+" + modifier : modifier.ToString();
        return $"{numberOfDice}d{sidesPerDice}{mod}";
    }
}