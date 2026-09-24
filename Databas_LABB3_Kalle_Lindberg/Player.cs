using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABB2;

public class Player : LevelElement
{
    public int Health { get; set; }
    public string Name { get; set; }
    public int Turns { get; set; }
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

    // Returns false if player presses Escape (save & quit)
    public bool UpdatePlayer(List<LevelElement> elements)
    {
        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
            return false;

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
            if (X != newX || Y != newY)
                Turns++;

            X = newX;
            Y = newY;
        }

        return true;
    }
}