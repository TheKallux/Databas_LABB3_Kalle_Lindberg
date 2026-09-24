using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LABB2;

public class LevelData
{
    private List<LevelElement> elements;
    private bool[,] discovered;
    private int width;
    private int height;

    public List<LevelElement> Elements
    {
        get { return elements; }
    }

    public LevelData()
    {
        elements = new List<LevelElement>();
    }

    public Player Player { get; private set; }

    public void Load(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        this.height = lines.Length;
        this.width = lines[0].Length;

        discovered = new bool[width, height];
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                char symbol = line[x];
                switch (symbol)
                {
                    case '#':
                        Wall wall = new Wall();
                        wall.X = x;
                        wall.Y = y;
                        elements.Add(wall);
                        break;
                    case 'r':
                        Rat rat = new Rat();
                        rat.X = x;
                        rat.Y = y;
                        elements.Add(rat);
                        break;
                    case 's':
                        Snake snake = new Snake();
                        snake.X = x;
                        snake.Y = y;
                        elements.Add(snake);
                        break;
                    case '@':
                        Player player = new Player();
                        player.X = x;
                        player.Y = y;
                        Player = player;
                        elements.Add(player);
                        break;
                    default:
                        break;
                }
            }
        }
        DiscoverStartArea(6);

    }

    private void DiscoverStartArea(int radius)
    {
        if (Player == null) return;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int distance = Math.Abs(x - Player.X) + Math.Abs(y - Player.Y);
                if (distance <= radius)
                {
                    bool hasWall = false;
                    foreach (var element in elements)
                    {
                        if (element is Wall && element.X == x && element.Y == y)
                        {
                            hasWall = true;
                            break;
                        }
                    }

                    if (hasWall)
                    {
                        discovered[x, y] = true;
                    }
                }
            }
        }
    }
    public bool IsVisible(int x, int y)
    {
        if (Player == null) return false;
        int distance = Math.Abs(x - Player.X) + Math.Abs(y - Player.Y);
        return distance <= 5;
    }

    public void UpdateDiscovered()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsVisible(x, y))
                {
                    bool hasWall = false;
                    foreach (var element in Elements)
                    {
                        if (element is Wall && element.X == x && element.Y == y)
                        {
                            hasWall = true;
                            break;
                        }
                    }

                    if (hasWall)
                    {
                        discovered[x, y] = true;
                    }
                }
            }
        }
    }

    public bool IsDiscovered(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;
        return discovered[x, y];
    }
}