using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABB2;

public class Player : LevelElement
{
    public int Health { get; set; }
    public string Name { get; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public Player()
    {
        Symbol = '@';
        Color = ConsoleColor.White;
        Health = 100;
        Name = "Player";
        AttackDice = new Dice(2, 6, 2);  
        DefenceDice = new Dice(2, 6, 0);  
    }

    public override void Draw(bool isVisible, bool isDiscovered)
    {
        Console.SetCursorPosition(X, Y);
        Console.ForegroundColor = Color;
        Console.Write(Symbol);
        Console.ResetColor();
    }

    public void UpdatePlayer(List<LevelElement> elements)
    {
        ConsoleKeyInfo key = Console.ReadKey(true);

        int newX = X;
        int newY = Y;
        if (key.Key == ConsoleKey.UpArrow) newY--;
        else if (key.Key == ConsoleKey.DownArrow) newY++;
        else if (key.Key == ConsoleKey.LeftArrow) newX--;
        else if (key.Key == ConsoleKey.RightArrow) newX++;

        bool collision = false;
        foreach (var element in elements)
        {
            if ((element is Wall || element is Enemy) && element.X == newX && element.Y == newY)
            {
                collision = true;
                break;
            }
        }

        if (!collision)
        {
            X = newX;
            Y = newY;
        }
    }
}