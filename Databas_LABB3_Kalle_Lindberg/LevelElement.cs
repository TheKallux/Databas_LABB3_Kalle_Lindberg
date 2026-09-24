using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABB2;

public abstract class LevelElement
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public ConsoleColor Color { get; set; }

    public virtual void Draw(bool isVisible, bool isDiscovered)
    {
        if (!isDiscovered) return;
        Console.SetCursorPosition(X, Y);
        Console.ForegroundColor = Color;
        Console.Write(Symbol);
        Console.ResetColor();
    }
}