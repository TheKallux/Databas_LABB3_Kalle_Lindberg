using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LABB2;

public class Rat : Enemy
{
    private static Random random = new Random();

    public Rat()
    {
        Symbol = 'r';
        Color = ConsoleColor.Red;
        Health = 10;
        Name = "Rat";
        AttackDice = new Dice(1, 6, 1);  
        DefenceDice = new Dice(1, 6, 0); 
    }

    public override void Update(Player player, List<LevelElement> elements)
    {
        int[] directions = { -1, 0, 1 };
        int dx = directions[random.Next(3)];
        int dy = directions[random.Next(3)];

        while (dx == 0 && dy == 0)
        {
            dx = directions[random.Next(3)];
            dy = directions[random.Next(3)];
        }

        int newX = X + dx;
        int newY = Y + dy;

        bool collision = false;
        foreach (var element in elements)
        {
            if (element.X == newX && element.Y == newY)
            {
                if (element is Wall || element is Enemy || element is Player)
                {
                    collision = true;
                    break;
                }
            }
        }

        if (!collision)
        {
            X = newX;
            Y = newY;
        }
    }
}