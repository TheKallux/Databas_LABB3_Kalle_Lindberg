using System;
using System.Collections.Generic;
namespace LABB2;

public class GameLoop
{
    private readonly GameRepository repo;
    private LevelData level;
    private Combat combat;
    private SaveGameDocument save = new();

    public GameLoop(GameRepository repo)
    {
        this.repo = repo;
        level = new LevelData();
        combat = new Combat();
    }

    public async Task StartAsync()
    {
        try
        {
            Console.SetWindowSize(140, 30);
        }
        catch
        {
        }

        if (!await SetupGameAsync()) return;

        Console.CursorVisible = false;
        Console.Clear();
        level.UpdateDiscovered();
        foreach (var element in level.Elements)
        {
            bool isVisible = level.IsVisible(element.X, element.Y);
            bool isDiscovered = level.IsDiscovered(element.X, element.Y);
            element.Draw(isVisible, isDiscovered);
        }
        DrawPlayerHealth();
        await RunAsync();
    }

    private async Task<bool> SetupGameAsync()
    {
        while (true)
        {
            var saves = await repo.GetLivingSavesAsync();

            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("=== DUNGEON CRAWLER ===");
            Console.WriteLine();
            Console.WriteLine("N - New character");
            if (saves.Count > 0)
                Console.WriteLine("C - Continue with a saved character");
            Console.WriteLine("Q - Quit");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.N)
            {
                if (await CreateNewCharacterAsync(saves)) return true;
            }
            else if (key == ConsoleKey.C && saves.Count > 0)
            {
                if (await ContinueCharacterAsync(saves)) return true;
            }
            else if (key == ConsoleKey.Q)
            {
                return false;
            }
        }
    }

    private async Task<bool> CreateNewCharacterAsync(List<SaveGameDocument> saves)
    {
        Console.Clear();
        Console.CursorVisible = true;
        Console.Write("Enter character name (empty to go back): ");
        string name = (Console.ReadLine() ?? "").Trim();
        if (name == "") return false;

        if (saves.Any(s => s.PlayerName.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("A living character with that name already exists.");
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey(true);
            return false;
        }

        var classes = await repo.GetClassesAsync();
        if (classes.Count == 0)
        {
            Console.WriteLine("No character classes found in the database.");
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey(true);
            return false;
        }

        Console.WriteLine();
        for (int i = 0; i < classes.Count; i++)
            Console.WriteLine($"{i + 1}. {classes[i].Name} (HP {classes[i].StartHp})");
        Console.WriteLine();

        CharacterClassDocument? chosen = null;
        while (chosen == null)
        {
            Console.Write("Choose class: ");
            if (int.TryParse(Console.ReadLine(), out int number) && number >= 1 && number <= classes.Count)
                chosen = classes[number - 1];
        }

        level.Load("Level1.txt");
        level.Player.Name = name;
        level.Player.Health = chosen.StartHp;
        level.Player.Symbol = chosen.Symbol;

        save = new SaveGameDocument
        {
            PlayerName = name,
            ClassName = chosen.Name
        };
        return true;
    }

    private async Task<bool> ContinueCharacterAsync(List<SaveGameDocument> saves)
    {
        Console.Clear();
        Console.CursorVisible = true;
        Console.WriteLine("Saved characters:");
        Console.WriteLine();
        for (int i = 0; i < saves.Count; i++)
        {
            var s = saves[i];
            Console.WriteLine($"{i + 1}. {s.PlayerName} - {s.ClassName}, HP {s.Player.Health}, turns {s.Turns}");
        }
        Console.WriteLine("0. Back");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Choose character: ");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                if (number == 0) return false;
                if (number >= 1 && number <= saves.Count)
                {
                    save = saves[number - 1];
                    var characterClass = await repo.GetClassByNameAsync(save.ClassName);
                    level.LoadFromSave(save, characterClass);
                    return true;
                }
            }
        }
    }

    private async Task RunAsync()
    {
        while (true)
        {
            Dictionary<LevelElement, (int X, int Y)> oldPositions = new Dictionary<LevelElement, (int, int)>();
            foreach (var element in level.Elements)
            {
                oldPositions[element] = (element.X, element.Y);
            }

            bool keepPlaying = level.Player.UpdatePlayer(level.Elements);
            if (!keepPlaying)
            {
                await SaveAsync(false);
                Console.Clear();
                Console.CursorVisible = true;
                Console.WriteLine("Game saved. See you next time!");
                return;
            }

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
            if (level.Player.Health <= 0)
            {
                await HandleDeathAsync();
                return;
            }

            CheckEnemyInitiatedCombat();
            if (level.Player.Health <= 0)
            {
                await HandleDeathAsync();
                return;
            }

            DrawPlayerHealth();
        }
    }

    private async Task SaveAsync(bool isDead)
    {
        level.FillSave(save);
        save.IsDead = isDead;
        await repo.SaveGameAsync(save);
    }

    private async Task HandleDeathAsync()
    {
        await SaveAsync(true);
        Console.Clear();
        Console.CursorVisible = true;
        Console.WriteLine("=== GAME OVER ===");
        Console.WriteLine();
        Console.WriteLine($"{level.Player.Name} has died after {level.Player.Turns} turns.");
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
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