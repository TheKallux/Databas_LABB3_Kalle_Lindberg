using System;
using System.Collections.Generic;

namespace LABB2;

public class Combat
{
    private const int COMBAT_TEXT_X = 82;
    private int currentLine = 0;

    public bool Fight(Player player, Enemy enemy, bool playerInitiated)
    {
        currentLine = 0;
        ClearCombatArea();

        WriteCombatLine("=== COMBAT ===");
        WriteCombatLine($"{player.Name} (HP: {player.Health}) vs");
        WriteCombatLine($"{enemy.Name} (HP: {enemy.Health})");
        WriteCombatLine("");

        bool enemyDied = false;

        if (playerInitiated)
        {
            enemyDied = PlayerAttacksFirst(player, enemy);
        }
        else
        {
            enemyDied = EnemyAttacksFirst(player, enemy);
        }

        WriteCombatLine("");
        WriteCombatLine("Press any key to continue...");
        Console.ReadKey(true);

        ClearCombatArea();

        return enemyDied;
    }

    private bool PlayerAttacksFirst(Player player, Enemy enemy)
    {
        WriteCombatLine("--- Player's Attack ---");
        int playerAttack = player.AttackDice.Throw();
        int enemyDefence = enemy.DefenceDice.Throw();

        WriteCombatLine($"{player.Name} attacks: {playerAttack}");
        WriteCombatLine($"{enemy.Name} defends: {enemyDefence}");


        if (playerAttack > enemyDefence)
        {
            int damage = playerAttack - enemyDefence;
            enemy.Health -= damage;
            WriteCombatLine($"HIT! {damage} damage!");
            WriteCombatLine($"{enemy.Name} HP: {enemy.Health}");
        }
        else
        {
            WriteCombatLine("MISS! Blocked!");
        }

        if (enemy.Health <= 0)
        {
            WriteCombatLine("");
            WriteCombatLine($"{enemy.Name} defeated!");
            return true;
        }

        WriteCombatLine("");
        WriteCombatLine("--- Enemy Counterattack ---");
        int enemyAttack = enemy.AttackDice.Throw();
        int playerDefence = player.DefenceDice.Throw();

        WriteCombatLine($"{enemy.Name} attacks: {enemyAttack}");
        WriteCombatLine($"{player.Name} defends: {playerDefence}");

        if (enemyAttack > playerDefence)
        {
            int damage = enemyAttack - playerDefence;
            player.Health -= damage;
            WriteCombatLine($"HIT! {damage} damage!");
            WriteCombatLine($"{player.Name} HP: {player.Health}");
        }
        else
        {
            WriteCombatLine("MISS! You blocked!");
        }

        if (player.Health <= 0)
        {
            WriteCombatLine("");
            WriteCombatLine("You died!");
        }

        return false;
    }

    private bool EnemyAttacksFirst(Player player, Enemy enemy)
    {
        WriteCombatLine("--- Enemy's Attack ---");
        int enemyAttack = enemy.AttackDice.Throw();
        int playerDefence = player.DefenceDice.Throw();

        WriteCombatLine($"{enemy.Name} attacks: {enemyAttack}");
        WriteCombatLine($"{player.Name} defends: {playerDefence}");

        if (enemyAttack > playerDefence)
        {
            int damage = enemyAttack - playerDefence;
            player.Health -= damage;
            WriteCombatLine($"HIT! {damage} damage!");
            WriteCombatLine($"{player.Name} HP: {player.Health}");
        }
        else
        {
            WriteCombatLine("MISS! You blocked!");
        }

        if (player.Health <= 0)
        {
            WriteCombatLine("");
            WriteCombatLine("You died!");
            return false;
        }

        WriteCombatLine("");
        WriteCombatLine("--- Player Counterattack ---");
        int playerAttack = player.AttackDice.Throw();
        int enemyDefence = enemy.DefenceDice.Throw();

        WriteCombatLine($"{player.Name} attacks: {playerAttack}");
        WriteCombatLine($"{enemy.Name} defends: {enemyDefence}");

        if (playerAttack > enemyDefence)
        {
            int damage = playerAttack - enemyDefence;
            enemy.Health -= damage;
            WriteCombatLine($"HIT! {damage} damage!");
            WriteCombatLine($"{enemy.Name} HP: {enemy.Health}");
        }
        else
        {
            WriteCombatLine("MISS! Blocked!");
        }

        if (enemy.Health <= 0)
        {
            WriteCombatLine("");
            WriteCombatLine($"{enemy.Name} defeated!");
            return true;
        }

        return false;
    }

    private void WriteCombatLine(string text)
    {
        try
        {
            Console.SetCursorPosition(COMBAT_TEXT_X, currentLine);
            Console.Write(text);
            currentLine++;
        }
        catch
        {
            currentLine++;
        }
    }

    private void ClearCombatArea()
    {
        for (int line = 0; line < 25; line++)
        {
            for (int col = 0; col < 40; col++)
            {
                try
                {
                    Console.SetCursorPosition(COMBAT_TEXT_X + col, line);
                    Console.Write(' ');
                }
                catch { }
            }
        }
    }
}