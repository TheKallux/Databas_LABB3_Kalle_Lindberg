using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABB2;

public class Wall : LevelElement
{
    public Wall()
    {
        Symbol = '#';
        Color = ConsoleColor.Gray;
    }

    public override void Draw(bool isVisible, bool isDiscovered)
    {
        if (!isDiscovered) return;
        Console.SetCursorPosition(X, Y);
        if (isVisible)
        {
            Console.ForegroundColor = Color;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write(Symbol);
        Console.ResetColor();
    }
}