using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LABB2;

public abstract class Enemy : LevelElement
{
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            if (value < 0)
                health = 0;
            else
                health = value;
        }
    }
    public string Name { get; protected set; }
    public Dice AttackDice { get; protected set; }
    public Dice DefenceDice { get; protected set; }

    public abstract void Update(Player player, List<LevelElement> elements);

    public override void Draw(bool isVisible, bool isDiscovered)
    {

        if (!isVisible) return;
        Console.SetCursorPosition(X, Y);
        Console.ForegroundColor = Color;
        Console.Write(Symbol);
        Console.ResetColor();
    }


}