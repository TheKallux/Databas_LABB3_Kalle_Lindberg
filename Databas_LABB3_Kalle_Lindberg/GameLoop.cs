using System;
using System.Collections.Generic;
namespace LABB2;
public class GameLoop
{
    private LevelData level;
    private Combat combat;
    public GameLoop()
    {
        level = new LevelData();
        combat = new Combat();
    }
    public void Start()
    {
        Console.CursorVisible = false;
        try
        {
            Console.SetWindowSize(140, 30);
        }
        catch
        {
        }
        level.Load("Level1.txt");
        Console.Clear();
        level.UpdateDiscovered();
        foreach (var element in level.Elements)
        {
            bool isVisible = level.IsVisible(element.X, element.Y);
            bool isDiscovered = level.IsDiscovered(element.X, element.Y);
            element.Draw(isVisible, isDiscovered);
        }
        DrawPlayerHealth();
        Run();
    }
    private void Run()
    {
        while (true)
        {
            Dictionary<LevelElement, (int X, int Y)> oldPositions = new Dictionary<LevelElement, (int, int)>();
            foreach (var element in level.Elements)
            {
                oldPositions[element] = (element.X, element.Y);
            }
            level.Player.UpdatePlayer(level.Elements);
            foreach (var element in level.Elements)
            {
                if (element is Enemy enemy)
                {
                    enemy.Update(level.Player, level.Elements);
                }
            }
            level.UpdateDiscovered();

            foreach (var kvp in oldPositions)
            {
                var element = kvp.Key;
                var oldPos = kvp.Value;

                if (oldPos.X != element.X || oldPos.Y != element.Y)
                {
                    Console.SetCursorPosition(oldPos.X, oldPos.Y);
                    Console.Write(' ');
                }
                else if (element is Enemy && !level.IsVisible(element.X, element.Y))
                {
                    Console.SetCursorPosition(element.X, element.Y);
                    Console.Write(' ');
                }
            }

            foreach (var element in level.Elements)
            {
                bool isVisible = level.IsVisible(element.X, element.Y);
                bool isDiscovered = level.IsDiscovered(element.X, element.Y);
                element.Draw(isVisible, isDiscovered);
            }

            CheckPlayerInitiatedCombat();
            CheckEnemyInitiatedCombat();
            DrawPlayerHealth();
        }
    }

    private bool CheckPlayerInitiatedCombat()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (var element in level.Elements)
        {
            if (element is Enemy enemy)
            {
                bool isAdjacent = (enemy.X == level.Player.X && Math.Abs(enemy.Y - level.Player.Y) == 1) ||
                                 (enemy.Y == level.Player.Y && Math.Abs(enemy.X - level.Player.X) == 1);
                if (isAdjacent)
                {
                    bool enemyDied = combat.Fight(level.Player, enemy, true);
                    if (enemyDied)
                    {
                        enemiesToRemove.Add(enemy);
                        Console.SetCursorPosition(enemy.X, enemy.Y);
                        Console.Write(' ');
                    }
                    break;
                }
            }
        }

        foreach (var enemy in enemiesToRemove)
        {
            level.Elements.Remove(enemy);
        }


        return enemiesToRemove.Count > 0;
    }

    private void CheckEnemyInitiatedCombat()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (var element in level.Elements)
        {
            if (element is Enemy enemy)
            {
                bool isAdjacent = (enemy.X == level.Player.X && Math.Abs(enemy.Y - level.Player.Y) == 1) ||
                                 (enemy.Y == level.Player.Y && Math.Abs(enemy.X - level.Player.X) == 1);
                if (isAdjacent)
                {
                    bool enemyDied = combat.Fight(level.Player, enemy, false);
                    if (enemyDied)
                    {
                        enemiesToRemove.Add(enemy);
                        Console.SetCursorPosition(enemy.X, enemy.Y);
                        Console.Write(' ');
                    }
                    break;
                }
            }
        }

        foreach (var enemy in enemiesToRemove)
        {
            level.Elements.Remove(enemy);
        }

    }

    private void DrawPlayerHealth()
    {
        Console.SetCursorPosition(0, 22);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"Player Health: {level.Player.Health}    ");
        Console.ResetColor();
    }
}