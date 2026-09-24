using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LABB2;

public class Snake : Enemy
{
    public Snake() : base("Snake", 's', ConsoleColor.Green, 25, new Dice(3, 4, 2), new Dice(1, 8, 0))
    {
    }

    public override void Update(Player player, List<LevelElement> elements)
    {
        double distance = Math.Sqrt(Math.Pow(player.X - X, 2) + Math.Pow(player.Y - Y, 2));
        if (distance <= 2)
        {
            int dx = 0;
            int dy = 0;
            if (player.X < X)
                dx = 1;
            else if (player.X > X)
                dx = -1;
            if (player.Y < Y)
                dy = 1;
            else if (player.Y > Y)
                dy = -1;

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
}